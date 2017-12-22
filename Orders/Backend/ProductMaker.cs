using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Orders
{
	public class ProductMaker
	{
		public int Id;
		public ProductNode Node;
		private DependencyNodeStoreManager<ProductInOrderNode> product_manager;
		private DataTable table;
		private DataTable dependency_table;
		private bool saved;

		public ProductMaker(int id, Gtk.NodeStore node_store)
		{
			Id = id;
			product_manager = new DependencyNodeStoreManager<ProductInOrderNode> (node_store);
		}

		~ProductMaker()
		{
			if (!saved) {
				table.Clear ();
				dependency_table.Clear ();
				SaveToDatabase ();
			}
		}

		private string Filter(int child_id) 
		{
			CommandBuilder builder = new CommandBuilder ("{} = {} AND {} = {}");
			//TODO Change this. too long line;
			builder.Insert (0, Database.ProductDependencyTable.ViewColumns [Dependency.PARENT_ID].Name);
			builder.Insert (1, Id.ToString());
			builder.Insert (2, Database.ProductDependencyTable.ViewColumns [Dependency.CHILD_ID].Name);
			builder.Insert (3, child_id.ToString());
			return builder.GetCommand ();
		}

		public void AddProduct (int id, int amount)
		{
			var cycle_checker = new CycleChecker (Id);
			if (cycle_checker.IsCycle (id))
				return;
			//TODO DialogInfo about it
			//TODO Check if it is not infinite loop
			if (Id != id) {
				product_manager.Add (id, amount);
			}
		}

		public void DeleteProduct(int id, int amount)
		{
			if (product_manager.Delete (id, amount)) {
				var rows = dependency_table.Select (Filter(id));
				if (rows.Length > 0)
					rows [0].Delete ();
			}
		}

		public void Clear()
		{
			foreach (ProductInOrderNode node in product_manager.NodeStore) {
				DeleteProduct (node.Id, node.Amount);
			}
		}

		public bool LoadFromDatabase ()
		{
			table = Database.ProductTable.Select (Product.ID, Id.ToString ());
			dependency_table = 
				Database.ProductDependencyTable.Select (Dependency.PARENT_ID, 
				                                        Id.ToString());
			if (table.Rows.Count == 0) {
				NewDatabaseTables ();
				return true;
			}
			Node = new ProductNode (table.Rows[0]);
			//TODO Dep table could be null or empty()?
			foreach (var child in dependency_table.Select()) {
				int childid = (int)child [1];
				int amount = (int)child [2];
				AddProduct (childid, amount);
			}
			return true;
		}

		public void NewDatabaseTables()
		{
			dependency_table = Database.ProductDependencyTable.CreateTable ();
			table = Database.ProductTable.CreateTable();
			var row = table.NewRow();
			row [0] = (int)Id;
			row [1] = "Change name";
			row [2] = 0.0;
			row [3] = 1;
			Node = new ProductNode (row);
			table.Rows.Add (row);
		}

		public void SaveToDatabase () 
		{
			if (Id == -1) {
				Id = Database.OrderTable.NewId ();
			}
			foreach (var child in product_manager.GetDependecyTable(Id)) {
				var rows = dependency_table.Select (Filter(child.ChildId));
				DataRow row = null;
				if (rows.Length == 0) {
					row = dependency_table.NewRow ();
					dependency_table.Rows.Add (row);
					row [0] = child.ParentId;
					row [1] = child.ChildId;
				} else {
					row = rows [0];
				}
				row [2] = child.Amount;
			}
			Database.ProductDependencyTable.Update (dependency_table);

			var prow = table.Rows [0];
			Node.ConvertToDataRow (prow);
			Database.ProductTable.Update (table);
		}
	}
}

