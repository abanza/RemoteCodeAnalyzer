////////////////////////////////////////////////////////////////////////////////////
// AddClassDependency.cs                                                          //
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
    public class AddClassDependency : AAction
    {
      private readonly Repository _repo;

      public AddClassDependency(Repository repo)
      {
        _repo = repo;
      }

      public override void doAction(CSemiExp semi)
      {
        (var keyClass, var dependencyClass) = (semi[0], semi[1]);
        
        if (!_repo.ClassDependencies.ContainsKey(keyClass))
        {
          _repo.ClassDependencies[keyClass] = new List<string>();
        }

        if (!_repo.ClassDependencies[keyClass].Contains(dependencyClass))
        {
          _repo.ClassDependencies[keyClass].Add(dependencyClass);
        }
      }
    }
}
