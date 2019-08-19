// Repository.cs                                                                  

using System.Collections.Generic;

namespace Parser.Parser
{
	public class Repository
	{
		ScopeStack<Elem> stack_ = new ScopeStack<Elem>();
		List<Elem> locations_ = new List<Elem>();
		Dictionary<string, List<Elem>> locationsTable_ = new Dictionary<string, List<Elem>>();

		//----< provides all actions access to current semiExp >-----------

		public CSemiExp semi
		{
			get;
			set;
		}

		// semi gets line count from toker who counts lines
		// while reading from its source

		public int lineCount  // saved by newline rule's action
		{
			get { return semi.lineCount; }
		}
		public int prevLineCount  // not used in this demo
		{
			get;
			set;
		}

		//----< enables recursively tracking entry and exit from scopes >--

		public int scopeCount
		{
			get;
			set;
		}

		public ScopeStack<Elem> stack  // pushed and popped by scope rule's action
		{
			get { return stack_; }
		}

		// the locations table is the result returned by parser's actions
		// in this demo

		public List<Elem> locations
		{
			get { return locations_; }
			set { locations_ = value; }
		}

		public Dictionary<string, List<Elem>> LocationsTable
		{
			get { return locationsTable_; }
			set { locationsTable_ = value; }
		}

		// Contains keys with classes and values with each class's dependencies
		public Dictionary<string, List<string>> ClassDependencies { get; set; } = new Dictionary<string, List<string>>();

		// Contains keys with classes and values with each class's member variables
		public Dictionary<string, List<string>> ClassMembers { get; set; } = new Dictionary<string, List<string>>();

		// Contains keys with classes and functions and values with each function's member variables used
		public Dictionary<(string Class, string Function), List<string>> FunctionMembers { get; set; } = new Dictionary<(string Class, string Function), List<string>>();
	}
}