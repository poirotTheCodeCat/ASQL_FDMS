CREATE VIEW TelemetryData AS 
SELECT TailNum, Accel_x, Accel_y, Accel_z, Weight, a.Altitude AS "Altitude", Pitch, Bank, TimeStamp
FROM Telemetry AS t
	INNER JOIN GForce AS g ON t.GForceID = g.GForceID
	INNER JOIN Altitude AS a ON t.AltitudeID = a.AltitudeID;