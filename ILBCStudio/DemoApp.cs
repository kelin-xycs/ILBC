using Xilium.CefGlue;

using JsCall;

namespace ILBCStudio
{
    internal sealed class DemoApp : CefApp
    {
        private CefRenderProcessHandler _renderProcessHandler = new DemoRenderProcessHandler();

        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return _renderProcessHandler;
        }
    }
}
