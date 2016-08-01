using System;
using System.Drawing;
using DevComponents.DotNetBar;

namespace BoardAutoTesting.Model
{
    public class OperationControl
    {
        /// <summary>
        /// 提示消息类型
        /// </summary>
        public enum TypeList { Incoming, Outgoing, Normal, Warning, Error, Pass, Running }

        /// <summary>
        /// 提示消息文字颜色
        /// </summary>
        private static readonly Color[] ColorList = 
        { Color.Green, Color.Blue, Color.Black, Color.Orange, Color.Red, Color.Green, Color.Yellow };

        public static void ShowStatus(LabelItem lblStatus, TypeList msgtype, string msg)
        {
            try
            {
                lblStatus.Invoke(new EventHandler(delegate
                {
                    lblStatus.BackColor = ColorList[(int)msgtype];
                    lblStatus.Text = msg;
                }));
            }
            catch
            {
                // ignored
            }
        }
    }
}