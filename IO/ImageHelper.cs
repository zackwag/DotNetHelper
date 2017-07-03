using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace Helper.IO
{
    public static class ImageHelper
    {
        public static bool IsImageFile(string fileExt)
        {
            return FileExtensionGroup.Images.Contains(fileExt.ToLower());
        }

        public static Size GetImageSize(string filePath)
        {
            return GetImageSize(FileHelper.SaveToMemory(filePath));
        }

        public static Size GetImageSize(Uri requestUri, NetworkCredential credentials = null)
        {
            try
            {
                return GetImageSize(FileHelper.SaveToMemory(requestUri, credentials));
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetImageSize method, message: " + ex.Message);
            }
        }

        public static Size GetImageSize(byte[] imageData)
        {
            try
            {
                using (var stream = new MemoryStream(imageData))
                {
                    return GetImageSize(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetImageSize method, message: " + ex.Message);
            }
        }

        public static Size GetImageSize(Stream stream)
        {
            try
            {
                using (var image = Image.FromStream(stream, false, false))
                {
                    return image.Size;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetImageSize method, message: " + ex.Message);
            }
        }

        public static byte[] CropImage(byte[] imageData, int width, int height, int x1, int y1, int x2, int y2, ImageFormat format)
        {
            try
            {
                var x = Math.Min(x1, x2);
                var y = Math.Min(y1, y2);

                using (var readStream = new MemoryStream())
                {
                    readStream.Write(imageData, 0, imageData.Length);

                    using (var oldImg = Image.FromStream(readStream))
                    {
                        using (var newImage = new Bitmap(width, height, PixelFormat.Format24bppRgb))
                        {
                            newImage.SetResolution(oldImg.HorizontalResolution, oldImg.VerticalResolution);

                            using (var g = Graphics.FromImage(newImage))
                            {
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.SmoothingMode = SmoothingMode.HighQuality;
                                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                g.CompositingQuality = CompositingQuality.HighQuality;
                                g.DrawImage(oldImg, 0, 0, width, height);
                                g.DrawImage(oldImg, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);

                                using (var saveStream = new MemoryStream())
                                {
                                    newImage.Save(saveStream, ImageFormat.Png);
                                    return saveStream.GetBuffer();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on CropImage method, message: " + ex.Message);
            }
        }

        public static byte[] ResizeImage(byte[] imageData, Size newSize, ImageFormat format)
        {
            try
            {
                using (var readStream = new MemoryStream(imageData))
                {
                    readStream.Write(imageData, 0, imageData.Length);

                    using (var oldImage = Image.FromStream(readStream))
                    {
                        newSize = GetResizedSize(oldImage.Size, newSize);

                        using (var newImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format24bppRgb))
                        {
                            using (var g = Graphics.FromImage(newImage))
                            {
                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                g.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize));

                                using (var saveStream = new MemoryStream())
                                {
                                    newImage.Save(saveStream, format);
                                    return saveStream.GetBuffer();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("error on ResizeImage method, message: " + ex.Message);
            }
        }

        public static Size GetResizedSize(Size currentSize, Size maxSize)
        {
            try
            {
                var aspectRatio = currentSize.Width / (double)currentSize.Height;
                var boxRatio = maxSize.Width / (double)maxSize.Height;
                double scaleFactor;

                if (boxRatio > aspectRatio) //Use height, since that is the most restrictive dimension of box.
                    scaleFactor = maxSize.Height / (double)currentSize.Height;
                else
                    scaleFactor = maxSize.Width / (double)currentSize.Width;

                return new Size(Convert.ToInt32(currentSize.Width * scaleFactor), Convert.ToInt32(currentSize.Height * scaleFactor));
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetResizedSize method, message: " + ex.Message);
            }
        }
    }
}
