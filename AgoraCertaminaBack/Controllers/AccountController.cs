using AgoraCertaminaBack.Authorization.AttributeHandler;
using AgoraCertaminaBack.Authorization.Settings;
using AgoraCertaminaBack.Authorization.Utils;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.UseCases.Tenants;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AgoraCertaminaBack.Authorization.AuthenticationEntities;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IAmazonCognitoIdentityProvider _cognito;
        private readonly ICognitoSettings _cognitoSettings;
        private readonly UserRequestContext _userContext;
        private readonly CreateTenant _createTenant;

        public AccountController(
            IAmazonCognitoIdentityProvider cognito,
            ICognitoSettings cognitoSettings,
            UserRequestContext userContext,
            CreateTenant createTenant)
        {
            _cognito = cognito;
            _cognitoSettings = cognitoSettings;
            _userContext = userContext;
            _createTenant = createTenant;
        }

        #region Tenant
        [AllowAnonymous]
        [HttpPost("tenant")]
        public async Task<IActionResult> TenantRegister(CreateTenantRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { error = "El nombre del tenant es requerido." });

            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { error = "El email es requerido." });

            try
            {
                var tenantResult = await _createTenant.Execute(request.Name);
                if (tenantResult.Errors.Any())
                    return BadRequest(new { error = tenantResult.Errors });

                var tenantId = tenantResult.Value.Id;
                var tenantName = tenantResult.Value.TenantName;

                var userRequest = new UserRequest
                {
                    Name = tenantName,
                    Lastname = "Administrator",
                    Email = request.Email,
                    Roles = new List<string> { "Administrator", "Operator", "Manager" },
                    OrganizationId = tenantId
                };

                return await AddUser(userRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error interno del servidor.",
                    detail = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("tenant/{tenantId}/admin")]
        public async Task<IActionResult> CreateAdminForExistingTenant(
            string tenantId,
            [FromBody] CreateAdminRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { error = "El email es requerido." });

            try
            {
                var userRequest = new UserRequest
                {
                    Name = request.Name ?? "Admin",
                    Lastname = request.LastName ?? "Administrator",
                    Email = request.Email,
                    Roles = new List<string> { "Administrator", "Operator", "Manager" },
                    OrganizationId = tenantId
                };

                return await AddUser(userRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        #endregion

        #region Users
        [HasPermissionOnAction(Constants.Actions.AddUsers)]
        [HttpPost("register")]
        public async Task<IActionResult> AddUser(UserRequest attributes)
        {
            string passwordTemporal = PasswordGenerator.Generate(12);

            try
            {
                var request = new AdminCreateUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = attributes.Email, // ✅ Email como username
                    TemporaryPassword = passwordTemporal,
                    MessageAction = "SUPPRESS",
                    UserAttributes = new List<AttributeType>
                    {
                        new AttributeType { Name = "name", Value = attributes.Name },
                        new AttributeType { Name = "family_name", Value = attributes.Lastname },
                        // ✅ NO incluir email - ya está en Username
                        new AttributeType { Name = "custom:organization_id", Value = attributes.OrganizationId ?? _userContext.OrganizationId }
                    }
                };

                var response = await _cognito.AdminCreateUserAsync(request);

                foreach (var role in attributes.Roles)
                {
                    try
                    {
                        await _cognito.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
                        {
                            UserPoolId = _cognitoSettings.UserPoolId,
                            Username = attributes.Email,
                            GroupName = role
                        });
                    }
                    catch (ResourceNotFoundException)
                    {
                        continue;
                    }
                }

                return Ok(new
                {
                    usuario = attributes.Email,
                    passwordTemporal = passwordTemporal
                });
            }
            catch (Exception ex)
            {
                if (ex is UsernameExistsException ||
                    ex is InvalidParameterException ||
                    ex.Message.Contains("CreateUser"))
                {
                    return BadRequest(new
                    {
                        error = "Errores al crear el usuario.",
                        message = ex.Message
                    });
                }

                if (ex is ResourceNotFoundException ||
                    ex is UserNotFoundException ||
                    ex.Message.Contains("AddUserToGroup"))
                {
                    return BadRequest(new
                    {
                        error = "Usuario se ha creado, pero hubo un problema al asignarlo a uno o más grupos.",
                        message = ex.Message
                    });
                }

                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HasPermissionOnAction(Constants.Actions.ReadUsers)]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] int? limit = 20, [FromQuery] string? paginationToken = null)
        {
            var request = new ListUsersRequest
            {
                UserPoolId = _cognitoSettings.UserPoolId,
                Limit = limit ?? 20
            };

            if (!string.IsNullOrEmpty(paginationToken))
            {
                request.PaginationToken = paginationToken;
            }

            try
            {
                var response = await _cognito.ListUsersAsync(request);

                // ✅ DEBUGGING: Ver qué trae Cognito
                Console.WriteLine($"Total usuarios en Cognito: {response.Users.Count}");
                Console.WriteLine($"OrganizationId del contexto: '{_userContext.OrganizationId}'");

                var users = response.Users.Select(user => new
                {
                    Username = user.Username,
                    Email = user.Attributes?.FirstOrDefault(a => a.Name == "email")?.Value,
                    Name = user.Attributes?.FirstOrDefault(a => a.Name == "name")?.Value,
                    LastName = user.Attributes?.FirstOrDefault(a => a.Name == "family_name")?.Value,
                    OrganizationId = user.Attributes?.FirstOrDefault(a => a.Name == "custom:organization_id")?.Value,
                }).ToList();

                // ✅ DEBUGGING: Ver los OrganizationId de cada usuario
                foreach (var u in users)
                {
                    Console.WriteLine($"Usuario: {u.Username}, OrgId: '{u.OrganizationId}'");
                }

                // ✅ Validar que el contexto tenga OrganizationId
                if (string.IsNullOrEmpty(_userContext.OrganizationId))
                {
                    return BadRequest(new { error = "El usuario autenticado no tiene un OrganizationId válido." });
                }

                // ✅ Filtrar por tenant del usuario loggeado
                var filteredUsers = users
                    .Where(u => !string.IsNullOrEmpty(u.OrganizationId) &&
                               u.OrganizationId.Equals(_userContext.OrganizationId, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Console.WriteLine($"Usuarios filtrados: {filteredUsers.Count}");

                // Si no hay usuarios después del filtro, retornar información útil
                if (!filteredUsers.Any())
                {
                    return Ok(new
                    {
                        users = new List<object>(),
                        paginationToken = response.PaginationToken,
                        totalCount = 0,
                        debug = new
                        {
                            totalInCognito = users.Count,
                            currentOrgId = _userContext.OrganizationId,
                            usersWithOrgId = users.Count(u => !string.IsNullOrEmpty(u.OrganizationId)),
                            uniqueOrgIds = users.Select(u => u.OrganizationId).Distinct().ToList()
                        }
                    });
                }

                // Obtener grupos de usuarios en paralelo
                var usersWithRoles = await Task.WhenAll(filteredUsers.Select(async user =>
                {
                    var groupsResponse = await _cognito.AdminListGroupsForUserAsync(new AdminListGroupsForUserRequest
                    {
                        UserPoolId = _cognitoSettings.UserPoolId,
                        Username = user.Username
                    });

                    var roles = groupsResponse.Groups.Select(g => g.GroupName).ToList();

                    return new
                    {
                        user.Username,
                        user.Email,
                        user.Name,
                        user.LastName,
                        user.OrganizationId,
                        Roles = roles
                    };
                }));

                return Ok(new
                {
                    users = usersWithRoles,
                    paginationToken = response.PaginationToken,
                    totalCount = usersWithRoles.Length
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HasPermissionOnAction(Constants.Actions.UpdateUsers)]
        [HttpPut("user/{username}")]
        public async Task<IActionResult> UpdateUserAsync(string username, [FromBody] UserRequest request)
        {
            try
            {
                // Obtener el usuario
                var getUserRequest = new AdminGetUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username
                };

                var getUserResponse = await _cognito.AdminGetUserAsync(getUserRequest);

                // Validar OrganizationId
                var organizationId = getUserResponse.UserAttributes
                    ?.FirstOrDefault(a => a.Name == "custom:organization_id")?.Value;

                if (string.IsNullOrEmpty(organizationId))
                {
                    return BadRequest(new { error = "El usuario no tiene definido el organization_id." });
                }

                if (organizationId != _userContext.OrganizationId)
                {
                    return BadRequest(new { error = "El usuario no pertenece al tenant." });
                }

                // Actualizar atributos del usuario
                var updateAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "name", Value = request.Name },
                    new AttributeType { Name = "family_name", Value = request.Lastname }
                    // ✅ NO actualizar email - está en el username y no se puede cambiar
                };

                await _cognito.AdminUpdateUserAttributesAsync(new AdminUpdateUserAttributesRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username,
                    UserAttributes = updateAttributes
                });

                // Actualizar grupos
                var currentGroups = await _cognito.AdminListGroupsForUserAsync(new AdminListGroupsForUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username
                });

                var currentGroupNames = currentGroups.Groups.Select(g => g.GroupName).ToHashSet(StringComparer.OrdinalIgnoreCase);
                var newGroupNames = request.Roles.Distinct(StringComparer.OrdinalIgnoreCase).ToHashSet();

                // Eliminar grupos que ya no están
                var groupsToRemove = currentGroupNames.Except(newGroupNames);
                foreach (var groupName in groupsToRemove)
                {
                    await _cognito.AdminRemoveUserFromGroupAsync(new AdminRemoveUserFromGroupRequest
                    {
                        UserPoolId = _cognitoSettings.UserPoolId,
                        Username = username,
                        GroupName = groupName
                    });
                }

                // Agregar nuevos grupos
                var groupsToAdd = newGroupNames.Except(currentGroupNames);
                foreach (var groupName in groupsToAdd)
                {
                    await _cognito.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
                    {
                        UserPoolId = _cognitoSettings.UserPoolId,
                        Username = username,
                        GroupName = groupName
                    });
                }

                return Ok(new { message = "Usuario actualizado correctamente." });
            }
            catch (UserNotFoundException)
            {
                return NotFound(new { error = "Usuario no encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al actualizar el usuario.", detail = ex.Message });
            }
        }

        [HasPermissionOnAction(Constants.Actions.DeleteUsers)]
        [HttpDelete("user/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try
            {
                // Obtener el usuario desde Cognito
                var getUserRequest = new AdminGetUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username
                };

                var getUserResponse = await _cognito.AdminGetUserAsync(getUserRequest);

                // Obtener el atributo OrganizationId
                var organizationId = getUserResponse.UserAttributes
                    ?.FirstOrDefault(attr => attr.Name == "custom:organization_id")?.Value;

                if (string.IsNullOrEmpty(organizationId))
                {
                    return BadRequest(new { error = "El usuario no tiene definido el organization_id." });
                }

                if (organizationId != _userContext.OrganizationId)
                {
                    return BadRequest(new { error = "El usuario no pertenece al tenant." });
                }

                // Eliminar el usuario
                var deleteRequest = new AdminDeleteUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username
                };

                await _cognito.AdminDeleteUserAsync(deleteRequest);

                return NoContent();
            }
            catch (UserNotFoundException)
            {
                return NotFound(new { error = "Usuario no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HasPermissionOnAction(Constants.Actions.ReadGroups)]
        [HttpGet("groups")]
        public async Task<IActionResult> GetAllGroups()
        {
            var request = new ListGroupsRequest
            {
                UserPoolId = _cognitoSettings.UserPoolId,
                Limit = 60
            };

            try
            {
                var response = await _cognito.ListGroupsAsync(request);

                var groups = response.Groups.Select(g => new
                {
                    groupName = g.GroupName,
                    description = g.Description
                }).ToList();

                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error al obtener la lista de grupos.", detail = ex.Message });
            }
        }
        #endregion
    }

    public class CreateAdminRequest
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
    }
}