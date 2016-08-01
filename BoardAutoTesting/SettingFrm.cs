﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.Windows.Forms;
using BoardAutoTesting.BLL;
using BoardAutoTesting.Model;
using BoardAutoTesting.Properties;
using BoardAutoTesting.Service;
using Commons;

namespace BoardAutoTesting
{
    public partial class SettingFrm : DevComponents.DotNetBar.Office2007Form
    {
        public SettingFrm()
        {
            InitializeComponent();
            //处理掉DataGridViewComboBoxColumn绑定数据源后,再绑定到DataTable中的Column时,提示"System.ArgumentException:DagaGridViewComboBoxCell值无效"的错误
            dgvLineInfo.DataError += delegate { };
        }

        private tCheckDataTestAteSoapClient _ate = new tCheckDataTestAteSoapClient();

        private void SettingFrm_Load(object sender, EventArgs e)
        {
            DataGridViewComboBoxColumn cmbLineInfo =
                (DataGridViewComboBoxColumn) dgvLineInfo.Columns["Line_Idx"];
            if (cmbLineInfo == null)
            {
                MessageUtil.ShowError("未找到对应的列！");
                return;
            }
            cmbLineInfo.Items.Clear();

            DataGridViewComboBoxColumn cmbRouteInfo =
                (DataGridViewComboBoxColumn) dgvLineInfo.Columns["Route_Name"];
            if (cmbRouteInfo == null)
            {
                MessageUtil.ShowError("未找到对应的列！");
                return;
            }

            BasicHttpBinding binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue
            };
            string remoteAddress = _ate.Endpoint.Address.Uri.ToString();
            EndpointAddress endpoint = new EndpointAddress(remoteAddress);
            _ate = new tCheckDataTestAteSoapClient(binding, endpoint);

