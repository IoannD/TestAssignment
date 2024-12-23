namespace TestAssignment.API.Validators;

internal static class FileValidator
{
    public static bool IsFileExtensionAllowed(IFormFile file, string[] allowedExtensions)
    {
        var extension = Path.GetExtension(file.FileName);
        return allowedExtensions.Contains(extension);
    }

    public static bool IsFileSizeWithinLimit(IFormFile file, long maxSizeInBytes)
    {
        return file.Length <= maxSizeInBytes;
    }
}