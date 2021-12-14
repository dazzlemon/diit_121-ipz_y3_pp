using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Refactor;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Refactor.Refactor Refactor = new Refactor.Refactor();
        private string filePath;
        private Stack<string> previousStates = new Stack<string>();
        private Stack<string> history = new Stack<string>();
        private Stack<string> nextStates = new Stack<string>();   

        public MainWindow()
        {
            InitializeComponent();
            InputBindings.Add(new KeyBinding(new WindowCommand(){ ExecuteDelegate = OpenFile_Click }, new KeyGesture(Key.O, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(new WindowCommand(){ ExecuteDelegate = SaveFile_Click }, new KeyGesture(Key.S, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(new WindowCommand(){ ExecuteDelegate = SaveAsFile_Click }, new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift)));
        }

        private void Undo(object sender, RoutedEventArgs e) 
        {
            if (history.Count != 0)
            {
                nextStates.Push(textEditor.Text);
                textEditor.Text = history.Pop();
            }

        }

        private void Redo(object sender, RoutedEventArgs e)
        {
            if (nextStates.Count != 0)
            {
                history.Push(textEditor.Text);
                textEditor.Text = nextStates.Pop();
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        textEditor.Text  = reader.ReadToEnd();
                    }
                }
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(filePath, textEditor.Text);
            System.Windows.MessageBox.Show("File is saved", "File status", MessageBoxButton.OK);
        }

        private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string filepath = saveFileDialog.FileName;
                    File.WriteAllText(filepath, textEditor.Text);
                    System.Windows.MessageBox.Show("File is saved", "File status", MessageBoxButton.OK);
                }
            }
        }

        private void RefactorMethodName_Click(object sender, RoutedEventArgs e)
        {
            MethodPropertiesWindow methodPropertiesWindow = new MethodPropertiesWindow();
            if (methodPropertiesWindow.ShowDialog() == true)
            {
                history.Push(textEditor.Text);
                nextStates.Clear();
                string refactoredText = Refactor.RenameMethod(
                    methodPropertiesWindow.OldMethodName,
                    methodPropertiesWindow.NewMethodName,
                    textEditor.Text);
                textEditor.Text = refactoredText;
            }
        }

        private void RefactorMagickNumber_Click(object sender, RoutedEventArgs e)
        {
            MagicNumberPropertiesWindow magicNumberPropertiesWindow = new MagicNumberPropertiesWindow();
            if (magicNumberPropertiesWindow.ShowDialog() == true) 
            {
                history.Push(textEditor.Text);
                nextStates.Clear();
                string refactoredText = Refactor.ReplaceMagicNumber(
                       magicNumberPropertiesWindow.MagicNumber,
                       magicNumberPropertiesWindow.NewMagicNumberName,
                       textEditor.Text);
                textEditor.Text = refactoredText;
            }
        }
    }
}
