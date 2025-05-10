using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableData
{
    public class TableRune : BaseTableData<TableRune, TableData_Rune>
    {
        public enum eID
        {
            Anger = 1,      //정령의 위력이 { value } 만큼 증가합니다.
            Focus,          //정령의 스킬 명중률이 { value } 만큼 증가합니다.
            Bold,           //정령의 치명타 명중률이 { value } 만큼 증가합니다.
            Vitality,       //정령 소환의 쿨타임이 { value } 턴 만큼 감소합니다.
            Calm,           //정령 소환의 비용이 { value } 만큼 감소합니다.
            Bonding,        //정령 소환 시 { value } 턴 동안 내 캐릭터에게 방어력 증가를 부여합니다.
            Disgust,        //정령 소환 시 { value } 턴 동안 모든 적에게 방어력 약화를 부여합니다.
            Comfort,        //정령 소환 시 내 캐릭터를 { value } 만큼 치유합니다.
        }

        public string GetString_Title(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return "없는 스킬";

            return TableManager.Instance.String.GetString(base.GetData(tableID).strID);
        }

        public string GetString_Desc(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return "없는 스킬";

            TableData_Rune data = base.GetData(tableID);
            return string.Format(TableManager.Instance.String.GetString(data.strID, TableString.eTYPE.Description), data.value);
        }

        public Sprite GetIcon(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, base.GetData(tableID).strIcon);
        }

        public List<TableData_Rune> GetRandomList(int nCount)
        {
            return base.m_listData.OrderBy(g => Guid.NewGuid()).Take(nCount).ToList();
        }
    }

    public class TableData_Rune : iTableData
    {
        //tableID strID value statusID strIcon
        public uint tableID { get; set; }
        public uint strID { get; set; }
        public float value { get; set; }
        public uint statusID { get; set; }
        public string strIcon { get; set; }
    }
}