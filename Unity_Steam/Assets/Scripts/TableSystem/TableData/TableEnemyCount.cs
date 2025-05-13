using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableEnemyCount : BaseTableData<TableEnemyCount, TableData_EnemyCount>
    {
        //Key : stage, Value : Dic<level, data>
        private Dictionary<int, Dictionary<int, stEnemyCount>> m_dicByStage = new Dictionary<int, Dictionary<int, stEnemyCount>>();

        protected override void dataProcessing()
        {
            base.dataProcessing();

            var enumData = base.GetEnumerator();
            while(enumData.MoveNext())
            {
                var data = enumData.Current.Value;
                var stage = data.stage;
                if(this.m_dicByStage.ContainsKey(stage) == false)
                {
                    this.m_dicByStage.Add(stage, new Dictionary<int, stEnemyCount>());
                }

                var level = data.lv;
                if(this.m_dicByStage[stage].ContainsKey(level) == true) continue;

                this.m_dicByStage[stage].Add(level, new stEnemyCount(data.minEnemyCount, data.maxEnemyCount));
            }
        }

        public int GetEnemyCount(int stage, int lv)
        {
            if(this.m_dicByStage.ContainsKey(stage) == false) return 3;
            if(this.m_dicByStage[stage].ContainsKey(lv) == false) return 3;

            var count = this.m_dicByStage[stage][lv];
            return Random.Range(count.Min, count.Max);
        }
    }

    public struct stEnemyCount
    {
        public int Min;
        public int Max;

        public stEnemyCount(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }
    }

    public class TableData_EnemyCount : iTableData
    {
        //tableID stage	lv minEnemyCount maxEnemyCount
        public uint tableID { get; set; }
        public int stage { get; set; }
        public int lv { get; set; }
        public int minEnemyCount { get; set; }
        public int maxEnemyCount { get; set; }
    }
}