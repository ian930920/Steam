using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableResource : BaseTableData<TableResource, TableData_Resource>
    {
        public enum eTYPE
        {
            Sound = 1,
            Atlas,
            Material,
            Popup,
        }
    }

    public class TableData_Resource : iTableData
    {
        //tableID type path
        public int tableID { get; set; }
        public int type { get; set; }
        public string path { get; set; }
    }
}