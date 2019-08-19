// AddMemberUsage.cs                                                              

using System.Collections.Generic;
using Parser.Parser;

namespace Parser.Actions
{
	public class AddMemberUsage : AAction
	{
		private readonly Repository _repo;

		public AddMemberUsage(Repository repo)
		{
			_repo = repo;
		}

		public override void doAction(CSemiExp semi)
		{
			(var keyClass, var keyFunction, var member) = (semi[0], semi[1], semi[2]);

			if (!_repo.FunctionMembers.ContainsKey((keyClass, keyFunction)))
			{
				_repo.FunctionMembers[(keyClass, keyFunction)] = new List<string>();
			}

			if (!_repo.FunctionMembers[(keyClass, keyFunction)].Contains(member))
			{
				_repo.FunctionMembers[(keyClass, keyFunction)].Add(member);
			}
		}
	}
}
