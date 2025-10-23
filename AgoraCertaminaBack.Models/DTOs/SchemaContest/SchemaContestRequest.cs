using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraCertaminaBack.Models.DTOs.SchemaContest
{
    public class CreateSchemaContestRequest
    {
        public required string SchemaName { get; set; }
        public List<SchemaTagRequest> Tags { get; set; } = new List<SchemaTagRequest>();
    }

    public class UpdateSchemaRequest
    {
        public required string SchemaName { get; set; }
        public required List<SchemaTagRequest> Tags { get; set; }
    }
}
