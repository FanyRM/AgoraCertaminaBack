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
                Color = request.Color
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
                Color = tag.Color
            };
        }

    }
}
