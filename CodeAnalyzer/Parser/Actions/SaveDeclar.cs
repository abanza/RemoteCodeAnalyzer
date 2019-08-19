// SaveDeclar.cs                                                                  

using System;
using Parser.Parser;

namespace Parser.Actions
{
	/////////////////////////////////////////////////////////
	// display public declaration
	public class SaveDeclar : AAction
	{
		public SaveDeclar(Repository repo)
		{
			repo_ = repo;
		}
		public override void doAction(CSemiExp semi)
		{
			Display.Display.displayActions(actionDelegate, "action SaveDeclar");
			Elem elem = new Elem();
			elem.type = semi[0];  // expects type
			elem.name = semi[1];  // expects name
			elem.beginLine = repo_.semi.lineCount;
			elem.endLine = elem.beginLine;
			elem.beginScopeCount = repo_.scopeCount;
			elem.endScopeCount = elem.beginScopeCount;
			if (displaySemi)
			{
				Console.Write("\n  line# {0,-5}", repo_.semi.lineCount - 1);
				Console.Write("entering ");
				string indent = new string(' ', 2 * repo_.stack.count);
				Console.Write("{0}", indent);
				display(semi); // defined in abstract action
			}
			repo_.locations.Add(elem);
		}
	}
}