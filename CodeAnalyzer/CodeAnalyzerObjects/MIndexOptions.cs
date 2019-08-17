////////////////////////////////////////////////////////////////////////////////////
// MIndexOptions.cs                                                               //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
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
