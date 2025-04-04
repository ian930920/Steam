using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableObjectPool : BaseTableData<TableObjectPool, TableData_ObjectPool>
    {
        public enum eID
        {
            Effect_Count = 1,           //ī��Ʈ
            Effect_ItemDrop,            //������ ��� ����Ʈ

            ReactiveUI_Button = 101,    //������ ��ư
            ReactiveUI_Gauge,           //�Ϲ� ������
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