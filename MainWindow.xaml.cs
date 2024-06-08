using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace notebook;

public partial class MainWindow
{
    private static string? _textPath;

    //private List<string> _contentTemp = [];
    //private int _index;
    private bool _saveTrigger = false;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Save()
    {
        if (_textPath != null) File.WriteAllText(_textPath, TextBox.Text);
        else
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "文本文件|*.txt|所有文件|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "保存文本文件"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                _textPath = saveFileDialog.FileName;
            }
        }

        if (_textPath == null) return;
        //_contentTemp.Add(TextBox.Text);
        //_index=_contentTemp.Count-1;
        File.WriteAllText(_textPath, TextBox.Text);
        if (Title.Contains('*'))
        {
            Title = "正在编辑: " + _textPath;
        }
    }

    private void LoadTextPath()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "文本文件|*.txt|所有文件|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Title = "打开文本文件",
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() != true) return;
        _textPath = openFileDialog.FileName;
        Title = "正在编辑: " + _textPath;
        //_contentTemp = [];
        TextBox.Text = File.ReadAllText(_textPath);
    }


    private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Save();
    }

    private void LoadBtn_OnClick(object sender, RoutedEventArgs e)
    {
        LoadTextPath();
    }

    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (_textPath is null) return;
        if (_saveTrigger) Save();
        var temp = File.ReadAllText(_textPath);
        if (temp != TextBox.Text && !Title.Contains('*'))
        {
            Title += "*";
        }
    }

    private void MainWindow_OnSaveExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        Save();
    }

    /*private void MainWindow_OnWithdrawExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        TextBox.Text = _contentTemp[_index];
        _index--;
    }
    private void MainWindow_OnRestoreExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (_index+1 > _contentTemp.Count - 1) return;
        _index++;
        TextBox.Text = _contentTemp[_index];
    }*/
    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        _saveTrigger = true;
    }

    private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
    {
        _saveTrigger = false;
    }
}

public static class Commands
{
    public static readonly RoutedUICommand Save = new RoutedUICommand("Save", "Save", typeof(MainWindow));
    //public static readonly RoutedUICommand Withdraw = new RoutedUICommand("Withdraw", "Withdraw", typeof(MainWindow));
    //public static readonly RoutedUICommand Restore = new RoutedUICommand("Restore", "Restore", typeof(MainWindow));
}