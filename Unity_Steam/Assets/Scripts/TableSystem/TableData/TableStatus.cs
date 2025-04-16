using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableStatus : BaseTableData<TableStatus, TableData_Status>
    {

    }

    public class TableData_Status : iTableData
    {
        //tableID strID value
        public uint tableID { get; set; }
        public uint strID { get; set; }
        public float value { get; set; }
    }
}