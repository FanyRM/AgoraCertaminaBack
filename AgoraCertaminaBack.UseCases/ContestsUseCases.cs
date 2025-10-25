using AgoraCertaminaBack.UseCases.Contests;

namespace AgoraCertaminaBack.UseCases
{
    public record class ContestsUseCases
    (
        ConvertToContestDTO ConvertToContestDTO,
        CreateContest CreateContest,
        DeleteByIdContest DeleteByIdContest,
        GetByIdContest GetByIdContest,
        GetContests GetContests,
        GetDisabledContests GetDisabledContests,
        GetEntityByIdContest GetEntityByIdContest,
        GetS3Files GetS3Files,
        UpdateContest UpdateContest
    );
}
