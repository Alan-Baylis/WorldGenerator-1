
using System;
using static Sean.WorldGenerator.Noise.CImplicitBasisFunction;

namespace Sean.WorldGenerator.Noise
{
    static class anl
    {
        public delegate double interp_func(double x);
        public delegate double noise_func2(double x, double y, uint c, interp_func f);
        public delegate double noise_func3(double x, double y, double z, uint c, interp_func f);
        public delegate double noise_func4(double x, double y, double z, double u, uint c, interp_func f);
        public delegate double noise_func6(double x, double y, double z, double u, double v, double w, uint c, interp_func f);
        public delegate double dist_func2(double x, double y, double z, double u);
        public delegate double dist_func3(double x, double y, double z, double u, double v, double w);
        public delegate double dist_func4(double x, double y, double z, double u, double v, double w, double i, double j);
        public delegate double dist_func6(double x, double y, double z, double u, double v, double w, double i, double j, double k, double l, double m, double n);

        // Worker noise functions
        public delegate double worker_noise_2(double x, double y, int i, int j, uint c);
        public delegate double worker_noise_3(double x, double y, double z, int i, int j, int k, uint c);
        public delegate double worker_noise_4(double x, double y, double z, double u, int i, int j, int k, int l, uint c);
        public delegate double worker_noise_6(double x, double y, double z, double u, double v, double w, int i, int j, int k, int l, int m, int n, uint c);


        public static double noInterp(double t)
        {
            return 0;
        }

        public static double linearInterp(double t)
        {
            return t;
        }

        public static double hermiteInterp(double t)
        {
            return (t * t * (3 - 2 * t));
        }

        public static double quinticInterp(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);

        }

        public static int fast_floor(double t)
        {
            return (t > 0 ? (int)t : (int)t - 1);
        }

        public static double array_dot2(double[] arr, double a, double b)
        {
            return a * arr[0] + b * arr[1];
        }

        public static double array_dot3(double[] arr, double a, double b, double c)
        {
            return a * arr[0] + b * arr[1] + c * arr[2];
        }

        public static double array_dot4(double[] arr, double x, double y, double z, double w)
        {
            return x * arr[0] + y * arr[1] + z * arr[2] + w * arr[3];
        }

        public static double array_dot6(double[] arr, double x, double y, double z, double w, double u, double v)
        {
            return x * arr[0] + y * arr[1] + z * arr[2] + w * arr[3] + u * arr[4] + v * arr[5];
        }

        // Distance

