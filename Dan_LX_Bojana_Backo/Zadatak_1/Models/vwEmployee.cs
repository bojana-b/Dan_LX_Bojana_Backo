//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zadatak_1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwEmployee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string NumberOfIdentityCard { get; set; }
        public string JMBG { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int Sector { get; set; }
        public int LocationID { get; set; }
        public Nullable<int> Manager { get; set; }
        public string Employee { get; set; }
        public string GenderIdentity { get; set; }
        public string SectorName { get; set; }
        public string Location { get; set; }
        public string ManagerName { get; set; }
    }
}
