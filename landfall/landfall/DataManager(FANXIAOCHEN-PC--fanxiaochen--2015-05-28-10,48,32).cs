using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json;

namespace landfall
{
  class DataManager
  {
    private string _dataFile = null;
    private List<User> _users;

    public DataManager(string dataFile)
    {
      _dataFile = dataFile;
      readDataFile();
    }

    public void readDataFile()
    {
      FileStream fs = new FileStream(_dataFile, FileMode.Open);
      BinaryReader br = new BinaryReader(fs);
      _users = JsonConvert.DeserializeObject<List<User>>(br.ToString());
    }

    public void saveDataFile()
    {
      FileStream fs = new FileStream
    }

    public void AddUser(User user)
    {

    }

    public void DeleteUser(string userName)
    {

    }
  }
}
