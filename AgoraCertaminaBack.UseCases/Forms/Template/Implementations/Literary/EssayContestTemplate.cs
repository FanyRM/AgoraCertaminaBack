using AgoraCertaminaBack.Models.DTOs.Form.Templates;
using AgoraCertaminaBack.UseCases.Forms.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraCertaminaBack.UseCases.Forms.Template.Implementations.Literary
{
    public class EssayContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "literary-essay";
        public string GetName() => "Concurso de Ensayo";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.LiteraryContest;
        public string GetDescription() => "Plantilla para concursos de ensayo académico o literario";

        public FormTemplateData GetTemplateData()
        {
            // Implementación similar...
            return new FormTemplateData();
        }
    }

    /// <summary>
    /// Plantilla para MICRORRELATO
    /// </summary>
    public class MicrostoryContestTemplate : IFormTemplateFactory
    {
        public string GetTemplateId() => "literary-microstory";
        public string GetName() => "Concurso de Microrrelato";
        public FormTemplateCategory GetCategory() => FormTemplateCategory.LiteraryContest;
        public string GetDescription() => "Plantilla para microrrelatos (máximo 500 palabras)";

        public FormTemplateData GetTemplateData()
        {
            // Implementación similar...
            return new FormTemplateData();
        }
    }
}

