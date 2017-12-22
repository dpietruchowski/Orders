using System;

namespace Orders
{
	public enum Order
	{
		ID,
		PRICE,
		DATE
	}

	public class OrderTable : ObjectTable<Order>
	{
		public OrderTable (): base("orders", 3, "order_newid()")
		{
			view_columns.Add (Order.ID, new Column ("id", typeof(int)));
			view_columns.Add (Order.PRICE, new Column ("price", typeof(float)));
			view_columns.Add (Order.DATE, new Column ("date", typeof(string)));

			updated_columns.Add (Order.ID, new Column ("id", typeof(int)));
			updated_columns.Add (Order.PRICE, new Column ("price", typeof(float)));
		}
	}
}

