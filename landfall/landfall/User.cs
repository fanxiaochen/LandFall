using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace landfall
{
  public struct TimeInterval
  {
    public int day;
    public int start;
    public int end;
  }

  public class User
  {
    public string _userName {get; set;}
    public string _pwd {get; set;}
    public TimeInterval[] _timeIntervals = null;

    public void ChangePassword(string newPwd)
    {
      _pwd = newPwd;
      App.dataManager.OverwriteUser(this);
    }

    virtual public bool AddUser(User user) { return false; }
    virtual public bool DeleteUser(string userName) { return false; }
    virtual public void ClearUsers() { }
  }

  public class Adminstrator : User
  {
    public override bool AddUser(User user)
    {
      return App.dataManager.AddUser(user);
    }

    public override bool DeleteUser(string userName)
    {
      return App.dataManager.DeleteUser(userName);
    }

    public override void ClearUsers()
    {
      App.dataManager.ClearUsers();
    }
  }
}
