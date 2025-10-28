using AgoraCertaminaBack.Models.Entities;
using AgoraCertaminaBack.Models.DTOs.Form;
using AgoraCertaminaBack.Models.Mappers;

namespace AgoraCertaminaBack.Models.Mappers
{
    public static class FormMappers
    {
        //public static FormProgrammedDTO ToFormProgrammedDTO(this FormProgrammed programmed)
        //{
        //    return new FormProgrammedDTO
        //    {
        //        Id = programmed.Id,
        //        FormId = programmed.FormId,
        //        FormName = programmed.FormName,
        //        Folio = programmed.Folio,
        //        LaunchDate = programmed.LaunchDate,
        //        LaunchType = programmed.LaunchType,
        //        CloseDate = programmed.CloseDate
        //    };
        //}

        //public static FormAssignedDTO ToFormAssignedDTO(this FormAssigned assigned)
        //{
        //    return new FormAssignedDTO
        //    {
        //        Id = assigned.Id,
        //        FormId = assigned.FormId,
        //        FormName = assigned.FormName,
        //        ParticipantId = assigned.ParticipantId,
        //        ParticipantName = assigned.ParticipantName,
        //        FormStatus = assigned.FormStatus,
        //        StartDate = assigned.StartDate,
        //        LastModifiedDate = assigned.LastModifiedDate,
        //        CloseDate = assigned.CloseDate
        //    };
        //}

        public static FormDTO ToFormDTO(this Form form, LaunchType? launchType = null)
        {
            return new FormDTO
            {
                Id = form.Id,
                FormName = form.FormName,
                OrganizationId = form.OrganizationId, // ← Agregar
                CreatedAt = form.CreatedAt,
                LaunchType = launchType,
                CountSchemaFields = form.FormFields?.Count(field => field.IsActive) ?? 0,
                Tags = form.Tags?
                    .Where(tag => tag.IsActive)
                    .Select(tag => tag.ToCustomTagDTO())
                    .ToList() ?? [],
                FormFields = form.FormFields?
                    .Where(field => field.IsActive)
                    .Select(field => field.ToCustomFieldDTO())
                    .ToList() ?? []
            };
        }

    }
}
