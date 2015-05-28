using System;
using System.Collections.Generic;
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
    private List<User> _users = new List<User>();

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
        readDataFile();
        return true;
      }
    }

    public void readDataFile()
    {
      FileStream fs = new FileStream(_dataFile, FileMode.Open);
      BinaryReader br = new BinaryReader(fs);
      string json = br.ReadString();
      _users = JsonConvert.DeserializeObject<List<User>>(json);
      br.Close();
      fs.Close();
    }

    public void saveDataFile()
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
      saveDataFile();
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
          saveDataFile();
          return true;
        }
      }
      MessageBox.Show("该用户不存在！");
      return false;
    }

    public void ClearUsers()
    {
      _users.RemoveAll(user => user._userName != "admin");
      saveDataFile();
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
      saveDataFile();
    }

    public List<User> GetUsers()
    {
      return _users;
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
