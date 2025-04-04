
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace TableData
{
    public class TableItem : BaseTableData<TableItem, TableData_Item>
    {
        public enum eINVEN_TYPE
        {
            None = 0,
            Currency,
        }

        public enum eID
	    {
            Gold = 101,
            Dia,
	    }

        public TableData_Item GetData(eID eItemID)
        {
            int nItemID = (int)eItemID;
            if(base.ContainsKey(nItemID) == false) return null;

            return base.GetData(nItemID);
        }

        public eINVEN_TYPE GetInvenType(int nItemID)
        {
            if(base.ContainsKey(nItemID) == false) return eINVEN_TYPE.None;

		    return (eINVEN_TYPE)base.GetData(nItemID).type;
        }

        public string GetString(int nItemID, TableString.eTYPE eStringType = TableString.eTYPE.Title)
        {
            if(base.ContainsKey(nItemID) == false) return $"없는 애이템인데요 {nItemID}";

            return ProjectManager.Instance.Table.GetString(base.GetData(nItemID).strID, eStringType);
        }

        public string GetString_ItemCount(stItem stItem)
        {
            if(base.ContainsKey(stItem.ItemID) == false) return $"없는 애이템인데요{stItem.ItemID}";

            return $"{this.GetString(stItem.ItemID)} {Utility_UI.GetCommaNumber(stItem.Count)} +STR 개";
        }

        public Sprite GetSprite(int nItemID)
        {
            if(base.ContainsKey(nItemID) == false) return null;

            return ProjectManager.Instance.Resource.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, base.GetData(nItemID).iconName);
        }
    }

    public class TableData_Item : iTableData
    {
        //tableID type resID iconName strID material ShortcutID
        public int tableID { get; set; }
        public int type { get; set; }
        public int resID { get; set; }
        public string iconName { get; set; }
        public int strID { get; set; }
        public int material { get; set; }
        public int ShortcutID { get; set; }
    }
}

public struct stItem
{
    public int ItemID;
    public uint Count;

    public TableData.TableItem.eINVEN_TYPE InvenType => ProjectManager.Instance.Table.Item.GetInvenType(this.ItemID);

    public stItem(int nItemID)
    {
        this.ItemID = nItemID;
        this.Count = 0;
    }

    public stItem(TableData.TableItem.eID eItemID)
    {
        this.ItemID = (int)eItemID;
        this.Count = 0;
    }

    public stItem(int nItemID, uint nCount)
    {
        this.ItemID = nItemID;
        this.Count = nCount;
    }

    public stItem(TableData.TableItem.eID eItemID, uint nCount)
    {
        this.ItemID = (int)eItemID;
        this.Count = nCount;
    }
}