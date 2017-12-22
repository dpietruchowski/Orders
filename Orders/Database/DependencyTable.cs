using System;
using System.Data;

namespace Orders
{
	public enum Dependency
	{
		PARENT_ID,
		CHILD_ID,
		AMOUNT
	}

	public class DependencyTable : Table<Dependency>
	{
		public DependencyTable (string name): base(name, 3)
		{
		}

		public DataTable SelectDependency(int parent_id)
		{
			return Select (Dependency.PARENT_ID, parent_id.ToString());
		}
	}
}

