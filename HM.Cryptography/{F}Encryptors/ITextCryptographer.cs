namespace HM.Cryptography;

public interface ITextCryptographer
{
    String Encrypt(String originText);

    String Decrypt(String encryptedText);
}
