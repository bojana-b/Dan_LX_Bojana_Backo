using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Commands;
using Zadatak_1.HelperMethods;
using Zadatak_1.Models;
using Zadatak_1.Views;

namespace Zadatak_1.ViewModels
{
    class EditEmployeeViewModel : ViewModelBase
    {
        EditEmployeeView editEmployeeView;
        ServiceEmployee serviceEmployee = new ServiceEmployee();
        ServiceSector serviceSector = new ServiceSector();
        Calculations calculator = new Calculations();

        public EditEmployeeViewModel(EditEmployeeView editEmployeeViewOpen, vwEmployee employeeToEdit)
        {
            editEmployeeView = editEmployeeViewOpen;
            this.employee = employeeToEdit;
            sector = employeeToEdit.SectorName;
            GenderList = serviceEmployee.GetAllGender();
            LocationList = serviceEmployee.GetAllLocations();
            ManagerList = serviceEmployee.GetAllManagers(employee);
            //gets user initial values before editing
            CheckIsEmployeeChanged = new vwEmployee
            {
                Name = employeeToEdit.Name,
                Surname = employeeToEdit.Surname,
                NumberOfIdentityCard = employeeToEdit.NumberOfIdentityCard,
                PhoneNumber = employeeToEdit.PhoneNumber,
                JMBG = employeeToEdit.JMBG,
                Gender = employeeToEdit.Gender,
                Sector = employeeToEdit.Sector,
                SectorName = employeeToEdit.SectorName,
                Location = employeeToEdit.Location,
                Manager = employee.Manager
            };
        }

        #region Properties
        public vwEmployee CheckIsEmployeeChanged { get; set; }

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
        
        /// <summary>
        /// This method invokes a methods for editing employee achecks if sector of employee exists. If not exist, invokes a method for adding sector.
        /// </summary>
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
                serviceEmployee.EditEmployee(employee);
                editEmployeeView.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>       
        /// This method checks if employee data is changed. If changed, checks if user input data is valid.
        /// </summary>
        /// <returns>True if data is changed and valid, false if not.</returns>  
        public bool CanSaveExecute()
        {
            DateTime date = DateTime.Now;
            try
            {
                //checks if user input data changed and valid               
                if (
                     (employee.Name != CheckIsEmployeeChanged.Name || employee.Surname != CheckIsEmployeeChanged.Surname || employee.NumberOfIdentityCard != CheckIsEmployeeChanged.NumberOfIdentityCard ||
                          employee.JMBG != CheckIsEmployeeChanged.JMBG || employee.Gender != CheckIsEmployeeChanged.Gender || employee.PhoneNumber != CheckIsEmployeeChanged.PhoneNumber
                          || sector != CheckIsEmployeeChanged.SectorName || employee.Location != CheckIsEmployeeChanged.Location || employee.Manager != CheckIsEmployeeChanged.Manager)
                     &&
                     !String.IsNullOrEmpty(employee.Name) && !String.IsNullOrEmpty(employee.Surname) && employee.NumberOfIdentityCard.Length == 9 && employee.NumberOfIdentityCard.All(Char.IsDigit)
                     && employee.JMBG.Length == 13 && employee.JMBG.All(Char.IsDigit) && Location != null && !String.IsNullOrEmpty(sector) && !String.IsNullOrEmpty(employee.PhoneNumber) &&
                     Gender != null && calculator.CalculateDateOfBirth(employee.JMBG, out date)
                     )
                {
                    //if (validation.ValidationForUnique(employee.NumberOfIdentityCard, employee.JMBG, employee.PhoneNumber) == true)
                    //{
                    //    Employee.DateOfBirth = date;
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
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
        /// <summary>
        /// This method invokes method for closing window of esiting employee.
        /// </summary>
        public void CancelExecute()
        {
            try
            {
                editEmployeeView.Close();
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
    }
}
