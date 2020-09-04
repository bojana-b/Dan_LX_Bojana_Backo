using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Zadatak_1.Models
{
    class ServiceEmployee : Logger
    {
        // Method that add Employee to database
        public void AddEmployee(vwEmployee employeeToAdd)
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    tblEmployee employee = new tblEmployee
                    {
                        Name = employeeToAdd.Name,
                        Surname = employeeToAdd.Surname,
                        DateOfBirth = employeeToAdd.DateOfBirth,
                        NumberOfIdentityCard = employeeToAdd.NumberOfIdentityCard,
                        JMBG = employeeToAdd.JMBG,
                        Gender = employeeToAdd.Gender,
                        PhoneNumber = employeeToAdd.PhoneNumber,
                        Sector = employeeToAdd.Sector,
                        LocationID = employeeToAdd.LocationID,
                        Manager = employeeToAdd.Manager
                    };
                    context.tblEmployees.Add(employee);
                    context.SaveChanges();
                    employeeToAdd.EmployeeID = employee.EmployeeID;
                    LogAction("Employee " + employeeToAdd.Employee + " created. ID: " + employeeToAdd.EmployeeID + " JMBG: " + employeeToAdd.JMBG);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // Method that create list of Employees from database
        public List<vwEmployee> GetAllEmployees()
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    List<vwEmployee> employees = new List<vwEmployee>();
                    employees = (from x in context.vwEmployees select x).ToList();
                    return employees;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        // Method that edit Employee from database
        public vwEmployee EditEmployee(vwEmployee employee)
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    tblEmployee employeeToEdit = context.tblEmployees.Where(x => x.EmployeeID == employee.EmployeeID).FirstOrDefault();
                    employeeToEdit.Name = employee.Name;
                    employeeToEdit.Surname = employee.Surname;
                    employeeToEdit.DateOfBirth = employee.DateOfBirth;
                    employeeToEdit.NumberOfIdentityCard = employee.NumberOfIdentityCard;
                    employeeToEdit.Gender = employee.Gender;
                    employeeToEdit.PhoneNumber = employee.PhoneNumber;
                    employeeToEdit.Sector = employee.Sector;
                    employeeToEdit.LocationID = employee.LocationID;
                    employeeToEdit.Manager = employee.Manager;
                    context.SaveChanges();
                    LogAction("Employee with ID " + employeeToEdit.EmployeeID + " updated.");
                    return employee;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        // Method that delete Employee from database
        public void DeleteEmployee(int employeeID)
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    //creating a list of employees for which employee with forwarded id is the manager
                    var employeeOfThisManager = context.tblEmployees.Where(x => x.Manager == employeeID).ToList();
                    //if the list is not empty, setting manager id to null for every employee in that list
                    if (employeeOfThisManager.Count() > 0)
                    {
                        foreach (var employee in employeeOfThisManager)
                        {
                            employee.Manager = null;
                            LogAction("Employee with ID " + employee.EmployeeID + " updated so he has no manager.");
                        }
                    }
                    //finding employee with forwarded id
                    tblEmployee employeeToDelete = context.tblEmployees.Where(x => x.EmployeeID == employeeID).FirstOrDefault();
                    //if that employee was the only in the sector, deleting sector
                    var peopleInSector = context.tblEmployees.Where(x => x.Sector == employeeToDelete.Sector).ToList();
                    if (peopleInSector.Count() == 1)
                    {
                        var sector = context.tblSectors.Where(x => x.SectorID == employeeToDelete.Sector).FirstOrDefault();
                        context.tblSectors.Remove(sector);
                        context.SaveChanges();
                        LogAction("Sector " + sector.SectorName + " with ID: " + sector.SectorID + " deleted.");
                    }
                    //removing employee from DbSet and saving changes to database
                    context.tblEmployees.Remove(employeeToDelete);
                    context.SaveChanges();
                    LogAction("Employee with ID " + employeeToDelete.EmployeeID + " deleted.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // Method that create list of Managers
        public List<vwEmployee> GetAllManagers(vwEmployee employee)
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    List<vwEmployee> employees = new List<vwEmployee>();
                    //inserting into list all employees except forwarded employee (finding him based on jmbg)
                    employees = context.vwEmployees.Where(x => x.JMBG != employee.JMBG).ToList();
                    return employees;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        // Method that create list of genders
        public List<vwGender> GetAllGender()
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    List<vwGender> genders = new List<vwGender>();
                    genders = (from x in context.vwGenders select x).ToList();
                    return genders;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        // This method reads locations from file, adds them to database
        public void AddLocations()
        {
            int id = 0;
            using (EmployeeDBEntities context = new EmployeeDBEntities())
            {
                if (File.Exists(@"..\..\Locations.txt"))
                {
                    string[] readFile = File.ReadAllLines(@"..\..\Locations.txt");

                    for (int i = 0; i < readFile.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(readFile[i]))
                        {
                            string[] trim = readFile[i].Split(',');
                            string address = trim[0];
                            string city = trim[1];
                            string country = trim[2];
                            id++;

                            tblLocation loc = new tblLocation()
                            {
                                LocationID = id,
                                Address = address,
                                City = city,
                                State = country
                            };
                            context.tblLocations.Add(loc);
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
        // This method creates a list of data from view of locations
        public List<vwLocation> GetAllLocations()
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    List<vwLocation> locations = new List<vwLocation>();
                    locations = (from x in context.vwLocations select x).OrderBy(x => x.Address).ToList();
                    return locations;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }
    }
}
