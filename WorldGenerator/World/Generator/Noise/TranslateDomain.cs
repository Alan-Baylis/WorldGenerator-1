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


    class CImplicitTranslateDomain : public CImplicitModuleBase
    {
        protected:
        CScalarParameter m_source, m_ax, m_ay, m_az, m_aw, m_au, m_av;

    CImplicitTranslateDomain::CImplicitTranslateDomain() : CImplicitModuleBase(), m_source(0.0),
        m_ax(0.0), m_ay(0.0), m_az(0.0), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(float src, float tx, float ty, float tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(float src, float tx, float ty, CImplicitModuleBase * tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(float src, float tx, CImplicitModuleBase * ty, float tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(float src, float tx, CImplicitModuleBase * ty, CImplicitModuleBase * tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(float src, CImplicitModuleBase * tx, float ty, float tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(float src, CImplicitModuleBase * tx, float ty, CImplicitModuleBase * tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(float src, CImplicitModuleBase * tx, CImplicitModuleBase * ty, float tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(float src, CImplicitModuleBase * tx, CImplicitModuleBase * ty, CImplicitModuleBase * tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(CImplicitModuleBase * src, float tx, float ty, float tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(CImplicitModuleBase * src, float tx, float ty, CImplicitModuleBase * tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(CImplicitModuleBase * src, float tx, CImplicitModuleBase * ty, float tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(CImplicitModuleBase * src, float tx, CImplicitModuleBase * ty, CImplicitModuleBase * tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(CImplicitModuleBase * src, CImplicitModuleBase * tx, float ty, float tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(CImplicitModuleBase * src, CImplicitModuleBase * tx, float ty, CImplicitModuleBase * tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(CImplicitModuleBase * src, CImplicitModuleBase * tx, CImplicitModuleBase * ty, float tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}

    CImplicitTranslateDomain::CImplicitTranslateDomain(CImplicitModuleBase * src, CImplicitModuleBase * tx, CImplicitModuleBase * ty, CImplicitModuleBase * tz) :
        CImplicitModuleBase(), m_source(src), m_ax(tx), m_ay(ty), m_az(tz), m_aw(0.0), m_au(0.0), m_av(0.0){}



    CImplicitTranslateDomain::~CImplicitTranslateDomain(){}

    void CImplicitTranslateDomain::setXAxisSource(CImplicitModuleBase * m)
    {
        m_ax.set(m);
    }
    void CImplicitTranslateDomain::setYAxisSource(CImplicitModuleBase * m)
    {
        m_ay.set(m);
    }
    void CImplicitTranslateDomain::setZAxisSource(CImplicitModuleBase * m)
    {
        m_az.set(m);
    }
    void CImplicitTranslateDomain::setWAxisSource(CImplicitModuleBase * m)
    {
        m_aw.set(m);
    }
    void CImplicitTranslateDomain::setUAxisSource(CImplicitModuleBase * m)
    {
        m_au.set(m);
    }
    void CImplicitTranslateDomain::setVAxisSource(CImplicitModuleBase * m)
    {
        m_av.set(m);
    }
    void CImplicitTranslateDomain::setXAxisSource(float v)
    {
        m_ax.set(v);
    }
    void CImplicitTranslateDomain::setYAxisSource(float v)
    {
        m_ay.set(v);
    }
    void CImplicitTranslateDomain::setZAxisSource(float v)
    {
        m_az.set(v);
    }
    void CImplicitTranslateDomain::setWAxisSource(float v)
    {
        m_aw.set(v);
    }
    void CImplicitTranslateDomain::setUAxisSource(float v)
    {
        m_au.set(v);
    }
    void CImplicitTranslateDomain::setVAxisSource(float v)
    {
        m_av.set(v);
    }
    void CImplicitTranslateDomain::setSource(CImplicitModuleBase * m)
    {
        m_source.set(m);
    }
    void CImplicitTranslateDomain::setSource(float v)
    {
        m_source.set(v);
    }

    float CImplicitTranslateDomain::get(float x, float y)
    {
        return m_source.get(x+m_ax.get(x,y), y+m_ay.get(x,y));
    }
    float CImplicitTranslateDomain::get(float x, float y, float z)
    {
        return m_source.get(x+m_ax.get(x,y,z), y+m_ay.get(x,y,z), z+m_az.get(x,y,z));
    }
    float CImplicitTranslateDomain::get(float x, float y, float z, float w)
    {
        return m_source.get(x+m_ax.get(x,y,z,w), y+m_ay.get(x,y,z,w), z+m_az.get(x,y,z,w), w+m_aw.get(x,y,z,w));
    }
    float CImplicitTranslateDomain::get(float x, float y, float z, float w, float u, float v)
    {
        return m_source.get(x+m_ax.get(x,y,z,w,u,v), y+m_ay.get(x,y,z,w,u,v), z+m_az.get(x,y,z,w,u,v),
            w+m_aw.get(x,y,z,w,u,v), u+m_au.get(x,y,z,w,u,v), v+m_av.get(x,y,z,w,u,v));
    }
};
