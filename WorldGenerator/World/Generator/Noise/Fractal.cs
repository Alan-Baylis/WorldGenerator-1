using System;

namespace Sean.WorldGenerator.Noise
{

    // Fractals are a special type of combiner that combine up to 20 noise sources using 
    // fractal methods pioneered by Perlin, Musgrave, and friends.They come in various 
    // types(specifiable through setType() or in the constructor). Each fractal has up to 
    // 20 built-in BasisFunctions whose basistype and interptype can be set via the provided methods.
    // Additionally, you can obtain a pointer to any source of the fractal via getBasis(). 
    // Any source module in the fractal may also be overridden by an external noise function 
    // via overrideSource(). The complexity of this system serves a purpose: "generic" fractals of a 
    // given type, with basis functions of all the same type, can easily be instance just by construction, 
    // yet more complex behavior can be produced by overriding layers with external sources, to build up very 
    // complex fractals, if so desired. The basic fractal types are:
    //
    // FBM
    // RIDGEDMULTI
    // BILLOW
    // MULTI
    // HYBRIDMULTI
    //
    // Fractals are highly parameterized.The two most useful parameters are numoctaves which determines how 
    // many layers contribute to the fractal, and frequency which specifies the density of the function.
    // Frequency mimics combining a ScaleDomain function to the source, multiplying the input coordinates 
    // by frequency before calculating the function. Other parameters that control the fractal are offset, 
    // lacunarity, gain and H.These parameters can have subtle, drastic, or no effect on the fractal, 
    // depending on the type, and they are typically best left alone.


    public enum EFractalTypes
    {
        FBM,
        RIDGEDMULTI,
        BILLOW,
        MULTI,
        HYBRIDMULTI,
        DECARPENTIERSWISS
    };

    class CImplicitFractal : CImplicitModuleBase
    {
        private const int MaxSources = 20;

        private std::shared_ptr<CImplicitBasisFunction> [] m_basis;
        new private CImplicitModuleBase [] m_source;
        private float [] m_exparray;
        private float [] [] m_correct;

        private float m_offset, m_gain, m_H;
        private float m_frequency, m_lacunarity;
        private uint m_numoctaves;
        private EFractalTypes m_type;
        private bool m_rotatedomain;

        CImplicitFractal (EFractalTypes type, uint basistype, uint interptype, int octaves, float freq, bool rotatedomain) : base ()
        {
            m_rotatedomain = rotatedomain;
            setNumOctaves (octaves);
            setFrequency (freq);
            setLacunarity (2.0f);
            setType (type);
            setAllSourceTypes (basistype, interptype);
            resetAllSources ();

            m_basis = new std::shared_ptr<CImplicitBasisFunction> [MaxSources];
            m_source = new CImplicitModuleBase [MaxSources];
            m_exparray = new float [MaxSources];
            m_correct = new float [MaxSources] [2];
        }

        void setNumOctaves (int n) { if (n >= MaxSources) n = MaxSources - 1; m_numoctaves = n; }
        void setFrequency (float f) { m_frequency = f; }
        void setLacunarity (float l) { m_lacunarity = l; }
        void setGain (float g) { m_gain = g; }
        void setOffset (float o) { m_offset = o; }
        void setH (float h) { m_H = h; }

        void setType (EFractalTypes t)
        {
            m_type = t;
            switch (t) {
            case EFractalTypes.FBM: m_H = 1.0f; m_gain = 0.5f; m_offset = 0; fBm_calcWeights (); break;
            case EFractalTypes.RIDGEDMULTI: m_H = 0.9f; m_gain = 0.5f; m_offset = 1; RidgedMulti_calcWeights (); break;
            case EFractalTypes.BILLOW: m_H = 1; m_gain = 0.5f; m_offset = 0; Billow_calcWeights (); break;
            case EFractalTypes.MULTI: m_H = 1; m_offset = 0; m_gain = 0; Multi_calcWeights (); break;
            case EFractalTypes.HYBRIDMULTI: m_H = 0.25f; m_gain = 1; m_offset = 0.7f; HybridMulti_calcWeights (); break;
            case EFractalTypes.DECARPENTIERSWISS: m_H = 0.9f; m_gain = 0.6f; m_offset = 0.15f; DeCarpentierSwiss_calcWeights (); break;
            default: m_H = 1.0f; m_gain = 0; m_offset = 0; fBm_calcWeights (); break;
            };
        }

