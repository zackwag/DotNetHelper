using Helper.Extensions;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Helper.IO
{
    public static class FileHelper
    {
        public static byte[] CreateArchive(IList<HttpPostedFile> files)
        {
            try
            {
                var fileStreams = files.ToDictionary(f => new FileInfo(f.FileName).Name, f => f.InputStream); //Use fileInfo to remove path -- Browser Quirk

                return CreateArchive(fileStreams);
            }
            catch (Exception ex)
            {
                throw new Exception("error on CreateArchive method, message: " + ex.Message);
            }
        }

        public static byte[] CreateArchive(Dictionary<string, Stream> fileStreams)
        {
            try
            {
                using (var zip = new ZipFile())
                {
                    foreach (var fileStream in fileStreams)
                    {
                        var byteContent = SaveToMemory(fileStream.Value);

                        zip.AddEntry(fileStream.Key, byteContent);
                    }

                    using (var writeStream = new MemoryStream())
                    {
                        zip.Save(writeStream);

                        //Reset stream position
                        writeStream.Seek(0, SeekOrigin.Begin);

                        var byteContent = SaveToMemory(writeStream);

                        return byteContent;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on CreateArchive method, message: " + ex.Message);
            }
        }

        public static byte[] SaveToMemory(Uri requestUri, NetworkCredential credentials = null)
        {
            try
            {
                using (var client = new WebClient())
                {
                    if (credentials.HasValue())
                        client.Credentials = credentials;

                    return client.DownloadData(requestUri);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on SaveToMemory method, message: " + ex.Message);
            }
        }

        public static byte[] SaveToMemory(HttpPostedFile file)
        {
            try
            {
                return SaveToMemory(file.InputStream);
            }
            catch (Exception ex)
            {
                throw new Exception("error on SaveToMemory method, message: " + ex.Message);
            }
        }

        public static byte[] SaveToMemory(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    return SaveToMemory(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on SaveToMemory method, message: " + ex.Message);
            }
        }

        public static byte[] SaveToMemory(Stream stream)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    if (stream == null) return ms.ToArray();

                    if (stream.CanSeek)
                        stream.Position = 0;

                    stream.CopyTo(ms);

                    if (ms.CanSeek)
                        ms.Position = 0;

                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on SaveToMemory method, message: " + ex.Message);
            }
        }

        public static string SaveToDisk(HttpPostedFile file, string directory, string fileName, Boolean checkFileName = true)
        {
            try
            {
                if (!Directory.Exists(directory)) return string.Empty;
                var checkedName = checkFileName ? GetCheckedFileName(directory, fileName) : fileName;

                file.SaveAs(Path.Combine(directory, checkedName));

                return checkedName;
            }
            catch (Exception ex)
            {
                throw new Exception("error on SaveToDisk method, message: " + ex.Message);
            }
        }

        public static string SaveToDisk(byte[] fileData, string directory, string fileName, Boolean checkFileName = true)
        {
            try
            {
                if (!Directory.Exists(directory)) return string.Empty;
                var checkedName = checkFileName ? GetCheckedFileName(directory, fileName) : fileName;

                using (var fs = new FileStream(Path.Combine(directory, checkedName), FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileData, 0, fileData.Length);
                }

                return checkedName;
            }
            catch (Exception ex)
            {
                throw new Exception("error on SaveToDisk method, message: " + ex.Message);
            }
        }

        public static string GetFileContents(string filePath)
        {
            try
            {
                return File.Exists(filePath) ? SaveToMemory(filePath).ToString(Encoding.UTF8) : string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetFileContents method, message: " + ex.Message);
            }
        }

        public static string GetFileContents(Stream stream)
        {
            try
            {
                return stream == null ? string.Empty : SaveToMemory(stream).ToString(Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetFileContents method, message: " + ex.Message);
            }
        }

        public static string GetCheckedFileName(string directory, string file)
        {
            //Check for iteration tag at end of file
            var name = Regex.Replace(file.GetFileName(), @"\(\d*\)$", string.Empty).ToUrlSlug();

            var ext = file.GetFileExtension().ToLower();

            //Build the path to save the file
            var pathToCheck = Path.Combine(directory, $"{name}.{ext}");

            //Check to see if the file exists, if it does, append to the name and try again.
            var count = 0;
            if (!File.Exists(pathToCheck))
                return $"{name}{(count == 0 ? string.Empty : $"({count})")}.{ext}";

            while (File.Exists(pathToCheck))
            {
                count++;

                pathToCheck = Path.Combine(directory, $"{name}({count}).{ext}");
            }

            return $"{name}{(count == 0 ? string.Empty : $"({count})")}.{ext}";
        }

        public static string GetFileSize(long fileSize)
        {
            var formats = new List<string>(new[] { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" });
            double tempSize = fileSize;
            int i;

            for (i = 0; i < formats.Count - 1 && tempSize >= 1024; i++)
            {
                tempSize = ((100 * tempSize / 1024) / 100.0);
            }

            return $"{tempSize:###,###,##0.##} {formats[i]}";
        }

        public static string GetMimeType(string ext)
        {
            try
            {
                string mimeType;

                using (var client = new WebClient())
                {
                    mimeType = client.DownloadString($"http://www.stdicon.com/ext/{ext}");
                }

                return mimeType;
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetMimeType method, message: " + ex.Message);
            }
        }
    }
}
