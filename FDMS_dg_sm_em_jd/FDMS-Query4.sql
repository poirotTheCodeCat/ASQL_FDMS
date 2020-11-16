DECLARE @time datetime = current_timestamp;
EXEC sp_Insert @TailNum = 'First', @Accel_x = 1.0, @Accel_y = 1.0, @Accel_z = 1.0, @Weight = 1.0, @Altitude = 1.0,
@Pitch = 1.0, @Bank = 1.0, @TimeStamp = @time;

SET @time = current_timestamp;
EXEC sp_Insert @TailNum = 'Second', @Accel_x = 2.0, @Accel_y = 2.0, @Accel_z = 2.0, @Weight = 2.0, @Altitude = 2.0,
@Pitch = 2.0, @Bank = 2.0, @TimeStamp = @time;

SET @time = current_timestamp;
EXEC sp_Insert @TailNum = 'Third', @Accel_x = 3.0, @Accel_y = 3.0, @Accel_z = 3.0, @Weight = 3.0, @Altitude = 3.0,
@Pitch = 3.0, @Bank = 3.0, @TimeStamp = @time;

SELECT * FROM TelemetryData;