using AgoraCertaminaBack.Models.DTOs.Contest;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Tag;
using AgoraCertaminaBack.Models.Entities;
using MongoDB.Bson;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class ContestMapper
    {
        #region Contest Mapping

        public static ContestDTO ConvertToContestDTO(this Contest contest)
        {
            return new ContestDTO
            {
                Id = contest.Id,
                ReferenceNumber = contest.ReferenceNumber,
                SchemaId = contest.SchemaId,
                SchemaName = contest.SchemaName,
                FormId = contest.FormId,
                ImageUrl = contest.ImageUrl,
                ContestName = contest.ContestName,
                DescriptionContest = contest.DescriptionContest,
                OrganizationName = contest.OrganizationName,
                StartDate = contest.StartDate,
                EndDate = contest.EndDate,
                IsPay = contest.IsPay,
                Price = contest.Price ?? 0,
                Tags = contest.Tags.Select(tag => new CustomTagDTO
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Color = tag.Color,

                }).ToList(),
                Fields = contest.Fields.Select(field => new FieldDTO
                {
                    Id = field.Id,
                    Name = field.FieldName,
                    IsBase = field.IsBase,
                    Value = field.Value?.ToString() ?? "",
                    CatalogId = field.CatalogId ?? "",
                    Type = field.Type
                }).ToList()
            };
        }

        public static Contest ConvertToContest(this CreateContestRequest createRequest)
        {
            return new Contest
            {
                Id = ObjectId.GenerateNewId().ToString(),
                SchemaId = createRequest.SchemaId,
                SchemaName = createRequest.SchemaName,
                FormId = createRequest.FormId,
                ContestName = createRequest.ContestName,
                DescriptionContest = createRequest.DescriptionContest,
                IsPay = createRequest.IsPay,
                Price = createRequest.Price ?? 0,
                StartDate = createRequest.StartDate,
                EndDate = createRequest.EndDate,
                Tags = createRequest.Tags?.ConvertToContestTags() ?? [],
                Fields = createRequest.Fields?.ConvertToContestFields() ?? [],
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsEvalued = false,
                IsSuspended = false
            };
        }

        public static Contest ApplyUpdateToContest(this Contest contest, ContestUpdateRequest updateRequest)
        {
            return new Contest
            {
                Id = contest.Id,
                ReferenceNumber = contest.ReferenceNumber,
                SchemaId = contest.SchemaId,
                SchemaName = contest.SchemaName,
                ContestName = updateRequest.ContestName,
                FormId = updateRequest.FormId,
                DescriptionContest = updateRequest.DescriptionContest,
                StartDate = updateRequest.StartDate,
                EndDate = updateRequest.EndDate,
                IsPay = updateRequest.IsPay,
                Price = updateRequest.Price ?? 0,
                Tags = contest.Tags,
                Fields = contest.Fields,
                CreatedAt = contest.CreatedAt,
                IsActive = contest.IsActive,
                IsEvalued = contest.IsEvalued,
                IsSuspended = contest.IsSuspended
            };
        }
        #endregion

        #region Tag Mapping

        public static Tag ConvertToTag(this CustomTagDTO tagDto)
        {
            return new Tag
            {
                Id = tagDto.Id,
                Name = tagDto.Name,
                Color = tagDto.Color
            };
        }

        public static List<Tag> ConvertToContestTags(this List<CustomTagDTO> tagDtos)
        {
            return tagDtos.Select(tagDto => tagDto.ConvertToTag()).ToList();
        }

        #endregion

        #region Field Mapping

        public static FieldValue ConvertToField(this FieldDTO fieldDto)
        {
            return new FieldValue
            {
                Id = fieldDto.Id,
                FieldName = fieldDto.Name,
                Type = fieldDto.Type,
                IsBase = fieldDto.IsBase,
                Value = fieldDto.Value,
                CreatedAt = DateTime.Now,
                IsActive = true,
            };
        }

        public static List<FieldValue> ConvertToContestFields(this List<FieldDTO> fieldDTOs)
        {
            return fieldDTOs.Select(fieldDto => fieldDto.ConvertToField()).ToList();
        }
        #endregion
    }
}