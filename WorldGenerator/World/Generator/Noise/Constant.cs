#ifndef IMPLICITCONSTANT_H
#define IMPLICITCONSTANT_H
#include "implicitmodulebase.h"

namespace anl
{
    class CImplicitConstant : public CImplicitModuleBase
    {
        public:
        CImplicitConstant();
        CImplicitConstant(ANLFloatType c);
        ~CImplicitConstant();

        void setConstant(ANLFloatType c);

        ANLFloatType get(ANLFloatType x, ANLFloatType y);
        ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
        ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
        ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);

        protected:
        ANLFloatType m_constant;
    };
};

#endif
#include "implicitconstant.h"

namespace anl
{
    CImplicitConstant::CImplicitConstant() : CImplicitModuleBase(), m_constant(0){}
    CImplicitConstant::CImplicitConstant(ANLFloatType c) : CImplicitModuleBase(), m_constant(c){}
	CImplicitConstant::~CImplicitConstant(){}

	void CImplicitConstant::setConstant(ANLFloatType c){m_constant=c;}

	ANLFloatType CImplicitConstant::get(ANLFloatType x, ANLFloatType y){return m_constant;}
	ANLFloatType CImplicitConstant::get(ANLFloatType x, ANLFloatType y, ANLFloatType z){return m_constant;}
	ANLFloatType CImplicitConstant::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w){return m_constant;}
	ANLFloatType CImplicitConstant::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v){return m_constant;}
};
