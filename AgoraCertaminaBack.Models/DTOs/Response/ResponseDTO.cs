// Models/DTOs/FormResponse/FormResponseRequests.cs
public record SaveFormResponseRequest
{
    public string? ResponseId { get; set; }  // null si es nuevo
    public string FormId { get; set; } = string.Empty;
    public List<FieldResponseRequest> FieldResponses { get; set; } = new();
}

public record SubmitFormResponseRequest
{
    public string ResponseId { get; set; } = string.Empty;
    public ParticipantInfoRequest ParticipantInfo { get; set; } = new();
}

public record FieldResponseRequest
{
    public string FieldId { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public record ParticipantInfoRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}