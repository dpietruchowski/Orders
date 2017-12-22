using System;

namespace Orders
{
	public class ProductDependencyTable : DependencyTable
	{
		public ProductDependencyTable (): base("product_dependency")
		{
			view_columns.Add (Dependency.PARENT_ID, new Column ("parent_id", typeof(int)));
			view_columns.Add (Dependency.CHILD_ID, new Column ("child_id", typeof(int)));
			view_columns.Add (Dependency.AMOUNT, new Column ("amount", typeof(int)));

			updated_columns.Add (Dependency.PARENT_ID, new Column ("parent_id", typeof(int)));
			updated_columns.Add (Dependency.CHILD_ID, new Column ("child_id", typeof(int)));
			updated_columns.Add (Dependency.AMOUNT, new Column ("amount", typeof(int)));
		}
	}
}

