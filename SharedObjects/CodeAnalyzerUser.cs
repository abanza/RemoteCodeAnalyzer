////////////////////////////////////////////////////////////////////////////////////
// CodeAnalyzerUser.cs                                                            //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
namespace SharedObjects
{
  public class CodeAnalyzerUser
  {
    public int Id { get; set; }
    public string Role { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public bool IsAdmin()
    {
        return Role == "Administrator";
    }
  }
}
