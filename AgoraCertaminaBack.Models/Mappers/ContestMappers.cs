using AgoraCertaminaBack.Models.DTOs.Contest;
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
                Tags = contest.Tags.Select(tag => new TagDTO
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Color = tag.Color
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
                Tags = createRequest.Tags?.ConvertToContestTags() ?? [],
                Fields = createRequest.Fields?.ConvertToContestFields() ?? [],
                CreatedAt = DateTime.Now,
                IsActive = true
            };
        }
        #endregion

        #region Tag Mapping

        public static Tag ConvertToTag(this TagDTO tagDto)
        {
            return new Tag
            {
                Id = tagDto.Id,
                Name = tagDto.Name,
                Color = tagDto.Color
            };
        }

        public static List<Tag> ConvertToContestTags(this List<TagDTO> tagDtos)
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