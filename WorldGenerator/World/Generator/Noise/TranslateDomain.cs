using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    // TranslateDomain is used to translate the input coordinates of a function.
    // Each axis is specifiable as a constant or noise source.This application of 
    // domain transformation is commonly called turbulence and is useful in generating 
    // many types of effects.Here is a single BasisFunction of type GRADIENT, transformed 
    // in the X axis by a fractal:

    public class CImplicitTranslateDomain : CImplicitModuleBase
    {
        private CScalarParameter m_source, m_ax, m_ay, m_az, m_aw, m_au, m_av;

        public CImplicitTranslateDomain() : base() { }

        public CImplicitTranslateDomain(double src, double tx, double ty, double tz) : base()
        {   m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(double src, double tx, double ty, CImplicitModuleBase tz) : base()
        {   m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz);  }

        public CImplicitTranslateDomain(double src, double tx, CImplicitModuleBase ty, double tz) : base()
        {   m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz);  }

        public CImplicitTranslateDomain(double src, double tx, CImplicitModuleBase ty, CImplicitModuleBase tz) : base()
        {   m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz);  }

        public CImplicitTranslateDomain(double src, CImplicitModuleBase tx, double ty, double tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(double src, CImplicitModuleBase tx, double ty, CImplicitModuleBase tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(double src, CImplicitModuleBase tx, CImplicitModuleBase ty, double tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(double src, CImplicitModuleBase tx, CImplicitModuleBase ty, CImplicitModuleBase tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(CImplicitModuleBase src, double tx, double ty, double tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(CImplicitModuleBase src, double tx, double ty, CImplicitModuleBase tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(CImplicitModuleBase src, double tx, CImplicitModuleBase ty, double tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(CImplicitModuleBase src, double tx, CImplicitModuleBase ty, CImplicitModuleBase tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(CImplicitModuleBase src, CImplicitModuleBase tx, double ty, double tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(CImplicitModuleBase src, CImplicitModuleBase tx, double ty, CImplicitModuleBase tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(CImplicitModuleBase src, CImplicitModuleBase tx, CImplicitModuleBase ty, double tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }

        public CImplicitTranslateDomain(CImplicitModuleBase src, CImplicitModuleBase tx, CImplicitModuleBase ty, CImplicitModuleBase tz) : base()
        { m_source = new CScalarParameter(src); m_ax = new CScalarParameter(tx); m_ay = new CScalarParameter(ty); m_az = new CScalarParameter(tz); }


        void setXAxisSource(CImplicitModuleBase m)
        {
            m_ax.set(m);
        }
        void setYAxisSource(CImplicitModuleBase m)
        {
            m_ay.set(m);
        }
        void setZAxisSource(CImplicitModuleBase m)
        {
            m_az.set(m);
        }
        void setWAxisSource(CImplicitModuleBase m)
        {
            m_aw.set(m);
        }
        void setUAxisSource(CImplicitModuleBase m)
        {
            m_au.set(m);
        }
        void setVAxisSource(CImplicitModuleBase m)
        {
            m_av.set(m);
        }
        void setXAxisSource(double v)
        {
            m_ax.set(v);
        }
        void setYAxisSource(double v)
        {
            m_ay.set(v);
        }
        void setZAxisSource(double v)
        {
            m_az.set(v);
        }
        void setWAxisSource(double v)
        {
            m_aw.set(v);
        }
        void setUAxisSource(double v)
        {
            m_au.set(v);
        }
        void setVAxisSource(double v)
        {
            m_av.set(v);
        }
        void setSource(CImplicitModuleBase m)
        {
            m_source.set(m);
        }
        void setSource(double v)
        {
            m_source.set(v);
        }

        public override double get(double x, double y)
        {
            return m_source.get(x + m_ax.get(x, y), y + m_ay.get(x, y));
        }
        public override double get(double x, double y, double z)
        {
            return m_source.get(x + m_ax.get(x, y, z), y + m_ay.get(x, y, z), z + m_az.get(x, y, z));
        }
        public override double get(double x, double y, double z, double w)
        {
            return m_source.get(x + m_ax.get(x, y, z, w), y + m_ay.get(x, y, z, w), z + m_az.get(x, y, z, w), w + m_aw.get(x, y, z, w));
        }
        public override double get(double x, double y, double z, double w, double u, double v)
        {
            return m_source.get(x + m_ax.get(x, y, z, w, u, v), y + m_ay.get(x, y, z, w, u, v), z + m_az.get(x, y, z, w, u, v),
                w + m_aw.get(x, y, z, w, u, v), u + m_au.get(x, y, z, w, u, v), v + m_av.get(x, y, z, w, u, v));
        }
    }
}
