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
using System.Timers;
using System.Diagnostics;
using System.Threading;

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

    private UserControlWindow userControlWindow = null;

    public System.Timers.Timer timer = new System.Timers.Timer(1000 * 60);
    private bool timeToLogout = false;

    private System.Timers.Timer taskMgrTimer = new System.Timers.Timer(100);


    public MainWindow()
    {
      InitializeComponent();

      InitTimer();

      //this.Show();
      //if (this.WindowState == WindowState.Minimized)
      //{
      //  this.WindowState = WindowState.Maximized;
      //}

      //this.Activate();
      //this.Topmost = true;
      //this.userName.Focus();
    }

    protected override void OnContentRendered(EventArgs e)
    {
      int activateCount = 0;
      const int CountMax = 100;
      while (!this.Activate() && activateCount < CountMax) 
      {
        Thread.Sleep(1000);
        activateCount++; 
      }
      //if (activateCount == CountMax)
      //{
      //  System.Windows.MessageBox.Show("无法获得系统当前焦点！");
      //}

      this.Activate();
      this.Topmost = true;
      this.userName.Focus();

      base.OnContentRendered(e);
    }

    public void InitTimer()
    {
      timer.Elapsed += OnTimedEvent;
      timer.Enabled = true;

      taskMgrTimer.Elapsed += OnTaskMgrTimedEvent;
      taskMgrTimer.Enabled = true;
    }

    public void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
      string strday = DateTime.Now.DayOfWeek.ToString();
      int day = App.dataManager.ConvertDayOfWeek(strday);
      int hour = DateTime.Now.Hour;
      int minute = DateTime.Now.Minute;

      // for administrator
      if (App.currentUser._timeIntervals == null)
        return;

      if (timeToLogout)
      {
        App.dataManager.getRecorder().GetEndTime();
        App.dataManager.getRecorder().WriteRecord();

        System.Windows.Forms.Application.Restart();
        Environment.Exit(0); // kill current landfall app...
        // App.Current.Shutdown() doesn't work here...
        return;
      }

      bool validTimeFlag = false;

      foreach (TimeInterval ti in App.currentUser._timeIntervals)
      {
        if (ti.day == day && ti.start <= hour && ti.end > hour)
        {
          validTimeFlag = true;

          int timeEps = ti.end * 60 - (hour * 60 + minute);
          if (timeEps <= 15 && timeEps > 0)
          {
            if (timeEps == 1)
              timeToLogout = true;

            string msg = String.Format("距离重新锁屏不到{0}分钟， 请处理好个人数据！\n如果想继续使用，请联系管理员！", timeEps.ToString());
            notifyIcon.BalloonTipText = msg;
            notifyIcon.ShowBalloonTip(30000);
           // System.Windows.MessageBox.Show(msg);
            return;
          }
        }
      }

      if (!validTimeFlag)
        timeToLogout = true;
    }

    public void OnTaskMgrTimedEvent(Object source, ElapsedEventArgs e)
    {
      Process[] ps = Process.GetProcesses();

      foreach(Process p in ps)
      {
        try
        {
          if (p.ProcessName.ToLower().Trim() == "taskmgr")
          {
            p.Kill();
            return;
          }
        }
        catch
        {
          System.Windows.MessageBox.Show("屏蔽任务管理器失败！");
          return;
        }
      }

    }

    public void InitNotifyIcon()
    {
      if (notifyIcon != null)
        return;

      string basePath = AppDomain.CurrentDomain.BaseDirectory;
      string dataSource = basePath + "landfall.ico";

      notifyIcon = new NotifyIcon();
      notifyIcon.BalloonTipText = "landfall runnning...";
      notifyIcon.Text = "landfall";
      notifyIcon.Icon = new System.Drawing.Icon(dataSource);
      notifyIcon.Visible = true;
      notifyIcon.ShowBalloonTip(2000);

      System.Windows.Forms.MenuItem itemControlPanel = new System.Windows.Forms.MenuItem("控制面板");
      System.Windows.Forms.MenuItem itemChangePwd = new System.Windows.Forms.MenuItem("修改密码");
      System.Windows.Forms.MenuItem itemLogout = new System.Windows.Forms.MenuItem("注销用户");
      System.Windows.Forms.MenuItem itemAbout = new System.Windows.Forms.MenuItem("关于landfall");

      System.Windows.Forms.MenuItem[] children = new System.Windows.Forms.MenuItem[] { itemControlPanel, itemChangePwd, itemLogout, itemAbout };
      notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(children);

      itemControlPanel.Click += new System.EventHandler(Controlpanel_Click);
      itemChangePwd.Click += new System.EventHandler(Changepwd_Click);
      itemLogout.Click += new System.EventHandler(Logout_Click);
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
        if (userControlWindow != null)
          userControlWindow.Close();

        userControlWindow = new UserControlWindow();
        userControlWindow.ShowDialog();
      }
    }

    public void Changepwd_Click(object sender, EventArgs e)
    {
      if (changePwdWindow != null)
        changePwdWindow.Close();

      changePwdWindow = new ChangePwdWindow();
      changePwdWindow.ShowDialog();
    }

    // restart application
    public void Logout_Click(object sender, EventArgs e)
    {
      App.dataManager.getRecorder().GetEndTime();
      App.dataManager.getRecorder().WriteRecord();

      System.Windows.Forms.Application.Restart();
      Environment.Exit(0);
    }

    public void About_Click(object sender, EventArgs e)
    {
      System.Windows.MessageBox.Show("I learned the value of hard work by working hard.");
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      string username = userName.Text;
      string pwd = pwdBox.Password;

      int loginFlag = App.dataManager.Login(username, pwd);
      if (loginFlag == 0)
      {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string dataSource = basePath + "landfall.txt";
        App.dataManager.getRecorder().GetRecordFile(dataSource);
        App.dataManager.getRecorder().GetDate();
        App.dataManager.getRecorder().GetStartTime();

        App.keyboardHook.KeyMaskStop();

        this.Visibility = Visibility.Hidden;
        this.userName.Text = null;
        this.pwdBox.Password = null;

        timer.Start();

        InitNotifyIcon();
      }
      else if (loginFlag == -1)
      {
        System.Windows.MessageBox.Show("用户名或密码不正确！");
      }
      else
      {
        System.Windows.MessageBox.Show("无法在此时间登陆！");
      }
    }
  }
}
