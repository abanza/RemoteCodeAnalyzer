////////////////////////////////////////////////////////////////////////////////////
// CodeAnalyzerFile.cs                                                            //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
namespace SharedObjects
{
    public class CodeAnalyzerFile
    {
      public int Id { get; set; }
      public string Name { get; set; }
      public byte[] Data { get; set; }
      public string FolderName { get; set; }
      public string Username { get; set; }
  }
}
