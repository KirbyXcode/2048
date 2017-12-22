using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Console2048
{
    /// <summary>
    /// 位置
    /// </summary>
    struct Location
    {
        public int RIndex { get; set; }

        public int CIndex { get; set; }

        public Location(int rIndex, int cIndex)
            : this()
        {
            this.RIndex = rIndex;
            this.CIndex = cIndex;
        }
    }
}
