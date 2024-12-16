namespace BaseApplication.Interfaces
{
    public interface ICypherService
    {
        Task<byte[]> EncryptAsync(string clearText, string passphrase);
        Task<string> DecryptAsync(byte[] encrypted, string passphrase);
        string GeneratePassword(int length = 8);
        string ConvertStringToBase64(string content);
        string ConvertBase64ToString(string content);
    }
}