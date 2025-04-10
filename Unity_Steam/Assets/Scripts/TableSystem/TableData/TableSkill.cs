using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableSkill : BaseTableData<TableSkill, TableData_Skill>
    {
        public enum eTYPE
        {
            Attack = 1,
            Heal,
            Status,
            Summon
        }

        public enum eTARGET_TYPE
        {
            Enemy_Select_1 = 1,         //1: 선택한 적 1
            Enemy_Random_2 = 4,         //4: 무작위 적 2
            Enemy_All = 5,              //5: 모든 적
            
            Self = 10,                  //10: 자신
            Friendly_Select_1 = 11,     //11: 선택한 아군 1
            Friendly_Random_1 = 14,     //14: 무작위 아군 2
            Friendly_All = 15,          //15: 모든 아군
        }

        public enum eID
        {

        }

        public string GetString(uint tableID, TableString.eTYPE eType = TableString.eTYPE.Title)
        {
            if(base.ContainsKey(tableID) == false) return "없는 스킬";

            return ProjectManager.Instance.Table.String.GetString(base.GetData(tableID).strID, eType);
        }
    }

    public class TableData_Skill : iTableData
    {
        //tableID type cost cooldown coe acc crit dur target buff debuff strID
        public uint tableID { get; set; }
        public uint type { get; set; }
        public uint cost { get; set; }
        public uint cooldown { get; set; }
        public float coe { get; set; }
        public float acc { get; set; }
        public float crit { get; set; }
        public uint dur { get; set; }
        public uint target { get; set; }
        public List<uint> listStatusID { get; set; }
        public uint strID { get; set; }
    }
}