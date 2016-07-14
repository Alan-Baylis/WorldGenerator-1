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


    class CImplicitAutoCorrect : CImplicitModuleBase
    {
        private CImplicitModuleBase m_source;
        private double m_low, m_high;
        private double m_scale2, m_offset2;
        private double m_scale3, m_offset3;
        private double m_scale4, m_offset4;
        private double m_scale6, m_offset6;

        public CImplicitAutoCorrect () : base ()
        {
            m_source = null; m_low = -1.0; m_high = 1.0;
        }
        public CImplicitAutoCorrect (double low, double high) : base ()
        {
            m_source = null; m_low = low; m_high = high;
            calculate ();
        }
        public CImplicitAutoCorrect (CImplicitModuleBase m, double low, double high) : base ()
        {
            m_source = m;
            m_low = low;
            m_high = high;
            calculate ();
        }

        void setSource (CImplicitModuleBase m)
        {
            m_source = m;
            calculate ();
        }

        void setRange (double low, double high)
        {
            m_low = low; m_high = high;
            calculate ();
        }

        void calculate ()
        {
            if (m_source == null) return;
            double mn, mx;
            LCG lcg = new LCG ();
            //lcg.setSeedTime();

            // Calculate 2D
            mn = 10000.0f;
            mx = -10000.0f;
            for (int c = 0; c < 10000; ++c) {
                double nx = lcg.get01 () * 4.0f - 2.0f;
                double ny = lcg.get01 () * 4.0f - 2.0f;

                double v = m_source.get (nx, ny);
                if (v < mn) mn = v;
                if (v > mx) mx = v;
            }
            m_scale2 = (m_high - m_low) / (mx - mn);
            m_offset2 = m_low - mn * m_scale2;

            // Calculate 3D
            mn = 10000.0f;
            mx = -10000.0f;
            for (int c = 0; c < 10000; ++c) {
                double nx = lcg.get01 () * 4.0f - 2.0f;
                double ny = lcg.get01 () * 4.0f - 2.0f;
                double nz = lcg.get01 () * 4.0f - 2.0f;

                double v = m_source.get (nx, ny, nz);
                if (v < mn) mn = v;
                if (v > mx) mx = v;
            }
            m_scale3 = (m_high - m_low) / (mx - mn);
            m_offset3 = m_low - mn * m_scale3;

            // Calculate 4D
            mn = 10000.0f;
            mx = -10000.0f;
            for (int c = 0; c < 10000; ++c) {
                double nx = lcg.get01 () * 4.0f - 2.0f;
                double ny = lcg.get01 () * 4.0f - 2.0f;
                double nz = lcg.get01 () * 4.0f - 2.0f;
                double nw = lcg.get01 () * 4.0f - 2.0f;

                double v = m_source.get (nx, ny, nz, nw);
                if (v < mn) mn = v;
                if (v > mx) mx = v;
            }
            m_scale4 = (m_high - m_low) / (mx - mn);
            m_offset4 = m_low - mn * m_scale4;

            // Calculate 6D
            mn = 10000.0f;
            mx = -10000.0f;
            for (int c = 0; c < 10000; ++c) {
                double nx = lcg.get01 () * 4.0f - 2.0f;
                double ny = lcg.get01 () * 4.0f - 2.0f;
                double nz = lcg.get01 () * 4.0f - 2.0f;
                double nw = lcg.get01 () * 4.0f - 2.0f;
                double nu = lcg.get01 () * 4.0f - 2.0f;
                double nv = lcg.get01 () * 4.0f - 2.0f;

                double v = m_source.get (nx, ny, nz, nw, nu, nv);
                if (v < mn) mn = v;
                if (v > mx) mx = v;
            }
            m_scale6 = (m_high - m_low) / (mx - mn);
            m_offset6 = m_low - mn * m_scale6;
        }


        public override double get (double x, double y)
        {
            if (m_source == null) return 0.0f;

            double v = m_source.get (x, y);
            return Math.Max (m_low, Math.Min (m_high, v * m_scale2 + m_offset2));
        }

        public override double get (double x, double y, double z)
        {
            if (m_source == null) return 0.0f;

            double v = m_source.get (x, y, z);
            return Math.Max (m_low, Math.Min (m_high, v * m_scale3 + m_offset3));
        }
        public override double get (double x, double y, double z, double w)
        {
            if (m_source == null) return 0.0f;

            double v = m_source.get (x, y, z, w);
            return Math.Max (m_low, Math.Min (m_high, v * m_scale4 + m_offset4));
        }

        public override double get (double x, double y, double z, double w, double u, double v)
        {
            if (m_source == null) return 0.0f;

            double val = m_source.get (x, y, z, w, u, v);
            return Math.Max (m_low, Math.Min (m_high, val * m_scale6 + m_offset6));
        }
    }
}
