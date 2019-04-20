using System;

using System.Windows.Forms;

using Xilium.CefGlue;

namespace ILBCStudio
{
    internal static class Program
    {
        [STAThread]
        private static int Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    //System.Diagnostics.Process.GetCurrentProcess().StartInfo.Arguments = "--allow-file-access-from-files";

            //    System.Diagnostics.Process process = new System.Diagnostics.Process();

                
            //    args = new string[1];
            //    args[0] = "--allow-file-access-from-files";

            //    process.StartInfo.Arguments = "--allow-file-access-from-files";

            //    process.StartInfo.FileName = "ILBCStudio.exe";

            //    process.Start();

            //    return 0;
            //}

            try
            {
                CefRuntime.Load();
            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
            catch (CefRuntimeException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 3;
            }

            //MessageBox.Show(args[0]);
            //args = new string[2];
            //args[0] = "--allow-file-access-from-files";
            //args[1] = "--disable-web-security";
            
                
            var mainArgs = new CefMainArgs(args);
            var app = new DemoApp();

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app, IntPtr.Zero);
            if (exitCode != -1)
                return exitCode;

            var settings = new CefSettings
            {
                MultiThreadedMessageLoop = true,

                LogSeverity = CefLogSeverity.Disable,
                LogFile = "CefGlue.log",

                NoSandbox = true
            };
            
            CefRuntime.Initialize(mainArgs, settings, app, IntPtr.Zero);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());

            CefRuntime.Shutdown();

            return 0;
        }
    }
}
