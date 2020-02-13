using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerProjekt
{
    public class ProcessesModel
    {
        private List<Process> processes;
        private Boolean areProcessesSortedByName = false;
        private Boolean areProcessesSortedById = false;
        private string filteredName = String.Empty;
        public ProcessesModel()
        {
            processes = new List<Process>(Process.GetProcesses());
        }

        public List<Process> GetProcesses()
        {
            return processes;
        }

        public List<Process> GetFilteredProcesses(string filterName)
        {
            filteredName = filterName;
            if (filterName.Equals(String.Empty))
            {
                return processes;
            }
            return processes.FindAll(p => p.ProcessName.Contains(filterName));
        }

        public List<Process> GetSortedProcessesBy(string name)
        {
            if (name.Equals("ProcessName"))
            {
                areProcessesSortedByName = true;
                areProcessesSortedById = false;
                return processes.OrderBy(proc => proc.ProcessName).ToList();
            }
            else 
            {
                areProcessesSortedById = true;
                areProcessesSortedByName = false;
                return processes.OrderBy(proc => proc.Id).ToList();
            }
        }

        public List<Process> GetRefreshedProcesses()
        {
            processes = Process.GetProcesses().ToList();
            
            if (areProcessesSortedByName)
            {
                processes = GetSortedProcessesBy("ProcessName");
            } else if (areProcessesSortedById)
            {
                processes = GetSortedProcessesBy("Id");
            }

            return GetFilteredProcesses(filteredName);
        }
    }
}
