using System.Text;

namespace HM.Cryptography;

public class XorTextCryptographer : XorCryptographerBase, ITextCryptographer
{
    public String Encrypt(String originText)
    {
        if (Key.Length == 0)
        {
            return originText;
        }
        Byte[] bytes = Encoding.UTF8.GetBytes(originText);
        XOREncryptCore(bytes);
        return Convert.ToHexString(bytes);
    }

    public String Decrypt(String encryptedText)
    {
        if (Key.Length == 0)
        {
            return encryptedText;
        }
        Byte[] bytes = Convert.FromHexString(encryptedText);
        XOREncryptCore(bytes);
        return Encoding.UTF8.GetString(bytes);
    }

    public XorTextCryptographer(Key key) : base(key) { }
}
