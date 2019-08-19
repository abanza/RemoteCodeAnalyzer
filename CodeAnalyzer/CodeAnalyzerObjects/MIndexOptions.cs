// MIndexOptions.cs                                                               

namespace CodeAnalyzer.CodeAnalyzerObjects
{
	public class MIndexOptions
	{
		public float LinesOfCodeWeight { get; set; } = 1f;
		public float ComplexityWeight { get; set; } = 1f;
		public float CohesionWeight { get; set; } = 1f;
		public float CouplingWeight { get; set; } = 1f;
	}
}
