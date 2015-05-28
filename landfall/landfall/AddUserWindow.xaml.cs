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
  /// Interaction logic for AddUserWindow.xaml
  /// </summary>
  public partial class AddUserWindow : Window
  {
    public AddUserWindow()
    {
      InitializeComponent();
    }

    private void Button_Click_Confirm(object sender, RoutedEventArgs e)
    {
      User newUser = new User();
      newUser._userName = this.userName.Text;
      newUser._pwd = this.pwdBox.Password;

      if (App.currentUser.AddUser(newUser))
        this.Close();

    }

    private void Button_Click_Cancel(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
