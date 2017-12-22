using System;

namespace Orders
{
	public partial class OrdersDialog : Gtk.Dialog
	{
		public OrderNode Selected { get; private set; }
		public OrdersDialog ()
		{
			this.Build ();
		}

		public void ShowAndRefresh()
		{
			Show ();
			orderswidget1.Refresh ();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			OnClose ();
			Selected = orderswidget1.Selected;
		}

		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			Selected = null;
			OnClose ();
		}
	}
}

