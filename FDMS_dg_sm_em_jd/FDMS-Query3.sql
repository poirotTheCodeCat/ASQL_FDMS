CREATE PROCEDURE sp_Insert
	@TailNum nvarchar(10),
	@Accel_x float,
	@Accel_y float,
	@Accel_z float,
	@Weight float,
	@Altitude float,
	@Pitch float,
	@Bank float,
	@TimeStamp datetime
AS
BEGIN 
	DECLARE 
		@GForceID int,
		@AltitudeID int,
		@Count int

	SET @Count = (SELECT COUNT(TailNum) FROM AircraftInfo WHERE TailNum = @TailNum)
	IF @Count = 0
	BEGIN
		INSERT INTO AircraftInfo (TailNum) VALUES (@TailNum)
	END
	INSERT INTO GForce (Accel_x, Accel_y, Accel_z, Weight) VALUES (@Accel_x, @Accel_y, @Accel_z, @Weight)
	INSERT INTO Altitude (Altitude, Pitch, Bank) VALUES (@Altitude, @Pitch, @Bank)

	SET @GForceID = (SELECT MAX(GForceID) FROM GForce)
	SET @AltitudeID = (SELECT MAX(AltitudeID) FROM Altitude)

	INSERT INTO Telemetry(TailNum, TimeStamp, GForceID, AltitudeID) VALUES 
	(@TailNum, @TimeStamp, @GForceID, @AltitudeID)
END

