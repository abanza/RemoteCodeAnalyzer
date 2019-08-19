// IRemoteRepositoryService.cs                                                    

using SharedObjects;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace RemoteRepositoryService
{
	[ServiceContract(Namespace = "RemoteRepositoryService")]
	public interface IRemoteRepositoryService
	{
		// Creates a user if one does not already exist
		[OperationContract]
		bool CreateUser(string role, string username, string password);

		// Creates a folder if one does not already exist
		[OperationContract]
		bool CreateFolder(string username, string folderName);

		// Gets the specified user if they exist
		[OperationContract]
		CodeAnalyzerUser GetUser(string username);

		// Gets the specified user if they exist
		[OperationContract]
		CodeAnalyzerUser GetUserByPassword(string username, string password);

		// Gets the number of users in the db
		[OperationContract]
		int GetNumUsers();

		// Returns a list of folders for all users
		[OperationContract]
		IEnumerable<CodeAnalyzerFolder> BrowseAllFolders();

		// Returns a list of folders under the specified user
		[OperationContract]
		IEnumerable<string> BrowseFolders(string username);

		// Returns a list of folders under the specified user
		[OperationContract]
		IEnumerable<CodeAnalyzerSharedFolder> BrowseSharedFolders(int userId);

		// Returns a list of files under the specified folder
		[OperationContract]
		IEnumerable<CodeAnalyzerFile> BrowseFiles(string username, string folder);

		// Returns a list of files under the specified folder and user id
		[OperationContract]
		IEnumerable<CodeAnalyzerFile> BrowseFilesByUserId(int userId, string folder);

		// Shares a folder with another user
		[OperationContract]
		bool ShareFolder(int ownerId, string username, string folderName);

		// Checks in the specified file
		[OperationContract(IsOneWay = true)]
		void CheckIn(CheckInData data);

		// Checks out the specified file
		[OperationContract]
		Stream CheckOut(string username, string filename, string folder);

		// Creates a report for the specified file
		[OperationContract]
		void CreateReport(int ownerId, string filename);
	}
}
