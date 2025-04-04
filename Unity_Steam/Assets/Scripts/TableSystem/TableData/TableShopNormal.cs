using System.Collections.Generic;
using System.Numerics;

namespace TableData
{
    public abstract class TableShopBase<T, D> : BaseTableData<T, D> where T : BaseTableData<T, D>  where D : TableData_ShopBase
    {
        public enum eTYPE
        {
            Normal = 1,
            Package,
        }

        public enum ePURCHASE_TYPE
        {
            IAP = 1,        //인앱결제
            IngameItem,     //인게임 재화
            AD,             //광고
        }

        public enum eLIMMIT_TYPE
        {
            Infinity = 1,   //무제한
            Account,        //계정당 1번
            Daily,          //일일 초기화
            Weekly,         //주간 초기화
            Monthly,        //월간 초기화
        }

        public enum eCATEGORY
        {
            Dia = 1,
            Costume,
            Food,
            Package,
        }

        //Key : Tab, Value : List<DataShop>
        protected Dictionary<int, List<D>> m_dicShopByCateogry = new Dictionary<int, List<D>>();

        //Key : ShopID, Value : List<stItem>
        protected Dictionary<int, List<stItem>> m_dicRewardList = new Dictionary<int, List<stItem>>();

        abstract public string GetString(int nShopID);
        
        public List<stItem> GetRewardList(int nShopID)
        {
            if(this.m_dicRewardList.ContainsKey(nShopID) == false) return new List<stItem>();

            return this.m_dicRewardList[nShopID];
        }

        public List<D> GetListByCategory(int nTab)
        {
            if(this.m_dicShopByCateogry.ContainsKey(nTab) == false) return new List<D>();

            return this.m_dicShopByCateogry[nTab];
        }

        public bool IsReadyAD(int nShopID)
        {
            if(base.ContainsKey(nShopID) == false) return false;
            
            if(this.GetCurrBuyableCount(nShopID) == 0) return false;

            //TODO Userdata
            //return UserDataManager.Instance.Time.IsActive(UserData_Time.eSAVE_TYPE.Shop, nShopID) == false;
            return true;
        }

        public bool IsDaily(int nShopID)
        {
            if(base.ContainsKey(nShopID) == false) return false;

            return base.GetData(nShopID).limitType == (int)eLIMMIT_TYPE.Daily;
        }

        public int GetCurrBuyableCount(int nShopID)
        {
            if(base.ContainsKey(nShopID) == false) return 0;

            //TODO Userdata
            //return base.GetData(nShopID).limitCount - UserDataManager.Instance.Shop.GetShopBuyCount(nShopID);
            return base.GetData(nShopID).limitCount - 0;
        }

        public bool IsADBuyable(int nShopID)
        {
            if(base.ContainsKey(nShopID) == false) return false;
            if(base.GetData(nShopID).purchaseType != (int)ePURCHASE_TYPE.AD) return false;

            //TODO Userdata
            //return base.GetData(nShopID).limitCount > UserDataManager.Instance.Shop.GetShopBuyCount(nShopID);
            return base.GetData(nShopID).limitCount > 0;
        }
    }

    public class TableData_ShopBase : iTableData
    {
        //tableID type productID purchaseType expose sort category strID strIcon limitType limitCount coolTime costItemID costValue listRewardItemID listRewardValue
        public int tableID { get; set; }
        public int type { get; set; }
        public string productID { get; set; }
        public int purchaseType { get; set; }
        public int expose { get; set; }
        public int sort { get; set; }
        public int category { get; set; }
        public int strID { get; set; }
        public string strIcon { get; set; }
        public int limitType { get; set; }
        public int limitCount { get; set; }
        public float coolTime { get; set; }
        public int costItemID { get; set; }
        public uint costValue { get; set; }
        public List<int> listRewardItemID { get; set; }
        public List<uint> listRewardValue { get; set; }
        public stItem stCost { get => new stItem(this.costItemID, this.costValue); }
    }

    public class TableShopNormal : TableShopBase<TableShopNormal, TableData_ShopNormal>
    {
        public enum eID
        {
            ChuruCharge_Dia = 402,      //츄루 충전 (옥)
            ChuruCharge_AD,             //츄루 충전 (츄루)
        }

        protected override void dataProcessing()
        {
            base.dataProcessing();

            base.m_dicShopByCateogry.Clear();
            base.m_dicRewardList.Clear();
            TableData_ShopNormal data = null;
            Dictionary<int, TableData_ShopNormal>.Enumerator enumData = base.GetEnumerator();
            while(enumData.MoveNext())
            {
                data = enumData.Current.Value;
                if(data.expose != 1) continue;

                int nCategory = data.category;
                if(this.m_dicShopByCateogry.ContainsKey(nCategory) == false)
                {
                    this.m_dicShopByCateogry.Add(nCategory, new List<TableData_ShopNormal>());
                }
                this.m_dicShopByCateogry[nCategory].Add(data);

                if(this.m_dicRewardList.ContainsKey(data.tableID) == false)
                {
                    this.m_dicRewardList.Add(data.tableID, new List<stItem>());
                }
                for(int i = 0, nMax = data.listRewardValue.Count; i < nMax; ++i)
                {
                    this.m_dicRewardList[data.tableID].Add(new stItem(data.listRewardItemID[i], data.listRewardValue[i]));
                }
            }
        }

        override public string GetString(int nShopID)
        {
            if(base.ContainsKey(nShopID) == false) return $"{nShopID} 없는 상품 ㅠ";

            TableData_ShopNormal data = base.GetData(nShopID);
            switch((eCATEGORY)data.category)
            {
                case eCATEGORY.Dia: return $"{ProjectManager.Instance.Table.Item.GetString_ItemCount(new stItem(data.listRewardItemID[0], data.listRewardValue[0]))}";

                case eCATEGORY.Food:
                case eCATEGORY.Costume:
                return string.Format(ProjectManager.Instance.Table.GetString(data.strID), data.listRewardValue[0]);
            }

            return $"{nShopID} 문제있는디";
        }

        public string GetStringBonus(int nShopID)
        {
            if(base.ContainsKey(nShopID) == false) return "";

            ulong uBonus = base.GetData(nShopID).bonusValue;
            if(uBonus == 0) return "";
            return $"+STR 보너스 {uBonus}+STR 개";
        }
    }

    public class TableData_ShopNormal : TableData_ShopBase
    {
        //bonusValue
        public ulong bonusValue { get; set; }
    }
}