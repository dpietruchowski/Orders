using System;
using System.Collections.Generic;
using System.Data;

namespace Orders
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class OrdersWidget : Gtk.Bin
	{
		public OrdersWidget ()
		{
			this.Build ();
			int i = 0;
			foreach(var column in OrderNode.ColumnNames) {
				ordersview.AppendColumn (column, new Gtk.CellRendererText (), "text", i);
				++i;
			}
			ordersview.NodeStore = new Gtk.NodeStore (typeof(OrderNode));
		}
		public OrderNode Selected 
		{ 
			get 
			{
				return (OrderNode) ordersview.NodeSelection.SelectedNode;
			}
		}

		public void Refresh()
		{
			ordersview.NodeStore.Clear ();
			var table = Database.OrderTable.Select (Order.ID, "");
			foreach (var row in table.Select()) {
				ordersview.NodeStore.AddNode (new OrderNode(row));
			}
		}
	}
}

