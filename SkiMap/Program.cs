using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Position { North, South, East, West };
//int[,] fillTable = new int[,] { { 9, 5, 7 }, { 7, 2, 5 }, { 6, 3, 1 } };
namespace SkiMap
{
    public class Program
    {
        static unsafe void Main(string[] args)
        {
            //-----------------------------------------------------------------------------------

            //If you want to insert program parameters randomly

            //int dim;
            //int biggerNum;
            //int[,] table;
            //InsertProgramParameters(out dim, out biggerNum, out table);
            //int[,] fillTable = FillScreenArray(table, dim, biggerNum);

            //--------------------------------------------------------------------------------------------------------------------


            //Test arrays
            int[,] fillTable = new int[,] { { 4, 8, 7, 3 }, { 2, 5, 9, 3 }, { 6, 3, 2, 5 }, { 4, 4, 1, 6 } };
            
            int dim = fillTable.GetLength(0);
            //FillScreenArray(fillTable, dim, 9);
            //WriteText(fillTable);

            FindCloseNumbers(fillTable, dim, null, new List<PositionEntity>());

        }

        unsafe private static void InsertProgramParameters(out int dim, out int biggerNum, out int[,] table)
        {
            string _val1 = "";
            Console.Write("Please enter the Matrix's size: ");
            EnterValues(ref _val1);

            string _val2 = "";
            Console.Write("Please enter the Matrix's biggest number : ");
            EnterValues(ref _val2);


            dim = Convert.ToInt32(_val1);
            biggerNum = Convert.ToInt32(_val2);

            table = new int[dim, dim];
        }

