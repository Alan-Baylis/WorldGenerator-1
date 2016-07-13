using System;
namespace Sean.WorldGenerator.Noise
{
    public abstract class DataSource
    {
        public DataSource ()
        {}

        public abstract double get (double x, double y);
        public abstract double get (double x, double y, double z);
        public abstract double get (double x, double y, double z, double w);
        public abstract double get (double x, double y, double z, double w, double u, double v);
    }

    public class StaticDataSource : DataSource
    {
        private double data;

        public override double get (double x, double y) { return data; }
        public override double get (double x, double y, double z) { return data; }
        public override double get (double x, double y, double z, double w) { return data; }
        public override double get (double x, double y, double z, double w, double u, double v) { return data; }
    }
}

