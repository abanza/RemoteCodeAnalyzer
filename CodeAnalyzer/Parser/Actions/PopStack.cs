// PopStack.cs                                                                    

using System;
using Parser.Parser;

namespace Parser.Actions
{
	/////////////////////////////////////////////////////////
	// pops scope info from stack when leaving scope
	public class PopStack : AAction
	{
		public PopStack(Repository repo)
		{
			repo_ = repo;
		}
		public override void doAction(CSemiExp semi)
		{
			Elem elem;
			try
			{
				elem = repo_.stack.pop(); Display.Display.displayActions(actionDelegate, $"action PopStack  ({elem.type}-{elem.name})");
				for (int i = 0; i < repo_.locations.Count; ++i)
				{
					Elem temp = repo_.locations[i];
					if (elem.type == temp.type)
					{
						if (elem.name == temp.name)
						{
							if ((repo_.locations[i]).endLine == 0)
							{
								(repo_.locations[i]).endLine = repo_.semi.lineCount;
								(repo_.locations[i]).endScopeCount = repo_.scopeCount;
								break;
							}
						}
					}
				}
			}
			catch
			{
				return;
			}
			CSemiExp local = new CSemiExp();
			local.Add(elem.type).Add(elem.name);
			if (local[0] == "control")
				return;

			if (displaySemi)
			{
				Console.Write("\n  line# {0,-5}", repo_.semi.lineCount);
				Console.Write("leaving  ");
				string indent = new string(' ', 2 * (repo_.stack.count + 1));
				Console.Write("{0}", indent);
				display(local); // defined in abstract action
			}
		}
	}
}