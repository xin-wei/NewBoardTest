using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using LabelManager2;

namespace BoardAutoTesting.Model
{
    public class CodeSoftHelper
    {
        ApplicationClass _lbl;
        Document _doc;
        int _printQty;

        public enum EnumCounterBase
        {
            LppxBaseBinary = 2,
            LppxBaseOctal = 8,
            LppxBaseDecimal = 10,
            LppxBaseHexadecimal = 16,
            LppxBaseAlphabetic = 26,
            LppxBaseAlphaNumeric = 36,
            LppxBaseCustom = 255,
        }

        public void OpenCodeSoft()
        {
            _lbl = new ApplicationClass();
        }

        /// <summary>
        /// 检查模板路径是否存在
        /// </summary>
        /// <param name="filePatch"></param>
        public bool CheckFileExist(string filePatch)
        {
            return File.Exists(filePatch);
        }

        /// <summary>
        /// 调用模板
        /// </summary>
        /// <param name="filePatch"></param>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public string OpenLabelFile(string filePatch, bool readOnly)
        {
            try
            {
                _lbl.Documents.Open(filePatch, readOnly); // 调用设计好的label文件
                _doc = _lbl.ActiveDocument;
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 填充模板变量
        /// </summary>
        /// <param name="dicVariables"></param>
        public void Fill_Label_Variables(Dictionary<string, object> dicVariables)
        {
            foreach (KeyValuePair<string, object> variables in dicVariables)
            {
                try
                {
                    _doc.Variables.FormVariables.Item(variables.Key).Value = variables.Value.ToString(); //给参数传值
                    SendLog(string.Format("Fill Variable[{0}]-->{1}", variables.Key, variables.Value));
                }
                catch
                {
                    // ignored
                }
            }
        }

        /// <summary>
        /// 清除模板变量默认值
        /// </summary>
        public void ClearVariables()
        {
            for (int i = 0; i < _doc.Variables.FormVariables.Count; i++)
            {
                _doc.Variables.FormVariables.Item(_doc.Variables.FormVariables.Item(i + 1).Name).Value = "";
            }
        }

        /// <summary>
        /// 设置打印数量
        /// </summary>
        /// <param name="num"></param>
        public void SetPrintNum(int num)
        {
            _printQty = num;
        }

        /// <summary>
        /// 设置打印坐标
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public void SetPrintPosition(decimal positionX, decimal positionY)
        {
            _doc.Format.MarginLeft = Convert.ToInt32(positionX*100);
            _doc.Format.MarginTop = Convert.ToInt32(positionY*100);
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        public void PrintLabel()
        {
            SendLog("PrintQty-->" + _printQty);
            _doc.PrintDocument(_printQty);
        }

        /// <summary>
        /// 退出CodeSoft
        /// </summary>
        public void QuitCodeSoft()
        {
            try
            {
                _lbl.Quit();
            }
            catch
            {
                // ignored
            }
        }

        public void SendLog(string strLog)
        {
            _deleEvent = SaveLog;
            _deleEvent.BeginInvoke(strLog, null, null);
        }

        delegate void RunEvent(string strLog);

        RunEvent _deleEvent;

        private void SaveLog(string strLog)
        {
            #region 存储失败日志在服务器

            try
            {
                string todayDate = System.DateTime.Now.ToString("yyyyMMdd");
                string filePatch = Environment.CurrentDirectory + "\\log";
                if (!Directory.Exists(filePatch))
                    Directory.CreateDirectory(filePatch);
                //如果文件a.txt存在就打开，不存在就新建 .append 是追加写 
                FileStream fst = new FileStream(string.Format("{0}\\{1}.log", filePatch, todayDate), FileMode.Append);
                //写数据到a.txt格式 
                StreamWriter swt = new StreamWriter(fst, System.Text.Encoding.GetEncoding("utf-8"));
                swt.WriteLine(strLog + "  Time:" + System.DateTime.Now.ToString(CultureInfo.InvariantCulture));
                swt.Close();
                fst.Close();
            }
            catch
            {
                // ignored
            }

            #endregion
        }
    }
}