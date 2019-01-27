using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiMap
{
    public class CNode
    {
        private int ? valueTree;
        private int xPos;
        private int yPos;
        private int level;
        private CNode son;
        private CNode brother;


        public int? ValueTree { get { return valueTree; } set { valueTree = value; } }
        public int XPos { get { return xPos; } set { xPos = value; } }
        public int YPos { get { return yPos; } set { yPos = value; } }
        public int Level { get { return level; } set { level = value; } }
        public CNode Son { get { return son; } set { son = value; } }
        public CNode Brother { get { return brother; } set { brother = value; } }

        public CNode()
        {
            valueTree = null;
            xPos = 0;
            yPos = 0;
            level = 0;
            son = null;
            brother = null;
        }
    }
}
