﻿using System;
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
  /// Interaction logic for DeleteUserWindow.xaml
  /// </summary>
  public partial class DeleteUserWindow : Window
  {
    public DeleteUserWindow()
    {
      InitializeComponent();
    }

    private void Button_Click_Confirm(object sender, RoutedEventArgs e)
    {
      if (this.adminPwdBox.Password != App.currentUser._pwd)
      {
        MessageBox.Show("管理员密码错误！");
        return;
      }

      if (App.currentUser.DeleteUser(this.userName.Text))
      {
        MessageBox.Show("删除用户成功！");
        this.Close();
        return;
      }
    }

    private void Button_Click_Cancel(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
