using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public class MapClass
    {
        public double[,] Map;
        int size;
        public MapClass(int _size)
        {
            size = _size;
            Map = new double[size, size];
        }
        public MapClass(int _size, double[,] _map)
        {
            size = _size;
            Map = _map;
        }
        public MapClass(MapClass _map)
        {
            size = _map.size;
            Map = _map.Map;
        }

    }

    public class MapCollection
    {
        public List<MapClass> Maps;
        public int noiseindex;
        public int digitalizedindex;
        public int readyindex;
        public int seed;
        public int size;
        public MapCollection(List<MapClass> list,int _seed,int _size, int noise)
        {
            Maps = list;
            noiseindex = noise;
            seed = _seed;
            size = _size;
        }
    }
    public class RiverPoint
    {
        public int x;
        public int y;
        public double stream;
    }
    public class Map
    {
        public int seed;
        public int size;
        public double waterLevel;
        public double mountainLevel;
        public int[] distribution;
        public double[,] map;
        public double[,] digitalized;
        public double[,] smoothMap;
        public List<List<RiverPoint>> AllRivers;
        public double[,] rivers;
    }
}
