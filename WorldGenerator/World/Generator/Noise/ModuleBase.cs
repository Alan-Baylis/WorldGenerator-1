using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    // Base class of implicit (2D, 4D, 6D) noise functions

    // Noise values are obtained by calling one of the get() methods provided, 
    // with the appropriate number of coordinates. Note that the performance of
    // the system as a hold is affected by the dimensionality of the function called, 
    // so a 6D function will take significantly longer than a 2D function. 
    // Typical applications will probably stick with 2D or 3D versions; the higher
    // orders are provided for the purpose of seamless mapping.

    public abstract class CImplicitModuleBase
    {
        private double m_spacing = 0.0001f; // DerivSpacing

        public virtual void setSeed (uint seed)
        {
        }

        public abstract double get (double x, double y);
        public abstract double get (double x, double y, double z);
        public abstract double get (double x, double y, double z, double w);
        public abstract double get (double x, double y, double z, double w, double u, double v);

        public double get_dx (double x, double y)
        {
            return (get (x - m_spacing, y) - get (x + m_spacing, y)) / m_spacing;
        }

        public double get_dy (double x, double y)
        {
            return (get (x, y - m_spacing) - get (x, y + m_spacing)) / m_spacing;
        }

        public double get_dx (double x, double y, double z)
        {
            return (get (x - m_spacing, y, z) - get (x + m_spacing, y, z)) / m_spacing;
        }

        public double get_dy (double x, double y, double z)
        {
            return (get (x, y - m_spacing, z) - get (x, y + m_spacing, z)) / m_spacing;
        }

        public double get_dz (double x, double y, double z)
        {
            return (get (x, y, z - m_spacing) - get (x, y, z + m_spacing)) / m_spacing;
        }

        public double get_dx (double x, double y, double z, double w)
        {
            return (get (x - m_spacing, y, z, w) - get (x + m_spacing, y, z, w)) / m_spacing;
        }

        public double get_dy (double x, double y, double z, double w)
        {
            return (get (x, y - m_spacing, z, w) - get (x, y + m_spacing, z, w)) / m_spacing;
        }

        public double get_dz (double x, double y, double z, double w)
        {
            return (get (x, y, z - m_spacing, w) - get (x, y, z + m_spacing, w)) / m_spacing;
        }

        public double get_dw (double x, double y, double z, double w)
        {
            return (get (x, y, z, w - m_spacing) - get (x, y, z, w + m_spacing)) / m_spacing;
        }

        public double get_dx (double x, double y, double z, double w, double u, double v)
        {
            return (get (x - m_spacing, y, z, w, u, v) - get (x + m_spacing, y, z, w, u, v)) / m_spacing;
        }

        public double get_dy (double x, double y, double z, double w, double u, double v)
        {
            return (get (x, y - m_spacing, z, w, u, v) - get (x, y + m_spacing, z, w, u, v)) / m_spacing;
        }

        public double get_dz (double x, double y, double z, double w, double u, double v)
        {
            return (get (x, y, z - m_spacing, w, u, v) - get (x, y, z + m_spacing, w, u, v)) / m_spacing;
        }

        public double get_dw (double x, double y, double z, double w, double u, double v)
        {
            return (get (x, y, z, w - m_spacing, u, v) - get (x, y, z, w + m_spacing, u, v)) / m_spacing;
        }

        public double get_du (double x, double y, double z, double w, double u, double v)
        {
            return (get (x, y, z, w, u - m_spacing, v) - get (x, y, z, w, u + m_spacing, v)) / m_spacing;
        }

        public double get_dv (double x, double y, double z, double w, double u, double v)
        {
            return (get (x, y, z, w, u, v - m_spacing) - get (x, y, z, w, u, v + m_spacing)) / m_spacing;
        }
    }
}
