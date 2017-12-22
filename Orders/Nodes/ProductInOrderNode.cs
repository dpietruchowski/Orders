using System;
using System.Data;
using System.Collections.Generic;

namespace Orders
{
	public class ProductInOrderNode : Gtk.TreeNode, DependencyNodeInterface
	{
		static readonly public List<string> ColumnNames = 
			new List<string>(new string[] {
				"Id",
				"Name",
				"Amount"
			});

		public ProductInOrderNode (DataRow row, int _amount)
		{
			Id = (int)row[0];
			Name = (string)row[1];
			Amount = _amount;
		}

		int DependencyNodeInterface.Amount { 
			get {
				return Amount;
			}
			set {
				Amount = value;
			}
		}

		[Gtk.TreeNodeValue (Column=0)]
		public int Id { get; private set; }
		[Gtk.TreeNodeValue (Column=1)]
		public string Name { get; private set; }
		[Gtk.TreeNodeValue (Column=2)]
		public int Amount { 
			get {
				return amount;
			}
			set {
				amount = value;
				OnChanged ();
			}
		}
		private int amount;
	}
}

