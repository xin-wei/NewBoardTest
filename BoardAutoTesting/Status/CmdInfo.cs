namespace BoardAutoTesting.Status
{
    public class CmdInfo
    {
        /// <summary>
        /// *IN:busy#，有板子在测试
        /// </summary>
        public static string InBusy
        {
            get { return "*IN:busy#"; }
        }

        /// <summary>
        /// *OUT:busy#，线体非空
        /// </summary>
        public static string OutBusy
        {
            get { return "*OUT:busy#"; }
        }

        /// <summary>
        /// *OUT:idle#，没有板子在测试
        /// </summary>
        public static string OutIdle
        {
            get { return "*OUT:idle#"; }
        }

        /// <summary>
        /// *NEXT:busy#，线体非空
        /// </summary>
        public static string NextBusy
        {
            get { return "*NEXT:busy#"; }
        }

        /// <summary>
        /// *REP:exception#，没有板子在测试
        /// </summary>
        public static string RepException
        {
            get { return "*REP:exception#"; }
        }

        /// <summary>
        /// 遇到不属于任何途程的产品，蜂鸣器报警
        /// </summary>
        public static string Beep
        {
            get { return "*BEEP#"; }
        }

        /// <summary>
        /// *Door:Open#
        /// </summary>
        public static string DoorOpen
        {
            get { return "*Door:Open#"; }
        }

        /// <summary>
        /// *Door:Close#
        /// </summary>
        public static string DoorClose
        {
            get { return "*Door:Close#"; }
        }

        /// <summary>
        /// *Open:Get#
        /// </summary>
        public static string OpenGet
        {
            get { return "*Open:Get#"; }
        }

        /// <summary>
        /// *Close:Get#
        /// </summary>
        public static string CloseGet
        {
            get { return "*Close:Get#"; }
        }

        /// <summary>
        /// *PRO:FAIL#
        /// </summary>
        public static string ProductFail
        {
            get { return "*PRO:FAIL#"; }
        }

        /// <summary>
        /// *PRO:PASS#
        /// </summary>
        public static string ProductPass
        {
            get { return "*PRO:PASS#"; }
        }

        /// <summary>
        /// *PRO:GET#
        /// </summary>
        public static string ProductGet
        {
            get { return "*PRO:GET#"; }
        }

        /// <summary>
        /// :IN?#
        /// </summary>
        public static string CanIn
        {
            get { return ":IN?#"; }
        }

        /// <summary>
        /// *IN#
        /// </summary>
        public static string GoIn
        {
            get { return "*IN#"; }
        }

        /// <summary>
        /// *IN:GET#
        /// </summary>
        public static string GoInGet
        {
            get { return "*IN:GET#"; }
        }

        /// <summary>
        /// *IN:OK#
        /// </summary>
        public static string GoInOk
        {
            get { return "*IN:OK#"; }
        }

        /// <summary>
        /// *NEXT#
        /// </summary>
        public static string GoNext
        {
            get { return "*NEXT#"; }
        }

        /// <summary>
        /// *NEXT:GET#
        /// </summary>
        public static string GoNextGet
        {
            get { return "*NEXT:GET#"; }
        }

        /// <summary>
        /// *NEXT:OK#
        /// </summary>
        public static string GoNextOk
        {
            get { return "*NEXT:OK#"; }
        }

        /// <summary>
        /// *OUT#
        /// </summary>
        public static string GoOut
        {
            get { return "*OUT#"; }
        }

        /// <summary>
        /// *OUT:GET#
        /// </summary>
        public static string GoOutGet
        {
            get { return "*OUT:GET#"; }
        }

        /// <summary>
        /// *OUT:OK#
        /// </summary>
        public static string GoOutOk
        {
            get { return "*OUT:OK#"; }
        }

        /// <summary>
        /// *REP#
        /// </summary>
        public static string GoRetest
        {
            get { return "*REP#"; }
        }

        /// <summary>
        /// *REP:GET#
        /// </summary>
        public static string GoRetestGet
        {
            get { return "*REP:GET#"; }
        }

        /// <summary>
        /// *REP:OK#
        /// </summary>
        public static string GoRetestOk
        {
            get { return "*REP:OK#"; }
        }

        /// <summary>
        /// *RESULT:PASS#
        /// </summary>
        public static string ResultPass
        {
            get { return "*RESULT:PASS#"; }
        }

        /// <summary>
        /// *RESULT:RETEST#
        /// </summary>
        public static string ResultRetest
        {
            get { return "*RESULT:RETEST#"; }
        }

        /// <summary>
        /// *RESULT:FAIL#
        /// </summary>
        public static string ResultFail
        {
            get { return "*RESULT:FAIL#"; }
        }

        /// <summary>
        /// *TEST:MAC?#
        /// </summary>
        public static string TestMac
        {
            get { return "*TEST:MAC?#"; }
        }

        /// <summary>
        /// *OUTPUT#
        /// </summary>
        public static string OutPut
        {
            get { return "*OUTPUT#"; }
        }

        /// <summary>
        /// *R:ON#
        /// </summary>
        public static string RLightOn
        {
            get { return "*R:ON#"; }
        }

        /// <summary>
        /// *R:OFF#
        /// </summary>
        public static string RLightOff
        {
            get { return "*R:OFF#"; }
        }
    }
}