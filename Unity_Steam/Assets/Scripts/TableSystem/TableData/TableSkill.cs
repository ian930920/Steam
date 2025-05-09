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

        public TableData_Skill GetDataBySummonID(uint summonID)
        {
            if(TableManager.Instance.Summon.ContainsKey(summonID) == false) return null;

            return base.GetData(TableManager.Instance.Summon.GetData(summonID).skillID);
        }

        public string GetString_Title(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return "없는 스킬";

            return TableManager.Instance.String.GetString(base.GetData(tableID).strID);
        }

        public string GetString_Desc(uint tableID, int value)
        {
            if(base.ContainsKey(tableID) == false) return "없는 스킬";

            var data = base.GetData(tableID);
            return string.Format(TableManager.Instance.String.GetString(data.strID, TableString.eTYPE.Description), Utility_UI.GetCommaNumber(value), data.dur);
        }

        public bool IsFriendlyTarget(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return false;

            var data = base.GetData(tableID);

            switch((eTYPE)data.type)
            {
                case eTYPE.Heal:
                case eTYPE.Summon:
                return true;
            }

            switch((eTARGET_TYPE)data.target)
            {
                case eTARGET_TYPE.Enemy_Select_1:
                case eTARGET_TYPE.Enemy_Random_2:
                case eTARGET_TYPE.Enemy_All:
                return false;

                case eTARGET_TYPE.Self:
                case eTARGET_TYPE.Friendly_Select_1:
                case eTARGET_TYPE.Friendly_Random_1:
                case eTARGET_TYPE.Friendly_All:
                return true;
            }

            return false;
        }

        public eTARGET_TYPE GetTargetType(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return eTARGET_TYPE.Enemy_Select_1;

            return (eTARGET_TYPE)base.GetData(tableID).target;
        }

        public int GetDefaultDamage(uint tableID, Stat_Character statDefault, Stat_Additional statAdditional)
        {
            if(base.ContainsKey(tableID) == false) return 0;

            var coe = base.GetData(tableID).coe;
            if(statAdditional.GetStat(Stat_Additional.eTYPE.Coe) > 0) coe += coe * statAdditional.GetStat(Stat_Additional.eTYPE.Coe);
            return (int)(coe * statDefault.GetStat(Stat_Character.eTYPE.Strength));
        }

        public int GetCooldownTurn(uint tableID, Stat_Additional statAdditional)
        {
            if(base.ContainsKey(tableID) == false) return 0;

            return base.GetData(tableID).cooldown - (int)statAdditional.GetStat(Stat_Additional.eTYPE.Cooldown);
        }
    }

    public class TableData_Skill : iTableData
    {
        //tableID type cooldown coe acc crit dur target buff debuff strID resID
        public uint tableID { get; set; }
        public uint type { get; set; }
        public int cooldown { get; set; }
        public float coe { get; set; }
        public float acc { get; set; }
        public float crit { get; set; }
        public int dur { get; set; }
        public uint target { get; set; }
        public List<uint> listStatusID { get; set; }
        public uint strID { get; set; }
        public uint resID { get; set; }
    }
}