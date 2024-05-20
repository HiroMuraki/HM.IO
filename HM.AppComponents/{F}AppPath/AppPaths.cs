using System.Collections;

namespace HM.AppComponents;

public class AppPaths<TToken> :
    IEnumerable<AppPath>
    where TToken : notnull
{
    public AppPath this[TToken token]
    {
        get => _appPaths[token];
        set => _appPaths[token] = value;
    }

    public void EnsureAppPathsCreated()
    {
        foreach (AppPath appPath in _appPaths.Values)
        {
            switch (appPath.PathType)
            {
                case AppPathType.Directory when !Directory.Exists(appPath.Path):
                    Directory.CreateDirectory(appPath.Path);
                    break;
                case AppPathType.File when !File.Exists(appPath.Path):
                    File.Create(appPath.Path).Dispose();
                    break;
            }
        }
    }

    public IEnumerator<AppPath> GetEnumerator()
    {
        foreach (AppPath appPath in _appPaths.Values)
        {
            yield return appPath;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #region NonPublic
    private readonly Dictionary<TToken, AppPath> _appPaths = [];
    #endregion
}

