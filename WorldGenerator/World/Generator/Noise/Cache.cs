using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    struct SCache
    {
        float x,y,z,w,u,v;
        float val;
        bool valid;

        SCache() : valid(false){}
    };
    class CImplicitCache : public CImplicitModuleBase
    {
         protected:
        //CImplicitModuleBase * m_source;
        CScalarParameter m_source;
        SCache m_c2, m_c3, m_c4, m_c6;
  
    CImplicitCache::CImplicitCache() : CImplicitModuleBase(), m_source(0.0) {}
    CImplicitCache::CImplicitCache(float v) : CImplicitModuleBase(), m_source(v) {}
    CImplicitCache::CImplicitCache(CImplicitModuleBase * v) : CImplicitModuleBase(), m_source(v) {}
    CImplicitCache::~CImplicitCache() {}
    void CImplicitCache::setSource(CImplicitModuleBase * m)
    {
        m_source.set(m);
    }
    void CImplicitCache::setSource(float v)
    {
        m_source.set(v);
    }
    float CImplicitCache::get(float x, float y)
    {
        if(!m_c2.valid || m_c2.x!=x || m_c2.y!=y)
        {
            m_c2.x=x;
            m_c2.y=y;
            m_c2.valid=true;
            m_c2.val=m_source.get(x,y);
        }
        return m_c2.val;
    }

    float CImplicitCache::get(float x, float y, float z)
    {
        if(!m_c3.valid || m_c3.x!=x || m_c3.y!=y || m_c3.z!=z)
        {
            m_c3.x=x;
            m_c3.y=y;
            m_c3.z=z;
            m_c3.valid=true;
            m_c3.val=m_source.get(x,y,z);
        }
        return m_c3.val;
    }

    float CImplicitCache::get(float x, float y, float z, float w)
    {
        if(!m_c4.valid || m_c4.x!=x || m_c4.y!=y || m_c4.z!=z || m_c4.w!=w)
        {
            m_c4.x=x;
            m_c4.y=y;
            m_c4.z=z;
            m_c4.w=w;
            m_c4.valid=true;
            m_c4.val=m_source.get(x,y,z,w);
        }
        return m_c4.val;
    }

    float CImplicitCache::get(float x, float y, float z, float w, float u, float v)
    {
        if(!m_c6.valid || m_c6.x!=x || m_c6.y!=y || m_c6.z!=z || m_c6.w!=w || m_c6.u!=u || m_c6.v!=v)
        {
            m_c6.x=x;
            m_c6.y=y;
            m_c6.z=z;
            m_c6.w=w;
            m_c6.u=u;
            m_c6.v=v;
            m_c6.valid=true;
            m_c6.val=m_source.get(x,y,z,w,u,v);
        }
        return m_c6.val;
    }
};
