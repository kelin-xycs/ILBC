using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace ILBCStudio.JsCall
{
    class DemoRenderProcessHandler : CefRenderProcessHandler
    {

        public CefV8Handler Cef;

        //  add by kelin 2019/03/26
        internal static CefBrowser _browser;

        //  add by kelin 2019/03/26
        protected override void OnBrowserCreated(CefBrowser browser)
        {
            base.OnBrowserCreated(browser);

            _browser = browser;
        }

        /// <summary>
        /// 通过反射机制 注册c#函数到JS
        /// </summary>
        public void RegisterJs()
        {
            JsEvent js = new JsEvent();

            Cef = new CefJsV8Handler(js);

            string javascriptCode = CefJavaScriptEx.CreateJsCodeByObject(js, "Cef");

            CefRuntime.RegisterExtension("Cef", javascriptCode, Cef);
        }

        protected override void OnWebKitInitialized()
        {
            // 注册JS函数
            RegisterJs();
        }
    }
}
