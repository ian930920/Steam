using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableUser : BaseTableData<TableUser, TableData_User>
    {
        public enum eID
        {
            User = 1,
        }

        public enum eTYPE
        {
            User,
            Enemy,
        }

        public Sprite GetSprite(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.Characer, base.GetData(tableID).strSprite);
        }
    }

    public class TableData_Character : iTableData
    {
        //tableID hp strength resID strSprite
        public uint tableID { get; set; }
        public int hp { get; set; }
        public uint resID { get; set; }
        public string strSprite { get; set; }
    }

    public class TableData_User : TableData_Character
    {
        //tableID hp maxMana resID strSprite
        public int maxMana { get; set; }
    }
}