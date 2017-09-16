using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.Session
{
    public enum DbSessionLifeCycle
    {
        /// <summary>
        /// 瞬态
        /// </summary>
        Transient = 1,
        /// <summary>
        /// 执行作用域
        /// [ Transaction | ... ]
        /// </summary>
        Scoped = 2
    }
}
