namespace AgoraCertaminaBack.Models.DTOs.Form.Templates
{
    public enum FormTemplateCategory
    {
        Empty = 0,                    // Formulario vacío (comportamiento actual)
        ContestRegistration = 1,      // Inscripción a concurso general
        LiteraryContest = 2,          // Concurso literario (poesía, cuento, ensayo)
        ArtContest = 3,               // Concurso de arte (pintura, escultura, fotografía)
        AcademicCompetition = 4,      // Competencia académica (matemáticas, ciencias, debate)
        TalentShow = 5,               // Concurso de talentos (música, danza, teatro)
        SportsCompetition = 6         // Competencia deportiva
    }
}