namespace Dfe.SchoolAccount.SignIn.Models;

public sealed class Organisation
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Category Category { get; set; } = null!;

    public string Urn { get; set; } = null!;

    public string Uid { get; set; } = null!;

    public string UkPrn { get; set; } = null!;

    public string LegacyId { get; set; } = null!;

    public string Sid { get; set; } = null!;

    public string DistrictAdministrative_Code { get; set; } = null!;
}
