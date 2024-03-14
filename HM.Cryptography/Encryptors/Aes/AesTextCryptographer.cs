using System.Text;

namespace HM.Cryptography.Encryptors.Aes;

public class AesTextCryptographer : AesCryptographerBase, ITextCryptographer
{
    public Encoding Encoding { get; set; } = Encoding.UTF8;

    public String Decrypt(String encryptedText)
    {
        Byte[] decryptedBytes = ProcessCore(Convert.FromHexString(encryptedText), _aes.CreateDecryptor());
        return Encoding.GetString(decryptedBytes);
    }
    public String Encrypt(String originText)
    {
        Byte[] encryptedBytes = ProcessCore(Encoding.GetBytes(originText), _aes.CreateEncryptor());
        return Convert.ToHexString(encryptedBytes);
    }

    public AesTextCryptographer(Byte[] key) : base(key) { }
    public AesTextCryptographer(Byte[] key, Byte[] iv) : base(key, iv) { }
}
