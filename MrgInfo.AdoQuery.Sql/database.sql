SET STATISTICS PROFILE ON
SET STATISTICS TIME ON

CREATE DATABASE AdoQuery
GO

CREATE LOGIN AdoQuery WITH PASSWORD = 'AdoQuery', CHECK_POLICY = OFF, DEFAULT_DATABASE = AdoQuery
--- CREATE LOGIN DOMAIN\USER FROM WINDOWS WITH DEFAULT_DATABASE = AdoQuery
GO

USE AdoQuery

CREATE USER AdoQuery FOR LOGIN AdoQuery WITH DEFAULT_SCHEMA = dbo
GO

EXEC sp_addrolemember 'db_owner', 'AdoQuery'
GO

CREATE TABLE Product(
    ProductId INTEGER NOT NULL PRIMARY KEY,
    Code VARCHAR(5),
    Name VARCHAR(100),
    UnitPrice NUMERIC(6,2) NOT NULL)
GO

INSERT INTO Product VALUES(10, 'AB123', 'Leather Sofa', 1000)
INSERT INTO Product VALUES(20, 'AB456', 'Baby Chair', 200.25)
INSERT INTO Product VALUES(30, 'AB789', 'Sport Shoes', 250.60)
INSERT INTO Product VALUES(40, 'PQ123', 'Sony Digital Camera', 399)
INSERT INTO Product VALUES(50, 'PQ456', 'Hitachi HandyCam', 1050)
INSERT INTO Product VALUES(60, 'PQ789', 'GM Saturn', 2250.99)
INSERT INTO Product VALUES(70, 'PQ945', null, 150.15)
GO