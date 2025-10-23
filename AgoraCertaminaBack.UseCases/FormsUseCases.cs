using AgoraCertaminaBack.UseCases.Forms;
using AgoraCertaminaBack.UseCases.Forms.FieldsForm;
using AgoraCertaminaBack.UseCases.Forms.TagsForm;

namespace AgoraCertaminaBack.UseCases
{
    //public record class FormProgrammedUseCases(
    //    CreateFormProgrammed CreateFormProgrammed,
    //    DeleteFormProgrammed DeleteFormProgrammed,
    //    GetAccessKeyFormProgrammed GetAccessKeyFormProgrammed,
    //    GetAllFormsProgrammed GetAllFormsProgrammed,
    //    GetByIdFormProgrammed GetByIdFormProgrammed,
    //    GetByIdFormProgrammedDTO GetByIdFormProgrammedDTO,
    //    GetPublicFormPreview GetPublicFormPreview,
    //    GetPublicFormProgrammed GetPublicFormProgrammed,
    //    UpdateFormProgrammed UpdateFormProgrammed,
    //    ValidatePublicFormAccess ValidatePublicFormAccess
    //);

    //public record class FormAssignedUseCases(
    //    CreateFormAssignment CreateFormAssignment,
    //    DeleteFormAssigned DeleteFormAssigned,
    //    GetAccessKeyFormAssigned GetAccessKeyFormAssigned,
    //    GetAllFormAssignments GetAllFormAssignments,
    //    GetByIdFormAssigned GetByIdFormAssigned,
    //    GetByIdFormAssignedDTO GetByIdFormAssignedDTO,
    //    ValidatePrivateFormAccess ValidatePrivateFormAccess,
    //    GetPrivateFormPreview GetPrivateFormPreview,
    //    GetPrivateFormProgrammed GetPrivateFormProgrammed,
    //    UpdateFormAssigned UpdateFormAssigned
    //);

    public record class FormsUseCases(
        CreateForm CreateForm,
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
       //GetByIdFormVariations GetByIdFormVariations
    );

    public record class FormTagsUseCases(
       //AssignFormTag AssignFormTag,
       //GetAllFormTags GetAllFormTags,
       DeleteFormTag DeleteFormTag
    );
}
