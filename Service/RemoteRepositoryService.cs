// RemoteRepositoryService.cs                                                     

#define SERVICE_PRINT

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using LocalRepository;
using SharedObjects;

namespace RemoteRepositoryService
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, IncludeExceptionDetailInFaults = true)]
	public class RemoteRepositoryService : IRemoteRepositoryService
	{
		private readonly IRepository _repository;

		public RemoteRepositoryService()
		{
			//_repository = new FileRepository(@"C:\RemoteRepository");
			_repository = new DbRepository();
		}

		public bool CreateUser(string role, string username, string password)
		{
			_repository.AddUser(new CodeAnalyzerUser
			{
				Role = role,
				Username = username,
				Password = password
			});

			return true;
		}

		public IEnumerable<CodeAnalyzerUser> GetUsers()
		{
			return _repository.GetUsers();
		}

		public int GetNumUsers()
		{
			return _repository.GetUsers().Count();
		}

		public CodeAnalyzerUser GetUser(string username)
		{
			return _repository.GetUser(username);
		}
		public CodeAnalyzerUser GetUserByPassword(string username, string password)
		{
			return _repository.GetUser(username, password);
		}

		public IEnumerable<CodeAnalyzerFolder> BrowseAllFolders()
		{
			return _repository.GetFolders();
		}
		public IEnumerable<string> BrowseFolders(string username)
		{
			return _repository.GetFolders(username);
		}

		public IEnumerable<CodeAnalyzerSharedFolder> BrowseSharedFolders(int userId)
		{
			return _repository.GetSharedFolders(userId);
		}

		public IEnumerable<CodeAnalyzerFile> BrowseFiles(string username, string project)
		{
			return _repository.GetFiles(username, project);
		}

		public IEnumerable<CodeAnalyzerFile> BrowseFilesByUserId(int userId, string project)
		{
			return _repository.GetFiles(userId, project);
		}

		public bool ShareFolder(int ownerId, string username, string folderName)
		{
			return _repository.ShareFolder(ownerId, username, folderName);
		}

		public void CheckIn(CheckInData data)
		{
			using (var ms = new MemoryStream())
			{
				data.File.CopyTo(ms);
				_repository.CheckIn(data.Username, data.Project, data.Filename, ms.ToArray());
			}
		}

		public Stream CheckOut(string username, string filename, string project)
		{
			var file = _repository.CheckOut(username, project, filename);

			if (file == null)
			{
				throw new ArgumentException("The specified file does not exist");
			}

			return new MemoryStream(file.Data);
		}

		public bool CreateFolder(string username, string projectName)
		{
			return _repository.CreateFolder(username, projectName) > 0;
		}

		public void CreateReport(int ownerId, string filename)
		{
			_repository.CreateReport(ownerId, filename);
		}

	}
}
