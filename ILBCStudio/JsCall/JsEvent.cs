﻿using System.Text;
using System.IO;

using Xilium.CefGlue;

namespace JsCall
{
    public class JsEvent
    {
        public void OpenSaveFileDialog()
        {
            CefProcessMessage m = CefProcessMessage.Create("OpenSaveFileDialog");
            
            DemoRenderProcessHandler._browser.SendProcessMessage(CefProcessId.Browser, m);
        }

        public void SaveFile(string file, string content)
        {
            byte[] b = Encoding.UTF8.GetBytes(content);

            using (FileStream stream = File.Create(file))
            {
                stream.Write(b, 0, b.Length);
            }
        }
    }
}
