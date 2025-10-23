using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.DTOs.Tag;
using AgoraCertaminaBack.Models.Entities;
using MongoDB.Bson;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class TagMappers
    {
        public static Tag CustomTagRequestToCustomTag(this CustomTagRequest request)
        {
            return new Tag
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Color = request.Color
            };
        }

        public static SchemaTagDTO ToCustomTagDTO(this Tag tag)
        {
            return new SchemaTagDTO
            {
                Id = tag.Id,
                Name = tag.Name,
                Color = tag.Color
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

    }
}
