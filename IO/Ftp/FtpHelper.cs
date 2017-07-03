using Helper.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Helper.IO.Ftp
{
    public class FtpHelper
    {
        #region GET METHODS
        public static IList<FtpListDetail> GetFiles(FtpConnection connection, string path = null)
        {
            try
            {
                return (from d in GetDirectoryListing(connection, path)
                    where !d.IsDirectory
                    orderby d.Name
                    select d).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetFiles method, message: " + ex.Message);
            }
        }

        public static IList<FtpListDetail> GetDirectories(FtpConnection connection, string path = null)
        {
            try
            {
                return (from d in GetDirectoryListing(connection, path)
                    where d.IsDirectory
                    orderby d.Name
                    select d).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetFiles method, message: " + ex.Message);
            }
        }

        private static IEnumerable<FtpListDetail> GetDirectoryListing(FtpConnection connection, string path = null)
        {
            try
            {
                var returnVal = default(List<FtpListDetail>);
                var request = connection.GenerateRequest(WebRequestMethods.Ftp.ListDirectoryDetails, path);

                using (var response = request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var line = reader.ReadLine();

                        if (line.HasValue())
                        {
                            returnVal = new List<FtpListDetail>();

                            while (line != null)
                            {
                                returnVal.Add(new FtpListDetail(line, connection.CurrentPath));
                                line = reader.ReadLine();
                            }
                        }
                    }
                }

                return returnVal;
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetDirectoryListing method, message: " + ex.Message);
            }
        }

        private static string GetCheckedFileName(FtpConnection connection, string fileNameToCheck, string path = null)
        {
            try
            {
                var newFileName = Regex.Replace(fileNameToCheck.GetFileName().ToUrlSlug(), @"(-\d*)$", string.Empty);
                var fileExt = fileNameToCheck.GetFileExtension().ToLower();
                var fullFileName = $"{newFileName}.{fileExt}";
                var files = GetFiles(connection, path);

                if (!files.Any(f => f.Name.Equals(fullFileName, StringComparison.OrdinalIgnoreCase)))
                    return fullFileName;
                {
                    var count = 1;
                    while (files.Any(f => f.Name.Equals(fullFileName, StringComparison.OrdinalIgnoreCase)))
                    {
                        fullFileName = $"{newFileName}-{count}.{fileExt}";

                        if (files.Any(f => f.Name.Equals(fullFileName, StringComparison.OrdinalIgnoreCase)))
                            count++;
                    }
                }

                return fullFileName;
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetCheckedFileName method, message: " + ex.Message);
            }
        }

        #endregion

        #region UPLOAD METHODS

        public static string Upload(FtpConnection connection, HttpPostedFile file, string path = null)
        {
            try
            {
                return Upload(connection, file.FileName, file.InputStream, path);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Upload method, message: " + ex.Message);
            }
        }

        public static string Upload(FtpConnection connection, string fileName, Stream stream, string path = null)
        {
            try
            {
                return Upload(connection, fileName, FileHelper.SaveToMemory(stream), path);
            }
            catch (Exception ex)
            {
                throw new Exception("error on Upload method, message: " + ex.Message);
            }
        }

        public static string Upload(FtpConnection connection, string fileName, Byte[] fileData, string path = null)
        {
            try
            {
                var fullFileName = GetCheckedFileName(connection, fileName, path);

                path = $"{(string.IsNullOrEmpty(path) ? string.Empty : path)}/{fullFileName}";

                var request = connection.GenerateRequest(WebRequestMethods.Ftp.UploadFile, path);
                request.KeepAlive = false;
                request.ContentLength = fileData.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(fileData, 0, fileData.Length);
                }

                return fullFileName;
            }
            catch (Exception ex)
            {
                throw new Exception("error on Upload method, message: " + ex.Message);
            }
        }
        #endregion

        #region DOWNLOAD METHODS
        public static string Download(FtpConnection connection, string sourceFile, string destPath, string sourcePath = null)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    Download(connection, sourceFile, ms, sourcePath);

                    return FileHelper.SaveToDisk(FileHelper.SaveToMemory(ms), destPath, sourceFile, false);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on Download method, message: " + ex.Message);
            }
        }

        public static void Download(FtpConnection connection, string fileName, Stream stream, string path = null)
        {
            path = $"{(string.IsNullOrEmpty(path) ? string.Empty : path)}/{fileName}";

            var request = connection.GenerateRequest(WebRequestMethods.Ftp.DownloadFile, path);
            using (var response = request.GetResponse())
            {
                using (var ftpStream = response.GetResponseStream())
                {
                    ftpStream?.CopyTo(stream);
                }
            }
        }

        #endregion

        #region DELETE METHODS

        public static void Delete(FtpConnection connection, string fileName, string path = null)
        {
            try
            {
                var files = GetFiles(connection, path);

                if (!files.Any(f => f.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase))) return;
                path = $"{(string.IsNullOrEmpty(path) ? string.Empty : path)}/{fileName}";

                connection.GenerateRequest(WebRequestMethods.Ftp.DeleteFile, path).GetResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("error on DeleteFile method, message: " + ex.Message);
            }
        }

        #endregion

        #region RENAME METHODS

        public static void Rename(FtpConnection connection, string sourceFile, string destFile, string path = null)
        {
            try
            {
                var files = GetFiles(connection, path);

                if (!files.Any(f => f.Name.Equals(sourceFile, StringComparison.OrdinalIgnoreCase))) return;
                path = $"{(string.IsNullOrEmpty(path) ? string.Empty : path)}/{sourceFile}";

                var request = connection.GenerateRequest(WebRequestMethods.Ftp.Rename, path);
                request.RenameTo = destFile;
                request.GetResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("error on DeleteFile method, message: " + ex.Message);
            }
        }
        #endregion
    }
}