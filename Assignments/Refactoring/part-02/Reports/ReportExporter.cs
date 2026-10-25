using System;
using System.Collections.Generic;
using System.Text;

namespace RefactoringLab.Part02.Reports
{
	//Refactoring Class
	public abstract class ReportExporter
	{
		public void Export(string path)
		{
			var rows = Load();
			if (!Validate(rows))
				throw new InvalidOperationException("Invalid data");
			var content = Format(rows);
			Save(path, content);
		}
		private bool Validate(List<string[]> rows) =>
	        rows.Count > 1 && rows[0].Length > 0;

		private void Save(string path, string content) =>
		    File.WriteAllText(path, content);

		protected abstract string Format(List<string[]> rows);

		private List<string[]> Load() =>
		[
			["Id", "Name"],
			["1", "Keyboard"],
			["2", "Mouse"]
		];
	}
}