        void setAllSourceTypes (uint basis_type, uint interp)
        {
            for (int i = 0; i < MaxSources; ++i) {
                //m_basis[i].setType(basis_type);
                //m_basis[i].setInterp(interp);
                m_basis [i] = std::shared_ptr<CImplicitBasisFunction> (new CImplicitBasisFunction (basis_type, interp, m_rotatedomain));
                //if(!m_rotatedomain) m_basis[i]->setRotationAngle(1,0,0,0);
            }
        }

        void setSourceType (int which, uint type, uint interp)
        {
            if (which >= MaxSources || which < 0) return;
            if (!m_basis [which]) return;
            m_basis [which]->setType (type);
            m_basis [which]->setInterp (interp);
            //if(!m_rotatedomain) m_basis[which]->setRotationAngle(1,0,0,0);
        }

        void overrideSource (int which, CImplicitModuleBase b)
        {
            if (which < 0 || which >= MaxSources) return;
            m_source [which] = b;
        }

        void resetSource (int which)
        {
            if (which < 0 || which >= MaxSources) return;
            if (!m_basis [which]) return;
            m_source [which] = m_basis [which].get ();
        }

        void resetAllSources ()
        {
            for (int c = 0; c < MaxSources; ++c) {
                m_source [c] = m_basis [c].get ();
                //if(!m_rotatedomain) m_basis[c]->setRotationAngle(1,0,0,0);
            }
        }

        public override void setSeed (uint seed)
        {
            for (int c = 0; c < MaxSources; ++c) {
                m_source [c].setSeed ((uint)(seed + c * 300));
                //if(!m_rotatedomain) m_basis[c]->setRotationAngle(1,0,0,0);
                // else m_basis[c]->setNoRotation();
            }
        }

        CImplicitModuleBase getBasis (int which)
        {
            if (which < 0 || which >= MaxSources) return null;
            return m_basis [which].get ();
        }

        public override float get (float x, float y)
        {
            float v;
            switch (m_type) {
            case EFractalTypes.FBM: v = fBm_get (x, y); break;
            case EFractalTypes.RIDGEDMULTI: v = RidgedMulti_get (x, y); break;
            case EFractalTypes.BILLOW: v = Billow_get (x, y); break;
            case EFractalTypes.MULTI: v = Multi_get (x, y); break;
            case EFractalTypes.HYBRIDMULTI: v = HybridMulti_get (x, y); break;
            case EFractalTypes.DECARPENTIERSWISS: v = DeCarpentierSwiss_get (x, y); break;
            default: v = fBm_get (x, y); break;
            }
            //return clamp(v,-1.0,1.0);
            return v;
        }

        public override float get (float x, float y, float z)
        {
            float val;
            switch (m_type) {
            case EFractalTypes.FBM: val = fBm_get (x, y, z); break;
            case EFractalTypes.RIDGEDMULTI: val = RidgedMulti_get (x, y, z); break;
            case EFractalTypes.BILLOW: val = Billow_get (x, y, z); break;
            case EFractalTypes.MULTI: val = Multi_get (x, y, z); break;
            case EFractalTypes.HYBRIDMULTI: val = HybridMulti_get (x, y, z); break;
            case EFractalTypes.DECARPENTIERSWISS: val = DeCarpentierSwiss_get (x, y, z); break;
            default: val = fBm_get (x, y, z); break;
            }
            //return clamp(val,-1.0,1.0);
            return val;
        }

        public override float get (float x, float y, float z, float w)
        {
            float val;
            switch (m_type) {
            case EFractalTypes.FBM: val = fBm_get (x, y, z, w); break;
            case EFractalTypes.RIDGEDMULTI: val = RidgedMulti_get (x, y, z, w); break;
            case EFractalTypes.BILLOW: val = Billow_get (x, y, z, w); break;
            case EFractalTypes.MULTI: val = Multi_get (x, y, z, w); break;
            case EFractalTypes.HYBRIDMULTI: val = HybridMulti_get (x, y, z, w); break;
            case EFractalTypes.DECARPENTIERSWISS: val = DeCarpentierSwiss_get (x, y, z, w); break;
            default: val = fBm_get (x, y, z, w); break;
            }
            return val;
        }

