#if EXPERIEMENTAL
namespace HM.Common.Experimental;

public static class OptionExtensions
{
    public static Option<T> AsOption<T>(this T? obj)
        where T : class
    {
        return new Option<T>(obj);
    }
}
#endif