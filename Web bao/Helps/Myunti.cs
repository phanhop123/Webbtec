namespace WebQuanLyhs.Helps
{
    public class Myunti
    {
        public static string UploadHinh(IFormFile File, string folder)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, File.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    File.CopyTo(myfile);
                }
                return File.FileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
