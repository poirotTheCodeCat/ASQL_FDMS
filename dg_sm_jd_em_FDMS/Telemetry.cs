using System;
using System.Collections.Generic;
using System.Text;

namespace dg_sm_jd_em_FDMS
{
    class Telemetry
    {
        private String tailNum;
        private double accel_x;
        private double accel_y;
        private double accel_z;
        private double weight;
        private double altitude;
        private double pitch;
        private double bank;
        private DateTime timeStamp;

        public Telemetry()
        {

        }
        public Telemetry(String tail, double x, double y, double z, double w, double a, double p, double b, DateTime ts)
        {
            tailNum = tail;
            accel_x = x;
            accel_y = y;
            accel_z = z;
            weight = w;
            altitude = a;
            pitch = p;
            timeStamp = ts;
        }

        public string TailNum
        {
            get { return tailNum; }
            set { tailNum = value; }
        }

        public double Accel_x
        {
            get { return accel_x; }
            set { accel_x = value; }
        }
        public double Accel_y
        {
            get { return accel_y; }
            set { accel_y = value; }
        }
        public double Accel_z
        {
            get { return accel_z; }
            set { accel_z = value; }
        }
        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public double Altitude
        {
            get { return altitude; }
            set { altitude = value; }
        }
        public double Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }
        public double Bank
        {
            get { return bank; }
            set { bank = value; }
        }
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

    }
}
