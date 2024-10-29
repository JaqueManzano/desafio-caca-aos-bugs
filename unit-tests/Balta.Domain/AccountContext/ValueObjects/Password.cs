using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Balta.Domain.AccountContext.ValueObjects.Exceptions;
using Balta.Domain.SharedContext.Abstractions;
using Balta.Domain.SharedContext.ValueObjects;

namespace Balta.Domain.AccountContext.ValueObjects;

public record Password : ValueObject
{
    #region Constants

    private const int MinLength = 8;
    private const int MaxLength = 48;
    private const string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    private const string Special = "!@#$%ˆ&*(){}[];";
    #endregion


    public class ConcreteDateTimeProvider     {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }

    #region Constructors

    private Password(string hash)
    {
        Hash = hash;
        ExpiresAtUtc = null;
        MustChange = false;
    }

    #endregion

    #region Factories

    public static Password ShouldCreate(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new InvalidPasswordException("Password cannot be null or empty");

        if (string.IsNullOrWhiteSpace(plainText))
            throw new InvalidPasswordException("Password cannot be null or empty");

        if (plainText.Length < MinLength)
            throw new InvalidPasswordException($"Password should have at least {MinLength} characters");

        if (plainText.Length > MaxLength)
            throw new InvalidPasswordException($"Password should have less than {MaxLength} characters");

        var hash = ShouldHashPassword(plainText);
        
        return new Password(hash);
    }

    #endregion

    #region Properties

    public string Hash { get; }
    public DateTime? ExpiresAtUtc { get; private set; }
    public bool MustChange { get; private set; }

    #endregion

    #region Public Methods
    public bool IsPasswordExpired()
    {
        return DateTime.UtcNow >= ExpiresAtUtc;
    }

    public void SetNewExpiresAtUtc(DateTime? dateTime)
    { 
        ExpiresAtUtc = dateTime; 
    }

    public void SetMustChange()
    {
        MustChange = true;
    }

    public static string ShouldGenerate(
        short length = 16,
        bool includeSpecialChars = true,
        bool upperCase = true)
    {
        var chars = includeSpecialChars ? (Valid + Special) : Valid;
        var startRandom = upperCase ? 26 : 0;
        var index = 0;
        var res = new char[length];
        var rnd = new Random();
                
        if (!upperCase) 
        {
            res[0] = chars[rnd.Next(0, 26)];
            res[1] = chars[rnd.Next(26, 52)];
            index = 2;
        }

        while (index < length)
            res[index++] = chars[rnd.Next(startRandom, chars.Length)];
                
        return new string(res.OrderBy(_ => rnd.Next()).ToArray());
    }

    public static bool ShouldMatch(
        string hash,
        string password,
        short keySize = 32,
        int iterations = 10000,
        char splitChar = '.')
    {
        password += Configuration.Api.PasswordSalt;

        var parts = hash.Split(splitChar, 3);
        if (parts.Length != 3)
            return false;

        var hashIterations = Convert.ToInt32(parts[0]);
        var salt = Convert.FromBase64String(parts[1]);
        var key = Convert.FromBase64String(parts[2]);

        if (hashIterations != iterations)
            return false;

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256);
        var keyToCheck = algorithm.GetBytes(keySize);

        return keyToCheck.SequenceEqual(key);
    }

    public static bool IsStrongPassword(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            return false;

        bool hasUpperCase = password.Any(char.IsUpper);
        bool hasLowerCase = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecialChar = Regex.IsMatch(password, @"[\W_]");

        if (!hasUpperCase || !hasLowerCase || !hasDigit || !hasSpecialChar)
            return false;

        for (int i = 0; i < password.Length - 1; i++)
        {
            if (password[i] + 1 == password[i + 1] || password[i] - 1 == password[i + 1])
                return false;
        }

        return true;
    }
    #endregion

    #region Private Methods

    private static string ShouldHashPassword(
        string password,
        short saltSize = 16,
        short keySize = 32,
        int iterations = 10000,
        char splitChar = '.')
    {
        if (string.IsNullOrEmpty(password))
            throw new InvalidPasswordException("Password should not be null or empty");

        password += Configuration.Api.PasswordSalt;

        using var algorithm = new Rfc2898DeriveBytes(
            password,
            saltSize,
            iterations,
            HashAlgorithmName.SHA256);
        var key = Convert.ToBase64String(algorithm.GetBytes(keySize));
        var salt = Convert.ToBase64String(algorithm.Salt);

        return $"{iterations}{splitChar}{salt}{splitChar}{key}";
    }

    #endregion

    #region Operators

    public static implicit operator Password(string plainTextPassword) => new(plainTextPassword);
    public static implicit operator string(Password password) => password.Hash;

    #endregion

    #region Overrides

    public override string ToString() => Hash;

    #endregion
}