        public override float get (float x, float y, float z, float w, float u, float v)
        {
            float val;
            switch (m_type) {
            case EFractalTypes.FBM: val = fBm_get (x, y, z, w, u, v); break;
            case EFractalTypes.RIDGEDMULTI: val = RidgedMulti_get (x, y, z, w, u, v); break;
            case EFractalTypes.BILLOW: val = Billow_get (x, y, z, w, u, v); break;
            case EFractalTypes.MULTI: val = Multi_get (x, y, z, w, u, v); break;
            case EFractalTypes.HYBRIDMULTI: val = HybridMulti_get (x, y, z, w, u, v); break;
            case EFractalTypes.DECARPENTIERSWISS: val = DeCarpentierSwiss_get (x, y, z, w, u, v); break;
            default: val = fBm_get (x, y, z, w, u, v); break;
            }

            return val;
        }

        void fBm_calcWeights ()
        {
            //std::cout << "Weights: ";
            for (int i = 0; i < MaxSources; ++i) {
                m_exparray [i] = (float)Math.Pow (m_lacunarity, -i * m_H);
            }
            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            float minvalue = 0.0f, maxvalue = 0.0f;
            for (int i = 0; i < MaxSources; ++i) {
                minvalue += -1.0f * m_exparray [i];
                maxvalue += 1.0f * m_exparray [i];

                float A = -1.0f, B = 1.0f;
                float scale = (B - A) / (maxvalue - minvalue);
                float bias = A - minvalue * scale;
                m_correct [i] [0] = scale;
                m_correct [i] [1] = bias;

                //std::cout << minvalue << " " << maxvalue << " " << scale << " " << bias << std::endl;
            }
        }

        void RidgedMulti_calcWeights ()
        {
            for (int i = 0; i < MaxSources; ++i) {
                m_exparray [i] = (float)Math.Pow (m_lacunarity, -i * m_H);
            }
            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            float minvalue = 0.0f, maxvalue = 0.0f;
            for (int i = 0; i < MaxSources; ++i) {
                minvalue += (m_offset - 1.0f) * (m_offset - 1.0f) * m_exparray [i];
                maxvalue += (m_offset) * (m_offset) * m_exparray [i];

                float A = -1.0f, B = 1.0f;
                float scale = (B - A) / (maxvalue - minvalue);
                float bias = A - minvalue * scale;
                m_correct [i] [0] = scale;
                m_correct [i] [1] = bias;
            }

        }

        void DeCarpentierSwiss_calcWeights ()
        {
            for (int i = 0; i < MaxSources; ++i) {
                m_exparray [i] = (float)Math.Pow (m_lacunarity, -i * m_H);
            }
            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            float minvalue = 0.0f, maxvalue = 0.0f;
            for (int i = 0; i < MaxSources; ++i) {
                minvalue += (m_offset - 1.0f) * (m_offset - 1.0f) * m_exparray [i];
                maxvalue += (m_offset) * (m_offset) * m_exparray [i];

                float A = -1.0f, B = 1.0f;
                float scale = (B - A) / (maxvalue - minvalue);
                float bias = A - minvalue * scale;
                m_correct [i] [0] = scale;
                m_correct [i] [1] = bias;
            }

        }

        void Billow_calcWeights ()
        {
            for (int i = 0; i < MaxSources; ++i) {
                m_exparray [i] = (float)Math.Pow (m_lacunarity, -i * m_H);
            }

            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            float minvalue = 0.0f, maxvalue = 0.0f;
            for (int i = 0; i < MaxSources; ++i) {
                minvalue += -1.0f * m_exparray [i];
                maxvalue += 1.0f * m_exparray [i];

                float A = -1.0f, B = 1.0f;
                float scale = (B - A) / (maxvalue - minvalue);
                float bias = A - minvalue * scale;
                m_correct [i] [0] = scale;
                m_correct [i] [1] = bias;
            }

        }

        void Multi_calcWeights ()
        {
            for (int i = 0; i < MaxSources; ++i) {
                m_exparray [i] = (float)Math.Pow (m_lacunarity, -i * m_H);
            }
            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            float minvalue = 1.0f, maxvalue = 1.0f;
            for (int i = 0; i < MaxSources; ++i) {
                minvalue *= -1.0f * m_exparray [i] + 1.0f;
                maxvalue *= 1.0f * m_exparray [i] + 1.0f;

                float A = -1.0f, B = 1.0f;
                float scale = (B - A) / (maxvalue - minvalue);
                float bias = A - minvalue * scale;
                m_correct [i] [0] = scale;
                m_correct [i] [1] = bias;
            }
        }

