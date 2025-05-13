using System.Linq;
using System.Collections.Generic;
using System;

namespace TableData
{
    public class TableEvent : BaseTableData<TableEvent, TableData_Event>
    {
        public static readonly int INVAILD_EVENT = 0;

        public string GetString(uint tableID, TableString.eTYPE eStringType = TableString.eTYPE.Title)
        {
            if(base.ContainsKey(tableID) == false) return "없는 시나리오";

            return TableManager.Instance.String.GetString(base.GetData(tableID).strID, eStringType);
        }

        public uint GetRandomEvent()
        {
            var list = base.m_listData.Where(data => UserDataManager.Instance.Session.IsContainEvent(data.tableID) == false && data.battleChance != 1);
            if(list.Count() == 0) return 0;

            return list.OrderBy(g => Guid.NewGuid()).First().tableID;
        }

        public uint GetRandomBattleEvent()
        {
            var list = base.m_listData.Where(data => UserDataManager.Instance.Session.IsContainEvent(data.tableID) == false && data.battleChance == 1);
            if(list.Count() == 0) return 0;

            return list.OrderBy(g => Guid.NewGuid()).First().tableID;
        }
    }

    public class TableData_Event : iTableData
    {
        //tableID strID battleChance arrOption
        public uint tableID { get; set; }
        public uint strID { get; set; }
        public int battleChance { get; set; }
        public List<uint> arrOption { get; set; }
    }
}