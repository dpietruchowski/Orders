using System;
using System.Data;
using System.Collections.Generic;

namespace Orders
{
	[Gtk.TreeNode (ListOnly=true)]
	public class OrderNode : Gtk.TreeNode
	{
		static readonly public List<string> ColumnNames = 
			new List<string>(new string[] {
				"Id",
				"Price",
				"Date"
			});
		public OrderNode (DataRow row)
		{
			Id = (int)row[0];
			Price = (float)row[1];
			//Date = row[2].ToString();
		}

		[Gtk.TreeNodeValue (Column=0)]
		public int Id { get; private set; }
		[Gtk.TreeNodeValue (Column=1)]
		public float Price { get; private set; }
		[Gtk.TreeNodeValue (Column=2)]
		public string Date { get; private set; }

		public void ConvertToDataRow(DataRow new_data_row)
		{
			new_data_row [0] = Id;
			new_data_row [1] = Price;
			//new_data_row [2] = Date;
		}
	}

}

