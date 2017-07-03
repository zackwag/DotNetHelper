using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Helper.Extensions;

namespace Helper.Security
{
    public class UserImpersonation
    {
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        private WindowsImpersonationContext _impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(string lpszUserName, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        private UserImpersonation()
        {
            //empty inaccessible constructor for factory design
        }

        public static WindowsImpersonationContext GetContext(NetworkCredential userCredentials)
        {
            try
            {
                return GetContext(userCredentials.UserName, userCredentials.Domain, userCredentials.Password);
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetContext method, message: " + ex.Message);
            }
        }

        public static WindowsImpersonationContext GetContext(string userName, string domain, string password)
        {
            try
            {
                var ui = new UserImpersonation();

                return ui.ImpersonateValidUser(userName, domain, password) ? ui._impersonationContext : null;
            }
            catch (Exception ex)
            {
                throw new Exception("error on GetContext method, message: " + ex.Message);
            }
        }

        private bool ImpersonateValidUser(string userName, string domain, string password)
        {
            var token = IntPtr.Zero;
            var tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        var tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);

                        _impersonationContext = tempWindowsIdentity.Impersonate();

                        if (!_impersonationContext.IsNull())
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }
    }
}
