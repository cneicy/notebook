using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

namespace notebook;

public partial class MainWindow
{
    private static string? _textPath;
    private bool _saveTrigger;

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
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Title = "保存文本文件"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                _textPath = saveFileDialog.FileName;
            }
        }

        if (_textPath == null) return;
        File.WriteAllText(_textPath, TextBox.Text);
        if (Title.Contains('*'))
        {
            Title = "正在编辑: " + _textPath;
        }
    }

    private void Load()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "文本文件|*.txt|所有文件|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            Title = "打开文本文件",
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() != true) return;
        _textPath = openFileDialog.FileName;
        Title = "正在编辑: " + _textPath;
        TextBox.Text = File.ReadAllText(_textPath);
    }


    private void SaveBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Save();
    }

    private void LoadBtn_OnClick(object sender, RoutedEventArgs e)
    {
        Load();
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
    
    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        _saveTrigger = true;
    }

    private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
    {
        _saveTrigger = false;
    }

    private void TextBox_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control) return;
        if (sender is not TextBox textBox) return;
        var fontSize = textBox.FontSize + (e.Delta > 0 ? 2 : -2);
        if (fontSize > 10)
        {
            textBox.FontSize = fontSize;
        }
    }
    
}

public static class Commands
{
    public static readonly RoutedUICommand Save = new("Save", "Save", typeof(MainWindow));
}