// Cohesion.cs                                                                    

using CodeAnalyzer.CodeAnalyzerObjects;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Metrics
{
	public class Cohesion
	{
		public static int Calculate(List<ElementProperty> elements, ElementProperty element)
		{
			var lcom = 0;

			// Methods in this class
			var methods = elements
			  .Where(e => e.Type == "function" && e.FunctionClass == element.Name)
			  .ToList();

			// For every pair of methods
			for (var i = 0; i < methods.Count; i++)
			{
				for (var j = i + 1; j < methods.Count; j++)
				{
					var m = methods[i];
					var n = methods[j];

					// Increment lcom1 if they access no common members
					if ((m.Members.Count == 0 || n.Members.Count == 0)
						&& !m.Members.Intersect(n.Members).Any())
					{
						lcom++;
					}
				}
			}
			return lcom;
		}
	}
}
