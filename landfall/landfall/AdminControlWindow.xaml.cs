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
  /// Interaction logic for AdminControlWindow.xaml
  /// </summary>
  public partial class AdminControlWindow : Window
  {
    public AddUserWindow addUserWindow = null;
    public DeleteUserWindow deleteUserWindow = null;
    public ClearAllWindow clearAllWindow = null;

    public AdminControlWindow()
    {
      InitializeComponent();

      ((this.FindName("listView")) as ListView).ItemsSource = App.dataManager.GetUsers();
    }

    private void Button_Click_Add(object sender, RoutedEventArgs e)
    {
      if (addUserWindow != null)
        addUserWindow.Close();

      addUserWindow = new AddUserWindow();
      addUserWindow.ShowDialog();
    }

    private void Button_Click_Delete(object sender, RoutedEventArgs e)
    {
      if (deleteUserWindow != null)
        deleteUserWindow.Close();

      deleteUserWindow = new DeleteUserWindow();
      deleteUserWindow.ShowDialog();
    }

    private void Button_Click_Clear(object sender, RoutedEventArgs e)
    {
      if (clearAllWindow != null)
        clearAllWindow.Close();

      clearAllWindow = new ClearAllWindow();
      clearAllWindow.ShowDialog();
    }
  }
}
