using System;
using System.Diagnostics;
using System.Linq;

namespace Zadatak_1.Models
{
    class ServiceSector : Logger
    {
        // Method that add sector tu database
        public void AddSector(string sectorToAdd)
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    tblSector sector = new tblSector
                    {
                        SectorName = sectorToAdd
                    };
                    context.tblSectors.Add(sector);
                    context.SaveChanges();
                    LogAction("Sector " + sector.SectorName + " with ID " + sector.SectorID + " created.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        // This method checks if sector already exists in database
        public bool IsSectorExists(string sectorName)
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    tblSector sector = context.tblSectors.Where(x => x.SectorName == sectorName).FirstOrDefault();
                    if (sector != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return false;
            }
        }

        // This method finds sector in DbSet based on a forwared name
        public int FindSector(string sectorName)
        {
            try
            {
                using (EmployeeDBEntities context = new EmployeeDBEntities())
                {
                    return context.vwSectors.Where(x => x.SectorName == sectorName).Select(x => x.SectorID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception" + ex.Message.ToString());
                return 0;
            }
        }
    }
}