        void HybridMulti_calcWeights ()
        {
            for (int i = 0; i < MaxSources; ++i) {
                m_exparray [i] = (float)Math.Pow (m_lacunarity, -i * m_H);
            }
            // Calculate scale/bias pairs by guessing at minimum and maximum values and remapping to [-1,1]
            float minvalue = 1.0f, maxvalue = 1.0f;
            float weightmin, weightmax;
            float A = -1.0f, B = 1.0f, scale, bias;

            minvalue = m_offset - 1.0f;
            maxvalue = m_offset + 1.0f;
            weightmin = m_gain * minvalue;
            weightmax = m_gain * maxvalue;

            scale = (B - A) / (maxvalue - minvalue);
            bias = A - minvalue * scale;
            m_correct [0] [0] = scale;
            m_correct [0] [1] = bias;

            for (int i = 1; i < MaxSources; ++i) {
                if (weightmin > 1.0) weightmin = 1.0f;
                if (weightmax > 1.0) weightmax = 1.0f;

                float signal = (m_offset - 1.0f) * m_exparray [i];
                minvalue += signal * weightmin;
                weightmin *= m_gain * signal;

                signal = (m_offset + 1.0f) * m_exparray [i];
                maxvalue += signal * weightmax;
                weightmax *= m_gain * signal;

                scale = (B - A) / (maxvalue - minvalue);
                bias = A - minvalue * scale;
                m_correct [i] [0] = scale;
                m_correct [i] [1] = bias;
            }
        }

