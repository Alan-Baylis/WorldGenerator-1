#ifndef IMPLICIT_ROTATEDOMAIN_H
#define IMPLICIT_ROTATEDOMAIN_H
#include "implicitmodulebase.h"

namespace anl
{
    /*
Given angle r in radians and unit vector u = ai + bj + ck or [a,b,c]', define:

q0 = cos(r/2),  q1 = sin(r/2) a,  q2 = sin(r/2) b,  q3 = sin(r/2) c

and construct from these values the rotation matrix:

     (q0² + q1² - q2² - q3²)      2(q1q2 - q0q3)          2(q1q3 + q0q2)

Q  =      2(q2q1 + q0q3)     (q0² - q1² + q2² - q3²)      2(q2q3 - q0q1)

          2(q3q1 - q0q2)          2(q3q2 + q0q1)     (q0² - q1² - q2² + q3²)

Multiplication by Q then effects the desired rotation, and in particular:

Q u = u
*/

class CImplicitRotateDomain : public CImplicitModuleBase
{
    public:
    CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, ANLFloatType ay, ANLFloatType az, ANLFloatType angle_deg);
    CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, ANLFloatType ay, ANLFloatType az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, ANLFloatType ay, CImplicitModuleBase * az, ANLFloatType angle_deg);
    CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, ANLFloatType ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, CImplicitModuleBase * ay, ANLFloatType az, ANLFloatType angle_deg);
    CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, CImplicitModuleBase * ay, ANLFloatType az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, ANLFloatType angle_deg);
    CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, ANLFloatType ay, ANLFloatType az, ANLFloatType angle_deg);
    CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, ANLFloatType ay, ANLFloatType az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, ANLFloatType ay, CImplicitModuleBase * az, ANLFloatType angle_deg);
    CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, ANLFloatType ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, ANLFloatType az, ANLFloatType angle_deg);
    CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, ANLFloatType az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, ANLFloatType angle_deg);
    CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, ANLFloatType ay, ANLFloatType az, ANLFloatType angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, ANLFloatType ay, ANLFloatType az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, ANLFloatType ay, CImplicitModuleBase * az, ANLFloatType angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, ANLFloatType ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, CImplicitModuleBase * ay, ANLFloatType az, ANLFloatType angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, CImplicitModuleBase * ay, ANLFloatType az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, ANLFloatType angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, ANLFloatType ay, ANLFloatType az, ANLFloatType angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, ANLFloatType ay, ANLFloatType az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, ANLFloatType ay, CImplicitModuleBase * az, ANLFloatType angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, ANLFloatType ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, ANLFloatType az, ANLFloatType angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, ANLFloatType az, CImplicitModuleBase * angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, ANLFloatType angle_deg);
    CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg);

    ~CImplicitRotateDomain();
    void setSource(CImplicitModuleBase * m);
    void setSource(ANLFloatType v);
    void setAxis(ANLFloatType ax, ANLFloatType ay, ANLFloatType az);
    void setAxis(CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az);
    void setAxisX(ANLFloatType ax);
    void setAxisY(ANLFloatType ay);
    void setAxisZ(ANLFloatType az);
    void setAxisX(CImplicitModuleBase * ax);
    void setAxisY(CImplicitModuleBase * ay);
    void setAxisZ(CImplicitModuleBase * az);

    void setAngle(ANLFloatType a);
    void setAngle(CImplicitModuleBase * a);

    ANLFloatType get(ANLFloatType x, ANLFloatType y);
    ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z);
    ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
    ANLFloatType get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);

    protected:
    ANLFloatType m_rotmatrix[3][3];
    CScalarParameter m_ax,m_ay,m_az, m_angledeg;
    CScalarParameter m_source;

    void calculateRotMatrix(ANLFloatType x, ANLFloatType y);
    void calculateRotMatrix(ANLFloatType x, ANLFloatType y, ANLFloatType z);
    void calculateRotMatrix(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w);
    void calculateRotMatrix(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v);
};
};

