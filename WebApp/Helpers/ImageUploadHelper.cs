namespace WebApp.Helpers;


// Den är min idea! men chatgpt hjälpte mig med att skriva den :)))
public static class ImageUploadHelper
{
    public static async Task<string?> UploadAsync(IFormFile file, IWebHostEnvironment environment, string folder = "images")
    {
        if (file == null || file.Length == 0)
            return null;

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(environment.WebRootPath, folder, fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{folder}/{fileName}";

    }
}
