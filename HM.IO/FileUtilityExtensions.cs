namespace HM.IO;

public static class FileUtilityExtensions
{
    #region Methods
    #region CompareEquality
    public static async Task<Boolean> CompareEqualityAsync(this FileUtility self, String filePath1, String filePath2)
        => await self.CompareEqualityAsync(EntryPath.CreateFromPath(filePath1), EntryPath.CreateFromPath(filePath2));

    public static async Task<Boolean> CompareEqualityAsync(this FileUtility self, String filePath1, String filePath2, CancellationToken cancellationToken)
        => await self.CompareEqualityAsync(EntryPath.CreateFromPath(filePath1), EntryPath.CreateFromPath(filePath2), cancellationToken);
    #endregion

    #region Copy
    public static async Task CopyAsync(this FileUtility self, String sourceFilePath, String destinationFilePath)
        => await self.CopyAsync(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath));

    public static async Task CopyAsync(this FileUtility self, String sourceFilePath, String destinationFilePath, Boolean overwrite)
        => await self.CopyAsync(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath), overwrite);

    public static async Task CopyAsync(this FileUtility self, String sourceFilePath, String destinationFilePath, CancellationToken cancellationToken)
        => await self.CopyAsync(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath), cancellationToken);

    public static async Task CopyAsync(this FileUtility self, String sourceFilePath, String destinationFilePath, Boolean overwrite, CancellationToken cancellationToken)
        => await self.CopyAsync(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath), overwrite, cancellationToken);
    #endregion

    #region Move
    public static async Task MoveAsync(this FileUtility self, String sourceFilePath, String destinationFilePath)
        => await self.MoveAsync(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath));

    public static async Task MoveAsync(this FileUtility self, String sourceFilePath, String destinationFilePath, Boolean overwrite)
        => await self.MoveAsync(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath), overwrite);

    public static async Task MoveAsync(this FileUtility self, String sourceFilePath, String destinationFilePath, CancellationToken cancellationToken)
        => await self.MoveAsync(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath), cancellationToken);

    public static async Task MoveAsync(this FileUtility self, String sourceFilePath, String destinationFilePath, Boolean overwrite, CancellationToken cancellationToken)
        => await self.MoveAsync(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath), overwrite, cancellationToken);
    #endregion

    #region MetaData
    public static void CopyTimestamps(this FileUtility self, String sourceFilePath, String destinationFilePath)
        => self.CopyTimestamps(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath));

    public static void CopyAttributes(this FileUtility self, String sourceFilePath, String destinationFilePath)
        => self.CopyAttributes(EntryPath.CreateFromPath(sourceFilePath), EntryPath.CreateFromPath(destinationFilePath));
    #endregion

    #region ComputeHash
    public static async Task<String> ComputeHashAsync(this FileUtility self, String filePath)
        => await self.ComputeHashAsync(EntryPath.CreateFromPath(filePath));

    public static async Task<String> ComputeHashAsync(this FileUtility self, String filePath, CancellationToken cancellationToken)
        => await self.ComputeHashAsync(EntryPath.CreateFromPath(filePath), cancellationToken);
    #endregion
    #endregion
}