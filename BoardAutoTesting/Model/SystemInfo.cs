using System;

namespace BoardAutoTesting.Model
{
    [Serializable]
    public class SystemInfo
    {
        public bool IsLogin { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public string PartNumber { get; set; }

        public string PartName { get; set; }

        public string LineId { get; set; }

        public string WoId { get; set; }

    }
}