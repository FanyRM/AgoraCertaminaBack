using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Models.DTOs.Tag;
using Microsoft.AspNetCore.Http;

namespace AgoraCertaminaBack.Models.DTOs.Contest
{
    public class CreateContestRequest
    {
        public required string SchemaId { get; set; }
        public required string SchemaName { get; set; }
        public List<TagDTO> Tags { get; set; } = [];
        public List<FieldDTO> Fields { get; set; } = [];
    }

    public class ContestUpdateRequest
    {
        public List<TagDTO>? Tags { get; set; }
        public List<FieldDTO>? Fields { get; set; }
    }

}
