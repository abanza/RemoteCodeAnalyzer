// DetectClassDependency.cs                                                       

using System.Collections.Generic;
using System.Linq;
using Parser.Parser;

namespace Parser.Rules
{
	public class DetectClassDependency : ARule
	{
		private readonly List<Elem> _classes;
		private readonly Repository _repo;

		public DetectClassDependency(Repository repo, List<Elem> classes)
		{
			_classes = classes;
			_repo = repo;
		}

		public override bool test(CSemiExp semi)
		{
			var typeList = new List<string> { "class", "interface", "struct" };

			var currentClassName = _repo.stack.GetContext(e => e.type == "class")?.name;
			if (currentClassName == null)
			{
				return false;
			}

			var possibleDependencies = _classes
				.Where(e => typeList.Contains(e.type))
				.Where(c => c.name != currentClassName)
				.Select(c => c.name);

			foreach (var dependency in possibleDependencies)
			{
				var tokenIndex = semi.FindFirst(dependency);

				if (tokenIndex != -1)
				{
					var local = new CSemiExp();
					local.Add(currentClassName);
					local.Add(dependency);
					doActions(local);
				}
			}

			return false;
		}
	}
}
