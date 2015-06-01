using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;

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
      }
    }

    public static DataManager dataManager = new DataManager();
    public static User currentUser = null;
    public static LandfallHook.KeyboardHook keyboardHook = new LandfallHook.KeyboardHook();
   
  }

}
