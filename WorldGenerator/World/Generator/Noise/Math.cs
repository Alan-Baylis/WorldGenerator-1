using System;
namespace Sean.WorldGenerator.Noise
{
    public enum EMathOperation
    {
        // Unary math ops
        ACOS,
        ASIN,
        ATAN,
        COS,
        SIN,
        TAN,
        ABS,
        FLOOR,
        CEIL,
        EXP,
        LOG10,
        LOG2,
        LOGN,
        ONEMINUS,
        SQRT,
        INTEGER,
        FRACTIONAL,
        EASECUBIC,
        EASEQUINTIC,

        // EBinary Math Operations
        POW,
        FMOD,
        BIAS,
        GAIN,
        PMINUS,
        SUM,
        MULTIPLY,
        DIVIDE,
        SUBTRACT,
        MAXIMUM,
        MINIMUM
    };

    public class CImplicitMath : CImplicitModuleBase
    {
        private EMathOperation m_op;
        private CScalarParameter m_source;
        private CScalarParameter m_parameter;

        CImplicitMath() : base()
        { m_op = EMathOperation.ABS; m_source = new CScalarParameter(0.0); m_parameter = new CScalarParameter(0.0); }

        CImplicitMath(EMathOperation op, double source, double p) : base()
        { m_op = op; m_source = new CScalarParameter(source); m_parameter = new CScalarParameter(p); }
        CImplicitMath(EMathOperation op, CImplicitModuleBase source, double p) : base()
        { m_op = op; m_source = new CScalarParameter(source); m_parameter = new CScalarParameter(p); }
        CImplicitMath(EMathOperation op, double source, CImplicitModuleBase p) : base()
        { m_op = op; m_source = new CScalarParameter(source); m_parameter = new CScalarParameter(p); }
        CImplicitMath(EMathOperation op, CImplicitModuleBase source, CImplicitModuleBase p) : base()
        { m_op = op; m_source = new CScalarParameter(source); m_parameter = new CScalarParameter(p); }

        public void setSource(double v)
        {
            m_source.set(v);
        }

        public void setSource(CImplicitModuleBase b)
        {
            m_source.set(b);
        }

        public void setParameter(double v)
        {
            m_parameter.set(v);
        }

        public void setParameter(CImplicitModuleBase b)
        {
            m_parameter.set(b);
        }

        public void setOperation(EMathOperation op)
        {
            m_op = op;
        }

        public override double get(double x, double y)
        {
            double v = m_source.get(x, y);
            double p = m_parameter.get(x, y);
            return applyOp(v, p);
        }

        public override double get(double x, double y, double z)
        {
            double v = m_source.get(x, y, z);
            double p = m_parameter.get(x, y, z);
            return applyOp(v, p);
        }

        public override double get(double x, double y, double z, double w)
        {
            double v = m_source.get(x, y, z, w);
            double p = m_parameter.get(x, y, z, w);
            return applyOp(v, p);
        }

        public override double get(double x, double y, double z, double w, double u, double v)
        {
            double val = m_source.get(x, y, z, w, u, v);
            double p = m_parameter.get(x, y, z, w, u, v);
            return applyOp(val, p);
        }


        private double applyOp(double v, double p)
        {
            switch (m_op)
            {
                case EMathOperation.ACOS: return Math.Acos(v); 
                case EMathOperation.ASIN: return Math.Asin(v);
                case EMathOperation.ATAN: return Math.Atan(v);
                case EMathOperation.COS: return Math.Cos(v);
                case EMathOperation.SIN: return Math.Sin(v);
                case EMathOperation.TAN: return Math.Tan(v);
                case EMathOperation.ABS: return (v < 0) ? -v : v;
                case EMathOperation.FLOOR: return Math.Floor(v);
                case EMathOperation.CEIL: return Math.Ceiling(v);
                case EMathOperation.POW: return Math.Pow(v, p);
                case EMathOperation.EXP: return Math.Exp(v);
                case EMathOperation.LOG10: return Math.Log10(v);
                case EMathOperation.LOG2: return Math.Log(v) / Math.Log(2.0);
                case EMathOperation.LOGN: return Math.Log(v);
                case EMathOperation.FMOD: return Math.IEEERemainder(v, p);
                case EMathOperation.BIAS: return Utility.bias(p, v);
                case EMathOperation.GAIN: return Utility.gain(p, v);
                case EMathOperation.ONEMINUS: return 1.0 - v;
                case EMathOperation.PMINUS: return p - v;
                case EMathOperation.SQRT: return Math.Sqrt(v);
                case EMathOperation.INTEGER: return (double)(int)v;
                case EMathOperation.FRACTIONAL: return v - (double)(int)v;
                case EMathOperation.EASECUBIC: return Utility.hermite_blend(v);
                case EMathOperation.EASEQUINTIC: return Utility.quintic_blend(v);
                case EMathOperation.SUM: return v + p;
                case EMathOperation.MULTIPLY: return v * p; 
                case EMathOperation.DIVIDE: return v / p; 
                case EMathOperation.SUBTRACT: return v - p; 
                case EMathOperation.MAXIMUM: return Math.Max(v, p); 
                case EMathOperation.MINIMUM: return Math.Min(v, p); 
                default: return v; 
            }
        }
    }
}
