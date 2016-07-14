using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    // Gradient has the effect of allowing you to specify a line segment in N-dimensional space by
    // setting endpoints.The endpoints are set via setGradient() and the segment extends from
    // P1 = (x1, y1, z1, w1, u1, v1) to P2=(x2,y2,z2,w2,u2,v2). Only the coordinates pertinent to
    // the dimensionality of the function being called are used, however.
    // A gradient field is aligned with this segment such that any input coordinate is projected
    // onto the line formed by the segment, and a value is assigned based on where on the line the
    // projected point lies.If it lies on or beyond P1, the value is assigned as 0. If it lies on or
    // beyond P2 the value is assigned as 1. Anything in between is assigned as a linear interpolation
    // between 0 and 1. (This behavior may be changing; I may remove the clamping aspect, and allow
    // values to go infinitely depending on distance from the "origin" point.Some useful things could
    // be done in this fashion.) The result is a smooth gradient field oriented along an axis.

    public class CImplicitGradient : CImplicitModuleBase
    {
        private double m_gx1, m_gy1, m_gz1, m_gw1, m_gu1, m_gv1;
        private double m_gx2, m_gy2, m_gz2, m_gw2, m_gu2, m_gv2;
        private double m_x, m_y, m_z, m_w, m_u, m_v;
        private double m_vlen;

        public CImplicitGradient () : base ()
        {
            setGradient (0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        public CImplicitGradient (double x1, double x2, double y1, double y2, double z1=0, double z2=0,
            double w1=0, double w2=0, double u1=0, double u2=0, double v1=0, double v2=0)
        {
            setGradient (x1, x2, y1, y2, z1, z2, w1, w2, u1, u2, v1, v2);
        }

        public void setGradient (double x1, double x2, double y1, double y2, double z1 = 0, double z2 = 0,
            double w1 = 0, double w2 = 0, double u1 = 0, double u2 = 0, double v1 = 0, double v2 = 0)
        {
            m_gx1 = x1;
            m_gx2 = x2;
            m_gy1 = y1;
            m_gy2 = y2;
            m_gz1 = z1;
            m_gz2 = z2;
            m_gw1 = w1;
            m_gw2 = w2;
            m_gu1 = u1;
            m_gu2 = u2;
            m_gv1 = v1;
            m_gv2 = v2;

            m_x = x2 - x1;
            m_y = y2 - y1;
            m_z = z2 - z1;
            m_w = w2 - w1;
            m_u = u2 - u1;
            m_v = v2 - v1;

            m_vlen = (m_x * m_x + m_y * m_y + m_z * m_z + m_w * m_w + m_u * m_u + m_v * m_v);
        }

        public override double get (double x, double y)
        {
            // Subtract from (1) and take dotprod
            double dx = x - m_gx1;
            double dy = y - m_gy1;
            double dp = dx * m_x + dy * m_y;
            dp /= m_vlen;
            //dp=clamp(dp/m_vlen,0.0,1.0);
            //return lerp(dp,1.0,-1.0);
            return dp;
        }

        public override double get (double x, double y, double z)
        {
            double dx = x - m_gx1;
            double dy = y - m_gy1;
            double dz = z - m_gz1;
            double dp = dx * m_x + dy * m_y + dz * m_z;
            dp /= m_vlen;
            //dp=clamp(dp/m_vlen,0.0,1.0);
            //return lerp(dp,1.0,-1.0);
            return dp;
        }

        public override double get (double x, double y, double z, double w)
        {
            double dx = x - m_gx1;
            double dy = y - m_gy1;
            double dz = z - m_gz1;
            double dw = w - m_gw1;
            double dp = dx * m_x + dy * m_y + dz * m_z + dw * m_w;
            dp /= m_vlen;
            //dp=clamp(dp/m_vlen,0.0,1.0);
            //return lerp(dp,1.0,-1.0);
            return dp;
        }

        public override double get (double x, double y, double z, double w, double u, double v)
        {
            double dx = x - m_gx1;
            double dy = y - m_gy1;
            double dz = z - m_gz1;
            double dw = w - m_gw1;
            double du = u - m_gu1;
            double dv = v - m_gv1;
            double dp = dx * m_x + dy * m_y + dz * m_z + dw * m_w + du * m_u + dv * m_v;
            dp /= m_vlen;
            //dp=clamp(dp/m_vlen,0.0,1.0);
            //return lerp(clamp(dp,0.0,1.0),1.0,-1.0);
            return dp;
        }
    }
}
