// PrintFunction.cs                                                               

using System;
using Parser.Parser;

namespace Parser.Actions
{
	///////////////////////////////////////////////////////////
	// action to print function signatures - not used in demo
	public class PrintFunction : AAction
	{
		public PrintFunction(Repository repo)
		{
			repo_ = repo;
		}
		public override void display(CSemiExp semi)
		{
			Console.Write("\n    line# {0}", repo_.semi.lineCount - 1);
			Console.Write("\n    ");
			for (int i = 0; i < semi.count; ++i)
				if (semi[i] != "\n" && !semi.isComment(semi[i]))
					Console.Write("{0} ", semi[i]);
		}
		public override void doAction(CSemiExp semi)
		{
			display(semi);
		}
	}
}