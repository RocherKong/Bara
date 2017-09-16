using Bara.Abstract.Core;
using Bara.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Abstract.Config
{
    public interface IConfigLoader : IDisposable
    {
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="baraMapper"></param>
        /// <returns></returns>
        BaraMapConfig Load(string filePath, IBaraMapper baraMapper);
    }
}
