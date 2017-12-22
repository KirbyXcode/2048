using System;
using System.Collections.Generic;
 
namespace Console2048
{
    /// <summary>
    /// 负责处理游戏算法(移动数据、合并数据、生成数字、游戏结束……)
    /// </summary>
    class GameCore
    {
        #region 数据
        private int[,] map;
        private int[] mergeArray;
        private int[] removeZeroArray;

        public int[,] Map
        {
            get
            { return map; }

        }
#endregion

        public GameCore()
        {
            map = new int[4, 4];
            mergeArray = new int[4];
            removeZeroArray = new int[4];
            emptyLOCList = new List<Location>(16);
            random = new Random();
            originalMap = new int[4, 4];
        }

        #region 合并数据
        private void RemoveZero()
        {
            //每次去零前，将去零数组清空
            Array.Clear(removeZeroArray, 0, 4);//0 0  0 0 
            int index = 0;
            for (int i = 0; i < mergeArray.Length; i++)
            {
                if (mergeArray[i] != 0)
                    removeZeroArray[index++] = mergeArray[i];
            }
            removeZeroArray.CopyTo(mergeArray, 0);
        }

        private void Merge()
        {
            RemoveZero();
            for (int i = 0; i < mergeArray.Length - 1; i++)
            {
                if (mergeArray[i] == mergeArray[i + 1])
                {
                    mergeArray[i] += mergeArray[i + 1];
                    mergeArray[i + 1] = 0;
                    //统计成绩
                }
            }
            RemoveZero();
        }
        #endregion

        #region 移动
        private void MoveUp()
        {
            for (int c = 0; c < 4; c++)
            {
                for (int r = 0; r < 4; r++)
                    mergeArray[r] = map[r, c];
                Merge();
                for (int r = 0; r < 4; r++)
                    map[r, c] = mergeArray[r];
            }
        }

        private void MoveDown()
        {
            for (int c = 0; c < 4; c++)
            {
                for (int r = 3; r >= 0; r--)
                {
                    mergeArray[3 - r] = map[r, c];
                }
                Merge();
                for (int r = 3; r >= 0; r--)
                {
                    map[r, c] = mergeArray[3 - r];
                }
            }
        }

        private void MoveLeft()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                    mergeArray[c] = map[r, c];

                Merge();

                for (int c = 0; c < 4; c++)
                    map[r, c] = mergeArray[c];
            }
        }

        private void MoveRight()
        {
            for (int r = 0; r < 4; r++)
            {
                for (int c = 3; c >= 0; c--)
                    mergeArray[3 - c] = map[r, c];

                Merge();

                for (int c = 3; c >= 0; c--)
                    map[r, c] = mergeArray[3 - c];
            }
        }

        private int[,] originalMap;
        public bool IsChange { get; set; }

        public void Move(MoveDirection direction)
        {
            //移动前记录Map
            Array.Copy(map, originalMap, map.Length);
            IsChange = false;//假设没有发生改变

            switch (direction)
            {
                case MoveDirection.Up:
                    MoveUp();
                    break;
                case MoveDirection.Down:
                    MoveDown();
                    break;
                case MoveDirection.Left:
                    MoveLeft();
                    break;
                case MoveDirection.Right:
                    MoveRight();
                    break;
            }
            CheckMapChange();
        }

        private void CheckMapChange()
        {
            //移动后对比originalMap  map
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (originalMap[r, c] != map[r, c])
                    {
                        //改变
                        IsChange = true;
                        return;
                    }
                }
            }
        }
        #endregion

        #region 生成新数字 
        public List<Location> emptyLOCList;
        //1.查找所有空白位置
        private void CalculateEmpty()
        {
            //每次统计空位置，都先清空之前的数据
            emptyLOCList.Clear();

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (map[r, c] == 0)
                    {
                        //记录 Location（r ,  c）
                        emptyLOCList.Add(new Location(r, c));
                    }
                }
            }
        }

        private Random random;
        public void GenerateNumber(out Location targetLOC, out int randomNumber)
        {
            CalculateEmpty();

            //如果有空位置
            if (emptyLOCList.Count > 0)
            {
                //生成数字
                //2.随机选择一个位置
                int index = random.Next(0, emptyLOCList.Count);
                targetLOC = emptyLOCList[index];
                //3.生成数字   2(90%)   4(10%) 
                randomNumber = map[targetLOC.RIndex, targetLOC.CIndex] = random.Next(0, 10) == 0 ? 4 : 2;
                //空位置已经被占，需要移除
                emptyLOCList.RemoveAt(index);
            }
            else
            {
                targetLOC = new Location(-1, -1);
                randomNumber = -1;
            }
        }
#endregion

        //游戏是否结束
        public bool IsOver()
        {
            //如果有空位置  则游戏不能结束
            if (emptyLOCList.Count > 0) return false;

            //能否合并
            //for (int r = 0; r < 4; r++)
            //{
            //    for (int c = 0; c < 3; c++)
            //    {
            //        if (map[r, c] == map[r, c + 1])
            //            return false;
            //    }
            //}
            //for (int c = 0; c < 4; c++)
            //{
            //    for (int r = 0; r < 3; r++)
            //    {
            //        if (map[r, c] == map[r + 1, c])
            //            return false;
            //    }
            //}

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    //水平                                     或者    垂直
                    if (map[r, c] == map[r, c + 1] ||  map[c,r] == map[c+1,r])
                        return false;
                }
            }

            return true;//游戏结束
        }
    }
}
