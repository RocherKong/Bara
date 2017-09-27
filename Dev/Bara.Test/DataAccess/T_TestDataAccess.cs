using Bara.Abstract.Core;
using Bara.DataAccess.Impl;
using Bara.Test.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Test.DataAccess
{
    public class T_TestDataAccess : DataAccessGeneric<T_Test>
    {
        private IBaraMapper _baraMapper;
        public T_TestDataAccess(IBaraMapper baraMapper) : base(baraMapper)
        {
            this._baraMapper = baraMapper;
        }
    }
}