#endif
#include "implicitrotatedomain.h"
#include <cmath>

namespace anl
{

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, ANLFloatType ay, ANLFloatType az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, ANLFloatType ay, ANLFloatType az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, ANLFloatType ay, CImplicitModuleBase * az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, ANLFloatType ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, CImplicitModuleBase * ay, ANLFloatType az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, CImplicitModuleBase * ay, ANLFloatType az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, ANLFloatType ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, ANLFloatType ay, ANLFloatType az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, ANLFloatType ay, ANLFloatType az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, ANLFloatType ay, CImplicitModuleBase * az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, ANLFloatType ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, ANLFloatType az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, ANLFloatType az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(ANLFloatType src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, ANLFloatType ay, ANLFloatType az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, ANLFloatType ay, ANLFloatType az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, ANLFloatType ay, CImplicitModuleBase * az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, ANLFloatType ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, CImplicitModuleBase * ay, ANLFloatType az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, CImplicitModuleBase * ay, ANLFloatType az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, ANLFloatType ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, ANLFloatType ay, ANLFloatType az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, ANLFloatType ay, ANLFloatType az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, ANLFloatType ay, CImplicitModuleBase * az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, ANLFloatType ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, ANLFloatType az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, ANLFloatType az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, ANLFloatType angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }

    CImplicitRotateDomain::CImplicitRotateDomain(CImplicitModuleBase * src, CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az, CImplicitModuleBase * angle_deg) :
        CImplicitModuleBase(), m_ax(ax), m_ay(ay), m_az(az), m_angledeg(angle_deg), m_source(src)
    {
    }


    CImplicitRotateDomain::~CImplicitRotateDomain(){}

    void CImplicitRotateDomain::setSource(CImplicitModuleBase * m){m_source.set(m);}
    void CImplicitRotateDomain::setSource(ANLFloatType v){m_source.set(v);}

    void CImplicitRotateDomain::calculateRotMatrix(ANLFloatType x, ANLFloatType y)
    {
        ANLFloatType angle=m_angledeg.get(x,y)*360.0 * 3.14159265/180.0;
        ANLFloatType ax=m_ax.get(x,y);
        ANLFloatType ay=m_ay.get(x,y);
        ANLFloatType az=m_az.get(x,y);

        ANLFloatType cosangle=cos(angle);
        ANLFloatType sinangle=sin(angle);

        m_rotmatrix[0][0] = 1.0 + (1.0-cosangle)*(ax*ax-1.0);
        m_rotmatrix[1][0] = -az*sinangle+(1.0-cosangle)*ax*ay;
        m_rotmatrix[2][0] = ay*sinangle+(1.0-cosangle)*ax*az;

        m_rotmatrix[0][1] = az*sinangle+(1.0-cosangle)*ax*ay;
        m_rotmatrix[1][1] = 1.0 + (1.0-cosangle)*(ay*ay-1.0);
        m_rotmatrix[2][1] = -ax*sinangle+(1.0-cosangle)*ay*az;

        m_rotmatrix[0][2] = -ay*sinangle+(1.0-cosangle)*ax*az;
        m_rotmatrix[1][2] = ax*sinangle+(1.0-cosangle)*ay*az;
        m_rotmatrix[2][2] = 1.0 + (1.0-cosangle)*(az*az-1.0);
    }

