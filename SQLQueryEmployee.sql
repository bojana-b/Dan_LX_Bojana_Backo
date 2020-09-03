IF DB_ID('EmployeeDB') IS NULL
    create database EmployeeDB;
GO	
use EmployeeDB
--Deleting tables and views, if they exist
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tblEmployee')
	drop table tblEmployee;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tblSector')
	drop table tblSector;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tblGender')
	drop table tblGender;
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tblLocation')
	drop table tblLocation;
IF EXISTS(select * FROM sys.views where name = 'vwEmployee')
	drop view vwEmployee;
IF EXISTS(select * FROM sys.views where name = 'vwSector')
	drop view vwSector;
IF EXISTS(select * FROM sys.views where name = 'vwGender')
	drop view vwGender;
IF EXISTS(select * FROM sys.views where name = 'vwLocation')
	drop view vwLocation;

--Creating a table of locations
use EmployeeDB
create table tblLocation(
	LocationID int IDENTITY(1,1) PRIMARY KEY,
	Address varchar(50) NOT NULL,
	City varchar(50) NOT NULL,
	State varchar(50) NOT NULL,
);
--Creating a table of genders
use EmployeeDB
create table tblGender(
	GenderID int IDENTITY(1,1) PRIMARY KEY,
	GenderIdentity varchar(10) NOT NULL
);
--Creating a table of sectors
use EmployeeDB
create table tblSector(
	SectorID int IDENTITY(1,1) PRIMARY KEY,
	SectorName varchar(100) UNIQUE NOT NULL
);
--Creating a table of employees
use EmployeeDB
create table tblEmployee(
	EmployeeID int IDENTITY(1,1) PRIMARY KEY,
	Name varchar(30) NOT NULL,
	Surname varchar(50) NOT NULL,
	DateOfBirth date NOT NULL,
	NumberOfIdentityCard varchar(9) UNIQUE NOT NULL,
	JMBG varchar(13) UNIQUE NOT NULL,
	Gender int FOREIGN KEY REFERENCES tblGender(GenderID) NOT NULL,
	PhoneNumber varchar(13) UNIQUE NOT NULL,
	Sector int FOREIGN KEY REFERENCES tblSector(SectorID) NOT NULL,
	LocationID int FOREIGN KEY REFERENCES tblLocation(LocationID) NOT NULL,
	Manager int FOREIGN KEY REFERENCES tblEmployee(EmployeeID),
	CONSTRAINT CHK_DateOfBirth CHECK(DATEDIFF(day,DateOfBirth,GETDATE())>= 5479),
	CONSTRAINT CHK_JMBG CHECK(LEN(JMBG)=13 AND ISNUMERIC(JMBG)=1),
	CONSTRAINT CHK_PhoneNumber CHECK(PhoneNumber LIKE'+3816[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]' OR PhoneNumber LIKE '+3816[0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
);

--Inserting values into table of genders
insert into tblGender values
	('Male'),
	('Female'),
	('Other');

--Creating a view of locations
GO
create view vwLocation as
	select *, Address + ', '+ City + ', ' + State 'Location'
	from tblLocation;

--Creating a view of genders
GO
create view vwGender as
	select *
	from tblGender;

--Creating a view of sectors
GO
create view vwSector as
	select *
	from tblSector;

--Creating a view of employees
GO
create view vwEmployee as
	select e.*, e.Name + ' ' + e.Surname 'Employee', g.GenderIdentity, s.SectorName, l.Address +', '+ l.City +', '+ l.State 'Location', m.Name + ' ' + m.Surname 'ManagerName'
	from tblEmployee e 
	INNER JOIN tblLocation l 
	ON e.LocationID = l.LocationID
	INNER JOIN tblGender g
	ON e.Gender = g.GenderID
	INNER JOIN tblSector s
	ON e.Sector = s.SectorID
	LEFT JOIN tblEmployee m
	ON e.Manager = m.EmployeeID;