namespace HM.IO;

public static class FileUtilityExtensions
{
    public static async Task<Boolean> CompareEqualityAsync(this FileUtility self, EntryPath filePath1, EntryPath filePath2)
        => await self.CompareEqualityAsync(filePath1, filePath2, CancellationToken.None);

    public static async Task CopyAsync(this FileUtility self, EntryPath sourceFilePath, EntryPath destinationFilePath)
        => await self.CopyAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    public static async Task CopyAsync(this FileUtility self, EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite)
        => await self.CopyAsync(sourceFilePath, destinationFilePath, overwrite, CancellationToken.None);

    public static async Task CopyAsync(this FileUtility self, EntryPath sourceFilePath, EntryPath destinationFilePath, CancellationToken cancellationToken)
        => await self.CopyAsync(sourceFilePath, destinationFilePath, false, cancellationToken);

    public static async Task MoveAsync(this FileUtility self, EntryPath sourceFilePath, EntryPath destinationFilePath)
        => await self.MoveAsync(sourceFilePath, destinationFilePath, false, CancellationToken.None);

    public static async Task MoveAsync(this FileUtility self, EntryPath sourceFilePath, EntryPath destinationFilePath, Boolean overwrite)
        => await self.MoveAsync(sourceFilePath, destinationFilePath, overwrite, CancellationToken.None);

    public static async Task MoveAsync(this FileUtility self, EntryPath sourceFilePath, EntryPath destinationFilePath, CancellationToken cancellationToken)
        => await self.MoveAsync(sourceFilePath, destinationFilePath, false, cancellationToken);

    public static async Task<String> ComputeHashAsync(this FileUtility self, EntryPath filePath)
        => await self.ComputeHashAsync(filePath, CancellationToken.None);
}