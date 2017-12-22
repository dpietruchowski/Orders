using System;
using System.Data;
using Npgsql;

namespace Orders
{
	//Table must have column "id"
	public class ObjectTable<T> : Table<T>
	{
		private readonly string newid_function;
		public ObjectTable (string name, int n_columns, string _newid_function): base(name, n_columns)
		{
			newid_function = _newid_function;
		}

		public int NewId()
		{
			int id;
			using (var Connection = new NpgsqlConnection(Database.ConnectionStr)) {
				var command = new NpgsqlCommand ("SELECT " + newid_function, Connection);
				Connection.Open ();
				//TODO add exception
				id = (int)command.ExecuteScalar();
			}
			return id;
		}

		public bool Exists(int id) {
			var builder = new CommandBuilder ("WHERE id = {}");
			builder.Insert (0, id.ToString ());
			return base.Exists (builder.GetCommand());
		}
	}
}

