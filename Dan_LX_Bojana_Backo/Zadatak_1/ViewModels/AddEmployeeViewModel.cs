using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Commands;
using Zadatak_1.HelperMethods;
using Zadatak_1.Models;
using Zadatak_1.Views;

namespace Zadatak_1.ViewModels
{
    class AddEmployeeViewModel : ViewModelBase
    {
        AddEmployeeView addEmployeeView;
        Calculations calculator = new Calculations();
        ServiceEmployee serviceEmployee = new ServiceEmployee();
        ServiceSector serviceSector = new ServiceSector();
        //Locations locations = new Locations();

        public AddEmployeeViewModel(AddEmployeeView addEmployeeViewOpen)
        {
            addEmployeeView = addEmployeeViewOpen;
            GenderList = serviceEmployee.GetAllGender();
            //LocationList = locations.GetAllLocations();
            ManagerList = serviceEmployee.GetAllEmployees();
            employee = new vwEmployee();
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

        private vwEmployee manager;
        public vwEmployee Manager
        {
            get
            {
                return manager;
            }
            set
            {
                manager = value;
                OnPropertyChanged("Manager");
            }
        }

        private List<vwEmployee> managerList;
        public List<vwEmployee> ManagerList
        {
            get
            {
                return managerList;
            }
            set
            {
                managerList = value;
                OnPropertyChanged("EmployeeList");
            }
        }

        private vwLocation location;
        public vwLocation Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        }

        private List<vwLocation> locationList;
        public List<vwLocation> LocationList
        {
            get
            {
                return locationList;
            }
            set
            {
                locationList = value;
                OnPropertyChanged("LocationList");
            }
        }

        private vwGender gender;
        public vwGender Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        private List<vwGender> genderList;
        public List<vwGender> GenderList
        {
            get
            {
                return genderList;
            }
            set
            {
                genderList = value;
                OnPropertyChanged("GenderList");
            }
        }

        private string sector;
        public string Sector
        {
            get
            {
                return sector;
            }
            set
            {
                sector = value;
                OnPropertyChanged("Sector");
            }
        }
        #endregion

        #region Commands
        // Save Button
        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }
        public void SaveExecute()
        {
            try
            {
                if (serviceSector.IsSectorExists(Sector) == false)
                {
                    serviceSector.AddSector(sector);
                }
                Employee.Sector = serviceSector.FindSector(sector);
                Employee.LocationID = Convert.ToInt32(Location.LocationID);
                Employee.Gender = Convert.ToInt32(Gender.GenderID);
                if (Manager != null)
                {
                    Employee.Manager = Convert.ToInt32(Manager.EmployeeID);
                }
                serviceEmployee.AddEmployee(employee);
                addEmployeeView.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        public bool CanSaveExecute()
        {
            DateTime date = DateTime.Now;
            try
            {
                //checks if user input data valid
                if (!String.IsNullOrEmpty(employee.Name) && !String.IsNullOrEmpty(employee.Surname) && employee.NumberOfIdentityCard.Length == 9 && employee.NumberOfIdentityCard.All(Char.IsDigit)
                    && employee.JMBG.Length == 13 && employee.JMBG.All(Char.IsDigit) && Location != null && !String.IsNullOrEmpty(sector) && !String.IsNullOrEmpty(employee.PhoneNumber)  && Gender != null && calculator.CalculateDateOfBirth(employee.JMBG, out date) == true
                    )
                {
                    Employee.DateOfBirth = date;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        // Cancel Button
        private ICommand cancel;
        public ICommand Cancel
        {
            get
            {
                if (cancel == null)
                {
                    cancel = new RelayCommand(param => CancelExecute(), param => CanCancelExecute());
                }
                return cancel;
            }
        }
        public void CancelExecute()
        {
            try
            {
                addEmployeeView.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public bool CanCancelExecute()
        {
            return true;
        }
        #endregion
    }
}