////////////////////////////////////////////////////////////////////////////////////
// Print.cs                                                                       //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
using System;
using Parser.Parser;

namespace Parser.Actions
{
  /////////////////////////////////////////////////////////
  // concrete printing action, useful for debugging
  public class Print : AAction
  {
    public Print(Repository repo)
    {
      repo_ = repo;
    }
    public override void doAction(CSemiExp semi)
    {
      Console.Write("\n  line# {0}", repo_.semi.lineCount - 1);
      display(semi);
    }
  }
}