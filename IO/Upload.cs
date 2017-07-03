using Helper.Extensions;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Helper.IO
{
    public class Upload
    {
        public static string File(string file, string directory, bool deleteOnSuccess = true)
        {
            try
            {
                var fileName = FileHelper.GetCheckedFileName(directory, file);

                if (deleteOnSuccess)
                    System.IO.File.Move(file, Path.Combine(directory, fileName));
                else
                    System.IO.File.Copy(file, Path.Combine(directory, fileName));

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("error on File method, message: " + ex.Message);
            }
        }

        public static string File(HttpPostedFile file, string directory)
        {
            try
            {
                var fileName = FileHelper.GetCheckedFileName(directory, new FileInfo(file.FileName).Name); //Use fileInfo to remove path -- Browser Quirk

                file.SaveAs(Path.Combine(directory, fileName));

                return fileName;
            }
            catch (Exception ex)
            {
                throw new Exception("error on File method, message: " + ex.Message);
            }
        }

        public static string Thumbnail(HttpPostedFile file, string directory, Size newSize)
        {
            try
            {
                var origImgData = FileHelper.SaveToMemory(file.InputStream);
                var newImgData = ImageHelper.ResizeImage(origImgData, newSize, ImageFormat.Png);

                var newFileName = FileHelper.GetCheckedFileName(directory, $"{new FileInfo(file.FileName).Name.GetFileName().ToUrlSlug()}.png");

                return FileHelper.SaveToDisk(newImgData, directory, newFileName);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Thumbnail method, message: " + ex.Message);
            }
        }
    }
}
