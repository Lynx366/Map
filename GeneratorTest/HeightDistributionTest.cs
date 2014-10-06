using System;
using Xunit;
using FluentAssertions;
using Generator;

namespace GeneratorTest
{
    public class HeightDistributionTest : IDisposable
    {
        double[,] _map = new double[,]
            {
                {0.1 , 0.2 , 0.3},
                {0.4 , 0.5 , 0.6},
                {0.7 , 0.8 , 0.9},
            };
        public HeightDistributionTest()
        {
            
        }
        [Fact]
        public void calculateDistribution_PositiveTest()
        {
            _map[0,2] = 0.1;
            int[] distribution = Generator.HeightDistribution.calculateDistribution(_map,10);
            distribution[1].Should().Be(2);
            distribution[3].Should().Be(0);
        }
        [Fact]
        public void calculateWaterLevel_PositiveTest()
        {
            int[] distribution = { 0 , 1 , 1 , 1 ,
                                   1 , 1 , 1 ,
                                   1 , 1 , 1};
            double waterLevel = Generator.HeightDistribution.calculateWaterLevel(distribution, 0.5, 10);
            waterLevel.Should().Be(0.5);
        }
        [Fact]
        public void calculateMountainLevel_PositiveTest()
        {
            int[] distribution = { 0 , 1 , 1 , 1 ,
                                   1 , 1 , 1 ,
                                   1 , 1 , 1};
            double waterLevel = Generator.HeightDistribution.calculateMountainLevel(distribution, 0.5, 10);
            waterLevel.Should().Be(0.6);
        }
        [Fact]
        public void DigitalizeMap_PositiveTest()
        {
            Map testMap = new Map();
            testMap.map = _map;
            testMap = Digitalization.digitalizeMap(testMap, 0.3, 0.3);
            testMap.digitalized[0, 0].Should().Be(0);
            testMap.digitalized[2, 2].Should().Be(1);
        
        }
        public void Dispose()
        { }
    }
}
