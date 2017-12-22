using System;
using System.Data;
using System.Collections.Generic;

namespace Orders
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ProductsWidget : Gtk.Bin
	{
		public ProductsWidget ()
		{
			this.Build ();
			int i = 0;
			foreach(var column in ProductNode.ColumnNames) {
				productsview.AppendColumn (column, new Gtk.CellRendererText (), "text", i);
				columnbox.InsertText (i, column);
				++i;
			}
			productsview.NodeStore = new Gtk.NodeStore (typeof(ProductNode));
		}

		protected void OnSearchbuttonClicked (object sender, EventArgs e)
		{		
			productsview.NodeStore.Clear ();
			int column = columnbox.Active;
			string value = filterentry.Text;
			var table = Database.ProductTable.Select ((Product)column, value);
			foreach (var row in table.Select()) {
				productsview.NodeStore.AddNode (new ProductNode(row));
			}
		}

		public ProductNode Selected 
		{ 
			get 
			{
				return (ProductNode) productsview.NodeSelection.SelectedNode;
			}
		}
	}
}