        public static double distEuclid2(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double distEuclid3(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double dz = z2 - z1;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static double distEuclid4(double x1, double y1, double z1, double w1, double x2, double y2, double z2, double w2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double dz = z2 - z1;
            double dw = w2 - w1;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
        }

        public static double distEuclid6(double x1, double y1, double z1, double w1, double u1, double v1, double x2, double y2, double z2, double w2, double u2, double v2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;
            double dz = z2 - z1;
            double dw = w2 - w1;
            double du = u2 - u1;
            double dv = v2 - v1;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw + du * du + dv * dv);
        }


        public static double distManhattan2(double x1, double y1, double x2, double y2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            //return Math.Max(dx,dy);
            return dx + dy;
        }

        public static double distManhattan3(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            double dz = Math.Abs(z2 - z1);
            //return Math.Max(dz,Math.Max(dx,dy));
            return dx + dy + dz;
        }

        public static double distManhattan4(double x1, double y1, double z1, double w1, double x2, double y2, double z2, double w2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            double dz = Math.Abs(z2 - z1);
            double dw = Math.Abs(w2 - w1);
            //return Math.Max(dw,Math.Max(dz,Math.Max(dx,dy)));
            return dx + dy + dz + dw;
        }

        public static double distManhattan6(double x1, double y1, double z1, double w1, double u1, double v1, double x2, double y2, double z2, double w2, double u2, double v2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            double dz = Math.Abs(z2 - z1);
            double dw = Math.Abs(w2 - w1);
            double du = Math.Abs(u2 - u1);
            double dv = Math.Abs(v2 - v1);
            //return Math.Max(du,Math.Max(dv,Math.Max(dw,Math.Max(dz,Math.Max(dx,dy)))));
            return dx + dy + dz + dw + du + dv;
        }

        public static double distGreatestAxis2(double x1, double y1, double x2, double y2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            return Math.Max(dx, dy);
        }

        public static double distGreatestAxis3(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            double dz = Math.Abs(z2 - z1);
            return Math.Max(dz, Math.Max(dx, dy));
        }

        public static double distGreatestAxis4(double x1, double y1, double z1, double w1, double x2, double y2, double z2, double w2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            double dz = Math.Abs(z2 - z1);
            double dw = Math.Abs(w2 - w1);
            return Math.Max(dw, Math.Max(dz, Math.Max(dx, dy)));
        }

        public static double distGreatestAxis6(double x1, double y1, double z1, double w1, double u1, double v1, double x2, double y2, double z2, double w2, double u2, double v2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            double dz = Math.Abs(z2 - z1);
            double dw = Math.Abs(w2 - w1);
            double du = Math.Abs(u2 - u1);
            double dv = Math.Abs(v2 - v1);
            return Math.Max(du, Math.Max(dv, Math.Max(dw, Math.Max(dz, Math.Max(dx, dy)))));
        }

        public static double distLeastAxis2(double x1, double y1, double x2, double y2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            return Math.Min(dx, dy);
        }

        public static double distLeastAxis3(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            double dz = Math.Abs(z2 - z1);
            return Math.Min(dz, Math.Min(dx, dy));
        }

        public static double distLeastAxis4(double x1, double y1, double z1, double w1, double x2, double y2, double z2, double w2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            double dz = Math.Abs(z2 - z1);
            double dw = Math.Abs(w2 - w1);
            return Math.Min(dw, Math.Min(dz, Math.Min(dx, dy)));
        }

        public static double distLeastAxis6(double x1, double y1, double z1, double w1, double u1, double v1, double x2, double y2, double z2, double w2, double u2, double v2)
        {
            double dx = Math.Abs(x2 - x1);
            double dy = Math.Abs(y2 - y1);
            double dz = Math.Abs(z2 - z1);
            double dw = Math.Abs(w2 - w1);
            double du = Math.Abs(u2 - u1);
            double dv = Math.Abs(v2 - v1);
            return Math.Min(du, Math.Min(dv, Math.Min(dw, Math.Min(dz, Math.Min(dx, dy)))));
        }

        public static byte compute_hash_double_2(double x, double y, uint seed)
        {
            //double d[3]={x,y,(double)seed};
            //uint s=sizeof(d) / sizeof(uint);
            //return xor_fold_hash(fnv_32_a_buf(d, s));

            uint hash = Hashing.FNV_32_INIT;
            hash = Hashing.fnv_32_a_combine(hash, (uint)(x * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(y * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, seed);
            return Hashing.xor_fold_hash(hash);
        }


        public static byte compute_hash_double_3(double x, double y, double z, uint seed)
        {
            //double d[4]={x,y,z,(double)seed};
            //uint s=sizeof(d) / sizeof(uint);
            //return xor_fold_hash(fnv_32_a_buf(d, s));
            uint hash = Hashing.FNV_32_INIT;
            hash = Hashing.fnv_32_a_combine(hash, (uint)(x * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(y * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(z * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, seed);
            return Hashing.xor_fold_hash(hash);

        }

        public static byte compute_hash_double_4(double x, double y, double z, double w, uint seed)
        {
            //double d[5]={x,y,z,w,(double)seed};
            //uint s=sizeof(d) / sizeof(uint);
            //return xor_fold_hash(fnv_32_a_buf(d, s));
            uint hash = Hashing.FNV_32_INIT;
            hash = Hashing.fnv_32_a_combine(hash, (uint)(x * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(y * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(z * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(w * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, seed);
            return Hashing.xor_fold_hash(hash);
        }

        public static byte compute_hash_double_6(double x, double y, double z, double w, double u, double v, uint seed)
        {
            //double d[7]={x,y,z,w,u,v,(double)seed};
            //uint s=sizeof(d) / sizeof(uint);
            //return xor_fold_hash(fnv_32_a_buf(d, s));
            uint hash = Hashing.FNV_32_INIT;
            hash = Hashing.fnv_32_a_combine(hash, (uint)(x * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(y * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(z * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(w * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(u * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, (uint)(v * (double)1000000));
            hash = Hashing.fnv_32_a_combine(hash, seed);
            return Hashing.xor_fold_hash(hash);
        }




        public static double value_noise_2(double x, double y, int ix, int iy, uint seed)
        {
            uint n = Hashing.hash_coords_2(ix, iy, seed) % 256;
            double noise = (double)n / 255.0;
            return noise * 2.0 - 1.0;
        }
        public static double value_noise_3(double x, double y, double z, int ix, int iy, int iz, uint seed)
        {
            uint n = Hashing.hash_coords_3(ix, iy, iz, seed) % 256;
            double noise = (double)n / (255.0);
            return noise * 2.0 - 1.0;
        }
        public static double value_noise_4(double x, double y, double z, double w, int ix, int iy, int iz, int iw, uint seed)
        {
            uint n = Hashing.hash_coords_4(ix, iy, iz, iw, seed) % 256;
            double noise = (double)n / 255.0;
            return noise * 2.0 - 1.0;
        }
        public static double value_noise_6(double x, double y, double z, double w, double u, double v, int ix, int iy, int iz, int iw, int iu, int iv, uint seed)
        {
            uint n = Hashing.hash_coords_6(ix, iy, iz, iw, iu, iv, seed) % 256;
            double noise = (double)n / 255.0;
            return noise * 2.0 - 1.0;
        }

        public static double grad_noise_2(double x, double y, int ix, int iy, uint seed)
        {
            uint hash = Hashing.hash_coords_2(ix, iy, seed) % 4;
            double[] vec = Lut.gradient2D_lut[hash];

            double dx = x - (double)ix;
            double dy = y - (double)iy;

            return (dx * vec[0] + dy * vec[1]);
        }

        public static double grad_noise_3(double x, double y, double z, int ix, int iy, int iz, uint seed)
        {
            uint hash = Hashing.hash_coords_3(ix, iy, iz, seed) % 12;
            double[] vec = Lut.gradient3D_lut[hash];

            double dx = x - (double)ix;
            double dy = y - (double)iy;
            double dz = z - (double)iz;
            return (dx * vec[0] + dy * vec[1] + dz * vec[2]);
        }

        public static double grad_noise_4(double x, double y, double z, double w, int ix, int iy, int iz, int iw, uint seed)
        {
            uint hash = Hashing.hash_coords_4(ix, iy, iz, iw, seed) % 32;
            double[] vec = Lut.gradient4D_lut[hash];

            double dx = x - (double)ix;
            double dy = y - (double)iy;
            double dz = z - (double)iz;
            double dw = w - (double)iw;

            return (dx * vec[0] + dy * vec[1] + dz * vec[2] + dw * vec[3]);

        }

        public static double grad_noise_6(double x, double y, double z, double w, double u, double v, int ix, int iy, int iz, int iw, int iu, int iv, uint seed)
        {
            uint hash = Hashing.hash_coords_6(ix, iy, iz, iw, iu, iv, seed) % 192;
            double[] vec = Lut.gradient6D_lut[hash];

            double dx = x - (double)ix;
            double dy = y - (double)iy;
            double dz = z - (double)iz;
            double dw = w - (double)iw;
            double du = u - (double)iu;
            double dv = v - (double)iv;

            return (dx * vec[0] + dy * vec[1] + dz * vec[2] + dw * vec[3] + du * vec[4] + dv * vec[5]);

        }



        // Edge/Face/Cube/Hypercube interpolation
        public static double lerp(double s, double v1, double v2)
        {
            return v1 + s * (v2 - v1);
        }

        public static double interp_X_2(double x, double y, double xs, int x0, int x1, int iy, uint seed, worker_noise_2 noisefunc)
        {
            double v1 = noisefunc(x, y, x0, iy, seed);
            double v2 = noisefunc(x, y, x1, iy, seed);
            return lerp(xs, v1, v2);
        }

        public static double interp_XY_2(double x, double y, double xs, double ys, int x0, int x1, int y0, int y1, uint seed, worker_noise_2 noisefunc)
        {
            double v1 = interp_X_2(x, y, xs, x0, x1, y0, seed, noisefunc);
            double v2 = interp_X_2(x, y, xs, x0, x1, y1, seed, noisefunc);
            return lerp(ys, v1, v2);
        }

        public static double interp_X_3(double x, double y, double z, double xs, int x0, int x1, int iy, int iz, uint seed, worker_noise_3 noisefunc)
        {
            double v1 = noisefunc(x, y, z, x0, iy, iz, seed);
            double v2 = noisefunc(x, y, z, x1, iy, iz, seed);
            return lerp(xs, v1, v2);
        }

        public static double interp_XY_3(double x, double y, double z, double xs, double ys, int x0, int x1, int y0, int y1, int iz, uint seed, worker_noise_3 noisefunc)
        {
            double v1 = interp_X_3(x, y, z, xs, x0, x1, y0, iz, seed, noisefunc);
            double v2 = interp_X_3(x, y, z, xs, x0, x1, y1, iz, seed, noisefunc);
            return lerp(ys, v1, v2);
        }

        public static double interp_XYZ_3(double x, double y, double z, double xs, double ys, double zs, int x0, int x1, int y0, int y1, int z0, int z1, uint seed, worker_noise_3 noisefunc)
        {
            double v1 = interp_XY_3(x, y, z, xs, ys, x0, x1, y0, y1, z0, seed, noisefunc);
            double v2 = interp_XY_3(x, y, z, xs, ys, x0, x1, y0, y1, z1, seed, noisefunc);
            return lerp(zs, v1, v2);
        }

        public static double interp_X_4(double x, double y, double z, double w, double xs, int x0, int x1, int iy, int iz, int iw, uint seed, worker_noise_4 noisefunc)
        {
            double v1 = noisefunc(x, y, z, w, x0, iy, iz, iw, seed);
            double v2 = noisefunc(x, y, z, w, x1, iy, iz, iw, seed);
            return lerp(xs, v1, v2);
        }

        public static double interp_XY_4(double x, double y, double z, double w, double xs, double ys, int x0, int x1, int y0, int y1, int iz, int iw, uint seed, worker_noise_4 noisefunc)
        {
            double v1 = interp_X_4(x, y, z, w, xs, x0, x1, y0, iz, iw, seed, noisefunc);
            double v2 = interp_X_4(x, y, z, w, xs, x0, x1, y1, iz, iw, seed, noisefunc);
            return lerp(ys, v1, v2);
        }

        public static double interp_XYZ_4(double x, double y, double z, double w, double xs, double ys, double zs, int x0, int x1, int y0, int y1, int z0, int z1, int iw, uint seed, worker_noise_4 noisefunc)
        {
            double v1 = interp_XY_4(x, y, z, w, xs, ys, x0, x1, y0, y1, z0, iw, seed, noisefunc);
            double v2 = interp_XY_4(x, y, z, w, xs, ys, x0, x1, y0, y1, z1, iw, seed, noisefunc);
            return lerp(zs, v1, v2);
        }

        public static double interp_XYZW_4(double x, double y, double z, double w, double xs, double ys, double zs, double ws, int x0, int x1, int y0, int y1, int z0, int z1, int w0, int w1, uint seed, worker_noise_4 noisefunc)
        {
            double v1 = interp_XYZ_4(x, y, z, w, xs, ys, zs, x0, x1, y0, y1, z0, z1, w0, seed, noisefunc);
            double v2 = interp_XYZ_4(x, y, z, w, xs, ys, zs, x0, x1, y0, y1, z0, z1, w1, seed, noisefunc);
            return lerp(ws, v1, v2);
        }


        public static double interp_X_6(double x, double y, double z, double w, double u, double v, double xs, int x0, int x1, int iy, int iz, int iw, int iu, int iv, uint seed, worker_noise_6 noisefunc)
        {
            double v1 = noisefunc(x, y, z, w, u, v, x0, iy, iz, iw, iu, iv, seed);
            double v2 = noisefunc(x, y, z, w, u, v, x1, iy, iz, iw, iu, iv, seed);
            return lerp(xs, v1, v2);
        }

        public static double interp_XY_6(double x, double y, double z, double w, double u, double v, double xs, double ys, int x0, int x1, int y0, int y1, int iz, int iw, int iu, int iv, uint seed, worker_noise_6 noisefunc)
        {
            double v1 = interp_X_6(x, y, z, w, u, v, xs, x0, x1, y0, iz, iw, iu, iv, seed, noisefunc);
            double v2 = interp_X_6(x, y, z, w, u, v, xs, x0, x1, y1, iz, iw, iu, iv, seed, noisefunc);
            return lerp(ys, v1, v2);
        }

        public static double interp_XYZ_6(double x, double y, double z, double w, double u, double v, double xs, double ys, double zs, int x0, int x1, int y0, int y1, int z0, int z1, int iw, int iu, int iv, uint seed, worker_noise_6 noisefunc)
        {
            double v1 = interp_XY_6(x, y, z, w, u, v, xs, ys, x0, x1, y0, y1, z0, iw, iu, iv, seed, noisefunc);
            double v2 = interp_XY_6(x, y, z, w, u, v, xs, ys, x0, x1, y0, y1, z1, iw, iu, iv, seed, noisefunc);
            return lerp(zs, v1, v2);
        }

        public static double interp_XYZW_6(double x, double y, double z, double w, double u, double v, double xs, double ys, double zs, double ws, int x0, int x1, int y0, int y1, int z0, int z1, int w0, int w1, int iu, int iv, uint seed, worker_noise_6 noisefunc)
        {
            double v1 = interp_XYZ_6(x, y, z, w, u, v, xs, ys, zs, x0, x1, y0, y1, z0, z1, w0, iu, iv, seed, noisefunc);
            double v2 = interp_XYZ_6(x, y, z, w, u, v, xs, ys, zs, x0, x1, y0, y1, z0, z1, w1, iu, iv, seed, noisefunc);
            return lerp(ws, v1, v2);
        }

        public static double interp_XYZWU_6(double x, double y, double z, double w, double u, double v, double xs, double ys, double zs, double ws, double us, int x0, int x1, int y0, int y1, int z0, int z1, int w0, int w1, int u0, int u1, int iv, uint seed, worker_noise_6 noisefunc)
        {
            double v1 = interp_XYZW_6(x, y, z, w, u, v, xs, ys, zs, ws, x0, x1, y0, y1, z0, z1, w0, w1, u0, iv, seed, noisefunc);
            double v2 = interp_XYZW_6(x, y, z, w, u, v, xs, ys, zs, ws, x0, x1, y0, y1, z0, z1, w0, w1, u1, iv, seed, noisefunc);
            return lerp(us, v1, v2);
        }

        public static double interp_XYZWUV_6(double x, double y, double z, double w, double u, double v, double xs, double ys, double zs, double ws, double us, double vs, int x0, int x1, int y0, int y1, int z0, int z1, int w0, int w1, int u0, int u1, int v0, int v1, uint seed, worker_noise_6 noisefunc)
        {
            double val1 = interp_XYZWU_6(x, y, z, w, u, v, xs, ys, zs, ws, us, x0, x1, y0, y1, z0, z1, w0, w1, u0, u1, v0, seed, noisefunc);
            double val2 = interp_XYZWU_6(x, y, z, w, u, v, xs, ys, zs, ws, us, x0, x1, y0, y1, z0, z1, w0, w1, u0, u1, v1, seed, noisefunc);
            return lerp(vs, val1, val2);
        }



        // The usable noise functions

        public static double value_noise2D(double x, double y, uint seed, interp_func interp)
        {
            int x0 = fast_floor(x);
            int y0 = fast_floor(y);

            int x1 = x0 + 1;
            int y1 = y0 + 1;

            double xs = interp((x - (double)x0));
            double ys = interp((y - (double)y0));

            return interp_XY_2(x, y, xs, ys, x0, x1, y0, y1, seed, value_noise_2);
        }

        public static double value_noise3D(double x, double y, double z, uint seed, interp_func interp)
        {
            int x0 = fast_floor(x);
            int y0 = fast_floor(y);
            int z0 = fast_floor(z);
            int x1 = x0 + 1;
            int y1 = y0 + 1;
            int z1 = z0 + 1;

            double xs = interp((x - (double)x0));
            double ys = interp((y - (double)y0));
            double zs = interp((z - (double)z0));

            return interp_XYZ_3(x, y, z, xs, ys, zs, x0, x1, y0, y1, z0, z1, seed, value_noise_3);
        }

        public static double value_noise4D(double x, double y, double z, double w, uint seed, interp_func interp)
        {
            int x0 = fast_floor(x);
            int y0 = fast_floor(y);
            int z0 = fast_floor(z);
            int w0 = fast_floor(w);

            int x1 = x0 + 1;
            int y1 = y0 + 1;
            int z1 = z0 + 1;
            int w1 = w0 + 1;

            double xs = interp((x - (double)x0));
            double ys = interp((y - (double)y0));
            double zs = interp((z - (double)z0));
            double ws = interp((w - (double)w0));

            return interp_XYZW_4(x, y, z, w, xs, ys, zs, ws, x0, x1, y0, y1, z0, z1, w0, w1, seed, value_noise_4);
        }

        public static double value_noise6D(double x, double y, double z, double w, double u, double v, uint seed, interp_func interp)
        {
            int x0 = fast_floor(x);
            int y0 = fast_floor(y);
            int z0 = fast_floor(z);
            int w0 = fast_floor(w);
            int u0 = fast_floor(u);
            int v0 = fast_floor(v);

            int x1 = x0 + 1;
            int y1 = y0 + 1;
            int z1 = z0 + 1;
            int w1 = w0 + 1;
            int u1 = u0 + 1;
            int v1 = v0 + 1;

            double xs = interp((x - (double)x0));
            double ys = interp((y - (double)y0));
            double zs = interp((z - (double)z0));
            double ws = interp((w - (double)w0));
            double us = interp((u - (double)u0));
            double vs = interp((v - (double)v0));

            return interp_XYZWUV_6(x, y, z, w, u, v, xs, ys, zs, ws, us, vs, x0, x1, y0, y1, z0, z1, w0, w1, u0, u1, v0, v1, seed, value_noise_6);
        }

        public static double gradient_noise2D(double x, double y, uint seed, interp_func interp)
        {
            int x0 = fast_floor(x);
            int y0 = fast_floor(y);

            int x1 = x0 + 1;
            int y1 = y0 + 1;

            double xs = interp((x - (double)x0));
            double ys = interp((y - (double)y0));

            return interp_XY_2(x, y, xs, ys, x0, x1, y0, y1, seed, grad_noise_2);
        }

        public static double gradient_noise3D(double x, double y, double z, uint seed, interp_func interp)
        {
            int x0 = fast_floor(x);
            int y0 = fast_floor(y);
            int z0 = fast_floor(z);

            int x1 = x0 + 1;
            int y1 = y0 + 1;
            int z1 = z0 + 1;

            double xs = interp((x - (double)x0));
            double ys = interp((y - (double)y0));
            double zs = interp((z - (double)z0));

            return interp_XYZ_3(x, y, z, xs, ys, zs, x0, x1, y0, y1, z0, z1, seed, grad_noise_3);
        }

        public static double gradient_noise4D(double x, double y, double z, double w, uint seed, interp_func interp)
        {
            int x0 = fast_floor(x);
            int y0 = fast_floor(y);
            int z0 = fast_floor(z);
            int w0 = fast_floor(w);

            int x1 = x0 + 1;
            int y1 = y0 + 1;
            int z1 = z0 + 1;
            int w1 = w0 + 1;

            double xs = interp((x - (double)x0));
            double ys = interp((y - (double)y0));
            double zs = interp((z - (double)z0));
            double ws = interp((w - (double)w0));

            return interp_XYZW_4(x, y, z, w, xs, ys, zs, ws, x0, x1, y0, y1, z0, z1, w0, w1, seed, grad_noise_4);
        }
        public static double gradient_noise6D(double x, double y, double z, double w, double u, double v, uint seed, interp_func interp)
        {
            int x0 = fast_floor(x);
            int y0 = fast_floor(y);
            int z0 = fast_floor(z);
            int w0 = fast_floor(w);
            int u0 = fast_floor(u);
            int v0 = fast_floor(v);

            int x1 = x0 + 1;
            int y1 = y0 + 1;
            int z1 = z0 + 1;
            int w1 = w0 + 1;
            int u1 = u0 + 1;
            int v1 = v0 + 1;

            double xs = interp((x - (double)x0));
            double ys = interp((y - (double)y0));
            double zs = interp((z - (double)z0));
            double ws = interp((w - (double)w0));
            double us = interp((u - (double)u0));
            double vs = interp((v - (double)v0));

            return interp_XYZWUV_6(x, y, z, w, u, v, xs, ys, zs, ws, us, vs, x0, x1, y0, y1, z0, z1, w0, w1, u0, u1, v0, v1, seed, grad_noise_6);
        }

        public static double gradval_noise2D(double x, double y, uint seed, interp_func interp)
        {
            return value_noise2D(x, y, seed, interp) + gradient_noise2D(x, y, seed, interp);
        }

        public static double gradval_noise3D(double x, double y, double z, uint seed, interp_func interp)
        {
            return value_noise3D(x, y, z, seed, interp) + gradient_noise3D(x, y, z, seed, interp);
        }

        public static double gradval_noise4D(double x, double y, double z, double w, uint seed, interp_func interp)
        {
            return value_noise4D(x, y, z, w, seed, interp) + gradient_noise4D(x, y, z, w, seed, interp);
        }

        public static double gradval_noise6D(double x, double y, double z, double w, double u, double v, uint seed, interp_func interp)
        {
            return value_noise6D(x, y, z, w, u, v, seed, interp) + gradient_noise6D(x, y, z, w, u, v, seed, interp);
        }

        public static double white_noise2D(double x, double y, uint seed, interp_func interp)
        {
            byte hash = compute_hash_double_2(x, y, seed);
            return Lut.whitenoise_lut[hash];
        }

        public static double white_noise3D(double x, double y, double z, uint seed, interp_func interp)
        {
            byte hash = compute_hash_double_3(x, y, z, seed);
            //std::cout << (uint)hash << std::endl;
            return Lut.whitenoise_lut[hash];
        }

        public static double white_noise4D(double x, double y, double z, double w, uint seed, interp_func interp)
        {
            byte hash = compute_hash_double_4(x, y, z, w, seed);
            return Lut.whitenoise_lut[hash];
        }

        public static double white_noise6D(double x, double y, double z, double w, double u, double v, uint seed, interp_func interp)
        {
            byte hash = compute_hash_double_6(x, y, z, w, u, v, seed);
            return Lut.whitenoise_lut[hash];
        }

        public static void add_dist(ref double[] f, ref double[] disp, double testdist, double testdisp)
        {
            int index;
            // Compare the given distance to the ones already in f
            if (testdist < f[3])
            {
                index = 3;
                while (index > 0 && testdist < f[index - 1]) index--;
                for (int i = 3; i-- > index;)
                {
                    f[i + 1] = f[i];
                    disp[i + 1] = disp[i];
                }
                f[index] = testdist;
                disp[index] = testdisp;
            }
        }


        // Cellular functions. Compute distance (for cellular modules) and displacement (for voronoi modules)



        public static void cellular_function2D(double x, double y, uint seed, ref double[] f, ref double[] disp, dist_func2 distance)
        {
            int xint = fast_floor(x);
            int yint = fast_floor(y);

            for (int c = 0; c < 4; ++c) { f[c] = 99999.0; disp[c] = 0.0; }

            {
                for (int ycur = yint - 3; ycur <= yint + 3; ++ycur)
                {
                    for (int xcur = xint - 3; xcur <= xint + 3; ++xcur)
                    {
                        double xpos = (double)xcur + value_noise_2(x, y, xcur, ycur, seed);
                        double ypos = (double)ycur + value_noise_2(x, y, xcur, ycur, seed + 1);
                        //double xdist=xpos-x;
                        //double ydist=ypos-y;
                        //double dist=(xdist*xdist + ydist*ydist);
                        double dist = distance(xpos, ypos, x, y);
                        int xval = fast_floor(xpos);
                        int yval = fast_floor(ypos);
                        double dsp = value_noise_2(x, y, xval, yval, seed + 3);
                        add_dist(ref f, ref disp, dist, dsp);
                    }
                }
            }
        }

        public static void cellular_function3D(double x, double y, double z, uint seed, ref double[] f, ref double[] disp, dist_func3 distance)
        {
            int xint = fast_floor(x);
            int yint = fast_floor(y);
            int zint = fast_floor(z);

            for (int c = 0; c < 4; ++c) { f[c] = 99999.0; disp[c] = 0.0; }

            for (int zcur = zint - 2; zcur <= zint + 2; ++zcur)
            {
                for (int ycur = yint - 2; ycur <= yint + 2; ++ycur)
                {
                    for (int xcur = xint - 2; xcur <= xint + 2; ++xcur)
                    {
                        double xpos = (double)xcur + value_noise_3(x, y, z, xcur, ycur, zcur, seed);
                        double ypos = (double)ycur + value_noise_3(x, y, z, xcur, ycur, zcur, seed + 1);
                        double zpos = (double)zcur + value_noise_3(x, y, z, xcur, ycur, zcur, seed + 2);
                        //double xdist=xpos-x;
                        //double ydist=ypos-y;
                        //double zdist=zpos-z;
                        //double dist=(xdist*xdist + ydist*ydist + zdist*zdist);
                        double dist = distance(xpos, ypos, zpos, x, y, z);
                        int xval = fast_floor(xpos);
                        int yval = fast_floor(ypos);
                        int zval = fast_floor(zpos);
                        double dsp = value_noise_3(x, y, z, xval, yval, zval, seed + 3);
                        add_dist(ref f, ref disp, dist, dsp);
                    }
                }
            }
        }

        public static void cellular_function4D(double x, double y, double z, double w, uint seed, ref double[] f, ref double[] disp, dist_func4 distance)
        {
            int xint = fast_floor(x);
            int yint = fast_floor(y);
            int zint = fast_floor(z);
            int wint = fast_floor(w);

            for (int c = 0; c < 4; ++c) { f[c] = 99999.0; disp[c] = 0.0; }

            for (int wcur = wint - 2; wcur <= wint + 2; ++wcur)
            {
                for (int zcur = zint - 2; zcur <= zint + 2; ++zcur)
                {
                    for (int ycur = yint - 2; ycur <= yint + 2; ++ycur)
                    {
                        for (int xcur = xint - 2; xcur <= xint + 2; ++xcur)
                        {
                            double xpos = (double)xcur + value_noise_4(x, y, z, w, xcur, ycur, zcur, wcur, seed);
                            double ypos = (double)ycur + value_noise_4(x, y, z, w, xcur, ycur, zcur, wcur, seed + 1);
                            double zpos = (double)zcur + value_noise_4(x, y, z, w, xcur, ycur, zcur, wcur, seed + 2);
                            double wpos = (double)wcur + value_noise_4(x, y, z, w, xcur, ycur, zcur, wcur, seed + 3);
                            //double xdist=xpos-x;
                            //double ydist=ypos-y;
                            //double zdist=zpos-z;
                            //double wdist=wpos-w;
                            //double dist=(xdist*xdist + ydist*ydist + zdist*zdist + wdist*wdist);
                            double dist = distance(xpos, ypos, zpos, wpos, x, y, z, w);
                            int xval = fast_floor(xpos);
                            int yval = fast_floor(ypos);
                            int zval = fast_floor(zpos);
                            int wval = fast_floor(wpos);
                            double dsp = value_noise_4(x, y, z, w, xval, yval, zval, wval, seed + 3);
                            add_dist(ref f, ref disp, dist, dsp);
                        }
                    }
                }
            }
        }

        public static void cellular_function6D(double x, double y, double z, double w, double u, double v, uint seed, ref double[] f, ref double[] disp, dist_func6 distance)
        {
            int xInt = fast_floor(x);
            int yInt = fast_floor(y);
            int zInt = fast_floor(z);
            int wInt = fast_floor(w);
            int uInt = fast_floor(u);
            int vInt = fast_floor(v);

            for (int c = 0; c < 4; ++c) { f[c] = 99999.0; disp[c] = 0.0; }

            for (int vcur = vInt - 1; vcur <= vInt + 1; ++vcur)
            {
                for (int ucur = uInt - 1; ucur <= uInt + 1; ++ucur)
                {

                    for (int wcur = wInt - 2; wcur <= wInt + 2; ++wcur)
                    {
                        for (int zcur = zInt - 2; zcur <= zInt + 2; ++zcur)
                        {
                            for (int ycur = yInt - 2; ycur <= yInt + 2; ++ycur)
                            {
                                for (int xcur = xInt - 2; xcur <= xInt + 2; ++xcur)
                                {
                                    double xpos = (double)xcur + value_noise_6(x, y, z, w, u, v, xcur, ycur, zcur, wcur, ucur, vcur, seed);
                                    double ypos = (double)ycur + value_noise_6(x, y, z, w, u, v, xcur, ycur, zcur, wcur, ucur, vcur, seed + 1);
                                    double zpos = (double)zcur + value_noise_6(x, y, z, w, u, v, xcur, ycur, zcur, wcur, ucur, vcur, seed + 2);
                                    double wpos = (double)wcur + value_noise_6(x, y, z, w, u, v, xcur, ycur, zcur, wcur, ucur, vcur, seed + 3);
                                    double upos = (double)ucur + value_noise_6(x, y, z, w, u, v, xcur, ycur, zcur, wcur, ucur, vcur, seed + 4);
                                    double vpos = (double)vcur + value_noise_6(x, y, z, w, u, v, xcur, ycur, zcur, wcur, ucur, vcur, seed + 5);
                                    //double xdist=xpos-x;
                                    //double ydist=ypos-y;
                                    //double zdist=zpos-z;
                                    //double wdist=wpos-w;
                                    //double udist=upos-u;
                                    //double vdist=vpos-v;
                                    //double dist=(xdist*xdist + ydist*ydist + zdist*zdist + wdist*wdist + udist*udist + vdist*vdist);
                                    double dist = distance(xpos, ypos, zpos, wpos, upos, vpos, x, y, z, w, u, v);
                                    int xval = fast_floor(xpos);
                                    int yval = fast_floor(ypos);
                                    int zval = fast_floor(zpos);
                                    int wval = fast_floor(wpos);
                                    int uval = fast_floor(upos);
                                    int vval = fast_floor(vpos);
                                    double dsp = value_noise_6(x, y, z, w, u, v, xval, yval, zval, wval, uval, vval, seed + 6);
                                    add_dist(ref f, ref disp, dist, dsp);
                                }
                            }
                        }
                    }
                }
            }
        }


        private const double F2 = 0.36602540378443864676372317075294;
        private const double G2 = 0.21132486540518711774542560974902;
        private const double F3 = 1.0 / 3.0;
        private const double G3 = 1.0 / 6.0;


        public static double simplex_noise2D(double x, double y, uint seed, interp_func interp)
        {
            double s = (x + y) * F2;
            int i = fast_floor(x + s);
            int j = fast_floor(y + s);

            double t = (i + j) * G2;
            double X0 = i - t;
            double Y0 = j - t;
            double x0 = x - X0;
            double y0 = y - Y0;

            int i1, j1;
            if (x0 > y0)
            {
                i1 = 1; j1 = 0;
            }
            else
            {
                i1 = 0; j1 = 1;
            }

            double x1 = x0 - (double)i1 + G2;
            double y1 = y0 - (double)j1 + G2;
            double x2 = x0 - 1.0 + 2.0 * G2;
            double y2 = y0 - 1.0 + 2.0 * G2;

            // Hash the triangle coordinates to index the gradient table
            uint h0 = Hashing.hash_coords_2(i, j, seed) % 4;
            uint h1 = Hashing.hash_coords_2(i + i1, j + j1, seed) % 4;
            uint h2 = Hashing.hash_coords_2(i + 1, j + 1, seed) % 4;

            // Now, index the tables
            double[] g0 = Lut.gradient2D_lut[h0];
            double[] g1 = Lut.gradient2D_lut[h1];
            double[] g2 = Lut.gradient2D_lut[h2];

            double n0, n1, n2;
            // Calculate the contributions from the 3 corners
            double t0 = 0.5 - x0 * x0 - y0 * y0;
            if (t0 < 0) n0 = 0;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * array_dot2(g0, x0, y0);
            }

            double t1 = 0.5 - x1 * x1 - y1 * y1;
            if (t1 < 0) n1 = 0;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * array_dot2(g1, x1, y1);
            }

            double t2 = 0.5 - x2 * x2 - y2 * y2;
            if (t2 < 0) n2 = 0;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * array_dot2(g2, x2, y2);
            }

            // Add contributions together
            return (70.0 * (n0 + n1 + n2)) * 1.42188695 + 0.001054489;
        }


        public static double simplex_noise3D(double x, double y, double z, uint seed, interp_func interp)
        {
            //static double F3 = 1.0/3.0;
            //static double G3 = 1.0/6.0;
            double n0, n1, n2, n3;

            double s = (x + y + z) * F3;
            int i = fast_floor(x + s);
            int j = fast_floor(y + s);
            int k = fast_floor(z + s);

            double t = (i + j + k) * G3;
            double X0 = i - t;
            double Y0 = j - t;
            double Z0 = k - t;

            double x0 = x - X0;
            double y0 = y - Y0;
            double z0 = z - Z0;

            int i1, j1, k1;
            int i2, j2, k2;

            if (x0 >= y0)
            {
                if (y0 >= z0)
                {
                    i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                }
                else if (x0 >= z0)
                {
                    i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1;
                }
                else
                {
                    i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1;
                }
            }
            else
            {
                if (y0 < z0)
                {
                    i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1;
                }
                else if (x0 < z0)
                {
                    i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1;
                }
                else
                {
                    i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                }
            }

            double x1 = x0 - i1 + G3;
            double y1 = y0 - j1 + G3;
            double z1 = z0 - k1 + G3;
            double x2 = x0 - i2 + 2.0 * G3;
            double y2 = y0 - j2 + 2.0 * G3;
            double z2 = z0 - k2 + 2.0 * G3;
            double x3 = x0 - 1.0 + 3.0 * G3;
            double y3 = y0 - 1.0 + 3.0 * G3;
            double z3 = z0 - 1.0 + 3.0 * G3;

            uint h0, h1, h2, h3;

            h0 = Hashing.hash_coords_3(i, j, k, seed) % 12;
            h1 = Hashing.hash_coords_3(i + i1, j + j1, k + k1, seed) % 12;
            h2 = Hashing.hash_coords_3(i + i2, j + j2, k + k2, seed) % 12;
            h3 = Hashing.hash_coords_3(i + 1, j + 1, k + 1, seed) % 12;

            double[] g0 = Lut.gradient3D_lut[h0];
            double[] g1 = Lut.gradient3D_lut[h1];
            double[] g2 = Lut.gradient3D_lut[h2];
            double[] g3 = Lut.gradient3D_lut[h3];

            double t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0;
            if (t0 < 0.0) n0 = 0.0;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * array_dot3(g0, x0, y0, z0);
            }

            double t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
            if (t1 < 0.0) n1 = 0.0;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * array_dot3(g1, x1, y1, z1);
            }

            double t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
            if (t2 < 0) n2 = 0.0;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * array_dot3(g2, x2, y2, z2);
            }

            double t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
            if (t3 < 0) n3 = 0.0;
            else
            {
                t3 *= t3;
                n3 = t3 * t3 * array_dot3(g3, x3, y3, z3);
            }

            return (32.0 * (n0 + n1 + n2 + n3)) * 1.25086885 + 0.0003194984;
        }

        public static double simplex_noise4D(double x, double y, double z, double w, uint seed, interp_func interp)
        {
            int[,] simplex = new int[64, 4] {
    {0,1,2,3},{0,1,3,2},{0,0,0,0},{0,2,3,1},{0,0,0,0},{0,0,0,0},{0,0,0,0},{1,2,3,0},
    {0,2,1,3},{0,0,0,0},{0,3,1,2},{0,3,2,1},{0,0,0,0},{0,0,0,0},{0,0,0,0},{1,3,2,0},
    {0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},
    {1,2,0,3},{0,0,0,0},{1,3,0,2},{0,0,0,0},{0,0,0,0},{0,0,0,0},{2,3,0,1},{2,3,1,0},
    {1,0,2,3},{1,0,3,2},{0,0,0,0},{0,0,0,0},{0,0,0,0},{2,0,3,1},{0,0,0,0},{2,1,3,0},
    {0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},{0,0,0,0},
    {2,0,1,3},{0,0,0,0},{0,0,0,0},{0,0,0,0},{3,0,1,2},{3,0,2,1},{0,0,0,0},{3,1,2,0},
    {2,1,0,3},{0,0,0,0},{0,0,0,0},{0,0,0,0},{3,1,0,2},{0,0,0,0},{3,2,0,1},{3,2,1,0}};

            double F4 = (Math.Sqrt(5.0) - 1.0) / 4.0;
            double G4 = (5.0 - Math.Sqrt(5.0)) / 20.0;
            double n0, n1, n2, n3, n4; // Noise contributions from the five corners
                                       // Skew the (x,y,z,w) space to determine which cell of 24 simplices we're in
            double s = (x + y + z + w) * F4; // Factor for 4D skewing
            int i = fast_floor(x + s);
            int j = fast_floor(y + s);
            int k = fast_floor(z + s);
            int l = fast_floor(w + s);
            double t = (i + j + k + l) * G4; // Factor for 4D unskewing
            double X0 = i - t; // Unskew the cell origin back to (x,y,z,w) space
            double Y0 = j - t;
            double Z0 = k - t;
            double W0 = l - t;
            double x0 = x - X0; // The x,y,z,w distances from the cell origin
            double y0 = y - Y0;
            double z0 = z - Z0;
            double w0 = w - W0;
            // For the 4D case, the simplex is a 4D shape I won't even try to describe.
            // To find out which of the 24 possible simplices we're in, we need to
            // determine the magnitude ordering of x0, y0, z0 and w0.
            // The method below is a good way of finding the ordering of x,y,z,w and
            // then find the correct traversal order for the simplex we’re in.
            // First, six pair-wise comparisons are performed between each possible pair
            // of the four coordinates, and the results are used to add up binary bits
            // for an integer index.
            int c1 = (x0 > y0) ? 32 : 0;
            int c2 = (x0 > z0) ? 16 : 0;
            int c3 = (y0 > z0) ? 8 : 0;
            int c4 = (x0 > w0) ? 4 : 0;
            int c5 = (y0 > w0) ? 2 : 0;
            int c6 = (z0 > w0) ? 1 : 0;
            int c = c1 + c2 + c3 + c4 + c5 + c6;
            int i1, j1, k1, l1; // The integer offsets for the second simplex corner
            int i2, j2, k2, l2; // The integer offsets for the third simplex corner
            int i3, j3, k3, l3; // The integer offsets for the fourth simplex corner
                                // simplex[c] is a 4-vector with the numbers 0, 1, 2 and 3 in some order.
                                // Many values of c will never occur, since e.g. x>y>z>w makes x<z, y<w and x<w
                                // impossible. Only the 24 indices which have non-zero entries make any sense.
                                // We use a thresholding to set the coordinates in turn from the largest magnitude.
                                // The number 3 in the "simplex" array is at the position of the largest coordinate.
            i1 = simplex[c, 0] >= 3 ? 1 : 0;
            j1 = simplex[c, 1] >= 3 ? 1 : 0;
            k1 = simplex[c, 2] >= 3 ? 1 : 0;
            l1 = simplex[c, 3] >= 3 ? 1 : 0;
            // The number 2 in the "simplex" array is at the second largest coordinate.
            i2 = simplex[c, 0] >= 2 ? 1 : 0;
            j2 = simplex[c, 1] >= 2 ? 1 : 0;
            k2 = simplex[c, 2] >= 2 ? 1 : 0;
            l2 = simplex[c, 3] >= 2 ? 1 : 0;
            // The number 1 in the "simplex" array is at the second smallest coordinate.
            i3 = simplex[c, 0] >= 1 ? 1 : 0;
            j3 = simplex[c, 1] >= 1 ? 1 : 0;
            k3 = simplex[c, 2] >= 1 ? 1 : 0;
            l3 = simplex[c, 3] >= 1 ? 1 : 0;
            // The fifth corner has all coordinate offsets = 1, so no need to look that up.
            double x1 = x0 - i1 + G4; // Offsets for second corner in (x,y,z,w) coords
            double y1 = y0 - j1 + G4;
            double z1 = z0 - k1 + G4;
            double w1 = w0 - l1 + G4;
            double x2 = x0 - i2 + 2.0 * G4; // Offsets for third corner in (x,y,z,w) coords
            double y2 = y0 - j2 + 2.0 * G4;
            double z2 = z0 - k2 + 2.0 * G4;
            double w2 = w0 - l2 + 2.0 * G4;
            double x3 = x0 - i3 + 3.0 * G4; // Offsets for fourth corner in (x,y,z,w) coords
            double y3 = y0 - j3 + 3.0 * G4;
            double z3 = z0 - k3 + 3.0 * G4;
            double w3 = w0 - l3 + 3.0 * G4;
            double x4 = x0 - 1.0 + 4.0 * G4; // Offsets for last corner in (x,y,z,w) coords
            double y4 = y0 - 1.0 + 4.0 * G4;
            double z4 = z0 - 1.0 + 4.0 * G4;
            double w4 = w0 - 1.0 + 4.0 * G4;
            // Work out the hashed gradient indices of the five simplex corners
            uint h0, h1, h2, h3, h4;
            h0 = Hashing.hash_coords_4(i, j, k, l, seed) % 32;
            h1 = Hashing.hash_coords_4(i + i1, j + j1, k + k1, l + l1, seed) % 32;
            h2 = Hashing.hash_coords_4(i + i2, j + j2, k + k2, l + l2, seed) % 32;
            h3 = Hashing.hash_coords_4(i + i3, j + j3, k + k3, l + l3, seed) % 32;
            h4 = Hashing.hash_coords_4(i + 1, j + 1, k + 1, l + 1, seed) % 32;

            double[] g0 = Lut.gradient4D_lut[h0];
            double[] g1 = Lut.gradient4D_lut[h1];
            double[] g2 = Lut.gradient4D_lut[h2];
            double[] g3 = Lut.gradient4D_lut[h3];
            double[] g4 = Lut.gradient4D_lut[h4];


            // Calculate the contribution from the five corners
            double t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0 - w0 * w0;
            if (t0 < 0) n0 = 0.0;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * array_dot4(g0, x0, y0, z0, w0);
            }
            double t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1 - w1 * w1;
            if (t1 < 0) n1 = 0.0;
            else
            {
                t1 *= t1;
                n1 = t1 * t1 * array_dot4(g1, x1, y1, z1, w1);
            }
            double t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2 - w2 * w2;
            if (t2 < 0) n2 = 0.0;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * array_dot4(g2, x2, y2, z2, w2);
            }
            double t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3 - w3 * w3;
            if (t3 < 0) n3 = 0.0;
            else
            {
                t3 *= t3;
                n3 = t3 * t3 * array_dot4(g3, x3, y3, z3, w3);
            }
            double t4 = 0.6 - x4 * x4 - y4 * y4 - z4 * z4 - w4 * w4;
            if (t4 < 0) n4 = 0.0;
            else
            {
                t4 *= t4;
                n4 = t4 * t4 * array_dot4(g4, x4, y4, z4, w4);
            }
            // Sum up and scale the result to cover the range [-1,1]
            return 27.0 * (n0 + n1 + n2 + n3 + n4);
        }



        public struct SVectorOrdering4
        {
            SVectorOrdering4(double v, int X, int Y, int Z, int W)
            {
                coord = v;
                x = X; y = Y; z = Z; w = W;
            }
            public double coord;
            int x, y, z, w;
        };

        public static bool svec4Compare(SVectorOrdering4 v1, SVectorOrdering4 v2)
        {
            if (v1.coord > v2.coord) return true;
            return false;
        }

        public struct SVectorOrdering : System.Collections.Generic.IComparer<SVectorOrdering>
        {
            public SVectorOrdering(double v, int a)
            {
                val = v;
                axis = a;
            }
            public double val;
            public int axis;

            public int Compare(SVectorOrdering x, SVectorOrdering y)
            {
                return x.val.CompareTo(y.val);
            }
        };

        public static bool vectorOrderingCompare(SVectorOrdering v1, SVectorOrdering v2)
        {
            if (v1.val > v2.val) return true;
            return false;
        }

        public static void sortBy_4(ref double[] l1, ref int[] l2)
        {
            System.Collections.Generic.List<SVectorOrdering> a = new System.Collections.Generic.List<SVectorOrdering>();
            for (int c = 0; c < 4; ++c) {
                a.Add(new SVectorOrdering(l1[c], l2[c]));
            }
            a.Sort();
            for (int c = 0; c < 4; ++c) l2[c] = a[c].axis;
        }
        public static void sortBy_6(ref double[] l1, ref int[] l2)
        {
            System.Collections.Generic.List<SVectorOrdering> a = new System.Collections.Generic.List<SVectorOrdering>();
            for (int c = 0; c < 6; ++c) {
                a.Add(new SVectorOrdering(l1[c],l2[c]));
            }
            a.Sort();
            for (int c = 0; c < 6; ++c) l2[c] = a[c].axis;
        }

        public static double new_simplex_noise4D(double x, double y, double z, double w, uint seed, interp_func interp)
        {
            // f = ((self.d + 1) ** .5 - 1) / self.d
            double F4 = (Math.Sqrt(5.0) - 1.0) / 4.0;

            // g=self.f/(1+self.d*self.f)
            double G4 = F4 / (1.0 + 4.0 * F4);

            double sideLength = 2.0 / (4.0 * F4 + 1.0);
            double a = Math.Sqrt((sideLength * sideLength) - ((sideLength / 2.0) * (sideLength / 2.0)));
            double cornerToFace = Math.Sqrt((a * a + (a / 2.0) * (a / 2.0)));
            double cornerToFaceSquared = cornerToFace * cornerToFace;

            double valueScaler = Math.Pow(3.0, -0.5);
            // Rough estimated/expirmentally determined function
            // for scaling output to be -1 to 1
            valueScaler *= Math.Pow(3.0, -3.5) * 100.0 + 13.0;

            double[] loc = new double[4] { x, y, z, w };
            double s = 0;
            for (int c = 0; c < 4; ++c) s += loc[c];
            s *= F4;

            int[] skewLoc = new int[4] { anl.fast_floor(x + s), anl.fast_floor(y + s), anl.fast_floor(z + s), anl.fast_floor(w + s) };
            int[] intLoc = new int[4] { anl.fast_floor(x + s), anl.fast_floor(y + s), anl.fast_floor(z + s), anl.fast_floor(w + s) };
            double unskew = 0.0;
            for (int c = 0; c < 4; ++c) unskew += skewLoc[c];
            unskew *= G4;
            double[] cellDist = new double[4]{loc[0]-(double)skewLoc[0]+unskew, loc[1]-(double)skewLoc[1]+unskew,
        loc[2]-(double)skewLoc[2]+unskew, loc[3]-(double)skewLoc[3]+unskew};
            int[] distOrder = new int[4] { 0, 1, 2, 3 };
            sortBy_4(ref cellDist, ref distOrder);

            int[] newDistOrder = new int[5] { -1, distOrder[0], distOrder[1], distOrder[2], distOrder[3] };

            double n = 0.0;
            double skewOffset = 0.0;

            for (int c = 0; c < 5; ++c)
            {
                int i = newDistOrder[c];
                if (i != -1) intLoc[i] += 1;

                double[] u = new double[4];
                for (int d = 0; d < 4; ++d)
                {
                    u[d] = cellDist[d] - (intLoc[d] - skewLoc[d]) + skewOffset;
                }

                double t = cornerToFaceSquared;

                for (int d = 0; d < 4; ++d)
                {
                    t -= u[d] * u[d];
                }

                if (t > 0.0)
                {
                    uint h = Hashing.hash_coords_4(intLoc[0], intLoc[1], intLoc[2], intLoc[3], seed) % 32;
                    double[] vec = Lut.gradient4D_lut[h];
                    double gr = 0.0;
                    for (int d = 0; d < 4; ++d)
                    {
                        gr += vec[d] * u[d];
                    }

                    n += gr * t * t * t * t;
                }
                skewOffset += G4;
            }
            n *= valueScaler;
            return n;
        }



        public static double simplex_noise6D(double x, double y, double z, double w, double u, double v, uint seed, interp_func interp)
        {
            // Skew
            //self.f = ((self.d + 1) ** .5 - 1) / self.d

            double F4 = (Math.Sqrt(7.0) - 1.0) / 6.0; //(Math.Sqrt(5.0)-1.0)/4.0;

            // Unskew
            // self.g=self.f/(1+self.d*self.f)
            double G4 = F4 / (1.0 + 6.0 * F4);

            double sideLength = Math.Sqrt(6.0) / (6.0 * F4 + 1.0);
            double a = Math.Sqrt((sideLength * sideLength) - ((sideLength / 2.0) * (sideLength / 2.0)));
            double cornerFace = Math.Sqrt(a * a + (a / 2.0) * (a / 2.0));

            double cornerFaceSqrd = cornerFace * cornerFace;

            //self.valueScaler=(self.d-1)**-.5
            double valueScaler = Math.Pow(5.0, -0.5);
            valueScaler *= Math.Pow(5.0, -3.5) * 100 + 13;

            double[] loc = new double[6] { x, y, z, w, u, v };
            double s = 0;
            for (int c = 0; c < 6; ++c) s += loc[c];
            s *= F4;

            int[] skewLoc = new int[6] { fast_floor(x + s), fast_floor(y + s), fast_floor(z + s), fast_floor(w + s), fast_floor(u + s), fast_floor(v + s) };
            int[] intLoc = new int[6] { fast_floor(x + s), fast_floor(y + s), fast_floor(z + s), fast_floor(w + s), fast_floor(u + s), fast_floor(v + s) };
            double unskew = 0.0;
            for (int c = 0; c < 6; ++c) unskew += skewLoc[c];
            unskew *= G4;
            double[] cellDist = new double[6] {
                loc[0]-(double)skewLoc[0]+unskew, loc[1]-(double)skewLoc[1]+unskew,
                loc[2]-(double)skewLoc[2]+unskew, loc[3]-(double)skewLoc[3]+unskew,
                loc[4]-(double)skewLoc[4]+unskew, loc[5]-(double)skewLoc[5]+unskew};
            int[] distOrder = new int[6] { 0, 1, 2, 3, 4, 5 };
            sortBy_6(ref cellDist, ref distOrder);

            int[] newDistOrder = new int[7] { -1, distOrder[0], distOrder[1], distOrder[2], distOrder[3], distOrder[4], distOrder[5] };

            double n = 0.0;
            double skewOffset = 0.0;

            for (int c = 0; c < 7; ++c)
            {
                int i = newDistOrder[c];
                if (i != -1) intLoc[i] += 1;

                double[] u1 = new double[6];
                for (int d = 0; d < 6; ++d)
                {
                    u1[d] = cellDist[d] - (intLoc[d] - skewLoc[d]) + skewOffset;
                }

                double t = cornerFaceSqrd;

                for (int d = 0; d < 6; ++d)
                {
                    t -= u1[d] * u1[d];
                }

                if (t > 0.0)
                {
                    uint h = Hashing.hash_coords_6(intLoc[0], intLoc[1], intLoc[2], intLoc[3], intLoc[4], intLoc[5], seed) % 192;
                    double[] vec = Lut.gradient6D_lut[h];
                    double gr = 0.0;
                    for (int d = 0; d < 6; ++d)
                    {
                        gr += vec[d] * u1[d];
                    }

                    n += gr * t * t * t * t;
                }
                skewOffset += G4;
            }
            n *= valueScaler;
            return n;
        }
    }
}
