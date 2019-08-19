// DetectFunction.cs                                                              

using Parser.Parser;

namespace Parser.Rules
{
	/////////////////////////////////////////////////////////
	// rule to dectect function definitions
	public class DetectFunction : ARule
	{
		private readonly Repository _repo;

		public DetectFunction(Repository repo)
		{
			_repo = repo;
		}

		public static bool isSpecialToken(string token)
		{
			string[] SpecialToken = { "if", "for", "foreach", "while", "catch", "using" };
			foreach (string stoken in SpecialToken)
				if (stoken == token)
					return true;
			return false;
		}
		public override bool test(CSemiExp semi)
		{
			Display.Display.displayRules(actionDelegate, "rule   DetectFunction");
			if (semi[semi.count - 1] != "{")
				return false;

			int index = semi.FindFirst("(");
			if (index > 0 && !isSpecialToken(semi[index - 1]))
			{
				var currentClassName = _repo.stack.GetContext(e => e.type == "class")?.name;
				CSemiExp local = new CSemiExp();
				local
				  .Add("function")
				  .Add(semi[index - 1])
				  .Add(currentClassName);
				doActions(local);
				return true;
			}
			return false;
		}
	}
}