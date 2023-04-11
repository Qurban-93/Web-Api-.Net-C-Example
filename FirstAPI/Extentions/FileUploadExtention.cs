using System.Runtime.CompilerServices;

namespace FirstAPI.Extentions
{
    public static class FileUploadExtention
    {
        public static bool CheckSize(this IFormFile file , int size)
        {
            return file.Length / 1024 / 1024 > size;
        }

        public static bool CheckType(this IFormFile file )
        {
            return file.ContentType.Contains("image");
        }

        public static string SaveImage(this IFormFile file ,string root, IWebHostEnvironment env)
        {
            string fileName = Guid.NewGuid() + file.FileName;

            string fullPath = Path.Combine(env.WebRootPath ,root, fileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
         
            return  fileName;
        }
    }
}
