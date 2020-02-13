using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace TaskManagerProjekt.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, System.ComponentModel.INotifyPropertyChanged
    {
        #region Fields and Properties
        private ObservableCollection<Process> _processes;
        private ProcessesModel _processesModel;

        private string _filterText = String.Empty;
        public string FilterText
        {
            get => _filterText; 
            set
            {
                _filterText = value;
                FilterProcesses(_filterText);
                OnPropertyChanged("FilterText");
            }
        }
        public ObservableCollection<Process> Processes
        {
            get => _processes; 
            set
            {
                _processes = value;
                OnPropertyChanged("ProcessesList");
            }
        }

        private Process _selectedProcess;
        public Process SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged("SelectedProcess");
            }
        }

        private Boolean _killProcessBtnEnabled;

        public Boolean KillProcessBtnEnabled
        {
            get => _killProcessBtnEnabled;
            set
            {
                _killProcessBtnEnabled = value;
                OnPropertyChanged("KillProcessBtnEnabled");
            }
        }

        private Boolean _changePriorityBtnEnabled;

        public Boolean ChangePriorityBtnEnabled
        {
            get => _changePriorityBtnEnabled;
            set
            {
                _changePriorityBtnEnabled = value;
                OnPropertyChanged("ChangePriorityBtnEnabled");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
        public MainViewModel()
        {
            CreateRefreshProcessesCommand();
            _processesModel = new ProcessesModel();
            Processes = new ObservableCollection<Process>(_processesModel.GetProcesses());
            KillProcessBtnEnabled = false;
            ChangePriorityBtnEnabled = false;
        }

        public void SortBy(string name)
        {
            Processes.Clear();
            foreach (var sortedProcess in _processesModel.GetSortedProcessesBy(name))
            {
                Processes.Add(sortedProcess);
            }
        }

        public void ChangePriority(ProcessPriorityClass priority)
        {
            try
            {
                SelectedProcess.PriorityClass = priority;
                OnPropertyChanged("SelectedProcess");
                ChangePriorityBtnEnabled = false;
            }
            catch (Win32Exception e)
            {
                MessageBox.Show("Nie mo¿esz zmieniæ priorytetu tego procesu!");
            }
            
        }

        public void KillSelectedProcess()
        {
            try
            {
                SelectedProcess.Kill();
                SelectedProcess = null;
                RefreshProcesses();
                KillProcessBtnEnabled = false;
            }
            catch (Win32Exception e)
            {
                MessageBox.Show("Nie mo¿esz zabiæ tego procesu!");
            }

        }

        public void FilterProcesses(string filterName)
        {
            Processes.Clear();
            foreach (var filteredProcess in _processesModel.GetFilteredProcesses(filterName))
            {
                Processes.Add(filteredProcess);
            }
        }

        public void RefreshProcesses()
        {
            Processes.Clear();
            var refreshedProcesses = _processesModel.GetRefreshedProcesses();
            
            foreach (var process in refreshedProcesses)
            {
                Processes.Add(process);
            }
            
            if (SelectedProcess != null && !Processes.Any(p => p.Id == SelectedProcess.Id))
                RemoveSelectedProcess();
        }

        private void RemoveSelectedProcess()
        {
            SelectedProcess = null;
            KillProcessBtnEnabled = false;
            ChangePriorityBtnEnabled = false;
        }

        public void SetSelectedProcess(Process process)
        {
            SelectedProcess = process;
            ChangePriorityBtnEnabled = true;
            KillProcessBtnEnabled = true;
        }

        #region Commands
        public ICommand RefreshProcessesCommand
        {
            get;
            internal set;
        }

        private bool CanExecuteRefreshProcessesCommand()
        {
            return true;
        }

        private void CreateRefreshProcessesCommand()
        {
            RefreshProcessesCommand = new RelayCommand(RefreshProcessesExecute, CanExecuteRefreshProcessesCommand);
        }

        public void RefreshProcessesExecute()
        {
            RefreshProcesses();
        }

        #endregion
    }
}