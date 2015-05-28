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
  /// Interaction logic for ChangePwdWindow.xaml
  /// </summary>
  public partial class ChangePwdWindow : Window
  {
    public ChangePwdWindow()
    {
      InitializeComponent();
    }

    private void Button_Click_Confirm(object sender, RoutedEventArgs e)
    {
      if (this.pwdBox.Password != App.currentUser._pwd)
      {
        MessageBox.Show("密码错误！");
      }
      else
      {
        this.Close();
        NewPwdWindow newPwdWindow = new NewPwdWindow();
        newPwdWindow.ShowDialog();
      }
    }

    private void Button_Click_Cancel(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
