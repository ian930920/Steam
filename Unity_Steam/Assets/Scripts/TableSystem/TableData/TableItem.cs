using UnityEngine;

namespace TableData
{
    public class TableItem : BaseTableData<TableItem, TableData_Item>
    {
        public enum eID
	    {
            Ticket = 101,
            Dia,
	    }

        public enum eTYPE
	    {
            Cunsume = 1,
            Rune,
	    }

        public TableData_Item GetData(eID eItemID)
        {
            uint nItemID = (uint)eItemID;
            if(base.ContainsKey(nItemID) == false) return null;

            return base.GetData(nItemID);
        }

        public string GetString(uint tableID, TableString.eTYPE eStringType = TableString.eTYPE.Title)
        {
            if(base.ContainsKey(tableID) == false) return $"@@@ {tableID}는 없는 애이템ㅠㅠ";

            return TableManager.Instance.String.GetString(base.GetData(tableID).strID, eStringType);
        }

        public string GetString_ItemCount(stItem stItem)
        {
            if(base.ContainsKey(stItem.ItemID) == false) return $"@@@ {stItem.ItemID}는 없는 애이템ㅠㅠ";

            switch((eID)stItem.ItemID)
            {
                case eID.Ticket:
                return $"{this.GetString(stItem.ItemID)} {Utility_UI.GetCommaNumber(stItem.Count)}장";
            }

            return $"{this.GetString(stItem.ItemID)} {Utility_UI.GetCommaNumber(stItem.Count)}개";
        }

        public Sprite GetIcon(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.UI, base.GetData(tableID).strIcon);
        }
    }

    public class TableData_Item : iTableData
    {
        //tableID type strIcon strID
        public uint tableID { get; set; }
        public int type { get; set; }
        public string strIcon { get; set; }
        public uint strID { get; set; }
    }
}

public struct stItem
{
    public uint ItemID;
    public int Count;
    public TableData.TableItem.eTYPE ItemType => (TableData.TableItem.eTYPE)TableManager.Instance.Item.GetData(ItemID).type;

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

    public stItem(uint nItemID, int nCount)
    {
        this.ItemID = nItemID;
        this.Count = nCount;
    }

    public stItem(TableData.TableItem.eID eItemID, int nCount)
    {
        this.ItemID = (uint)eItemID;
        this.Count = nCount;
    }
}