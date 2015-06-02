using System;
using System.Collections.Generic;
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
using System.Data;

namespace landfall
{
  /// <summary>
  /// Interaction logic for UserControlWindow.xaml
  /// </summary>
  public partial class UserControlWindow : Window
  {
    public DataSet dataSet = new DataSet();

    public UserControlWindow()
    {
      InitializeComponent();

      App.dataManager.InitTimeTable();

      dataSet.Tables.Add(App.dataManager.getDataTable());
      dataGrid.ItemsSource = dataSet.Tables[0].DefaultView;

      App.dataManager.UpdateTimeTableFromSingle();
    }
  }
}
