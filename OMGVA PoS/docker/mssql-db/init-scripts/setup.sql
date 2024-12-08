USE [master];
GO

-- Create a login
CREATE LOGIN [omgva]
    WITH PASSWORD = 'admin', 
    CHECK_POLICY = OFF;
GO

-- Create a new database
CREATE DATABASE OmgvaPos;
GO

-- Use the new database to associate the user with it
USE OmgvaPos;
GO

CREATE USER [omgva] 
FOR LOGIN [omgva];
GO

-- Assign the user to the db_owner role
ALTER ROLE [db_owner] 
ADD MEMBER [omgva];
GO