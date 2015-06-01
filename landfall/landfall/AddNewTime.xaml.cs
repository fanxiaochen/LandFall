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

namespace landfall
{
  /// <summary>
  /// Interaction logic for AddNewTimeWindowWindow.xaml
  /// </summary>
  public partial class AddNewTimeWindow : Window
  {
    private string userName = null;

    public AddNewTimeWindow()
    {
      InitializeComponent();

      ((this.FindName("listView")) as ListView).ItemsSource = App.dataManager.GetUsers();
    }

    private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      var item = ((FrameworkElement)e.OriginalSource).DataContext as User;
      if (item != null)
      {
        userName = item._userName;
        this.Close();
      }
    }

    public string getUserName()
    {
      return userName;
    }

    private void cancel_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
