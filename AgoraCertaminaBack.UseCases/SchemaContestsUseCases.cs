using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.UseCases.SchemaContests;
using AgoraCertaminaBack.UseCases.SchemaContests.SchemaField;
using AgoraCertaminaBack.UseCases.SchemaContests.SchemaTag;
using ControlStockAPI.UseCases.SchemaContests.SchemaTag;

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

    public record class SchemaFieldsUseCases(
      CreateSchemaField CreateSchemaField,
      GetAllSchemaFields GetAllSchemaFields,
      EditSchemaField EditSchemaField,
      DeleteSchemaField DeleteSchemaField
   );

    public record class SchemaTagsUseCases(
       AssignSchemaTag AssignSchemaTag,
       GetAllSchemaTags GetAllSchemaTags,
       DeleteSchemaTag DeleteSchemaTag
    );
}
