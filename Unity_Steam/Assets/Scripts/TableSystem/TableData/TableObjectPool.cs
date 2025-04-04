using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableObjectPool : BaseTableData<TableObjectPool, TableData_ObjectPool>
    {
        public enum eID
        {
            Effect_Count = 1,           //카운트
            Effect_ItemDrop,            //아이템 드랍 이펙트

            ReactiveUI_Button = 101,    //반응형 버튼
            ReactiveUI_Gauge,           //일반 게이지
        }

        public enum eTYPE
        {
            Fx = 1,
            UI,
        }
    }

    public class TableData_ObjectPool : TableData_Resource
    {
        //tableID type poolCount path
        public int poolCount { get; set; }
    }
}