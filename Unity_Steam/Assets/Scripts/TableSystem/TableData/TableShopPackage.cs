using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;

namespace TableData
{
    public class TableShopPackage : TableShopBase<TableShopPackage, TableData_ShopPackage>
    {
        public enum eID
        {
            AD_REMOVE = 1001,           //광고 제거 

            AutoMouse = 3001,           //자동 쥐잡기
            AutoServing,                //자동 주문받기
            AutoVisitStamp,             //자동 방문도장
            AutoClean,                  //자동 치우기
        }

        protected override void dataProcessing()
        {
            base.dataProcessing();

            base.m_dicShopByCateogry.Clear();
            base.m_dicRewardList.Clear();
            List<stItem> listReward = new List<stItem>();
            TableData_ShopPackage data = null;
            Dictionary<uint, TableData_ShopPackage>.Enumerator enumData = base.GetEnumerator();
            while(enumData.MoveNext())
            {
                data = enumData.Current.Value;
                if(data.expose != 1) continue;

                int nCategory = data.category;
                if(this.m_dicShopByCateogry.ContainsKey(nCategory) == false)
                {
                    this.m_dicShopByCateogry.Add(nCategory, new List<TableData_ShopPackage>());
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

        override public string GetString(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return $"{tableID} 없는 상품 ㅠ";

            return ProjectManager.Instance.Table.String.GetString(base.GetData(tableID).strID);
        }

        public bool IsActiveShop(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return false;
            if(string.IsNullOrEmpty(base.GetData(tableID).deadline) == true) return true;

            return (Convert.ToDateTime(base.GetData(tableID).deadline) - DateTime.Now).TotalSeconds > 0;
        }
    }

    public class TableData_ShopPackage : TableData_ShopBase
    {
        //deadline
        public string deadline { get; set; }
    }
}