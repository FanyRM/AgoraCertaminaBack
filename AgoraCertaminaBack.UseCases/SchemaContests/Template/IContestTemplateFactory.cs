using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgoraCertaminaBack.Models.DTOs.SchemaContest;
using AgoraCertaminaBack.Models.General;

namespace AgoraCertaminaBack.UseCases.SchemaContests.Template
{
    public interface IContestTemplateFactory
    {
        ContestTemplateData GetContestTemplateData();
        CategoriesEnum GetCategory();
        string GetDescription();
    }
}
