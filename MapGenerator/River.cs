using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public static class River
    {
        
        public static Map CreateRiver(Map _map)
        {
            var first = findStartingPoint(_map);
            first.stream = 0.1;
            List<RiverPoint> newRiver = new List<RiverPoint>();
            newRiver.Add(first);
            _map.rivers[first.x, first.y] = 1;
            bool flag = true;
            do
            {
                int jump = 5;
                var result = FindNextPoint(newRiver, _map, jump);                
                while (result == null)
                {
                    jump += 5;
                    if(jump>30)
                        break;
                    result = FindNextPoint(newRiver, _map, jump);                        
                }
                if (result == null)
                    flag = false;
                if (_map.digitalized[newRiver.Last().x, newRiver.Last().y] == 0)
                    flag = false;

            } while (flag == true);
            _map.AllRivers.Add(newRiver);
            return _map;
        }
        public static Map AlternativeCreateRiver(Map _map)
        {
            var first = findStartingPoint(_map);
            first.stream = 0.1;
            return _map;
        }
        public static List<RiverPoint> FindNextPoint(List<RiverPoint> previous, Map _map, int range = 10)
        {
            RiverPoint result = new RiverPoint();
            result.stream = previous.Last().stream;
            int x = previous.Last().x;
            int y = previous.Last().y;
            int newx = x;
            int newy = y;
            for (int i = x - range; i < x + range; i++)
                for (int k = y - range; k < y + range; k++ )
                {
                    if(Math.Pow(i-x,2) + Math.Pow(k-y,2) < Math.Pow(range,2) &&
                       i>=0 && i<_map.size && k>=0 && k<_map.size && i!=x && k!=y)
                    {
                       if(_map.map[newx,newy] > _map.map[i,k])
                       { 
                         newx = i;
                         newy = k;
                       }
                    }
                }
            if (newx == previous.Last().x && newy == previous.Last().y)
                return null;
            result.x = newx;
            result.y = newy;
            _map.rivers[newx,newy] = 1;
            previous.Add(result);
            return previous;
        }

        public static RiverPoint findStartingPoint(Map _map)
        {
            Random rnd = new Random(_map.seed + _map.AllRivers.Count);
            int X;
            int Y = -1;
            do
            {
                X = rnd.Next(0, _map.size);
                List<int> Yaxis = new List<int>();
                for(int i=0;i<_map.size;i++)
                    if (_map.digitalized[X, i] == 1)
                        Yaxis.Add(i);
                if (Yaxis.Count > 0)
                    Y = Yaxis[rnd.Next(0, Yaxis.Count)];
            } while (Y < 0);
            var result = new RiverPoint();
            result.x = X;
            result.y = Y;
            return result;
        }
        public static List<RiverPoint> step(Map _map, List<RiverPoint> RiverCollection)
        {
            int x = RiverCollection.Last().x;
            int y = RiverCollection.Last().y;
            double[] values = new double[8];
            double pointValue = _map.smoothMap[x, y];
            if(x + 1 < _map.size)
                values[0] = _map.smoothMap[x + 1, y];
            else
                values[0] = 1;
            if(x - 1 >= 0)
                values[1] = _map.smoothMap[x - 1, y];
            else
                values[1] = 1;
            if(y + 1 <_map.size)
                values[2] = _map.smoothMap[x, y + 1];
            else
                values[2] = 1;
            if(y - 1 >= 0)
                values[3] = _map.smoothMap[x, y - 1];
            else
                values[3] = 1;
            if(x + 1 < _map.size && y + 1 < _map.size)
                values[4] = _map.smoothMap[x + 1, y + 1];
            else
                values[4] = 1;
            if(x + 1 < _map.size && y - 1 >= 0)
                values[5] = _map.smoothMap[x + 1, y - 1];
            else
                values[5] = 1;
            if(x - 1 >= 0 && y + 1 < _map.size)
                values[6] = _map.smoothMap[x - 1, y + 1];
            else
                values[6] = 1;
            if (x - 1 >= 0 && y - 1 >= 0)
                values[7] = _map.smoothMap[x - 1, y - 1];
            else
                values[7] = 1;

            int index = 0;
            for(int i=1;i<8;i++)
                if (values[index] > values[i])
                    index = i;
            if (index == 0 && values[0] > _map.smoothMap[x, y])
                return null;
            RiverPoint newPoint = new RiverPoint();
            newPoint.stream = RiverCollection.Last().stream;
            switch(index)
            {
                case 0:
                    newPoint.x = x + 1;
                    newPoint.y = y;
                    break;
                case 1:
                    newPoint.x = x - 1;
                    newPoint.y = y;
                    break;
                case 2:
                    newPoint.x = x;
                    newPoint.y = y+1;
                    break;
                case 3:
                    newPoint.x = x;
                    newPoint.y = y-1;
                    break;
                case 4:
                    newPoint.x = x + 1;
                    newPoint.y = y + 1;
                    break;
                case 5:
                    newPoint.x = x + 1;
                    newPoint.y = y - 1;
                    break;
                case 6:
                    newPoint.x = x - 1;
                    newPoint.y = y + 1;
                    break;
                case 7:
                    newPoint.x = x - 1;
                    newPoint.y = y - 1;
                    break;                    
            }
            RiverCollection.Add(newPoint);
            return RiverCollection;
            
        }

    }
}
