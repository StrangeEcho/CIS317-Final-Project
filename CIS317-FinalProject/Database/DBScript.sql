CREATE TABLE IF NOT EXISTS Clients 
(
    ClientId INTEGER PRIMARY KEY,
    Username VARCHAR(50),
    Name VARCHAR(75),
    Email VARCHAR(50),
    Balance NUMERIC
);

CREATE TABLE IF NOT EXISTS Bankers
(
    BankerId INTEGER PRIMARY KEY,
    Username VARCHAR(50),
    Password VARCHAR(100)
)