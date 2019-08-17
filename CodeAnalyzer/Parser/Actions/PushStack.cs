////////////////////////////////////////////////////////////////////////////////////
// PushStack.cs                                                                   //
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
  // pushes scope info on stack when entering new scope
  public class PushStack : AAction
  {
    public PushStack(Repository repo)
    {
      repo_ = repo;
    }
    public override void doAction(CSemiExp semi)
    {
      Display.Display.displayActions(actionDelegate, $"action PushStack ({semi[0]}-{semi[1]})");
      ++repo_.scopeCount;
      Elem elem = new Elem();
      elem.type = semi[0];  // expects type
      elem.name = semi[1];  // expects name
      if (semi.count > 2)
      {
        // expects class in which function resides or null if not inside a class
        elem.functionClass = semi[2];
      }
      elem.beginLine = repo_.semi.lineCount - 1;
      elem.endLine = 0;
      elem.beginScopeCount = repo_.scopeCount;
      elem.endScopeCount = 0;
      repo_.stack.push(elem);
      if (displayStack)
        repo_.stack.display();
      if (displaySemi)
      {
        Console.Write("\n  line# {0,-5}", repo_.semi.lineCount - 1);
        Console.Write("entering ");
        string indent = new string(' ', 2 * repo_.stack.count);
        Console.Write("{0}", indent);
        display(semi); // defined in abstract action
      }
      if (elem.type == "control" || elem.name == "anonymous")
        return;
      repo_.locations.Add(elem);
    }
  }
}