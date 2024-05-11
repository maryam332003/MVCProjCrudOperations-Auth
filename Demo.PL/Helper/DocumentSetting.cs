using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public class DocumentSetting
    {
        public static string UploadFile(IFormFile file,string FolderName)
        {
            // 1 - Get location Folder path

            // string folderPath = Directory.GetCurrentDirectory() + "wwwroot\\files\\" + folderName;

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", FolderName);

            // 2 - Get file name make it unique

            string fileName = $"{Guid.NewGuid()}{file.FileName}";


            // 3 - Get File Path -->Folder Path + File Name

            string filePath = Path.Combine(folderPath, fileName);


            // 4 - Save file as stream : Data per time

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);
            return fileName;
        }
        public static void Delete(string fileName, string FolderName)
        {

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", FolderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
