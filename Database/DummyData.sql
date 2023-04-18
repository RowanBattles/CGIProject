USE [evv]

DELETE FROM [dbo].[Categories];
DELETE FROM [dbo].[Journeys];
DELETE FROM [dbo].[Stopovers];
DELETE FROM [dbo].[Users];

DBCC CHECKIDENT ('[dbo].[Categories]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Journeys]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Stopovers]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Users]', RESEED, 0);

INSERT INTO [dbo].[Users] ([UUID], [FullName], [Score])
VALUES 
('Billy-277a-4d11-9c85-9ae01d06df43', 'Billy Hofland', 20),
('Rowan-5e4f-4b4d-bb81-9e9f9d56f59c', 'Rowan Battles', 0),
('Stijn-3e8c-4fae-bd36-482c225b9f38', 'Stijn van der Vliet', 0),
('Joost-67e6-46a8-8a69-cf26e7bca3b9', 'Joost Raemakers', 0),
('Jonathan-9010-43c1-86fb-8f81cd787d62', 'Jonathan Kitten', 0);

INSERT INTO [dbo].[Categories] ([Vehicle_Name], [Emission])
VALUES 
('Walking', 0),
('Bicycle', 0),
('E-Bicycle', 5),
('B-Scooter', 53),
('E-Scooter', 19),
('Tram', 57),
('Train', 0),
('B-Scooter', 64),
('Bus', 140),
('E-Car', 53),
('H-Car', 127),
('D-Car', 213),
('B-Car', 224),
('Motorcycle', 137);

INSERT INTO [dbo].[Journeys] ([User_ID], [Total_Distance], [Total_Emission], [Start], [End], [Date], [Score])
VALUES 
(1, 23.75, 3080, 'Thuis', 'Fontys ICT', '2023-03-20', 500),
(2, 2.8, 0, 'Thuis', 'Fontys ICT', '2023-03-20', 400),
(3, 14.3, 0, 'Thuis', 'Fontys ICT', '2022-03-03', 300),
(4, 96.5, 0, 'Thuis', 'Fontys ICT', '2022-03-03', 200),
(5, 55, 0, 'Thuis', 'Fontys ICT', '2022-03-03', 100);

INSERT INTO [dbo].[Stopovers] ([Vehicle_ID], [Journey_ID], [Distance], [Start], [End], [Emission])
VALUES 
(1, 1, 0.75, 'Thuis', 'Varendonk', 0),
(9, 1, 22, 'Varendonk', 'Eindhoven Centraal', 3080),
(1, 1, 1, 'Eindhoven Centraal', 'Fontys ICT', 0),
(1, 2, 2.8, 'Thuis', 'Fontys ICT', 0),
(1, 3, 0.3, 'Thuis', 'Helmond het Hout', 0),
(7, 3, 13, 'Helmond het Hout', 'Eindhoven Centraal', 0),
(1, 3, 1, 'Eindhoven Centraal', 'Fontys ICT', 0),
(2, 4, 6.5, 'Thuis', 'Maastricht Centraal', 0),
(7, 4, 89.0, 'Maastricht Centraal', 'Eindhoven Centraal', 0),
(1, 4, 1, 'Eindhoven Centraal', 'Fontys ICT', 0),
(2, 5, 10, 'Thuis', 'Zaltbommel Station', 0),
(7, 5, 14, 'Zaltbommel Station', 's-Hertogenbosch Centraal', 0),
(7, 5, 40, 's-Hertogenbosch Centraal', 'Eindhoven Centraal', 0),
(1, 5, 1, 'Eindhoven Centraal', 'Fontys ICT', 0);

SELECT * FROM [dbo].[Users];
SELECT * FROM [dbo].[Categories];
SELECT * FROM [dbo].[Journeys];
SELECT * FROM [dbo].[Stopovers];