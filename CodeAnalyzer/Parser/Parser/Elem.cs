// Elem.cs                                                                        

using System;
using System.Text;

namespace Parser.Parser
{
	public class Elem
	{
		public string type { get; set; }
		public string name { get; set; }
		public int beginLine { get; set; }
		public int endLine { get; set; }
		public int beginScopeCount { get; set; }
		public int endScopeCount { get; set; }
		public string functionClass;

		public override string ToString()
		{
			StringBuilder temp = new StringBuilder();
			temp.Append("{");
			temp.Append(String.Format("{0,-10}", type)).Append(" : ");
			temp.Append(String.Format("{0,-10}", name)).Append(" : ");
			temp.Append(String.Format("{0,-5}", beginLine));  // line of scope start
			temp.Append(String.Format("{0,-5}", endLine));    // line of scope end
			temp.Append("}");
			return temp.ToString();
		}
	}
}