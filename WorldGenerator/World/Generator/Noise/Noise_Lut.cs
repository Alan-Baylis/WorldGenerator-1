// Lookup tables for 2D, 4D and 6D gradient oise.
// Generated with boost::random, using a lagged Fibonacci generator and a uniform_on_sphere distribution.
#include "../types.h"

extern double gradient2D_lut[256][2];
extern double gradient3D_lut[256][3];
extern double gradient4D_lut[256][4];
extern double gradient6D_lut[256][6];
extern double whitenoise_lut[256];
// Lookup tables for 2D, 3D, 4D and 6D gradient and simplex noise.

#include "../types.h"


double gradient2D_lut[4][2]=
{
	{0,1},
	{0,-1},
	{1,0},
	{-1,0}
};

double gradient3D_lut[12][3]=
{
	{0,0.70710678,0.70710678},
	{0,-0.70710678,0.70710678},
	{0,0.70710678,-0.70710678},
	{0,-0.70710678,-0.70710678},
	{0.70710678,0,0.70710678},
	{-0.70710678,0,0.70710678},
	{0.70710678,0,-0.70710678},
	{-0.70710678,0,-0.70710678},
	{0.70710678,0.70710678,0},
	{-0.70710678,0.70710678,0},
	{0.70710678,-0.70710678,0},
	{-0.70710678,-0.70710678,0}
};

double gradient4D_lut[32][4]=
{
	{0,0.5773503,0.5773503,0.5773503},
	{0,-0.5773503,0.5773503,0.5773503},
	{0,0.5773503,-0.5773503,0.5773503},
	{0,0.5773503,0.5773503,-0.5773503},
	{0,-0.5773503,-0.5773503,0.5773503},
	{0,-0.5773503,0.5773503,-0.5773503},
	{0,0.5773503,-0.5773503,-0.5773503},
	{0,-0.5773503,-0.5773503,-0.5773503},
	
	{0.5773503,0,0.5773503,0.5773503},
	{-0.5773503,0,0.5773503,0.5773503},
	{0.5773503,0,-0.5773503,0.5773503},
	{0.5773503,0,0.5773503,-0.5773503},
	{-0.5773503,0,-0.5773503,0.5773503},
	{-0.5773503,0,0.5773503,-0.5773503},
	{0.5773503,0,-0.5773503,-0.5773503},
	{-0.5773503,0,-0.5773503,-0.5773503},
	
	{0.5773503,0.5773503,0,0.5773503},
	{-0.5773503,0.5773503,0,0.5773503},
	{0.5773503,-0.5773503,0,0.5773503},
	{0.5773503,0.5773503,0,-0.5773503},
	{-0.5773503,-0.5773503,0,0.5773503},
	{-0.5773503,0.5773503,0,-0.5773503},
	{0.5773503,-0.5773503,0,-0.5773503},
	{-0.5773503,-0.5773503,0,-0.5773503},
	
	{0.5773503,0.5773503,0.5773503,0},
	{-0.5773503,0.5773503,0.5773503,0},
	{0.5773503,-0.5773503,0.5773503,0},
	{0.5773503,0.5773503,-0.5773503,0},
	{-0.5773503,0.5773503,-0.5773503,0},
	{-0.5773503,-0.5773503,0.5773503,0},
	{0.5773503,-0.5773503,-0.5773503,0},
	{-0.5773503,-0.5773503,-0.5773503,0}
};

