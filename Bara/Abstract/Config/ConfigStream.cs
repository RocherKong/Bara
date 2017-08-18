using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bara.Abstract.Config
{
    public class ConfigStream
    {
        public String Path { get; set; }

        public Stream Stream { get; set; }
    }
}
