using System;
using System.Data;
using System.Collections.Generic;

namespace Orders
{
	[Gtk.TreeNode (ListOnly=true)]
	public class ChildProductNode : ProductNode, DependencyNodeInterface
	{
		static readonly public new List<string> ColumnNames;
		static ChildProductNode()
		{
			ColumnNames = new List<string> (ProductNode.ColumnNames);
			ColumnNames.Add("Amount");
			ColumnNames.Add("Number of package");
			ColumnNames.Add("Price");
		}

		public ChildProductNode (DataRow row, int amount): base(row)
		{
			Amount = amount;
		}
		int amount;

		int DependencyNodeInterface.Amount { 
			get {
				return Amount;
			}
			set {
				Amount = value;
			}
		}

		[Gtk.TreeNodeValue (Column=4)]
		public int Amount { 
			get {
				return amount;
			}
			set {
				amount = value;
				NPackage = (amount / base.PackageAmount);
				if (amount % base.PackageAmount > 0)
					NPackage += 1;

				Price = NPackage * base.PackagePrice;
				OnChanged ();
			}
		}
		[Gtk.TreeNodeValue (Column=5)]
		public int NPackage { get; private set; }
		[Gtk.TreeNodeValue (Column=6)]
		public float Price { get; private set; }
	}
}

