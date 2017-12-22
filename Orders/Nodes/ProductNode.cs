using System;
using System.Data;
using System.Collections.Generic;

namespace Orders
{
	[Gtk.TreeNode (ListOnly=true)]
	public class ProductNode : Gtk.TreeNode
	{
		static readonly public List<string> ColumnNames = 
			new List<string>(new string[] {
				"Id",
				"Name",
				"Package Price",
				"Package Amount"
			});
		public ProductNode (DataRow row)
		{
			Id = (int)row[0];
			Name = (string)row[1];
			PackagePrice = (float)row[2];
			PackageAmount = (int)row[3];
		}

		[Gtk.TreeNodeValue (Column=0)]
		public int Id { get; private set; }
		[Gtk.TreeNodeValue (Column=1)]
		public string Name { get; set; }
		[Gtk.TreeNodeValue (Column=2)]
		public float PackagePrice { get; set; }
		[Gtk.TreeNodeValue (Column=3)]
		public int PackageAmount { get; set; }

		public void ConvertToDataRow(DataRow new_data_row)
		{
			new_data_row [0] = Id;
			new_data_row [1] = Name;
			new_data_row [2] = PackagePrice;
			new_data_row [3] = PackageAmount;
		}
	}
}

