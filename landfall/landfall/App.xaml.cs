using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices; 

namespace landfall
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      string basePath = AppDomain.CurrentDomain.BaseDirectory;
      string dataSource = basePath + "landfall.dat";
      if (!dataManager.loadFile(dataSource))
      {
        App.Current.Shutdown();
      }
      else
      {
        base.OnStartup(e);
        App.keyboardHook.KeyMaskStart();
        Taskbar.Hide();
        RunWhenStarted();
      }
    }

    public static DataManager dataManager = new DataManager();
    public static User currentUser = null;
    public static LandfallHook.KeyboardHook keyboardHook = new LandfallHook.KeyboardHook();

    public static void RunWhenStarted()
    {
      // start up when opening the computer
      RegistryKey HKLM = Registry.LocalMachine;
      RegistryKey Run = HKLM.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\");
      try
      {
        Run.SetValue("landfall", Assembly.GetExecutingAssembly().Location);
        HKLM.Close();
      }
      catch//没有权限会异常 
      {
        MessageBox.Show("设置开机启动失败！");
      }
    }

  }

}
