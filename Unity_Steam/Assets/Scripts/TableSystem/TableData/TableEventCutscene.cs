using System.Linq;
using System.Collections.Generic;
using System;
using static UnityEngine.Rendering.DebugUI;

namespace TableData
{
    public class TableEventCutscene : BaseTableData<TableEventCutscene, TableData_EventCutscene>
    {
        public enum eID
        {
            Start = 1,
        }

        public string GetStringByIdx(eID eTableID, int nIdx)
        {
            uint tableID = (uint)eTableID;
            if(base.ContainsKey(tableID) == false) return "없는 시나리오";

            return TableManager.Instance.String.GetString(base.GetData(tableID).listStr[nIdx], TableString.eTYPE.Description);
        }

        public int GetScenarioCount(eID eTableID)
        {
            uint tableID = (uint)eTableID;
            if(base.ContainsKey(tableID) == false) return 0;

            return base.GetData(tableID).listStr.Count;
        }
    }

    public class TableData_EventCutscene : iTableData
    {
        //tableID listStr strSprite
        public uint tableID { get; set; }
        public List<uint> listStr { get; set; }
        public string strSprite { get; set; }
    }
}