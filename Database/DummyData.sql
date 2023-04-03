USE evv;

DELETE FROM Categories;
DELETE FROM Users;
DELETE FROM Journeys;
DELETE FROM Stopovers;

DBCC CHECKIDENT (Categories, RESEED, 0);
DBCC CHECKIDENT (Users, RESEED, 0);
DBCC CHECKIDENT (Journeys, RESEED, 0);
DBCC CHECKIDENT (Stopovers, RESEED, 0);


INSERT INTO Users (UUID, FullName)
VALUES
('Billy-277a-4d11-9c85-9ae01d06df43', 'Billy Hofland'),
('Rowan-5e4f-4b4d-bb81-9e9f9d56f59c', 'Rowan Battles'),
('Stijn-3e8c-4fae-bd36-482c225b9f38', 'Stijn van der Vliet'),
('Joost-67e6-46a8-8a69-cf26e7bca3b9', 'Joost Raemakers'),
('Jonathan-9010-43c1-86fb-8f81cd787d62', 'Jonathan Kitten');

INSERT INTO Categories (Vehicle_Name, Emission)
VALUES
('Walking', 0),
('Bicycle', 0),
('E-Train', 2),
('E-Bicycle', 3),
('E-Scooter', 17),
('E-Car', 54),
('G-Scooter', 62),
('D-Train', 90),
('Tram', 96),
('Bus', 96),
('Metro', 96),
('H-Car', 110),
('G-Motorcycle', 129),
('D-Car', 131),
('G-Car', 149)


INSERT INTO Journeys (User_ID, Total_Distance, Total_Emission, [Start], [End], Date, Score)
VALUES
(1, 23.75, 2112, 'Thuis', 'Fontys ICT', '2023-03-20', 202),
(2, 2.8, 0, 'Thuis', 'Fontys ICT', '2023-03-20', 500),
(3, 14.3, 26, 'Thuis', 'Fontys ICT', '2022-03-03', 494),
(4, 96.5, 178, 'Thuis', 'Fontys ICT', '2022-03-03', 494),
(5, 55, 108, 'Thuis', 'Fontys ICT', '2022-03-03', 494);

INSERT INTO Stopovers (Journey_ID, Vehicle_ID, Distance, [Start], [End], Emission)
VALUES 
(1, 1, 0.75, 'Thuis', 'Varendonk', 0),
(1, 10, 22, 'Varendonk', 'Eindhoven Centraal', 2112),
(1, 1, 1, 'Eindhoven Centraal', 'Fontys ICT', 0),
(2, 2, 2.8, 'Thuis', 'Fontys ICT', 0),
(3, 1, 0.3, 'Thuis', 'Helmond het Hout', 0),
(3, 3, 13, 'Helmond het Hout', 'Eindhoven Centraal', 26),
(3, 1, 1, 'Eindhoven Centraal', 'Fontys ICT', 0),
(4, 2, 6.5, 'Thuis', 'Maastricht Centraal', 0),
(4, 3, 89.0, 'Maastricht Centraal', 'Eindhoven Centraal', 178),
(4, 1, 1, 'Eindhoven Centraal', 'Fontys ICT', 0),
(5, 2, 8, 'Thuis', 'Zaltbommel Station', 0),
(5, 3, 14, 'Zaltbommel Station', 's-Hertogenbosch Centraal', 28),
(5, 3, 40, 's-Hertogenbosch Centraal', 'Eindhoven Centraal', 80),
(5, 1, 1, 'Eindhoven Centraal', 'Fontys ICT', 0);

SELECT * FROM Categories;
SELECT * FROM Journeys;
SELECT * FROM Stopovers;
SELECT * FROM Users;	