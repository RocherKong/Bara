using System;

namespace Bara.Abstract.Session
{
    public interface IDbConnectionSessionStore : IDisposable
    {
       IDbConnectionSession LocalSession { get; }

        void Store(IDbConnectionSession session);
    }
}
