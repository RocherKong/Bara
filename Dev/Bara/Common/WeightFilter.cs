using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bara.Common
{
    /// <summary>
    /// 权重筛选器
    /// </summary>
    public class WeightFilter<T>
    {
        public class WeightSource
        {
            public T Source { get; set; }

            public int Weight { get; set; }
        }

        public WeightSource Elect(IEnumerable<WeightSource> WeightSources)
        {
            var random = new Random();
            int totalWeight = WeightSources.Sum(x => x.Weight);
            int position = random.Next(1, totalWeight);
            var source = FindSourceByPosition(WeightSources, position);
            return source;
        }

        private WeightSource FindSourceByPosition(IEnumerable<WeightSource> weightSources, int position)
        {
            int StartIndex = 0;
            foreach (var weightSource in weightSources)
            {
                int resultPosition = weightSource.Weight + position;
                if (position > StartIndex && position < resultPosition)
                {
                    return weightSource;
                }
                StartIndex += weightSource.Weight;
            }

            return null;
        }
    }


}
