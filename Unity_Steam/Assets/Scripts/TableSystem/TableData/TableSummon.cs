using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace TableData
{
    public class TableSummon : BaseTableData<TableSummon, TableData_Summon>
    {
        public enum eRARITY
        {
            Common,         //일반
            Uncommon,       //고급
            Rare,           //희귀
            Legendary,      //전설
        }

        public Sprite GetSprite(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ProjectManager.Instance.Resource.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, base.GetData(tableID).strSprite);
        }

        public string GetSkillString(uint tableID, TableString.eTYPE eType = TableString.eTYPE.Title)
        {
            if(base.ContainsKey(tableID) == false) return "없는 소환수";

            return ProjectManager.Instance.Table.Skill.GetString(base.GetData(tableID).skillID, eType);
        }
    }

    public class TableData_Summon : iTableData
    {
        //tableID rarity skillID resID
        public uint tableID { get; set; }
        public uint rarity { get; set; }
        public uint skillID { get; set; }
        public uint resID { get; set; }
        public string strSprite { get; set; }
    }
}