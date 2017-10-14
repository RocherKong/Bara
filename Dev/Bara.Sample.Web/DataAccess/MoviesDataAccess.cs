using Bara.Abstract.Core;
using Bara.Sample.Web.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bara.Sample.Web.DataAccess
{
    public class MoviesDataAccess : Bara.DataAccess.Impl.DataAccessGeneric<T_Movie>
    {
        private readonly IBaraMapper _baraMapper;
        public MoviesDataAccess(IBaraMapper baraMapper)
        {
            this._baraMapper = baraMapper;
        }



    }
}
