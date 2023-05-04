namespace Dfe.SchoolAccount.SignIn.DsiClient;

using System.Security.Cryptography;
using System.Text;

public sealed class TokenBuilder
{
    private readonly ITokenEncoder tokenEncoder;
    private readonly ITokenDataSerializer tokenDataSerializer;

    public TokenBuilder(ITokenDataSerializer tokenDataSerializer, ITokenData tokenData, ITokenEncoder tokenEncoder, IJsonWebAlgorithm jsonWebAlgorithm)
    {
        this.tokenDataSerializer = tokenDataSerializer;
        this.TokenData = tokenData;
        this.tokenEncoder = tokenEncoder;
        this.JsonWebAlgorithm = jsonWebAlgorithm;
    }

    public IJsonWebAlgorithm JsonWebAlgorithm { get; private set; }

    public string Issuer { get; internal set; } = null!;

    public string Audience { get; internal set; } = null!;

    public byte[] SecretKey { get; internal set; } = null!;

    public string Algorithm { get; internal set; } = null!;

    public ITokenData TokenData { get; internal set; } = null!;

    public string CreateToken()
    {
        if (string.IsNullOrWhiteSpace(this.Algorithm)) {
            throw new Exception("Algorithm");
        }
        if (string.IsNullOrWhiteSpace(this.Issuer)) {
            throw new Exception("Issuer");
        }
        if (string.IsNullOrWhiteSpace(this.Audience)) {
            throw new Exception("Audience");
        }
        if (this.SecretKey == null || this.SecretKey.Length < 1) {
            throw new Exception("SecretKey");
        }

        byte[] headerBytes = Encoding.UTF8.GetBytes(this.tokenDataSerializer.Serialize(this.TokenData.Header));
        byte[] payloadBytes = Encoding.UTF8.GetBytes(this.tokenDataSerializer.Serialize(this.TokenData.Payload));
        byte[] bytesToSign = Encoding.UTF8.GetBytes($"{this.tokenEncoder.Base64Encode(headerBytes)}.{this.tokenEncoder.Base64Encode(payloadBytes)}");
        byte[] signedBytes = this.SignToken(this.SecretKey, bytesToSign);

        string token = $"{this.tokenEncoder.Base64Encode(headerBytes)}.{this.tokenEncoder.Base64Encode(payloadBytes)}.{this.tokenEncoder.Base64Encode(signedBytes)}";

        return token;
    }

    private byte[] SignToken(byte[] key, byte[] bytesToSign)
    {
        using (var algorithm = HMAC.Create(this.Algorithm)) {
            if (algorithm == null) {
                throw new Exception("Crytography Creation");
            }

            algorithm.Key = key;
            return algorithm.ComputeHash(bytesToSign);
        }
    }
}
