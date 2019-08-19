// Print.cs                                                                       

using System;
using Parser.Parser;

namespace Parser.Actions
{
	/////////////////////////////////////////////////////////
	// concrete printing action, useful for debugging
	public class Print : AAction
	{
		public Print(Repository repo)
		{
			repo_ = repo;
		}
		public override void doAction(CSemiExp semi)
		{
			Console.Write("\n  line# {0}", repo_.semi.lineCount - 1);
			display(semi);
		}
	}
}