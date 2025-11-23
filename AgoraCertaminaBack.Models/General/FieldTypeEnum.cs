using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraCertaminaBack.Models.General
{
    public enum FieldTypeEnum
    {
        String = 0,
        Boolean = 1,
        Integer = 2,
        Float = 3,         // ← Mantener por compatibilidad (si lo usas en otros lados)
        Decimal = 3,       // ← MISMO valor que Float (puedes tener ambos)
        Date = 4,
        Datetime = 5,
        Image = 6,
        Archive = 7,
        CustomCatalog = 8  // ← CRÍTICO: Debe ser 8
    }
}
