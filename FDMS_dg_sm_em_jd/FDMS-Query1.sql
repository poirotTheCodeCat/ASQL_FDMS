Use GroundTerminal; 

DROP TABLE IF EXISTS Telemetry ; 
DROP TABLE IF EXISTS AircraftInfo;
DROP TABLE IF EXISTS GForce; 
DROP TABLE IF EXISTS Altitude;
DROP VIEW IF EXISTS TelemetryData;
DROP PROCEDURE IF EXISTS sp_Insert;

CREATE TABLE AircraftInfo (
TailNum varchar(10) NOT NULL, 
PRIMARY KEY (TailNum)
);

CREATE TABLE GForce (
GForceID int identity(1,1) primary key,
Accel_x float, 
Accel_y float,
Accel_z float, 
Weight float,
);

CREATE TABLE Altitude (
AltitudeID int identity(1,1) primary key,
Altitude float, 
Pitch float,
Bank float, 
);

CREATE TABLE Telemetry (
TelemetryID int identity(1,1) primary key,
TailNum varchar(10),
GForceID int,
AltitudeID int, 
TimeStamp datetime,
FOREIGN KEY (TailNum) REFERENCES AircraftInfo(TailNum),
FOREIGN KEY (GForceID) REFERENCES GForce(GForceID), 
FOREIGN KEY (AltitudeID) REFERENCES Altitude(AltitudeID)
);


