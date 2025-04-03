namespace Company.G01.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName) 
        {
            //string folerPath = "C:\\Users\\DELL\\OneDrive\\Desktop\\C#Route\\Company.G01\\Company.G01.PL\\wwwroot\\files\\" + folderName;
            //var folerPath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;

            //var folerPath = Path.Combine(Directory.GetCurrentDirectory() + @"wwwroot\files" + folderName);

            //var fileName = $"{Guid.NewGuid()}_{file.FileName}";

            //var filePath = Path.Combine(folerPath, fileName);

            //using var fileStream = new FileStream(filePath , FileMode.Create);

            //file.CopyTo(fileStream);    

            //return fileName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName);

            // Ensure the directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Generate a unique filename
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            var filePath = Path.Combine(folderPath, fileName);

            // Save the file
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            return fileName;
        }

        public static void DeleteFile(string fileName , string folderName ) 
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory() , @"wwwroot\files" + folderName , fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }


    }
}
