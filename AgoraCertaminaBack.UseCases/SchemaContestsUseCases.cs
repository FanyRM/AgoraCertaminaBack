using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.UseCases.SchemaContests;

namespace AgoraCertaminaBack.UseCases
{
    public record class SchemaContestsUseCases
    (
        CreateSchema CreateSchema,
        DeleteSchemaById DeleteSchemaById,
        GetAllSchemas GetAllSchemas,
        GetByIdSchema GetByIdSchema,
        GetDataSchemaById GetDataSchemaById,
        GetEntityByIdSchema GetEntityByIdSchema,
        UpdateSchema UpdateSchema
    );
}
