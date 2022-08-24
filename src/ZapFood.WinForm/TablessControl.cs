using System;
using System.Windows.Forms;

namespace ZapFood.WinForm {

	class TablessControl: TabControl {

		const int TCM_ADJUSTRECT = 0x1328;

		protected override void WndProc(ref Message m) {

			// Hide the tab headers at runtime.
			if(m.Msg == TCM_ADJUSTRECT && !DesignMode) {
				m.Result = (IntPtr)1;
				return;
			}
			base.WndProc(ref m);
		}

	}

}