double gradient6D_lut[192][6]=
{
	{0,0.447214,0.447214,0.447214,0.447214,0.447214},
	{0,0.447214,0.447214,0.447214,0.447214,-0.447214},
	{0,0.447214,0.447214,0.447214,-0.447214,0.447214},
	{0,0.447214,0.447214,0.447214,-0.447214,-0.447214},
	{0,0.447214,0.447214,-0.447214,0.447214,0.447214},
	{0,0.447214,0.447214,-0.447214,0.447214,-0.447214},
	{0,0.447214,0.447214,-0.447214,-0.447214,0.447214},
	{0,0.447214,0.447214,-0.447214,-0.447214,-0.447214},
	{0,0.447214,-0.447214,0.447214,0.447214,0.447214},
	{0,0.447214,-0.447214,0.447214,0.447214,-0.447214},
	{0,0.447214,-0.447214,0.447214,-0.447214,0.447214},
	{0,0.447214,-0.447214,0.447214,-0.447214,-0.447214},
	{0,0.447214,-0.447214,-0.447214,0.447214,0.447214},
	{0,0.447214,-0.447214,-0.447214,0.447214,-0.447214},
	{0,0.447214,-0.447214,-0.447214,-0.447214,0.447214},
	{0,0.447214,-0.447214,-0.447214,-0.447214,-0.447214},
	{0,-0.447214,0.447214,0.447214,0.447214,0.447214}, //
	{0,-0.447214,0.447214,0.447214,0.447214,-0.447214},
	{0,-0.447214,0.447214,0.447214,-0.447214,0.447214},
	{0,-0.447214,0.447214,0.447214,-0.447214,-0.447214},
	{0,-0.447214,0.447214,-0.447214,0.447214,0.447214},
	{0,-0.447214,0.447214,-0.447214,0.447214,-0.447214},
	{0,-0.447214,0.447214,-0.447214,-0.447214,0.447214},
	{0,-0.447214,0.447214,-0.447214,-0.447214,-0.447214},
	{0,-0.447214,-0.447214,0.447214,0.447214,0.447214},
	{0,-0.447214,-0.447214,0.447214,0.447214,-0.447214},
	{0,-0.447214,-0.447214,0.447214,-0.447214,0.447214},
	{0,-0.447214,-0.447214,0.447214,-0.447214,-0.447214},
	{0,-0.447214,-0.447214,-0.447214,0.447214,0.447214},
	{0,-0.447214,-0.447214,-0.447214,0.447214,-0.447214},
	{0,-0.447214,-0.447214,-0.447214,-0.447214,0.447214},
	{0,-0.447214,-0.447214,-0.447214,-0.447214,-0.447214},
	
	{0.447214,0,0.447214,0.447214,0.447214,0.447214},
	{0.447214,0,0.447214,0.447214,0.447214,-0.447214},
	{0.447214,0,0.447214,0.447214,-0.447214,0.447214},
	{0.447214,0,0.447214,0.447214,-0.447214,-0.447214},
	{0.447214,0,0.447214,-0.447214,0.447214,0.447214},
	{0.447214,0,0.447214,-0.447214,0.447214,-0.447214},
	{0.447214,0,0.447214,-0.447214,-0.447214,0.447214},
	{0.447214,0,0.447214,-0.447214,-0.447214,-0.447214},
	{0.447214,0,-0.447214,0.447214,0.447214,0.447214},
	{0.447214,0,-0.447214,0.447214,0.447214,-0.447214},
	{0.447214,0,-0.447214,0.447214,-0.447214,0.447214},
	{0.447214,0,-0.447214,0.447214,-0.447214,-0.447214},
	{0.447214,0,-0.447214,-0.447214,0.447214,0.447214},
	{0.447214,0,-0.447214,-0.447214,0.447214,-0.447214},
	{0.447214,0,-0.447214,-0.447214,-0.447214,0.447214},
	{0.447214,0,-0.447214,-0.447214,-0.447214,-0.447214},
	{-0.447214,0,0.447214,0.447214,0.447214,0.447214}, //
	{-0.447214,0,0.447214,0.447214,0.447214,-0.447214},
	{-0.447214,0,0.447214,0.447214,-0.447214,0.447214},
	{-0.447214,0,0.447214,0.447214,-0.447214,-0.447214},
	{-0.447214,0,0.447214,-0.447214,0.447214,0.447214},
	{-0.447214,0,0.447214,-0.447214,0.447214,-0.447214},
	{-0.447214,0,0.447214,-0.447214,-0.447214,0.447214},
	{-0.447214,0,0.447214,-0.447214,-0.447214,-0.447214},
	{-0.447214,0,-0.447214,0.447214,0.447214,0.447214},
	{-0.447214,0,-0.447214,0.447214,0.447214,-0.447214},
	{-0.447214,0,-0.447214,0.447214,-0.447214,0.447214},
	{-0.447214,0,-0.447214,0.447214,-0.447214,-0.447214},
	{-0.447214,0,-0.447214,-0.447214,0.447214,0.447214},
	{-0.447214,0,-0.447214,-0.447214,0.447214,-0.447214},
	{-0.447214,0,-0.447214,-0.447214,-0.447214,0.447214},
	{-0.447214,0,-0.447214,-0.447214,-0.447214,-0.447214},
	
	{0.447214,0.447214,0,0.447214,0.447214,0.447214},
	{0.447214,0.447214,0,0.447214,0.447214,-0.447214},
	{0.447214,0.447214,0,0.447214,-0.447214,0.447214},
	{0.447214,0.447214,0,0.447214,-0.447214,-0.447214},
	{0.447214,0.447214,0,-0.447214,0.447214,0.447214},
	{0.447214,0.447214,0,-0.447214,0.447214,-0.447214},
	{0.447214,0.447214,0,-0.447214,-0.447214,0.447214},
	{0.447214,0.447214,0,-0.447214,-0.447214,-0.447214},
	{-0.447214,0.447214,0,0.447214,0.447214,0.447214},
	{-0.447214,0.447214,0,0.447214,0.447214,-0.447214},
	{-0.447214,0.447214,0,0.447214,-0.447214,0.447214},
	{-0.447214,0.447214,0,0.447214,-0.447214,-0.447214},
	{-0.447214,0.447214,0,-0.447214,0.447214,0.447214},
	{-0.447214,0.447214,0,-0.447214,0.447214,-0.447214},
	{-0.447214,0.447214,0,-0.447214,-0.447214,0.447214},
	{-0.447214,0.447214,0,-0.447214,-0.447214,-0.447214},
	{0.447214,-0.447214,0,0.447214,0.447214,0.447214}, //
	{0.447214,-0.447214,0,0.447214,0.447214,-0.447214},
	{0.447214,-0.447214,0,0.447214,-0.447214,0.447214},
	{0.447214,-0.447214,0,0.447214,-0.447214,-0.447214},
	{0.447214,-0.447214,0,-0.447214,0.447214,0.447214},
	{0.447214,-0.447214,0,-0.447214,0.447214,-0.447214},
	{0.447214,-0.447214,0,-0.447214,-0.447214,0.447214},
	{0.447214,-0.447214,0,-0.447214,-0.447214,-0.447214},
	{-0.447214,-0.447214,0,0.447214,0.447214,0.447214},
	{-0.447214,-0.447214,0,0.447214,0.447214,-0.447214},
	{-0.447214,-0.447214,0,0.447214,-0.447214,0.447214},
	{-0.447214,-0.447214,0,0.447214,-0.447214,-0.447214},
	{-0.447214,-0.447214,0,-0.447214,0.447214,0.447214},
	{-0.447214,-0.447214,0,-0.447214,0.447214,-0.447214},
	{-0.447214,-0.447214,0,-0.447214,-0.447214,0.447214},
	{-0.447214,-0.447214,0,-0.447214,-0.447214,-0.447214},
	
	{0.447214,0.447214,0.447214,0,0.447214,0.447214},
	{0.447214,0.447214,0.447214,0,0.447214,-0.447214},
	{0.447214,0.447214,0.447214,0,-0.447214,0.447214},
	{0.447214,0.447214,0.447214,0,-0.447214,-0.447214},
	{0.447214,0.447214,-0.447214,0,0.447214,0.447214},
	{0.447214,0.447214,-0.447214,0,0.447214,-0.447214},
	{0.447214,0.447214,-0.447214,0,-0.447214,0.447214},
	{0.447214,0.447214,-0.447214,0,-0.447214,-0.447214},
	{0.447214,-0.447214,0.447214,0,0.447214,0.447214},
	{0.447214,-0.447214,0.447214,0,0.447214,-0.447214},
	{0.447214,-0.447214,0.447214,0,-0.447214,0.447214},
	{0.447214,-0.447214,0.447214,0,-0.447214,-0.447214},
	{0.447214,-0.447214,-0.447214,0,0.447214,0.447214},
	{0.447214,-0.447214,-0.447214,0,0.447214,-0.447214},
	{0.447214,-0.447214,-0.447214,0,-0.447214,0.447214},
	{0.447214,-0.447214,-0.447214,0,-0.447214,-0.447214},
	{-0.447214,0.447214,0.447214,0,0.447214,0.447214},
	{-0.447214,0.447214,0.447214,0,0.447214,-0.447214},
	{-0.447214,0.447214,0.447214,0,-0.447214,0.447214},
	{-0.447214,0.447214,0.447214,0,-0.447214,-0.447214},
	{-0.447214,0.447214,-0.447214,0,0.447214,0.447214},
	{-0.447214,0.447214,-0.447214,0,0.447214,-0.447214},
	{-0.447214,0.447214,-0.447214,0,-0.447214,0.447214},
	{-0.447214,0.447214,-0.447214,0,-0.447214,-0.447214},
	{-0.447214,-0.447214,0.447214,0,0.447214,0.447214},
	{-0.447214,-0.447214,0.447214,0,0.447214,-0.447214},
	{-0.447214,-0.447214,0.447214,0,-0.447214,0.447214},
	{-0.447214,-0.447214,0.447214,0,-0.447214,-0.447214},
	{-0.447214,-0.447214,-0.447214,0,0.447214,0.447214},
	{-0.447214,-0.447214,-0.447214,0,0.447214,-0.447214},
	{-0.447214,-0.447214,-0.447214,0,-0.447214,0.447214},
	{-0.447214,-0.447214,-0.447214,0,-0.447214,-0.447214},
	
	{0.447214,0.447214,0.447214,0.447214,0,0.447214},
	{0.447214,0.447214,0.447214,0.447214,0,-0.447214},
	{0.447214,0.447214,0.447214,-0.447214,0,0.447214},
	{0.447214,0.447214,0.447214,-0.447214,0,-0.447214},
	{0.447214,0.447214,-0.447214,0.447214,0,0.447214},
	{0.447214,0.447214,-0.447214,0.447214,0,-0.447214},
	{0.447214,0.447214,-0.447214,-0.447214,0,0.447214},
	{0.447214,0.447214,-0.447214,-0.447214,0,-0.447214},
	{0.447214,-0.447214,0.447214,0.447214,0,0.447214},
	{0.447214,-0.447214,0.447214,0.447214,0,-0.447214},
	{0.447214,-0.447214,0.447214,-0.447214,0,0.447214},
	{0.447214,-0.447214,0.447214,-0.447214,0,-0.447214},
	{0.447214,-0.447214,-0.447214,0.447214,0,0.447214},
	{0.447214,-0.447214,-0.447214,0.447214,0,-0.447214},
	{0.447214,-0.447214,-0.447214,-0.447214,0,0.447214},
	{0.447214,-0.447214,-0.447214,-0.447214,0,-0.447214},
	{-0.447214,0.447214,0.447214,0.447214,0,0.447214},
	{-0.447214,0.447214,0.447214,0.447214,0,-0.447214},
	{-0.447214,0.447214,0.447214,-0.447214,0,0.447214},
	{-0.447214,0.447214,0.447214,-0.447214,0,-0.447214},
	{-0.447214,0.447214,-0.447214,0.447214,0,0.447214},
	{-0.447214,0.447214,-0.447214,0.447214,0,-0.447214},
	{-0.447214,0.447214,-0.447214,-0.447214,0,0.447214},
	{-0.447214,0.447214,-0.447214,-0.447214,0,-0.447214},
	{-0.447214,-0.447214,0.447214,0.447214,0,0.447214},
	{-0.447214,-0.447214,0.447214,0.447214,0,-0.447214},
	{-0.447214,-0.447214,0.447214,-0.447214,0,0.447214},
	{-0.447214,-0.447214,0.447214,-0.447214,0,-0.447214},
	{-0.447214,-0.447214,-0.447214,0.447214,0,0.447214},
	{-0.447214,-0.447214,-0.447214,0.447214,0,-0.447214},
	{-0.447214,-0.447214,-0.447214,-0.447214,0,0.447214},
	{-0.447214,-0.447214,-0.447214,-0.447214,0,-0.447214},
	
	{0.447214,0.447214,0.447214,0.447214,0.447214,0},
	{0.447214,0.447214,0.447214,0.447214,-0.447214,0},
	{0.447214,0.447214,0.447214,-0.447214,0.447214,0},
	{0.447214,0.447214,0.447214,-0.447214,-0.447214,0},
	{0.447214,0.447214,-0.447214,0.447214,0.447214,0},
	{0.447214,0.447214,-0.447214,0.447214,-0.447214,0},
	{0.447214,0.447214,-0.447214,-0.447214,0.447214,0},
	{0.447214,0.447214,-0.447214,-0.447214,-0.447214,0},
	{0.447214,-0.447214,0.447214,0.447214,0.447214,0},
	{0.447214,-0.447214,0.447214,0.447214,-0.447214,0},
	{0.447214,-0.447214,0.447214,-0.447214,0.447214,0},
	{0.447214,-0.447214,0.447214,-0.447214,-0.447214,0},
	{0.447214,-0.447214,-0.447214,0.447214,0.447214,0},
	{0.447214,-0.447214,-0.447214,0.447214,-0.447214,0},
	{0.447214,-0.447214,-0.447214,-0.447214,0.447214,0},
	{0.447214,-0.447214,-0.447214,-0.447214,-0.447214,0},
	{-0.447214,0.447214,0.447214,0.447214,0.447214,0},
	{-0.447214,0.447214,0.447214,0.447214,-0.447214,0},
	{-0.447214,0.447214,0.447214,-0.447214,0.447214,0},
	{-0.447214,0.447214,0.447214,-0.447214,-0.447214,0},
	{-0.447214,0.447214,-0.447214,0.447214,0.447214,0},
	{-0.447214,0.447214,-0.447214,0.447214,-0.447214,0},
	{-0.447214,0.447214,-0.447214,-0.447214,0.447214,0},
	{-0.447214,0.447214,-0.447214,-0.447214,-0.447214,0},
	{-0.447214,-0.447214,0.447214,0.447214,0.447214,0},
	{-0.447214,-0.447214,0.447214,0.447214,-0.447214,0},
	{-0.447214,-0.447214,0.447214,-0.447214,0.447214,0},
	{-0.447214,-0.447214,0.447214,-0.447214,-0.447214,0},
	{-0.447214,-0.447214,-0.447214,0.447214,0.447214,0},
	{-0.447214,-0.447214,-0.447214,0.447214,-0.447214,0},
	{-0.447214,-0.447214,-0.447214,-0.447214,0.447214,0},
	{-0.447214,-0.447214,-0.447214,-0.447214,-0.447214,0}
};

