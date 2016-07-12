/*
Copyright (c) 2008 Joshua Tippetts

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

namespace Sean.WorldGenerator.Noise
{
    abstract public class CBasePRNG
    {
        abstract public uint get ();
        abstract public void setSeed (uint s);

        public void setSeedTime()
        {
            setSeed((uint)(System.DateTime.Now.Ticks));
        }

        public uint getTarget(uint t)
        {
            float v=get01();
            return (uint)(v*(float)t);
        }

        public uint getRange(uint low, uint high)
        {
            if (high < low) {
                uint temp = low;
                low = high;
                high = temp;
            }
            float range = (float)((high - low)+1);
            float val = (float)low + get01()*range;
            return (uint)(val);
        }

        public float get01()
        {
            return ((float)get() / (float)(uint.MaxValue));
        }
    }

    class LCG : CBasePRNG
    {
        public LCG()
        {
            setSeed(10000);
        }

        public override void setSeed(uint s)
        {
            m_state=s;
        }

        public override uint get()
        {
            return (m_state=69069*m_state+362437);
        }

        protected uint m_state;
    }

    // The following generators are based on generators created by George Marsaglia
    // They use the an LCG object for seeding, to initialize various
    // state and tables. Seeding them is a bit more involved than an LCG.
    public class Xorshift : CBasePRNG
    {
        public Xorshift()
        {
            setSeed(10000);
        }

        public override void setSeed(uint s)
        {
            LCG lcg = new LCG ();
            lcg.setSeed(s);
            m_x=lcg.get();
            m_y=lcg.get();
            m_z=lcg.get();
            m_w=lcg.get();
            m_v=lcg.get();
        }

        public override uint get()
        {
            uint t;
            t=(m_x^(m_x>>7)); m_x=m_y; m_y=m_z; m_z=m_w; m_w=m_v;
            m_v=(m_v^(m_v<<6))^(t^(t<<13));
            return (m_y+m_y+1)*m_v;
        }

        protected uint m_x, m_y, m_z, m_w, m_v;
    };

    public class MWC256 : CBasePRNG
    {
        public MWC256()
        {
            setSeed(10000);
        }

        public override void setSeed(uint s)
        {
            LCG lcg = new LCG ();
            lcg.setSeed(s);
            for(int i=0; i<256; ++i)
            {
                m_Q[i]=lcg.get();
            }
            c=lcg.getTarget(809430660);
        }

        public override uint get()
        {
            ulong t,a=809430660;
            byte i=255;
            t=a*m_Q[++i]+c; c=(uint)(t>>32);
            return(m_Q[i]=(uint)t);
        }

        protected uint[] m_Q = new uint[256];
        protected uint c;
    }

    public class CMWC4096 : CBasePRNG
    {
        public CMWC4096()
        {
            setSeed(10000);
        }

        public override void setSeed(uint s)
        {
            LCG lcg = new LCG ();
            lcg.setSeed(s);   // Seed the global random source

            // Seed the table
            for(int i=0; i<4096; ++i)
            {
                m_Q[i]=lcg.get();
            }

            c=lcg.getTarget(18781);
        }

        public override uint get()
        {
            ulong t, a=18782, b=4294967295;
            uint i=2095;
            uint r=(uint)(b-1);

            i=(i+1)&4095;
            t=a*m_Q[i]+c;
            c=(uint)(t>>32); t=(t&b)+c;
            if(t>r) { c++; t=t-b;}
            return (m_Q[i]=(uint)(r-t));
        }

        protected uint[] m_Q = new uint[4096];
        protected uint c;
    }

    public class KISS : CBasePRNG
    {
        public KISS()
        {
            setSeed(10000);
        }

        public override void setSeed(uint s)
        {
            LCG lcg = new LCG ();
            lcg.setSeed(s);
            z=lcg.get();
            w=lcg.get();
            jsr=lcg.get();
            jcong=lcg.get();
        }

        public override uint get()
        {
            z=36969*(z&65535)+(z>>16);
            w=18000*(w&65535)+(w>>16);
            uint mwc = (z<<16)+w;

            jcong=69069*jcong+1234567;

            jsr^=(jsr<<17); jsr^=(jsr>>13); jsr^=(jsr<<5);

            return ((mwc^jcong) + jsr);
        }

        public uint z,w,jsr,jcong;
    }
}

