using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    // ScaleOffset applies a scaling and translation factor to the output of its source function, as v*scale+offset.

    public class CImplicitScaleOffset : CImplicitModuleBase
    {
        private CImplicitModuleBase m_source;
        private CScalarParameter m_scale { get; set; }
        private CScalarParameter m_offset { get; set; }

        public CImplicitScaleOffset(CImplicitModuleBase source, double scale, double offset) : base()
        {
            m_source = source;
            m_scale = new CScalarParameter(scale);
            m_offset = new CScalarParameter(offset);
        }

        public override double get (double x, double y)
        {
            return m_source.get (x, y) * m_scale.get (x, y) + m_offset.get (x, y);
        }

        public override double get (double x, double y, double z)
        {
            return m_source.get (x, y, z) * m_scale.get (x, y, z) + m_offset.get (x, y, z);
        }

        public override double get (double x, double y, double z, double w)
        {
            return m_source.get (x, y, z, w) * m_scale.get (x, y, z, w) + m_offset.get (x, y, z, w);
        }

        public override double get (double x, double y, double z, double w, double u, double v)
        {
            return m_source.get (x, y, z, w, u, v) * m_scale.get (x, y, z, w, u, v) + m_offset.get(x,y,z,w,u,v);
        }
    }
}
