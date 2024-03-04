using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace HM.AppDatabase;

public static class IDatabaseExtensions
{
    public static bool TryQuery<T>(this IDatabase<T> self, Expression<Func<T, bool>> predicate, [NotNullWhen(true)] out T? result)
        where T : class
    {
        result = self.Query(predicate);

        return result != null;
    }
}
