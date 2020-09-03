using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Commands;
using Zadatak_1.Models;
using Zadatak_1.Views;

namespace Zadatak_1.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        MainWindow main;
        ServiceEmployee serviceEmployee;
        //Locations locations = new Locations();

        public MainWindowViewModel(MainWindow mainOpen)
        {
            main = mainOpen;
            serviceEmployee = new ServiceEmployee();
            EmployeeList = serviceEmployee.GetAllEmployees();
            //locations.AddLocations();
        }

        #region Properties
        private vwEmployee employee;
        public vwEmployee Employee
        {
            get
            {
                return employee;
            }
            set
            {
                employee = value;
                OnPropertyChanged("Employee");
            }
        }


        private List<vwEmployee> employeeList;
        public List<vwEmployee> EmployeeList
        {
            get
            {
                return employeeList;
            }
            set
            {
                employeeList = value;
                OnPropertyChanged("EmployeeList");
            }
        }
        #endregion

        #region Commands
        // DeleteEmployee Button
        private ICommand deleteEmployee;
        public ICommand DeleteEmployee
        {
            get
            {
                if (deleteEmployee == null)
                {
                    deleteEmployee = new RelayCommand(param => DeleteEmployeeExecute(), param => CanDeleteEmployeeExecute());
                }
                return deleteEmployee;
            }
        }
        public void DeleteEmployeeExecute()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure that you want to delete the employee?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                try
                {
                    if (Employee != null)
                    {
                        serviceEmployee.DeleteEmployee(Employee.EmployeeID);
                        EmployeeList = serviceEmployee.GetAllEmployees();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        public bool CanDeleteEmployeeExecute()
        {
            return true;
        }

        // EditEmployee Button
        private ICommand editEmployee;
        public ICommand EditEmployee
        {
            get
            {
                if (editEmployee == null)
                {
                    editEmployee = new RelayCommand(param => EditEmployeeExecute(), param => CanEditEmployeeExecute());
                }
                return editEmployee;
            }
        }
        public void EditEmployeeExecute()
        {
            try
            {
                //EditEmployeeView editUser = new EditEmployeeView(Employee);
                //editUser.ShowDialog();
                ////invokes method to update a list of users
                //EmployeeList = employees.GetAllEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public bool CanEditEmployeeExecute()
        {
            return true;
        }

        // AddEmployee Button
        private ICommand addEmployee;
        public ICommand AddEmployee
        {
            get
            {
                if (addEmployee == null)
                {
                    addEmployee = new RelayCommand(param => AddEmployeeExecute(), param => CanAddEmployeeExecute());
                }
                return addEmployee;
            }
        }
        public void AddEmployeeExecute()
        {
            try
            {
                AddEmployeeView addEmployee = new AddEmployeeView();
                addEmployee.ShowDialog();
                EmployeeList = serviceEmployee.GetAllEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool CanAddEmployeeExecute()
        {
            return true;
        }
        #endregion
    }
}
