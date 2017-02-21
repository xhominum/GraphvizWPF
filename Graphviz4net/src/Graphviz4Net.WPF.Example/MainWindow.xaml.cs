
namespace Graphviz4Net.WPF.Example
{
    using System.Threading;
    using System.Windows;
    using SimpleLogger;
    using SimpleLogger.Logging.Handlers;

    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            this.viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
            InitializeComponent();
            this.AddNewEdge.Click += AddNewEdgeClick;
            this.AddNewPerson.Click += AddNewPersonClick;
			this.UpdatePerson.Click += UpdatePersonClick;
            this.AddNote.Click += AddNote_Click;
            //NewWindowHandler();
            SimpleLogger.Logger.LoggerHandlerManager
            .AddHandler(new ConsoleLoggerHandler())
            .AddHandler(new FileLoggerHandler())
            .AddHandler(new DebugConsoleLoggerHandler());

            Logger.Log("test log");
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.CreateNote();
        }

        void UpdatePersonClick(object sender, RoutedEventArgs e)
		{
			this.viewModel.UpdatePersonName = (string) this.UpdatePersonName.SelectedItem;
			this.viewModel.UpdatePerson();
		}

        private void AddNewPersonClick(object sender, RoutedEventArgs e)
        {
            this.viewModel.CreatePerson();
        }


        private void AddNewEdgeClick(object sender, RoutedEventArgs e)
        {
            this.viewModel.NewEdgeStart = (string) this.NewEdgeStart.SelectedItem;
            this.viewModel.NewEdgeEnd = (string)this.NewEdgeEnd.SelectedItem;
            this.viewModel.CreateEdge();
        }

        private void NewWindowHandler()
        {
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

        private void ThreadStartingPoint()
        {
            WindowOutput tempWindow = new WindowOutput();
            tempWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
        }

    }
}
