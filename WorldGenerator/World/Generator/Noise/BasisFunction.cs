using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    public class CImplicitBasisFunction : CImplicitModuleBase
    {
        public enum EBasisTypes
        {
            VALUE,
            GRADIENT,
            GRADVAL,
            SIMPLEX,
            WHITE
        }

        public enum EInterpTypes
        {
            NONE,
            LINEAR,
            CUBIC,
            QUINTIC
        }

        private double[] m_scale, m_offset;
        private anl.interp_func m_interp;
        private anl.noise_func2 m_2d;
        private anl.noise_func3 m_3d;
        private anl.noise_func4 m_4d;
        private anl.noise_func6 m_6d;
        private uint m_seed;

        private double[,] m_rotmatrix;
        private double cos2d, sin2d;
        private bool m_rotatedomain;

        public CImplicitBasisFunction() : base()
        {
            m_rotmatrix = new double[3, 3];
            m_scale = new double[4];
            m_offset = new double[4];

            m_rotatedomain = true;
            setType(EBasisTypes.GRADIENT);
            setInterp(EInterpTypes.QUINTIC);
            setSeed(1000);
        }
        public CImplicitBasisFunction(EBasisTypes type, EInterpTypes interp, bool rotatedomain) : base()
        {
            m_rotmatrix = new double[3, 3];
            m_scale = new double[4];
            m_offset = new double[4];

            m_rotatedomain = rotatedomain;
            setType(type);
            setInterp(interp);
            setSeed(1000);
        }

        override public void setSeed(uint seed)
        {
            m_seed = seed;
            LCG lcg = new LCG();
            lcg.setSeed(seed);

            if (m_rotatedomain)
            {
                double ax, ay, az;
                double len;

                ax = lcg.get01();
                ay = lcg.get01();
                az = lcg.get01();
                len = Math.Sqrt(ax * ax + ay * ay + az * az);
                ax /= len;
                ay /= len;
                az /= len;
                setRotationAngle(ax, ay, az, (lcg.get01() * 3.141592 * 2.0));
                double angle = (lcg.get01() * 3.14159265 * 2.0);
                cos2d = Math.Cos(angle);
                sin2d = Math.Sin(angle);
            }
            else
            {
                setNoRotation();
            }
        }

        public void setType(EBasisTypes type)
        {
            switch (type)
            {
                case EBasisTypes.VALUE: m_2d = anl.value_noise2D; m_3d = anl.value_noise3D; m_4d = anl.value_noise4D; m_6d = anl.value_noise6D; break;
                case EBasisTypes.GRADIENT: m_2d = anl.gradient_noise2D; m_3d = anl.gradient_noise3D; m_4d = anl.gradient_noise4D; m_6d = anl.gradient_noise6D; break;
                case EBasisTypes.GRADVAL: m_2d = anl.gradval_noise2D; m_3d = anl.gradval_noise3D; m_4d = anl.gradval_noise4D; m_6d = anl.gradval_noise6D; break;
                case EBasisTypes.WHITE: m_2d = anl.white_noise2D; m_3d = anl.white_noise3D; m_4d = anl.white_noise4D; m_6d = anl.white_noise6D; break;
                case EBasisTypes.SIMPLEX: m_2d = anl.simplex_noise2D; m_3d = anl.simplex_noise3D; m_4d = anl.simplex_noise4D; m_6d = anl.simplex_noise6D; break;
                default: m_2d = anl.gradient_noise2D; m_3d = anl.gradient_noise3D; m_4d = anl.gradient_noise4D; m_6d = anl.gradient_noise6D; break;
            }
            setMagicNumbers(type);
        }

        public void setInterp(EInterpTypes interp)
        {
            switch (interp)
            {
                case EInterpTypes.NONE: m_interp = anl.noInterp; break;
                case EInterpTypes.LINEAR: m_interp = anl.linearInterp; break;
                case EInterpTypes.CUBIC: m_interp = anl.hermiteInterp; break;
                default: m_interp = anl.quinticInterp; break;
            }
        }

        public override double get(double x, double y)
        {
            double nx, ny;
            nx = x * cos2d - y * sin2d;
            ny = y * cos2d + x * sin2d;
            return m_2d(nx, ny, m_seed, m_interp);
        }
        public override double get(double x, double y, double z)
        {
            double nx, ny, nz;
            nx = (m_rotmatrix[0, 0] * x) + (m_rotmatrix[1, 0] * y) + (m_rotmatrix[2, 0] * z);
            ny = (m_rotmatrix[0, 1] * x) + (m_rotmatrix[1, 1] * y) + (m_rotmatrix[2, 1] * z);
            nz = (m_rotmatrix[0, 2] * x) + (m_rotmatrix[1, 2] * y) + (m_rotmatrix[2, 2] * z);
            return m_3d(nx, ny, nz, m_seed, m_interp);
        }
        public override double get(double x, double y, double z, double w)
        {
            double nx, ny, nz;
            nx = (m_rotmatrix[0, 0] * x) + (m_rotmatrix[1, 0] * y) + (m_rotmatrix[2, 0] * z);
            ny = (m_rotmatrix[0, 1] * x) + (m_rotmatrix[1, 1] * y) + (m_rotmatrix[2, 1] * z);
            nz = (m_rotmatrix[0, 2] * x) + (m_rotmatrix[1, 2] * y) + (m_rotmatrix[2, 2] * z);
            return m_4d(nx, ny, nz, w, m_seed, m_interp);
        }
        public override double get(double x, double y, double z, double w, double u, double v)
        {
            double nx, ny, nz;
            nx = (m_rotmatrix[0, 0] * x) + (m_rotmatrix[1, 0] * y) + (m_rotmatrix[2, 0] * z);
            ny = (m_rotmatrix[0, 1] * x) + (m_rotmatrix[1, 1] * y) + (m_rotmatrix[2, 1] * z);
            nz = (m_rotmatrix[0, 2] * x) + (m_rotmatrix[1, 2] * y) + (m_rotmatrix[2, 2] * z);
            return m_6d(nx, ny, nz, w, u, v, m_seed, m_interp);
        }

        void setNoRotation()
        {
            m_rotatedomain = false;
            m_rotmatrix[0, 0] = 1;
            m_rotmatrix[1, 0] = 0;
            m_rotmatrix[2, 0] = 0;

            m_rotmatrix[0, 1] = 0;
            m_rotmatrix[1, 1] = 1;
            m_rotmatrix[2, 1] = 0;

            m_rotmatrix[0, 2] = 0;
            m_rotmatrix[1, 2] = 0;
            m_rotmatrix[2, 2] = 1;

            cos2d = 1;
            sin2d = 0;
        }

        void setRotationAngle(double x, double y, double z, double angle)
        {
            m_rotatedomain = true;
            m_rotmatrix[0, 0] = (1 + (1 - Math.Cos(angle)) * (x * x - 1));
            m_rotmatrix[1, 0] = (-z * Math.Sin(angle) + (1 - Math.Cos(angle)) * x * y);
            m_rotmatrix[2, 0] = (y * Math.Sin(angle) + (1 - Math.Cos(angle)) * x * z);

            m_rotmatrix[0, 1] = (z * Math.Sin(angle) + (1 - Math.Cos(angle)) * x * y);
            m_rotmatrix[1, 1] = (1 + (1 - Math.Cos(angle)) * (y * y - 1));
            m_rotmatrix[2, 1] = (-x * Math.Sin(angle) + (1 - Math.Cos(angle)) * y * z);

            m_rotmatrix[0, 2] = (-y * Math.Sin(angle) + (1 - Math.Cos(angle)) * x * z);
            m_rotmatrix[1, 2] = (x * Math.Sin(angle) + (1 - Math.Cos(angle)) * y * z);
            m_rotmatrix[2, 2] = (1 + (1 - Math.Cos(angle)) * (z * z - 1));

            cos2d = Math.Cos(angle);
            sin2d = Math.Sin(angle);
        }

        private void setMagicNumbers(EBasisTypes type)
        {
            // This function is a damned hack.
            // The underlying noise functions don't return values in the range [-1,1] cleanly, and the ranges vary depending
            // on basis type and dimensionality. There's probably a better way to correct the ranges, but for now I'm just
            // setting the magic numbers m_scale and m_offset manually to empirically determined magic numbers.
            switch (type)
            {
                case EBasisTypes.VALUE:
                    {
                        m_scale[0] = 1.0f; m_offset[0] = 0.0f;
                        m_scale[1] = 1.0f; m_offset[1] = 0.0f;
                        m_scale[2] = 1.0f; m_offset[2] = 0.0f;
                        m_scale[3] = 1.0f; m_offset[3] = 0.0f;
                    }
                    break;

                case EBasisTypes.GRADIENT:
                    {
                        m_scale[0] = 1.86848f; m_offset[0] = -0.000118f;
                        m_scale[1] = 1.85148f; m_offset[1] = -0.008272f;
                        m_scale[2] = 1.64127f; m_offset[2] = -0.01527f;
                        m_scale[3] = 1.92517f; m_offset[3] = 0.03393f;
                    }
                    break;

                case EBasisTypes.GRADVAL:
                    {
                        m_scale[0] = 0.6769f; m_offset[0] = -0.00151f;
                        m_scale[1] = 0.6957f; m_offset[1] = -0.133f;
                        m_scale[2] = 0.74622f; m_offset[2] = 0.01916f;
                        m_scale[3] = 0.7961f; m_offset[3] = -0.0352f;
                    }
                    break;

                case EBasisTypes.WHITE:
                    {
                        m_scale[0] = 1.0f; m_offset[0] = 0.0f;
                        m_scale[1] = 1.0f; m_offset[1] = 0.0f;
                        m_scale[2] = 1.0f; m_offset[2] = 0.0f;
                        m_scale[3] = 1.0f; m_offset[3] = 0.0f;
                    }
                    break;

                default:
                    {
                        m_scale[0] = 1.0f; m_offset[0] = 0.0f;
                        m_scale[1] = 1.0f; m_offset[1] = 0.0f;
                        m_scale[2] = 1.0f; m_offset[2] = 0.0f;
                        m_scale[3] = 1.0f; m_offset[3] = 0.0f;
                    }
                    break;
            }
        }
    }
}
