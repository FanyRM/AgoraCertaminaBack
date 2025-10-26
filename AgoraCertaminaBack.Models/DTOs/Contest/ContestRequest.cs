using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Models.DTOs.CustomTag;
using AgoraCertaminaBack.Models.DTOs.Tag;
using Microsoft.AspNetCore.Http;

namespace AgoraCertaminaBack.Models.DTOs.Contest
{
    public class CreateContestRequest
    {
        public required string SchemaId { get; set; }
        public required string SchemaName { get; set; }
        public required string ContestName { get; set; }
        public required string FormId { get; set; }
        public required string ImageUrl { get; set; }
        public required string DescriptionContest { get; set; }
        public required bool IsPay { get; set; }
        public double? Price { get; set; }
        public List<CustomTagDTO> Tags { get; set; } = [];
        public List<FieldDTO> Fields { get; set; } = [];
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ContestUpdateRequest
    {
        public required string ContestName { get; set; }
        public required string FormId { get; set; }
        public required string ImageUrl { get; set; }
        public required string DescriptionContest { get; set; }
        public required bool IsPay { get; set; }
        public double? Price { get; set; }
        public List<CustomTagDTO>? Tags { get; set; }
        public List<FieldDTO>? Fields { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ContestStatusUpdateRequest
    {
        public required bool IsSuspended { get; set; }
        public required bool IsEvalued { get; set; }
    }
}
