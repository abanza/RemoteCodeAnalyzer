////////////////////////////////////////////////////////////////////////////////////
// AddMemberUsage.cs                                                              //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using Parser.Parser;

namespace Parser.Actions
{
    public class AddMemberUsage : AAction
    {
      private readonly Repository _repo;

      public AddMemberUsage(Repository repo)
      {
        _repo = repo;
      }

      public override void doAction(CSemiExp semi)
      {
        (var keyClass, var keyFunction, var member) = (semi[0], semi[1], semi[2]);

        if (!_repo.FunctionMembers.ContainsKey((keyClass, keyFunction)))
        {
          _repo.FunctionMembers[(keyClass, keyFunction)] = new List<string>();
        }

        if (!_repo.FunctionMembers[(keyClass, keyFunction)].Contains(member))
        {
          _repo.FunctionMembers[(keyClass, keyFunction)].Add(member);
        }
    }
    }
}
