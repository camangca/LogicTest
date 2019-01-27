using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiMap
{
    public class PositionEntity
    {
        public Enum PositionName { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int Value { get; set; }
        public int Height { get; set; }
    }
}
