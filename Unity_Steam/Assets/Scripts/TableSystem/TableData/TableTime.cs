using System.Collections.Generic;
using System.Linq;

namespace TableData
{
    public class TableTime : BaseTableData<TableTime, TableData_Time>
    {
        public enum eID
        {
            Null_4 = 1,
            Null_5,
            Null_1,
            Null_2,
            Offline_Reward,                 //오프라인 보상
            CatTrap,                        //고양이 함정 유지
            Order_Heart,                    //밥상 주문 명성 대기시간
            Fishing_Event,                  //낚시 이벤트
            Churu,                          //츄르 충전시간
            Gambling,                       //고양이 야바위 대기시간
            Null_3,
            Mouse,
        }

        public float GetDurationTime(eID eTableID)
        {
            return base.GetData((int)eTableID).durationTime;
        }

        public float GetCoolTime(eID eTableID)
        {
            return base.GetData((int)eTableID).coolTime;
        }

        public TimeManager.eTYPE GetType(int nTimeID)
        {
            if(base.ContainsKey(nTimeID) == false) return TimeManager.eTYPE.Game;

            return (TimeManager.eTYPE)base.GetData(nTimeID).type;
        }
    }

    public class TableData_Time : iTableData
    {
        //tableID coolTime durationTime type
        public int tableID { get; set; }
        public float coolTime { get; set; }
        public float durationTime { get; set; }
        public int type { get; set; }
    }
}