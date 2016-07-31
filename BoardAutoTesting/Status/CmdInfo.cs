namespace BoardAutoTesting.Status
{
    public class CmdInfo
    {
        public static string DoorOpen
        {
            get { return "*Door:Open#"; }
        }

        public static string DoorClose
        {
            get { return "*Door:Close#"; }
        }

        public static string OpenGet
        {
            get { return "*Open:Get#"; }
        }

        public static string CloseGet
        {
            get { return "*Close:Get#"; }
        }

        public static string ProductFail
        {
            get { return "*PRO:FAIL#"; }
        }

        public static string ProductPass
        {
            get { return "*PRO:PASS#"; }
        }

        public static string ProductGet
        {
            get { return "*PRO:GET#"; }
        }

        public static string CanIn
        {
            get { return ":IN?#"; }
        }

        public static string GoIn
        {
            get { return "*IN#"; }
        }

        public static string GoInGet
        {
            get { return "*IN:GET#"; }
        }

        public static string GoInOk
        {
            get { return "*IN:OK#"; }
        }

        public static string GoNext
        {
            get { return "*NEXT#"; }
        }

        public static string GoNextGet
        {
            get { return "*NEXT:GET#"; }
        }

        public static string GoNextOk
        {
            get { return "*NEXT:OK#"; }
        }

        public static string GoOut
        {
            get { return "*OUT#"; }
        }

        public static string GoOutGet
        {
            get { return "*OUT:GET#"; }
        }

        public static string GoOutOk
        {
            get { return "*OUT:OK#"; }
        }

        public static string GoRetest
        {
            get { return "*REP#"; }
        }

        public static string GoRetestGet
        {
            get { return "*REP:GET#"; }
        }

        public static string GoRetestOk
        {
            get { return "*REP:OK#"; }
        }

        public static string ResultPass
        {
            get { return "*RESULT:PASS#"; }
        }

        public static string ResultRetest
        {
            get { return "*RESULT:RETEST#"; }
        }

        public static string ResultFail
        {
            get { return "*RESULT:FAIL#"; }
        }

        public static string TestMac
        {
            get { return "*TEST:MAC?#"; }
        }

        public static string OutPut
        {
            get { return "*OUTPUT#"; }
        }

        public static string RLightOn
        {
            get { return "*R:ON#"; }
        }

        public static string RLightOff
        {
            get { return "*R:OFF#"; }
        }
    }
}