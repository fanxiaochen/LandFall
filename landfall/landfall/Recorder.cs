using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace landfall
{
  public class Recorder
  {
    private string recordFile = null;
    private string startTime = null;
    private string endTime = null;
    private string date = null;

    public string GetRecordFile()
    {
      return recordFile;
    }

    public void GetRecordFile(string recordFile)
    {
      this.recordFile = recordFile;
    }

    public void GetStartTime()
    {
      this.startTime = DateTime.Now.ToString("T");
    }

    public void GetEndTime()
    {
      this.endTime = DateTime.Now.ToString("T");
    }

    public void GetDate()
    {
      this.date = DateTime.Now.ToString("D");
    }

    public void WriteRecord()
    {
      FileStream fs = new FileStream(recordFile, FileMode.Append);
      StreamWriter sw = new StreamWriter(fs);
      sw.Write(String.Format("用户: {0}   日期: {1}   登录时间: {2}——{3}\r\n", App.currentUser._userName, this.date,
        this.startTime, this.endTime));
      sw.Write("-------------------------------------------------------------------------\r\n");
      sw.Close();
      fs.Close();
    }
  }
}
