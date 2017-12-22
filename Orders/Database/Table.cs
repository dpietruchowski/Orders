using System;
using System.Data;
using System.Collections.Generic;
using Npgsql;

namespace Orders
{
	public class Table<T>
	{
		public class Column
		{
			public Column(string name, Type type)
			{
				Name = name;
				Type = type;
			}
			public readonly string Name;
			public readonly Type Type;
			public readonly bool Inserted;
		}
		public readonly string Name;
		protected Dictionary<T, Column> view_columns;
		public IDictionary<T, Column> ViewColumns 
		{
			get {
				return view_columns;
			}
		}

		protected Dictionary<T, Column> updated_columns;

		public Table(string name, int n_columns)
		{
			Name = name;
			view_columns = new Dictionary<T, Column> (n_columns);
			updated_columns = new Dictionary<T, Column> (n_columns);
		}

		public string SelectCommand()
		{
			var builder = new CommandBuilder ("SELECT {} FROM {}");
			foreach(var column_names in ViewColumns.Values) {
				builder.Insert (0, column_names.Name);
			}
			builder.Insert (1, Name);
			return builder.GetCommand ();
		}

		public bool Exists(string filter) {
			var builder = new CommandBuilder ("SELECT count(1) FROM {}");
			builder.Insert (0, Name);
			bool exists = false;
			using (var Connection = new NpgsqlConnection(Database.ConnectionStr)) {
				using (var command = new NpgsqlCommand (builder.GetCommand(), Connection)) {
					Connection.Open ();
					exists = (int)command.ExecuteScalar () == 1;
				}
			}
			return exists;
		}

		private DataTable Select(string filter)
		{
			var select_command = SelectCommand ();
			select_command += " ";
			select_command += filter; 
			DataTable table = new DataTable ();
			using (var Connection = new NpgsqlConnection(Database.ConnectionStr)) {
				using (var command = new NpgsqlCommand(select_command, Connection))
				using (var data = new NpgsqlDataAdapter (command))
					data.Fill (table);
			}
			return table;
		}

		public DataTable Select(T column, string value) 
		{
			string filter;
			if (value == "" || column == null) {
				return Select(null);
			} else if (ViewColumns [column].Type == typeof(string)) {
				filter = "WHERE {} LIKE '%{}%'";
			} else {
				filter = "WHERE {} = {}";
			}
			string column_name = ViewColumns [column].Name;
			var builder = new CommandBuilder (filter);
			builder.Insert(0, column_name);
			builder.Insert(1, value);

			return Select(builder.GetCommand ());
		}

		public DataRow SelectRow(T column, string value) 
		{
			var table = Select (column, value);
			if (table.Rows.Count == 0)
				return null;
			return table.Rows [0];
		}

		public void Update(DataTable table)
		{
			if (table.Rows.Count == 0)
				return;
			
			var builder = new CommandBuilder ("SELECT {} FROM {}");
			foreach(var column_names in updated_columns.Values) {
				builder.Insert (0, column_names.Name);
			}
			builder.Insert (1, Name);

			using (var Connection = new NpgsqlConnection(Database.ConnectionStr)) {
				using (var adapter = new NpgsqlDataAdapter (builder.GetCommand (), Connection))
				using (var cmd_builder = new NpgsqlCommandBuilder (adapter)) {
					adapter.Update (table);
				}
			}
		}

		public void Update(DataRow row)
		{
			var table = CreateTable ();
			table.Rows.Add (row);
			Update (table);
		}

		public DataTable CreateTable()
		{
			var table = new DataTable ();
			foreach (var column in updated_columns.Values) {
				table.Columns.Add (column.Name, column.Type);
			}
			return table;
		}
	}
}

