using AffineTransformationsIn3D.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AffineTransformationsIn3D.Geometry
{
    public static class Models
    {
        public static Mesh Sphere(double diameter, int slices, int stacks)
        {
            var radius = diameter / 2;
            var vertices = new Vector[slices * stacks];
            var normals = new Vector[slices * stacks];
            var indices = new int[slices * (stacks - 1)][];
            for (int stack = 0; stack < stacks; ++stack)
                for (int slice = 0; slice < slices; ++slice)
                {
                    var theta = Math.PI * stack / (stacks - 1.0);
                    var phi = 2 * Math.PI * (slice / (slices - 1.0));
                    vertices[stack * slices + slice] = new Vector(
                        radius * Math.Sin(theta) * Math.Cos(phi),
                        radius * Math.Sin(theta) * Math.Sin(phi),
                        radius * Math.Cos(theta));
                    normals[stack * slices + slice] = vertices[stack * slices + slice].Normalize();
                }
            for (int stack = 0; stack < stacks - 1; ++stack)
                for (int slice = 0; slice < slices; ++slice)
                    indices[stack * slices + slice] = new int[4] {
                        stack * slices + ((slice + 1) % slices),
                        (stack + 1) * slices + ((slice + 1) % slices),
                        (stack + 1) * slices + slice,
                        stack * slices + slice,};
            return new MeshWithNormals(vertices, normals, indices);
        }

        public static Mesh Cube(double size)
        {
            //double s = size / 2;
            //return new Mesh(new Vector[8]
            //    {
            //        new Vector(-s, -s, -s),
            //        new Vector(-s, s, -s),
            //        new Vector(s, s, -s),
            //        new Vector(s, -s, -s),
            //        new Vector(-s, -s, s),
            //        new Vector(-s, s, s),
            //        new Vector(s, s, s),
            //        new Vector(s, -s, s)
            //    }, new int[6][]
            //    {
            //        new int[4] { 3, 2, 1, 0 },
            //        new int[4] { 0, 1, 5, 4 },
            //        new int[4] { 1, 2, 6, 5 },
            //        new int[4] { 2, 3, 7, 6 },
            //        new int[4] { 3, 0, 4, 7 },
            //        new int[4] { 4, 5, 6, 7 }
            //    });

            double s = size / 2;
            return new MeshWithTexture(Resources.ImageTexture,
                new Vector[24]
                {
                    new Vector(-s, -s, s),
                    new Vector(s, -s, s),
                    new Vector(s, s, s),
                    new Vector(-s, s, s),

                    new Vector(s, -s, s),
                    new Vector(s, -s, -s),
                    new Vector(s, s, -s),
                    new Vector(s, s, s),

                    new Vector(s, -s, -s),
                    new Vector(-s, -s, -s),
                    new Vector(-s, s, -s),
                    new Vector(s, s, -s),

                    new Vector(-s, -s, -s),
                    new Vector(-s, -s, s),
                    new Vector(-s, s, s),
                    new Vector(-s, s, -s),

                    new Vector(-s, -s, -s),
                    new Vector(s, -s, -s),
                    new Vector(s, -s, s),
                    new Vector(-s, -s, s),

                    new Vector(-s, s, s),
                    new Vector(s, s, s),
                    new Vector(s, s, -s),
                    new Vector(-s, s, -s),
                },
                new Vector[24]
                {
                    new Vector(0, 0),
                    new Vector(1, 0),
                    new Vector(1, 1),
                    new Vector(0, 1),

                    new Vector(0, 0),
                    new Vector(1, 0),
                    new Vector(1, 1),
                    new Vector(0, 1),

                    new Vector(0, 0),
                    new Vector(1, 0),
                    new Vector(1, 1),
                    new Vector(0, 1),

                    new Vector(0, 0),
                    new Vector(1, 0),
                    new Vector(1, 1),
                    new Vector(0, 1),

                    new Vector(0, 0),
                    new Vector(1, 0),
                    new Vector(1, 1),
                    new Vector(0, 1),

                    new Vector(0, 0),
                    new Vector(1, 0),
                    new Vector(1, 1),
                    new Vector(0, 1),
                },
                new int[6][]
                {
                   new int[4] { 0, 3, 2, 1 },
                   new int[4] { 4, 7, 6, 5 },
                   new int[4] { 8, 11, 10, 9 },
                   new int[4] { 12, 15, 14, 13 },
                   new int[4] { 16, 19, 18, 17 },
                   new int[4] { 20, 23, 22, 21 },
                });
        }

        public static Mesh Icosahedron(double size)
        {
            var vertices = new Vector[12];
            var indices = new int[20][];
            double R = (size * Math.Sqrt(2.0 * (5.0 + Math.Sqrt(5.0)))) / 4;
            double r = (size * Math.Sqrt(3.0) * (3.0 + Math.Sqrt(5.0))) / 12;
            for (int i = 0; i < 5; ++i)
            {
                vertices[2 * i] = new Vector(
                    r * Math.Cos(2 * Math.PI / 5 * i),
                    R / 2,
                    r * Math.Sin(2 * Math.PI / 5 * i));
                vertices[2 * i + 1] = new Vector(
                    r * Math.Cos(2 * Math.PI / 5 * i + 2 * Math.PI / 10),
                    -R / 2,
                    r * Math.Sin(2 * Math.PI / 5 * i + 2 * Math.PI / 10));
            }
            vertices[10] = new Vector(0, R, 0);
            vertices[11] = new Vector(0, -R, 0);
            for (int i = 0; i < 10; i += 2)
                indices[i] = new int[3] { (i + 1) % 10, (i + 2) % 10, i };
            for (int i = 1; i < 10; i += 2)
                indices[i] = new int[3] { (i + 1) % 10, i, (i + 2) % 10 };
            for (int i = 0; i < 5; ++i)
            {
                indices[10 + 2 * i] = new int[3] { 10, 2 * i, (2 * (i + 1)) % 10 };
                indices[10 + 2 * i + 1] = new int[3] { 11, (2 * (i + 1) + 1) % 10, 2 * i + 1 };
            }
            return new Mesh(vertices, indices);
        }

        public static Mesh Tetrahedron(double size)
        {
            var vertices = new Vector[4];
            var indices = new int[4][];
            double h = Math.Sqrt(2.0 / 3.0) * size;
            vertices[0] = new Vector(-size / 2, 0, h / 3);
            vertices[1] = new Vector(0, 0, -h * 2 / 3);
            vertices[2] = new Vector(size / 2, 0, h / 3);
            vertices[3] = new Vector(0, h, 0);
            indices[0] = new int[3] { 0, 2, 1 };
            indices[1] = new int[3] { 1, 3, 0 };
            indices[2] = new int[3] { 0, 3, 2 };
            indices[3] = new int[3] { 2, 3, 1 };
            return new Mesh(vertices, indices);
        }

        public static Mesh Plot(
            double x0, double x1, double dx, double z0, double z1, double dz,
            Func<double, double, double> function, double AngleX = Math.PI/4,
			double AngleY = Math.PI / 2, double AngleZ = Math.PI / 4)
        {
            int nx = (int)((x1 - x0) / dx);
            int nz = (int)((z1 - z0) / dz);
            var vertices = new Vector[nx * nz];
            var indices = new int[(nx - 1) * (nz - 1)][];
            for (int i = 0; i < nx; ++i)
                for (int j = 0; j < nz; ++j)
                {
                    var x = x0 + dx * i;
                    var z = z0 + dz * j;
                    vertices[i * nz + j] = new Vector(x, function(x, z), z);
                }
            for (int i = 0; i < nx - 1; ++i)
                for (int j = 0; j < nz - 1; j++)
                {
                    indices[i * (nz - 1) + j] = new int[4] {
                        i * nz + j,
                        (i + 1) * nz + j,
                        (i + 1) * nz + j + 1,
                        i * nz + j + 1
                    };
                }

			Mesh m = new Mesh(vertices, indices);

			m.Apply(Transformations.RotateX(-AngleX) *
					Transformations.RotateY(0) *
					Transformations.RotateZ(0));

			m.Apply(Transformations.RotateX(0) *
					Transformations.RotateY(0) *
					Transformations.RotateZ(-AngleZ));
			
			m.Apply(Transformations.RotateX(0) *
					Transformations.RotateY(-AngleY) *
					Transformations.RotateZ(0));

			int[][] del_y = DelBadY(vertices, indices, nz, nx);

			m = new Mesh(vertices, del_y);


			m.Apply(Transformations.RotateX(0) *
					Transformations.RotateY(AngleY) *
					Transformations.RotateZ(0));

			m.Apply(Transformations.RotateX(0) *
					Transformations.RotateY(0) *
					Transformations.RotateZ(AngleZ));

			m.Apply(Transformations.RotateX(AngleX) *
					Transformations.RotateY(0) *
					Transformations.RotateZ(0));

			return m;
        }

        private static int[][] DelBadY(Vector[] vertices, int[][] indices, int nz, int nx)
        {
            List<int[]> res_indices = new List<int[]>();

            for (int i = 0; i < nx - 1; ++i)
            {
                double y_max = double.MinValue;
                double y_min = double.MaxValue;
                for (int j = 0; j < i; ++j)
                {
                    if ((vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][0]].Y > y_max ||
                        vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][2]].Y > y_max) &&
                        (vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][1]].Y > y_max ||
                        vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][3]].Y > y_max))
                    {
                        int [] res = new int[2];
                        res[0] = indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][0];
                        res[1] = indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][3];

                        //res_indices.Add(indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)]);
                        res_indices.Add(res);
                        y_max = Math.Max(Math.Max(vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][0]].Y,
                                                  vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][1]].Y),
                                         Math.Max(vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][2]].Y,
                                                  vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][3]].Y));
                    }

                    if ((vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][0]].Y < y_min ||
                        vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][2]].Y < y_min )&&
                        (vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][1]].Y < y_min ||
                        vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][3]].Y < y_min))
                    {
                        int[] res = new int[2];
                        res[0] = indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][0];
                        res[1] = indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][3];

                        //res_indices.Add(indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)]);
                        res_indices.Add(res);
                        y_min = Math.Min(Math.Min(vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][0]].Y,
                                                  vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][1]].Y),
                                         Math.Min(vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][2]].Y,
                                                  vertices[indices[(nz - (2 + j)) * (nz - 1) + (i - j - 1)][3]].Y));
                    }
                }
            }


            for (int i = 0; i < nx - 1; ++i)
            {
                double y_max = double.MinValue;
                double y_min = double.MaxValue;
                for (int j = i; j >= 0; --j)
                {
                    if ((vertices[indices[(j + 1) * nx - (i + 2)][0]].Y > y_max ||
                        vertices[indices[(j + 1) * nx - (i + 2)][2]].Y > y_max) &&
                        (vertices[indices[(j + 1) * nx - (i + 2)][1]].Y > y_max ||
                        vertices[indices[(j + 1) * nx - (i + 2)][3]].Y > y_max))
                    {
                        int[] res = new int[2];
                        res[0] = indices[(j + 1) * nx - (i + 2)][0];
                        res[1] = indices[(j + 1) * nx - (i + 2)][3];

                        //res_indices.Add(indices[(j + 1) * nx - (i + 2)]);

                        res_indices.Add(res);
                        y_max = Math.Max(Math.Max(vertices[indices[(j + 1) * nx - (i + 2)][0]].Y,
                                                  vertices[indices[(j + 1) * nx - (i + 2)][1]].Y),
                                         Math.Max(vertices[indices[(j + 1) * nx - (i + 2)][2]].Y,
                                                  vertices[indices[(j + 1) * nx - (i + 2)][3]].Y));
                    }

                    if ((vertices[indices[(j + 1) * nx - (i + 2)][0]].Y < y_min ||
                        vertices[indices[(j + 1) * nx - (i + 2)][2]].Y < y_min) &&
                        (vertices[indices[(j + 1) * nx - (i + 2)][1]].Y < y_min ||
                        vertices[indices[(j + 1) * nx - (i + 2)][3]].Y < y_min))
                    {
                        int[] res = new int[2];
                        res[0] = indices[(j + 1) * nx - (i + 2)][0];
                        res[1] = indices[(j + 1) * nx - (i + 2)][3];

                        //res_indices.Add(indices[(j + 1) * nx - (i + 2)]);

                        res_indices.Add(res);
                        y_min = Math.Min(Math.Min(vertices[indices[(j + 1) * nx - (i + 2)][0]].Y,
                                                  vertices[indices[(j + 1) * nx - (i + 2)][1]].Y),
                                         Math.Min(vertices[indices[(j + 1) * nx - (i + 2)][2]].Y,
                                                  vertices[indices[(j + 1) * nx - (i + 2)][3]].Y));
                    }
                }
            }
            return res_indices.ToArray();
        }


        public static Mesh RotationFigure(IList<Vector> initial, int axis, int density)
        {
            Debug.Assert(0 <= axis && axis < 3);
            var n = initial.Count;
            var vertices = new Vector[n * density];
            var indices = new int[density * (n - 1)][];
            Func<double, Matrix> rotation;
            switch (axis)
            {
                case 0: rotation = Transformations.RotateX; break;
                case 1: rotation = Transformations.RotateY; break;
                default: rotation = Transformations.RotateZ; break;
            }
            for (int i = 0; i < density; ++i)
                for (int j = 0; j < n; ++j)
                    vertices[i * n + j] = initial[j] * rotation(2 * Math.PI * i / density);
            for (int i = 0; i < density; ++i)
                for (int j = 0; j < n - 1; ++j)
                    indices[i * (n - 1) + j] = new int[4] {
                        i * n + j + 1,
                        (i + 1) % density * n + j + 1,
                        (i + 1) % density * n + j,
                        i * n + j, };
            return new Mesh(vertices, indices);
        }
    }
}
