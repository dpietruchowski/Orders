using System;
using System.Collections.Generic;

namespace Orders
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ProductWidget : Gtk.Bin
	{
		ProductMaker product;
		public ProductsDialog Products { get; set; }
		public ProductWidget ()
		{
			this.Build ();
			productview.NodeStore = new Gtk.NodeStore (typeof(ProductNode));
			List<Gtk.EditedHandler> callbacks = new List<Gtk.EditedHandler>();
			callbacks.Add (this.NameEditCallback);
			callbacks.Add (this.PriceEditCallback);
			callbacks.Add (this.AmountEditCallback);
			productview.AppendColumn ("Id", new Gtk.CellRendererText(), "text", 0);
			for (int k = 1; k < ProductNode.ColumnNames.Count; ++k) {
				Gtk.CellRendererText editableCell = new Gtk.CellRendererText();
				editableCell.Editable = true;
				editableCell.Edited += callbacks[k-1];
				productview.AppendColumn (ProductNode.ColumnNames[k], editableCell, "text", k);
			}
			productview.Columns [0].MaxWidth = 0;

			dependencyview.NodeStore = new Gtk.NodeStore (typeof(ProductInOrderNode));
			int i = 0;
			foreach (var column in ProductInOrderNode.ColumnNames) {
				dependencyview.AppendColumn (column, new Gtk.CellRendererText (), "text", i);
				++i;
			}
		}

		public void Init(int id) 
		{
			product = new ProductMaker (id, dependencyview.NodeStore);
			product.LoadFromDatabase ();
			productview.NodeStore.AddNode (product.Node);
		}


		void IdEditCallback(object o, Gtk.EditedArgs args) {
		}

		void NameEditCallback(object o, Gtk.EditedArgs args) {
			var node = productview.NodeStore.GetNode(new Gtk.TreePath (args.Path)) as ProductNode;
			node.Name = args.NewText;
		}
		void PriceEditCallback(object o, Gtk.EditedArgs args) {
			var node = productview.NodeStore.GetNode(new Gtk.TreePath (args.Path)) as ProductNode;
			node.PackagePrice = float.Parse(args.NewText);
		}
		void AmountEditCallback(object o, Gtk.EditedArgs args) {
			var node = productview.NodeStore.GetNode(new Gtk.TreePath (args.Path)) as ProductNode;
			node.PackageAmount = int.Parse(args.NewText);
		}

		protected void OnAddbuttonClicked (object sender, EventArgs e)
		{
			Products.Show();

			Products.Run ();
			var selected = Products.Selected;
			if (selected != null)
				product.AddProduct (selected.Id, Products.Amount);
			Products.Hide ();
		}

		protected void OnDeletebuttonClicked (object sender, EventArgs e)
		{
			var selected = (ProductInOrderNode)dependencyview.NodeSelection.SelectedNode;
			if (selected != null) {
				product.DeleteProduct (selected.Id, 1);
			}
		}

		protected void OnClearbuttonClicked (object sender, EventArgs e)
		{
			throw new NotImplementedException ();
		}

		protected void OnSavebuttonClicked (object sender, EventArgs e)
		{
			product.SaveToDatabase ();
		}
	}
}

