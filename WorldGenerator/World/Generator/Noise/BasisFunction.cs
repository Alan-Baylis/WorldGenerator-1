using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    public class CImplicitBasisFunction : CImplicitModuleBase
    {

        enum EBasisTypes
        {
            VALUE,
            GRADIENT,
            GRADVAL,
            SIMPLEX,
            WHITE
        };

        enum EInterpTypes
        {
            NONE,
            LINEAR,
            CUBIC,
            QUINTIC
        };


        public delegate float interp_func (float x);
        public delegate float noise_func2(float x, float y, uint c, interp_func f);
        public delegate float noise_func3(float x,float y,float z ,uint c, interp_func f);
        public delegate float noise_func4(float x,float y,float z,float u,uint c, interp_func f);
        public delegate float noise_func6(float x,float y,float z,float u,float v,float w,uint c,interp_func f);

        private float [] m_scale, m_offset;
        private interp_func m_interp;
        private noise_func2 m_2d;
        private noise_func3 m_3d;
        private noise_func4 m_4d;
        private noise_func6 m_6d;
        private uint m_seed;

        private float [,] m_rotmatrix;
        private float cos2d, sin2d;
        private bool m_rotatedomain;


        CImplicitBasisFunction () : base ()
        {
            m_rotmatrix = new float [3, 3];
            m_scale = new float [4];
             m_offset = new float [4];

            m_rotatedomain = true;
            setType (EBasisTypes.GRADIENT);
            setInterp (EInterpTypes.QUINTIC);
            setSeed (1000);

        }
        CImplicitBasisFunction (EBasisTypes type, EInterpTypes interp, bool rotatedomain) : base ()
        {
            m_rotmatrix = new float [3, 3];
            m_scale = new float [4];
                     m_offset = new float [4];

            m_rotatedomain = rotatedomain;
            setType (type);
            setInterp (interp);
            setSeed (1000);
        }

        void setSeed (uint seed)
        {
            m_seed = seed;
            LCG lcg;
            lcg.setSeed (seed);

            if (m_rotatedomain) {
                float ax, ay, az;
                float len;

                ax = lcg.get01 ();
                ay = lcg.get01 ();
                az = lcg.get01 ();
                len = (float)Math.Sqrt (ax * ax + ay * ay + az * az);
                ax /= len;
                ay /= len;
                az /= len;
                setRotationAngle (ax, ay, az, lcg.get01 () * 3.141592 * 2.0);
                float angle = lcg.get01 () * 3.14159265 * 2.0;
                cos2d = (float)Math.Cos (angle);
                sin2d = (float)Math.Sin (angle);
            } else {
                setNoRotation ();
            }
        }

        void setType (EBasisTypes type)
        {
            switch (type) {
            case EBasisTypes.VALUE: m_2d = value_noise2D; m_3d = value_noise3D; m_4d = value_noise4D; m_6d = value_noise6D; break;
            case EBasisTypes.GRADIENT: m_2d = gradient_noise2D; m_3d = gradient_noise3D; m_4d = gradient_noise4D; m_6d = gradient_noise6D; break;
            case EBasisTypes.GRADVAL: m_2d = gradval_noise2D; m_3d = gradval_noise3D; m_4d = gradval_noise4D; m_6d = gradval_noise6D; break;
            case EBasisTypes.WHITE: m_2d = white_noise2D; m_3d = white_noise3D; m_4d = white_noise4D; m_6d = white_noise6D; break;
            case EBasisTypes.SIMPLEX: m_2d = simplex_noise2D; m_3d = simplex_noise3D; m_4d = simplex_noise4D; m_6d = simplex_noise6D; break;
            default: m_2d = gradient_noise2D; m_3d = gradient_noise3D; m_4d = gradient_noise4D; m_6d = gradient_noise6D; break;
            }
            setMagicNumbers (type);
        }

        void setInterp (EInterpTypes interp)
        {
            switch (interp) {
            case EInterpTypes.NONE: m_interp = noInterp; break;
            case EInterpTypes.LINEAR: m_interp = linearInterp; break;
            case EInterpTypes.CUBIC: m_interp = hermiteInterp; break;
            default: m_interp = quinticInterp; break;
            }
        }

        public override float get (float x, float y)
        {
            float nx, ny;
            nx = x * cos2d - y * sin2d;
            ny = y * cos2d + x * sin2d;
            return m_2d (nx, ny, m_seed, m_interp);
        }
        public override float get (float x, float y, float z)
        {
            float nx, ny, nz;
            nx = (m_rotmatrix [0, 0] * x) + (m_rotmatrix [1, 0] * y) + (m_rotmatrix [2, 0] * z);
            ny = (m_rotmatrix [0, 1] * x) + (m_rotmatrix [1, 1] * y) + (m_rotmatrix [2, 1] * z);
            nz = (m_rotmatrix [0, 2] * x) + (m_rotmatrix [1, 2] * y) + (m_rotmatrix [2, 2] * z);
            return m_3d (nx, ny, nz, m_seed, m_interp);
        }
        public override float get (float x, float y, float z, float w)
        {
            float nx, ny, nz;
            nx = (m_rotmatrix [0, 0] * x) + (m_rotmatrix [1, 0] * y) + (m_rotmatrix [2, 0] * z);
            ny = (m_rotmatrix [0, 1] * x) + (m_rotmatrix [1, 1] * y) + (m_rotmatrix [2, 1] * z);
            nz = (m_rotmatrix [0, 2] * x) + (m_rotmatrix [1, 2] * y) + (m_rotmatrix [2, 2] * z);
            return m_4d (nx, ny, nz, w, m_seed, m_interp);
        }
        public override float get (float x, float y, float z, float w, float u, float v)
        {
            float nx, ny, nz;
            nx = (m_rotmatrix [0, 0] * x) + (m_rotmatrix [1, 0] * y) + (m_rotmatrix [2, 0] * z);
            ny = (m_rotmatrix [0, 1] * x) + (m_rotmatrix [1, 1] * y) + (m_rotmatrix [2, 1] * z);
            nz = (m_rotmatrix [0, 2] * x) + (m_rotmatrix [1, 2] * y) + (m_rotmatrix [2, 2] * z);
            return m_6d (nx, ny, nz, w, u, v, m_seed, m_interp);
        }

        void setNoRotation ()
        {
            m_rotatedomain = false;
            m_rotmatrix [0, 0] = 1;
            m_rotmatrix [1, 0] = 0;
            m_rotmatrix [2, 0] = 0;

            m_rotmatrix [0, 1] = 0;
            m_rotmatrix [1, 1] = 1;
            m_rotmatrix [2, 1] = 0;

            m_rotmatrix [0, 2] = 0;
            m_rotmatrix [1, 2] = 0;
            m_rotmatrix [2, 2] = 1;

            cos2d = 1;
            sin2d = 0;
        }

        void setRotationAngle (float x, float y, float z, float angle)
        {
            m_rotatedomain = true;
            m_rotmatrix [0, 0] = (float)(1 + (1 - Math.Cos (angle)) * (x * x - 1));
            m_rotmatrix [1, 0] = (float)(-z * Math.Sin (angle) + (1 - Math.Cos (angle)) * x * y);
            m_rotmatrix [2, 0] = (float)(y * Math.Sin (angle) + (1 - Math.Cos (angle)) * x * z);

            m_rotmatrix [0, 1] = (float)(z * Math.Sin (angle) + (1 - Math.Cos (angle)) * x * y);
            m_rotmatrix [1, 1] = (float)(1 + (1 - Math.Cos (angle)) * (y * y - 1));
            m_rotmatrix [2, 1] = (float)(-x * Math.Sin (angle) + (1 - Math.Cos (angle)) * y * z);

            m_rotmatrix [0, 2] = (float)(-y * Math.Sin (angle) + (1 - Math.Cos (angle)) * x * z);
            m_rotmatrix [1, 2] = (float)(x * Math.Sin (angle) + (1 - Math.Cos (angle)) * y * z);
            m_rotmatrix [2, 2] = (float)(1 + (1 - Math.Cos (angle)) * (z * z - 1));

            cos2d = (float)Math.Cos (angle);
            sin2d = (float)Math.Sin (angle);
        }

        private void setMagicNumbers (EBasisTypes type)
        {
            // This function is a damned hack.
            // The underlying noise functions don't return values in the range [-1,1] cleanly, and the ranges vary depending
            // on basis type and dimensionality. There's probably a better way to correct the ranges, but for now I'm just
            // setting the magic numbers m_scale and m_offset manually to empirically determined magic numbers.
            switch (type) {
            case EBasisTypes.VALUE: {
                    m_scale [0] = 1.0f; m_offset [0] = 0.0f;
                    m_scale [1] = 1.0f; m_offset [1] = 0.0f;
                    m_scale [2] = 1.0f; m_offset [2] = 0.0f;
                    m_scale [3] = 1.0f; m_offset [3] = 0.0f;
                }
                break;

            case EBasisTypes.GRADIENT: {
                    m_scale [0] = 1.86848f; m_offset [0] = -0.000118f;
                    m_scale [1] = 1.85148f; m_offset [1] = -0.008272f;
                    m_scale [2] = 1.64127f; m_offset [2] = -0.01527f;
                    m_scale [3] = 1.92517f; m_offset [3] = 0.03393f;
                }
                break;

            case EBasisTypes.GRADVAL: {
                    m_scale [0] = 0.6769f; m_offset [0] = -0.00151f;
                    m_scale [1] = 0.6957f; m_offset [1] = -0.133f;
                    m_scale [2] = 0.74622f; m_offset [2] = 0.01916f;
                    m_scale [3] = 0.7961f; m_offset [3] = -0.0352f;
                }
                break;

            case EBasisTypes.WHITE: {
                    m_scale [0] = 1.0f; m_offset [0] = 0.0f;
                    m_scale [1] = 1.0f; m_offset [1] = 0.0f;
                    m_scale [2] = 1.0f; m_offset [2] = 0.0f;
                    m_scale [3] = 1.0f; m_offset [3] = 0.0f;
                }
                break;

            default: {
                    m_scale [0] = 1.0f; m_offset [0] = 0.0f;
                    m_scale [1] = 1.0f; m_offset [1] = 0.0f;
                    m_scale [2] = 1.0f; m_offset [2] = 0.0f;
                    m_scale [3] = 1.0f; m_offset [3] = 0.0f;
                }
                break;
            };
        }
    }
