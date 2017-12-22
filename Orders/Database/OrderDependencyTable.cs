using System;

namespace Orders
{
	public class OrderDependencyTable : DependencyTable
	{
		public OrderDependencyTable (): base("order_product")
		{
			view_columns.Add (Dependency.PARENT_ID, new Column ("order_id", typeof(int)));
			view_columns.Add (Dependency.CHILD_ID, new Column ("product_id", typeof(int)));
			view_columns.Add (Dependency.AMOUNT, new Column ("amount", typeof(int)));
			
			updated_columns.Add (Dependency.PARENT_ID, new Column ("order_id", typeof(int)));
			updated_columns.Add (Dependency.CHILD_ID, new Column ("product_id", typeof(int)));
			updated_columns.Add (Dependency.AMOUNT, new Column ("amount", typeof(int)));
		}
	}
}

