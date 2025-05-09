﻿using BaseApplication.Interfaces;
using PasswordGenerator;
using System.Security.Cryptography;
using System.Text;

namespace BaseInfrastructure.Services;

public class CypherService : ICypherService
{
    public async Task<byte[]> EncryptAsync(string clearText, string passphrase)
    {
        using Aes aes = Aes.Create();
        aes.Key = DeriveKeyFromPassword(passphrase);
        aes.IV = IV;
        using MemoryStream output = new();
        using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
        await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(clearText));
        await cryptoStream.FlushFinalBlockAsync();
        return output.ToArray();
    }

    public async Task<string> DecryptAsync(byte[] encrypted, string passphrase)
    {
        using Aes aes = Aes.Create();
        aes.Key = DeriveKeyFromPassword(passphrase);
        aes.IV = IV;
        using MemoryStream input = new(encrypted);
        using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
        using MemoryStream output = new();
        await cryptoStream.CopyToAsync(output);
        return Encoding.Unicode.GetString(output.ToArray());
    }

    public string GeneratePassword(int length = 8) => 
        new Password(length).IncludeLowercase().IncludeUppercase().IncludeSpecial().IncludeNumeric().Next();




    private static byte[] DeriveKeyFromPassword(string password)
    {
        var emptySalt = Array.Empty<byte>();
        var iterations = 1000;
        var desiredKeyLength = 16; // 16 bytes equal 128 bits.
        var hashMethod = HashAlgorithmName.SHA384;
        return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                         emptySalt,
                                         iterations,
                                         hashMethod,
                                         desiredKeyLength);
    }

    public string ConvertStringToBase64(string content)
    {
        var textBytes = Encoding.UTF8.GetBytes(content);
        var base64String = Convert.ToBase64String(textBytes);
        return base64String;
    }

    public string ConvertBase64ToString(string content)
    {
        var base64EncodedBytes = Convert.FromBase64String(content);
        var inputString = Encoding.UTF8.GetString(base64EncodedBytes);
        return inputString;
    }

    private readonly byte[] IV =
    {
        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
    };
}