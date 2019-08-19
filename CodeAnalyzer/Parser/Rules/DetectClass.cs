// DetectClass.cs                                                                 

using System;
using Parser.Parser;

namespace Parser.Rules
{
	/////////////////////////////////////////////////////////
	// rule to dectect class definitions
	public class DetectClass : ARule
	{
		public override bool test(CSemiExp semi)
		{
			Display.Display.displayRules(actionDelegate, "rule   DetectClass");
			int indexCL = semi.Contains("class");
			int indexIF = semi.Contains("interface");
			int indexST = semi.Contains("struct");

			int index = Math.Max(indexCL, indexIF);
			index = Math.Max(index, indexST);
			if (index != -1)
			{
				CSemiExp local = new CSemiExp();
				local.displayNewLines = false;
				local.Add(semi[index]).Add(semi[index + 1]);
				doActions(local);
				return true;
			}
			return false;
		}
	}
}