using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    /*************************************************
    CImplicitAutoCorrect

        Define a function that will attempt to correct the range of its input to fall within a desired output
    range.
        Operates by sampling the input function a number of times across an section of the domain, and using the
    calculated min/max values to generate scale/offset pair to apply to the input. The calculate() function performs
    this sampling and calculation, and is called automatically whenever a source is set, or whenever the desired output
    ranges are set. Also, the function may be called manually, if desired.
    ***************************************************/



    // The AutoCorrect module is a tool to help tame the wild beasts that are fractals.When a function 
    // is set as a source to AutoCorrect, the calculate() method is called.This method will sample the 
    // input function a number of times across a region of the domain, and attempt to calculate a set of 
    // "correction parameters" to remap the function's output to a different range. Multi-fractals especially 
    // are notorious for outputting values in odd ranges, and this function provides a drop-in method for correcting 
    // them. Due to the necessity of sampling the function a number of times, there is some processing overhead when 
    // calculate() is called.


    class CImplicitAutoCorrect : public CImplicitModuleBase
    {
        protected:
        CImplicitModuleBase * m_source;
        ANLFloatType m_low, m_high;
        ANLFloatType m_scale2, m_offset2;
        ANLFloatType m_scale3, m_offset3;
        ANLFloatType m_scale4, m_offset4;
        ANLFloatType m_scale6, m_offset6;



    CImplicitAutoCorrect::CImplicitAutoCorrect() : CImplicitModuleBase(), m_source(0), m_low(-1.0), m_high(1.0){}
    CImplicitAutoCorrect::CImplicitAutoCorrect(ANLFloatType low, ANLFloatType high) : CImplicitModuleBase(), m_source(0), m_low(low), m_high(high){calculate();}
    CImplicitAutoCorrect::CImplicitAutoCorrect(CImplicitModuleBase * m, ANLFloatType low, ANLFloatType high) : CImplicitModuleBase(), m_source(m), m_low(low), m_high(high){calculate();}

    void CImplicitAutoCorrect::setSource(CImplicitModuleBase * m)
    {
        m_source=m;
        calculate();
    }

    void CImplicitAutoCorrect::setRange(ANLFloatType low, ANLFloatType high)
    {
        m_low=low; m_high=high;
        calculate();
    }

    void CImplicitAutoCorrect::calculate()
    {
        if(!m_source) return;
        ANLFloatType mn,mx;
        LCG lcg;
        //lcg.setSeedTime();

        // Calculate 2D
        mn=10000.0;
        mx=-10000.0;
        for(int c=0; c<10000; ++c)
        {
            ANLFloatType nx=lcg.get01()*4.0-2.0;
            ANLFloatType ny=lcg.get01()*4.0-2.0;

            ANLFloatType v=m_source->get(nx,ny);
            if(v<mn) mn=v;
            if(v>mx) mx=v;
        }
        m_scale2=(m_high-m_low) / (mx-mn);
        m_offset2=m_low-mn*m_scale2;

        // Calculate 3D
        mn=10000.0;
        mx=-10000.0;
        for(int c=0; c<10000; ++c)
        {
            ANLFloatType nx=lcg.get01()*4.0-2.0;
            ANLFloatType ny=lcg.get01()*4.0-2.0;
            ANLFloatType nz=lcg.get01()*4.0-2.0;

            ANLFloatType v=m_source->get(nx,ny,nz);
            if(v<mn) mn=v;
            if(v>mx) mx=v;
        }
        m_scale3=(m_high-m_low) / (mx-mn);
        m_offset3=m_low-mn*m_scale3;

        // Calculate 4D
        mn=10000.0;
        mx=-10000.0;
        for(int c=0; c<10000; ++c)
        {
            ANLFloatType nx=lcg.get01()*4.0-2.0;
            ANLFloatType ny=lcg.get01()*4.0-2.0;
            ANLFloatType nz=lcg.get01()*4.0-2.0;
            ANLFloatType nw=lcg.get01()*4.0-2.0;

            ANLFloatType v=m_source->get(nx,ny,nz,nw);
            if(v<mn) mn=v;
            if(v>mx) mx=v;
        }
        m_scale4=(m_high-m_low) / (mx-mn);
        m_offset4=m_low-mn*m_scale4;

        // Calculate 6D
        mn=10000.0;
        mx=-10000.0;
        for(int c=0; c<10000; ++c)
        {
            ANLFloatType nx=lcg.get01()*4.0-2.0;
            ANLFloatType ny=lcg.get01()*4.0-2.0;
            ANLFloatType nz=lcg.get01()*4.0-2.0;
            ANLFloatType nw=lcg.get01()*4.0-2.0;
            ANLFloatType nu=lcg.get01()*4.0-2.0;
            ANLFloatType nv=lcg.get01()*4.0-2.0;

            ANLFloatType v=m_source->get(nx,ny,nz,nw,nu,nv);
            if(v<mn) mn=v;
            if(v>mx) mx=v;
        }
        m_scale6=(m_high-m_low) / (mx-mn);
        m_offset6=m_low-mn*m_scale6;
    }


    ANLFloatType CImplicitAutoCorrect::get(ANLFloatType x, ANLFloatType y)
    {
        if(!m_source) return 0.0;

        ANLFloatType v=m_source->get(x,y);
        return std::max(m_low, std::min(m_high, v*m_scale2+m_offset2));
    }

    ANLFloatType CImplicitAutoCorrect::get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
    {
        if(!m_source) return 0.0;

        ANLFloatType v=m_source->get(x,y,z);
        return std::max(m_low, std::min(m_high, v*m_scale3+m_offset3));
    }
    ANLFloatType CImplicitAutoCorrect::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
    {
        if(!m_source) return 0.0;

        ANLFloatType v=m_source->get(x,y,z,w);
        return std::max(m_low, std::min(m_high, v*m_scale4+m_offset4));
    }

    ANLFloatType CImplicitAutoCorrect::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
    {
        if(!m_source) return 0.0;

        ANLFloatType val=m_source->get(x,y,z,w,u,v);
        return std::max(m_low, std::min(m_high, val*m_scale6+m_offset6));
    }
};
