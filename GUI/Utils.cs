﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public static class Utils//Encapsula metodos auxiliares
    {
        public static Stack<Brush> COLORS = new Stack<Brush>();

        public static Point[] GetIntersectionPoints(Geometry g1, Geometry g2)//Hallar interseccion entre dos Geometrys
        {
            Geometry og1 = g1.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));
            Geometry og2 = g2.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));

            CombinedGeometry cg = new CombinedGeometry(GeometryCombineMode.Intersect, og1, og2);

            PathGeometry pg = cg.GetFlattenedPathGeometry();
            Point[] result = new Point[pg.Figures.Count];
            for (int i = 0; i < pg.Figures.Count; i++)
            {
                Rect fig = new PathGeometry(new PathFigure[] { pg.Figures[i] }).Bounds;
                result[i] = new Point(fig.Left + fig.Width / 2.0, fig.Top + fig.Height / 2.0);
            }
            return result;
        }
        public static Geometry PointToGeometry(Point point)//poder tartar los puntos como figuras geometricas interceptables y dibujables
        {
            EllipseGeometry representation = new EllipseGeometry();
            representation.Center = point;
            representation.RadiusX = 4.99;
            representation.RadiusY = 4.99;

            return representation;
        }
        public static int Measure(Point point1, Point point2)//distancia entre dos puntos
        {
            int measure = Convert.ToInt32(Math.Sqrt(Math.Pow(point2.Y - point1.Y, 2) + Math.Pow(point2.X - point1.X, 2)));

            return measure;
        }
        public static void SelectColor(string color)
        {
            //Posibles colores
            switch (color)
            {
                case "blue": COLORS.Push(Brushes.Blue); break;
                case "red": COLORS.Push(Brushes.Red); break;
                case "black": COLORS.Push(Brushes.Black); break;
                case "yellow": COLORS.Push(Brushes.Yellow); break;
                case "cyan": COLORS.Push(Brushes.Cyan); break;
                case "gray": COLORS.Push(Brushes.Gray); break;
                case "white": COLORS.Push(Brushes.White); break;
                case "gren": COLORS.Push(Brushes.Green); break;
                case "magenta": COLORS.Push(Brushes.Magenta); break;

                default: MessageBox.Show("The color selected is not a valid color brush.");break;
            }

        }
        public static void RestoreColor()//go backward in colors
        {
            if (COLORS.Count > 1)
            {
                COLORS.Pop();
            }
        }
        public static IEnumerable<Point> Points(Geometry figure)//puntos aleatorios de f(figure)
        {
            if (figure is LineGeometry line)
            {
                double m = (line.EndPoint.Y - line.StartPoint.Y) / (line.EndPoint.X - line.StartPoint.X); // Calcula la pendiente
                double b = line.StartPoint.Y - (m * line.StartPoint.X); // Calcula el intercepto
               
                    for (int i = 0; i < 10; i++)
                    {
                    double x = Random.Shared.NextDouble() * 10; // Genera un número aleatorio entre el inicio y el fin de la línea
                        double y = m * x + b;
                        yield return new Point(x, y);
                    }               
            }
            else if (figure is PathGeometry arc)
            {
                //Es un arco
            }
            else if (figure is EllipseGeometry circle) //Es un circulo
            {
                if (circle.RadiusX == 4.99)//Descartar que sea la representacion geometrica de un punto
                {
                    yield return circle.Center;
                }
                else 
                { 
                   for (int i = 0; i < 10; i++)//Puntos aleatorios de la circunferencia
                   {
                        double angle = Random.Shared.NextDouble() * 2 * Math.PI; // Genera un ángulo aleatorio entre 0 y 2π
                        double x = circle.Center.X + circle.RadiusX * Math.Cos(angle); // Calcula la coordenada x del punto
                        double y = circle.Center.Y + circle.RadiusX * Math.Sin(angle); // 
                        yield return new Point(x, y);
                   }
                }
            }
           
        }
        public static double[] Randoms()//Puntos aleatorios entre cero y uno 
        {
            int size = Random.Shared.Next(2,10);
            double[] result = new double[size];

            int c = 0;
            while (c < size)
            {
                result[c] = Random.Shared.NextDouble();
                c++;
            }
            return result;
        }
        public static void SavePath(Path path, string fileName)//Serializar la figura para poder ser representada posteriormente
        {
            // Define la ruta del archivo
            string filePath = $"C:\\Users\\javie\\OneDrive\\Documentos\\GitHub\\geo_walle\\GUI\\Serials\\{fileName}.xaml";

            // Crea un nuevo archivo
            using (var file = System.IO.File.Create(filePath)) { }

            // Guarda el objeto Path en el archivo
            var xaml = XamlWriter.Save(path);
            System.IO.File.WriteAllText(filePath, xaml);
        }
        public static void LoadAllPaths(Canvas myCanvas)//deserializar 
        {
            string directoryPath = "C:\\Users\\javie\\OneDrive\\Documentos\\GitHub\\geo_walle\\GUI\\Serials\\";
            // Obtiene todos los archivos XAML
            string[] filePaths = System.IO.Directory.GetFiles(directoryPath, "*.xaml");

            // Deserializa y añade cada Path al Canvas
            foreach (string filePath in filePaths)
            {
                var xaml = System.IO.File.ReadAllText(filePath);
                Path path = (Path)XamlReader.Parse(xaml);
                myCanvas.Children.Add(path);
            }
        }
        public static void ClearSerials()//eliminar los archivos de el anterior Run
        {
            string directoryPath = "C:\\Users\\javie\\OneDrive\\Documentos\\GitHub\\geo_walle\\GUI\\Serials\\";
            string[] filePaths = System.IO.Directory.GetFiles(directoryPath, "*.xaml");

            foreach (var filepath in filePaths)
            {
                System.IO.File.Delete(filepath);
            }
        }
    }
}
