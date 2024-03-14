namespace HM.Cryptography.Encryptors;

public interface ITextCryptographer
{
    String Encrypt(String originText);

    String Decrypt(String encryptedText);
}
