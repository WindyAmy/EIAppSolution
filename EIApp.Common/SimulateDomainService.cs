using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EIApp.Common
{
    public class SimulateDomainService
    {
        public static bool ImpersonateValidUser(String userName, String domain, String password)
        {
            //ServiceContext.SetThreadPrincipal();
            WindowsImpersonationContext impersonationContext;

            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
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
        /// <summary>
        /// 模拟登录
        /// </summary>
        /// <returns></returns>
        //public static bool ImpersonateValidUser()
        //{
        //    return ImpersonateValidUser(GetValue("account"), GetValue("domain"), GetValue("pwd"));
        //}
        ///// <summary>   
        ///// 读取指定key的值   
        ///// </summary>   
        ///// <param name="key"></param>   
        ///// <returns></returns>   
        //public static string GetValue(string key)
        //{
        //    return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        //}

        //public static CustomReportCredentials GetReportCredentials(String account, String domain, String pwd)
        //{
        //    return new CustomReportCredentials(account, pwd, domain);
        //}
        ///// <summary>
        ///// 获取报表认证信息
        ///// </summary>
        ///// <returns></returns>
        //public static CustomReportCredentials GetReportCredentials()
        //{
        //    return GetReportCredentials(GetValue("account"), GetValue("domain"), GetValue("pwd"));
        //}
        #region win32 api extend method
        [DllImport("advapi32.dll")]
        static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern bool CloseHandle(IntPtr handle);

        const int LOGON32_LOGON_INTERACTIVE = 2;
        const int LOGON32_PROVIDER_DEFAULT = 0;
        #endregion


    }
}
