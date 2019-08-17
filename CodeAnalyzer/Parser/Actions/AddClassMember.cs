////////////////////////////////////////////////////////////////////////////////////
// AddClassMember.cs                                                              //
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
  class AddClassMember : AAction
  {
    private readonly Repository _repo;

    public AddClassMember(Repository repo)
    {
      _repo = repo;
    }

    public override void doAction(CSemiExp semi)
    {
      (var keyClass, var member) = (semi[0], semi[1]);

      if (!_repo.ClassMembers.ContainsKey(keyClass))
      {
        _repo.ClassMembers[keyClass] = new List<string>();
      }

      if (!_repo.ClassMembers[keyClass].Contains(member))
      {
        _repo.ClassMembers[keyClass].Add(member);
      }
    }
  }
}
