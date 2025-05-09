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

            Regeneration = 2001,    //매 턴 시작 시 최대 체력의 10% 회복
            Shield,                 //받는 피해 흡수
            Transcendence,          //이번 턴 동안 소환수 소환에 필요한 모든 마나 비용 1 감소. 비용은 최소 1까지만 감소할 수 있음
            Rage,                   //다음 공격 1회는 반드시 치명타
            Blessing,               //매 턴 시작 시 회복되는 마나 1 증가
            Defense_Enhancement,    //받는 피해 50% 감소
            Attack_Enhancement,     //최종 피해 50% 증가
            Summon_Protection,      //다른 아군에 의해 보호받는 상태. 받는 피해를 보호 시전자에게 이전
            Add_Mana,               //사용 즉시 마나 2 회복
        }

        public enum eEFFECT_TYPE
        {
            None,
            Negative,
            Positive,
        }

        public float GetValue(eID eTableID)
        {
            uint tableID = (uint)eTableID;
            if(base.ContainsKey(tableID) == false) return 0;

            return base.GetData(tableID).value;
        }

        public Sprite GetIcon(uint tableID)
        {
            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, $"Status_{tableID}");
        }

        public eEFFECT_TYPE GetType(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return eEFFECT_TYPE.None;

            return (eEFFECT_TYPE)base.GetData(tableID).type;
        }
    }

    public class TableData_Status : iTableData
    {
        //tableID type strID value
        public uint tableID { get; set; }
        public int type { get; set; }
        public uint strID { get; set; }
        public float value { get; set; }
    }
}