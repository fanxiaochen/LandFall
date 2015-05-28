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
  /// Interaction logic for NewPwdWindow.xaml
  /// </summary>
  public partial class NewPwdWindow : Window
  {
    public NewPwdWindow()
    {
      InitializeComponent();
    }

    private void Button_Click_Confirm(object sender, RoutedEventArgs e)
    {
      if (this.pwdBox1.Password != this.pwdBox2.Password)
      {
        MessageBox.Show("密码输入不一致！");
        return;
      }
      App.currentUser.ChangePassword(this.pwdBox1.Password);
      this.Close();
      MessageBox.Show("修改成功！");
    }

    private void Button_Click_Cancel(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
