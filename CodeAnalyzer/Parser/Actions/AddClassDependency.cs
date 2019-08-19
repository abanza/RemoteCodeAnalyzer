// AddClassDependency.cs                                                          


using System.Collections.Generic;
using Parser.Parser;

namespace Parser.Actions
{
	public class AddClassDependency : AAction
	{
		private readonly Repository _repo;

		public AddClassDependency(Repository repo)
		{
			_repo = repo;
		}

		public override void doAction(CSemiExp semi)
		{
			(var keyClass, var dependencyClass) = (semi[0], semi[1]);

			if (!_repo.ClassDependencies.ContainsKey(keyClass))
			{
				_repo.ClassDependencies[keyClass] = new List<string>();
			}

			if (!_repo.ClassDependencies[keyClass].Contains(dependencyClass))
			{
				_repo.ClassDependencies[keyClass].Add(dependencyClass);
			}
		}
	}
}
