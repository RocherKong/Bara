using Bara.Abstract.Core;
using Bara.Core.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Test
{
    public class TestBase : IDisposable
    {
        public IBaraMapper _baraMapper;
        public TestBase()
        {
            this._baraMapper = new BaraMapper();
        }

        public void Dispose()
        {
            _baraMapper.Dispose();
        }
    }
}
