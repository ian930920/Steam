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

            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, $"Icon_{base.GetData(tableID).strSprite}");
        }

        public Sprite GetSprite(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, base.GetData(tableID).strSprite);
        }

        public int GetRandomListCount(int nCount)
        {
            var buyableCount = base.m_listData.Count(data => UserDataManager.Instance.Summon.IsContainsSummon(data.tableID) == false);
            if(nCount > buyableCount) nCount = buyableCount;
            return nCount;
        }

        public List<TableData_Summon> GetRandomList(int nCount)
        {
            nCount = this.GetRandomListCount(nCount);
            return base.m_listData.Where(data => UserDataManager.Instance.Summon.IsContainsSummon(data.tableID) == false).OrderBy(g => Guid.NewGuid()).Take(nCount).ToList();
        }

        public uint GetRandomSummonID()
        {
            var list = this.GetRandomList(1);
            if(list.Count == 0) return 0;

            return this.GetRandomList(1)[0].tableID;
        }

        public string GetString_SkillDesc(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return "없는 정령";

            var dataSkill = TableManager.Instance.Skill.GetData(base.GetData(tableID).skillID);
            return string.Format(TableManager.Instance.String.GetString(dataSkill.strID, TableString.eTYPE.Description), Utility_UI.GetCommaNumber(dataSkill.coe), dataSkill.dur);
        }
    }

    public class TableData_Summon : iTableData
    {
        //tableID strID maxRune rarity cost skillID resID buyCost
        public uint tableID { get; set; }
        public uint strID { get; set; }
        public int maxRune { get; set; }
        public uint rarity { get; set; }
        public int cost { get; set; }
        public uint skillID { get; set; }
        public uint resID { get; set; }
        public string strSprite { get; set; }
        public int buyCost { get; set; }
    }
}