using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableSkill : BaseTableData<TableSkill, TableData_Skill>
    {
        public enum eTYPE
        {
            Attack,
            Heal,
        }

    }

    public class TableData_Skill : iTableData
    {
        //tableID type cost cooldown coe acc dur target buff debuff
        public uint tableID { get; set; }
        public uint cost { get; set; }
        public uint cooldown { get; set; }
        public uint coe { get; set; }
        public uint acc { get; set; }
        public uint dur { get; set; }
        public uint target { get; set; }
        public List<uint> listBuffID { get; set; }
        public List<uint> listDebuffID { get; set; }
    }
}