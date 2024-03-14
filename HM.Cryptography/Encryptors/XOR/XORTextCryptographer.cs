using System.Text;

namespace HM.Cryptography.Encryptors.XOR;

public class XORTextCryptographer : XORCryptographerBase, ITextCryptographer
{
    public String Encrypt(String originText)
    {
        if (_originKey.Length == 0)
        {
            return originText;
        }
        Byte[] bytes = Encoding.UTF8.GetBytes(originText);
        XOREncryptCore(bytes);
        return Convert.ToHexString(bytes);
    }

    public String Decrypt(String encryptedText)
    {
        if (_originKey.Length == 0)
        {
            return encryptedText;
        }
        Byte[] bytes = Convert.FromHexString(encryptedText);
        XOREncryptCore(bytes);
        return Encoding.UTF8.GetString(bytes);
    }

    public XORTextCryptographer(Byte[] key) : base(key) { }
}
