-- Insert dummy data into Groups table
INSERT INTO dbo.Groups (name)
VALUES ('Group A'),
       ('Group B'),
       ('Group C');

-- Insert dummy data into Users table
INSERT INTO dbo.Users (UUID, FullName, Score)
VALUES ('123e4567-e89b-12d3-a456-426614174000', 'John Doe', 100),
       ('123e4567-e89b-12d3-a456-426614174001', 'Jane Doe', 200),
       ('123e4567-e89b-12d3-a456-426614174002', 'Bob Smith', 150);

-- Insert dummy data into GroupUsers table
INSERT INTO dbo.GroupUsers (user_id, group_id, user_is_admin)
VALUES (1, 1, 1),
       (2, 1, 0),
       (3, 2, 1);

-- Insert dummy data into Journeys table
INSERT INTO dbo.Journeys (User_ID, Total_Distance, Total_Emission, Start, [End], Date, Score)
VALUES (1, 100, 80, 'Seattle', 'Portland', '2022-01-01', 100),
       (2, 200, 0, 'New York', 'Boston', '2022-02-01', 200),
       (3, 150, 0, 'Los Angeles', 'San Francisco', '2022-03-01', 150);

-- Insert dummy data into Stopovers table
INSERT INTO dbo.Stopovers (Vehicle_ID, Journey_ID, Distance, Start, [End], Emission)
VALUES (1, 1, 50, 'Seattle', 'Olympia', 40),
       (1, 1, 50, 'Olympia', 'Portland', 40),
       (2, 2, 100, 'New York', 'Hartford', 0),
       (2, 2, 100, 'Hartford', 'Boston', 0),
       (3, 3, 75, 'Los Angeles', 'San Jose', 0),
       (3, 3, 75, 'San Jose', 'San Francisco', 0);