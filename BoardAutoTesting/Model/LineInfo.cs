using System;
using System.Data;

namespace BoardAutoTesting.Model
{
    public class LineInfo
    {
        public LineInfo(DataRow drLineInfo)
        {
            CraftId = drLineInfo["Craft_Idx"].ToString();
            LineIdx = drLineInfo["Route_Name"].ToString();
            RouteName = drLineInfo["Line_Idx"].ToString();
            McuIp = drLineInfo["Mcu_Ip"].ToString();
            AteIp = drLineInfo["Ate_Ip"].ToString();
            CraftEsn = drLineInfo["Line_ESN"].ToString();
            LineEsn = drLineInfo["Craft_ESN"].ToString();
            PortId = drLineInfo["Port_Id"].ToString();
            IsRepair = Convert.ToBoolean(drLineInfo["Is_Repair"]);
            IsOut = Convert.ToBoolean(drLineInfo["Is_Out"].ToString());
        }

        public LineInfo()
        {
            // TODO: Complete member initialization
        }

        public string CraftId { get; set; }
        public string LineIdx { get; set; }
        public string RouteName { get; set; }
        public string McuIp { get; set; }
        public string AteIp { get; set; }
        public string CraftEsn { get; set; }
        public string LineEsn { get; set; }
        public bool IsRepair { get; set; }
        public bool IsOut { get; set; }
        public string PortId { get; set; }
    }
}