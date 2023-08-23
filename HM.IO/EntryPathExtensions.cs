namespace HM.IO;

public static class EntryPathExtensions
{
    public static Boolean IsSubPathOf(this EntryPath self, EntryPath path)
    {
        if (self.Routes.Length <= path.Routes.Length)
        {
            return false;
        }

        for (Int32 i = 0; i < path.Routes.Length; i++)
        {
            if (self.Routes[i] != path.Routes[i])
            {
                return false;
            }
        }

        return true;
    }
}
