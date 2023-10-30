CREATE TABLE Client (
  Id SERIAL PRIMARY KEY NOT NULL,
  UserName VARCHAR NOT NULL,
  Email VARCHAR NOT NULL,
  Phone VARCHAR NOT NULL,
  Role VARCHAR NOT NULL,
  RegistrationDate TIMESTAMP,
  Password VARCHAR NOT NULL,
  Amount float NOT NULL
);

CREATE TABLE Payment (
  Id SERIAL PRIMARY KEY NOT NULL,
  Name VARCHAR NOT NULL,
  Description VARCHAR NOT NULL,
  Cost FLOAT NOT NULL,
  DateOperazion TIMESTAMP NOT NULL,
  ClientId INTEGER,
  FOREIGN KEY (ClientId) REFERENCES Client (Id)
);

CREATE TABLE Transport (
  Id SERIAL PRIMARY KEY NOT NULL,
  Type VARCHAR NOT NULL,
  Speed FLOAT,
  RentPrice FLOAT NOT NULL,
  Color VARCHAR,
  Availability BOOLEAN NOT NULL
);

CREATE TABLE Rental (
  Id SERIAL PRIMARY KEY NOT NULL,
  Name VARCHAR NOT NULL,
  Description VARCHAR NOT NULL,
  DurationDays INTEGER CHECK (DurationDays >= 0),
  DurationHours INTEGER CHECK (DurationHours >= 0 AND DurationHours < 24),
  DurationMinutes INTEGER NOT NULL CHECK (DurationMinutes >= 0 AND DurationMinutes < 60),
  TotalCost FLOAT,
  TransportId INTEGER,
  ClientId INTEGER,
  FOREIGN KEY (TransportId) REFERENCES Transport (Id),
  FOREIGN KEY (ClientId) REFERENCES Client (Id)
);


INSERT INTO Client (UserName, Email, Phone, Role, RegistrationDate, Password, Amount)
VALUES
  ('John', 'john@example.com', '1234567890', 'user', NOW(), 'password123', 10.5),
  ('Alice', 'alice@example.com', '9876543210', 'user', NOW(), 'securepass', 0.0),
  ('Bob', 'bob@example.com', '5555555555', 'admin', NOW(), 'bobpass', 1480.0),
  ('Emily', 'emily@example.com', '3333333333', 'user', NOW(), 'emilypass', 0.0),
  ('David', 'david@example.com', '7777777777', 'user', NOW(), 'davidpass', 25.0);


INSERT INTO Payment (Name, Description, Cost, DateOperazion, ClientId)
VALUES
  ('Payment 1', 'Payment Description 1', 5.0, NOW() - interval '7 days', 1),
  ('Payment 2', 'Payment Description 2', 2.5, NOW() - interval '5 days', 1),
  ('Payment 3', 'Payment Description 3', 100.50, NOW() - interval '30 days', 3),
  ('Payment 4', 'Payment Description 4', 500.0, NOW() - interval '15 days', 3),
  ('Payment 5', 'Payment Description 5', 10.0, NOW() - interval '10 days', 5);


INSERT INTO Transport (Type, Speed, RentPrice, Color, Availability)
VALUES
  ('Automobile', 50.0, 20.0, 'red', true),
  ('Scooter', 20.0, 10.0, 'blue', false),
  ('Bicycle', 15.0, 5.0, 'green', true),
  ('Motorcycle', 80.0, 25.0, 'black', true),
  ('Automobile', 45.0, 18.0, 'silver', false),
  ('Scooter', 22.0, 12.0, 'yellow', true),
  ('Bicycle', 18.0, 7.0, 'orange', true),
  ('Motorcycle', 85.0, 28.0, 'blue', false),
  ('Automobile', 55.0, 21.0, 'green', false),
  ('Scooter', 21.0, 11.0, 'red', true),
  ('Bicycle', 16.0, 6.0, 'blue', false),
  ('Motorcycle', 75.0, 27.0, 'silver', true);

INSERT INTO Rental (Name, Description, DurationDays, DurationHours, DurationMinutes, TotalCost, TransportId)
VALUES
  ('Аренда автомобиля', 'Аренда автомобиля на час', 0, 1, 0, 20.0, 1),
  ('Аренда мотоцикла', 'Аренда мотоцикла на 30 минут', 0, 1, 0, 15.0, 3),
  ('Аренда велосипеда', 'Аренда велосипеда на 1 день', 1, 0, 0, 10.0, 4),
  ('Аренда автомобиля', 'Аренда автомобиля на 2 часа', 0, 2, 0, 40.0, 5),
  ('Аренда самоката', 'Аренда самоката на 15 минут', 0, 0, 15, 5.0, 2);

