CREATE DATABASE SampleDB

USE SampleDb

CREATE TABLE Users(Id int not null, [Name] Nvarchar(150) not null, Email Nvarchar(150) not null, [Profile]  Nvarchar(150) PRIMARY KEY(Id))

CREATE TABLE Roles(Id int not null, RoleName Nvarchar(150) PRIMARY KEY (Id))

CREATE TABLE UserRoles(UserId int not null, RoleId int not null Primary key(UserId, RoleId)
	CONSTRAINT Fk_User_UserId Foreign Key(UserId) references SampleDb.dbo.Users(Id),
	CONSTRAINT Fk_Role_RoleId Foreign Key(RoleId) references SampleDb.dbo.Roles(Id))

INSERT INTO Users(Id, [Name], Email, [Profile])
VALUES(1, 'Mike', 'mike@sample.com', 'Everyone'),
      (2, 'John', 'john@sample.com', 'Everyone')

INSERT INTO Roles(Id, RoleName)
VALUES(10, 'Supervisor'),
	  (20, 'Administrator')

INSERT INTO UserRoles(UserId, RoleId)
VALUES(1, 10),
	  (1, 20),
	  (2, 20)

-- =============== Install this tool, if you don't already have it
--dotnet tool install --global dotnet-ef --version 3.1.3

--================== run this command to generate the entities and DbContext
-- dotnet ef dbcontext scaffold "Server=DVTL597LKC2;Initial Catalog=SampleDb;Integrated Security=True;" Microsoft.EntityFrameworkCore.SqlServer -o DBContext -c "SampleDbContext"