    void CImplicitRotateDomain::calculateRotMatrix(ANLFloatType x, ANLFloatType y, ANLFloatType z)
    {
        ANLFloatType angle=m_angledeg.get(x,y,z)*360.0 * 3.14159265/180.0;
        ANLFloatType ax=m_ax.get(x,y,z);
        ANLFloatType ay=m_ay.get(x,y,z);
        ANLFloatType az=m_az.get(x,y,z);

        ANLFloatType cosangle=cos(angle);
        ANLFloatType sinangle=sin(angle);

        m_rotmatrix[0][0] = 1.0 + (1.0-cosangle)*(ax*ax-1.0);
        m_rotmatrix[1][0] = -az*sinangle+(1.0-cosangle)*ax*ay;
        m_rotmatrix[2][0] = ay*sinangle+(1.0-cosangle)*ax*az;

        m_rotmatrix[0][1] = az*sinangle+(1.0-cosangle)*ax*ay;
        m_rotmatrix[1][1] = 1.0 + (1.0-cosangle)*(ay*ay-1.0);
        m_rotmatrix[2][1] = -ax*sinangle+(1.0-cosangle)*ay*az;

        m_rotmatrix[0][2] = -ay*sinangle+(1.0-cosangle)*ax*az;
        m_rotmatrix[1][2] = ax*sinangle+(1.0-cosangle)*ay*az;
        m_rotmatrix[2][2] = 1.0 + (1.0-cosangle)*(az*az-1.0);
    }
    void CImplicitRotateDomain::calculateRotMatrix(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
    {
        ANLFloatType angle=m_angledeg.get(x,y,z,w)*360.0 * 3.14159265/180.0;
        ANLFloatType ax=m_ax.get(x,y,z,w);
        ANLFloatType ay=m_ay.get(x,y,z,w);
        ANLFloatType az=m_az.get(x,y,z,w);

        ANLFloatType cosangle=cos(angle);
        ANLFloatType sinangle=sin(angle);

        m_rotmatrix[0][0] = 1.0 + (1.0-cosangle)*(ax*ax-1.0);
        m_rotmatrix[1][0] = -az*sinangle+(1.0-cosangle)*ax*ay;
        m_rotmatrix[2][0] = ay*sinangle+(1.0-cosangle)*ax*az;

        m_rotmatrix[0][1] = az*sinangle+(1.0-cosangle)*ax*ay;
        m_rotmatrix[1][1] = 1.0 + (1.0-cosangle)*(ay*ay-1.0);
        m_rotmatrix[2][1] = -ax*sinangle+(1.0-cosangle)*ay*az;

        m_rotmatrix[0][2] = -ay*sinangle+(1.0-cosangle)*ax*az;
        m_rotmatrix[1][2] = ax*sinangle+(1.0-cosangle)*ay*az;
        m_rotmatrix[2][2] = 1.0 + (1.0-cosangle)*(az*az-1.0);
    }
    void CImplicitRotateDomain::calculateRotMatrix(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
    {
        ANLFloatType angle=m_angledeg.get(x,y,z,w,u,v)*360.0 * 3.14159265/180.0;
        ANLFloatType ax=m_ax.get(x,y,z,w,u,v);
        ANLFloatType ay=m_ay.get(x,y,z,w,u,v);
        ANLFloatType az=m_az.get(x,y,z,w,u,v);

        ANLFloatType cosangle=cos(angle);
        ANLFloatType sinangle=sin(angle);

        m_rotmatrix[0][0] = 1.0 + (1.0-cosangle)*(ax*ax-1.0);
        m_rotmatrix[1][0] = -az*sinangle+(1.0-cosangle)*ax*ay;
        m_rotmatrix[2][0] = ay*sinangle+(1.0-cosangle)*ax*az;

        m_rotmatrix[0][1] = az*sinangle+(1.0-cosangle)*ax*ay;
        m_rotmatrix[1][1] = 1.0 + (1.0-cosangle)*(ay*ay-1.0);
        m_rotmatrix[2][1] = -ax*sinangle+(1.0-cosangle)*ay*az;

        m_rotmatrix[0][2] = -ay*sinangle+(1.0-cosangle)*ax*az;
        m_rotmatrix[1][2] = ax*sinangle+(1.0-cosangle)*ay*az;
        m_rotmatrix[2][2] = 1.0 + (1.0-cosangle)*(az*az-1.0);
    }
    void CImplicitRotateDomain::setAxis(ANLFloatType ax, ANLFloatType ay, ANLFloatType az)
    {
        m_ax.set(ax);
        m_ay.set(ay);
        m_az.set(az);
    }

    void CImplicitRotateDomain::setAxis(CImplicitModuleBase * ax, CImplicitModuleBase * ay, CImplicitModuleBase * az)
    {
        m_ax.set(ax);
        m_ay.set(ay);
        m_az.set(az);
    }
    void CImplicitRotateDomain::setAxisX(ANLFloatType ax)
    {
        m_ax.set(ax);
    }
    void CImplicitRotateDomain::setAxisY(ANLFloatType ay)
    {
        m_ay.set(ay);
    }
    void CImplicitRotateDomain::setAxisZ(ANLFloatType az)
    {
        m_az.set(az);
    }
    void CImplicitRotateDomain::setAxisX(CImplicitModuleBase * ax)
    {
        m_ax.set(ax);
    }
    void CImplicitRotateDomain::setAxisY(CImplicitModuleBase * ay)
    {
        m_ay.set(ay);
    }
    void CImplicitRotateDomain::setAxisZ(CImplicitModuleBase * az)
    {
        m_az.set(az);
    }

    void CImplicitRotateDomain::setAngle(ANLFloatType a)
    {
        m_angledeg.set(a);
    }
    void CImplicitRotateDomain::setAngle(CImplicitModuleBase * a)
    {
        m_angledeg.set(a);
    }

    ANLFloatType CImplicitRotateDomain::get(ANLFloatType x, ANLFloatType y)
    {
        ANLFloatType nx,ny;
        ANLFloatType angle=m_angledeg.get(x,y)*360.0 * 3.14159265/180.0;
        ANLFloatType cos2d=cos(angle);
        ANLFloatType sin2d=sin(angle);
        nx = x*cos2d-y*sin2d;
        ny = y*cos2d+x*sin2d;
        return m_source.get(nx,ny);
    }

    ANLFloatType CImplicitRotateDomain::get(ANLFloatType x, ANLFloatType y, ANLFloatType z)
    {
        calculateRotMatrix(x,y,z);
        ANLFloatType nx, ny, nz;
        nx = (m_rotmatrix[0][0]*x) + (m_rotmatrix[1][0]*y) + (m_rotmatrix[2][0]*z);
        ny = (m_rotmatrix[0][1]*x) + (m_rotmatrix[1][1]*y) + (m_rotmatrix[2][1]*z);
        nz = (m_rotmatrix[0][2]*x) + (m_rotmatrix[1][2]*y) + (m_rotmatrix[2][2]*z);
        return m_source.get(nx,ny,nz);
    }
    ANLFloatType CImplicitRotateDomain::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w)
    {

        calculateRotMatrix(x,y,z,w);
        ANLFloatType nx, ny, nz;
        nx = (m_rotmatrix[0][0]*x) + (m_rotmatrix[1][0]*y) + (m_rotmatrix[2][0]*z);
        ny = (m_rotmatrix[0][1]*x) + (m_rotmatrix[1][1]*y) + (m_rotmatrix[2][1]*z);
        nz = (m_rotmatrix[0][2]*x) + (m_rotmatrix[1][2]*y) + (m_rotmatrix[2][2]*z);
        return m_source.get(nx,ny,nz,w);
    }
    ANLFloatType CImplicitRotateDomain::get(ANLFloatType x, ANLFloatType y, ANLFloatType z, ANLFloatType w, ANLFloatType u, ANLFloatType v)
    {

        calculateRotMatrix(x,y,z,w,u,v);
        ANLFloatType nx, ny, nz;
        nx = (m_rotmatrix[0][0]*x) + (m_rotmatrix[1][0]*y) + (m_rotmatrix[2][0]*z);
        ny = (m_rotmatrix[0][1]*x) + (m_rotmatrix[1][1]*y) + (m_rotmatrix[2][1]*z);
        nz = (m_rotmatrix[0][2]*x) + (m_rotmatrix[1][2]*y) + (m_rotmatrix[2][2]*z);
        return m_source.get(nx,ny,nz,w,u,v);
    }
};
