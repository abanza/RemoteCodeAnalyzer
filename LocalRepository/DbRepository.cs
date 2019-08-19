// DbRepository.cs                                                                

using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using SharedObjects;

namespace LocalRepository
{
	public class DbRepository : IRepository
	{
		private IDbConnection Connection => new SqlConnection(ConnectionString);

		private static string ConnectionString => @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RemoteRepository;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

		public IEnumerable<CodeAnalyzerUser> GetUsers()
		{
			const string command = "select * from Users order by Username";
			return Connection.Query<CodeAnalyzerUser>(command);
		}

		public CodeAnalyzerUser GetUser(string username)
		{
			const string command = "select top 1 * from Users where username = @username order by Username";
			return Connection.Query<CodeAnalyzerUser>(command, new { username }).FirstOrDefault();
		}

		public CodeAnalyzerUser GetUser(int userId)
		{
			const string command = "select * from Users where Id = @userId";
			return Connection.Query<CodeAnalyzerUser>(command, new { userId }).FirstOrDefault();
		}

		public CodeAnalyzerUser GetUser(string username, string password)
		{
			const string command = "select top 1 * from Users where username = @username and password = @password order by Username";
			return Connection.Query<CodeAnalyzerUser>(command, new { username, password }).FirstOrDefault();
		}

		public void AddUser(CodeAnalyzerUser user)
		{
			const string command = "insert into Users(Role, Username, Password) values(@Role, @Username, @Password)";
			Connection.Query<CodeAnalyzerUser>(command,
			  new
			  {
				  user.Role,
				  user.Username,
				  user.Password
			  });
		}

		public void ClearUsers()
		{
			const string command = "delete from Users";
			Connection.Query(command);
		}

		public void ClearFolders()
		{
			const string command = "delete from Folders";
			Connection.Query(command);
		}

		public void ClearFiles()
		{
			const string command = "delete from Files";
			Connection.Query(command);
		}

		public void ClearAll()
		{
			ClearUsers();
			ClearFolders();
			ClearFiles();
		}

		public bool UserExists(string username)
		{
			var command = "select * from Users where Username = @Username";
			return Connection.Query<CodeAnalyzerUser>(command, new { username }).Any();
		}

		public void CheckIn(string username, string folderName, string filename, byte[] fileData)
		{
			var userId = GetUserId(username);
			var folderId = GetFolderId(username, folderName);

			var command = "exec CheckIn @UserId, @FolderId, @Filename, @FileData";
			Connection.Query(command, new { userId, folderId, filename, fileData });
		}

		public CodeAnalyzerFile CheckOut(string username, string folderName, string filename)
		{
			var userId = GetUserId(username);
			var folderId = GetFolderId(username, folderName);

			var command = "exec CheckOut @UserId, @FolderId, @Filename";
			return Connection.Query<CodeAnalyzerFile>(command, new { userId, folderId, filename })
			  .FirstOrDefault();
		}

		public int GetFolderId(string username, string name)
		{
			var userId = GetUserId(username);
			var command = "select top 1 Id from Folders where UserId = @UserId and Name = @Name order by Id desc";
			return Connection.Query<int>(command, new { userId, name })
			  .FirstOrDefault();
		}

		public int GetUserId(string name)
		{
			var command = "select top 1 Id from Users where Username = @Name order by Id desc";
			return Connection.Query<int>(command, new { name })
			  .FirstOrDefault();
		}

		public IEnumerable<CodeAnalyzerFolder> GetFolders()
		{
			var command = "select * from Folders order by UserId";
			return Connection.Query<CodeAnalyzerFolder>(command);
		}

		public IEnumerable<string> GetFolders(string username)
		{
			var userId = GetUserId(username);
			var command = "select Name from Folders where UserId = @UserId order by Name";
			return Connection.Query<string>(command, new { userId });
		}

		public IEnumerable<CodeAnalyzerSharedFolder> GetSharedFolders(int userId)
		{
			var command = "select b.Id, b.Name, a.OwnerId, c.username as OwnerName from SharedFolders a inner join Folders b on a.FolderId = b.Id inner join Users c ON c.Id = a.OwnerId where a.UserId = @UserId";
			return Connection.Query<CodeAnalyzerSharedFolder>(command, new { userId });
		}

		public int CreateFolder(string username, string folderName)
		{
			var userId = GetUserId(username);
			var command = "insert into Folders(UserId, Name) values(@UserId, @FolderName)";
			Connection.Query(command, new { userId, folderName });

			return 1;
		}

		public IEnumerable<CodeAnalyzerFile> GetFiles(string username, string folder)
		{
			var userId = GetUserId(username);
			var folderId = GetFolderId(username, folder);
			//var command = "select a.Id, a.Name, a.FileData as Data, b.Name as FolderName from Files a inner join Folders b on a.FolderId = b.Id where FolderId = @FolderId order by Name";
			var command = "select a.Id, a.Name, a.FileData as Data, b.Name as FolderName, u.Username as UserName from Files a inner join Folders b on a.FolderId = b.Id inner join Users u on u.Id = b.UserId where FolderId = @FolderId order by Name";
			return Connection.Query<CodeAnalyzerFile>(command, new { userId, folderId });
		}

		public IEnumerable<CodeAnalyzerFile> GetFiles(int userId, string folder)
		{
			var user = GetUser(userId);
			var folderId = GetFolderId(user.Username, folder);
			//var command = "select a.Id, a.Name, a.FileData as Data, b.Name as FolderName from Files a inner join Folders b on a.FolderId = b.Id where FolderId = @FolderId order by Name";
			var command = "select a.Id, a.Name, a.FileData as Data, b.Name as FolderName, u.Username as UserName from Files a inner join Folders b on a.FolderId = b.Id inner join Users u on u.Id = b.UserId where FolderId = @FolderId order by Name";
			return Connection.Query<CodeAnalyzerFile>(command, new { userId, folderId });
		}

		public bool ShareFolder(int ownerId, string username, string folderName)
		{
			var user = GetUser(username);
			var owner = GetUser(ownerId);
			var userId = user.Id;
			var folderId = GetFolderId(owner.Username, folderName);

			const string checkCommand = "select * from SharedFolders where ownerId=@ownerId and userId=@userId and folderId=@folderId";
			var exists = Connection.Query<int>(checkCommand, new { ownerId, userId, folderId }).FirstOrDefault();
			if (exists == 1)
				return false;

			const string command = "insert into SharedFolders(OwnerId, UserId, FolderId) values(@ownerId, @userId, @folderId)";
			Connection.Query(command, new { ownerId, userId, folderId });
			return true;
		}

		public void CreateReport(int ownerId, string filename)
		{
			//var userId = GetUserId(username);
			//var folderId = GetFolderId(username, folderName);
			var files = new List<string>();
			//ParserProcess.ParseFiles(files);


			//var command = "exec CheckOut @UserId, @FolderId, @Filename";
			//return Connection.Query<CodeAnalyzerFile>(command, new { userId, folderId, filename })
			//  .FirstOrDefault();
		}
	}
}
