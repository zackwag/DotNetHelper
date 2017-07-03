using System.Net.Mime;

namespace Helper.IO
{
    public static class CommonContentTypes
    {
        public static ContentType Bmp => GetType("image/bmp");

        public static ContentType Doc => GetType("application/msword");

        public static ContentType Docx => GetType("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

        public static ContentType Exe => GetType("application/octet-stream");

        public static ContentType GenericDownload => GetType("application/download");

        public static ContentType Gif => GetType("image/gif");

        public static ContentType Ics => GetType("text/calendar");

        public static ContentType Html => GetType("text/html");

        public static ContentType Javascript => GetType("application/x-javascript");

        public static ContentType Jpg => GetType("image/jpeg");

        public static ContentType Json => GetType("application/json");

        public static ContentType Msg => GetType("application/vnd.ms-outlook");

        public static ContentType Pdf => GetType("application/pdf");

        public static ContentType Ppt => GetType("application/vnd.ms-powerpoint");

        public static ContentType Pptx  => GetType("application/vnd.openxmlformats-officedocument.presentationml.presentation");

        public static ContentType Mdb => GetType("application/vnd.ms-access");

        public static ContentType Mp3 => GetType("audio/mpeg");

        public static ContentType Mp4Audio => GetType("audio/mp4");

        public static ContentType Mp4Video => GetType("video/mp4");

        public static ContentType Mpeg => GetType("video/mpeg");

        public static ContentType Png => GetType("image/png");

        public static ContentType Rtf => GetType("application/rtf");

        public static ContentType Tiff => GetType("image/tiff");

        public static ContentType Text => GetType("text/plain");

        public static ContentType VCalendar => GetType("text/x-vcalendar");

        public static ContentType VCard => GetType("text/x-vcard");

        public static ContentType Wav => GetType("audio/x-wav");

        public static ContentType Xhtml => GetType("application/xhtml+xml");

        public static ContentType Xls => GetType("application/vnd.ms-excel");

        public static ContentType Xlsx => GetType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        public static ContentType Xml => GetType("application/xml");

        public static ContentType Zip => GetType("application/zip");

        private static ContentType GetType(string contentType)
        {
            return new ContentType(contentType);
        }
    }
}
