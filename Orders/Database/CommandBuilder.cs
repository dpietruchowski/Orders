using System;

namespace Orders
{
	public class CommandBuilder
	{
		private string command;
		string args_str = "{}";
		string separator = ", ";
		public CommandBuilder(string _command)
		{
			command = _command;
		}

		public void Insert (int position, string value)
		{
			var index = command.IndexOf (args_str);
			for (int i = 0; i < position; ++i) {
				index = command.IndexOf (args_str, index + 1);
			}
			command = command.Insert (index, value + ", ");
		}
		public string GetCommand()
		{
			string _command = command.Replace (separator + args_str, "");
			_command = _command.Replace (args_str, "");
			return _command;
		}
	}
}

