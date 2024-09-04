using System.Security.Cryptography;
using BillTrack.Core.Interfaces.Utils;

namespace BillTrack.Auth.Utils;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 300000;
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA512;
    private const char Delimeter = ';';
    
    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

        return string.Join(Delimeter, Convert.ToHexString(salt), Convert.ToHexString(hash));
    }

    public bool Verify(string passwordHash, string inputPassword)
    {
        var elements = passwordHash.Split(Delimeter);
        var salt = Convert.FromHexString(elements[0]);
        var hash = Convert.FromHexString(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}