            try
            {
                /*加载线体*/
                Ping pNet = new Ping();
                pNet.Send("sfis.phicomm.com.cn", 1000);

                foreach (var item in _ate.Get_Line_List())
                    cmbLineInfo.Items.Add(item);

                /*加载途程*/
                DataTable dtRoute = new DataTable();
                //加载必须ATE工具过站站位
                string xmlRoute = _ate.Get_Route_Info(null, null, "1", null);

                DataSet dsStationListTemp1 = XmlConvertor.ConvertXMLToDataSet(xmlRoute);
                if (dsStationListTemp1.Tables.Count > 0)
                {
                    dtRoute = dsStationListTemp1.Tables[0].Clone();
                    foreach (DataRow item in dsStationListTemp1.Tables[0].Rows)
                    {
                        dtRoute.ImportRow(item);
                    }
                }

                //加载ATE与DCT过站站位
                xmlRoute = _ate.Get_Route_Info(null, null, "0", null);
                DataSet dsStationListTemp2 = XmlConvertor.ConvertXMLToDataSet(xmlRoute);
                if (dsStationListTemp2.Tables.Count > 0)
                {
                    if (dtRoute.Rows.Count <= 0)
                    {
                        dtRoute = dsStationListTemp2.Tables[0].Clone();
                    }
                    foreach (DataRow item in 
                        XmlConvertor.ConvertXMLToDataSet(xmlRoute).Tables[0].Rows)
                    {
                        dtRoute.ImportRow(item);
                    }
                }

                //加载ATE与DCT过站站位
                xmlRoute = _ate.Get_Route_Info(null, null, "3", null);
                DataSet dsStationListTemp3 = XmlConvertor.ConvertXMLToDataSet(xmlRoute);
                if (dsStationListTemp3.Tables.Count > 0)
                {
                    if (dtRoute.Rows.Count <= 0)
                    {
                        dtRoute = dsStationListTemp3.Tables[0].Clone();
                    }
                    foreach (DataRow item in 
                        XmlConvertor.ConvertXMLToDataSet(xmlRoute).Tables[0].Rows)
                    {
                        dtRoute.ImportRow(item);
                    }
                }
                if (dtRoute.Rows.Count <= 0)
                {
                    MessageUtil.ShowError("获取站位异常");
                    return;
                }
                dtRoute = DataTableHelper.SortedTable(dtRoute, "CRAFTNAME");
                dtRoute.TableName = "Dt_Route";
                XmlConvertor.ObjectToXml(dtRoute, false);

                foreach (DataRow item in dtRoute.Rows)
                {
                    cmbRouteInfo.Items.Add(item["CRAFTNAME"].ToString());
                }

            }
            catch (Exception)
            {
                //读取XML文件
                object lineLst =
                    XmlConvertor.XmlToObject(
                        "<?xml version='1.0' encoding='utf-8'?><ArrayOfString xmlnsasi='http://www.w3.org/2001/XMLSchema-instance' xmlnsasd='http://www.w3.org/2001/XMLSchema'><string>A060201</string><string>A060202</string><string>A060203</string><string>A060204</string><string>A080201</string><string>A080202</string><string>A080203</string><string>A080204</string><string>A080205</string><string>A090201</string><string>A090202</string><string>A090203</string><string>D060101</string><string>D060102</string><string>D060103</string><string>P080201</string><string>P080201</string><string>P080202</string><string>P080202</string><string>P080203</string><string>P080203</string><string>P080204</string><string>P080204</string><string>P080205</string><string>P080205</string><string>S060101</string><string>S060102</string><string>S060103</string><string>S060104</string><string>S060111</string><string>S080105</string><string>S080106</string><string>S080107</string><string>S080108</string><string>S080109</string><string>S080110</string><string>T060101</string><string>T060201</string><string>T060202</string><string>T060203</string><string>T060204</string><string>T061012</string><string>T100101</string><string>T100102</string><string>T100103</string><string>T100104</string><string>T100105</string><string>T100106</string><string>T101012</string></ArrayOfString>",
                        typeof (string[]));

                var enumerable = lineLst as string[];
                if (enumerable != null)
                    foreach (var item in enumerable)
                        cmbLineInfo.Items.Add(item);

                object routeLst =
                    XmlConvertor.XmlToObject(
                        "<?xml version='1.0' encoding='utf-8'?><DataTable><xs:schema id='NewDataSet' xmlns='' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:msdata='urn:schemas-microsoft-com:xml-msdata'><xs:element name='NewDataSet' msdata:IsDataSet='true' msdata:MainDataTable='Dt_Route' msdata:Locale='en-US'><xs:complexType><xs:choice minOccurs='0' maxOccurs='unbounded'><xs:element name='Dt_Route' msdata:CaseSensitive='False' msdata:Locale='en-US'><xs:complexType><xs:sequence><xs:element name='CRAFTID' type='xs:string' minOccurs='0' /><xs:element name='CRAFTNAME' type='xs:string' minOccurs='0' /><xs:element name='CRAFTPARAMETERURL' type='xs:string' minOccurs='0' /><xs:element name='BEWORKSEG' type='xs:string' minOccurs='0' /><xs:element name='TESTFLAG' type='xs:string' minOccurs='0' /><xs:element name='CHECKTOOLSFLAG' type='xs:string' minOccurs='0' /></xs:sequence></xs:complexType></xs:element></xs:choice></xs:complexType></xs:element></xs:schema><diffgr:diffgram xmlns:msdata='urn:schemas-microsoft-com:xml-msdata' xmlns:diffgr='urn:schemas-microsoft-com:xml-diffgram-v1'><DocumentElement><Dt_Route diffgr:id='Dt_Route1' msdata:rowOrder='0' diffgr:hasChanges='inserted'><CRAFTID>1008</CRAFTID><CRAFTNAME>1008</CRAFTNAME><CRAFTPARAMETERURL>1008</CRAFTPARAMETERURL><BEWORKSEG>WH</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route2' msdata:rowOrder='1' diffgr:hasChanges='inserted'><CRAFTID>B13042600000349</CRAFTID><CRAFTNAME>10G_TEST</CRAFTNAME><CRAFTPARAMETERURL>10G_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route3' msdata:rowOrder='2' diffgr:hasChanges='inserted'><CRAFTID>B13051200000373</CRAFTID><CRAFTNAME>1G_TEST</CRAFTNAME><CRAFTPARAMETERURL>1G_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route4' msdata:rowOrder='3' diffgr:hasChanges='inserted'><CRAFTID>B13042400000346</CRAFTID><CRAFTNAME>A_MAC_PRINT</CRAFTNAME><CRAFTPARAMETERURL>A_MAC_PRINT</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route5' msdata:rowOrder='4' diffgr:hasChanges='inserted'><CRAFTID>B13100600000487</CRAFTID><CRAFTNAME>A_MAC_PRINT_IN</CRAFTNAME><CRAFTPARAMETERURL>A_MAC_PRINT_IN</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route6' msdata:rowOrder='5' diffgr:hasChanges='inserted'><CRAFTID>B13041000000301</CRAFTID><CRAFTNAME>ADSL_3KM</CRAFTNAME><CRAFTPARAMETERURL>ADSL_3KM</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route7' msdata:rowOrder='6' diffgr:hasChanges='inserted'><CRAFTID>B13041000000302</CRAFTID><CRAFTNAME>ADSL_4KM</CRAFTNAME><CRAFTPARAMETERURL>ADSL_4KM</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route8' msdata:rowOrder='7' diffgr:hasChanges='inserted'><CRAFTID>B13032900000259</CRAFTID><CRAFTNAME>ANT_TEST</CRAFTNAME><CRAFTPARAMETERURL>ANT_TEST</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route9' msdata:rowOrder='8' diffgr:hasChanges='inserted'><CRAFTID>B14061800000264</CRAFTID><CRAFTNAME>ANT_TEST_G</CRAFTNAME><CRAFTPARAMETERURL>测试G网</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route10' msdata:rowOrder='9' diffgr:hasChanges='inserted'><CRAFTID>B13032900000255</CRAFTID><CRAFTNAME>ASS_CIT_TEST</CRAFTNAME><CRAFTPARAMETERURL>ASS_CIT_TEST</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route11' msdata:rowOrder='10' diffgr:hasChanges='inserted'><CRAFTID>B13032900000283</CRAFTID><CRAFTNAME>ASS_VI</CRAFTNAME><CRAFTPARAMETERURL>ASS_VI</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route12' msdata:rowOrder='11' diffgr:hasChanges='inserted'><CRAFTID>S5140067</CRAFTID><CRAFTNAME>ASSY_ANTMainTest</CRAFTNAME><CRAFTPARAMETERURL>ASSY_ANTMainTest</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route13' msdata:rowOrder='12' diffgr:hasChanges='inserted'><CRAFTID>S5140068</CRAFTID><CRAFTNAME>ASSY_ANTModuleTest</CRAFTNAME><CRAFTPARAMETERURL>ASSY_ANTModuleTest</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route14' msdata:rowOrder='13' diffgr:hasChanges='inserted'><CRAFTID>S5140074</CRAFTID><CRAFTNAME>ASSY_AudioTest</CRAFTNAME><CRAFTPARAMETERURL>ASSY_AudioTest</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route15' msdata:rowOrder='14' diffgr:hasChanges='inserted'><CRAFTID>S5140064</CRAFTID><CRAFTNAME>ASSY_BurningInput</CRAFTNAME><CRAFTPARAMETERURL>ASSY_BurningInput</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route16' msdata:rowOrder='15' diffgr:hasChanges='inserted'><CRAFTID>S5140065</CRAFTID><CRAFTNAME>ASSY_BurningOutput</CRAFTNAME><CRAFTPARAMETERURL>ASSY_BurningOutput</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route17' msdata:rowOrder='16' diffgr:hasChanges='inserted'><CRAFTID>S5140060</CRAFTID><CRAFTNAME>ASSY_DownloadMain</CRAFTNAME><CRAFTPARAMETERURL>ASSY_DownloadMain</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route18' msdata:rowOrder='17' diffgr:hasChanges='inserted'><CRAFTID>S5140061</CRAFTID><CRAFTNAME>ASSY_DownloadModule</CRAFTNAME><CRAFTPARAMETERURL>ASSY_DownloadModule</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route19' msdata:rowOrder='18' diffgr:hasChanges='inserted'><CRAFTID>S5140069</CRAFTID><CRAFTNAME>ASSY_IMEIWrite</CRAFTNAME><CRAFTPARAMETERURL>ASSY_IMEIWrite</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route20' msdata:rowOrder='19' diffgr:hasChanges='inserted'><CRAFTID>S5140059</CRAFTID><CRAFTNAME>ASSY_Input</CRAFTNAME><CRAFTPARAMETERURL>ASSY_Input</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route21' msdata:rowOrder='20' diffgr:hasChanges='inserted'><CRAFTID>S5140078</CRAFTID><CRAFTNAME>ASSY_IPQC</CRAFTNAME><CRAFTPARAMETERURL>ASSY_IPQC</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route22' msdata:rowOrder='21' diffgr:hasChanges='inserted'><CRAFTID>S5140070</CRAFTID><CRAFTNAME>ASSY_MACPrint</CRAFTNAME><CRAFTPARAMETERURL>ASSY_MACPrint</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route23' msdata:rowOrder='22' diffgr:hasChanges='inserted'><CRAFTID>B1610001</CRAFTID><CRAFTNAME>ASSY_MMITestElec</CRAFTNAME><CRAFTPARAMETERURL>ASSY_MMITestElec</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>3</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route24' msdata:rowOrder='23' diffgr:hasChanges='inserted'><CRAFTID>S5140062</CRAFTID><CRAFTNAME>ASSY_MMITestMain</CRAFTNAME><CRAFTPARAMETERURL>ASSY_MMITestMain</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route25' msdata:rowOrder='24' diffgr:hasChanges='inserted'><CRAFTID>S5140063</CRAFTID><CRAFTNAME>ASSY_MMITestModule</CRAFTNAME><CRAFTPARAMETERURL>ASSY_MMITestModule</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route26' msdata:rowOrder='25' diffgr:hasChanges='inserted'><CRAFTID>S5140073</CRAFTID><CRAFTNAME>ASSY_OpticalTest</CRAFTNAME><CRAFTPARAMETERURL>ASSY_OpticalTest</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route27' msdata:rowOrder='26' diffgr:hasChanges='inserted'><CRAFTID>S5140076</CRAFTID><CRAFTNAME>ASSY_PingTest</CRAFTNAME><CRAFTPARAMETERURL>ASSY_PingTest</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route28' msdata:rowOrder='27' diffgr:hasChanges='inserted'><CRAFTID>S5140075</CRAFTID><CRAFTNAME>ASSY_Reset</CRAFTNAME><CRAFTPARAMETERURL>ASSY_Reset</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>3</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route29' msdata:rowOrder='28' diffgr:hasChanges='inserted'><CRAFTID>B517001</CRAFTID><CRAFTNAME>ASSY_SensorTest</CRAFTNAME><CRAFTPARAMETERURL>ASSY_SensorTest</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route30' msdata:rowOrder='29' diffgr:hasChanges='inserted'><CRAFTID>B1546001</CRAFTID><CRAFTNAME>ASSY_SNPrint</CRAFTNAME><CRAFTPARAMETERURL>ASSY_SNPrint</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route31' msdata:rowOrder='30' diffgr:hasChanges='inserted'><CRAFTID>B1548001</CRAFTID><CRAFTNAME>ASSY_SNWrite</CRAFTNAME><CRAFTPARAMETERURL>ASSY_SNWrite</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route32' msdata:rowOrder='31' diffgr:hasChanges='inserted'><CRAFTID>S5140071</CRAFTID><CRAFTNAME>ASSY_ThroughputMain</CRAFTNAME><CRAFTPARAMETERURL>ASSY_ThroughputMain</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route33' msdata:rowOrder='32' diffgr:hasChanges='inserted'><CRAFTID>S5140072</CRAFTID><CRAFTNAME>ASSY_ThroughputModule</CRAFTNAME><CRAFTPARAMETERURL>ASSY_ThroughputModule</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route34' msdata:rowOrder='33' diffgr:hasChanges='inserted'><CRAFTID>S5140066</CRAFTID><CRAFTNAME>ASSY_USBTest</CRAFTNAME><CRAFTPARAMETERURL>ASSY_USBTest</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route35' msdata:rowOrder='34' diffgr:hasChanges='inserted'><CRAFTID>S5140077</CRAFTID><CRAFTNAME>ASSY_VI</CRAFTNAME><CRAFTPARAMETERURL>ASSY_VI</CRAFTPARAMETERURL><BEWORKSEG>Assembly</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route36' msdata:rowOrder='35' diffgr:hasChanges='inserted'><CRAFTID>B13041100000309</CRAFTID><CRAFTNAME>BOX_PRINT</CRAFTNAME><CRAFTPARAMETERURL>BOX_PRINT</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route37' msdata:rowOrder='36' diffgr:hasChanges='inserted'><CRAFTID>B13060500000391</CRAFTID><CRAFTNAME>BT_C</CRAFTNAME><CRAFTPARAMETERURL>BT_C</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route38' msdata:rowOrder='37' diffgr:hasChanges='inserted'><CRAFTID>B13032700000239</CRAFTID><CRAFTNAME>BT_MAIN</CRAFTNAME><CRAFTPARAMETERURL>BT_MAIN</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route39' msdata:rowOrder='38' diffgr:hasChanges='inserted'><CRAFTID>B13032700000241</CRAFTID><CRAFTNAME>BT_MODUL</CRAFTNAME><CRAFTPARAMETERURL>BT_MODUL</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route40' msdata:rowOrder='39' diffgr:hasChanges='inserted'><CRAFTID>B13032900000252</CRAFTID><CRAFTNAME>BURN_IN</CRAFTNAME><CRAFTPARAMETERURL>BURN_IN</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route41' msdata:rowOrder='40' diffgr:hasChanges='inserted'><CRAFTID>B14011700000089</CRAFTID><CRAFTNAME>BURN_IN_I</CRAFTNAME><CRAFTPARAMETERURL>BURN_IN_I</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route42' msdata:rowOrder='41' diffgr:hasChanges='inserted'><CRAFTID>B439001</CRAFTID><CRAFTNAME>BURN_IN_II</CRAFTNAME><CRAFTPARAMETERURL>BURN_IN_II</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route43' msdata:rowOrder='42' diffgr:hasChanges='inserted'><CRAFTID>B14011700000090</CRAFTID><CRAFTNAME>BURN_IN_O</CRAFTNAME><CRAFTPARAMETERURL>BURN_IN_O</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route44' msdata:rowOrder='43' diffgr:hasChanges='inserted'><CRAFTID>B427002</CRAFTID><CRAFTNAME>BURNINGSET_TEST</CRAFTNAME><CRAFTPARAMETERURL>BURNINGSET_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route45' msdata:rowOrder='44' diffgr:hasChanges='inserted'><CRAFTID>B13041900000339</CRAFTID><CRAFTNAME>CAMERA_TEST</CRAFTNAME><CRAFTPARAMETERURL>CAMERA_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route46' msdata:rowOrder='45' diffgr:hasChanges='inserted'><CRAFTID>B13032900000284</CRAFTID><CRAFTNAME>CARTON_LABLE_PRINT</CRAFTNAME><CRAFTPARAMETERURL>CARTON_LABLE_PRINT</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route47' msdata:rowOrder='46' diffgr:hasChanges='inserted'><CRAFTID>B13041400000319</CRAFTID><CRAFTNAME>CHK_MAC</CRAFTNAME><CRAFTPARAMETERURL>CHK_MAC</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route48' msdata:rowOrder='47' diffgr:hasChanges='inserted'><CRAFTID>B505001</CRAFTID><CRAFTNAME>CHUCE_1</CRAFTNAME><CRAFTPARAMETERURL>CHUCE</CRAFTPARAMETERURL><BEWORKSEG>SMT</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route49' msdata:rowOrder='48' diffgr:hasChanges='inserted'><CRAFTID>B13032700000237</CRAFTID><CRAFTNAME>CIT_TEST</CRAFTNAME><CRAFTPARAMETERURL>CIT_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route50' msdata:rowOrder='49' diffgr:hasChanges='inserted'><CRAFTID>B427001</CRAFTID><CRAFTNAME>DIAGSNAKE_TEST</CRAFTNAME><CRAFTPARAMETERURL>DIAGSNAKE_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route51' msdata:rowOrder='50' diffgr:hasChanges='inserted'><CRAFTID>S5140008</CRAFTID><CRAFTNAME>DIP_ICT</CRAFTNAME><CRAFTPARAMETERURL>DIP_ICT</CRAFTPARAMETERURL><BEWORKSEG>DIP</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route52' msdata:rowOrder='51' diffgr:hasChanges='inserted'><CRAFTID>S5140006</CRAFTID><CRAFTNAME>DIP_Input</CRAFTNAME><CRAFTPARAMETERURL>DIP_Input</CRAFTPARAMETERURL><BEWORKSEG>DIP</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route53' msdata:rowOrder='52' diffgr:hasChanges='inserted'><CRAFTID>S5140009</CRAFTID><CRAFTNAME>DIP_IPQC</CRAFTNAME><CRAFTPARAMETERURL>DIP_IPQC</CRAFTPARAMETERURL><BEWORKSEG>DIP</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route54' msdata:rowOrder='53' diffgr:hasChanges='inserted'><CRAFTID>S5140007</CRAFTID><CRAFTNAME>DIP_VI</CRAFTNAME><CRAFTPARAMETERURL>DIP_VI</CRAFTPARAMETERURL><BEWORKSEG>DIP</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route55' msdata:rowOrder='54' diffgr:hasChanges='inserted'><CRAFTID>B13032700000231</CRAFTID><CRAFTNAME>DL_MAIN</CRAFTNAME><CRAFTPARAMETERURL>DL_MAIN</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route56' msdata:rowOrder='55' diffgr:hasChanges='inserted'><CRAFTID>B13032700000233</CRAFTID><CRAFTNAME>DL_MODUL</CRAFTNAME><CRAFTPARAMETERURL>DL_MODUL</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route57' msdata:rowOrder='56' diffgr:hasChanges='inserted'><CRAFTID>B13051400000378</CRAFTID><CRAFTNAME>DX_TEST</CRAFTNAME><CRAFTPARAMETERURL>DX_TEST</CRAFTPARAMETERURL><BEWORKSEG>SMT</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route58' msdata:rowOrder='57' diffgr:hasChanges='inserted'><CRAFTID>B427003</CRAFTID><CRAFTNAME>FACTORYSET_TEST</CRAFTNAME><CRAFTPARAMETERURL>FACTORYSET_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route59' msdata:rowOrder='58' diffgr:hasChanges='inserted'><CRAFTID>B13102100000499</CRAFTID><CRAFTNAME>FQC</CRAFTNAME><CRAFTPARAMETERURL>FQC_A_T_P</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route60' msdata:rowOrder='59' diffgr:hasChanges='inserted'><CRAFTID>B428001</CRAFTID><CRAFTNAME>FQC.2</CRAFTNAME><CRAFTPARAMETERURL>FQC</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route61' msdata:rowOrder='60' diffgr:hasChanges='inserted'><CRAFTID>B438001</CRAFTID><CRAFTNAME>FT_4G</CRAFTNAME><CRAFTPARAMETERURL>FT_4G</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route62' msdata:rowOrder='61' diffgr:hasChanges='inserted'><CRAFTID>B13032700000245</CRAFTID><CRAFTNAME>FT_MAIN</CRAFTNAME><CRAFTPARAMETERURL>FT_MAIN</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route63' msdata:rowOrder='62' diffgr:hasChanges='inserted'><CRAFTID>B13032700000244</CRAFTID><CRAFTNAME>FT_MODUL</CRAFTNAME><CRAFTPARAMETERURL>FT_MODUL</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route64' msdata:rowOrder='63' diffgr:hasChanges='inserted'><CRAFTID>B13111800000520</CRAFTID><CRAFTNAME>LABLE_BINDING</CRAFTNAME><CRAFTPARAMETERURL>LABLE_BINDING</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route65' msdata:rowOrder='64' diffgr:hasChanges='inserted'><CRAFTID>B510001</CRAFTID><CRAFTNAME>LOCK</CRAFTNAME><CRAFTPARAMETERURL>LOCK_DEVICE</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route66' msdata:rowOrder='65' diffgr:hasChanges='inserted'><CRAFTID>B13121300000528</CRAFTID><CRAFTNAME>LUMINOUS_POWER_TEST</CRAFTNAME><CRAFTPARAMETERURL>LUMINOUS_POWER_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route67' msdata:rowOrder='66' diffgr:hasChanges='inserted'><CRAFTID>B13041000000299</CRAFTID><CRAFTNAME>MAC_WRITE</CRAFTNAME><CRAFTPARAMETERURL>MAC_WRITE</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route68' msdata:rowOrder='67' diffgr:hasChanges='inserted'><CRAFTID>B13032900000266</CRAFTID><CRAFTNAME>MII_WORK</CRAFTNAME><CRAFTPARAMETERURL>MII_WORK</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route69' msdata:rowOrder='68' diffgr:hasChanges='inserted'><CRAFTID>B14060800000246</CRAFTID><CRAFTNAME>MIRROR_IMAGE_DL</CRAFTNAME><CRAFTPARAMETERURL>Mirror-Image-DL</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route70' msdata:rowOrder='69' diffgr:hasChanges='inserted'><CRAFTID>NA</CRAFTID><CRAFTNAME>NA</CRAFTNAME><CRAFTPARAMETERURL>NA</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route71' msdata:rowOrder='70' diffgr:hasChanges='inserted'><CRAFTID>S5140102</CRAFTID><CRAFTNAME>PACK_AttachmentLink</CRAFTNAME><CRAFTPARAMETERURL>PACK_AttachmentLink</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route72' msdata:rowOrder='71' diffgr:hasChanges='inserted'><CRAFTID>S5140101</CRAFTID><CRAFTNAME>PACK_BoxPrint</CRAFTNAME><CRAFTPARAMETERURL>PACK_BoxPrint</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route73' msdata:rowOrder='72' diffgr:hasChanges='inserted'><CRAFTID>B13032900000267</CRAFTID><CRAFTNAME>PACK_CARTON</CRAFTNAME><CRAFTPARAMETERURL>PACK_CARTON</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route74' msdata:rowOrder='73' diffgr:hasChanges='inserted'><CRAFTID>S5140106</CRAFTID><CRAFTNAME>PACK_FQC</CRAFTNAME><CRAFTPARAMETERURL>PACK_FQC</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route75' msdata:rowOrder='74' diffgr:hasChanges='inserted'><CRAFTID>S5140098</CRAFTID><CRAFTNAME>PACK_Input</CRAFTNAME><CRAFTPARAMETERURL>PACK_Input</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route76' msdata:rowOrder='75' diffgr:hasChanges='inserted'><CRAFTID>S5140099</CRAFTID><CRAFTNAME>PACK_LableLink</CRAFTNAME><CRAFTPARAMETERURL>PACK_LableLink</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route77' msdata:rowOrder='76' diffgr:hasChanges='inserted'><CRAFTID>B528001</CRAFTID><CRAFTNAME>PACK_MIIWork</CRAFTNAME><CRAFTPARAMETERURL>PACK_MIIWork</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route78' msdata:rowOrder='77' diffgr:hasChanges='inserted'><CRAFTID>S5140103</CRAFTID><CRAFTNAME>PACK_PackCarton</CRAFTNAME><CRAFTPARAMETERURL>PACK_PackCarton</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route79' msdata:rowOrder='78' diffgr:hasChanges='inserted'><CRAFTID>S5140104</CRAFTID><CRAFTNAME>PACK_PackPallet</CRAFTNAME><CRAFTPARAMETERURL>PACK_PackPallet</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route80' msdata:rowOrder='79' diffgr:hasChanges='inserted'><CRAFTID>B13032900000269</CRAFTID><CRAFTNAME>PACK_PALLET</CRAFTNAME><CRAFTPARAMETERURL>PACK_PALLET</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route81' msdata:rowOrder='80' diffgr:hasChanges='inserted'><CRAFTID>S5140100</CRAFTID><CRAFTNAME>PACK_Reset</CRAFTNAME><CRAFTPARAMETERURL>PACK_Reset</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route82' msdata:rowOrder='81' diffgr:hasChanges='inserted'><CRAFTID>S5140107</CRAFTID><CRAFTNAME>PACK_StoreIn</CRAFTNAME><CRAFTPARAMETERURL>PACK_StoreIn</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route83' msdata:rowOrder='82' diffgr:hasChanges='inserted'><CRAFTID>S5140105</CRAFTID><CRAFTNAME>PACK_VI</CRAFTNAME><CRAFTPARAMETERURL>PACK_VI</CRAFTPARAMETERURL><BEWORKSEG>Pack</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route84' msdata:rowOrder='83' diffgr:hasChanges='inserted'><CRAFTID>B431001</CRAFTID><CRAFTNAME>PACK_VI_2</CRAFTNAME><CRAFTPARAMETERURL>PACK_VI_2</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route85' msdata:rowOrder='84' diffgr:hasChanges='inserted'><CRAFTID>S5140030</CRAFTID><CRAFTNAME>PCBA_AudioTest</CRAFTNAME><CRAFTPARAMETERURL>PCBA_AudioTest</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route86' msdata:rowOrder='85' diffgr:hasChanges='inserted'><CRAFTID>S5140014</CRAFTID><CRAFTNAME>PCBA_BurningInput</CRAFTNAME><CRAFTPARAMETERURL>PCBA_BurningInput</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route87' msdata:rowOrder='86' diffgr:hasChanges='inserted'><CRAFTID>S5140015</CRAFTID><CRAFTNAME>PCBA_BurningOutput</CRAFTNAME><CRAFTPARAMETERURL>PCBA_BurningOutput</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route88' msdata:rowOrder='87' diffgr:hasChanges='inserted'><CRAFTID>S5140031</CRAFTID><CRAFTNAME>PCBA_Check</CRAFTNAME><CRAFTPARAMETERURL>PCBA_Check</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route89' msdata:rowOrder='88' diffgr:hasChanges='inserted'><CRAFTID>B14010900000083</CRAFTID><CRAFTNAME>PCBA_CHK</CRAFTNAME><CRAFTPARAMETERURL /><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route90' msdata:rowOrder='89' diffgr:hasChanges='inserted'><CRAFTID>B13101000000495</CRAFTID><CRAFTNAME>PCBA_CIT</CRAFTNAME><CRAFTPARAMETERURL>PCBA_CIT</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route91' msdata:rowOrder='90' diffgr:hasChanges='inserted'><CRAFTID>S5140023</CRAFTID><CRAFTNAME>PCBA_CITTest</CRAFTNAME><CRAFTPARAMETERURL>PCBA_CITTest</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>3</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route92' msdata:rowOrder='91' diffgr:hasChanges='inserted'><CRAFTID>S5140016</CRAFTID><CRAFTNAME>PCBA_DownloadMain</CRAFTNAME><CRAFTPARAMETERURL>PCBA_DownloadMain</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>3</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route93' msdata:rowOrder='92' diffgr:hasChanges='inserted'><CRAFTID>S5140017</CRAFTID><CRAFTNAME>PCBA_DownloadModule</CRAFTNAME><CRAFTPARAMETERURL>PCBA_DownloadModule</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route94' msdata:rowOrder='93' diffgr:hasChanges='inserted'><CRAFTID>B1616001</CRAFTID><CRAFTNAME>PCBA_ExtractCIT</CRAFTNAME><CRAFTPARAMETERURL>PCBA_ExtractCIT</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route95' msdata:rowOrder='94' diffgr:hasChanges='inserted'><CRAFTID>S5140036</CRAFTID><CRAFTNAME>PCBA_FQC</CRAFTNAME><CRAFTPARAMETERURL>PCBA_FQC</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route96' msdata:rowOrder='95' diffgr:hasChanges='inserted'><CRAFTID>B1544002</CRAFTID><CRAFTNAME>PCBA_IMEIWRITE</CRAFTNAME><CRAFTPARAMETERURL>PCBA_IMEIWrite</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route97' msdata:rowOrder='96' diffgr:hasChanges='inserted'><CRAFTID>S5140013</CRAFTID><CRAFTNAME>PCBA_Input</CRAFTNAME><CRAFTPARAMETERURL>PCBA_Input</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route98' msdata:rowOrder='97' diffgr:hasChanges='inserted'><CRAFTID>S5140035</CRAFTID><CRAFTNAME>PCBA_IPQC</CRAFTNAME><CRAFTPARAMETERURL>PCBA_IPQC</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route99' msdata:rowOrder='98' diffgr:hasChanges='inserted'><CRAFTID>S5140024</CRAFTID><CRAFTNAME>PCBA_MACWrite</CRAFTNAME><CRAFTPARAMETERURL>PCBA_MACWrite</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route100' msdata:rowOrder='99' diffgr:hasChanges='inserted'><CRAFTID>S5140029</CRAFTID><CRAFTNAME>PCBA_OpticalTest</CRAFTNAME><CRAFTPARAMETERURL>PCBA_OpticalTest</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route101' msdata:rowOrder='100' diffgr:hasChanges='inserted'><CRAFTID>S5140033</CRAFTID><CRAFTNAME>PCBA_PackCarton</CRAFTNAME><CRAFTPARAMETERURL>PCBA_PackCarton</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route102' msdata:rowOrder='101' diffgr:hasChanges='inserted'><CRAFTID>S5140034</CRAFTID><CRAFTNAME>PCBA_PackPallet</CRAFTNAME><CRAFTPARAMETERURL>PCBA_PackPallet</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route103' msdata:rowOrder='102' diffgr:hasChanges='inserted'><CRAFTID>S5140019</CRAFTID><CRAFTNAME>PCBA_RFCALMAIN</CRAFTNAME><CRAFTPARAMETERURL>PCBA_RFCalMain</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>3</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route104' msdata:rowOrder='103' diffgr:hasChanges='inserted'><CRAFTID>S5140021</CRAFTID><CRAFTNAME>PCBA_RFTestMain</CRAFTNAME><CRAFTPARAMETERURL>PCBA_RFTestMain</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route105' msdata:rowOrder='104' diffgr:hasChanges='inserted'><CRAFTID>S5140022</CRAFTID><CRAFTNAME>PCBA_RFTestModule</CRAFTNAME><CRAFTPARAMETERURL>PCBA_RFTestModule</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route106' msdata:rowOrder='105' diffgr:hasChanges='inserted'><CRAFTID>S5140018</CRAFTID><CRAFTNAME>PCBA_SNWrite</CRAFTNAME><CRAFTPARAMETERURL>PCBA_SNWrite</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route107' msdata:rowOrder='106' diffgr:hasChanges='inserted'><CRAFTID>S5140037</CRAFTID><CRAFTNAME>PCBA_StoreIn</CRAFTNAME><CRAFTPARAMETERURL>PCBA_StoreIn</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route108' msdata:rowOrder='107' diffgr:hasChanges='inserted'><CRAFTID>B13041700000332</CRAFTID><CRAFTNAME>PCBA_THORU_TEST</CRAFTNAME><CRAFTPARAMETERURL>PCBA_THORU_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route109' msdata:rowOrder='108' diffgr:hasChanges='inserted'><CRAFTID>S5140027</CRAFTID><CRAFTNAME>PCBA_ThroughputMain</CRAFTNAME><CRAFTPARAMETERURL>PCBA_ThroughputMain</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route110' msdata:rowOrder='109' diffgr:hasChanges='inserted'><CRAFTID>S5140028</CRAFTID><CRAFTNAME>PCBA_ThroughputModule</CRAFTNAME><CRAFTPARAMETERURL>PCBA_ThroughputModule</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route111' msdata:rowOrder='110' diffgr:hasChanges='inserted'><CRAFTID>S5140032</CRAFTID><CRAFTNAME>PCBA_VI</CRAFTNAME><CRAFTPARAMETERURL>PCBA_VI</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route112' msdata:rowOrder='111' diffgr:hasChanges='inserted'><CRAFTID>S5140025</CRAFTID><CRAFTNAME>PCBA_WiFiTestMain</CRAFTNAME><CRAFTPARAMETERURL>PCBA_WiFiTestMain</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route113' msdata:rowOrder='112' diffgr:hasChanges='inserted'><CRAFTID>S5140026</CRAFTID><CRAFTNAME>PCBA_WiFiTestModule</CRAFTNAME><CRAFTPARAMETERURL>PCBA_WiFiTestModule</CRAFTPARAMETERURL><BEWORKSEG>PCBA_Test</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route114' msdata:rowOrder='113' diffgr:hasChanges='inserted'><CRAFTID>B13041500000327</CRAFTID><CRAFTNAME>PING_TEST</CRAFTNAME><CRAFTPARAMETERURL>PING_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route115' msdata:rowOrder='114' diffgr:hasChanges='inserted'><CRAFTID>B13041100000308</CRAFTID><CRAFTNAME>PRODUCT_PRINT</CRAFTNAME><CRAFTPARAMETERURL>PRODUCT_PRINT</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route116' msdata:rowOrder='115' diffgr:hasChanges='inserted'><CRAFTID>B452001</CRAFTID><CRAFTNAME>RESET</CRAFTNAME><CRAFTPARAMETERURL>RESET</CRAFTPARAMETERURL><BEWORKSEG>ASSEMBLY</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route117' msdata:rowOrder='116' diffgr:hasChanges='inserted'><CRAFTID>B14061900000286</CRAFTID><CRAFTNAME>RESET_TEST</CRAFTNAME><CRAFTPARAMETERURL>RESET_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route118' msdata:rowOrder='117' diffgr:hasChanges='inserted'><CRAFTID>B13041000000300</CRAFTID><CRAFTNAME>REST_TEST</CRAFTNAME><CRAFTPARAMETERURL>REST_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route119' msdata:rowOrder='118' diffgr:hasChanges='inserted'><CRAFTID>B13042600000351</CRAFTID><CRAFTNAME>RS232_TEST</CRAFTNAME><CRAFTPARAMETERURL>RS232_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route120' msdata:rowOrder='119' diffgr:hasChanges='inserted'><CRAFTID>B13042600000353</CRAFTID><CRAFTNAME>RS485_TEST</CRAFTNAME><CRAFTPARAMETERURL>RS485_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route121' msdata:rowOrder='120' diffgr:hasChanges='inserted'><CRAFTID>S5140001</CRAFTID><CRAFTNAME>SMT_Input</CRAFTNAME><CRAFTPARAMETERURL>SMT_Input</CRAFTPARAMETERURL><BEWORKSEG>SMT</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route122' msdata:rowOrder='121' diffgr:hasChanges='inserted'><CRAFTID>S5140003</CRAFTID><CRAFTNAME>SMT_IPQC</CRAFTNAME><CRAFTPARAMETERURL>SMT_IPQC</CRAFTPARAMETERURL><BEWORKSEG>SMT</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route123' msdata:rowOrder='122' diffgr:hasChanges='inserted'><CRAFTID>B537001</CRAFTID><CRAFTNAME>SMT_OUT</CRAFTNAME><CRAFTPARAMETERURL>SMT_OUT</CRAFTPARAMETERURL><BEWORKSEG>SMT</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route124' msdata:rowOrder='123' diffgr:hasChanges='inserted'><CRAFTID>B522001</CRAFTID><CRAFTNAME>SMT_STOCKIN</CRAFTNAME><CRAFTPARAMETERURL>SMT_STOCKIN</CRAFTPARAMETERURL><BEWORKSEG>SMT</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route125' msdata:rowOrder='124' diffgr:hasChanges='inserted'><CRAFTID>S5140002</CRAFTID><CRAFTNAME>SMT_VI</CRAFTNAME><CRAFTPARAMETERURL>SMT_VI</CRAFTPARAMETERURL><BEWORKSEG>SMT</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route126' msdata:rowOrder='125' diffgr:hasChanges='inserted'><CRAFTID>B13032700000235</CRAFTID><CRAFTNAME>SN_WRITE</CRAFTNAME><CRAFTPARAMETERURL>SN_WRITE</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route127' msdata:rowOrder='126' diffgr:hasChanges='inserted'><CRAFTID>B13032900000270</CRAFTID><CRAFTNAME>STOCK_IN</CRAFTNAME><CRAFTPARAMETERURL>STOCK_IN</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route128' msdata:rowOrder='127' diffgr:hasChanges='inserted'><CRAFTID>B13071600000410</CRAFTID><CRAFTNAME>THPUT30DB_TEST</CRAFTNAME><CRAFTPARAMETERURL>THPUT30DB_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route129' msdata:rowOrder='128' diffgr:hasChanges='inserted'><CRAFTID>B450001</CRAFTID><CRAFTNAME>THPUT35DB_TEST</CRAFTNAME><CRAFTPARAMETERURL>THPUT35DB_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route130' msdata:rowOrder='129' diffgr:hasChanges='inserted'><CRAFTID>B450002</CRAFTID><CRAFTNAME>THPUT70DB_TEST</CRAFTNAME><CRAFTPARAMETERURL>THPUT35DB_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route131' msdata:rowOrder='130' diffgr:hasChanges='inserted'><CRAFTID>B13071600000413</CRAFTID><CRAFTNAME>THPUT75DB_TEST</CRAFTNAME><CRAFTPARAMETERURL>THPUT75DB_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route132' msdata:rowOrder='131' diffgr:hasChanges='inserted'><CRAFTID>B13051000000369</CRAFTID><CRAFTNAME>THROU_TEST</CRAFTNAME><CRAFTPARAMETERURL>THROU_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route133' msdata:rowOrder='132' diffgr:hasChanges='inserted'><CRAFTID>B14061900000276</CRAFTID><CRAFTNAME>THROUGHPUT_2.4G_TEST</CRAFTNAME><CRAFTPARAMETERURL>Throughput_2.4G_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route134' msdata:rowOrder='133' diffgr:hasChanges='inserted'><CRAFTID>B14061900000277</CRAFTID><CRAFTNAME>THROUGHPUT_5.8G_TEST</CRAFTNAME><CRAFTPARAMETERURL>Throughput5.8G_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route135' msdata:rowOrder='134' diffgr:hasChanges='inserted'><CRAFTID>B13041100000314</CRAFTID><CRAFTNAME>THROUGHPUT_TEST</CRAFTNAME><CRAFTPARAMETERURL>THROUGHPUT_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route136' msdata:rowOrder='135' diffgr:hasChanges='inserted'><CRAFTID>B13051000000362</CRAFTID><CRAFTNAME>UPDATE_FW</CRAFTNAME><CRAFTPARAMETERURL>UPDATE_FW</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route137' msdata:rowOrder='136' diffgr:hasChanges='inserted'><CRAFTID>B14042300000182</CRAFTID><CRAFTNAME>WAN_TEST</CRAFTNAME><CRAFTPARAMETERURL>WNA_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route138' msdata:rowOrder='137' diffgr:hasChanges='inserted'><CRAFTID>B13081600000430</CRAFTID><CRAFTNAME>WEB_TEST</CRAFTNAME><CRAFTPARAMETERURL>WEB_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route139' msdata:rowOrder='138' diffgr:hasChanges='inserted'><CRAFTID>B14012100000091</CRAFTID><CRAFTNAME>WEIGHT</CRAFTNAME><CRAFTPARAMETERURL>WEIGHT</CRAFTPARAMETERURL><BEWORKSEG>PACK</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route140' msdata:rowOrder='139' diffgr:hasChanges='inserted'><CRAFTID>B14061900000278</CRAFTID><CRAFTNAME>WIFI_2.4G_TEST</CRAFTNAME><CRAFTPARAMETERURL>WIFI_2.4G_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route141' msdata:rowOrder='140' diffgr:hasChanges='inserted'><CRAFTID>B14061900000280</CRAFTID><CRAFTNAME>WIFI_5.8G_TEST</CRAFTNAME><CRAFTPARAMETERURL>WIFI_5.8G_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route142' msdata:rowOrder='141' diffgr:hasChanges='inserted'><CRAFTID>B13041300000317</CRAFTID><CRAFTNAME>WIFI_TEST</CRAFTNAME><CRAFTPARAMETERURL>WIFI_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>1</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route><Dt_Route diffgr:id='Dt_Route143' msdata:rowOrder='142' diffgr:hasChanges='inserted'><CRAFTID>B13060800000395</CRAFTID><CRAFTNAME>ZJ_TEST</CRAFTNAME><CRAFTPARAMETERURL>ZJ_TEST</CRAFTPARAMETERURL><BEWORKSEG>TEST</BEWORKSEG><TESTFLAG>0</TESTFLAG><CHECKTOOLSFLAG /></Dt_Route></DocumentElement></diffgr:diffgram></DataTable>",
                        typeof (DataTable));
                var dataTable = routeLst as DataTable;
                if (dataTable != null)
                    foreach (DataRow item in dataTable.Rows)
                        cmbRouteInfo.Items.Add(item["CRAFTNAME"].ToString());
            }
            dgvLineInfo.AutoGenerateColumns = false;
            ShowInfo();
        }

