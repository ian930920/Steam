using System.Collections.Generic;

namespace TableData
{
    public class TableRouteStep : BaseTableData<TableRouteStep, TableData_RouteStep>
    {
        public enum eSTEP_TYPE
        {
            Battle = 1,     //적과 전투
            Event,          //이벤트
            Box,            //상자
            Battle_Event,   //적과 전투 & 이벤트
            Battle_Box,     //적과 전투 & 상자
            Boss            //보스
        }

        public readonly static int MAX_STEP = 5;
        public readonly static int MAX_STAGE = 3;

        protected override void dataProcessing()
        {
            base.dataProcessing();

            var enumData = base.GetEnumerator();
            while(enumData.MoveNext())
            {
                enumData.Current.Value.CreateList();
            }
        }

        public eSTEP_TYPE GetRandomTableID(int nLevel)
        {
            if(base.ContainsKey((uint)nLevel) == false) return eSTEP_TYPE.Battle;

            var listPercent = base.GetData((uint)nLevel).ListPercent;

            float fTotalWeight = 0.0f;
            for(int i = 0, nMax = listPercent.Count; i < nMax; ++i)
            {
                fTotalWeight += listPercent[i];
            }
            
            float fRandom = UnityEngine.Random.Range(0f, fTotalWeight);
            float fAccum = 0.0f;
            for(int i = 0, nMax = listPercent.Count; i < nMax; ++i)
            {
                fAccum += listPercent[i];
                if(fRandom <= fAccum) return (eSTEP_TYPE)(i + 1);
            }

            return eSTEP_TYPE.Battle; // 비정상 상황
        }
    }

    public class TableData_RouteStep : iTableData
    {
        //tableID battle event treasure battleEvent battleTreasure boss
        public uint tableID { get; set; }
        public float battle { get; set; }
        public float @event { get; set; }
        public float treasure { get; set; }
        public float battleEvent { get; set; }
        public float battleTreasure { get; set; }
        public float boss { get; set; }

        public List<float> ListPercent = new List<float>();

        public void CreateList()
        {
            this.ListPercent.Clear();
            this.ListPercent.Add(battle);
            this.ListPercent.Add(@event);
            this.ListPercent.Add(treasure);
            this.ListPercent.Add(battleEvent);
            this.ListPercent.Add(battleTreasure);
            this.ListPercent.Add(boss);
        }
    }
}