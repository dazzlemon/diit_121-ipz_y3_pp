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
        private string fileContent;

        private Action<object, RoutedEventArgs> command;

        public MainWindow()
        {
            command = OpenFile_Click;
            InitializeComponent();
            //add this one statement to bind a new keyboard command shortcut
            //add a new key-binding, and pass in your command object instance which contains the Execute method which WPF will execute
            InputBindings.Add(new KeyBinding(new WindowCommand(){ ExecuteDelegate = command }, new KeyGesture(Key.O, ModifierKeys.Control)));
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
                        fileContent = reader.ReadToEnd();
                    }
                    textEditor.Text = fileContent;
                }
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(filePath, textEditor.Text);
            System.Windows.MessageBox.Show("File is saved", "File status", MessageBoxButton.OK);
        }

        private void RefactorMethodName_Click(object sender, RoutedEventArgs e)
        {
            MethodPropertiesWindow methodPropertiesWindow = new MethodPropertiesWindow();
            if (methodPropertiesWindow.ShowDialog() == true)
            {
                string refactoredText = Refactor.RenameMethod(
                    methodPropertiesWindow.OldMethodName,
                    methodPropertiesWindow.NewMethodName,
                    fileContent);
                textEditor.Text = refactoredText;
                fileContent = refactoredText;
            }
        }

        private void RefactorMagickNumber_Click(object sender, RoutedEventArgs e)
        {
            MagicNumberPropertiesWindow magicNumberPropertiesWindow = new MagicNumberPropertiesWindow();
            if (magicNumberPropertiesWindow.ShowDialog() == true) 
            {
                string refactoredText = Refactor.ReplaceMagicNumber(
                       magicNumberPropertiesWindow.MagicNumber,
                       magicNumberPropertiesWindow.NewMagicNumberName,
                       fileContent);
                textEditor.Text = refactoredText;
                fileContent = refactoredText;
            }
        }
    }
}
