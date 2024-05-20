namespace HM.AppComponents;

public interface ISignalReceiver<TSignalArg>
{
    void Receive(TSignalArg signalArg);
}
