using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public static class MapCreator
    {
        public static Map Create(int seed, int size, double waterPercentage, double mountainPercentage,int iterations,int riverCount)
        {
            waterPercentage /= 100;
            mountainPercentage /= 100;
            Map newMap = new Map();
            newMap.seed = seed;
            newMap.size = size;
            var result = NoiseGenerator.GenerateNoise(size,seed,iterations);
            newMap.map = result[1].Map;
            newMap.smoothMap = result[0].Map;
            newMap = Digitalization.digitalizeMap(newMap, waterPercentage, mountainPercentage);
            newMap.AllRivers = new List<List<RiverPoint>>();
            newMap.rivers = new double[size, size];
            for (int i = 0; i < riverCount; i++ )
                River.CreateRiver(newMap);
            return newMap;
        }
    }
}
