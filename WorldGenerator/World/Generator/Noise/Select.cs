using System;

namespace Sean.WorldGenerator.Noise
{
    // Select is used to choose between one function or another based on the output value of a 
    // third control function.Two parameters control how the select is performed, threshold and 
    // falloff.Threshold determines where the dividing line is; values on one side of threshold 
    // are taken from one source module, while values on the other side are taken from the second 
    // source.Falloff defines the width of a soft "blend" zone that straddles threshold, helping 
    // to smooth the transition between the two functions.

    public class CImplicitSelect : CImplicitModuleBase
    {
        private CScalarParameter m_low, m_high, m_control;
        private CScalarParameter m_threshold, m_falloff;
        public CImplicitSelect() : base() { }

        public CImplicitSelect(double low, double high, double control, double threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, double high, double control, double threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, double high, double control, CImplicitModuleBase threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, double high, double control, CImplicitModuleBase threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, double high, CImplicitModuleBase control, double threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, double high, CImplicitModuleBase control, double threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, double high, CImplicitModuleBase control, CImplicitModuleBase threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, double high, CImplicitModuleBase control, CImplicitModuleBase threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, CImplicitModuleBase high, double control, double threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, CImplicitModuleBase high, double control, double threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, CImplicitModuleBase high, double control, CImplicitModuleBase threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, CImplicitModuleBase high, double control, CImplicitModuleBase threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, CImplicitModuleBase high, CImplicitModuleBase control, double threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, CImplicitModuleBase high, CImplicitModuleBase control, double threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, CImplicitModuleBase high, CImplicitModuleBase control, CImplicitModuleBase threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(double low, CImplicitModuleBase high, CImplicitModuleBase control, CImplicitModuleBase threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, double high, double control, double threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, double high, double control, double threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, double high, double control, CImplicitModuleBase threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, double high, double control, CImplicitModuleBase threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, double high, CImplicitModuleBase control, double threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, double high, CImplicitModuleBase control, double threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, double high, CImplicitModuleBase control, CImplicitModuleBase threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, double high, CImplicitModuleBase control, CImplicitModuleBase threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, CImplicitModuleBase high, double control, double threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, CImplicitModuleBase high, double control, double threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, CImplicitModuleBase high, double control, CImplicitModuleBase threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, CImplicitModuleBase high, double control, CImplicitModuleBase threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, CImplicitModuleBase high, CImplicitModuleBase control, double threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, CImplicitModuleBase high, CImplicitModuleBase control, double threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, CImplicitModuleBase high, CImplicitModuleBase control, CImplicitModuleBase threshold, double falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }
        public CImplicitSelect(CImplicitModuleBase low, CImplicitModuleBase high, CImplicitModuleBase control, CImplicitModuleBase threshold, CImplicitModuleBase falloff) : base()
        { m_low = new CScalarParameter(low); m_high = new CScalarParameter(high); m_control = new CScalarParameter(control); m_threshold = new CScalarParameter(threshold); m_falloff = new CScalarParameter(falloff); }



        public void setLowSource(CImplicitModuleBase b)
        {
            m_low.set(b);
        }
        public void setHighSource(CImplicitModuleBase b)
        {
            m_high.set(b);
        }
        public void setControlSource(CImplicitModuleBase b)
        {
            m_control.set(b);
        }

        public void setLowSource(double b)
        {
            m_low.set(b);
        }
        public void setHighSource(double b)
        {
            m_high.set(b);
        }
        public void setControlSource(double b)
        {
            m_control.set(b);
        }

        public void setThreshold(double t)
        {
            //m_threshold=t;
            m_threshold.set(t);
        }
        public void setFalloff(double f)
        {
            //m_falloff=f;
            m_falloff.set(f);
        }
        public void setThreshold(CImplicitModuleBase m)
        {
            m_threshold.set(m);
        }
        public void setFalloff(CImplicitModuleBase m)
        {
            m_falloff.set(m);
        }

        public override double get(double x, double y)
        {
            double control = m_control.get(x, y);
            double falloff = m_falloff.get(x, y);
            double threshold = m_threshold.get(x, y);

            if (falloff > 0.0)
            {
                if (control < (threshold - falloff))
                {
                    // Lies outside of falloff area below threshold, return first source
                    return m_low.get(x, y);
                }
                else if (control > (threshold + falloff))
                {
                    // Lise outside of falloff area above threshold, return second source
                    return m_high.get(x, y);
                }
                else
                {
                    // Lies within falloff area.
                    double lower = threshold - falloff;
                    double upper = threshold + falloff;
                    double blend = Utility.quintic_blend((control - lower) / (upper - lower));
                    return anl.lerp(blend, m_low.get(x, y), m_high.get(x, y));
                }
            }
            else
            {
                if (control < threshold) return m_low.get(x, y);
                else return m_high.get(x, y);
            }
        }

        public override double get(double x, double y, double z)
        {
            double control = m_control.get(x, y, z);
            double falloff = m_falloff.get(x, y, z);
            double threshold = m_threshold.get(x, y, z);

            if (falloff > 0.0)
            {
                if (control < (threshold - falloff))
                {
                    // Lies outside of falloff area below threshold, return first source
                    return m_low.get(x, y, z);
                }
                else if (control > (threshold + falloff))
                {
                    // Lise outside of falloff area above threshold, return second source
                    return m_high.get(x, y, z);
                }
                else
                {
                    // Lies within falloff area.
                    double lower = threshold - falloff;
                    double upper = threshold + falloff;
                    double blend = Utility.quintic_blend((control - lower) / (upper - lower));
                    return anl.lerp(blend, m_low.get(x, y, z), m_high.get(x, y, z));
                }
            }
            else
            {
                //std::cout << "Control: " << control << " Threshold: " << threshold << " Low: " << m_low.get(x,y,z) << " High: " << m_high.get(x,y,z) << std::endl;
                if (control < threshold) return m_low.get(x, y, z);
                else return m_high.get(x, y, z);
            }
        }

        public override double get(double x, double y, double z, double w)
        {
            double control = m_control.get(x, y, z, w);
            double falloff = m_falloff.get(x, y, z, w);
            double threshold = m_threshold.get(x, y, z, w);

            if (falloff > 0.0)
            {
                if (control < (threshold - falloff))
                {
                    // Lies outside of falloff area below threshold, return first source
                    return m_low.get(x, y, z, w);
                }
                else if (control > (threshold + falloff))
                {
                    // Lise outside of falloff area above threshold, return second source
                    return m_high.get(x, y, z, w);
                }
                else
                {
                    // Lies within falloff area.
                    double lower = threshold - falloff;
                    double upper = threshold + falloff;
                    double blend = Utility.quintic_blend((control - lower) / (upper - lower));
                    return anl.lerp(blend, m_low.get(x, y, z, w), m_high.get(x, y, z, w));
                }
            }
            else
            {
                if (control < threshold) return m_low.get(x, y, z, w);
                else return m_high.get(x, y, z, w);
            }
        }

        public override double get(double x, double y, double z, double w, double u, double v)
        {
            double control = m_control.get(x, y, z, w, u, v);
            double falloff = m_falloff.get(x, y, z, w, u, v);
            double threshold = m_threshold.get(x, y, z, w, u, v);

            if (falloff > 0.0)
            {
                if (control < (threshold - falloff))
                {
                    // Lies outside of falloff area below threshold, return first source
                    return m_low.get(x, y, z, w, u, v);
                }
                else if (control > (threshold + falloff))
                {
                    // Lise outside of falloff area above threshold, return second source
                    return m_high.get(x, y, z, w, u, v);
                }
                else
                {
                    // Lies within falloff area.
                    double lower = threshold - falloff;
                    double upper = threshold + falloff;
                    double blend = Utility.quintic_blend((control - lower) / (upper - lower));
                    return anl.lerp(blend, m_low.get(x, y, z, w, u, v), m_high.get(x, y, z, w, u, v));
                }
            }
            else
            {
                if (control < threshold) return m_low.get(x, y, z, w, u, v);
                else return m_high.get(x, y, z, w, u, v);
            }
        }


    }
}
