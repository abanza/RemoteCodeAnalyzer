// MIndex.cs                                                                      

using CodeAnalyzer.CodeAnalyzerObjects;
using System.Collections.Generic;

namespace Parser.Metrics
{
	public class MIndex
	{
		public static MIndexOptions Options;

		public static float Calculate(List<ElementProperty> elements, ElementProperty element)
		{
			if (Options == null)
			{
				Options = new MIndexOptions();
			}

			return element.LinesOfCode * Options.LinesOfCodeWeight
			  + element.CyclomaticComplexity * Options.ComplexityWeight
			  + element.Cohesion * Options.CohesionWeight
			  + element.Dependencies.Count * Options.CouplingWeight;
		}
	}
}
