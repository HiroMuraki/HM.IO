namespace HM.IO;

/// <summary>
/// Represents an interface for an error handler that handles exceptions.
/// </summary>
public interface IErrorHandler
{
    /// <summary>
    /// Handles the specified exception.
    /// </summary>
    /// <param name="e">The exception to be handled.</param>
    /// <returns>True if the exception was successfully handled; otherwise, false.</returns>
    Boolean Handle(Exception e);
}
