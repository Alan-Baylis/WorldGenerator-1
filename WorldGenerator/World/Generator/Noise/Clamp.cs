#ifndef IMPLICIT_CLAMP_H
#define IMPLICIT_CLAMP_H
#include "implicitmodulebase.h"

namespace anl
{
    class CImplicitClamp : public CImplicitModuleBase
    {
        public:
        CImplicitClamp();
        CImplicitClamp(ANLFloatType source, ANLFloatType low, ANLFloatType high);
        CImplicitClamp(ANLFloatType source, ANLFloatType low, CImplicitModuleBase *  high);
        CImplicitClamp(ANLFloatType source, CImplicitModuleBase *  low, ANLFloatType high);
        CImplicitClamp(ANLFloatType source, CImplicitModuleBase *  low, CImplicitModuleBase *  high);
        CImplicitClamp(CImplicitModuleBase *  source, ANLFloatType low, ANLFloatType high);
        CImplicitClamp(CImplicitModuleBase *  source, ANLFloatType low, CImplicitModuleBase *  high);
        CImplicitClamp(CImplicitModuleBase *  source, CImplicitModuleBase *  low, ANLFloatType high);
        CImplicitClamp(CImplicitModuleBase *  source, CImplicitModuleBase *  low, CImplicitModuleBase *  high);

        ~CImplicitClamp();

        void setRange(ANLFloatType low, ANLFloatType high);
        void setRange(ANLFloatType low, CImplicitModuleBase * high);
        void setRange(CImplicitModuleBase * low, ANLFloatType high);
        void setRange(CImplicitModuleBase * low, CImplicitModuleBase * high);

        void setSource(ANLFloatType b);
        void setSource(CImplicitModuleBase * b);

        ANLFloatType get(ANLFloatType x, ANLFloatType y);
        ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
        ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
        ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);

        protected:
        //CImplicitModuleBase * m_source;
        //ANLFloatType m_low, m_high;
        CScalarParameter m_source, m_low, m_high;
    };
};
#endif
#include "implicitclamp.h"
#include "utility.h"

namespace anl
{
	CImplicitClamp::CImplicitClamp():CImplicitModuleBase(), m_source(0.0), m_low(0.0), m_high(1.0){};
    CImplicitClamp::CImplicitClamp(ANLFloatType source, ANLFloatType low, ANLFloatType high) : CImplicitModuleBase(), m_source(source), m_low(low), m_high(high){}
    CImplicitClamp::CImplicitClamp(ANLFloatType source, ANLFloatType low, CImplicitModuleBase *  high) : CImplicitModuleBase(), m_source(source), m_low(low), m_high(high){}
    CImplicitClamp::CImplicitClamp(ANLFloatType source, CImplicitModuleBase *  low, ANLFloatType high) : CImplicitModuleBase(), m_source(source), m_low(low), m_high(high){}
    CImplicitClamp::CImplicitClamp(ANLFloatType source, CImplicitModuleBase *  low, CImplicitModuleBase *  high) : CImplicitModuleBase(), m_source(source), m_low(low), m_high(high){}
    CImplicitClamp::CImplicitClamp(CImplicitModuleBase *  source, ANLFloatType low, ANLFloatType high) : CImplicitModuleBase(), m_source(source), m_low(low), m_high(high){}
    CImplicitClamp::CImplicitClamp(CImplicitModuleBase *  source, ANLFloatType low, CImplicitModuleBase *  high) : CImplicitModuleBase(), m_source(source), m_low(low), m_high(high){}
    CImplicitClamp::CImplicitClamp(CImplicitModuleBase *  source, CImplicitModuleBase *  low, ANLFloatType high) : CImplicitModuleBase(), m_source(source), m_low(low), m_high(high){}
    CImplicitClamp::CImplicitClamp(CImplicitModuleBase *  source, CImplicitModuleBase *  low, CImplicitModuleBase *  high) : CImplicitModuleBase(), m_source(source), m_low(low), m_high(high){}
    CImplicitClamp::~CImplicitClamp(){}

    void CImplicitClamp::setRange(ANLFloatType low, ANLFloatType high)
    {
        m_low.set(low);
        m_high.set(high);
    }

    void CImplicitClamp::setRange(ANLFloatType low, CImplicitModuleBase * high)
    {
        m_low.set(low);
        m_high.set(high);
    }

    void CImplicitClamp::setRange(CImplicitModuleBase * low, ANLFloatType high)
    {
        m_low.set(low);
        m_high.set(high);
    }

    void CImplicitClamp::setRange(CImplicitModuleBase * low, CImplicitModuleBase * high)
    {
        m_low.set(low);
        m_high.set(high);
    }


    void CImplicitClamp::setSource(ANLFloatType b)
    {
        m_source.set(b);
    }

    void CImplicitClamp::setSource(CImplicitModuleBase * b)
    {
        m_source.set(b);
    }

    ANLFloatType CImplicitClamp::get(ANLFloatType x, ANLFloatType y)
    {
        return clamp(m_source.get(x,y),m_low.get(x,y),m_high.get(x,y));
    }

    ANLFloatType CImplicitClamp::get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
    {
        return clamp(m_source.get(x,y,z),m_low.get(x,y,z),m_high.get(x,y,z));
    }

    ANLFloatType CImplicitClamp::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
    {
        return clamp(m_source.get(x,y,z,w),m_low.get(x,y,z,w),m_high.get(x,y,z,w));
    }

    ANLFloatType CImplicitClamp::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
    {
        return clamp(m_source.get(x,y,z,w,u,v),m_low.get(x,y,z,w,u,v),m_high.get(x,y,z,w,u,v));
    }
};
