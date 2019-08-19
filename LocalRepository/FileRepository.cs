// FileRepository.cs                                                              

using System;
using System.Collections.Generic;
using SharedObjects;
using System.IO;
using System.Linq;

namespace LocalRepository
{
	public class FileRepository : IRepository
	{
		private readonly DirectoryInfo _root;
		private readonly string _usersFilePath;
		private readonly string _projectsFilePath;

		public FileRepository(string rootDirectoryName)
		{
			_root = Directory.CreateDirectory(rootDirectoryName);
			_usersFilePath = Path.Combine(_root.FullName, "users");
			_projectsFilePath = Path.Combine(_root.FullName, "projects");
		}
		public void AddUser(CodeAnalyzerUser user)
		{
			using (var fs = new StreamWriter(_usersFilePath))
			{
				fs.WriteLine($"{user.Username}, {user.Password}, {user.Role}");
			}
		}

		public void CheckIn(string username, string projectName, string filename, byte[] fileData)
		{
			throw new NotImplementedException();
		}

		public CodeAnalyzerFile CheckOut(string username, string projectName, string filename)
		{
			throw new NotImplementedException();
		}

		public void ClearAll()
		{
			throw new NotImplementedException();
		}

		public void ClearFiles()
		{
			throw new NotImplementedException();
		}

		public void ClearFolders()
		{
			throw new NotImplementedException();
		}

		public void ClearUsers()
		{
			File.WriteAllText(_usersFilePath, string.Empty);
		}

		public int CreateFolder(string username, string projectName)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CodeAnalyzerFile> GetFiles(string username, string project)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CodeAnalyzerFile> GetFiles(int userId, string project)
		{
			throw new NotImplementedException();
		}

		public int GetFolderId(string username, string name)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CodeAnalyzerFolder> GetFolders()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetFolders(string username)
		{
			throw new NotImplementedException();
			//try
			//{
			//    return File.ReadAllLines(_projectsFilePath)
			//        .Select(line => new CodeAnalyzerUser
			//        {
			//            Username = line.Split(',')[0],
			//            Role = line.Split(',')[1]
			//        })
			//        .FirstOrDefault(u => string.Compare(u.Username, username, StringComparison.CurrentCultureIgnoreCase) == 0);
			//}
			//catch (Exception e)
			//{
			//    return null;
			//}
		}

		public IEnumerable<CodeAnalyzerSharedFolder> GetSharedFolders(int userId)
		{
			throw new NotImplementedException();
		}


		public CodeAnalyzerUser GetUser(string username)
		{
			try
			{
				return File.ReadAllLines(_usersFilePath)
					.Select(line => new CodeAnalyzerUser
					{
						Username = line.Split(',')[0],
						Role = line.Split(',')[1]
					})
					.FirstOrDefault(u => string.Compare(u.Username, username, StringComparison.CurrentCultureIgnoreCase) == 0);
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public CodeAnalyzerUser GetUser(string username, string password)
		{
			try
			{
				return File.ReadAllLines(_usersFilePath)
					.Select(line => new CodeAnalyzerUser
					{
						Username = line.Split(',')[0],
						Password = line.Split(',')[1],
						Role = line.Split(',')[2]
					})
					.FirstOrDefault(u => string.Compare(u.Username, username, StringComparison.CurrentCultureIgnoreCase) == 0 &&
					string.Compare(u.Password, password, StringComparison.CurrentCultureIgnoreCase) == 0);
			}
			catch (Exception e)
			{
				return null;
			}
		}

		public int GetUserId(string name)
		{
			try
			{
				return File.ReadAllLines(_usersFilePath)
					.Where(line => string.Compare(line.Split(',')[2], name, StringComparison.CurrentCultureIgnoreCase) == 0)
					.Select(line => Convert.ToInt32(line.Split(',')[2]))
					.FirstOrDefault();
			}
			catch (Exception e)
			{
				return -1;
			}
		}

		public IEnumerable<CodeAnalyzerUser> GetUsers()
		{
			throw new NotImplementedException();
		}

		public bool UserExists(string username)
		{
			throw new NotImplementedException();
		}

		public bool ShareFolder(int ownerId, string username, string folderName)
		{
			throw new NotImplementedException();
		}

		public void CreateReport(int ownerId, string filename)
		{
			throw new NotImplementedException();
		}

	}
}
