using System.Linq;
using System.Collections.Generic;

namespace TableData
{
    public class TableEventOption : BaseTableData<TableEventOption, TableData_EventOption>
    {
        public enum eREWARD_TYPE
        {
            None,       //0 none
            Tichek,     //1 티켓
            Rune,       //2 룬
            Summon,     //3 정령
            Heal,       //4 체력회복
        }

        public string GetString_Option(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return "없는 이벤트 옵션";

            return TableManager.Instance.String.GetString(base.GetData(tableID).strOption, TableString.eTYPE.Description);
        }

        public string GetString_Result(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return "없는 이벤트 옵션 결과";

            return TableManager.Instance.String.GetString(base.GetData(tableID).strResult, TableString.eTYPE.Description);
        }
    }

    public class TableData_EventOption : iTableData
    {
        //tableID strOption strResult isBattle rewardType rewardID rewardValue
        public uint tableID { get; set; }
        public uint strOption { get; set; }
        public uint strResult { get; set; }
        public int isBattle { get; set; }
        public int rewardType { get; set; }
        public int rewardID { get; set; }
        public int rewardValue { get; set; }
    }
}