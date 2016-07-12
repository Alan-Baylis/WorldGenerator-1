#ifndef IMPLICIT_COMBINER_H
#define IMPLICIT_COMBINER_H
#include "implicitmodulebase.h"

namespace anl
{
    enum ECombinerTypes
    {
        ADD,
        MULT,
        MAX,
        MIN,
        AVG
    };

    class CImplicitCombiner : public CImplicitModuleBase
    {
        public:
        CImplicitCombiner(unsigned int type);
        void setType(unsigned int type);
        void clearAllSources();
        void setSource(int which, CImplicitModuleBase * b);
        ANLFloatType get(ANLFloatType x, ANLFloatType y);
        ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
        ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
        ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);

        protected:
        CImplicitModuleBase * m_sources[MaxSources];
        unsigned int m_type;

        ANLFloatType Add_get(ANLFloatType x, ANLFloatType y);
        ANLFloatType Add_get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
        ANLFloatType Add_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
        ANLFloatType Add_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);
        ANLFloatType Mult_get(ANLFloatType x, ANLFloatType y);
        ANLFloatType Mult_get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
        ANLFloatType Mult_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
        ANLFloatType Mult_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);
        ANLFloatType Min_get(ANLFloatType x, ANLFloatType y);
        ANLFloatType Min_get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
        ANLFloatType Min_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
        ANLFloatType Min_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);
        ANLFloatType Max_get(ANLFloatType x, ANLFloatType y);
        ANLFloatType Max_get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
        ANLFloatType Max_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
        ANLFloatType Max_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);
        ANLFloatType Avg_get(ANLFloatType x, ANLFloatType y);
        ANLFloatType Avg_get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
        ANLFloatType Avg_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
        ANLFloatType Avg_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);
    };
};

#endif
#include "implicitcombiner.h"

namespace anl
{
    CImplicitCombiner::CImplicitCombiner(unsigned int type)
	{
		m_type=type;
		clearAllSources();
	}

	void CImplicitCombiner::setType(unsigned int type)
	{
		m_type=type;
	}

	void CImplicitCombiner::clearAllSources()
	{
		for(int c=0; c<MaxSources; ++c) m_sources[c]=0;
	}

	void CImplicitCombiner::setSource(int which, CImplicitModuleBase * b)
	{
		if(which<0 || which>=MaxSources) return;
		m_sources[which]=b;
	}

	ANLFloatType CImplicitCombiner::get(ANLFloatType x, ANLFloatType y)
	{
		switch(m_type)
		{
		case ADD: return Add_get(x,y); break;
		case MULT: return Mult_get(x,y); break;
		case MAX: return Max_get(x,y); break;
		case MIN: return Min_get(x,y); break;
		case AVG: return Avg_get(x,y); break;
		default: return 0.0; break;
		}
	}

	ANLFloatType CImplicitCombiner::get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
	{
	    switch(m_type)
		{
		case ADD: return Add_get(x,y,z); break;
		case MULT: return Mult_get(x,y,z); break;
		case MAX: return Max_get(x,y,z); break;
		case MIN: return Min_get(x,y,z); break;
		case AVG: return Avg_get(x,y,z); break;
		default: return 0.0; break;
		}
	}

	ANLFloatType CImplicitCombiner::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
	{
		switch(m_type)
		{
		case ADD: return Add_get(x,y,z,w); break;
		case MULT: return Mult_get(x,y,z,w); break;
		case MAX: return Max_get(x,y,z,w); break;
		case MIN: return Min_get(x,y,z,w); break;
		case AVG: return Avg_get(x,y,z,w); break;
		default: return 0.0; break;
		}
	}

	ANLFloatType CImplicitCombiner::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
	{
		switch(m_type)
		{
		case ADD: return Add_get(x,y,z,w,u,v); break;
		case MULT: return Mult_get(x,y,z,w,u,v); break;
		case MAX: return Max_get(x,y,z,w,u,v); break;
		case MIN: return Min_get(x,y,z,w,u,v); break;
		case AVG: return Avg_get(x,y,z,w,u,v); break;
		default: return 0.0; break;
		}
	}


