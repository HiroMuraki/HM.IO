namespace HM.Cryptographers;

public abstract class CryptographerBase
{
    public Byte[] Key
    {
        get
        {
            _originKey.CopyTo(_copiedKey, 0);
            return _copiedKey;
        }
        set
        {
            _originKey = new Byte[value.Length];
            value.CopyTo(_originKey, 0);
            value.CopyTo(_copiedKey, 0);
        }
    }

    #region NonPublic
    protected Byte[] _copiedKey = Array.Empty<Byte>();
    protected Byte[] _originKey = Array.Empty<Byte>();
    #endregion
}
