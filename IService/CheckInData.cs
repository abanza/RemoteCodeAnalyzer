////////////////////////////////////////////////////////////////////////////////////
// CheckInData.cs                                                                 //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
using System.IO;
using System.ServiceModel;

namespace RemoteRepositoryService
{
  [MessageContract]
  public class CheckInData
  {
    [MessageHeader(MustUnderstand = true)]
    public string Username { get; set; }
    [MessageHeader(MustUnderstand = true)]
    public string Project { get; set; }
    [MessageHeader(MustUnderstand = true)]
    public string Filename { get; set; }
    [MessageBodyMember(Order = 1)]
    public Stream File { get; set; }
  }
}