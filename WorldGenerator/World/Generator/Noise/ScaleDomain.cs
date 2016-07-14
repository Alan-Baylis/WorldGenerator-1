using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    // ScaleDomain applies a domain scaling to the coordinates input to the function.

    public class CImplicitScaleDomain : CImplicitModuleBase
    {
        //CImplicitModuleBase * m_source;
        private CScalarParameter m_source;
        private CScalarParameter m_sx, m_sy, m_sz, m_sw, m_su, m_sv;

        public CImplicitScaleDomain() : base()
        {
            m_source = new CScalarParameter(0.0f); m_sx = new CScalarParameter(1.0f); m_sy = new CScalarParameter(1.0f);
            m_sz = new CScalarParameter(1.0f); m_sw = new CScalarParameter(1.0f); m_su = new CScalarParameter(1.0f); m_sv = new CScalarParameter(1.0f);
        }

        public CImplicitScaleDomain(double src, double x, double y, double z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(double src, double x, double y, CImplicitModuleBase z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(double src, double x, CImplicitModuleBase y, double z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(double src, double x, CImplicitModuleBase y, CImplicitModuleBase z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(double src, CImplicitModuleBase x, double y, double z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(double src, CImplicitModuleBase x, double y, CImplicitModuleBase z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(double src, CImplicitModuleBase x, CImplicitModuleBase y, double z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(double src, CImplicitModuleBase x, CImplicitModuleBase y, CImplicitModuleBase z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(CImplicitModuleBase src, double x, double y, double z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(CImplicitModuleBase src, double x, double y, CImplicitModuleBase z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(CImplicitModuleBase src, double x, CImplicitModuleBase y, double z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(CImplicitModuleBase src, double x, CImplicitModuleBase y, CImplicitModuleBase z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(CImplicitModuleBase src, CImplicitModuleBase x, double y, double z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(CImplicitModuleBase src, CImplicitModuleBase x, double y, CImplicitModuleBase z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(CImplicitModuleBase src, CImplicitModuleBase x, CImplicitModuleBase y, double z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }

        public CImplicitScaleDomain(CImplicitModuleBase src, CImplicitModuleBase x, CImplicitModuleBase y, CImplicitModuleBase z, double w, double u, double v) : base()
        { m_source = new CScalarParameter(src); m_sx = new CScalarParameter(x); m_sy = new CScalarParameter(y); m_sz = new CScalarParameter(z); m_sw = new CScalarParameter(1); m_su = new CScalarParameter(u); m_sv = new CScalarParameter(v); }



        public void setScale(double x, double y, double z, double w, double u, double v)
        {
            m_sx.set(x); m_sy.set(y); m_sz.set(z); m_sw.set(w); m_su.set(u); m_sv.set(v);
        }

        public void setXScale(double x) { m_sx.set(x); }
        public void setYScale(double x) { m_sy.set(x); }
        public void setZScale(double x) { m_sz.set(x); }
        public void setWScale(double x) { m_sw.set(x); }
        public void setUScale(double x) { m_su.set(x); }
        public void setVScale(double x) { m_sv.set(x); }
        public void setXScale(CImplicitModuleBase x) { m_sx.set(x); }
        public void setYScale(CImplicitModuleBase y) { m_sy.set(y); }
        public void setZScale(CImplicitModuleBase z) { m_sz.set(z); }
        public void setWScale(CImplicitModuleBase w) { m_sw.set(w); }
        public void setUScale(CImplicitModuleBase u) { m_su.set(u); }
        public void setVScale(CImplicitModuleBase v) { m_sv.set(v); }


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
            return m_source.get(x * m_sx.get(x, y), y * m_sy.get(x, y));
        }

        public override double get(double x, double y, double z)
        {
            return m_source.get(x * m_sx.get(x, y, z), y * m_sy.get(x, y, z), z * m_sz.get(x, y, z));
        }

        public override double get(double x, double y, double z, double w)
        {
            return m_source.get(x * m_sx.get(x, y, z, w), y * m_sy.get(x, y, z, w), z * m_sz.get(x, y, z, w), w * m_sw.get(x, y, z, w));
        }

        public override double get(double x, double y, double z, double w, double u, double v)
        {
            return m_source.get(x * m_sx.get(x, y, z, w, u, v), y * m_sy.get(x, y, z, w, u, v), z * m_sz.get(x, y, z, w, u, v),
                w * m_sw.get(x, y, z, w, u, v), u * m_su.get(x, y, z, w, u, v), v * m_sv.get(x, y, z, w, u, v));
        }
    }
}
