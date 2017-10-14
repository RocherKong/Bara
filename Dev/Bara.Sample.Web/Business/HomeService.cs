using Bara.Sample.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bara.Sample.Web.Business
{
    public class HomeService
    {
        private readonly MoviesDataAccess _dao_movies;
        public HomeService(MoviesDataAccess dao_movies)
        {
            this._dao_movies = dao_movies;
        }


    }
}
