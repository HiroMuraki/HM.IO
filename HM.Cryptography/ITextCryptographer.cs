namespace HM.Cryptographers;

public interface ITextCryptographer
{
    String Encrypt(String originText);

    String Decrypt(String encryptedText);
}
