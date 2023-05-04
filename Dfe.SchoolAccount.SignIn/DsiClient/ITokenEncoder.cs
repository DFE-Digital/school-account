namespace Dfe.SchoolAccount.SignIn.DsiClient;

public interface ITokenEncoder
{
    string Base64Encode(byte[] stringInput);
}
