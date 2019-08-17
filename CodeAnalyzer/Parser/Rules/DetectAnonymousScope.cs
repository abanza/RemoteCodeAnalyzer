////////////////////////////////////////////////////////////////////////////////////
// DetectAnonymousScope.cs                                                        //
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
  // detect entering anonymous scope
  // - expects namespace, class, and function scopes
  //   already handled, so put this rule after those
  public class DetectAnonymousScope : ARule
  {
    public override bool test(CSemiExp semi)
    {
      Display.Display.displayRules(actionDelegate, "rule   DetectAnonymousScope");
      int index = semi.Contains("{");
      if (index != -1)
      {
        CSemiExp local = new CSemiExp();
        // create local semiExp with tokens for type and name
        local.displayNewLines = false;
        local.Add("control").Add("anonymous");
        doActions(local);
        return true;
      }
      return false;
    }
  }
}