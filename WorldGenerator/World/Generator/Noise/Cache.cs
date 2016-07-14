using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    public struct SCache
    {
        public double x, y, z, w, u, v;
        public double val;
        public bool valid;
    };

    class CImplicitCache : CImplicitModuleBase
    {
        //CImplicitModuleBase * m_source;
        private CScalarParameter m_source;
        private SCache m_c2, m_c3, m_c4, m_c6;

        CImplicitCache() : base() { }
        CImplicitCache(double v) : base()
        { m_source = new CScalarParameter(v); }
        CImplicitCache(CImplicitModuleBase v) : base()
        { m_source = new CScalarParameter(v); }

        public void setSource(CImplicitModuleBase m)
        {
            m_source.set(m);
        }
        public void setSource(double v)
        {
            m_source.set(v);
        }
        public override double get(double x, double y)
        {
            if (!m_c2.valid || m_c2.x != x || m_c2.y != y)
            {
                m_c2.x = x;
                m_c2.y = y;
                m_c2.valid = true;
                m_c2.val = m_source.get(x, y);
            }
            return m_c2.val;
        }

        public override double get(double x, double y, double z)
        {
            if (!m_c3.valid || m_c3.x != x || m_c3.y != y || m_c3.z != z)
            {
                m_c3.x = x;
                m_c3.y = y;
                m_c3.z = z;
                m_c3.valid = true;
                m_c3.val = m_source.get(x, y, z);
            }
            return m_c3.val;
        }

        public override double get(double x, double y, double z, double w)
        {
            if (!m_c4.valid || m_c4.x != x || m_c4.y != y || m_c4.z != z || m_c4.w != w)
            {
                m_c4.x = x;
                m_c4.y = y;
                m_c4.z = z;
                m_c4.w = w;
                m_c4.valid = true;
                m_c4.val = m_source.get(x, y, z, w);
            }
            return m_c4.val;
        }

        public override double get(double x, double y, double z, double w, double u, double v)
        {
            if (!m_c6.valid || m_c6.x != x || m_c6.y != y || m_c6.z != z || m_c6.w != w || m_c6.u != u || m_c6.v != v)
            {
                m_c6.x = x;
                m_c6.y = y;
                m_c6.z = z;
                m_c6.w = w;
                m_c6.u = u;
                m_c6.v = v;
                m_c6.valid = true;
                m_c6.val = m_source.get(x, y, z, w, u, v);
            }
            return m_c6.val;
        }
    }
}