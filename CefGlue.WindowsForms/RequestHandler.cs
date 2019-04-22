using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Windows.Forms;

//  add by kelin 2019/04/19
namespace Xilium.CefGlue.WindowsForms
{
    internal class RequestHandler : CefRequestHandler
    {
        protected override CefResourceHandler GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
        {
            Uri url = new Uri(request.Url);

            if (url.Host == "localapp")
                return new ResourceHandler(browser, frame, request);
            else if (url.Host == "localfile")
                return new FileResourceHandler(browser, frame, request);

            return base.GetResourceHandler(browser, frame, request);
        }
    }

    internal class ResourceHandler : CefResourceHandler
    {
        private CefBrowser browser;
        private CefFrame frame;
        private CefRequest request;

        private Stream stream;

        public ResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
        {
            this.browser = browser;
            this.frame = frame;
            this.request = request;
        }

        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            try
            {
                Uri url = new Uri(request.Url);

                string resourceName = url.LocalPath.Substring(1).Replace('/', '.');

                resourceName = HttpUtility.UrlDecode(resourceName);

                Assembly assm = Assembly.GetEntryAssembly();

                stream = assm.GetManifestResourceStream(resourceName);

                if (stream == null)
                    throw new Exception("找不到 名为 \"" + resourceName + "\" 的资源 。");

                callback.Continue();
                return true;

            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Dispose();

                MessageBox.Show(ex.ToString());

                return false;
            }
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            try
            {
                byte[] b = new byte[bytesToRead];

                bytesRead = stream.Read(b, 0, bytesToRead);

                if (bytesRead == 0)
                {
                    stream.Dispose();
                    return false;
                }

                response.Write(b, 0, bytesRead);

                return true;
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Dispose();

                MessageBox.Show(ex.ToString());

                bytesRead = 0;
                return false;
            }
        }

        protected override bool CanGetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override bool CanSetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = -1;
            redirectUrl = null;
        }

        protected override void Cancel()
        {
            if (stream != null)
                stream.Dispose();
        }
    }

    internal class FileResourceHandler : CefResourceHandler
    {
        private CefBrowser browser;
        private CefFrame frame;
        private CefRequest request;

        private Stream stream;

        public FileResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
        {
            this.browser = browser;
            this.frame = frame;
            this.request = request;
        }

        protected override bool ProcessRequest(CefRequest request, CefCallback callback)
        {
            try
            {
                Uri url = new Uri(request.Url);

                string u = url.AbsoluteUri.Substring(17);

                int i = u.IndexOf('/');

                string file = u.Substring(0, i) + ":" + u.Substring(i);

                file = HttpUtility.UrlDecode(file);

                stream = File.Open(file, FileMode.Open);

                callback.Continue();
                return true;
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Dispose();

                MessageBox.Show(ex.ToString());

                return false;
            }
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            try
            {
                byte[] b = new byte[bytesToRead];

                bytesRead = stream.Read(b, 0, bytesToRead);

                if (bytesRead == 0)
                {
                    stream.Dispose();
                    return false;
                }

                response.Write(b, 0, bytesRead);

                return true;
            }
            catch (Exception ex)
            {
                if (stream != null)
                    stream.Dispose();

                MessageBox.Show(ex.ToString());

                bytesRead = 0;
                return false;
            }
        }

        protected override bool CanGetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override bool CanSetCookie(CefCookie cookie)
        {
            return false;
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = -1;
            redirectUrl = null;
        }

        protected override void Cancel()
        {
            if (stream != null)
                stream.Dispose();
        }
    }
}
