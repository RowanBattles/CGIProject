USE evv;

DELETE FROM Categories;

DBCC CHECKIDENT (Categories, RESEED, 0);

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


SELECT * FROM Categories;
