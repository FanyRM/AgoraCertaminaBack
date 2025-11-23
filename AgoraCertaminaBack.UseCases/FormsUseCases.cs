using AgoraCertaminaBack.UseCases.Forms;
using AgoraCertaminaBack.UseCases.Forms.FieldsForm;

namespace AgoraCertaminaBack.UseCases
{
    public record class FormsUseCases(
        CreateForm CreateForm,
        CreateFormFromTemplate CreateFormFromTemplate,  // ← NUEVO
        GetAvailableTemplates GetAvailableTemplates,    // ← NUEVO
        GetAllForms GetAllForms,
        GetByIdForm GetByIdForm,
        GetByIdFormDTO GetByIdFormDTO,
        UpdateForm UpdateForm,
        DeleteFormById DeleteFormById
    );

    public record class FormFieldsUseCases(
       CreateFormField CreateFormField,
       GetAllFormFields GetAllFormFields,
       UpdateFormField UpdateFormField,
       UpdateFormFieldsOrder UpdateFormFieldsOrder,
       DeleteFormField DeleteFormField
    );
}