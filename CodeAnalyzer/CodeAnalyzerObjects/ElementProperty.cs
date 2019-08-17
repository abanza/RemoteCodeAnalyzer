////////////////////////////////////////////////////////////////////////////////////
// ElementProperty.cs                                                             //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza,  updated from Prof. Jim Fawcett codes              //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
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