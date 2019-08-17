////////////////////////////////////////////////////////////////////////////////////
// DetectClass.cs                                                                 //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
using System;
using Parser.Parser;

namespace Parser.Rules
{
  /////////////////////////////////////////////////////////
  // rule to dectect class definitions
  public class DetectClass : ARule
  {
    public override bool test(CSemiExp semi)
    {
      Display.Display.displayRules(actionDelegate, "rule   DetectClass");
      int indexCL = semi.Contains("class");
      int indexIF = semi.Contains("interface");
      int indexST = semi.Contains("struct");

      int index = Math.Max(indexCL, indexIF);
      index = Math.Max(index, indexST);
      if (index != -1)
      {
        CSemiExp local = new CSemiExp();
        local.displayNewLines = false;
        local.Add(semi[index]).Add(semi[index + 1]);
        doActions(local);
        return true;
      }
      return false;
    }
  }
}