////////////////////////////////////////////////////////////////////////////////////
// DetectPublicDeclar.cs                                                          //
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
  // detect public declaration
  public class DetectPublicDeclar : ARule
  {
    public override bool test(CSemiExp semi)
    {
      Display.Display.displayRules(actionDelegate, "rule   DetectPublicDeclar");
      int index = semi.Contains(";");
      if (index != -1)
      {
        index = semi.Contains("public");
        if (index == -1)
          return true;
        CSemiExp local = new CSemiExp();
 
        local.displayNewLines = false;
        local.Add("public "+semi[index+1]).Add(semi[index+2]);

        index = semi.Contains("=");
        if (index != -1)
        {
          doActions(local);
          return true;
        }
        index = semi.Contains("(");
        if(index == -1)
        {
          doActions(local);
          return true;
        }
      }
      return false;
    }
  }
}