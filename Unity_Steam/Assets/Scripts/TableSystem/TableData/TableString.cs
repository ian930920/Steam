using System.Runtime.InteropServices;

namespace TableData
{
    public class TableString : BaseTableData<TableString, TableData_String>
    {
        public enum eID
        {
            None = 0,
            
            GameTitle = 1,      //게임 타이틀
            Confirm,            //확인
            Cancle,             //취소
            Time_Sec,           //초
            Time_Min,           //분
            Time_Hour,          //시간
        }

        public enum eLANGUAGE
        {
            KO,     //한국어
            EN,     //영어
            JA,     //일본어
            CHS,    //중국어 간체
            CHT,    //중국어 번체
            NONE,   //설정안함
        }

        public enum eTYPE
        {
            Title,
            Description
        }

        protected override void dataProcessing()
        {
            base.m_listData.Clear();
            for(int i = 0; i < base.m_listData.Count; ++i)
            {
                base.m_listData[i].ReplaceFormat();
            }
        }

        public string GetString(uint tableID, eTYPE eType = eTYPE.Title)
        {
            if(base.ContainsKey(tableID) == false)
            {
                ProjectManager.Instance.LogError($"StringData : {tableID}는 존재하지 않는 키");
                return "X";
            }

            return base.GetData(tableID).GetLang(eLANGUAGE.KO, eType);
        }

        public string GetString(eID eStringID, eTYPE eType = eTYPE.Title)
        {
            uint tableID = (uint)eStringID;

            return this.GetString(tableID, eType);
        }
    }

    public class TableData_String : iTableData
    {
        //tableID title desc
        public uint tableID { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        //public string engtitle { get; set; }
        //public string engdes { get; set; }

        public void ReplaceFormat()
        {
            title = title.Replace("\\n", "\n");
            desc = desc.Replace("\\n", "\n");
            //engtitle = engtitle.Replace("\\n", "\n");
            //engdes = engdes.Replace("\\n", "\n");
        }

        public string GetLang(TableString.eLANGUAGE eLang, TableString.eTYPE eType)
        {
            switch(eLang)
            {
                case TableString.eLANGUAGE.KO:
                return eType == TableString.eTYPE.Title ? title : desc;
                //case TableString.eLANGUAGE.EN: return eType == TableString.eTYPE.Title ? engtitle : engdes;
                //case Define.eLANGUAGE.JA: return Japanese;
                //case Define.eLANGUAGE.CHS: return Chinese_s;
                //case Define.eLANGUAGE.CHT: return Chinese_t;
            }
            return "WrongLanguageID";
        }
    }
}