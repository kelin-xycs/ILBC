namespace Xilium.CefGlue.WindowsForms
{
    using System;

    internal sealed class CefWebLifeSpanHandler : CefLifeSpanHandler
    {
        private readonly CefWebBrowser _core;

        public CefWebLifeSpanHandler(CefWebBrowser core)
        {
            _core = core;
        }

        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);

            _core.OnBrowserAfterCreated(browser);
        }

        protected override bool DoClose(CefBrowser browser)
        {
            // TODO: ... dispose core
            return false;
        }

		protected override void OnBeforeClose(CefBrowser browser)
		{
			if (_core.InvokeRequired)
				_core.BeginInvoke((Action)_core.OnBeforeClose);
			else
				_core.OnBeforeClose();
		}
    }
}
