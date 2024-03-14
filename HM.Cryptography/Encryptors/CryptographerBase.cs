namespace HM.Cryptography.Encryptors;

public abstract class CryptographerBase
{
    public Key Key
    {
        get
        {
            return _key;
        }
    }

    #region NonPublic
    protected readonly Key _key;
    protected CryptographerBase(Key key)
    {
        _key = key;
    }
    #endregion
}

