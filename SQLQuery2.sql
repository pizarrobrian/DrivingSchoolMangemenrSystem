Use StudentInfo;

CREATE TABLE Students (
    StudentID INT PRIMARY KEY IDENTITY(1,1),    -- Primary Key with auto-increment
    Username VARCHAR(50) NOT NULL,             -- Username (required)
    Password VARCHAR(20) NOT NULL,             -- Password (required)
    FirstName NVARCHAR(100) NULL,              -- First name
    LastName NVARCHAR(100) NULL,               -- Last name
    DateOfBirth DATE NULL,                     -- Date of birth
    Phone NVARCHAR(20) NULL,                   -- Phone number
    Email NVARCHAR(100) NULL,                  -- Email address
    Address NVARCHAR(200) NULL,                -- Address
    ProfileImage VARBINARY(MAX) NULL,          -- Profile image
    HasSubmittedForm BIT NULL,                 -- Indicates if form was submitted
    Balance INT NULL                           -- Account balance
);

SELECT * FROM Students;
DBCC CHECKIDENT ('Students', RESEED, 1000);
DELETE FROM Students WHERE StudentID IN (1001, 1005, 1006, 1007);

UPDATE Students
SET Balance = 8000
WHERE Balance IS NULL;

