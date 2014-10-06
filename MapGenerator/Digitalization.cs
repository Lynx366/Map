using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public static class Digitalization
    {
        public static Map digitalizeMap(Map _map, double waterPercentage, double mountainPercentage)
        {
            Map newMap = new Map();
            int precission = 1024;
            int size = _map.map.GetLength(0);
            double[,] digitalized = new double[size, size];
            int[] distribution = HeightDistribution.calculateDistribution(_map.map,precission);
            double waterLevel = HeightDistribution.calculateWaterLevel(distribution, waterPercentage, precission);
            double mountainLevel = HeightDistribution.calculateMountainLevel(distribution, mountainPercentage, precission);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size;j++ )
                {
                    if (_map.map[i, j] >= mountainLevel)
                        digitalized[i, j] = 1;
                    else if (_map.map[i, j] >= waterLevel && _map.map[i,j] < mountainLevel)
                        digitalized[i, j] = 0.5;
                    else
                        digitalized[i,j] = 0;
                }
                    newMap.map = _map.map;
                    newMap.waterLevel = waterLevel;
                    newMap.mountainLevel = mountainLevel;
                    newMap.distribution = distribution;
                    newMap.digitalized = Filter(digitalized);
                    newMap.seed = _map.seed;
                    newMap.size = _map.size;
                    newMap.smoothMap = _map.smoothMap;
                    return newMap;
        }
        public static double[,] Filter(double[,] _map)
        {
            int size = _map.GetLength(0);
            double[,] filtered = new double[size,size];
            for(int i = 1;i<size-1;i++)
                for(int j=1;j<size-1;j++)
                {
                    double[] vector = new double[5];
                    vector[0] = _map[i, j];
                    vector[1] = _map[i+1, j];
                    vector[2] = _map[i-1, j];
                    vector[3] = _map[i, j+1];
                    vector[4] = _map[i, j-1];
                    Array.Sort(vector);
                    double value = vector[2];
                    filtered[i, j] = value;
                    filtered[i+1, j] = value;
                    filtered[i-1, j] = value;
                    filtered[i, j+1] = value;
                    filtered[i, j-1] = value;
                    
                }
            filtered[0, 0] = _map[0, 0];
            filtered[size-1, 0] = _map[size-1, 0];
            filtered[0, size-1] = _map[0, size-1];
            filtered[size-1, size-1] = _map[size-1,size-1];
            return filtered;
        }
    }
}
