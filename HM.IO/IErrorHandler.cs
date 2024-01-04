namespace HM.IO;

/// <include file='IErrorHandler.xml' path='IErrorHandler/Class[@name="IErrorHandler"]/*' />
public interface IErrorHandler
{
    /// <include file='IErrorHandler.xml' path='IErrorHandler/Methods/Instance[@name="Handle[Exception]"]/*' />
    Boolean Handle(Exception e);
}
