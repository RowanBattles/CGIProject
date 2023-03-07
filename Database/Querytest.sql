SET IDENTITY_INSERT dbo.Journeys off;
SET IDENTITY_INSERT dbo.Users on;
DELETE FROM evv.dbo.Users WHERE User_ID = 1;
DELETE FROM evv.dbo.Users WHERE User_ID = 2;
INSERT INTO evv.dbo.Users(User_ID, UUID)
VALUES (1,1), 
(2, 40);
SET IDENTITY_INSERT dbo.Users off;
DELETE FROM evv.dbo.Journeys WHERE Journey_ID = 2;
DELETE FROM evv.dbo.Journeys WHERE Journey_ID = 1;

SET IDENTITY_INSERT dbo.Journeys on;
INSERT INTO evv.dbo.Journeys(Journey_ID, User_ID, Total_Distance, Total_Emission, "Start", "End", "Date")
VALUES (1, 1, 25, 20, 5, 5, '2023-03-07'),
(2, 2, 10, 15, 2, 2, '2022-02-05');
SELECT * FROM dbo.Journeys;