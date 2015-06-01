using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

using Newtonsoft.Json;

namespace landfall
{
  public class DataManager
  {
    private string _dataFile = null;
    private ObservableCollection<User> _users = new ObservableCollection<User>();

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
      FileStream fs = new FileStream(_dataFile, FileMode.Create);
      BinaryWriter bw = new BinaryWriter(fs);
      string json = JsonConvert.SerializeObject(_users, Formatting.Indented);
      bw.Write(json);
      bw.Flush();
      bw.Close();
      fs.Close();
    }

    public bool AddUser(User user)
    {
      foreach (User usr in _users)
      {
        if (usr._userName == user._userName)
        {
          MessageBox.Show("该用户已存在！");
          return false;
        }
      }
      _users.Add(user);
      SaveDataFile();
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
          return true;
        }
      }
      MessageBox.Show("该用户不存在！");
      return false;
    }

    public void ClearUsers()
    {
      User admin = new User();
      foreach (User user in _users)
      {
        if (user._userName == "admin")
        {
          admin._userName = user._userName;
          admin._pwd = user._pwd;
          break;
        }
      }
      _users.Clear();
      _users.Add(admin);
      SaveDataFile();
    }

    public void OverwriteUser(User currentUser)
    {
      foreach (User user in _users)
      {
        if (user._userName == currentUser._userName)
        {
          user._pwd = currentUser._pwd;
          user._timeIntervals = currentUser._timeIntervals;
          break;
        }
      }
      SaveDataFile();
    }

    public ObservableCollection<User> GetUsers()
    {
      return _users;
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

    public bool Login(string userName, string pwd)
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
            return true;
          }
        }
        else
        {
          if (user._userName == userName && user._pwd == pwd)
          {
            App.currentUser = user;
            return true;
          }
        }
      }

      return false;
    }
  }
}
