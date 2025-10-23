using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraCertaminaBack.Models.DTOs.Tag
{
    public class CustomTagRequest
    {
        public required string Name { get; set; }
        public required string Color { get; set; }
    }

    public class EditCustomTagRequest
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
    }
}