	ANLFloatType CImplicitCombiner::Add_get(ANLFloatType x, ANLFloatType y)
	{
		ANLFloatType value=0.0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c]) value+=m_sources[c]->get(x,y);
		}
		return value;
	}
	ANLFloatType CImplicitCombiner::Add_get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
	{
	    ANLFloatType value=0;
	    for(int c=0; c<MaxSources; ++c)
	    {
	        if(m_sources[c]) value+=m_sources[c]->get(x,y,z);
	    }
	    return value;
	}
	ANLFloatType CImplicitCombiner::Add_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
	{
		ANLFloatType value=0.0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c]) value+=m_sources[c]->get(x,y,z,w);
		}
		return value;
	}
	ANLFloatType CImplicitCombiner::Add_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
	{
		ANLFloatType value=0.0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c]) value+=m_sources[c]->get(x,y,z,w,u,v);
		}
		return value;
	}

	ANLFloatType CImplicitCombiner::Mult_get(ANLFloatType x, ANLFloatType y)
	{
		ANLFloatType value=1.0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c]) value*=m_sources[c]->get(x,y);
		}
		return value;
	}
	ANLFloatType CImplicitCombiner::Mult_get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
	{
		ANLFloatType value=1.0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c]) value*=m_sources[c]->get(x,y,z);
		}
		return value;
	}
	ANLFloatType CImplicitCombiner::Mult_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
	{
		ANLFloatType value=1.0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c]) value*=m_sources[c]->get(x,y,z,w);
		}
		return value;
	}
	ANLFloatType CImplicitCombiner::Mult_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
	{
		ANLFloatType value=1.0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c]) value*=m_sources[c]->get(x,y,z,w,u,v);
		}
		return value;
	}

	ANLFloatType CImplicitCombiner::Min_get(ANLFloatType x, ANLFloatType y)
	{
		ANLFloatType mn;
		int c=0;
		while(c<MaxSources && !m_sources[c]) ++c;
		if(c==MaxSources) return 0.0;
		mn=m_sources[c]->get(x,y);

		for(int d=c; d<MaxSources; ++d)
		{
			if(m_sources[d])
			{
				ANLFloatType v=m_sources[d]->get(x,y);
				if(v<mn) mn=v;
			}
		}

		return mn;
	}

	ANLFloatType CImplicitCombiner::Min_get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
	{
		ANLFloatType mn;
		int c=0;
		while(c<MaxSources && !m_sources[c]) ++c;
		if(c==MaxSources) return 0.0;
		mn=m_sources[c]->get(x,y,z);

		for(int d=c; d<MaxSources; ++d)
		{
			if(m_sources[d])
			{
				ANLFloatType v=m_sources[d]->get(x,y,z);
				if(v<mn) mn=v;
			}
		}

		return mn;
	}

	ANLFloatType CImplicitCombiner::Min_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
	{
		ANLFloatType mn;
		int c=0;
		while(c<MaxSources && !m_sources[c]) ++c;
		if(c==MaxSources) return 0.0;
		mn=m_sources[c]->get(x,y,z,w);

		for(int d=c; d<MaxSources; ++d)
		{
			if(m_sources[d])
			{
				ANLFloatType v=m_sources[d]->get(x,y,z,w);
				if(v<mn) mn=v;
			}
		}

		return mn;
	}

	ANLFloatType CImplicitCombiner::Min_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
	{
		ANLFloatType mn;
		int c=0;
		while(c<MaxSources && !m_sources[c]) ++c;
		if(c==MaxSources) return 0.0;
		mn=m_sources[c]->get(x,y,z,w,u,v);

		for(int d=c; d<MaxSources; ++d)
		{
			if(m_sources[d])
			{
				ANLFloatType val=m_sources[d]->get(x,y,z,w,u,v);
				if(val<mn) mn=val;
			}
		}

		return mn;
	}

	ANLFloatType CImplicitCombiner::Max_get(ANLFloatType x, ANLFloatType y)
	{
		ANLFloatType mn;
		int c=0;
		while(c<MaxSources && !m_sources[c]) ++c;
		if(c==MaxSources) return 0.0;
		mn=m_sources[c]->get(x,y);

		for(int d=c; d<MaxSources; ++d)
		{
			if(m_sources[d])
			{
				ANLFloatType v=m_sources[d]->get(x,y);
				if(v>mn) mn=v;
			}
		}

		return mn;
	}

	ANLFloatType CImplicitCombiner::Max_get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
	{
		ANLFloatType mn;
		int c=0;
		while(c<MaxSources && !m_sources[c]) ++c;
		if(c==MaxSources) return 0.0;
		mn=m_sources[c]->get(x,y,z);

		for(int d=c; d<MaxSources; ++d)
		{
			if(m_sources[d])
			{
				ANLFloatType v=m_sources[d]->get(x,y,z);
				if(v>mn) mn=v;
			}
		}

		return mn;
	}

	ANLFloatType CImplicitCombiner::Max_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
	{
		ANLFloatType mn;
		int c=0;
		while(c<MaxSources && !m_sources[c]) ++c;
		if(c==MaxSources) return 0.0;
		mn=m_sources[c]->get(x,y,z,w);

		for(int d=c; d<MaxSources; ++d)
		{
			if(m_sources[d])
			{
				ANLFloatType v=m_sources[d]->get(x,y,z,w);
				if(v>mn) mn=v;
			}
		}

		return mn;
	}

	ANLFloatType CImplicitCombiner::Max_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
	{
		ANLFloatType mn;
		int c=0;
		while(c<MaxSources && !m_sources[c]) ++c;
		if(c==MaxSources) return 0.0;
		mn=m_sources[c]->get(x,y,z,w,u,v);

		for(int d=c; d<MaxSources; ++d)
		{
			if(m_sources[d])
			{
				ANLFloatType val=m_sources[d]->get(x,y,z,w,u,v);
				if(val>mn) mn=val;
			}
		}

		return mn;
	}

	ANLFloatType CImplicitCombiner::Avg_get(ANLFloatType x, ANLFloatType y)
	{
		ANLFloatType count=0;
		ANLFloatType value=0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c])
			{
				value+=m_sources[c]->get(x,y);
				count+=1.0;
			}
		}
		if(count==0.0) return 0.0;
		return value/count;
	}

	ANLFloatType CImplicitCombiner::Avg_get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
	{
		ANLFloatType count=0;
		ANLFloatType value=0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c])
			{
				value+=m_sources[c]->get(x,y,z);
				count+=1.0;
			}
		}
		if(count==0.0) return 0.0;
		return value/count;
	}

	ANLFloatType CImplicitCombiner::Avg_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
	{
		ANLFloatType count=0;
		ANLFloatType value=0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c])
			{
				value+=m_sources[c]->get(x,y,z,w);
				count+=1.0;
			}
		}
		if(count==0.0) return 0.0;
		return value/count;
	}

	ANLFloatType CImplicitCombiner::Avg_get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
	{
		ANLFloatType count=0;
		ANLFloatType value=0;
		for(int c=0; c<MaxSources; ++c)
		{
			if(m_sources[c])
			{
				value+=m_sources[c]->get(x,y,z,w,u,v);
				count+=1.0;
			}
		}
		if(count==0.0) return 0.0;
		return value/count;
	}
};
