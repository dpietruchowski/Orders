using System;

namespace Orders
{
	public enum Product
	{
		ID,
		NAME,
		PACKAGE_PRICE,
		PACKAGE_AMOUNT
	}

	public class ProductTable : ObjectTable<Product>
	{
		public ProductTable (): base("products", 4, "product_newid()")
		{
			view_columns.Add (Product.ID, new Column ("id", typeof(int)));
			view_columns.Add (Product.NAME, new Column ("name", typeof(string)));
			view_columns.Add (Product.PACKAGE_PRICE, new Column ("package_price", typeof(float)));
			view_columns.Add (Product.PACKAGE_AMOUNT, new Column ("package_amount", typeof(int)));
			
			updated_columns.Add (Product.ID, new Column ("id", typeof(int)));
			updated_columns.Add (Product.NAME, new Column ("name", typeof(string)));
			updated_columns.Add (Product.PACKAGE_PRICE, new Column ("package_price", typeof(float)));
			updated_columns.Add (Product.PACKAGE_AMOUNT, new Column ("package_amount", typeof(int)));
		}
	}
}

