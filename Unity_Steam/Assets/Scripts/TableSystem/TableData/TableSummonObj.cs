using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableSummonObj : BaseTableData<TableSummonObj, TableData_SummonObj>
    {
        public Sprite GetSprite(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            TableData_SummonObj data = base.GetData(tableID);
            return ResourceManager.Instance.GetSprite(data.resID, data.strSprite);
        }
    }

    public class TableData_SummonObj : iTableData
    {
        //tableID resID strSprite
        public uint tableID { get; set; }
        public uint resID { get; set; }
        public string strSprite { get; set; }
    }
}