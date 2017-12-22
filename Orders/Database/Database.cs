using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Orders
{
	//TODO Singleton
	public class Database
	{
		public static readonly string ConnectionStr = "Host=localhost;Username=testuser;" +
			"Password=admin1;Database=testdb";
		public static readonly ProductTable ProductTable = new ProductTable ();
		public static readonly OrderTable OrderTable = new OrderTable ();
		public static readonly ProductDependencyTable ProductDependencyTable = 
			new ProductDependencyTable ();
		public static readonly OrderDependencyTable OrderDependencyTable = 
			new OrderDependencyTable ();
		private Database()
		{}
	}
}

