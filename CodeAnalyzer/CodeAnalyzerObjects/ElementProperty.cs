// ElementProperty.cs                                                             

using System.Collections.Generic;

namespace CodeAnalyzer.CodeAnalyzerObjects
{
	public class ElementProperty
	{
		public ElementProperty()
		{
			Dependencies = new List<ElementProperty>();
			Members = new List<string>();
		}
		public string Type { get; set; }
		public string Name { get; set; }
		public int LinesOfCode { get; set; }
		public string FunctionClass { get; set; }
		public int CyclomaticComplexity { get; set; }
		public List<ElementProperty> Dependencies { get; set; }
		public List<string> Members { get; set; }
		public int Cohesion { get; set; }
		public float MIndex { get; set; }
	}
}