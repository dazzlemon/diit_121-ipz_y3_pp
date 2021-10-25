using System;
using System.Collections.Generic;
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

namespace Editor
{
    /// <summary>
    /// Interaction logic for MagickNumberPropertiesWindow.xaml
    /// </summary>
    public partial class MagicNumberPropertiesWindow : Window
    {
       
        public MagicNumberPropertiesWindow()
        {
            InitializeComponent();
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string MagicNumber => oldNameTextBox.Text;
        public string NewMagicNumberName => newNameTextBox.Text;
    }
}
