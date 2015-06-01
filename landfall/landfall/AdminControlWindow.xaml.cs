using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Data;


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

    public DataSet dataSet = new DataSet();
    public DataTable timeDt = new DataTable("Time Table");

    public AdminControlWindow()
    {
      InitializeComponent();

      InitTimeTable();

      ((this.FindName("listView")) as ListView).ItemsSource = App.dataManager.GetUsers();

      dataSet.Tables.Add(timeDt);
      dataGrid.ItemsSource = dataSet.Tables[0].DefaultView;

    }

    public void InitTimeTable()
    {
      DataColumn times = new DataColumn("时间");
      DataColumn mon = new DataColumn("周一");
      DataColumn tus = new DataColumn("周二");
      DataColumn wed = new DataColumn("周三");
      DataColumn thu = new DataColumn("周四");
      DataColumn fri = new DataColumn("周五");
      DataColumn sat = new DataColumn("周六");
      DataColumn sun = new DataColumn("周日");

      timeDt.Columns.Add(times);
      timeDt.Columns.Add(mon);
      timeDt.Columns.Add(tus);
      timeDt.Columns.Add(wed);
      timeDt.Columns.Add(thu);
      timeDt.Columns.Add(fri);
      timeDt.Columns.Add(sat);
      timeDt.Columns.Add(sun);

      timeDt.Rows.Add("08-10", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("10-12", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("12-14", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("14-16", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("16-18", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("18-20", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("20-22", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("22-24", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("00-02", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("02-04", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("04-06", " ", " ", " ", " ", " ", " ", " ");
      timeDt.Rows.Add("06-08", " ", " ", " ", " ", " ", " ", " ");

      //TimeTableFromUsers();

    }

    public void TimeTableFromUsers()
    {
      ObservableCollection<User> users = App.dataManager.GetUsers();
      foreach (User user in users)
      {
        foreach (TimeInterval ti in user._timeIntervals)
        {
          int col = ti.day;
          int row = -1;
          if (ti.start >= 8)
          {
            row = (ti.start - 8) / 2;
          }
          else
          {
            row = (ti.start + 24 - 8) / 2;
          }

          timeDt.Rows[row][col] = user._userName;
        }
      }
    }

    public void UsersFromTimeTable()
    {
      App.dataManager.ClearTimeIntervals();
      ObservableCollection<User> users = App.dataManager.GetUsers();

      // 12 time intervals and 1 column for times, 7 columns for days
      for (int row = 0; row < 12; ++row)
      {
        for (int col = 1; col < 8; ++col)
        {
          string userName = timeDt.Rows[row][col] as string;
          if (userName == " " )
            continue;

          int index = App.dataManager.FindUserIndex(userName);
          if (index != -1)
          {
            int day = col;
            int start = (row * 2 + 8) < 24 ? (row * 2 + 8) : (row * 2 + 8 - 24);
            int end = start + 2;

            TimeInterval timeInterval = new TimeInterval();
            timeInterval.day = day;
            timeInterval.start = start;
            timeInterval.end = end;
            users[index]._timeIntervals.Add(timeInterval);
          }
        }
      }
    }

    private void Table_Click_Add(object sender, RoutedEventArgs e)
    {
      AddNewTimeWindow addNewTimeWindow = new AddNewTimeWindow();
      addNewTimeWindow.ShowDialog();
      string addName = addNewTimeWindow.getUserName();

      int rowIndex = dataGrid.Items.IndexOf(dataGrid.CurrentItem);
      int colIndex = this.dataGrid.SelectedCells[0].Column.DisplayIndex;
      timeDt.Rows[rowIndex][colIndex] = addName;
      UsersFromTimeTable();
    }

    private void Table_Click_Delete(object sender, RoutedEventArgs e)
    {
      int rowIndex = dataGrid.Items.IndexOf(dataGrid.CurrentItem);
      int colIndex = this.dataGrid.SelectedCells[0].Column.DisplayIndex;
      timeDt.Rows[rowIndex][colIndex] = " ";
      UsersFromTimeTable();
    }

    private void Table_Click_Clear(object sender, RoutedEventArgs e)
    {
      // 12 time intervals and 1 column for times, 7 columns for days
      for (int row = 0; row < 12; ++row)
      {
        for (int col = 1; col < 8; ++col)
        {
          timeDt.Rows[row][col] = " ";
        }
      }

      UsersFromTimeTable();
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
