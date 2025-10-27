using AgoraCertaminaBack.Models.Entities;
using MongoDB.Bson;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Form.FormTag;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class CustomTagMappers
    {
        public static Tag ToCustomTag(this CustomTagRequest request)
        {
            return new Tag
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Color = request.Color,
                Category = request.Category,
            };
        }

        public static Tag ToCustomTag(this ActionFormTagRequest request)
        {
            return new Tag
            {
                Id = string.IsNullOrEmpty(request.Id) ? ObjectId.GenerateNewId().ToString() : request.Id,
                Name = request.Name,
                Color = request.Color,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static Tag ToCustomTag(this SchemaTagRequest request)
        {
            return new Tag
            {
                Id = string.IsNullOrEmpty(request.Id) ? ObjectId.GenerateNewId().ToString() : request.Id,
                Name = request.Name,
                Color = request.Color,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public static CustomTagDTO ToCustomTagDTO(this Tag tag)
        {
            return new CustomTagDTO
            {
                Id = tag.Id,
                Name = tag.Name,
                Color = tag.Color,
                Category = tag.Category,
            };
        }

        public static Tag ConvertToTagContest(this CustomTagDTO tagDto)
        {
            return new Tag
            {
                Id = tagDto.Id,
                Name = tagDto.Name,
                Color = tagDto.Color
            };
        }

        public static List<Tag> ConvertToAllContestsTags(this List<CustomTagDTO> tagDtos)
        {
            return tagDtos.Select(tagDto => tagDto.ConvertToTagContest()).ToList();
        }

    }
}
