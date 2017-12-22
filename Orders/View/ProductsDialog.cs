using System;

namespace Orders
{
	public partial class ProductsDialog : Gtk.Dialog
	{
		public ProductNode Selected { get; private set; }
		public int Amount { get; private set; }
		public ProductsDialog ()
		{
			this.Build ();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			OnClose ();
			Selected = productswidget2.Selected;
			Amount = 1;
		}

		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			Selected = null;
			OnClose ();
		}
	}
}

