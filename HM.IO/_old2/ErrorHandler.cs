#if OLD2
namespace HM.IO;

public class ErrorHandler : IErrorHandler
{
    public static ErrorHandler Create(Func<Exception, Boolean> errorHandler)
    {
        return new ErrorHandler(errorHandler);
    }

    public Boolean Handle(Exception e)
    {
        return _errorHandler(e);
    }

    #region NonPublic
    private readonly Func<Exception, Boolean> _errorHandler;
    private ErrorHandler(Func<Exception, Boolean> errorHandler)
    {
        _errorHandler = errorHandler;
    }
    #endregion
}
#endif