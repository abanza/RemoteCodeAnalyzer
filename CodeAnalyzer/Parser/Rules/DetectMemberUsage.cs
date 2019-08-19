// DetectMemberUsage.cs                                                           

using System.Collections.Generic;
using Parser.Parser;

namespace Parser.Rules
{
	public class DetectMemberUsage : ARule
	{
		private readonly Repository _repo;
		private readonly Dictionary<string, List<string>> _classMembers;

		public DetectMemberUsage(Repository repo, Dictionary<string, List<string>> classMembers)
		{
			_repo = repo;
			_classMembers = classMembers;
		}

		public override bool test(CSemiExp semi)
		{
			var currentClassName = _repo.stack.GetContext(e => e.type == "class")?.name;
			var currentFunctionName = _repo.stack.GetContext(e => e.type == "function")?.name;
			if (currentClassName == null || currentFunctionName == null)
			{
				return false;
			}

			if (!_classMembers.ContainsKey(currentClassName))
			{
				return false;
			}

			var classMembers = _classMembers[currentClassName];
			foreach (var member in classMembers)
			{
				var tokenIndex = semi.FindFirst(member);
				if (tokenIndex != -1)
				{
					var local = new CSemiExp();
					local.Add(currentClassName);
					local.Add(currentFunctionName);
					local.Add(member);
					doActions(local);
					return false;
				}
			}


			return false;
		}
	}
}
