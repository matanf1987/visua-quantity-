using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryWPF
{
    public class ExamineeData
    {


        public Dictionary<string, string> paramsMap = new Dictionary<string, string>();
        public string userName { get; set; }
        public string participantNumber { get; set; }

        public string CellColor1 { get; set; }
        public string CellColor2 { get; set; }
        public string CellColor3 { get; set; }
        public string CellColor4 { get; set; }
        public string CellColor5 { get; set; }
        public string CellColor6 { get; set; }
        public string CellColor7 { get; set; }
        public string CellColor8 { get; set; }
        public string CellColor9 { get; set; }
        public string CellColor10 { get; set; }
        public string CellColor11 { get; set; }
        public string CellColor12 { get; set; }
        public string CellColor13 { get; set; }
        public string CellColor14 { get; set; }
        public string CellColor15 { get; set; }
        public string CellColor16 { get; set; }
        public string CellColor17 { get; set; }
        public string CellColor18 { get; set; }
        public string CellColor19 { get; set; }
        public string CellColor20 { get; set; }

        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string Date3 { get; set; }
        public string Date4 { get; set; }
        public string Date5 { get; set; }
        public string Date6 { get; set; }
        public string Date7 { get; set; }
        public string Date8 { get; set; }
        public string Date9 { get; set; }
        public string Date10 { get; set; }
        public string Date11 { get; set; }
        public string Date12 { get; set; }
        public string Date13 { get; set; }
        public string Date14 { get; set; }
        public string Date15 { get; set; }
        public string Date16 { get; set; }
        public string Date17 { get; set; }
        public string Date18 { get; set; }
        public string Date19 { get; set; }
        public string Date20 { get; set; }

        public string ToolTip1 { get; set; }
        public string ToolTip2 { get; set; }
        public string ToolTip3 { get; set; }
        public string ToolTip4 { get; set; }
        public string ToolTip5 { get; set; }
        public string ToolTip6 { get; set; }
        public string ToolTip7 { get; set; }
        public string ToolTip8 { get; set; }
        public string ToolTip9 { get; set; }
        public string ToolTip10 { get; set; }
        public string ToolTip11 { get; set; }
        public string ToolTip12 { get; set; }
        public string ToolTip13 { get; set; }
        public string ToolTip14 { get; set; }
        public string ToolTip15 { get; set; }
        public string ToolTip16 { get; set; }
        public string ToolTip17 { get; set; }
        public string ToolTip18 { get; set; }
        public string ToolTip19 { get; set; }
        public string ToolTip20 { get; set; }

        public ExamineeData()
        {


            for (int i = 1; i <= 20; i++)
            {
                paramsMap["ToolTip" + i] = "";
                paramsMap["CellColor" + i] = "";
                paramsMap["Date" + i] = "";
            }

        }

        public void UpdateGlobalParams()
        {
            CellColor1 = paramsMap["CellColor1"];
            CellColor2 = paramsMap["CellColor2"];
            CellColor3 = paramsMap["CellColor3"];
            CellColor4 = paramsMap["CellColor4"];
            CellColor5 = paramsMap["CellColor5"];
            CellColor6 = paramsMap["CellColor6"];
            CellColor7 = paramsMap["CellColor7"];
            CellColor8 = paramsMap["CellColor8"];
            CellColor9 = paramsMap["CellColor9"];
            CellColor10 = paramsMap["CellColor10"];
            CellColor11 = paramsMap["CellColor11"];
            CellColor12 = paramsMap["CellColor12"];
            CellColor13 = paramsMap["CellColor13"];
            CellColor14 = paramsMap["CellColor14"];
            CellColor15 = paramsMap["CellColor15"];
            CellColor16 = paramsMap["CellColor16"];
            CellColor17 = paramsMap["CellColor17"];
            CellColor18 = paramsMap["CellColor18"];
            CellColor19 = paramsMap["CellColor19"];
            CellColor20 = paramsMap["CellColor20"];

            Date1 = paramsMap["Date1"];
            Date2 = paramsMap["Date2"];
            Date3 = paramsMap["Date3"];
            Date4 = paramsMap["Date4"];
            Date5 = paramsMap["Date5"];
            Date6 = paramsMap["Date6"];
            Date7 = paramsMap["Date7"];
            Date8 = paramsMap["Date8"];
            Date9 = paramsMap["Date9"];
            Date10 = paramsMap["Date10"];
            Date11 = paramsMap["Date11"];
            Date12 = paramsMap["Date12"];
            Date13 = paramsMap["Date13"];
            Date14 = paramsMap["Date14"];
            Date15 = paramsMap["Date15"];
            Date16 = paramsMap["Date16"];
            Date17 = paramsMap["Date17"];
            Date18 = paramsMap["Date18"];
            Date19 = paramsMap["Date19"];
            Date20 = paramsMap["Date20"];

            ToolTip1 = paramsMap["ToolTip1"];
            ToolTip2 = paramsMap["ToolTip2"];
            ToolTip3 = paramsMap["ToolTip3"];
            ToolTip4 = paramsMap["ToolTip4"];
            ToolTip5 = paramsMap["ToolTip5"];
            ToolTip6 = paramsMap["ToolTip6"];
            ToolTip7 = paramsMap["ToolTip7"];
            ToolTip8 = paramsMap["ToolTip8"];
            ToolTip9 = paramsMap["ToolTip9"];
            ToolTip10 = paramsMap["ToolTip10"];
            ToolTip11 = paramsMap["ToolTip11"];
            ToolTip12 = paramsMap["ToolTip12"];
            ToolTip13 = paramsMap["ToolTip13"];
            ToolTip14 = paramsMap["ToolTip14"];
            ToolTip15 = paramsMap["ToolTip15"];
            ToolTip16 = paramsMap["ToolTip16"];
            ToolTip17 = paramsMap["ToolTip17"];
            ToolTip18 = paramsMap["ToolTip18"];
            ToolTip19 = paramsMap["ToolTip19"];
            ToolTip20 = paramsMap["ToolTip20"];




        }
    }
}
