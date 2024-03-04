namespace HM.Common;

public static class ParallelSafe
{
    public static void Run(Action action, Action waitAction, Action releaseAction)
    {
        waitAction();

        try
        {
            action();
        }
        finally
        {
            releaseAction();
        }
    }

    public static T Run<T>(Func<T> func, Action waitAction, Action releaseAction)
    {
        waitAction();

        try
        {
            return func();
        }
        finally
        {
            releaseAction();
        }
    }

    public static async Task RunAsync(Func<Task> asyncAction, Action waitAction, Action releaseAction)
    {
        waitAction();

        try
        {
            await asyncAction();
        }
        finally
        {
            releaseAction();
        }
    }

    public static async Task<T> RunAsync<T>(Func<Task<T>> asyncAction, Action waitAction, Action releaseAction)
    {
        waitAction();

        try
        {
            return await asyncAction();
        }
        finally
        {
            releaseAction();
        }
    }
}
