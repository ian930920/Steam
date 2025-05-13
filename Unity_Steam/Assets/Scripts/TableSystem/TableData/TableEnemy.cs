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

        private Dictionary<int, List<uint>> m_dicByType = new Dictionary<int, List<uint>>();

        protected override void dataProcessing()
        {
            base.dataProcessing();

            var enumData = base.GetEnumerator();
            while(enumData.MoveNext())
            {
                var type = enumData.Current.Value.type;
                if(this.m_dicByType.ContainsKey(type) == false)
                {
                    this.m_dicByType.Add(type, new List<uint>());
                }

                var tableID = enumData.Current.Key;
                if(this.m_dicByType[type].Contains(tableID) == true) continue;
                this.m_dicByType[type].Add(tableID);
            }
        }

        public Sprite GetSprite(uint tableID)
        {
            if(base.ContainsKey(tableID) == false) return null;

            return ResourceManager.Instance.GetSpriteByAtlas(ResourceManager.eATLAS_ID.Character, base.GetData(tableID).strSprite);
        }

        public uint GetRandomEnemyByType(int nType)
        {
            if(this.m_dicByType.ContainsKey(nType) == false) return (uint)eID.Enemy_1;

            return this.m_dicByType[nType][Random.Range(0, this.m_dicByType[nType].Count)];
        }
    }

    public class TableData_Enemy : TableData_Character
    {
        //tableID type hp strength listSkillID resID strSprite
        public int type { get; set; }
        public int strength { get; set; }
        public List<uint> listSkillID { get; set; }
    }
}