        float fBm_get (float x, float y)
        {
            float sum = 0.0f;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y);
                sum += n * amp;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
            }
            return sum;
        }

        float fBm_get (float x, float y, float z)
        {
            float sum = 0.0f;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y, z);
                sum += n * amp;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
            }
            return sum;
        }

        float fBm_get (float x, float y, float z, float w)
        {
            float sum = 0.0f;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y, z, w);
                sum += n * amp;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
            }
            return sum;
        }

        float fBm_get (float x, float y, float z, float w, float u, float v)
        {
            float sum = 0.0f;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            u *= m_frequency;
            v *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y, z, w);
                sum += n * amp;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
                u *= m_lacunarity;
                v *= m_lacunarity;
            }
            return sum;
        }

        float Multi_get (float x, float y)
        {
            float value = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                value *= m_source [i].get (x, y) * m_exparray [i] + 1.0f;
                x *= m_lacunarity;
                y *= m_lacunarity;
            }
            return value * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float Multi_get (float x, float y, float z, float w)
        {
            float value = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                value *= m_source [i].get (x, y, z, w) * m_exparray [i] + 1.0f;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
            }
            return value * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float Multi_get (float x, float y, float z)
        {
            float value = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                value *= m_source [i].get (x, y, z) * m_exparray [i] + 1.0f;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
            }
            return value * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float Multi_get (float x, float y, float z, float w, float u, float v)
        {
            float value = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            u *= m_frequency;
            v *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                value *= m_source [i].get (x, y, z, w, u, v) * m_exparray [i] + 1.0f;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
                u *= m_lacunarity;
                v *= m_lacunarity;
            }
            return value * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float Billow_get (float x, float y)
        {
            float sum = 0.0f;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y);
                sum += (2.0f * Math.Abs (n) - 1.0f) * amp;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
            }
            return sum;
        }

        float Billow_get (float x, float y, float z, float w)
        {
            float sum = 0.0f;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y, z, w);
                sum += (2.0f * Math.Abs (n) - 1.0f) * amp;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
            }
            return sum;
        }

        float Billow_get (float x, float y, float z)
        {
            float sum = 0.0f;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y, z);
                sum += (2.0f * Math.Abs (n) - 1.0f) * amp;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
            }
            return sum;
        }

        float Billow_get (float x, float y, float z, float w, float u, float v)
        {
            float sum = 0.0f;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            u *= m_frequency;
            v *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y, z, w, u, v);
                sum += (2.0f * Math.Abs (n) - 1.0f) * amp;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
                u *= m_lacunarity;
                v *= m_lacunarity;
            }
            return sum;
        }

        float RidgedMulti_get (float x, float y)
        {
            float sum = 0;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y);
                n = 1.0f - Math.Abs (n);
                sum += amp * n;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
            }
            return sum;
            /*float result=0.0, signal;
            x*=m_frequency;
            y*=m_frequency;
            for(uint i=0; i<m_numoctaves; ++i)
            {
                signal=m_source[i]->get(x,y);
                signal=m_offset-fabs(signal);
                signal *= signal;
                result +=signal*m_exparray[i];
                x*=m_lacunarity;
                y*=m_lacunarity;
            }
            return result*m_correct[m_numoctaves-1][0] + m_correct[m_numoctaves-1][1];*/
        }

        float RidgedMulti_get (float x, float y, float z, float w)
        {
            float result = 0.0f, signal;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                signal = m_source [i].get (x, y, z, w);
                signal = m_offset - Math.Abs (signal);
                signal *= signal;
                result += signal * m_exparray [i];
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
            }
            return result * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float RidgedMulti_get (float x, float y, float z)
        {
            float sum = 0;
            float amp = 1.0f;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x, y, z);
                n = 1.0f - Math.Abs (n);
                sum += amp * n;
                amp *= m_gain;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
            }
            return sum;
        }

        float RidgedMulti_get (float x, float y, float z, float w, float u, float v)
        {
            float result = 0.0f, signal;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            u *= m_frequency;
            v *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                signal = m_source [i].get (x, y, z, w, u, v);
                signal = m_offset - Math.Abs (signal);
                signal *= signal;
                result += signal * m_exparray [i];
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
                u *= m_lacunarity;
                v *= m_lacunarity;
            }
            return result * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float HybridMulti_get (float x, float y)
        {
            float value, signal, weight;
            x *= m_frequency;
            y *= m_frequency;
            value = m_source [0].get (x, y) + m_offset;
            weight = m_gain * value;
            x *= m_lacunarity;
            y *= m_lacunarity;
            for (uint i = 1; i < m_numoctaves; ++i) {
                if (weight > 1.0) weight = 1.0f;
                signal = (m_source [i].get (x, y) + m_offset) * m_exparray [i];
                value += weight * signal;
                weight *= m_gain * signal;
                x *= m_lacunarity;
                y *= m_lacunarity;
            }
            return value * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float HybridMulti_get (float x, float y, float z)
        {
            float value, signal, weight;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            value = m_source [0].get (x, y, z) + m_offset;
            weight = m_gain * value;
            x *= m_lacunarity;
            y *= m_lacunarity;
            z *= m_lacunarity;
            for (uint i = 1; i < m_numoctaves; ++i) {
                if (weight > 1.0) weight = 1.0f;
                signal = (m_source [i].get (x, y, z) + m_offset) * m_exparray [i];
                value += weight * signal;
                weight *= m_gain * signal;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
            }
            return value * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float HybridMulti_get (float x, float y, float z, float w)
        {
            float value, signal, weight;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            value = m_source [0].get (x, y, z, w) + m_offset;
            weight = m_gain * value;
            x *= m_lacunarity;
            y *= m_lacunarity;
            z *= m_lacunarity;
            w *= m_lacunarity;
            for (uint i = 1; i < m_numoctaves; ++i) {
                if (weight > 1.0) weight = 1.0f;
                signal = (m_source [i].get (x, y, z, w) + m_offset) * m_exparray [i];
                value += weight * signal;
                weight *= m_gain * signal;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
            }
            return value * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float HybridMulti_get (float x, float y, float z, float w, float u, float v)
        {
            float value, signal, weight;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            u *= m_frequency;
            v *= m_frequency;
            value = m_source [0].get (x, y, z, w, u, v) + m_offset;
            weight = m_gain * value;
            x *= m_lacunarity;
            y *= m_lacunarity;
            z *= m_lacunarity;
            w *= m_lacunarity;
            u *= m_lacunarity;
            v *= m_lacunarity;
            for (uint i = 1; i < m_numoctaves; ++i) {
                if (weight > 1.0) weight = 1.0f;
                signal = (m_source [i].get (x, y, z, w, u, v) + m_offset) * m_exparray [i];
                value += weight * signal;
                weight *= m_gain * signal;
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
                u *= m_lacunarity;
                v *= m_lacunarity;
            }
            return value * m_correct [m_numoctaves - 1] [0] + m_correct [m_numoctaves - 1] [1];
        }

        float DeCarpentierSwiss_get (float x, float y)
        {
            float sum = 0;
            float amp = 1.0f;
            float dx_sum = 0;
            float dy_sum = 0;
            x *= m_frequency;
            y *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x + m_offset * dx_sum, y + m_offset * dy_sum);
                float dx = m_source [i].get_dx (x + m_offset * dx_sum, y + m_offset * dy_sum);
                float dy = m_source [i].get_dy (x + m_offset * dx_sum, y + m_offset * dy_sum);
                sum += amp * (1.0f - Math.Abs (n));
                dx_sum += amp * dx * -n;
                dy_sum += amp * dy * -n;
                amp *= m_gain * Utility.clamp (sum, (float)0.0, (float)1.0);
                x *= m_lacunarity;
                y *= m_lacunarity;
            }
            return sum;
        }

        float DeCarpentierSwiss_get (float x, float y, float z, float w)
        {
            float sum = 0;
            float amp = 1.0f;
            float dx_sum = 0;
            float dy_sum = 0;
            float dz_sum = 0;
            float dw_sum = 0;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum);
                float dx = m_source [i].get_dx (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum);
                float dy = m_source [i].get_dy (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum);
                float dz = m_source [i].get_dz (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum);
                float dw = m_source [i].get_dw (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum);
                sum += amp * (1.0f - Math.Abs (n));
                dx_sum += amp * dx * -n;
                dy_sum += amp * dy * -n;
                dz_sum += amp * dz * -n;
                dw_sum += amp * dw * -n;
                amp *= m_gain * Utility.clamp (sum, (float)0.0, (float)1.0);
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
            }
            return sum;
        }

        float DeCarpentierSwiss_get (float x, float y, float z)
        {
            float sum = 0;
            float amp = 1.0f;
            float dx_sum = 0;
            float dy_sum = 0;
            float dz_sum = 0;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum);
                float dx = m_source [i].get_dx (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum);
                float dy = m_source [i].get_dy (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum);
                float dz = m_source [i].get_dz (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum);
                sum += amp * (1.0f - Math.Abs (n));
                dx_sum += amp * dx * -n;
                dy_sum += amp * dy * -n;
                dz_sum += amp * dz * -n;
                amp *= m_gain * Utility.clamp (sum, (float)0.0, (float)1.0);
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
            }
            return sum;
        }

        float DeCarpentierSwiss_get (float x, float y, float z, float w, float u, float v)
        {
            float sum = 0;
            float amp = 1.0f;
            float dx_sum = 0;
            float dy_sum = 0;
            float dz_sum = 0;
            float dw_sum = 0;
            float du_sum = 0;
            float dv_sum = 0;
            x *= m_frequency;
            y *= m_frequency;
            z *= m_frequency;
            w *= m_frequency;
            u *= m_frequency;
            v *= m_frequency;
            for (uint i = 0; i < m_numoctaves; ++i) {
                float n = m_source [i].get (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum);
                float dx = m_source [i].get_dx (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dx_sum, w + m_offset * dw_sum, u + m_offset * du_sum, v + m_offset * dv_sum);
                float dy = m_source [i].get_dy (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum, u + m_offset * du_sum, v + m_offset * dv_sum);
                float dz = m_source [i].get_dz (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum, u + m_offset * du_sum, v + m_offset * dv_sum);
                float dw = m_source [i].get_dw (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum, u + m_offset * du_sum, v + m_offset * dv_sum);
                float du = m_source [i].get_du (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum, u + m_offset * du_sum, v + m_offset * dv_sum);
                float dv = m_source [i].get_dv (x + m_offset * dx_sum, y + m_offset * dy_sum, z + m_offset * dz_sum, w + m_offset * dw_sum, u + m_offset * du_sum, v + m_offset * dv_sum);
                sum += amp * (1.0f - Math.Abs (n));
                dx_sum += amp * dx * -n;
                dy_sum += amp * dy * -n;
                dz_sum += amp * dz * -n;
                dw_sum += amp * dw * -n;
                du_sum += amp * du * -n;
                dv_sum += amp * dv * -n;
                amp *= m_gain * Utility.clamp (sum, (float)0.0, (float)1.0);
                x *= m_lacunarity;
                y *= m_lacunarity;
                z *= m_lacunarity;
                w *= m_lacunarity;
                u *= m_lacunarity;
                v *= m_lacunarity;
            }
            return sum;
        }

    }
}