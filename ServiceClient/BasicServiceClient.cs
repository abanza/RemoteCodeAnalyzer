////////////////////////////////////////////////////////////////////////////////////
// BasicServiceClient.cs                                                          //
// ver 1.0                                                                        //
// Language:    C# Visual Studio 2017                                             //
// Platform:    HP G1 800, Windows 10                                             //
// Application: Remote Code Analyzer Project, CSE681 - Software Modeling Analysis //
// Author:      Adelard Banza, updated from Prof. Jim Fawcett codes               //
//              abanza@syr.edu                                                    //
////////////////////////////////////////////////////////////////////////////////////
using SharedObjects;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using RemoteRepositoryService;
using System.IO;
using System.Collections.Generic;

namespace ServiceClient
{
  public class BasicServiceClient
  {
    IRemoteRepositoryService svc;

    public BasicServiceClient(string url)
    {
      svc = CreateProxy<IRemoteRepositoryService>(url);
    }
    //----< returns a proxy of type T >----------------------------------

    static C CreateProxy<C>(string url)
    {
      BasicHttpBinding binding = new BasicHttpBinding();
      EndpointAddress address = new EndpointAddress(url);
      ChannelFactory<C> factory = new ChannelFactory<C>(binding, address);
      return factory.CreateChannel();
    }
    //----< Wrapper attempts to call service method several times >------

    T ServiceRetryWrapper<T>(Func<T> fnc)
    {
      int count = 0;
      T returnValue;
      while (true)
      {
        try
        {
          returnValue = fnc.Invoke();
          break;
        }
        catch (Exception exc)
        {
          if (count > 4)
          {
            throw new RetryException("Maximum retries exceeded"); ;
          }
          Console.Write("\n  {0}", exc.Message);
          Console.Write("\n  service failed {0} times - trying again", ++count);
          Thread.Sleep(100);
        }
      }
      return returnValue;
    }

    public bool CreateUser(string role, string username, string password)
    {
      return ServiceRetryWrapper(() => svc.CreateUser(role, username, password));
    }

    public void CheckIn(string username, string projectName, string filename, Stream filestream)
    {
      svc.CheckIn(new CheckInData
      {
        Username = username,
        Project = projectName,
        Filename = filename,
        File = filestream
      });
    }

    public Stream CheckOut(string username, string filename, string project = null)
    {
      return ServiceRetryWrapper(() => svc.CheckOut(username, filename, project));
    }

    public bool CreateFolder(string username, string projectName)
    {
      return ServiceRetryWrapper(() => svc.CreateFolder(username, projectName));
    }

    public CodeAnalyzerUser GetUser(string username)
    {
      return svc.GetUser(username);
    }

    public CodeAnalyzerUser GetUser(string username, string password)
    {
        return svc.GetUserByPassword(username, password);
    }

    public int GetNumUsers()
    {
      return svc.GetNumUsers();
    }

    public IEnumerable<CodeAnalyzerFolder> BrowseFolders()
    {
      return svc.BrowseAllFolders();
    }

    public IEnumerable<string> BrowseFolders(string username)
    {
      return svc.BrowseFolders(username);
    }

    public IEnumerable<CodeAnalyzerSharedFolder> BrowseSharedFolders(int userId)
    {
      return svc.BrowseSharedFolders(userId);
    }

    public IEnumerable<CodeAnalyzerFile> BrowseFiles(string username, string folderName)
    {
      return svc.BrowseFiles(username, folderName);
    }

    public IEnumerable<CodeAnalyzerFile> BrowseFiles(int userId, string folderName)
    {
      return svc.BrowseFilesByUserId(userId, folderName);
    }

    public bool ShareFolder(int ownerId, string username, string folderName)
    {
      return svc.ShareFolder(ownerId, username, folderName);
    }

    public void CreateReport(int ownerId, string file)
    {
      svc.CreateReport(ownerId, file);
    }
  }
}

#if PROG_CLIENT_TEST
    static void Main(string[] args)
    {
      Console.Title = "BasicHttp Client";
      Console.Write("\n  Starting Programmatic Basic Service Client");
      Console.Write("\n ============================================\n");

      string url = "http://localhost:8080/RemoteRepositoryService";
      ProgClient client = new ProgClient(url);

      var username = "Adelard";
      Console.WriteLine($"Creating user {username} as a Developer.");
      client.CreateUser("Developer", username);

      username = "Jennifer";
      Console.WriteLine($"Creating user {username} as a Developer.");
      client.CreateUser("Developer", username);

      username = "Lauren";
      Console.WriteLine($"Creating user {username} as a Adminstrator.");
      client.CreateUser("Adminstrator", username);

      var projectName = "TestProject1";
      Console.WriteLine($"Creating project {projectName} for user {username}.");
      client.CreateProject(username, projectName);

      var filepath = @"C:\Client.cs";
      var filename = Path.GetFileName(filepath);
      using (var fs = new FileStream(filepath, FileMode.Open))
      {
        Console.WriteLine($"Checking in file at path {filepath} for user {username} and project {projectName}.");
        client.CheckIn(username, projectName, filename, fs);
      }

      var waitTime = 500;
      Console.WriteLine($"Waiting for {waitTime}");
      Thread.Sleep(waitTime);

      var newFilePath = @"CheckedOut\Client.cs";
      Console.WriteLine($"Checking out file to path {newFilePath} for user {username} and project {projectName}.");
      var newFs = client.CheckOut(username, filename, "TestProject1");
      Directory.CreateDirectory("CheckedOut");
      newFs.CopyTo(new FileStream(newFilePath, FileMode.Create));

      Console.ReadKey(); // Wait for user to quit
    }

#endif