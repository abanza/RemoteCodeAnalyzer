// DetectNamespace.cs                                                             

using Parser.Parser;

namespace Parser.Rules
{
	////////////////////////////////////////////////////////
	// rule to detect namespace declarations
	public class DetectNamespace : ARule
	{
		public override bool test(CSemiExp semi)
		{
			Display.Display.displayRules(actionDelegate, "rule   DetectNamespace");
			int index = semi.Contains("namespace");
			if (index != -1)
			{
				CSemiExp local = new CSemiExp();
				// create local semiExp with tokens for type and name
				local.displayNewLines = false;
				local.Add(semi[index]).Add(semi[index + 1]);
				doActions(local);
				return true;
			}
			return false;
		}
	}
}