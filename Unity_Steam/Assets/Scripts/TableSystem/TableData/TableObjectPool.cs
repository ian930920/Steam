using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableObjectPool : BaseTableData<TableObjectPool, TableData_ObjectPool>
    {
        public enum eID
        {
            Char_User = 1001,           //유저
            Char_Enemy,                 //적
            Char_SummonObj,             //소환물

            ReactiveUI_Button = 2001,   //반응형 버튼
            ReactiveUI_Gauge,           //일반 게이지
            UI_CharaterStatusBar,       //캐릭터 상태바
            Effect_Count,               //카운트

            Effect_ItemDrop = 3001,     //아이템 드랍 이펙트
            Effect_Defence,             //방어 이펙트
            Effect_Damage,              //피격 이펙트
            Effect_Heal,                //힐 이펙트
            
            Effect_Summon_Defence = 3017,
        }

        public enum eTYPE
        {
            Prefab_Root_Normal = 1,
            Prefab_Root_UI,
            FX_Root_Normal,
            FX_Root_UI,
        }
    }

    public class TableData_ObjectPool : TableData_Resource
    {
        //tableID type poolCount path
        public int poolCount { get; set; }
    }
}