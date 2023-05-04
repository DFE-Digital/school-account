namespace Dfe.SchoolAccount.SignIn.DsiClient;

public interface ITokenData
{
    IDictionary<string, object> Header { get; set; }

    IDictionary<string, object> Payload { get; set; }
}
