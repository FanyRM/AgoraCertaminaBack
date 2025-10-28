using ROP;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Participant;
using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.General;
using AgoraCertaminaBack.Models.Mappers;

namespace AgoraCertaminaBack.UseCases.Participants
{
    public class GetAllParticipants(IMongoRepository<Participant> _mongoRepository, UserRequestContext _userRequest)
    {
        public async Task<Result<List<ParticipantDTO>>> Execute()
        {
            var participants = await _mongoRepository.FilterByAsync(
                filter => filter.OrganizationId == _userRequest.OrganizationId && filter.IsActive
            );

            var participantsDTO = participants.Select(e => e.ToParticipantDTO()).ToList();
            return participantsDTO;
        }

        public async Task<Result<PaginatedResult<ParticipantDTO>>> ExecutePaginated(int page = 1, int pageSize = 10, string? search = null)
        {
            try
            {
                // Para validar parámetros
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100;

                List<Participant> allEmployees;

                if (string.IsNullOrWhiteSpace(search))
                {
                    // Filtro solo por tenant y activo
                    allEmployees = (await _mongoRepository.FilterByAsync(
                        e => e.OrganizationId == _userRequest.OrganizationId && e.IsActive
                    )).ToList();
                }
                else
                {
                    // Primero obtener por tenant y activo, luego filtrar por búsqueda en memoria
                    var baseEmployees = await _mongoRepository.FilterByAsync(
                        e => e.OrganizationId == _userRequest.OrganizationId && e.IsActive
                    );

                    var searchTerm = search.ToLower().Trim();

                    allEmployees = baseEmployees.Where(participant =>
                        participant.FirstName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        participant.LastName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        participant.Email.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        participant.PhoneNumber.Contains(searchTerm) || (!string.IsNullOrEmpty(participant.ExternalId) && participant.ExternalId.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                    )
                    .ToList();
                }

                // Calcular paginación
                var totalCount = allEmployees.Count;
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Aplicar paginación
                var paginatedResult = allEmployees
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => e.ToParticipantDTO())
                    .ToList();

                var result = new PaginatedResult<ParticipantDTO>
                {
                    Items = paginatedResult,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages
                };

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<PaginatedResult<ParticipantDTO>>($"Error in obtaining employees: {ex.Message}");
            }
        }
    }
}