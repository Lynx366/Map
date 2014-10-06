using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Generator;

namespace MapViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Generator.Map Collection;
        int view = 0;
        public MainWindow()
        {
            InitializeComponent();
            size.Text = "512";
            seed.Text = GenerateSeed().ToString();
            iter.Text = "6";
            water.Text = "15";
            mountain.Text = "15";
            riverCount.Text = "10";
            Collection = new Generator.Map();
        }

        private void Show(double [,] Map)
        {
            int mapSize = Map.GetLength(0);
            Bitmap bmp = new Bitmap(mapSize, mapSize);
            for (int i = 0; i < mapSize; i++)
            {
                for (int k = 0; k < mapSize; k++)
                {
                    byte value = (byte)(255 * Map[i,k]);
                    bmp.SetPixel(i,k, System.Drawing.Color.FromArgb(255, value, value, value));
                }
            }
            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                              bmp.GetHbitmap(),
                              IntPtr.Zero,
                              System.Windows.Int32Rect.Empty,
                              BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            RenderOptions.SetBitmapScalingMode(Img, BitmapScalingMode.NearestNeighbor);
            Img.Height = mapSize * Convert.ToInt32(zoom.Text) / 100;
            Img.Width = mapSize * Convert.ToInt32(zoom.Text) / 100;
            Img.Source = bs;
        }

        private void generate(object sender, RoutedEventArgs e)
        {
            Generator.Map result = MapCreator.Create(Convert.ToInt32(seed.Text), Convert.ToInt32(size.Text), Convert.ToDouble(water.Text), Convert.ToDouble(mountain.Text), Convert.ToInt32(iter.Text),Convert.ToInt32(riverCount.Text));
            Collection = result;
            switch(view)
            {
                case 0: Show(Collection.map);
                    break;
                case 1: Show(Collection.digitalized);
                    break;
            }
        }
        private int GenerateSeed()
        {
            Random rnd = new Random();
            return rnd.Next();
        }

        private void zoomChange(object sender, RoutedEventArgs e)
        {
            if(Collection!=null)
                switch (view)
                {
                    case 0: Show(Collection.map);
                        break;
                    case 1: Show(Collection.digitalized);
                        break;
                    case 2: Show(Collection.smoothMap);
                        break;
                }
        }
        private void seedButton(object sender, RoutedEventArgs e)
        {
            seed.Text = GenerateSeed().ToString();
        }

        private void optimalIterations(object sender, RoutedEventArgs e)
        {
            int _size = Convert.ToInt32(size.Text);
            int result = (int)Math.Log(_size / 8, 2);
            iter.Text = result.ToString();
        }
        private void chose_2(object sender, RoutedEventArgs e)
        {
            Show(Collection.digitalized);
            view = 1;
        }
        private void chose_1(object sender, RoutedEventArgs e)
        {
            Show(Collection.map);
            view = 0;
        }
        private void chose_3(object sender, RoutedEventArgs e)
        {
            Show(Collection.smoothMap);
            view = 2;
        }


        private void colors(object sender, RoutedEventArgs e)
        {
            ShowWithColors(Collection.map,Collection.digitalized);
        }

        private void ShowWithColors(double[,] heightMap, double[,] digitalized)
        {
            int mapSize = heightMap.GetLength(0);
            double grassSpan = Collection.mountainLevel - Collection.waterLevel;
            Bitmap bmp = new Bitmap(mapSize, mapSize);
            for (int i = 0; i < mapSize; i++)
            {
                for (int k = 0; k < mapSize; k++)
                {
                    System.Drawing.Color pixel = new System.Drawing.Color();
                    switch(digitalized[i,k].ToString())
                    {
                        case "1":
                            byte value = (byte)(50 + ((heightMap[i, k]-Collection.mountainLevel) / (1-Collection.mountainLevel))*155);
                            pixel = System.Drawing.Color.FromArgb(255, value, value, value);
                            break;
                        case "0.5":
                            pixel = System.Drawing.Color.FromArgb(255, (byte)(100 + ((heightMap[i, k] - Collection.mountainLevel) / (Collection.mountainLevel - Collection.waterLevel)) * 78), (byte)(227 + ((heightMap[i, k] - Collection.mountainLevel) / (Collection.mountainLevel - Collection.waterLevel)) * 150), 0);
                            break;
                        case "0":
                            byte green1 = (byte)((heightMap[i, k] / Collection.waterLevel) * 200);
                            byte blue = (byte)(205 + (heightMap[i, k] / Collection.waterLevel) * 50);
                            pixel = System.Drawing.Color.FromArgb(255, 0, green1, blue);
                            break;                    
                    }                    
                    bmp.SetPixel(i, k, pixel);
                }
            }
            foreach (List<RiverPoint> singleRiver in Collection.AllRivers)
                foreach (RiverPoint point in singleRiver)
                {
                    for (int i = point.x; i <= point.x; i++)
                        for (int k = point.y; k <= point.y; k++)
                            if (i >= 0 && i < Collection.size && k >= 0 && k < Collection.size)
                                bmp.SetPixel(i, k, System.Drawing.Color.Violet);
                }
            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                              bmp.GetHbitmap(),
                              IntPtr.Zero,
                              System.Windows.Int32Rect.Empty,
                              BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            RenderOptions.SetBitmapScalingMode(Img, BitmapScalingMode.NearestNeighbor);
            Img.Height = mapSize * Convert.ToInt32(zoom.Text) / 100;
            Img.Width = mapSize * Convert.ToInt32(zoom.Text) / 100;
            Img.Source = bs;
        }

       

    }
}
