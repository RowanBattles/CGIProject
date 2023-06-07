-- Insert dummy data into Groups table
INSERT INTO dbo.Groups (name)
VALUES ('Group A'),
       ('Group B'),
       ('Group C');

-- Insert dummy data into Users table
INSERT INTO dbo.Users (UUID, FullName, Score)
VALUES ('123e4567-e89b-12d3-a456-426614174000', 'John Doe', 100),
       ('123e4567-e89b-12d3-a456-426614174001', 'Jane Doe', 200),
       ('123e4567-e89b-12d3-a456-426614174002', 'Bob Smith', 150),
	   ('123e4567-e89b-12d3-a456-426614174003', 'Alice Johnson', 120),
       ('123e4567-e89b-12d3-a456-426614174004', 'Bob Johnson', 130),
       ('123e4567-e89b-12d3-a456-426614174005', 'Charlie Johnson', 140),
       ('123e4567-e89b-12d3-a456-426614174006', 'Dave Johnson', 150),
       ('123e4567-e89b-12d3-a456-426614174007', 'Eve Johnson', 160),
       ('123e4567-e89b-12d3-a456-426614174008', 'Frank Johnson', 170),
       ('123e4567-e89b-12d3-a456-426614174009', 'Grace Johnson', 180),
       ('123e4567-e89b-12d3-a456-426614174010', 'Heidi Johnson', 190),
       ('123e4567-e89b-12d3-a456-426614174011', 'Ivan Johnson', 200),
       ('123e4567-e89b-12d3-a456-426614174012', 'Judy Johnson', 210);
-- Insert dummy data into GroupUsers table
INSERT INTO dbo.GroupUsers (user_id, group_id, user_is_admin)
VALUES (1, 1, 1),
       (2, 1, 0),
       (3, 2, 1),
	   (4, 1, 0),
       (5, 1, 0),
       (6, 2, 0),
       (7, 2, 0),
       (8, 3, 0),
       (9, 3, 0),
       (10, 1, 0),
       (11, 1, 0),
       (12, 2, 0),
       (13, 2, 0);

-- Insert dummy data into Journeys table
INSERT INTO dbo.Journeys (User_ID, Total_Distance, Total_Emission, Start, [End], Date, Score)
VALUES (1, 100, 80, 'Seattle', 'Portland', '2022-01-01', 100),
       (2, 200, 0, 'New York', 'Boston', '2022-02-01', 200),
       (3, 150, 0, 'Los Angeles', 'San Francisco', '2022-03-01', 150),
	   (4, 100, 80, 'Seattle', 'Portland', '2022-01-01', 100),
       (5, 200, 0, 'New York', 'Boston', '2022-02-01', 200),
       (6, 150, 0, 'Los Angeles', 'San Francisco', '2022-03-01', 150),
       (7, 120, 90, 'Chicago', 'Milwaukee', '2022-04-01', 120),
       (8, 130, 100, 'Houston', 'Dallas', '2022-05-01', 130),
       (9, 140, 110, 'Miami', 'Orlando', '2022-06-01', 140),
       (10, 150, 120, 'Atlanta', 'Nashville', '2022-07-01', 150),
       (11, 160, 130, 'Denver', 'Colorado Springs', '2022-08-01', 160),
       (12, 170, 140, 'Phoenix', 'Tucson', '2022-09-01', 170),
       (13, 180, 150, 'Philadelphia', 'Pittsburgh', '2022-10-01', 180);

-- Insert dummy data into Stopovers table
INSERT INTO dbo.Stopovers (Vehicle_ID, Journey_ID, Distance, Start, [End], Emission)
VALUES (1, 1, 50, 'Seattle', 'Olympia', 40),
       (1, 1, 50, 'Olympia', 'Portland', 40),
       (2, 2, 100, 'New York', 'Hartford', 0),
       (2, 2, 100, 'Hartford', 'Boston', 0),
       (3, 3, 75, 'Los Angeles', 'San Jose', 0),
       (3, 3, 75, 'San Jose', 'San Francisco', 0),
	   (1, 4, 50, 'Seattle', 'Olympia', 40),
       (1, 4, 50, 'Olympia', 'Portland', 40),
       (2, 5, 100, 'New York', 'Hartford', 0),
       (2, 5, 100, 'Hartford', 'Boston', 0),
       (3, 6, 75, 'Los Angeles', 'San Jose', 0),
       (3, 6, 75, 'San Jose', 'San Francisco', 0),
       (1, 7, 60, 'Chicago', 'Milwaukee', 45),
       (2, 8, 65, 'Houston', 'Dallas', 0),
       (3, 9, 70, 'Miami', 'Orlando', 0),
       (1,10 ,75,'Atlanta','Nashville',60),
       (2 ,11 ,80,'Denver','Colorado Springs',0),
       (3 ,12 ,85,'Phoenix','Tucson',0),
       (1 ,13 ,90,'Philadelphia','Pittsburgh',70);