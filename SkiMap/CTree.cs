using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiMap
{
    public class CTree
    {
        private CNode root;
        private CNode work;
        private int i = 0;
        private int height = 0;
        private int ? steeper = 0;

        public CNode Root { get { return root; } set { root = value; } }
        public int Height { get { return height; } set { height = value; } }
        public int ? Steeper { get { return steeper; } set { steeper = value; } }

        public CTree()
        {
            root = new CNode();
        }
        public CNode Insert(int pValue, CNode pNode)
        {
            //Si no hay nodo donde insertar, tomamos como si fuera en la raiz
            if (pNode == null)
            {
                root = new CNode();
                root.ValueTree = pValue;

                //No hay hijo
                root.Son = null;

                //No hay hermano
                root.Brother = null;
                return root;
            }
            //Verificamos si no tiene hijo
            //Insertamos el dato como hijo
            if (pNode.Son == null)
            {
                CNode temp = new CNode();
                temp.ValueTree = pValue;
                //Conectamos el nuevo nodo como hijo
                pNode.Son = temp;

                return temp;
            }
            else //Ya tiene un hijo, tenemos que insertarlo como hermano
            {
                work = pNode.Son;
                //recorre todos los hermanos hasta que llega al ultimo
                while (work.Brother != null)
                {
                    work = work.Brother;
                }
                //Creamos nodo temporal
                CNode temp = new CNode();
                temp.ValueTree = pValue;
                //Unimos el temporal al ultimo hermano
                work.Brother = temp;

                return temp;
            }
        }



        //Transversa preorder
        public void TransversaPreO(CNode pNode)
        {
            if (pNode == null)
                return;

            //Me proceso primero a mi
            for (int n = 0; n < i; n++)
                Console.Write("   ");

            Console.WriteLine(pNode.ValueTree);

            //Luego proceso a mi hijo

            if (pNode.Son != null)
            {
                i++;
                TransversaPreO(pNode.Son);
                i--;
            }
            //Si tengo hermanos los proceso
            if (pNode.Brother != null)
                TransversaPreO(pNode.Brother);
        }

        public CNode Find(int pValue, CNode pNode)
        {
            CNode finded = null;

            if (pNode == null)
                return finded;
            if (pNode.ValueTree == pValue)
            {
                finded = pNode;
                return finded;
            }
            //Luego proceso a mi hijo
            if (pNode.Son != null)
            {
                finded = Find(pValue, pNode.Son);

                if (finded != null)
                    return finded;
            }
            //Si tengo hermanos los proceso
            if (pNode.Brother != null)
            {
                finded = Find(pValue, pNode.Brother);
                if (finded != null)
                    return finded;
            }
            return finded;
        }

        public int FindMaxLevelTree(CNode SonNode)
        {
            List<CNode> propertiesTree = FindPropertiesTree(SonNode, new CNode(), new List<CNode>());
            propertiesTree = propertiesTree.OrderByDescending(x => x.Level).ToList();

            return propertiesTree.Select(x => x.Level).ToList()[0] + 1;
        }


        public int ? FindSteeperTree(CNode SonNode)
        {
            List<CNode> propertiesTree = FindPropertiesTree(SonNode, new CNode(), new List<CNode>());
            propertiesTree = propertiesTree.OrderByDescending(x => x.ValueTree).ToList();
            int ? steeperSize = propertiesTree.Select(x => x.ValueTree).ToList()[0] - propertiesTree.Select(x => x.ValueTree).ToList()[propertiesTree.Count - 1];
            return steeperSize;
        }

        private List<CNode> FindPropertiesTree(CNode SonNode, CNode porperty, List<CNode> heightList)
        {
            heightList.Add(porperty);
            CNode treeProperties = new CNode();
            if (SonNode == null)
            {
                treeProperties.Level = porperty.Level;
                treeProperties.ValueTree = SonNode.ValueTree;
                return heightList;
            }
            else
            {
                if (SonNode.Brother != null)
                {
                    if (SonNode.Son != null)
                    {
                        treeProperties.Level = porperty.Level + 1;
                        treeProperties.ValueTree = SonNode.ValueTree;
                        FindPropertiesTree(SonNode.Son, treeProperties, heightList);
                        FindPropertiesTree(SonNode.Brother, treeProperties, heightList);
                    }
                    else
                    {
                        treeProperties.Level = porperty.Level;
                        treeProperties.ValueTree = SonNode.ValueTree;
                        FindPropertiesTree(SonNode.Brother, treeProperties, heightList);
                    }
                }    
                else
                {
                    if (SonNode.Son != null)
                    {
                        treeProperties.Level = porperty.Level + 1;
                        treeProperties.ValueTree = SonNode.ValueTree;
                        FindPropertiesTree(SonNode.Son, treeProperties, heightList);
                    }
                    heightList.Add(SonNode);
                }
            }
            List<CNode> temp = heightList.Where(x => x.ValueTree == null).ToList<CNode>();
            foreach (CNode item in temp)
                heightList.Remove(item);
            
            return heightList;
        }

        public int SelectBestWay(CNode SonNode)
        {
            List<CNode> propertiesTree = FindPropertiesTree(SonNode, new CNode(), new List<CNode>());
            propertiesTree = propertiesTree.OrderBy(x => x.Level).ToList();

            return propertiesTree.Select(x => Convert.ToInt32(x.ValueTree)).ToList()[0] + 1;
        }
    }
}
