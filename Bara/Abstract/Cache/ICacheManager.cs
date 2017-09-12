using Bara.Abstract.Core;
using Bara.Core.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.Cache
{
    public interface ICacheManager
    {
        IBaraMapper BaraMapper { get; set; }

        object this[RequestContext context, Type type] { get; set; }

        void ResetMappedCaches();

        void TriggerFlush(RequestContext context);

        Queue<RequestContext> RequestQueue { get; set; }

        void FlushQueue();

        void ClearQueue();



    }
}
