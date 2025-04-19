using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableStatus : BaseTableData<TableStatus, TableData_Status>
    {
        public enum eID
        {
            Burn = 1000,    //화상
            Bleeding,       //출혈
            Fainting,       //기절
            Dark,           //암흑
            Curse,          //저주
            Weakened_Def,   //방어력 약화
            Weakened_Atk,   //공격력 약화
            Weakened_Hit,   //명중 약화
        }
    }

    public class TableData_Status : iTableData
    {
        //tableID strID value
        public uint tableID { get; set; }
        public uint strID { get; set; }
        public float value { get; set; }
    }
}