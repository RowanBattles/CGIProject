DELETE FROM [dbo].[Categories];
DELETE FROM [dbo].[Journeys];
DELETE FROM [dbo].[Stopovers];
DELETE FROM [dbo].[Users];

DBCC CHECKIDENT ('[dbo].[Categories]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Journeys]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Stopovers]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 0);

INSERT INTO [dbo].[Users] ([UUID])
VALUES 
('4d4cb605-277a-4d11-9c85-9ae01d06df43'),
('c12f1845-5e4f-4b4d-bb81-9e9f9d56f59c'),
('2ef14f51-3e8c-4fae-bd36-482c225b9f38'),
('f16e281b-67e6-46a8-8a69-cf26e7bca3b9'),
('d67fa413-9010-43c1-86fb-8f81cd787d62');

INSERT INTO [dbo].[Categories] ([Vehicle_Name], [Emission])
VALUES 
('Car', 100),
('Bike', 20),
('Bus', 200),
('Train', 50),
('Plane', 500);

INSERT INTO [dbo].[Journeys] ([User_ID], [Total_Distance], [Total_Emission], [Start], [End], [Date])
VALUES 
(1, 20, 100, 'New York', 'Boston', '2022-01-01'),
(2, 30, 120, 'San Francisco', 'Los Angeles', '2022-02-02'),
(3, 50, 250, 'London', 'Paris', '2022-03-03'),
(4, 100, 500, 'Sydney', 'Melbourne', '2022-04-04'),
(5, 200, 1000, 'Tokyo', 'Osaka', '2022-05-05');

INSERT INTO [dbo].[Stopovers] ([Vehicle_ID], [Journey_ID], [Distance], [Start], [End], [Emission])
VALUES 
(1, 1, 10, 'New York', 'Philadelphia', 50),
(1, 1, 10, 'Philadelphia', 'Boston', 50),
(2, 2, 30, 'San Francisco', 'Los Angeles', 60),
(3, 3, 20, 'London', 'Calais', 100),
(3, 3, 30, 'Calais', 'Paris', 100),
(4, 4, 50, 'Sydney', 'Canberra', 25),
(4, 4, 50, 'Canberra', 'Melbourne', 25),
(5, 5, 100, 'Tokyo', 'Kyoto', 250),
(5, 5, 100, 'Kyoto', 'Osaka', 250);

SELECT * FROM [dbo].[Users];
SELECT * FROM [dbo].[Categories];
SELECT * FROM [dbo].[Journeys];
SELECT * FROM [dbo].[Stopovers];