using System;
using System.Data;
using Gtk;

namespace Orders
{
	[Gtk.TreeNode (ListOnly=true)]
	public class GtkProductInOrderNode : GtkProductNode
	{
		public GtkProductInOrderNode (DataRow row, int amount): base(row)
		{
			Amount = amount;
		}
		int amount;

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