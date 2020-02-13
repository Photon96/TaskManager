using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskManagerProjekt
{
    /// <summary>
    /// Interaction logic for ProcessInfoControl.xaml
    /// </summary>
    public partial class ProcessInfoControl : UserControl
    {
        public static readonly DependencyProperty ProcessNameProperty =
            DependencyProperty.Register("ProcessName", typeof(String),
                typeof(ProcessInfoControl), new FrameworkPropertyMetadata(string.Empty));

        public String ProcessName
        {
            get => GetValue(ProcessNameProperty).ToString();
            set => SetValue(ProcessNameProperty, value);
        }


        public static readonly DependencyProperty ProcessIdProperty =
            DependencyProperty.Register("ProcessId", typeof(int),
                typeof(ProcessInfoControl), new FrameworkPropertyMetadata());

        public int ProcessId
        {
            get => (int)GetValue(ProcessIdProperty);
            set => SetValue(ProcessIdProperty, value);
        }

        public static readonly DependencyProperty ProcessNonPagedMemoryProperty =
            DependencyProperty.Register("ProcessNonPagedMemory", typeof(long),
                typeof(ProcessInfoControl), new FrameworkPropertyMetadata(null));

        public long ProcessNonPagedMemory
        {
            get => (long)GetValue(ProcessNonPagedMemoryProperty);
            set => SetValue(ProcessNonPagedMemoryProperty, value);
        }

        public static readonly DependencyProperty ProcessPagedMemoryProperty =
            DependencyProperty.Register("ProcessPagedMemory", typeof(long),
                typeof(ProcessInfoControl), new FrameworkPropertyMetadata(null));

        public long ProcessPagedMemory
        {
            get => (long)GetValue(ProcessPagedMemoryProperty);
            set => SetValue(ProcessPagedMemoryProperty, value);
        }

        public static readonly DependencyProperty ProcessPrivateMemoryProperty =
            DependencyProperty.Register("ProcessPrivateMemory", typeof(int),
                typeof(ProcessInfoControl), new FrameworkPropertyMetadata(null));

        public int ProcessPrivateMemory
        {
            get => (int)GetValue(ProcessPrivateMemoryProperty);
            set => SetValue(ProcessPrivateMemoryProperty, value);
        }

        public static readonly DependencyProperty ProcessVirtualMemoryProperty =
            DependencyProperty.Register("ProcessVirtualMemory", typeof(long),
                typeof(ProcessInfoControl), new FrameworkPropertyMetadata(null));

        public long ProcessVirtualMemory
        {
            get => (long)GetValue(ProcessVirtualMemoryProperty);
            set => SetValue(ProcessVirtualMemoryProperty, value);
        }

        public static readonly DependencyProperty ThreadsNumberProperty =
            DependencyProperty.Register("ThreadsNumber", typeof(int),
                typeof(ProcessInfoControl), new FrameworkPropertyMetadata(null));

        public int ThreadsNumber
        {
            get => (int)GetValue(ThreadsNumberProperty);
            set => SetValue(ThreadsNumberProperty, value);
        }

        public static readonly DependencyProperty PriorityProperty =
            DependencyProperty.Register("Priority", typeof(String),
                typeof(ProcessInfoControl), new FrameworkPropertyMetadata(string.Empty));

        public String Priority
        {
            get => GetValue(PriorityProperty).ToString();
            set => SetValue(PriorityProperty, value);
        }

        public ProcessInfoControl()
        {
            InitializeComponent();
        }
    }
}
