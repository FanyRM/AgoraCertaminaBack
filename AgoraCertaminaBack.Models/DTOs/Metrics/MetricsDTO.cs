using AgoraCertaminaBack.Models.DTOs.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgoraCertaminaBack.Models.DTOs.Metrics
{

    public class FormStatusDetailsDTO
    {
        public string FormProgrammedId { get; set; } = string.Empty;
        public FormStatus FormStatus { get; set; }
    }

    public class FormStatusCountDTO
    {
        public FormStatus FormStatus { get; set; }
        public int CountFormStatus { get; set; }
    }

    public class FormTypeDetailsDTO
    {
        public List<FormStatusDetailsDTO> Details { get; set; } = new();
        public List<FormStatusCountDTO> StatusCount { get; set; } = new();
    }

    public class MetricsDTO
    {
        public int WithAssignmentsCount { get; set; }
        public int AnonymousCount { get; set; }
        public int OpenCount { get; set; }
        public int AssignedWithFeedbackCount { get; set; }

        public FormTypeDetailsDTO WithAssignmentsDetails { get; set; } = new();
        public FormTypeDetailsDTO AnonymousDetails { get; set; } = new();
        public FormTypeDetailsDTO OpenDetails { get; set; } = new();
        public FormTypeDetailsDTO AssignedWithFeedbackDetails { get; set; } = new();
    }

}
