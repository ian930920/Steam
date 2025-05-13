using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableData
{
    public class TableRoute : BaseTableData<TableRoute, TableData_Route>
    {
        public enum eSTEP_TYPE
        {
            Battle = 1,     //적과 전투
            Event,          //이벤트
            Box,            //상자
            Battle_Event,   //적과 전투 & 이벤트
            Battle_Box,     //적과 전투 & 상자
            Boss            //보스
        }

        public enum eDEBUFF
        {
            Summon_COE_Debuff = 1001,   //정령의 공격력이 10%만큼 감소합니다
            Enemy_COE_Buff,             //적의 공격력이 20%만큼 증가합니다.
            Weakened_Hit_Debuff,        //공격의 명중률이 20%만큼 감소합니다.
            Summon_Cost_Debuff,         //정령 소환의 비용이 1 증가합니다.
        }

        public Sprite GetIcon(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, base.GetData(tableID).strSprite);
        }

        public Sprite GetSprite(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.BG, base.GetData(tableID).strSprite);
        }

        public List<TableData_Route> GetRandomList(int nCount)
        {
            return base.m_listData.OrderBy(g => Guid.NewGuid()).Take(nCount).ToList();
        }

        public string GetString(uint tableID, TableString.eTYPE eType = TableString.eTYPE.Title)
        {
            if(base.ContainsKey(tableID) == false) return "없는 루트";

            return TableManager.Instance.String.GetString(base.GetData(tableID).strID, eType);
        }
    }

    public class TableData_Route : iTableData
    {
        //tableID strID level strSprite
        public uint tableID { get; set; }
        public uint strID { get; set; }
        public int level { get; set; }
        public string strSprite { get; set; }
    }
}