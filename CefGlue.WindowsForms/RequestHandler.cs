using System;

using System.IO;

using System.Reflection;


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

            //return base.GetResourceHandler(browser, frame, request);
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
            if (stream != null)
                stream.Dispose();


            Uri url = new Uri(request.Url);

            string resourceName = url.LocalPath.Substring(1).Replace('/', '.');
            //if (url.Host == "localapp")
            //stream = File.Open(file, FileMode.Open);
            Assembly assm = Assembly.GetEntryAssembly();

            Stream s = assm.GetManifestResourceStream(resourceName);

            if (s == null)
                throw new Exception("找不到 名为 \"" + resourceName + "\" 的资源 。");

            this.stream = s;
            //this.image = Image.FromStream(s);


            callback.Continue();
            return true;
            //throw new NotImplementedException();
        }

        protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
        {
            byte[] b = new byte[bytesToRead];

            bytesRead = stream.Read(b, 0, bytesToRead);


            if (bytesRead == 0)
            {
                stream.Dispose();
                return false;
                //callback.Continue();
            }

            response.Write(b, 0, bytesRead);



            return true;
            //throw new NotImplementedException();
        }

        protected override bool CanGetCookie(CefCookie cookie)
        {
            return false;
            //throw new NotImplementedException();
        }

        protected override bool CanSetCookie(CefCookie cookie)
        {
            return false;
            //throw new NotImplementedException();
        }

        protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = -1;
            redirectUrl = null;
            //throw new NotImplementedException();
        }

        protected override void Cancel()
        {
            if (stream != null)
                stream.Dispose();
            //throw new NotImplementedException();
        }
    }

    //internal class ResourceHandler : CefResourceHandler
    //{
    //    private CefBrowser browser;
    //    private CefFrame frame;
    //    private CefRequest request;

    //    private Stream stream;

    //    public ResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
    //    {
    //        this.browser = browser;
    //        this.frame = frame;
    //        this.request = request;
    //    }

    //    protected override bool ProcessRequest(CefRequest request, CefCallback callback)
    //    {
    //        if (stream != null)
    //            stream.Dispose();

    //        string file = new Uri(request.Url).LocalPath;
    //        stream = File.Open(file, FileMode.Open);

    //        callback.Continue();
    //        return true;
    //        //throw new NotImplementedException();
    //    }

    //    protected override bool ReadResponse(Stream response, int bytesToRead, out int bytesRead, CefCallback callback)
    //    {
    //        byte[] b = new byte[bytesToRead];

    //        bytesRead = stream.Read(b, 0, bytesToRead);


    //        if (bytesRead == 0)
    //        {
    //            stream.Dispose();
    //            return false;
    //            //callback.Continue();
    //        }

    //        response.Write(b, 0, bytesRead);



    //        return true;
    //        //throw new NotImplementedException();
    //    }

    //    protected override bool CanGetCookie(CefCookie cookie)
    //    {
    //        return false;
    //        //throw new NotImplementedException();
    //    }

    //    protected override bool CanSetCookie(CefCookie cookie)
    //    {
    //        return false;
    //        //throw new NotImplementedException();
    //    }

    //    protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
    //    {
    //        responseLength = -1;
    //        redirectUrl = null;
    //        //throw new NotImplementedException();
    //    }

    //    protected override void Cancel()
    //    {
    //        if (stream != null)
    //            stream.Dispose();
    //        //throw new NotImplementedException();
    //    }
    //}

}
