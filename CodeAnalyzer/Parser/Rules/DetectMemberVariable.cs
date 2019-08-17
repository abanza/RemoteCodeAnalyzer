////////////////////////////////////////////////////////////////////////////////////
// DetectMemberVariable.cs                                                        //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Linq;
using Parser.Parser;

namespace Parser.Rules
{
  class DetectMemberVariable : ARule
  {
    private readonly Repository _repo;

    public DetectMemberVariable(Repository repo)
    {
      _repo = repo;
    }

    public override bool test(CSemiExp semi)
    {
      var accessModifiers = new List<string> { "public", "protected", "private", "internal" };
      var mutationModifier = new List<string> { "readonly", "const" };

      var currentClassName = _repo.stack.GetContext(e => e.type == "class")?.name;
      if (currentClassName == null)
      {
        // We're not inside a class
        return false;
      }

      var containsAccessModifier = accessModifiers.Any(a => semi.Contains(a) != -1);
      var containsParens = semi.Contains("(") != -1 || semi.Contains(")") != -1;
      if (containsAccessModifier && !containsParens)
      {
        var local = new CSemiExp();
        local.Add(currentClassName);
        if (semi.Contains("=") == -1)
        {
          local.Add(semi[semi.count - 2]);
          doActions(local);
          return true;
        }
        else
        {
          var index = semi.FindFirst("=");
          local.Add(semi[index - 1]);
          doActions(local);
          return true;
        }
      }

      return false;
    }
  }
}
