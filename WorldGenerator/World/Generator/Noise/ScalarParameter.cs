namespace Sean.WorldGenerator.Noise
{


    // Many functions have "parameters" that affect their functionality, 
    // aside from any "source" function they might have. An example is the 
    // CImplicitSelect function. This function has a control source, a low source, 
    // a high source, a threshold parameter, and a falloff parameter. 
    // The threshold parameter is used to select between the output of lowSource and highSource, 
    // depending on if the value of controlSource is above or below threshold. 
    // All five of these inputs are instances of what ANL calls a "scalar parameter". 
    // A scalar parameter can be set to either a constant (double-precision) value, 
    // or to another function. Most will default to some sane double-precision value (ie, 0), 
    // but if desired they can be overridden with any constant or any implicit functional output. 
    // In this way, complex behaviors can be obtained through a relatively simple interface.
        
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
