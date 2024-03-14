using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace HM.AppDatabase;

public static class IDatabaseExtensions
{
    public static Boolean TryQuery<T>(this IDatabase<T> self, Expression<Func<T, Boolean>> predicate, [NotNullWhen(true)] out T? result)
        where T : class
    {
        result = self.Query(predicate);

        return result != null;
    }
}
