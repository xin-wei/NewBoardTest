using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Model
{
    public class ProductInfo
    {
        public string RFID { get; set; }
        public string ESN { get; set; }
        public string IsPass { get; set; }
        public string RouteName { get; set; }
        public string CraftID { get; set; }
        public string CurrentIp { get; set; }
        public string OldIp { get; set; }
        public string ActionName { get; set; }
        public string ATEIp { get; set; }
    }
}
