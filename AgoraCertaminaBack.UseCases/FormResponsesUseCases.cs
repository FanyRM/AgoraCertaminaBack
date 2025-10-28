using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraCertaminaBack.UseCases
{
    public record class FormResponsesUseCases(
      GetFormToResponse GetFormToResponse,        // Obtener formulario
      SaveFormResponse SaveFormResponse,           // Guardar/actualizar respuesta
      SubmitFormResponse SubmitFormResponse       // Enviar respuesta final
  );
}
