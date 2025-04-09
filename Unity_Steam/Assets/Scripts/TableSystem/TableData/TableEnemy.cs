using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableEnemy : BaseTableData<TableEnemy, TableData_Enemy>
    {
        public enum eID
        {
            Enemy_1 = 1001,
        }

        public enum eTYPE
        {
            User,
            Enemy,
        }

        public Sprite GetSprite(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ProjectManager.Instance.Resource.GetSpriteByAtlas(ResourceManager.eATLAS_ID.Characer, base.GetData(tableID).strSprite);
        }
    }

    public class TableData_Enemy : iTableData
    {
        //tableID hp strength listSkillID resID strSprite
        public uint tableID { get; set; }
        public ulong hp { get; set; }
        public ulong strength { get; set; }
        public List<uint> listSkillID { get; set; }
        public uint resID { get; set; }
        public string strSprite { get; set; }
    }
}