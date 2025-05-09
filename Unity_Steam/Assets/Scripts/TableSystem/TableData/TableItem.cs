
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
            uint nItemID = (uint)eItemID;
            if(base.ContainsKey(nItemID) == false) return null;

            return base.GetData(nItemID);
        }

        public eINVEN_TYPE GetInvenType(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return eINVEN_TYPE.None;

		    return (eINVEN_TYPE)base.GetData(tableID).type;
        }

        public string GetString(uint tableID, TableString.eTYPE eStringType = TableString.eTYPE.Title)
        {
            if(base.ContainsKey(tableID) == false) return $"없는 애이템인데요 {tableID}";

            return TableManager.Instance.String.GetString(base.GetData(tableID).strID, eStringType);
        }

        public string GetString_ItemCount(stItem stItem)
        {
            if(base.ContainsKey(stItem.ItemID) == false) return $"없는 애이템인데요{stItem.ItemID}";

            return $"{this.GetString(stItem.ItemID)} {Utility_UI.GetCommaNumber(stItem.Count)} +STR 개";
        }

        public Sprite GetSprite(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, base.GetData(tableID).iconName);
        }
    }

    public class TableData_Item : iTableData
    {
        //tableID type resID strIcon strID grade material shortcutID
        public uint tableID { get; set; }
        public int type { get; set; }
        public uint resID { get; set; }
        public string iconName { get; set; }
        public uint strID { get; set; }
        public int material { get; set; }
        public uint shortcutID { get; set; }
    }
}

public struct stItem
{
    public uint ItemID;
    public uint Count;

    public TableData.TableItem.eINVEN_TYPE InvenType => TableManager.Instance.Item.GetInvenType(this.ItemID);

    public stItem(uint tableID)
    {
        this.ItemID = tableID;
        this.Count = 0;
    }

    public stItem(TableData.TableItem.eID eItemID)
    {
        this.ItemID = (uint)eItemID;
        this.Count = 0;
    }

    public stItem(uint nItemID, uint nCount)
    {
        this.ItemID = nItemID;
        this.Count = nCount;
    }

    public stItem(TableData.TableItem.eID eItemID, uint nCount)
    {
        this.ItemID = (uint)eItemID;
        this.Count = nCount;
    }
}