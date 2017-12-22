using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Orders
{
	public class OrderMaker
	{
		public int Id { get; private set; }
		private DependencyNodeStoreManager<ChildProductNode> full_order_manager;
		public DependencyNodeStoreManager<ProductInOrderNode> OrderManager;
		private DataTable table;
		private DataTable order_product_table;

		public OrderMaker(int id, Gtk.NodeStore full_node_store,
		                  Gtk.NodeStore node_store)
		{
			Id = id;
			full_order_manager = new DependencyNodeStoreManager<ChildProductNode> (full_node_store);
			OrderManager = new DependencyNodeStoreManager<ProductInOrderNode> (node_store);
		}

		private string Filter(int child_id) 
		{
			CommandBuilder builder = new CommandBuilder ("{} = {} AND {} = {}");
				//TODO Change this. too long line;
			builder.Insert (0, Database.OrderDependencyTable.ViewColumns [Dependency.PARENT_ID].Name);
			builder.Insert (1, Id.ToString());
			builder.Insert (2, Database.OrderDependencyTable.ViewColumns [Dependency.CHILD_ID].Name);
			builder.Insert (3, child_id.ToString());
			return builder.GetCommand ();
		}

		public void AddProduct (int id, int amount)
		{
			OrderManager.Add (id, amount);
			ReadDeps (id, amount, true);
		}

		public void DeleteProduct(int id, int amount)
		{
			if (OrderManager.Delete (id, amount)) {
				var rows = order_product_table.Select (Filter(id));
				if (rows.Length > 0)
					rows [0].Delete ();
			}
			ReadDeps (id, amount, false);
		}

		private void ReadDeps(int id, int amount, bool addToOrder)
		{
			if (addToOrder)
				full_order_manager.Add (id, amount);
			else
				full_order_manager.Delete (id, amount);
			DataTable table = Database.ProductDependencyTable.SelectDependency (id);
			foreach (var child in table.Select()) {
				ReadDeps ((int)child [1], (int)child [2] * amount, addToOrder);
			}
		}
		public List<Node> GetDependecyTable()
		{
			return OrderManager.GetDependecyTable (Id);
		}

		public float CountPrice()
		{
			float sum = 0;
			foreach (ChildProductNode node in full_order_manager.NodeStore) {
				sum += node.Price;
			}
			return sum;
		}

		public void Clear()
		{
			foreach (ProductInOrderNode node in OrderManager.NodeStore) {
				DeleteProduct (node.Id, node.Amount);
			}
		}
		
		public bool LoadFromDatabase ()
		{
			table = Database.OrderTable.Select (Order.ID, Id.ToString ());
			order_product_table = 
				Database.OrderDependencyTable.Select (Dependency.PARENT_ID, 
				                                      Id.ToString());
			if (table.Rows.Count == 0) {
				NewDatabaseTables ();
			}
			//TODO Dep table could be null or empty()?
			foreach (var child in order_product_table.Select()) {
				int childid = (int)child [1];
				int amount = (int)child [2];
				AddProduct (childid, amount);
			}
			return true;
		}

		public void NewDatabaseTables()
		{
			Id = -1;
			order_product_table = Database.OrderDependencyTable.CreateTable ();
			table = Database.OrderTable.CreateTable();
			var row = table.NewRow();
			row[0] = Id;
			row[1] = CountPrice();
			table.Rows.Add (row);
		}

		public void SaveToDatabase () 
		{
			if (Id == -1) {
				Id = Database.OrderTable.NewId ();
			}
			foreach (var child in OrderManager.GetDependecyTable(Id)) {
				var rows = order_product_table.Select (Filter(child.ChildId));
				DataRow row = null;
				if (rows.Length == 0) {
					row = order_product_table.NewRow ();
					order_product_table.Rows.Add (row);
					row [0] = child.ParentId;
					row [1] = child.ChildId;
				} else {
					row = rows [0];
				}
				row [2] = child.Amount;
			}
			Database.OrderDependencyTable.Update (order_product_table);

			var orow = table.Rows [0];
			orow[0] = Id;
			orow[1] = CountPrice();
			Database.OrderTable.Update (table);
		}
	}
}

