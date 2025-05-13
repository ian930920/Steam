using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableData
{
    public class TableEnemyType : BaseTableData<TableEnemyType, TableData_EnemyType>
    {
        public enum eTYPE // == tableID
        {
            Normal = 0,
            Elite = 1,
            Boss = 9,
        }

        //Key : lv, Value : list<stEnemyEncounter>
        private Dictionary<int, List<stEnemyEncounter>> m_dicByLevel = new Dictionary<int, List<stEnemyEncounter>>();

        protected override void dataProcessing()
        {
            base.dataProcessing();

            this.m_dicByLevel.Clear();
            this.m_dicByLevel.Add(1, new List<stEnemyEncounter>());
            this.m_dicByLevel.Add(2, new List<stEnemyEncounter>());
            this.m_dicByLevel.Add(3, new List<stEnemyEncounter>());

            var enumData = base.GetEnumerator();
            while(enumData.MoveNext())
            {
                var data = enumData.Current.Value;

                this.m_dicByLevel[1].Add(new stEnemyEncounter(data.tableID, data.lv1));
                this.m_dicByLevel[2].Add(new stEnemyEncounter(data.tableID, data.lv2));
                this.m_dicByLevel[3].Add(new stEnemyEncounter(data.tableID, data.lv3));
            }
        }

        public uint GetRandomEnemyType(int level)
        {
            if(this.m_dicByLevel.ContainsKey(level) == false)
            {
                ProjectManager.Instance.LogWarning($"GetRandomEnemyType Level {level} is not defined in m_dicByLevel.");
                return 0;
            }

            var list = this.m_dicByLevel[level];
            if(list == null || list.Count == 0) return 0;

            float totalPercent = list.Sum(x => x.Percent);
            float rand = Random.Range(0.0f, totalPercent);
            float accum = 0f;
            for(int i = 0, nMax = list.Count; i < nMax; ++i)
            {
                accum += list[i].Percent;
                if(rand <= accum) return list[i].TableID;
            }

            //fallback
            return list.Last().TableID;
        }
    }

    public struct stEnemyEncounter
    {
        public uint TableID;
        public float Percent;

        public stEnemyEncounter(uint tableID, float fPercent)
        {
            this.TableID = tableID;
            this.Percent = fPercent;
        }
    }

    public class TableData_EnemyType : iTableData
    {
        //tableID lv1 lv2 lv3
        public uint tableID { get; set; }
        public float lv1 { get; set; }
        public float lv2 { get; set; }
        public float lv3 { get; set; }
    }
}