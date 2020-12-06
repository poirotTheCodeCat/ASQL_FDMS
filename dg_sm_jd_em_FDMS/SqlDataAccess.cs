using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.Common;
using System.Configuration;

namespace dg_sm_jd_em_FDMS
{
    public class SqlDataAccess
    {
        /*
         * Function: getRecords(string tailNum, string connectionString)
         * Description: This retrieves telemetry records with a specific tail number from the database
         */
        public static List<Telemetry> getRecords(string tailNum, string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    List<Telemetry> telList = new List<Telemetry>();

                    // create sql query to use
                    String SqlCommandString = $"SELECT * FROM TelemetryData WHERE TailNum = '{tailNum}'";
                    SqlCommand cmd = new SqlCommand(SqlCommandString, connection);

                    connection.Open();  // open the connection

                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        // add the telemetry from the database to the list of telemetry objects
                        telList.Add(
                            new Telemetry(
                                reader["TailNum"].ToString(), Convert.ToDouble(reader["Accel_x"]), Convert.ToDouble(reader["Accel_y"]), Convert.ToDouble(reader["Accel_z"]),
                                Convert.ToDouble(reader["Weight"]), Convert.ToDouble(reader["Altitude"]), Convert.ToDouble(reader["Pitch"]), Convert.ToDouble(reader["Bank"]),
                                Convert.ToDateTime(reader["TimeStamp"]))
                            );
                    }

                    // close the reader and connection
                    reader.Close();
                    connection.Close();

                    return telList;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }


        /*
         * Function: insertRecord(Telemetry telInsert, string connectionString)
         * Description: Inserts a telemetry record into the database
         */
        public static void insertRecord(Telemetry telInsert, string connectionString)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("sp_Insert", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@tailnum", telInsert.TailNum));
                    cmd.Parameters.Add(new SqlParameter("@Accel_x", telInsert.Accel_x));
                    cmd.Parameters.Add(new SqlParameter("@Accel_y", telInsert.Accel_y));
                    cmd.Parameters.Add(new SqlParameter("@Accel_z", telInsert.Accel_z));
                    cmd.Parameters.Add(new SqlParameter("@Weight", telInsert.Weight));
                    cmd.Parameters.Add(new SqlParameter("@Altitude", telInsert.Altitude));
                    cmd.Parameters.Add(new SqlParameter("@Pitch", telInsert.Pitch));
                    cmd.Parameters.Add(new SqlParameter("@Bank", telInsert.Bank));
                    cmd.Parameters.Add(new SqlParameter("@TimeStamp", telInsert.TimeStamp));

                    // execute sql stored procedure 
                    cmd.ExecuteScalar();
                }
                catch
                {
                    throw new Exception("Could not insert recieved Telemetry data");
                }
                finally
                {
                    connection.Close();
                }
            }

        }
    }
}
