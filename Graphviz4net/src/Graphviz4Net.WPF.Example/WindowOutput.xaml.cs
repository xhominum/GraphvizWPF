using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Graphviz4Net.WPF.Example
{
    /// <summary>
    /// Interaction logic for WindowOutput.xaml
    /// </summary>
    public partial class WindowOutput : Window
    {
        public WindowOutput()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SimpleLogger.Logger.Log("WindowOutput", this);
        }
    }
}
