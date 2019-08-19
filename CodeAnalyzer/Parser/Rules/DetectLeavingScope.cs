// DetectLeavingScope.cs                                                          

using Parser.Parser;

namespace Parser.Rules
{
	/////////////////////////////////////////////////////////
	// detect leaving scope
	public class DetectLeavingScope : ARule
	{
		public override bool test(CSemiExp semi)
		{
			Display.Display.displayRules(actionDelegate, "rule   DetectLeavingScope");
			int index = semi.Contains("}");
			if (index != -1)
			{
				doActions(semi);
				return true;
			}
			return false;
		}
	}
}