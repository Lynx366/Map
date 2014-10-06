using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Generator
{
    public static class NoiseGenerator
    {
        public static double[,] Generate(int size, int seed)
        {
            Random rnd = new Random(seed);
            double[,] Map = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int k = 0; k < size; k++ )
                {
                    Map[i, k] = rnd.Next(0,2);
                }
                return Map;
        }
        public static double[,] getRandomScrap(double[,] Map, int level, int seed)
        {
            int size = (int)(Map.GetLength(0)/Math.Pow(2,level));
            double[,] scrapMap = new double[size, size];
            Random rnd = new Random(seed+level*2);
            int startingPoint = rnd.Next(0, Map.GetLength(0)/2);
            for (int i = 0; i < size; i++)
                for (int k = 0; k < size; k++)
                {
                    scrapMap[i, k] = Map[startingPoint + i, startingPoint + k];
                }

            return scrapMap;
        }
        public static List<MapClass> GenerateNoise(int size, int seed,int iterations)
        {
            int smoothPoint = 2;
            int cropping = (int)(size/10);
            size += cropping;
            List<MapClass> mapsList = new List<MapClass>();
            MapClass map = new MapClass(size);
            map.Map = Generate(size, seed);
            mapsList.Add(map);
            for(int i=2;i<iterations+2;i++)
            {
                MapClass temp = new MapClass(size);
                temp.Map = Interpolation(size,mapsList[0].Map,i,seed);
                mapsList.Add(temp);
            }
            MapClass Noise = new MapClass(size);
            MapClass SmoothNoise = new MapClass(size);
            for(int i = 0;i<size;i++)
                for(int k = 0;k<size;k++)
                {
                    double weight = 0.5;
                    for(int j = mapsList.Count-1;j>=0;j--)
                    {
                        Noise.Map[i, k] = Noise.Map[i, k] + (mapsList[j].Map[i, k] * weight);
                        weight /= 2;
                        if (j == mapsList.Count-1-smoothPoint)
                            SmoothNoise.Map[i,k] = Noise.Map[i,k];
                    }
                }    
            Noise.Map = Cropping(Noise.Map, size - cropping);
            SmoothNoise.Map = Cropping(SmoothNoise.Map, size - cropping);
            List<MapClass> result = new List<MapClass>();
            result.Add(SmoothNoise);
            result.Add(Noise);
            return result;
        }
        public static double[,] Cropping(double[,] _map, int size)
        {
            double[,] map = new double[size, size];
            for(int i = 0;i<size;i++)
                for(int k = 0;k<size;k++)
                {
                    map[i, k] = _map[i, k];
                }
            return map;
        }
        public static Bitmap ArrayToBitmap(double[,] _map)
        {
            int size = _map.GetLength(0);
            Bitmap bmp = new Bitmap(size, size);
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    byte value = (byte)(255 * _map[i, k]);
                    bmp.SetPixel(k, i, System.Drawing.Color.FromArgb(255, value, value, value));
                }
            }
            return bmp;
        }
        public static double[,] BitmapToArray(Bitmap bmp)
        {
            int size = bmp.Height;
            double[,] map = new double[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    map[i, k] = (double)(bmp.GetPixel(i, k)).B / 255;
                }
            }
            return map;
        }
        public static double[,] Interpolation(int size, double[,] _map, int level,int seed)
        {
            double[,] interpolatedMap = new double[size, size];
            double[,] scrap = getRandomScrap(_map, level, seed);
            int scrapSize = scrap.GetLength(0);
            Bitmap bmp = new Bitmap(scrapSize, scrapSize);
            bmp = ArrayToBitmap(scrap);
            float scale = size / scrapSize;
            var interpolated = new Bitmap((int)size, (int)size);
            var graph = Graphics.FromImage(interpolated);

            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var scaleWidth = (int)(bmp.Width * scale);
            var scaleHeight = (int)(bmp.Height * scale);
            var brush = new SolidBrush(Color.Black);
            graph.FillRectangle(brush, new RectangleF(0, 0, size, size));
            graph.DrawImage(bmp, new Rectangle(0, 0, size,size));

            return BitmapToArray(interpolated);
        }
    }
}
