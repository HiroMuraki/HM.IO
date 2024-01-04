namespace HM.IO;

/// <include file='Docs/ErrorHandler.xml' path='ErrorHandler/Class[@name="ErrorHandler"]/*' />

public class ErrorHandler : IErrorHandler
{
    /// <include file='Docs/ErrorHandler.xml' path='ErrorHandler/Methods/Static[@name="Create[Func&lt;Exception, Boolean&gt;]"]/*' />
    public static ErrorHandler Create(Func<Exception, Boolean> errorHandler)
    {
        return new ErrorHandler(errorHandler);
    }

    /// <include file='Docs/ErrorHandler.xml' path='ErrorHandler/Methods/Instance[@name="Handle[Exception]"]/*' />
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
