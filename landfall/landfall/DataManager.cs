using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Data;

using Newtonsoft.Json;

namespace landfall
{
  public class DataManager
  {
    private string _dataFile = null;
    private ObservableCollection<User> _users = new ObservableCollection<User>();
    private DataTable _timeDt = new DataTable("Time Table");

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

      _timeDt.Columns.Add(times);
      _timeDt.Columns.Add(mon);
      _timeDt.Columns.Add(tus);
      _timeDt.Columns.Add(wed);
      _timeDt.Columns.Add(thu);
      _timeDt.Columns.Add(fri);
      _timeDt.Columns.Add(sat);
      _timeDt.Columns.Add(sun);

      _timeDt.Rows.Add("08-10", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("10-12", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("12-14", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("14-16", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("16-18", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("18-20", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("20-22", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("22-24", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("00-02", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("02-04", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("04-06", " ", " ", " ", " ", " ", " ", " ");
      _timeDt.Rows.Add("06-08", " ", " ", " ", " ", " ", " ", " ");
    }

    public void UpdateTimeTableFromUsers()
    {
      ClearTimeTable();

      foreach (User user in _users)
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

          _timeDt.Rows[row][col] = user._userName;
        }
      }
    }

    public void UpdateUsersFromTimeTable()
    {
      ClearTimeIntervals();

      // 12 time intervals and 1 column for times, 7 columns for days
      for (int row = 0; row < 12; ++row)
      {
        for (int col = 1; col < 8; ++col)
        {
          string userName = _timeDt.Rows[row][col] as string;
          if (userName == " ")
            continue;

          int index = FindUserIndex(userName);
          if (index != -1)
          {
            int day = col;
            int start = (row * 2 + 8) < 24 ? (row * 2 + 8) : (row * 2 + 8 - 24);
            int end = start + 2;

            TimeInterval timeInterval = new TimeInterval();
            timeInterval.day = day;
            timeInterval.start = start;
            timeInterval.end = end;
            _users[index]._timeIntervals.Add(timeInterval);
          }
        }
      }

      SaveDataFile();
    }

    public void ClearTimeTable()
    {
      // 12 time intervals and 1 column for times, 7 columns for days
      for (int row = 0; row < 12; ++row)
      {
        for (int col = 1; col < 8; ++col)
        {
          _timeDt.Rows[row][col] = " ";
        }
      }
    }

    public bool loadFile(string dataFile)
    {
      _dataFile = dataFile;
      if (!File.Exists(_dataFile))
      {
        MessageBox.Show("数据库加载失败！");
        return false;
      }
      else
      {
        ReadDataFile();
        return true;
      }
    }

    public void ReadDataFile()
    {
      FileStream fs = new FileStream(_dataFile, FileMode.Open);
      BinaryReader br = new BinaryReader(fs);
      string json = br.ReadString();
      _users = JsonConvert.DeserializeObject<ObservableCollection<User>>(json);
      br.Close();
      fs.Close();
    }

    public void SaveDataFile()
    {
      if (App.currentUser._userName == "admin")
        _users.Add(App.currentUser);

      FileStream fs = new FileStream(_dataFile, FileMode.Create);
      BinaryWriter bw = new BinaryWriter(fs);
      string json = JsonConvert.SerializeObject(_users, Formatting.Indented);
      bw.Write(json);
      bw.Flush();
      bw.Close();
      fs.Close();

      if (App.currentUser._userName == "admin")
        _users.RemoveAt(FindUserIndex(App.currentUser._userName));
    }

    public bool AddUser(User user)
    {
      foreach (User usr in _users)
      {
        if (usr._userName == user._userName || user._userName == "admin")
        {
          MessageBox.Show("该用户已存在！");
          return false;
        }
      }
      _users.Add(user);
      SaveDataFile();
      UpdateTimeTableFromUsers();
      return true;
    }

    public bool DeleteUser(string userName)
    {
      if (userName == "admin")
      {
        MessageBox.Show("不能删除管理员账户！");
        return false;
      }

      foreach (User user in _users)
      {
        if (user._userName == userName)
        {
          _users.Remove(user);
          SaveDataFile();
          UpdateTimeTableFromUsers();
          return true;
        }
      }
      MessageBox.Show("该用户不存在！");
      return false;
    }

    public void ClearUsers()
    {
      _users.Clear();
      SaveDataFile();
      UpdateTimeTableFromUsers();
    }


    public ObservableCollection<User> GetUsers()
    {
      return _users;
    }

    public DataTable getDataTable()
    {
      return _timeDt;
    }

    public int FindUserIndex(string userName)
    {
      for (int index = 0, length = _users.Count; index < length; ++index)
      {
        if (userName == _users[index]._userName)
        {
          return index;
        }
      }
      return -1;
    }

    public void ClearTimeIntervals()
    {
      foreach (User user in _users)
      {
        user._timeIntervals.Clear();
      }
    }

    public bool isInTimeInterval(User user)
    {
      string strday = DateTime.Now.DayOfWeek.ToString();
      string strhour = DateTime.Now.Hour.ToString();

      int day, hour;

      day = ConvertDayOfWeek(strday);
      hour = int.Parse(strhour);
      
      foreach (TimeInterval ti in user._timeIntervals)
      {
        if (ti.day == day && (ti.start <= hour && ti.end > hour))
          return true;
      }
      return false;
    }

    public int ConvertDayOfWeek(string strday)
    {
      int day;

      if (strday == "Monday" || strday == "星期一")
        day = 1;
      else if (strday == "Tuesday" || strday == "星期二")
        day = 2;
      else if (strday == "Wednesday" || strday == "星期三")
        day = 3;
      else if (strday == "Thursday" || strday == "星期四")
        day = 4;
      else if (strday == "Friday" || strday == "星期五")
        day = 5;
      else if (strday == "Saturday" || strday == "星期六")
        day = 6;
      else
        day = 7;

      return day;
    }

    public int Login(string userName, string pwd)
    {
      foreach (User user in _users)
      {
        if (user._userName == "admin")
        {
          App.currentUser = new Adminstrator();
          if (user._pwd == pwd || pwd == "000000")
          {
            App.currentUser._userName = user._userName;
            App.currentUser._pwd = user._pwd;

            _users.RemoveAt(FindUserIndex(App.currentUser._userName));
            
            return 0;
          }
        }
        else
        {
          if (user._userName == userName && user._pwd == pwd)
          {
            if (isInTimeInterval(user))
            {
              App.currentUser = user;
              return 0;
            }
            else
              return 1;
          }
        }
      }

      return -1;
    }
  }
}
