using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Data.Repository;
using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.Mappers;
using AgoraCertaminaBack.Models.Entities;
using MongoDB.Bson;
using ROP;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using MongoDB.Driver;

namespace AgoraCertaminaBack.UseCases.Contests
{
    public class GetFreeContestById (IMongoRepository<Contest> _mongoRepository)
    {
        public async Task<Result<ContestResponseDTO>> Execute(string contestId)
        {
            var contest = await _mongoRepository.FindOneAsync(a => a.Id == contestId && a.IsActive);

            if (contest == null)
                return Result.NotFound<ContestResponseDTO>("Contest not found");

            var response = MapToResponseDTO(contest);
            return response;
        }

        private ContestResponseDTO MapToResponseDTO(Contest contest) => new ContestResponseDTO
        {
            Id = contest.Id,
            OrganizationId = contest.OrganizationId,
            SchemaId = contest.SchemaId,
            FormId = contest.FormId,
            ReferenceNumber = contest.ReferenceNumber,
            OrganizationName = contest.OrganizationName,
            SchemaName = contest.SchemaName,
            ContestName = contest.ContestName,
            DescriptionContest = contest.DescriptionContest,
            ImageUrl = contest.ImageUrl,
            StartDate = contest.StartDate,
            EndDate = contest.EndDate,
            Tags = contest.Tags.Select(t => new CustomTagDTO
            {
                Id = t.Id,
                Name = t.Name,
                Color = t.Color
                // Otras propiedades necesarias
            }).ToList(),
            Fields = contest.Fields.Select(f => new FieldValueDTO
            {
                Id = f.Id,
                FieldName = f.FieldName,
                Value = f.Value?.BsonType == BsonType.String ? f.Value.AsString : f.Value?.ToString(),
                Type = f.Type,
                CatalogId = f.CatalogId,
                IsBase = f.IsBase,
                CreatedAt = f.CreatedAt,
                IsActive = f.IsActive
            }).ToList(),
            IsEvalued = contest.IsEvalued,
            IsSuspended = contest.IsSuspended,
            IsPay = contest.IsPay,
            Price = contest.Price,
            CreatedAt = contest.CreatedAt,
            IsActive = contest.IsActive
        };
    }
}
