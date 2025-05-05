using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        public Sprite GetIcon(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ProjectManager.Instance.Resource.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, base.GetData(tableID).strSprite);
        }

        public Sprite GetSprite(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ProjectManager.Instance.Resource.GetSpriteByAtlas(ResourceManager.eATLAS_ID.Characer, base.GetData(tableID).strSprite);
        }

        public List<TableData_Summon> GetRandomList(int nCount)
        {
            return base.m_listData.OrderBy(g => Guid.NewGuid()).Take(nCount).ToList();
        }

        public string GetString_SkillDesc(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return "없는 정령";

            var dataSkill = ProjectManager.Instance.Table.Skill.GetData(base.GetData(tableID).skillID);
            return string.Format(ProjectManager.Instance.Table.String.GetString(dataSkill.strID, TableString.eTYPE.Description), Utility_UI.GetCommaNumber(dataSkill.coe), dataSkill.dur);
        }
    }

    public class TableData_Summon : iTableData
    {
        //tableID strID maxRune rarity cost skillID resID
        public uint tableID { get; set; }
        public uint strID { get; set; }
        public int maxRune { get; set; }
        public uint rarity { get; set; }
        public uint cost { get; set; }
        public uint skillID { get; set; }
        public uint resID { get; set; }
        public string strSprite { get; set; }
    }
}