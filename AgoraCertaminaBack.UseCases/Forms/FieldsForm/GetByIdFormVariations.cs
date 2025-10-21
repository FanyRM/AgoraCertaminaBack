//using ROP;
//using AgoraCertaminaBack.Models.Entities;
//using AgoraCertaminaBack.Models.General;
//using System;
//using System.Collections.Immutable;
//using System.Linq;

//namespace AgoraCertaminaBack.UseCases.Forms.FieldsForm
//{
//    public class GetByIdFormVariations
//    {
//        public Result<FieldVariationTypeEnum[]> Execute(FieldTypeEnum fieldType)
//        {
//            return GetVariations(fieldType)
//                .Map(v => v.ToArray()); //Se mapea para el imuatableArray 
//        }

//        public Result<ImmutableArray<FieldVariationTypeEnum>> GetVariations(FieldTypeEnum fieldType)
//        {
//            //Se hace la validación si existe en diccionario 
//            if (!FieldVariationType.Variations.ContainsKey(fieldType))
//                return Result.NotFound<ImmutableArray<FieldVariationTypeEnum>>("No variations were found for the type");
            
//            //se retorna el array que se encontro
//            var variations = FieldVariationType.Variations[fieldType];
//            //se retorna un succes 
//            return Result.Success(variations);
//        }
//    }
//}