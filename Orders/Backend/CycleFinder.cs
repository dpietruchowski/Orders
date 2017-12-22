using System;
using System.Data;

namespace Orders
{
	public class CycleChecker
	{
		private int id;
		private int new_child_id;
		private bool is_cycle;
		public CycleChecker (int _id)
		{
			id = _id;
		}

		public bool IsCycle(int _new_child_id)
		{
			new_child_id = _new_child_id;
			is_cycle = false;
			ReadDeps (id);
			return is_cycle;
		}

		private void ReadDeps(int id)
		{
			if (is_cycle)
				return;
			DataTable table = Database.ProductDependencyTable.Select (Dependency.CHILD_ID, id.ToString());
			foreach (var child in table.Select()) {
				int parent_id = (int)child [0];
				if (parent_id == new_child_id) {
					is_cycle = true;
					return;
				}
				ReadDeps (parent_id);
			}
		}
	}
}

