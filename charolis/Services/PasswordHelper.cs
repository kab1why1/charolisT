using System;

namespace charolis.Services;

public static class PasswordHelper
{
    private const int SaltSize = 16;
    private const int KeySize  = 32;
    private const int Iterations = 10000;

    public static string Hash(string password)
    {
        using var alg = new System.Security.Cryptography.Rfc2898DeriveBytes(
            password,
            SaltSize,
            Iterations,
            System.Security.Cryptography.HashAlgorithmName.SHA256);
        var key  = Convert.ToBase64String(alg.GetBytes(KeySize));
        var salt = Convert.ToBase64String(alg.Salt);
        return $"{Iterations}.{salt}.{key}";
    }

    public static bool Check(string hash, string password)
    {
        var parts     = hash.Split('.', 3);
        var iterations= int.Parse(parts[0]);
        var salt      = Convert.FromBase64String(parts[1]);
        var key       = Convert.FromBase64String(parts[2]);

        using var alg = new System.Security.Cryptography.Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            System.Security.Cryptography.HashAlgorithmName.SHA256);
        var keyToCheck = alg.GetBytes(KeySize);
        return keyToCheck.SequenceEqual(key);
    }
}

