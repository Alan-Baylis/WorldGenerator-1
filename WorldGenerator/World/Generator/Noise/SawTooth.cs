#ifndef IMPLICIT_SAWTOOTH_H
#define IMPLICIT_SAWTOOTH_H
#include "implicitmodulebase.h"

namespace anl
{
class CImplicitSawtooth : public CImplicitModuleBase
{
    public:
    CImplicitSawtooth();
    CImplicitSawtooth(ANLFloatType source, ANLFloatType period);
    CImplicitSawtooth(ANLFloatType source, CImplicitModuleBase * period);
    CImplicitSawtooth(CImplicitModuleBase * source, ANLFloatType period);
    CImplicitSawtooth(CImplicitModuleBase * source, CImplicitModuleBase * period);
    ~CImplicitSawtooth();

    void setSource(CImplicitModuleBase * s);
    void setSource(ANLFloatType s);
    void setPeriod(CImplicitModuleBase * p);
    void setPeriod(ANLFloatType p);

    ANLFloatType get(ANLFloatType x, ANLFloatType y);
    ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
    ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
    ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);

    protected:
    CScalarParameter m_source;
    CScalarParameter m_period;

};
};
#endif
#include "implicitsawtooth.h"
#include <cmath>
namespace anl
{
    CImplicitSawtooth::CImplicitSawtooth() : CImplicitModuleBase(), m_source(0.0), m_period(0.0){}
    CImplicitSawtooth::CImplicitSawtooth(ANLFloatType source, ANLFloatType period) : CImplicitModuleBase(), m_source(source), m_period(period){}
    CImplicitSawtooth::CImplicitSawtooth(ANLFloatType source, CImplicitModuleBase * period) : CImplicitModuleBase(), m_source(source), m_period(period){}
    CImplicitSawtooth::CImplicitSawtooth(CImplicitModuleBase * source, ANLFloatType period) : CImplicitModuleBase(), m_source(source), m_period(period){}
    CImplicitSawtooth::CImplicitSawtooth(CImplicitModuleBase * source, CImplicitModuleBase * period) : CImplicitModuleBase(), m_source(source), m_period(period){}
CImplicitSawtooth::~CImplicitSawtooth()
{
}

void CImplicitSawtooth::setSource(CImplicitModuleBase * s)
{
    m_source.set(s);
}

void CImplicitSawtooth::setSource(ANLFloatType s)
{
    m_source.set(s);
}

void CImplicitSawtooth::setPeriod(CImplicitModuleBase * p)
{
    m_period.set(p);
}

void CImplicitSawtooth::setPeriod(ANLFloatType p)
{
    m_period.set(p);
}

ANLFloatType CImplicitSawtooth::get(ANLFloatType x, ANLFloatType y)
{
    ANLFloatType val=m_source.get(x,y);
    ANLFloatType p=m_period.get(x,y);

    return 2.0*(val/p - floor(0.5 + val/p));
}

ANLFloatType CImplicitSawtooth::get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
{
    ANLFloatType val=m_source.get(x,y,z);
    ANLFloatType p=m_period.get(x,y,z);

    return 2.0*(val/p - floor(0.5 + val/p));
}

ANLFloatType CImplicitSawtooth::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
{
    ANLFloatType val=m_source.get(x,y,z,w);
    ANLFloatType p=m_period.get(x,y,z,w);

    return 2.0*(val/p - floor(0.5 + val/p));
}

ANLFloatType CImplicitSawtooth::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
{
    ANLFloatType val=m_source.get(x,y,z,w,u,v);
    ANLFloatType p=m_period.get(x,y,z,w,u,v);

    return 2.0*(val/p - floor(0.5 + val/p));
}
};
