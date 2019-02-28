using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Myo_Wpf.Cube
{
    public class CubeBuilder
    {
        public static Model3DGroup CreateCubeGroup(int width, int height, int depth)
        {
            Model3DGroup cube = new Model3DGroup();
            Point3D p0 = new Point3D(width / -2.0, height / -2.0, depth / -2.0);
            Point3D p1 = new Point3D(p0.X + width, p0.Y, p0.Z);
            Point3D p2 = new Point3D(p0.X + width, p0.Y, p0.Z + depth);
            Point3D p3 = new Point3D(p0.X, p0.Y, p0.Z + depth);
            Point3D p4 = new Point3D(p0.X, p0.Y + height, p0.Z);
            Point3D p5 = new Point3D(p0.X + width, p0.Y + height, p0.Z);
            Point3D p6 = new Point3D(p0.X + width, p0.Y + height, p0.Z + depth);
            Point3D p7 = new Point3D(p0.X, p0.Y + height, p0.Z + depth);
            //front side triangles
            cube.Children.Add(CreateTriangleModel(p3, p2, p6, Colors.Red));
            cube.Children.Add(CreateTriangleModel(p3, p6, p7, Colors.Red));
            //back side triangles
            cube.Children.Add(CreateTriangleModel(p1, p0, p4, Colors.Red));
            cube.Children.Add(CreateTriangleModel(p1, p4, p5, Colors.Red));
            //left side triangles
            cube.Children.Add(CreateTriangleModel(p0, p3, p7, Colors.Green));
            cube.Children.Add(CreateTriangleModel(p0, p7, p4, Colors.Green));
            //right side triangles
            cube.Children.Add(CreateTriangleModel(p2, p1, p5, Colors.Green));
            cube.Children.Add(CreateTriangleModel(p2, p5, p6, Colors.Green));
            //top side triangles
            cube.Children.Add(CreateTriangleModel(p7, p6, p5, Colors.Blue));
            cube.Children.Add(CreateTriangleModel(p7, p5, p4, Colors.Blue));
            //bottom side triangles
            cube.Children.Add(CreateTriangleModel(p2, p3, p0, Colors.Blue));
            cube.Children.Add(CreateTriangleModel(p2, p0, p1, Colors.Blue));


            //< DirectionalLight Color = "Gray" Direction = "1, -2, -3" />
            cube.Children.Add(new DirectionalLight { Color = Colors.White, Direction = new Vector3D(1, -2, -3) });
            cube.Children.Add(new DirectionalLight { Color = Colors.Green, Direction = new Vector3D(-1, 2, 3) });

            return cube;
        }

        private static Model3DGroup CreateTriangleModel(Point3D p0, Point3D p1, Point3D p2, Color color)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);

            for (int i = 0; i < 3; i++)
            {
                mesh.TriangleIndices.Add(i);
            }

            //Vector3D normal = CalculateNormal(p0, p1, p2);

            //for (int i = 0; i < 3; i++)
            //{
            //    mesh.Normals.Add(normal);
            //}

            Material material = new DiffuseMaterial(new SolidColorBrush(color));
            GeometryModel3D model = new GeometryModel3D(mesh, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }

        private static Vector3D CalculateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            var dir = Vector3D.CrossProduct(p1 - p0, p2 - p0);
            dir.Normalize();
            return dir;
        }
    }
}
