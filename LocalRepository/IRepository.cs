////////////////////////////////////////////////////////////////////////////////////
// IRepository.cs                                                                 //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using SharedObjects;

namespace LocalRepository
{
    public interface IRepository
    {
        void AddUser(CodeAnalyzerUser user);
        void CheckIn(string username, string projectName, string filename, byte[] fileData);
        CodeAnalyzerFile CheckOut(string username, string projectName, string filename);
        void ClearAll();
        void ClearFiles();
        void ClearFolders();
        void ClearUsers();
        int CreateFolder(string username, string projectName);
        IEnumerable<CodeAnalyzerFile> GetFiles(string username, string project);
        IEnumerable<CodeAnalyzerFile> GetFiles(int userId, string project);
        int GetFolderId(string username, string name);
        IEnumerable<CodeAnalyzerFolder> GetFolders();
        IEnumerable<string> GetFolders(string username);
        IEnumerable<CodeAnalyzerSharedFolder> GetSharedFolders(int userId);
        CodeAnalyzerUser GetUser(string username);
        CodeAnalyzerUser GetUser(string username, string password);
        int GetUserId(string name);
        IEnumerable<CodeAnalyzerUser> GetUsers();
        bool UserExists(string username);
        bool ShareFolder(int ownerId, string username, string folderName);
        void CreateReport(int ownerId, string filename);
  }
}