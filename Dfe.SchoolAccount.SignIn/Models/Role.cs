namespace Dfe.SchoolAccount.SignIn.Models;

public sealed class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string NumericId { get; set; }

    public Status Status { get; set; } = null!;
}
