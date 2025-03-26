create table staff
(
	staffID int primary key identity,
	sName varchar(50),
	sPhone varchar(50),
	sRole varchar(50),
	sNIC varchar(20)
)

SELECT * FROM staff;

SELECT sName FROM staff WHERE LOWER(sRole) = 'technician'

create table tblMain
(
MainID int primary key identity,
aDate Date,
aTime Varchar(15),
TechnicianName varchar(50),
status varchar(15),
total float,
received float,
change float
)

create table tblDetails
(
DetailID int primary key identity,
MainId int,
prodID int,
qty int,
price float,
amount float,
)

select * from tblMain m 
inner join tblDetails d on m.MainID = d.MainId



create table customer
(
cusID int primary key identity,
aDate Date,
cusName Varchar(50),
cusPhone varchar(20),
cusEmail varchar(50),
cusVehicleType varchar(50),
cusVehicleNo varchar(50)
)