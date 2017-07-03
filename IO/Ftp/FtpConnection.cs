using Helper.Extensions;
using System;
using System.Net;

namespace Helper.IO.Ftp
{
    public class FtpConnection
    {
        private string _userName;
        private string _password;

        public FtpConnection()
        { }

        public FtpConnection(string host, string userName, string password, int port = 21)
        {
            Host = host;
            _userName = userName;
            _password = password;
            Port = port;
        }

        public FtpConnection(string host, NetworkCredential credentials, int port = 21)
        {
            Host = host;
            _userName = credentials.UserName;
            _password = credentials.Password;
            Port = port;
        }

        public string Host { get; set; }

        public NetworkCredential Credentials
        {
            get
            {
                return new NetworkCredential(_userName, _password);
            }
            set
            {
                _userName = value.UserName;
                _password = value.Password;
            }
        }

        public int Port { get; set; }

        public string CurrentDirectory { get; set; } = string.Empty;

        public string CurrentPath
        {
            get
            {
                var currPath = "/";

                if (!string.IsNullOrEmpty(CurrentDirectory))
                    currPath = $"{(CurrentDirectory.StartsWith("/") ? string.Empty : "/")}{CurrentDirectory}";

                return currPath;
            }
        }

        public Uri Uri => new Uri($"ftp://{Host}{(Port.IsDefault() || Port == 21 ? string.Empty : ":" + Port)}{CurrentPath}");

        public void SetCurrentDirectory(string directory)
        {
            CurrentDirectory = string.IsNullOrEmpty(directory) || directory == "/" ? string.Empty : directory.StartsWith("/") ? directory : string.Format("/{0}", directory);
        }

        public FtpWebRequest GenerateRequest(string method, string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                    SetCurrentDirectory(path);

                var request = (FtpWebRequest)WebRequest.Create(Uri);
                request.UseBinary = true;
                request.Credentials = Credentials;
                request.Method = method;

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception("error on GenerateRequest method, message: " + ex.Message);
            }
        }
    }
}
