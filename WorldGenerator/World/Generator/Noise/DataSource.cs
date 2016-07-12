using System;
namespace Sean.WorldGenerator.Noise
{
    public abstract class DataSource
    {
        public DataSource ()
        {}

        public abstract float get (float x, float y);
        public abstract float get (float x, float y, float z);
        public abstract float get (float x, float y, float z, float w);
        public abstract float get (float x, float y, float z, float w, float u, float v);
    }

    public class StaticDataSource : DataSource
    {
        private float data;

        public override float get (float x, float y) { return data; }
        public override float get (float x, float y, float z) { return data; }
        public override float get (float x, float y, float z, float w) { return data; }
        public override float get (float x, float y, float z, float w, float u, float v) { return data; }
    }
}

