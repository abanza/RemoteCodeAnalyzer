using CodeAnalyzer.CodeAnalyzerObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ParserGui
{
  /// <summary>
  /// Interaction logic for FileDetails.xaml
  /// </summary>
  public partial class FileDetails : Window
  {
    public MainWindow ParentWindow { get; set; }
    public string ReportFileName { get; set; }

    public FileDetails()
    {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      var root = XElement.Load(ReportFileName);
      var classes = FindClasses(root);
      var i = 0;

      var classesList = classes.ToList();

      //DataSet dataSet = new DataSet();
      //dataSet.ReadXml(ReportFileName);
      //DataView dataView = new DataView(dataSet.Tables[0]);
      //dataGrid1.ItemsSource = dataView;

      //FileInfoHierarchy.ItemsSource = classesList;

      var listData = new List<Data>();
      foreach (var c in classesList)
      {
        var data = new Data();
        var className = (string)c.Element("Name");
        data.data = c;

        var functions = FindFunctionsByClass(root, className);
        foreach (var f in functions)
        {
          var funcData = new Data();
          funcData.data = f;
          data.functions.Add(funcData);
          //var fNode = new TreeViewItem();
          //var functionName = (string)f.Element("Name");
          //fNode.Header = functionName;

          //cNode.Items.Add(fNode);
        }

        listData.Add(data);
      }
      FileInfoHierarchy.DataContext = this;
      FileInfoHierarchy.ItemsSource = listData;//      FileInfoHierarchy.ItemsSource = ;

    
    //var cNode = new TreeViewItem();
    //  var className = (string)c.Element("Name");
    //  cNode.Header = c;
    //  //cNode.Header = className;

    //  var functions = FindFunctionsByClass(root, className);
    //  foreach (var f in functions)
    //  {
    //    var fNode = new TreeViewItem();
    //    var functionName = (string)f.Element("Name");
    //    fNode.Header = functionName;

    //    cNode.Items.Add(fNode);
    //  }

    //  //FileInfoHierarchy.Items.Add(cNode);
    //}

    // const string[] attributes = [ "LinesOfCode", "CyclomaticComplexity", "Cohesion", "MIndex"];

    //for (var )
    //{
    //  if()
    //  var label = new Label()Elements("ElementProperty") where (string)el.Element("Type") == "class" select el;
    }

    private IEnumerable<XElement> FindClasses(XElement root)
    {
      return from el in root.Elements("ElementProperty") where (string)el.Element("Type") == "class" select el;
    }

    private IEnumerable<XElement> FindFunctionsByClass(XElement root, string className)
    {
      return from el in root.Elements("ElementProperty") where (string)el.Element("Type") == "function" && (string)el.Element("FunctionClass") == className select el;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      ParentWindow.ResetFileDetails();
    }

    private void FileInfoHierarchy_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var el = (Data)e.NewValue;
      var data = el.data;
      CohesionLabel.Content = data.Element("Cohesion").Value;
      CyclomaticCompLabel.Content = data.Element("CyclomaticComplexity").Value;
      LinesOfCodeLabel.Content = data.Element("LinesOfCode").Value;
      MIndexLabel.Content = data.Element("MIndex").Value;
    }
  }

  public class Data
  {
    public XElement data { get; set; }
    public List<Data> functions { get; set; }
    public string name { get; set; }

    public Data()
    {
      functions = new List<Data>();
    }
  }
}
