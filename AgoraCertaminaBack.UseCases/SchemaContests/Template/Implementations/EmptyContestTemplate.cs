using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template.Implementations
{
    public class EmptyContestTemplate : IContestTemplateFactory
    {
        public CategoriesEnum GetCategory()
        {
            return CategoriesEnum.Empty;
        }

        public ContestTemplateData GetContestTemplateData()
        {
            return new ContestTemplateData();
        }

        public string GetDescription()
        {
            return "Convocatoria vacía - Sin campos predefinidos";
        }
    }
}
