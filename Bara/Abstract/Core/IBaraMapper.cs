using Bara.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.Core
{
    public interface IBaraMapper : IDisposable
    {
        BaraMapConfig BaraMapConfig { get; }
        void LoadConfig(BaraMapConfig config);
    }
}
