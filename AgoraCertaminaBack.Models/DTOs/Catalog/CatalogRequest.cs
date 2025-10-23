using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraCertaminaBack.Models.DTOs.Catalog
{
    public class CreateCatalogRequest
    {
        public required string Name { get; set; }
        public required List<string> Values { get; set; }
    }

    public class UpdateCatalogValuesRequest
    {
        public required List<string> Values { get; set; } = [];
    }

    public class CatalogResponse
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<string> Values { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
