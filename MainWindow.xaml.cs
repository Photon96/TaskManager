using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TaskManagerProjekt.ViewModel;

namespace TaskManagerProjekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _VM;
        private DispatcherTimer _Timer;
        public MainWindow()
        {
            InitializeComponent();
            _VM = new MainViewModel();
            SetTimer();
            this.DataContext = _VM;
            FilterTextGrid.DataContext = _VM;
            Priorities.ItemsSource = Enum.GetValues(typeof(ProcessPriorityClass)).Cast<ProcessPriorityClass>();
            Priorities.SelectedIndex = 0;
        }

        private void SetTimer()
        {
            _Timer = new DispatcherTimer();
            _Timer.Interval = TimeSpan.FromSeconds(10);
            _Timer.Tick += timer_Tick;
            _Timer.Start();
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
            _VM.SortBy(columnBinding.Path.Path);
            ListView lv = sender as ListView;
            lv.Items.Refresh();
        }

        private void GetSelectedProcess(object sender, MouseButtonEventArgs e)
        {
            var listView = (ListView)sender;
            _VM.SetSelectedProcess((Process)listView.SelectedItems[0]);
        }

        private void KillProcess_OnClick(object sender, RoutedEventArgs e)
        {
            _VM.KillSelectedProcess();
            
        }

        private void ChangePriority_OnClick(object sender, RoutedEventArgs e)
        {
            var priority = (ProcessPriorityClass)Priorities.SelectionBoxItem;
            _VM.ChangePriority(priority);
        }

        
        void timer_Tick(object sender, EventArgs e)
        {
            _VM.RefreshProcesses();
        }

        void StartNewProcess(object sender, DoWorkEventArgs e)
        {
            int timeout = (int)e.Argument;
            Process process = new Process();
            process.StartInfo.FileName = "notepad.exe";
            process.Start();
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() => _VM.RefreshProcesses()));
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(timeout*10);
            }
            if (!process.HasExited)
                process.Kill();
            (sender as BackgroundWorker).ReportProgress(0);
        }

        void ProcessTimeout_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Process_PB.Value = e.ProgressPercentage;
        }

        void StartNewProcess_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Cos poszlo nie tak");
                return;
            }
            _VM.RefreshProcesses();
        }

        private void StartProcess_Btn_OnClick(object sender, RoutedEventArgs e)
        {
            int timeout = 0;
            if (TimeoutProcess_TextBox.Text.Length > 0)
                timeout = Int32.Parse(TimeoutProcess_TextBox.Text);
            if (timeout > 0)
            {
                BackgroundWorker processWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true
                };
                processWorker.DoWork += StartNewProcess;
                processWorker.RunWorkerCompleted += StartNewProcess_Completed;
                processWorker.ProgressChanged += ProcessTimeout_ProgressChanged; 
                processWorker.RunWorkerAsync(timeout);
                _VM.RefreshProcesses();
            }
            else
            {
                MessageBox.Show("Timeout musi byc wiekszy od 0");
            }
        }
    }
}
