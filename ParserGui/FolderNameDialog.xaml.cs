/////////////////////////////////////////////////////////////////////////
// ProjectNameDialog.xaml.cs                                           //
//                                                                     //
//                                                                     //
//                                                                     //
// Brian Voskerijian, CSE681-Software Modeling & Analysis, Spring 2018 //
/////////////////////////////////////////////////////////////////////////
//
// - Implements the project name dialog
// - This has the sole purpose of getting the project name to create
/////////////////////////////////////////////////////////////////////////

using System.Windows;

namespace ParserGui
{
  /// <summary>
  /// Interaction logic for ProjectNameDialog.xaml
  /// </summary>
  public partial class FolderNameDialog : Window
  {
    public string FolderName { get; set; }
    public FolderNameDialog()
    {
      InitializeComponent();
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
      FolderName = ProjectNameTextBox.Text;
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
