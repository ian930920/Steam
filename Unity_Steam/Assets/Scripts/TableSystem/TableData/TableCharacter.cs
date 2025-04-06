using System.Collections.Generic;
using UnityEngine;

namespace TableData
{
    public class TableCharacter : BaseTableData<TableCharacter, TableData_Character>
    {
        public enum eID
        {
            User = 1,

            Enemy_1 = 10001,
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

    public class TableData_Character : iTableData
    {
        //tableID hp listSkillID resID strSprite
        public uint tableID { get; set; }
        public ulong hp { get; set; }
        public List<uint> listSkillID { get; set; }
        public uint resID { get; set; }
        public string strSprite { get; set; }
    }
}