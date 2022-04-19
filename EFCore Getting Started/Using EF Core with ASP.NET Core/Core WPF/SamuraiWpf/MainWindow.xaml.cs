using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SamuraiWpf
{
  public partial class MainWindow : Window
  {
    private readonly ConnectedData _repo = new ConnectedData();
    private Samurai _currentSamurai;
    private bool _isListChanging;
    private bool _isLoading;
    private ObjectDataProvider _samuraiViewSource;

    public MainWindow() {
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
      _isLoading = true;
      samuraiListBox.ItemsSource = _repo.SamuraisListInMemory();
      _samuraiViewSource = (ObjectDataProvider)FindResource("SamuraiViewSource");
      _isLoading = false;
      samuraiListBox.SelectedIndex = 0;
    }

    private void SamuraiListBox_OnSelectionChangedListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (!_isLoading) {
        _isListChanging = true;
        _currentSamurai = _repo.LoadSamuraiGraph((int)samuraiListBox.SelectedValue);
        _samuraiViewSource.ObjectInstance = _currentSamurai;
        _isListChanging = false;
      }
    }

    private void Save_Click(object sender, RoutedEventArgs e) {
      _repo.SaveChanges(_currentSamurai.GetType());
      //samuraiListBox.ItemsSource = _repo.SamuraisListInMemory();
     
    }

    private void realNameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
      if (!_isLoading && !_isListChanging) {
        if (_currentSamurai.SecretIdentity == null) {
          _currentSamurai.SecretIdentity = new SecretIdentity();
        }
        _currentSamurai.SecretIdentity.RealName = ((TextBox)sender).Text;
        _currentSamurai.IsDirty = true;
      }
    }

    private void New_Click(object sender, RoutedEventArgs e) {

      _currentSamurai = _repo.CreateNewSamurai();
      _samuraiViewSource.ObjectInstance = _currentSamurai;
      samuraiListBox.SelectedItem = _currentSamurai;
    }

    private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
      if (!_isLoading && !_isListChanging) {
        _currentSamurai.IsDirty = true;
      }
    }

    private void nameTextBox_LostFocus(object sender, RoutedEventArgs e) {
      samuraiListBox.Items.Refresh();
    }

    private void quotesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
      if (!_isLoading && !_isListChanging) {
        _currentSamurai.IsDirty = true;
      }
    }

    private void gotoBattles_Click(object sender, RoutedEventArgs e) {
      var battlesWindow = new BattlesWindow(); //create your new form.
      battlesWindow.Show(); //show the new form.
      this.Close(); //only if you want to close the current form.
    }
  }
  }
