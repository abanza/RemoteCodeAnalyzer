// DetectPublicDeclar.cs                                                          

using Parser.Parser;

namespace Parser.Rules
{
	/////////////////////////////////////////////////////////
	// detect public declaration
	public class DetectPublicDeclar : ARule
	{
		public override bool test(CSemiExp semi)
		{
			Display.Display.displayRules(actionDelegate, "rule   DetectPublicDeclar");
			int index = semi.Contains(";");
			if (index != -1)
			{
				index = semi.Contains("public");
				if (index == -1)
					return true;
				CSemiExp local = new CSemiExp();

				local.displayNewLines = false;
				local.Add("public " + semi[index + 1]).Add(semi[index + 2]);

				index = semi.Contains("=");
				if (index != -1)
				{
					doActions(local);
					return true;
				}
				index = semi.Contains("(");
				if (index == -1)
				{
					doActions(local);
					return true;
				}
			}
			return false;
		}
	}
}