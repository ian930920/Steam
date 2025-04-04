namespace TableData
{
    public class TableDefine : BaseTableData<TableDefine, TableData_Define>
    {
        public enum eID
        {
        }

        public ulong GetUlongValue(eID eID)
        {
            return base.GetData((uint)eID).uValue;
        }

        public float GetFloatValue(eID eID)
        {
            return base.GetData((uint)eID).fValue;
        }
    }

    public class TableData_Define : iTableData
    {
        //tableID uValue fValue
        public uint tableID { get; set; }
        public ulong uValue { get; set; }
        public float fValue { get; set; }
    }
}