USE Instructor

CREATE TABLE Instructor (
    InstructorID INT PRIMARY KEY IDENTITY(1,1), -- Primary Key with auto-increment
    Username NVARCHAR(50) NOT NULL,              -- Username (required)
    Password NVARCHAR(255) NOT NULL,             -- Password (required)
    FirstName NVARCHAR(100) NULL,                -- First name
    LastName NVARCHAR(100) NULL,                 -- Last name
    Phone NVARCHAR(20) NULL,                     -- Phone number
    Email NVARCHAR(100) NULL,                    -- Email address
    Certification NVARCHAR(100) NULL,            -- Certification information
    HireDate DATETIME NULL,                      -- Date of hire
    ProfileImage VARBINARY(MAX) NULL,            -- Profile image
    HasSubmittedForm BIT NULL,                   -- Indicates if form was submitted
    LicenseImage VARBINARY(MAX) NULL,            -- License image
    Salary DECIMAL(18, 2) NULL                   -- Salary field (decimal type for monetary values)
);
DBCC CHECKIDENT ('Instructor', RESEED, 2000);

SELECT * FROM Instructor;

