using System.Security.Cryptography;
using System.Text;
using LessonTool.Common.Domain.Interfaces;

namespace LessonTool.Common.Domain.Services;

public class HashService : IHashService
{
    private const int keySize = 64;
    private const int iterations = 350000;
    private HashAlgorithmName algorithm = HashAlgorithmName.SHA512;

    /// <summary>
    /// Performs a simple hash on a string
    /// </summary>
    /// <param name="content">Content to hash</param>
    public string HashString(string content)
    {
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(content)));
    }

    /// <summary>
    /// Performs a hash and salt of a secure string for storage
    /// </summary>
    /// <param name="content">Content to hash</param>
    /// <param name="salt">Generated salt to hash with</param>
    /// <returns></returns>
    public string HashStringWithSalt(string content, byte[] salt)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(content), salt, iterations, algorithm, keySize);
        return Convert.ToHexString(hash);
    }

    /// <summary>
    /// Generates a secure salt to use when hashing
    /// </summary>
    /// <returns></returns>
    public byte[] CreateSalt()
    {
        return RandomNumberGenerator.GetBytes(keySize);
    }
}
