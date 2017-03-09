using PostSharp.Samples.CustomLogging.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class DataItem
    {
        private static string[] _samples = new[] { "lorem", "ipsum", "dolor", "sit", "amet" };

        public DataItem(Random rand, int index)
        {
            Flag = rand.Next(2) == 0;
            Index = index;
            Column1 = Guid.NewGuid().ToString();
            Column2 = rand.Next(20) == 0 ? null : Guid.NewGuid().ToString();
            Column3 = Guid.NewGuid().ToString();
            Column4 = Guid.NewGuid().ToString();
            Column5 = _samples[rand.Next(_samples.Length)];
            Probability = rand.NextDouble();
        }

        public bool Flag { get; private set; }
        public int Index { get; private set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
        public string Column4 { get; set; }
        public string Column5 { get; set; }
        public double Probability { get; private set; }
    }


    /// <summary>
    /// Interaction logic for WindowOutput.xaml
    /// </summary>
    public partial class WindowOutput : Window
    {
        private Random _rand = new Random();

        readonly List<LogItem> _logItems;

        public List<LogItem> _Models { get; set; }
        public ListCollectionView _View { get; set; }

        public IEnumerable<DataItem> Items
        {
            get
            {
                return Enumerable.Range(0, 100).Select(index => new DataItem(_rand, index)).ToArray();
            }
        }

        public WindowOutput()
        {
            _logItems = ReadDatabase.Instance.FindAll().ToList();
            InitializeComponent();
            
            
            dataGrid.ItemsSource = _logItems;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SimpleLogger.Logger.Log("WindowOutput", this);
        }
    }
}