        public void ShowInfo()
        {
            List<LineInfo> lstLines = LineBll.GetModels();
            if (lstLines == null)
            {
                MessageUtil.ShowError("没有线体信息");
                Application.Exit();
                return;
            }

            dgvLineInfo.Rows.Clear();
            foreach (var line in lstLines)
            {
                dgvLineInfo.Rows.Add(line.CraftId, line.LineIdx, line.RouteName,
                    line.McuIp, line.AteIp, line.IsRepair, line.IsOut, line.PortId);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ((from DataGridViewRow row in dgvLineInfo.Rows 
                 select new LineInfo
            {
                McuIp = row.Cells["MCU_IP"].Value.ToString(),
                AteIp = row.Cells["ATE_IP"].Value.ToString(),
                LineIdx = row.Cells["Line_Idx"].Value.ToString(),
                PortId = row.Cells["Port_Id"].Value.ToString(),
                IsRepair = row.Cells["b_repair"].Value != null &&
                Convert.ToBoolean(row.Cells["b_repair"].Value),

                IsOut = row.Cells["b_Last"].Value != null &&
                Convert.ToBoolean(row.Cells["b_Last"].Value),

                RouteName = row.Cells["Route_Name"].Value.ToString(),
                CraftId = row.Cells["Craft_Idx"].Value.ToString()
            }).Any(line => !LineBll.SureToUpdateModel(line, "Craft_Idx")))
            {
                MessageUtil.ShowError(Resources.UpdateError);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void dgvLineInfo_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            dgvLineInfo.ClearSelection();
            dgvLineInfo.Rows[e.RowIndex].Selected = true;
            dgvLineInfo.CurrentCell =
                dgvLineInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void btnNewCraft_Click(object sender, EventArgs e)
        {
            InstallCraftFrm frm = new InstallCraftFrm(this);
            frm.ShowDialog();
            ShowInfo();
        }

        private void btnDeleteCraft_Click(object sender, EventArgs e)
        {
            LineInfo line = new LineInfo();
            if (dgvLineInfo.CurrentRow == null) return;
            line.CraftId = dgvLineInfo.CurrentRow.Cells["Craft_Idx"].Value.ToString();
            LineBll.DeleteModel(line);
            ShowInfo();
        }

        private void dgvLineInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            LineInfo line = new LineInfo
            {
                McuIp = dgvLineInfo.Rows[e.RowIndex].Cells["MCU_IP"].Value.ToString(),
                AteIp = dgvLineInfo.Rows[e.RowIndex].Cells["ATE_IP"].Value.ToString(),
                LineIdx = dgvLineInfo.Rows[e.RowIndex].Cells["Line_Idx"].Value.ToString(),
                PortId = dgvLineInfo.Rows[e.RowIndex].Cells["Port_Id"].Value.ToString(),
                IsRepair = dgvLineInfo.Rows[e.RowIndex].Cells["b_repair"].Value != null &&
                Convert.ToBoolean(dgvLineInfo.Rows[e.RowIndex].Cells["b_repair"].Value),

                IsOut = dgvLineInfo.Rows[e.RowIndex].Cells["b_Last"].Value != null &&
                Convert.ToBoolean(dgvLineInfo.Rows[e.RowIndex].Cells["b_Last"].Value),

                RouteName = dgvLineInfo.Rows[e.RowIndex].Cells["Route_Name"].Value.ToString(),
                CraftId = dgvLineInfo.Rows[e.RowIndex].Cells["Craft_Idx"].Value.ToString()
            };
            if (!LineBll.SureToUpdateModel(line, "Craft_Idx"))
            {
                MessageUtil.ShowError(Resources.UpdateError);
            }
        }

        private void dgvLineInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewComboBoxColumn combo = dgvLineInfo.Columns[e.ColumnIndex] as 
                DataGridViewComboBoxColumn;
            if (combo == null) return;

            dgvLineInfo.BeginEdit(false);
            DataGridViewComboBoxEditingControl comboEdit = dgvLineInfo.EditingControl as 
                DataGridViewComboBoxEditingControl;
            if (comboEdit != null)
            {
                comboEdit.DroppedDown = true;
            }

            DataGridViewTextBoxColumn textBox = dgvLineInfo.Columns[e.ColumnIndex] as
                DataGridViewTextBoxColumn;
            if (textBox != null)
            {
                dgvLineInfo.BeginEdit(true);
            }
        }

    }
}
