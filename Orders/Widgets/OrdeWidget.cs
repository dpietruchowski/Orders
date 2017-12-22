using System;

namespace Orders
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class OrdeWidget : Gtk.Bin
	{
		OrderMaker order;
		public ProductsDialog Products { get; set; }
		public OrdeWidget ()
		{
			this.Build ();
			orderview.NodeStore = new Gtk.NodeStore (typeof(ProductInOrderNode));
			int i = 0;
			foreach (var column in ProductInOrderNode.ColumnNames) {
				orderview.AppendColumn (column, new Gtk.CellRendererText (), "text", i);
				++i;
			}
			fullorderview.NodeStore = new Gtk.NodeStore (typeof(ChildProductNode));
			i = 0;
			foreach (var column in ChildProductNode.ColumnNames) {
				fullorderview.AppendColumn (column, new Gtk.CellRendererText (), "text", i);
				++i;
			}
		}

		public void Init(int id)
		{
			order = new OrderMaker (id, fullorderview.NodeStore, orderview.NodeStore);
			order.LoadFromDatabase ();
			pricelabel.Text = order.CountPrice ().ToString();
		}
		protected void OnAddbuttonClicked (object sender, EventArgs e)
		{
			Products.Show();

			Products.Run ();
			var selected = Products.Selected;
			if (selected != null)
				order.AddProduct (selected.Id, Products.Amount);
			Products.Hide ();
			pricelabel.Text = order.CountPrice ().ToString();
		}

		protected void OnDeletebuttonClicked (object sender, EventArgs e)
		{
			var selected = (ProductInOrderNode)orderview.NodeSelection.SelectedNode;
			if (selected != null) {
				order.DeleteProduct (selected.Id, 1);
				pricelabel.Text = order.CountPrice ().ToString();
			}
		}

		protected void OnClearbuttonClicked (object sender, EventArgs e)
		{
			pricelabel.Text = order.CountPrice ().ToString();
		}

		protected void OnSavebuttonClicked (object sender, EventArgs e)
		{
			order.SaveToDatabase ();
		}
	}
}