double whitenoise_lut[256]=
{
  -0.714286,
  0.301587,
  0.333333,
  -1,
  0.396825,
  -0.0793651,
  -0.968254,
  -0.047619,
  0.301587,
  -0.111111,
  0.015873,
  0.968254,
  -0.428571,
  0.428571,
  0.047619,
  0.84127,
  -0.015873,
  -0.746032,
  -0.809524,
  -0.619048,
  -0.301587,
  -0.68254,
  0.777778,
  0.365079,
  -0.460317,
  0.714286,
  0.142857,
  0.047619,
  -0.0793651,
  -0.492063,
  -0.873016,
  -0.269841,
  -0.84127,
  -0.809524,
  -0.396825,
  -0.777778,
  -0.396825,
  -0.746032,
  0.301587,
  -0.52381,
  0.650794,
  0.301587,
  -0.015873,
  0.269841,
  0.492063,
  -0.936508,
  -0.777778,
  0.555556,
  0.68254,
  -0.650794,
  -0.968254,
  0.619048,
  0.777778,
  0.68254,
  0.206349,
  -0.555556,
  0.904762,
  0.587302,
  -0.174603,
  -0.047619,
  -0.206349,
  -0.68254,
  0.111111,
  -0.52381,
  0.174603,
  -0.968254,
  -0.111111,
  -0.238095,
  0.396825,
  -0.777778,
  -0.206349,
  0.142857,
  0.904762,
  -0.111111,
  -0.269841,
  0.777778,
  -0.015873,
  -0.047619,
  -0.333333,
  0.68254,
  -0.238095,
  0.904762,
  0.0793651,
  0.68254,
  -0.301587,
  -0.333333,
  0.206349,
  0.52381,
  0.904762,
  -0.015873,
  -0.555556,
  0.396825,
  0.460317,
  -0.142857,
  0.587302,
  1,
  -0.650794,
  -0.333333,
  -0.365079,
  0.015873,
  -0.873016,
  -1,
  -0.777778,
  0.174603,
  -0.84127,
  -0.428571,
  0.365079,
  -0.587302,
  -0.587302,
  0.650794,
  0.714286,
  0.84127,
  0.936508,
  0.746032,
  0.047619,
  -0.52381,
  -0.714286,
  -0.746032,
  -0.206349,
  -0.301587,
  -0.174603,
  0.460317,
  0.238095,
  0.968254,
  0.555556,
  -0.269841,
  0.206349,
  -0.0793651,
  0.777778,
  0.174603,
  0.111111,
  -0.714286,
  -0.84127,
  -0.68254,
  0.587302,
  0.746032,
  -0.68254,
  0.587302,
  0.365079,
  0.492063,
  -0.809524,
  0.809524,
  -0.873016,
  -0.142857,
  -0.142857,
  -0.619048,
  -0.873016,
  -0.587302,
  0.0793651,
  -0.269841,
  -0.460317,
  -0.904762,
  -0.174603,
  0.619048,
  0.936508,
  0.650794,
  0.238095,
  0.111111,
  0.873016,
  0.0793651,
  0.460317,
  -0.746032,
  -0.460317,
  0.428571,
  -0.714286,
  -0.365079,
  -0.428571,
  0.206349,
  0.746032,
  -0.492063,
  0.269841,
  0.269841,
  -0.365079,
  0.492063,
  0.873016,
  0.142857,
  0.714286,
  -0.936508,
  1,
  -0.142857,
  -0.904762,
  -0.301587,
  -0.968254,
  0.619048,
  0.269841,
  -0.809524,
  0.936508,
  0.714286,
  0.333333,
  0.428571,
  0.0793651,
  -0.650794,
  0.968254,
  0.809524,
  0.492063,
  0.555556,
  -0.396825,
  -1,
  -0.492063,
  -0.936508,
  -0.492063,
  -0.111111,
  0.809524,
  0.333333,
  0.238095,
  0.174603,
  0.333333,
  0.873016,
  0.809524,
  -0.047619,
  -0.619048,
  -0.174603,
  0.84127,
  0.111111,
  0.619048,
  -0.0793651,
  0.52381,
  1,
  0.015873,
  0.52381,
  -0.619048,
  -0.52381,
  1,
  0.650794,
  -0.428571,
  0.84127,
  -0.555556,
  0.015873,
  0.428571,
  0.746032,
  -0.238095,
  -0.238095,
  0.936508,
  -0.206349,
  -0.936508,
  0.873016,
  -0.555556,
  -0.650794,
  -0.904762,
  0.52381,
  0.968254,
  -0.333333,
  -0.904762,
  0.396825,
  0.047619,
  -0.84127,
  -0.365079,
  -0.587302,
  -1,
  -0.396825,
  0.365079,
  0.555556,
  0.460317,
  0.142857,
  -0.460317,
  0.238095,
};

