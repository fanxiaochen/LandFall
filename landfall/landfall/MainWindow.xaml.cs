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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;

namespace landfall
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private NotifyIcon notifyIcon = null;

    private ChangePwdWindow changePwdWindow = null;

    private AdminControlWindow adminControlWindow = null;

    public MainWindow()
    {
      InitializeComponent();
    }

    public void InitNotifyIcon()
    {
      notifyIcon = new NotifyIcon();
      notifyIcon.BalloonTipText = "landfall runnning...";
      notifyIcon.Text = "landfall";
      notifyIcon.Icon = new System.Drawing.Icon("landfall.ico");
      notifyIcon.Visible = true;
      notifyIcon.ShowBalloonTip(2000);

      System.Windows.Forms.MenuItem itemControlPanel = new System.Windows.Forms.MenuItem("控制面板");
      System.Windows.Forms.MenuItem itemChangePwd = new System.Windows.Forms.MenuItem("修改密码");
      System.Windows.Forms.MenuItem itemAbout = new System.Windows.Forms.MenuItem("关于landfall");

      System.Windows.Forms.MenuItem[] children = new System.Windows.Forms.MenuItem[] { itemControlPanel, itemChangePwd, itemAbout };
      notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(children);

      itemControlPanel.Click += new System.EventHandler(Controlpanel_Click);
      itemChangePwd.Click += new System.EventHandler(Changepwd_Click);
      itemAbout.Click += new System.EventHandler(About_Click);
    }

    public void Controlpanel_Click(object sender, EventArgs e)
    {
      if (App.currentUser._userName == "admin")
      {
        if (adminControlWindow != null)
          adminControlWindow.Close();

        adminControlWindow = new AdminControlWindow();
        adminControlWindow.ShowDialog();
      }
      else
      {

      }
    }

    public void Changepwd_Click(object sender, EventArgs e)
    {
      if (changePwdWindow != null)
        changePwdWindow.Close();

      changePwdWindow = new ChangePwdWindow();
      changePwdWindow.ShowDialog();
    }

    public void About_Click(object sender, EventArgs e)
    {

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      string username = userName.Text;
      string pwd = pwdBox.Password;

      if (App.dataManager.Login(username, pwd))
      {
        this.Visibility = Visibility.Hidden;
        InitNotifyIcon();
      }
      else
      {
        System.Windows.MessageBox.Show("用户名或密码不正确！");
      }
    }
  }
}
