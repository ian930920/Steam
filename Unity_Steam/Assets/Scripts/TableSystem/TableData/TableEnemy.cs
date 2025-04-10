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

    public class TableData_Enemy : TableData_Character
    {
        //tableID hp strength listSkillID resID strSprite
        public ulong strength { get; set; }
        public List<uint> listSkillID { get; set; }
    }
}