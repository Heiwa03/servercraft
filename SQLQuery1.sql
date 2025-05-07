SELECT TOP (1000) [Id]
      ,[Name]
      ,[Description]
      ,[Price]
      ,[OldPrice]
      ,[ImageUrl]
      ,[Badge]
      ,[InStock]
      ,[Category]
  FROM [servercraft].[dbo].[Servers]

  INSERT INTO [servercraft].[dbo].[Servers]
([Id], [Name], [Description], [Price], [OldPrice], [ImageUrl], [Badge], [InStock], [Category])
VALUES
('server-101', 'Dell PowerEdge T40', 'Entry-level tower server for small business', 599, NULL, '"C:\Users\grigo\Desktop\servercraft\servercraft\Content\images\products\server1.png"', 'New', 1, 'Enterprise'),
('server-102', 'HP ProLiant ML30', 'Affordable server for growing businesses', 799, 899, '"C:\Users\grigo\Desktop\servercraft\servercraft\Content\images\products\server2.png"', 'Best Seller', 1, 'Enterprise');

DELETE FROM [servercraft].[dbo].[Servers];

INSERT INTO [servercraft].[dbo].[Servers]
([Id], [Name], [Description], [Price], [OldPrice], [ImageUrl], [Badge], [InStock], [Category])
VALUES
('server-104', 'StorageBox 8TB', 'High-capacity storage server', 1299, NULL, '/Content/images/products/server4.jpg', 'Popular', 1, 'Storage');


UPDATE [servercraft].[dbo].[Servers]
SET [ImageUrl] = '/Content/images/products/server3.jpg'
WHERE [Id] = 'server-101';

UPDATE [servercraft].[dbo].[Servers]
SET [ImageUrl] = '/Content/images/products/server6.png'
WHERE [Id] = 'server-106';

UPDATE [servercraft].[dbo].[Servers]
SET [ImageUrl] = '/Content/images/products/server4.jpg'
WHERE [Id] = 'server-104';

INSERT INTO [servercraft].[dbo].[Servers]
([Id], [Name], [Description], [Price], [OldPrice], [ImageUrl], [Badge], [InStock], [Category])
VALUES
('server-105', 'Lenovo ThinkSystem SR650', 'High-density enterprise server for mission-critical workloads', 2199, 2499, '/Content/images/products/server5.png', 'New', 1, 'Enterprise'),
('server-106', 'Fujitsu PRIMERGY TX1330', 'Flexible tower server for small and medium businesses', 999, NULL, '/Content/images/products/server6.png', NULL, 1, 'Enterprise');

-- Cloud Servers
INSERT INTO [servercraft].[dbo].[Servers]
([Id], [Name], [Description], [Price], [OldPrice], [ImageUrl], [Badge], [InStock], [Category])
VALUES
('server-107', 'CloudX Mini', 'Compact cloud server for scalable apps', 999, NULL, '/Content/images/products/server7.jpg', NULL, 1, 'Cloud');
('server-202', 'CloudFlex X2', 'Flexible cloud server with high-density compute', 1399, 1599, '/Content/images/products/server8.jpg', 'Best Seller', 1, 'Cloud');

INSERT INTO [servercraft].[dbo].[Servers]
([Id], [Name], [Description], [Price], [OldPrice], [ImageUrl], [Badge], [InStock], [Category])
VALUES
('server-108', 'Lenovo ThinkSystem ST50', 'Reliable entry-level server for SMBs', 699, NULL, '/Content/images/servers/server8.jpg', 'New', 1, 'Enterprise'),
('server-109', 'SuperMicro E302-9D', 'Compact server optimized for edge computing', 1199, 1299, '/Content/images/servers/server9.jpg', NULL, 1, 'Edge'),
('server-110', 'Dell PowerEdge R740', 'High-performance rack server for virtualization', 2499, 2699, '/Content/images/servers/server10.jpg', 'Best Seller', 1, 'Enterprise');