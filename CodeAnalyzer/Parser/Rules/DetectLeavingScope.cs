////////////////////////////////////////////////////////////////////////////////////
// DetectLeavingScope.cs                                                          //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
using Parser.Parser;

namespace Parser.Rules
{
  /////////////////////////////////////////////////////////
  // detect leaving scope
  public class DetectLeavingScope : ARule
  {
    public override bool test(CSemiExp semi)
    {
      Display.Display.displayRules(actionDelegate, "rule   DetectLeavingScope");
      int index = semi.Contains("}");
      if (index != -1)
      {
        doActions(semi);
        return true;
      }
      return false;
    }
  }
}