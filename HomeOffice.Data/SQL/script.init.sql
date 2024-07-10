DROP TABLE Time;

CREATE TABLE
    IF NOT EXISTS Users (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Username TEXT,
        Email TEXT,
        Password TEXT
    );

INSERT INTO
    Users (Username, Email, Password)
VALUES
    (
        'Abdelmalek',
        'malek_yamouni@gmx.de',
        ''
    )

CREATE TABLE
    IF NOT EXISTS Time(
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        UserId INTEGER NOT NULL,
        Date DATE NOT NULL,
        TotalMinutes INTEGER NOT NULL DEFAULT 0,
        FOREIGN KEY (UserId) REFERENCES Users (Id)
    );

UPDATE Time
SET TotalMinutes = 0
WHERE Id = 1;