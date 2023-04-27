namespace Dfe.SchoolAccount.Web.Models;

public sealed class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}
