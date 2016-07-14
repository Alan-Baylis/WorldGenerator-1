namespace Sean.WorldGenerator.Noise
{
    // Scalar parameter class
    public class CScalarParameter
    {
        public CScalarParameter(double v) { }

        public CScalarParameter(CImplicitModuleBase b)
        {
            m_val = 0;
            m_source = b;
        }
        public CScalarParameter(CScalarParameter p) { m_source = p.m_source; m_val = p.m_val; }


        public void set(double v)
        {
            m_source = null;
            m_val = v;
        }

        public void set(CImplicitModuleBase m)
        {
            m_source = m;
        }

        public double get(double x, double y)
        {
            if (m_source != null) return m_source.get(x, y);
            else return m_val;
        }

        public double get(double x, double y, double z)
        {
            if (m_source != null) return m_source.get(x, y, z);
            else return m_val;
        }

        public double get(double x, double y, double z, double w)
        {
            if (m_source != null) return m_source.get(x, y, z, w);
            else return m_val;
        }

        public double get(double x, double y, double z, double w, double u, double v)
        {
            if (m_source != null) return m_source.get(x, y, z, w, u, v);
            else return m_val;
        }

        private double m_val;
        private CImplicitModuleBase m_source;
    }
}
