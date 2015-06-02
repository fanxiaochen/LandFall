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

    public AdminControlWindow()
    {
      InitializeComponent();

      App.dataManager.InitTimeTable();

      ((this.FindName("listView")) as ListView).ItemsSource = App.dataManager.GetUsers();

      dataSet.Tables.Add(App.dataManager.getDataTable());
      dataGrid.ItemsSource = dataSet.Tables[0].DefaultView;

      App.dataManager.UpdateTimeTableFromUsers();

    }

    private void Table_Click_Add(object sender, RoutedEventArgs e)
    {
      AddNewTimeWindow addNewTimeWindow = new AddNewTimeWindow();
      addNewTimeWindow.ShowDialog();
      string addName = addNewTimeWindow.getUserName();

      int rowIndex = dataGrid.Items.IndexOf(dataGrid.CurrentItem);
      int colIndex = this.dataGrid.SelectedCells[0].Column.DisplayIndex;
      App.dataManager.getDataTable().Rows[rowIndex][colIndex] = addName;
      App.dataManager.UpdateUsersFromTimeTable();
    }

    private void Table_Click_Delete(object sender, RoutedEventArgs e)
    {
      int rowIndex = dataGrid.Items.IndexOf(dataGrid.CurrentItem);
      int colIndex = this.dataGrid.SelectedCells[0].Column.DisplayIndex;
      App.dataManager.getDataTable().Rows[rowIndex][colIndex] = " ";
      App.dataManager.UpdateUsersFromTimeTable();
    }

    private void Table_Click_Clear(object sender, RoutedEventArgs e)
    {
      // 12 time intervals and 1 column for times, 7 columns for days
      for (int row = 0; row < 12; ++row)
      {
        for (int col = 1; col < 8; ++col)
        {
          App.dataManager.getDataTable().Rows[row][col] = " ";
        }
      }

      App.dataManager.UpdateUsersFromTimeTable();
    }

    private void WatchPassword(object sender, RoutedEventArgs e)
    {
      string userName = (listView.SelectedItem as User)._userName;
      string pwd = App.dataManager.FindPassword(userName);
      MessageBox.Show(pwd);
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
