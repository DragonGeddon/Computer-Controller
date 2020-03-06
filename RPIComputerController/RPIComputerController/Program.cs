using System;
using System.Text;
using System.Threading;
using Microsoft.Management.Infrastructure;
using Microsoft.Management.Infrastructure.Options;
using System.Security;
using System.Diagnostics;
using System.IO;

namespace RPIComputerController
{
    class Program
    {
        static void Main(string[] args)
        {
            string computer = "DESKTOP-AE7NQIH";
            string domain = "WORKGROUP";
            string username = "AdminUserName";
            string plaintextpassword = "ATempPassword";

            SecureString securepassword = new SecureString();
            foreach (char c in plaintextpassword)
            {
                securepassword.AppendChar(c);
            }

            CimCredential Credentials = new CimCredential(PasswordAuthenticationMechanism.Default,
                                                          domain,
                                                          username,
                                                          securepassword);

            WSManSessionOptions SessionOptions = new WSManSessionOptions();
            SessionOptions.AddDestinationCredentials(Credentials);

            CimSession Session = CimSession.Create(computer, SessionOptions);


        }

        private static void startApp(String filename)
        {

            ProcessStartInfo psi = new ProcessStartInfo();

            psi.FileName = filename;

            psi.Arguments = Path.Combine(Path.GetPathRoot(Environment.SystemDirectory), "boot.ini");

            Process.Start(psi);
        }

        private static void startWebPage(String url)
        {
            Process.Start(url);
        }
    }
}