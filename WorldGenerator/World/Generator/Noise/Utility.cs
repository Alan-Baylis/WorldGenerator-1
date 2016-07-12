/*
Copyright (c) 2008 Joshua Tippetts

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using Sean.Shared;

namespace Sean.WorldGenerator.Noise
{
    static class Utility
    {
        static public float clamp (float v, float l, float h)
        {
            if (v < l) v = l;
            if (v > h) v = h;
            return v;
        }

        static float lerp (float t, float a, float b)
        {
            return a + t * (b - a);
        }

        static bool isPowerOf2 (int n)
        {
            return ((n - 1) & n) != 0;
        }

        static float hermite_blend (float t)
        {
            return (t * t * (3 - 2 * t));
        }

        static float quintic_blend (float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        static int fast_floor (float t)
        {
            return (t > 0 ? (int)t : (int)t - 1);
        }

        static float array_dot (float[] arr, float a, float b)
        {
            return a * arr [0] + b * arr [1];
        }

        static float array_dot3 (float[] arr, float a, float b, float c)
        {
            return a * arr [0] + b * arr [1] + c * arr [2];
        }

        static float bias (float b, float t)
        {
            return (float)(Math.Pow (t, Math.Log (b) / Math.Log (0.5)));
        }

        static float gain (float g, float t)
        {
            if (t < 0.5f) {
                return bias (1.0f - g, 2.0f * t) / 2.0f;
            } else {
                return 1.0f - bias (1.0f - g, 2.0f - 2.0f * t) / 2.0f;
            }
        }
    }
}

