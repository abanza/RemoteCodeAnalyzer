////////////////////////////////////////////////////////////////////////////////////
// MIndex.cs                                                                      //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
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