        unsafe private static void EnterValues(ref string _val)
        {
            ConsoleKeyInfo size;
            do
            {
                size = Console.ReadKey(true);
                if (size.Key != ConsoleKey.Backspace)
                {
                    double val = 0;
                    bool _x = double.TryParse(size.KeyChar.ToString(), out val);
                    if (_x)
                    {
                        _val += size.KeyChar;
                        Console.Write(size.KeyChar);
                    }
                }
                else
                {
                    if (size.Key == ConsoleKey.Backspace && _val.Length > 0)
                    {
                        _val = _val.Substring(0, (_val.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (size.Key != ConsoleKey.Enter);
            Console.Write("\r\n");
        }

        private static void FindCloseNumbers(int[,] fillTable, int dim, CNode pNode, List<PositionEntity> position)
        {
            CTree tree = new CTree();
            PositionEntity positionEntity;
            List<CTree> treesProperty = new List<CTree>();
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (pNode == null)
                    {
                        positionEntity = new PositionEntity();
                        
                        //Inserta La Raiz
                        CNode root = tree.Insert(fillTable[i, j], null);
                        root.XPos = i;
                        root.YPos = j;

                        positionEntity.Height = positionEntity.Height++;
                        //Tiene que llamar a la misma funcion
                        FindCloseNumbers(fillTable, dim, root, position);
                        
                        CTree treeProperty = new CTree();
                        treeProperty.Height = tree.FindMaxLevelTree(root);
                        treeProperty.Steeper = tree.FindSteeperTree(root);
                        treeProperty.Root = root;

                        treesProperty.Add(treeProperty);

                        Console.Write("Tree: ");
                        Console.Write("\r\n");
                        tree.TransversaPreO(root);
                    }
                    else
                    {
                        if (pNode.XPos == 0 && pNode.YPos == 0)
                        {
                            List<PositionEntity> positions = new List<PositionEntity> 
                            { 
                                new PositionEntity { PositionName = Position.East, XPosition = 0, YPosition = 1 , Value = fillTable[i + pNode.XPos, j + 1 + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.South, XPosition = 1, YPosition = 0, Value = fillTable[i + pNode.XPos + 1, j + pNode.YPos] } 
                            };
                            positions = positions.OrderByDescending(x => x.Value).ToList();
                            ValidatePosition(fillTable, dim, pNode, position, tree, i, j, positions);
                            return;
                        }
                        //Valida los elementos que estan en la fila 0
                        else if (pNode.XPos == 0 && pNode.YPos > 0 && pNode.YPos < dim - 1)
                        {
                            List<PositionEntity> positions = new List<PositionEntity> 
                            { 
                                new PositionEntity { PositionName = Position.West, XPosition = 0, YPosition = -1, Value = fillTable[i + pNode.XPos, j - 1 + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.East, XPosition = 0, YPosition = 1, Value = fillTable[i + pNode.XPos, j + 1 + pNode.YPos]}, 
                                new PositionEntity { PositionName = Position.South, XPosition = 1, YPosition = 0, Value = fillTable[i + pNode.XPos + 1, j + pNode.YPos]} 
                            };
                            positions = positions.OrderByDescending(x => x.Value).ToList();
                            ValidatePosition(fillTable, dim, pNode, position, tree, i, j, positions);
                            return;
                        }
                        //Valida los elemento que estan en la columna 0
                        else if (pNode.XPos > 0 && pNode.XPos < dim - 1 && pNode.YPos == 0)
                        {
                            List<PositionEntity> positions = new List<PositionEntity> 
                            { 
                                new PositionEntity { PositionName = Position.North, XPosition = -1, YPosition = 0 , Value = fillTable[i + pNode.XPos - 1, j + pNode.YPos]}, 
                                new PositionEntity { PositionName = Position.East, XPosition = 0, YPosition = 1, Value = fillTable[i + pNode.XPos, j + 1 + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.South, XPosition = 1, YPosition = 0, Value = fillTable[i + pNode.XPos + 1, j + pNode.YPos] } 
                            };
                            positions = positions.OrderByDescending(x => x.Value).ToList();
                            ValidatePosition(fillTable, dim, pNode, position, tree, i, j, positions);
                            return;
                        }
                        //Valida que el numero no este en la fila 0 ni en la columna 0 
                        else if (pNode.XPos > 0 && pNode.YPos > 0 && pNode.XPos < dim - 1 && pNode.YPos < dim - 1)
                        {
                            List<PositionEntity> positions = new List<PositionEntity> 
                            { 
                                new PositionEntity { PositionName = Position.North, XPosition = -1, YPosition = 0, Value = fillTable[i + pNode.XPos - 1, j + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.East, XPosition = 0, YPosition = 1, Value = fillTable[i + pNode.XPos, j + 1 + pNode.YPos] },
                                new PositionEntity { PositionName = Position.West, XPosition = 0, YPosition = -1, Value = fillTable[i + pNode.XPos, j - 1 + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.South, XPosition = 1, YPosition = 0, Value = fillTable[i + pNode.XPos + 1, j + pNode.YPos] } 
                            };
                            positions = positions.OrderByDescending(x => x.Value).ToList();
                            ValidatePosition(fillTable, dim, pNode, position, tree, i, j, positions);
                            return;
                        }
                        //Valida el numero en la posicion 0, dim-1
                        else if (pNode.XPos == 0 && pNode.YPos == dim - 1)
                        {
                            List<PositionEntity> positions = new List<PositionEntity> 
                            { 
                                new PositionEntity { PositionName = Position.West, XPosition = 0, YPosition = -1, Value = fillTable[i + pNode.XPos, j - 1 + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.South, XPosition = 1, YPosition = 0, Value = fillTable[i + pNode.XPos + 1, j + pNode.YPos] } 
                            };
                            positions = positions.OrderByDescending(x => x.Value).ToList();
                            ValidatePosition(fillTable, dim, pNode, position, tree, i, j, positions);
                            return;
                        }
                        //Valida los numeros que estan en la columna dim-1 y las filas > 0
                        else if (pNode.XPos > 0 && pNode.XPos < dim - 1 && pNode.YPos == dim - 1)
                        {
                            List<PositionEntity> positions = new List<PositionEntity> 
                            { 
                                new PositionEntity { PositionName = Position.North, XPosition = -1, YPosition = 0, Value = fillTable[i + pNode.XPos - 1, j + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.West, XPosition = 0, YPosition = -1, Value = fillTable[i + pNode.XPos, j - 1 + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.South, XPosition = 1, YPosition = 0, Value = fillTable[i + pNode.XPos + 1, j + pNode.YPos] } 
                            };
                            positions = positions.OrderByDescending(x => x.Value).ToList();
                            ValidatePosition(fillTable, dim, pNode, position, tree, i, j, positions);
                            return;
                        }
                        else if (pNode.XPos == dim - 1 && pNode.YPos == 0)
                        {
                            List<PositionEntity> positions = new List<PositionEntity> 
                            { 
                                new PositionEntity { PositionName = Position.North, XPosition = -1, YPosition = 0, Value = fillTable[i + pNode.XPos - 1, j + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.East, XPosition = 0, YPosition = 1, Value = fillTable[i + pNode.XPos, j + 1 + pNode.YPos] } 
                            };
                            positions = positions.OrderByDescending(x => x.Value).ToList();
                            ValidatePosition(fillTable, dim, pNode, position, tree, i, j, positions);
                            return;
                        }
                        else if (pNode.YPos > 0 && pNode.YPos < dim - 1 && pNode.XPos == dim - 1)
                        {
                            List<PositionEntity> positions = new List<PositionEntity> 
                            { 
                                new PositionEntity { PositionName = Position.North, XPosition = -1, YPosition = 0, Value = fillTable[i + pNode.XPos - 1, j + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.East, XPosition = 0, YPosition = 1, Value = fillTable[i + pNode.XPos, j + 1 + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.West, XPosition = 0, YPosition = -1, Value = fillTable[i + pNode.XPos, j - 1 + pNode.YPos] } 
                            };
                            positions = positions.OrderByDescending(x => x.Value).ToList();
                            ValidatePosition(fillTable, dim, pNode, position, tree, i, j, positions);
                            return;
                        }
                        else if (pNode.XPos == dim - 1 && pNode.YPos == dim - 1)
                        {
                            List<PositionEntity> positions = new List<PositionEntity> 
                            { 
                                new PositionEntity { PositionName = Position.North, XPosition = -1, YPosition = 0, Value = fillTable[i + pNode.XPos - 1, j + pNode.YPos] }, 
                                new PositionEntity { PositionName = Position.West, XPosition = 0, YPosition = -1, Value = fillTable[i + pNode.XPos, j - 1 + pNode.YPos] } 
                            };
                            positions = positions.OrderByDescending(x => x.Value).ToList();
                            ValidatePosition(fillTable, dim, pNode, position, tree, i, j, positions);
                            return;
                        }
                    }
                }
            }

            treesProperty.Sort(delegate(CTree a, CTree b)
            {
                int xdiff = a.Height.CompareTo(b.Height);
                if (xdiff != 0) return xdiff;
                else return Convert.ToInt32(a.Steeper).CompareTo(b.Steeper);
            });

            tree.SelectBestWay(treesProperty.Select(x => x.Root).ToList()[treesProperty.Count - 1]);

            Console.WriteLine("\r\n----------------------------LONGEST TREE-------------------------------\r\n");

            tree.TransversaPreO(treesProperty.Select(x => x.Root).ToList()[treesProperty.Count - 1]);
            Console.WriteLine("The Size of the tree longest Path: {0}", treesProperty.Select(x => x.Height).ToList()[treesProperty.Count - 1]);

            Console.WriteLine("The Size of the tree steepest path: {0}", treesProperty.Select(x => x.Steeper).ToList()[treesProperty.Count - 1]);

            Console.ReadLine();
        }

        private void SortList(List<CTree> treeList)
        {
            treeList.Sort(delegate(CTree a, CTree b)
            {
                int xdiff = a.Height.CompareTo(b.Height);
                if (xdiff != 0) return xdiff;
                else return Convert.ToInt32(a.Steeper).CompareTo(b.Steeper);
            });
        }

        private static void ValidatePosition(int[,] fillTable, int dim, CNode pNodo, List<PositionEntity> tree, CTree arbol, int i, int j, List<PositionEntity> position)
        {
            foreach (PositionEntity item in position)
            {
                if (pNodo.ValueTree > fillTable[pNodo.XPos + item.XPosition, pNodo.YPos + item.YPosition])
                {
                    CNode nodo = arbol.Insert(fillTable[i + item.XPosition + pNodo.XPos, j + item.YPosition + pNodo.YPos], pNodo);
                    nodo.XPos = i + item.XPosition + pNodo.XPos;
                    nodo.YPos = j + item.YPosition + pNodo.YPos;
                    FindCloseNumbers(fillTable, dim, nodo, tree);
                }
            }
            
        }

        private static int[,] FillScreenArray(int[,] table, int dim, int biggerDim)
        {
            
            // 100 integers: 0..99
            var queue = new Queue(Enumerable.Range(0, dim * dim).ToList<int>());
            var rng = new Random();

            int x = dim / 2, y = dim / 2;

            Console.Write("\r\n----------------------------ARRAY-------------------------------\r\n");

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    table[i, j] = rng.Next(1, biggerDim);
                    Console.Write("{0,3}", table[i, j]);
                }
                Console.Write("\n", Environment.NewLine);
            }
            Console.Write("\r\n\r\n\r\n");
            return table;
        }

        private static void WriteText(int[,] array)
        {

            string path = Environment.CurrentDirectory;

            List<string> linesToWrite = new List<string>();
            for (int i = 0; i < array.GetLength(0) ; i++)
            {
                StringBuilder line = new StringBuilder();
                for (int j = 0; j < array.GetLength(0); j++)
                    line.Append(array[i, j]).Append(" ");
                linesToWrite.Add(line.ToString());
            }
            System.IO.File.WriteAllLines(path + "\\Array.txt", linesToWrite.ToArray());
            
        }
    }
}
