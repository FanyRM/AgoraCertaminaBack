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
            // Validaciones iniciales
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { error = "El nombre del tenant es requerido." });

            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new { error = "El email es requerido." });

            try
            {
                // 1. Crear tenant en base de datos
                var tenantResult = await _createTenant.Execute(request.Name);
                if (tenantResult.Errors.Any())
                    return BadRequest(new { error = tenantResult.Errors });

                var tenantId = tenantResult.Value.Id;
                var tenantName = tenantResult.Value.TenantName;

                // 2. Generar password temporal
                string passwordTemporal = PasswordGenerator.Generate(12);

                try
                {
                    // 3. Crear usuario administrador en Cognito
                    var cognitoRequest = new AdminCreateUserRequest
                    {
                        UserPoolId = _cognitoSettings.UserPoolId,
                        Username = request.Email,
                        TemporaryPassword = passwordTemporal,
                        MessageAction = "SUPPRESS",
                        UserAttributes = new List<AttributeType>
                        {
                            new AttributeType { Name = "name", Value = tenantName },
                            new AttributeType { Name = "family_name", Value = "Administrator" },
                            new AttributeType { Name = "email", Value = request.Email },
                            new AttributeType { Name = "custom:tenant_id", Value = tenantId }
                        }
                    };

                    await _cognito.AdminCreateUserAsync(cognitoRequest);

                    // 4. Asignar roles de administrador
                    var adminRoles = new List<string> { "Administrator", "Operator", "Manager" };

                    foreach (var role in adminRoles)
                    {
                        try
                        {
                            await _cognito.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
                            {
                                UserPoolId = _cognitoSettings.UserPoolId,
                                Username = request.Email,
                                GroupName = role
                            });
                        }
                        catch (ResourceNotFoundException)
                        {
                            // El grupo no existe, continuar con el siguiente
                            continue;
                        }
                    }

                    // 5. Retornar éxito con credenciales
                    return Ok(new
                    {
                        message = "Tenant y usuario administrador creados correctamente",
                        tenantId = tenantId,
                        tenantName = tenantName,
                        usuario = request.Email,
                        passwordTemporal = passwordTemporal
                    });
                }
                catch (UsernameExistsException)
                {
                    // Usuario ya existe - idealmente deberías eliminar el tenant aquí
                    // await _deleteTenant.Execute(tenantId);
                    return BadRequest(new
                    {
                        error = "El email ya está registrado en el sistema.",
                        detail = "El tenant fue creado pero el usuario no pudo ser asociado. Contacte al administrador."
                    });
                }
                catch (InvalidParameterException ex)
                {
                    // Parámetro inválido en Cognito
                    return BadRequest(new
                    {
                        error = "Error de validación al crear el usuario.",
                        detail = ex.Message
                    });
                }
                catch (Exception cognitoEx)
                {
                    // Error general de Cognito
                    return BadRequest(new
                    {
                        error = "Error al crear el usuario administrador.",
                        detail = cognitoEx.Message
                    });
                }
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

        // Endpoint auxiliar para crear admin en tenants existentes (opcional, temporal)
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
                string passwordTemporal = PasswordGenerator.Generate(12);

                var cognitoRequest = new AdminCreateUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = request.Email,
                    TemporaryPassword = passwordTemporal,
                    MessageAction = "SUPPRESS",
                    UserAttributes = new List<AttributeType>
                    {
                        new AttributeType { Name = "name", Value = request.Name ?? "Admin" },
                        new AttributeType { Name = "family_name", Value = request.LastName ?? "Administrator" },
                        new AttributeType { Name = "email", Value = request.Email },
                        new AttributeType { Name = "custom:tenant_id", Value = tenantId }
                    }
                };

                await _cognito.AdminCreateUserAsync(cognitoRequest);

                var adminRoles = new List<string> { "Administrator", "Operator", "Manager" };
                foreach (var role in adminRoles)
                {
                    try
                    {
                        await _cognito.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
                        {
                            UserPoolId = _cognitoSettings.UserPoolId,
                            Username = request.Email,
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
                    message = "Usuario administrador creado correctamente",
                    usuario = request.Email,
                    passwordTemporal = passwordTemporal
                });
            }
            catch (UsernameExistsException)
            {
                return BadRequest(new { error = "El email ya está registrado en el sistema." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #endregion

        #region Users

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> AddUser(UserRequest attributes)
        {
            string passwordTemporal = PasswordGenerator.Generate(12);

            try
            {
                var request = new AdminCreateUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = attributes.Email,
                    TemporaryPassword = passwordTemporal,
                    MessageAction = "SUPPRESS",
                    UserAttributes = new List<AttributeType>
                    {
                        new AttributeType { Name = "name", Value = attributes.Name },
                        new AttributeType { Name = "family_name", Value = attributes.Lastname },
                        new AttributeType { Name = "email", Value = attributes.Email },
                        new AttributeType { Name = "custom:tenant_id", Value = attributes.TenantId ?? _userContext.OrganizationId }
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
            catch (UsernameExistsException)
            {
                return BadRequest(new
                {
                    error = "El usuario ya existe.",
                    message = "El email ya está registrado en el sistema."
                });
            }
            catch (InvalidParameterException ex)
            {
                return BadRequest(new
                {
                    error = "Error de validación.",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HasPermissionOnAction(Constants.Actions.ReadUsers)]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] int? limit = 20, [FromQuery] string? paginationToken = null)
        {
            try
            {
                var request = new ListUsersRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Limit = limit ?? 20,
                    PaginationToken = paginationToken
                };

                var response = await _cognito.ListUsersAsync(request);

                var users = response.Users.Select(u => new
                {
                    u.Username,
                    Email = u.Attributes?.FirstOrDefault(a => a.Name == "email")?.Value,
                    Name = u.Attributes?.FirstOrDefault(a => a.Name == "name")?.Value,
                    LastName = u.Attributes?.FirstOrDefault(a => a.Name == "family_name")?.Value,
                    TenantId = u.Attributes?.FirstOrDefault(a => a.Name == "custom:tenant_id")?.Value
                })
                .Where(u => u.TenantId == _userContext.OrganizationId)
                .ToList();

                var usersWithRoles = await Task.WhenAll(users.Select(async user =>
                {
                    var groupsResponse = await _cognito.AdminListGroupsForUserAsync(new AdminListGroupsForUserRequest
                    {
                        UserPoolId = _cognitoSettings.UserPoolId,
                        Username = user.Username
                    });

                    return new
                    {
                        user.Username,
                        user.Email,
                        user.Name,
                        user.LastName,
                        user.TenantId,
                        Roles = groupsResponse.Groups.Select(g => g.GroupName).ToList()
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
                return BadRequest(new { error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPut("user/{username}")]
        public async Task<IActionResult> UpdateUserAsync(string username, [FromBody] UserRequest request)
        {
            try
            {
                var getUserResponse = await _cognito.AdminGetUserAsync(new AdminGetUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username
                });

                var tenantId = getUserResponse.UserAttributes
                    ?.FirstOrDefault(a => a.Name == "custom:tenant_id")?.Value;

                if (tenantId != _userContext.OrganizationId)
                    return BadRequest(new { error = "El usuario no pertenece al tenant." });

                await _cognito.AdminUpdateUserAttributesAsync(new AdminUpdateUserAttributesRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username,
                    UserAttributes = new List<AttributeType>
                    {
                        new AttributeType { Name = "name", Value = request.Name },
                        new AttributeType { Name = "family_name", Value = request.Lastname },
                        new AttributeType { Name = "email", Value = request.Email }
                    }
                });

                var currentGroups = await _cognito.AdminListGroupsForUserAsync(new AdminListGroupsForUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username
                });

                var currentGroupNames = currentGroups.Groups.Select(g => g.GroupName).ToHashSet();
                var newGroupNames = request.Roles.Distinct().ToHashSet();

                var groupsToRemove = currentGroupNames.Except(newGroupNames);
                var groupsToAdd = newGroupNames.Except(currentGroupNames);

                foreach (var groupName in groupsToRemove)
                    await _cognito.AdminRemoveUserFromGroupAsync(new AdminRemoveUserFromGroupRequest
                    {
                        UserPoolId = _cognitoSettings.UserPoolId,
                        Username = username,
                        GroupName = groupName
                    });

                foreach (var groupName in groupsToAdd)
                    await _cognito.AdminAddUserToGroupAsync(new AdminAddUserToGroupRequest
                    {
                        UserPoolId = _cognitoSettings.UserPoolId,
                        Username = username,
                        GroupName = groupName
                    });

                return Ok(new { message = "Usuario actualizado correctamente." });
            }
            catch (UserNotFoundException)
            {
                return NotFound(new { error = "Usuario no encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HasPermissionOnAction(Constants.Actions.DeleteUsers)]
        [HttpDelete("user/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try
            {
                var getUserResponse = await _cognito.AdminGetUserAsync(new AdminGetUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username
                });

                var tenantId = getUserResponse.UserAttributes
                    ?.FirstOrDefault(a => a.Name == "custom:tenant_id")?.Value;

                if (tenantId != _userContext.OrganizationId)
                    return BadRequest(new { error = "El usuario no pertenece al tenant." });

                await _cognito.AdminDeleteUserAsync(new AdminDeleteUserRequest
                {
                    UserPoolId = _cognitoSettings.UserPoolId,
                    Username = username
                });

                return NoContent();
            }
            catch (UserNotFoundException)
            {
                return NotFound(new { error = "Usuario no encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        #endregion
    }

    // DTO adicional para crear admin en tenant existente
    public class CreateAdminRequest
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
    }
}