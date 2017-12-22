using System;
using System.Collections.Generic;
using Gtk;

namespace Orders
{
	public class Node
	{
		public int ParentId;
		public int ChildId;
		public int Amount;
		public Node(int parent_id, int child_id, int amount)
		{
			ParentId = parent_id;
			ChildId = child_id;
			Amount = amount;
		}
	}

	public interface DependencyNodeInterface
	{
		int Id { get; }
		int Amount { get; set; }
	}

	public class DependencyNodeStoreManager<T> where T : Gtk.TreeNode, DependencyNodeInterface
	{
		private Dictionary<int, T> nodes;
		public Gtk.NodeStore NodeStore { get; private set; }

		public DependencyNodeStoreManager(Gtk.NodeStore _node_store)
		{
			NodeStore = _node_store;
			nodes = new Dictionary<int, T> ();
		}

		public void Add(int id, int amount)
		{
			if (nodes.ContainsKey (id)) {
				nodes [id].Amount += amount;
			} else {
				var table = Database.ProductTable.Select (Product.ID, id.ToString ());
				var node_view = 
					(T)Activator.CreateInstance (typeof(T), table.Rows [0], amount);
				NodeStore.AddNode(node_view);
				nodes.Add (id, node_view);
			}
		}
		public bool Delete(int id, int amount)
		{			
			if (nodes.ContainsKey (id)) {
				nodes [id].Amount -= amount;
				if (nodes [id].Amount <= 0) {
					NodeStore.RemoveNode (nodes [id]);
					nodes.Remove (id);
					return true;
				}
			}
			return false;
		}

		public List<Node> GetDependecyTable(int id)
		{
			var list = new List<Node> (nodes.Count);
			foreach (var child in nodes) {
				list.Add(new Node(id, child.Value.Id, child.Value.Amount));
			}
			return list;
		}
	}
}

