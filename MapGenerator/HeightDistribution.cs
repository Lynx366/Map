using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public static class HeightDistribution
    {
        /// <summary>Calculates distributions of heights in map
        /// <para>_map - map to be distributed</para>
        /// <para>precission - precision of distributing map, default is 255</para>
        /// <para>returns array of distributions in _map</para>
        /// </summary>
        public static int[] calculateDistribution(double[,] _map, int precission = 255)
        {
            int size = _map.GetLength(0);
            int[] distribution = new int[precission+1];
            for (int i = 0; i < size; i++)
                for (int k=0;k<size;k++)
                {
                    int height = (int)(_map[i, k] * precission);
                    distribution[height] += 1;
                }
            return distribution;
        }
        public static double calculateWaterLevel(int[] distribution,double waterPercentage, int precission = 255)
        {
            int numberOfTiles = 0;
            foreach(int level in distribution)
            {
                numberOfTiles += level;
            }
            int waterTiles = (int)(waterPercentage * numberOfTiles);
            int tiles=0;
            int i=0;
            while (tiles < waterTiles) 
            {
                tiles += distribution[i];
                if (i < distribution.Length - 2)
                    i++;
                else
                    break;
            }
            return (double)i/(double)precission;
        }
        public static double calculateMountainLevel(int[] distribution, double mountainPercentage, int precission = 255)
        {
            int numberOfTiles = 0;
            foreach (int level in distribution)
            {
                numberOfTiles += level;
            }
            int mountainTiles = (int)(mountainPercentage * numberOfTiles);
            int tiles = 0;
            int i = distribution.Length;
            while (tiles < mountainTiles)
            {
                tiles += distribution[i-1];
                if (i > 1)
                    i--;
                else
                    break;
            }
            return (double)i / (double)precission;
        }
    }
}
