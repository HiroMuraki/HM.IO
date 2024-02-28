#if OLD2
namespace HM.IO;

/// <summary>
/// 	Represents an implementation of the <see cref="IErrorHandler"/> interface for handling exceptions.
/// </summary>
public class ErrorHandler : IErrorHandler
{
    /// <summary>
    /// 	Creates a new instance of <see cref="ErrorHandler"/> with the specified error handling function.
    /// </summary>
    /// <param name="errorHandler">The function that handles exceptions. It should return true if the exception is successfully handled; otherwise, false.</param>
    /// <returns>
    /// 	A new <see cref="ErrorHandler"/> instance.
    /// </returns>
    public static ErrorHandler Create(Func<Exception, Boolean> errorHandler)
    {
        return new ErrorHandler(errorHandler);
    }

    /// <summary>
    /// 	Handles the specified exception using the configured error handling function.
    /// </summary>
    /// <param name="e">The exception to be handled.</param>
    /// <returns>
    /// 	True if the exception was successfully handled; otherwise, false.
    /// </returns>
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