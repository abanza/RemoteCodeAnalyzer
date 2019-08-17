/////////////////////////////////////////////////////////////////////////
// MainWindow.xaml.cs                                                  //
//                                                                     //
//                                                                     //
//                                                                     //
// Brian Voskerijian, CSE681-Software Modeling & Analysis, Spring 2018 //
/////////////////////////////////////////////////////////////////////////
//
// - Implements the main window GUI for the CMA
/////////////////////////////////////////////////////////////////////////

using ServiceClient;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Path = System.IO.Path;
using SharedObjects;
using CodeAnalyzer;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CodeAnalyzer.CodeAnalyzerObjects;
using System.Xml.Linq;

namespace ParserGui
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public BasicServiceClient _Client { get; set; }
    private CodeAnalyzerUser _LoggedInUser { get; set; }
    private List<CodeAnalyzerFolder> _Folders { get; set; }
    private List<CodeAnalyzerSharedFolder> _SharedFolders { get; set; }
    private FileDetails fileDetails;

    // Encapsulate LoggedInUser to simplify logic behind logging in/out
    public CodeAnalyzerUser LoggedInUser {
      get { return _LoggedInUser; }
      set
      {
        _LoggedInUser = value;
        if (value != null)
        {
          // User is logged in, don't allow logging in again and show logged in username
          LogoutButton.Visibility = Visibility.Visible;
          LoggedInLabel.Content = $"Logged in as [{value.Username}]";
          LoginButton.IsEnabled = false;
          RefreshFolderList(); // Get folders for this user
          RefreshSharedFolderList(); // Get folders for this user
        }
        else
        {
          // User is logged out, so allow logging in again
          LogoutButton.Visibility = Visibility.Collapsed;
          LoggedInLabel.Content = string.Empty;
          LoginButton.IsEnabled = true;
        }
      }
    }

    public MainWindow()
    {
      InitializeComponent();

      // Initialize remote repository client
      string url = "http://localhost:8080/RemoteRepositoryService"; // Must match URL specified in host
            _Client = new ServiceClient.BasicServiceClient(url);

      UsernameTextBox.Focus();

      this.Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      if(_Client.GetNumUsers() <= 0)
      {
        RegisterTab.Visibility = Visibility.Visible;
      }

      // TODO: REMOVE AFTER TESTS
      //UsernameTextBox.Text = "dev";
      //PasswordTextBox.Text = "dev";
      //LoginButton_Click(this, null);
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
      var usernameText = UsernameTextBox.Text;
      var passwordText = PasswordTextBox.Text;
      //var user = _Client.GetUser(usernameText);
      CodeAnalyzerUser user = null;
      try
      {
        user = _Client.GetUser(usernameText, passwordText);
      }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }

      // Check if this user exists
      if (user == null)
      {
        RegisterTab.Visibility = Visibility.Collapsed;
        BrowseTab.Visibility = Visibility.Collapsed;
        LoginTab.Visibility = Visibility.Visible;

        MessageBox.Show("No user matching that username was found.", "No user found", MessageBoxButton.OK);
      }
      else 
      {
        UsernameTextBox.Text = "";
        PasswordTextBox.Text = "";

        LoginTab.Visibility = Visibility.Collapsed;
        BrowseTab.Visibility = Visibility.Visible;
        BrowseTab.Focus();
        if (user.Role == "Administrator")
          RegisterTab.Visibility = Visibility.Visible;
      }
      LoggedInUser = user;
    }

    private void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
      var usernameText = RegisterUsernameTextBox.Text;
      var password = RegisterPasswordTextBox.Text;

      if(string.IsNullOrWhiteSpace(usernameText) || string.IsNullOrWhiteSpace(password))
      { 
        MessageBox.Show("Please enter a valid username and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }


      var user = _Client.GetUser(usernameText);

      if (user == null)
      {
        var role = ((ComboBoxItem)RegisterRoleComboBox.SelectedItem).Content.ToString();

        // User does not exist, create a new one
        _Client.CreateUser(role, usernameText, password);
        MessageBox.Show($"Registered as new user [{usernameText}]", "New User Created", MessageBoxButton.OK);
        //user = _Client.GetUser(usernameText, password);
        //MessageBox.Show($"Registered and logged in as new user [{usernameText}]", "New User Created", MessageBoxButton.OK);
        //LoggedInUser = user;

        RegisterUsernameTextBox.Text = "";
        RegisterPasswordTextBox.Text = "";
        RegisterRoleComboBox.SelectedIndex = 0;
        LoginTab.Visibility = Visibility.Collapsed;
        BrowseTab.Visibility = Visibility.Visible;
      }
      else
      {
        // User does exist, deny registration
        MessageBox.Show($"Cannot register this username because a user with this name already exists.", "User Already Exists", MessageBoxButton.OK);
        LoginTab.Visibility = Visibility.Visible;
        BrowseTab.Visibility = Visibility.Collapsed;
      }
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
      RegisterTab.Visibility = Visibility.Collapsed;
      BrowseTab.Visibility = Visibility.Collapsed;
      LoginTab.Visibility = Visibility.Visible;
      LoginTab.Focus();

      LoggedInUser = null;
    }

    private void NewFolderButton_Click(object sender, RoutedEventArgs e)
    {
      var folderNameDialog = new FolderNameDialog();
      folderNameDialog.ShowDialog();
      if (folderNameDialog.FolderName != null)
      {
        // Project name was entered and dialog was not closed out, so create the project
        _Client.CreateFolder(LoggedInUser.Username, folderNameDialog.FolderName);
        RefreshFolderList();
      }
    }

    private void RefreshFolderList()
    {
      // Get folders from client
      if(_LoggedInUser == null)
        return;

      if(_LoggedInUser.IsAdmin())
      {
        var data = _Client.BrowseFolders();
        _Folders = data.ToList();
        FolderListBox.ItemsSource = _Folders.Select(x => x.Name).ToList();
      }
      else
      {
        FolderListBox.ItemsSource = _Client.BrowseFolders(_LoggedInUser.Username);
      }

      // Set listbox to first folder in the list
      if (FolderListBox.Items.Count > 0)
      {
        FolderListBox.SelectedIndex = 0;
      }
    }

    private void RefreshSharedFolderList()
    {
      // Get folders from client
      if (_LoggedInUser == null)
        return;

      _SharedFolders = _Client.BrowseSharedFolders(_LoggedInUser.Id).ToList();
      SharedFolderListBox.ItemsSource = _SharedFolders.Select(x => x.Name).ToList();

      // Set listbox to first folder in the list
      if (SharedFolderListBox.Items.Count > 0)
      {
        SharedFolderListBox.SelectedIndex = 0;
      }
    }

    private void FolderListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RefreshFileList();
    }

    private void RefreshFileList()
    {
      FilesLabel.Content = "Files";

      if (FolderListBox.SelectedItems.Count < 1)
      {
        FileListBox.ItemsSource = null;
        return;
      }

      var selectedFolder = FolderListBox.SelectedItems[0] as string;
      if (FolderListBox.SelectedItems[0] != null)
      {
        // Update share button state
        ShareFolderButton.IsEnabled = true;

        // Update files
        var userId = LoggedInUser.Id;
        if (_LoggedInUser.IsAdmin())
        {
          userId = _Folders[FolderListBox.SelectedIndex].UserId;
        }

        FileListBox.ItemsSource = _Client.BrowseFiles(userId, selectedFolder);
      }
      else
      {
        ShareFolderButton.IsEnabled = false;
      }
    }

    private void CheckInButton_Click(object sender, RoutedEventArgs e)
    {
      // Ensure a folder is selected
      if (FolderListBox.SelectedItems.Count == 0)
      {
        return;
      }

      // Get selected folder name
      var selectedFolderName = FolderListBox.SelectedItems[0] as string;

      // Open a file dialog
      var fileDialog = new OpenFileDialog
      {
        Filter = "C# Files|*.cs" // Only allow C# files to be checked in
      };
      var dialogResult = fileDialog.ShowDialog();

      // Check in selected files (may be more than one)
      if (dialogResult.HasValue && dialogResult.Value)
      {
        var selectedFiles = fileDialog.OpenFiles();
        var filenames = fileDialog.FileNames;

        for (var i = 0; i < selectedFiles.Length; i++)
        {
          var filename = Path.GetFileName(filenames[i]);
          _Client.CheckIn(_LoggedInUser.Username, selectedFolderName, filename, selectedFiles[i]);
        }
      }

      /* Wait for check in to complete before refreshing the file list
       Not good to block UI thread, but this is makes the UI behave more predictable to the user (new file shows up automatically).
       The delay is very short so it is not noticeable to the user. */ 
      Thread.Sleep(100);
      RefreshFileList();
    }

    private void CheckOutButton_Click(object sender, RoutedEventArgs e)
    {
      // Esure a file is selected
      if (FileListBox.SelectedItems.Count == 0)
      {
        return;
      }

      // Get selected folder/file name
      var selectedFile = FileListBox.SelectedItems[0] as CodeAnalyzerFile;

      // Open a save as dialog
      var saveAsDialog = new SaveFileDialog
      {
        Filter = "C# Files|*.cs",
        FileName = selectedFile?.Name ?? throw new InvalidOperationException(),
        DefaultExt = ".cs",
        
      };
      var dialogResult = saveAsDialog.ShowDialog();

      // Save file (may be more than one)
      if (dialogResult.HasValue && dialogResult.Value)
      {
        using (var hostFile = _Client.CheckOut(_LoggedInUser.Username, selectedFile?.Name, selectedFile?.FolderName))
        using (var clientFile = saveAsDialog.OpenFile())
        {
          hostFile.CopyTo(clientFile);
        }
      }
    }

    private void ShareFolderButton_Click(object sender, RoutedEventArgs e)
    {
      var selectedFolder = FolderListBox.SelectedItem as string;
      if (string.IsNullOrWhiteSpace(selectedFolder))
        return;

      var ownerId = LoggedInUser.Id;
      if(LoggedInUser.IsAdmin())
      {
        ownerId = _Folders[FolderListBox.SelectedIndex].UserId;
      }

      var usernameDialog = new UsernameDialog();
      usernameDialog.ShowDialog();
      if (usernameDialog.Username != null)
      {
        if(_Client.ShareFolder(ownerId, usernameDialog.Username, selectedFolder))
        {
          MessageBox.Show($"Shared folder {selectedFolder} with {usernameDialog.Username}", "Shared Folder", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
          MessageBox.Show($"Folder is already shared with {usernameDialog.Username}", "Shared Folder", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
    }

    private void SharedFolderListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      RefreshSharedFolderFiles();
    }

    private void SharedFolderListBox_GotFocus(object sender, RoutedEventArgs e)
    {
      RefreshSharedFolderFiles();
    }

    private void RefreshSharedFolderFiles()
    {
      FilesLabel.Content = "Shared Files";

      if (SharedFolderListBox.SelectedItems.Count < 1)
        return;

      var selectedFolder = SharedFolderListBox.SelectedItems[0] as string;
      if (SharedFolderListBox.SelectedItems[0] != null)
      {
        // Update files
        FileListBox.ItemsSource = _Client.BrowseFiles(_SharedFolders[SharedFolderListBox.SelectedIndex].OwnerName, selectedFolder);
      }
    }

    private void FileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var selectedFile = FileListBox.SelectedItem as CodeAnalyzerFile;
      if (selectedFile == null)
        return;

      var ownerUsername = selectedFile.Username;
      var selectedFolder = FolderListBox.SelectedItem as string;

      // Create a memory stream from the file so we can feed it to the parser
      var fileStream = _Client.CheckOut(ownerUsername, selectedFile.Name, selectedFolder);
      var stream = new MemoryStream();
      fileStream.CopyTo(stream);
      stream.Position = 0;

      var list = new List<Stream>();
      list.Add(stream);

      // Generate report
      var reportFilename = (Path.GetFileNameWithoutExtension(selectedFile.Name) + "-report.xml"); ;
      var xmlFileStream = File.Create(reportFilename);
      var parsed = ParserProcess.ParseFiles(list);
      try
      {
        var xs = new XmlSerializer(typeof(List<ElementProperty>));
        xs.Serialize(xmlFileStream, parsed.ToList());
        xmlFileStream.Close();
      }
      catch(Exception ex)
      {
        MessageBox.Show("There was a problem writing the XML report file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      MessageBox.Show("Report was successfully written to file.", "XML Report", MessageBoxButton.OK, MessageBoxImage.Information);

      //var doc = new XmlDocument();
      //doc.Load("sample.xml");
      //var root = doc.DocumentElement;

      if (fileDetails == null)
      {
        fileDetails = new FileDetails
        {
          ParentWindow = this,
          ReportFileName = reportFilename
        };

        fileDetails.Show();
      }
    }

    public void ResetFileDetails()
    {
      fileDetails = null;
    }
  }
}



/*
Project #4 – Remote Code Analyzer

Purpose:

This project develops a Remote Code Repository that uses the analysis engine you developed in Project #2. 
It is charged with committing/pulling source code files to/from a remote server. It must do this for 
source code file sets that may reside on one or more directories in local or remote machines. 
This will require you to develop both server and client programs where the server may reside 
on a remote machine.

The Remote Code Repository can be accesses only with valid credentials and supports two user roles: 
developer and administrator. As a developer, user can upload a new set of files to the remote server 
and his files are analyzed for quality and stored for later use. Developer can see only the files that 
they own and those for which they are granted permission by the owner. The user with the administrator 
role can see any directory that resides on the server.

In this project you will use the prototype from Project 3 and provide more GUI to use all of the 
services of Project 2. You need to design your GUI in such a way that the calculated metric values can 
be presented to the user in an elegant way.

Requirements:

The Dependency analyzer:
1. Shall compile and run with Visual Studio 2017.

2. Shall allow users to enter the system with a user name and password and only a user with admin 
privileges shall be able to create new users.

3. Shall provide the following options for a user:
a) Uploading new files as a directory.
b) Displaying the directories owned by the user (all directories if admin).
c) Displaying the directories shared with the user with the owner information.
d) Downloading a file or a complete directory.
e) Providing comments for a particular file or a directory and displaying the comments.
f) Searching in directories (with a file or directory name)

4. Shall display the files, class names or function names with an effective way to visualize their 
quality attributes (possibly color coded) and shall provide a way to expend the view 
hierarchically as user clicks the items.

5. Shall create an XML file that captures the file and directory properties.

6. Shall create a new file if the file or directory with the same name exists and create an XML file 
that captures the file properties for the new file as well.

7. Shall display, in a Graphical User Interface, the content of the file hierarchically. 
Upon clicking a file name, Shall provide all class names in that file by making LINQ queries into the XML created 
for Requirement #5 and 6 with a color code that reflects their maintainability. Upon clicking a class name, Shall 
provide all function names and their quality properties.

8. Shall provide an option to share a selected file or an entire directory with another user which can be selected 
from a list of users on GUI. Shall provide a way to enter comments regarding a selected directory or file.

9. Shall provide a client and server side code. Only the client side shall have a GUI.

10. Shall use Windows Communication Foundation (WCF) for all communication between client and server processes 
or machines. Communication shall be based on message-passing.

11. Shall use Windows Presentation Foundation (WPF) for all User display. Shall use child components to implement 
most of the client processing. The GUI should focus on Presentation, not processing.
*/