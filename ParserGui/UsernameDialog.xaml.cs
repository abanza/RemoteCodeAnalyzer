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
  /// Interaction logic for UsernameDialog.xaml
  /// </summary>
  public partial class UsernameDialog : Window
  {
    public string Username { get; set; }
    public UsernameDialog()
    {
      InitializeComponent();
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
      Username = UsernameTextBox.Text;
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }
  }
}
