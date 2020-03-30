using Kyoto.BUS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Kyoto.TEXT_DEFINE;

namespace Kyoto.GUI
{

    //*********** AUTHOR: PHẠM DUY KHÁNH***********/
    //*********** TEAM DEV: EDS***********/
    //*********** VERSION: 1.0***********/
    //*********** Content: Phần Chọn Phôi***********/
    public partial class frmSelectWorkPiece : Form
    {
        struct DataParameter
        {
            public int Process;
            public int Delay;
        }
        private DataParameter _inputDatameter;

        double PI_NUMBER = 3.1415;
        BUS_DATA_ALL manuTypeBus = new BUS_DATA_ALL();
        int ROWCOUNTCOMPONENT = 0;
        int COUNT_GROUP_BOX = 0;
        int HEIGHT_GROUPBOX_INPUT = 196;
        int HEIGHT_GROUPBOX_OUTPUT = 68;
        int WIDTH_GROUPBOX_INPUT = 1407;
        int WIDTH_GROUPBOX_OUTPUT = 1407;

        int HEIGHT_PANEL_INPUT = 171;
        int HEIGHT_PANEL_OUTPUT = 46;
        int WIDTH_PANEL_INPUT = 1395;
        int WIDTH_PANEL_OUTPUT = 1395;

        int CHECKBOX_HEIGHT = 17;
        int CHECKBOX_WIDTH = 32;

        int CBO_DONHAM_HEIGHT = 15;
        int CBO_DONHAM_WIDTH = 59;


        public string MESSAGE_WARNING_DELETE = "";
        public string BUTTON_ADD_TEXT = "";
        public string BUTTON_REMOVE_TEXT = "";
        public string BUTTON_CALCULATE_TEXT = "";
        public string TEXT_ACTIVE_CHECKBOXMASTER = "";
        public string ERROR1 = "";
        public string WARNING_DELETE_ROW_1 = "";
        public string TEXT_ALL_DO_NHAM = "";
        public string WARNING_NO_COMPONENT = "";
        public string TEXT_LBL_SHAPE = "";
        public string TEXT_LBL_MATERIAL = "";
        public string TEXT_LBL_PROCESS = "";
        public string TEXT_LBL_WEIGH = "";
        public string WAITING_CALCULATE_MESSAGE = "";
        public string PAM = "";
        public string TC = "";
        public string SELECTION_TC_PAM = "";
        public string PAM_DIALOG = "";

        List<int> rowListPanel = new List<int>();
        int funImage = 0;
        public frmSelectWorkPiece()
        {
            InitializeComponent();
        }
        private void frmSelectWorkPiece_Load(object sender, EventArgs e)
        {
            //Button_Add.Enabled = false;
            this.logicSelectLanguage();
            // Clear and Load Row
            rowListPanel.Clear();
            rowListPanel.Add(0);
            rowListPanel.Add(ROWCOUNTCOMPONENT);
            /*Create Label */
        }
        private void Janpanese()
        {
            MESSAGE_WARNING_DELETE = TextMessage.MESSAGE_WARNING_DELETE_JAPANNESE;
            ERROR1 = TextMessage.ERROR_1_JANPANESE;
            TEXT_ACTIVE_CHECKBOXMASTER = TextMessage.TEXT_ACTIVE_CHECKBOXMASTER_JAPANESE;
            WARNING_DELETE_ROW_1 = TextMessage.WARNING_DELETE_ROW_1_JAPANESE;
            WARNING_NO_COMPONENT = TextMessage.WARNING_NO_COMPONENT_JAPANESE;

            this.Text = "ツール選択ツール";

            lbl1_1.Text = "サイズ1";
            lbl2_1.Text = "サイズ2";
            lbl3_1.Text = "サイズ3";
            lbl4_1.Text = "サイズ4";
            lbl5_1.Text = "サイズ5";

            lbl6_1.Text = "カットサイズ1";
            lbl7_1.Text = "カットサイズ2";

            BUTTON_ADD_TEXT = TextMessage.BUTTON_ADD_TEXT_JAPANESE;
            BUTTON_REMOVE_TEXT = TextMessage.BUTTON_REMOVE_TEXT_JAPANESE;
            BUTTON_CALCULATE_TEXT = TextMessage.BUTTON_CALCULATE_TEXT_JAPANESE;

            TEXT_LBL_MATERIAL = TextMessage.TEXT_LBL_MATERIAL_JAPANESE;
            TEXT_LBL_SHAPE = TextMessage.TEXT_LBL_SHAPE_JAPANESE;

            TEXT_LBL_PROCESS = TextMessage.TEXT_LBL_PROCESS_JAPANESE;
            TEXT_LBL_WEIGH = TextMessage.TEXT_LBL_WEIGH_JAPANESE;

            TEXT_LBL_MATERIAL = TextMessage.TEXT_LBL_MATERIAL_JAPANESE;
            TEXT_LBL_SHAPE = TextMessage.TEXT_LBL_SHAPE_JAPANESE;
            TEXT_LBL_PROCESS = TextMessage.TEXT_LBL_PROCESS_JAPANESE;
            TEXT_LBL_WEIGH = TextMessage.TEXT_LBL_WEIGH_JAPANESE;

            TEXT_ALL_DO_NHAM = TextMessage.TEXT_ALL_DO_NHAM_JAPANESE;
            WAITING_CALCULATE_MESSAGE = TextMessage.WAITING_CALCULATE_MESSAGE_JAPANESE;

            PAM = TextMessage.PAM_JAPANESE;
            TC = TextMessage.TC_JAPANESE;
            SELECTION_TC_PAM = TextMessage.SELECTION_TC_PAM_JAPANESE;
            PAM_DIALOG = TextMessage.PAM_DIALOG_JAPANESE;
            lbl_Change_Color.Text = "色を変える";

        }
        private void VietNamese()
        {
            MESSAGE_WARNING_DELETE = TextMessage.MESSAGE_WARNING_DELETE_VIETNAMESE;
            ERROR1 = TextMessage.ERROR_1_VIETNAMESE;
            TEXT_ACTIVE_CHECKBOXMASTER = TextMessage.TEXT_ACTIVE_CHECKBOXMASTER_VIETNAMESE;
            WARNING_DELETE_ROW_1 = TextMessage.WARNING_DELETE_ROW_1_VIETNAMESE;
            WARNING_NO_COMPONENT = TextMessage.WARNING_NO_COMPONENT_VIETNAMESE;
            WAITING_CALCULATE_MESSAGE = TextMessage.WAITING_CALCULATE_MESSAGE_VIETNAMESE;
            this.Text = "TOOL CHỌN PHÔI";

            lbl1_1.Text = "Kích thước 1";
            lbl2_1.Text = "Kích thước 2";
            lbl3_1.Text = "Kích thước 3";
            lbl4_1.Text = "Kích thước 4";
            lbl5_1.Text = "Kích thước 5";

            lbl6_1.Text = "Kích thước cắt 1";
            lbl7_1.Text = "Kích thước cắt 2";

            BUTTON_ADD_TEXT = TextMessage.BUTTON_ADD_TEXT_VIETNAMESE;
            BUTTON_REMOVE_TEXT = TextMessage.BUTTON_REMOVE_TEXT_VIETNAMESE;
            BUTTON_CALCULATE_TEXT = TextMessage.BUTTON_CALCULATE_TEXT_VIETNAMESE;

            TEXT_LBL_MATERIAL = TextMessage.TEXT_LBL_MATERIAL_VIETNAMESE;
            TEXT_LBL_SHAPE = TextMessage.TEXT_LBL_SHAPE_VIETNAMESE;

            TEXT_LBL_PROCESS = TextMessage.TEXT_LBL_PROCESS_VIETNAMESE;
            TEXT_LBL_WEIGH = TextMessage.TEXT_LBL_WEIGH_VIETNAMESE;

            TEXT_ALL_DO_NHAM = TextMessage.TEXT_ALL_DO_NHAM_VIETNAMESE;

            TEXT_LBL_MATERIAL = TextMessage.TEXT_LBL_MATERIAL_VIETNAMESE;
            TEXT_LBL_SHAPE = TextMessage.TEXT_LBL_SHAPE_VIETNAMESE;
            TEXT_LBL_PROCESS = TextMessage.TEXT_LBL_PROCESS_VIETNAMESE;
            TEXT_LBL_WEIGH = TextMessage.TEXT_LBL_WEIGH_VIETNAMESE;

            PAM = TextMessage.PAM_VIETNAMESE;
            TC = TextMessage.TC_VIETNAMESE;
            SELECTION_TC_PAM = TextMessage.SELECTION_TC_PAM_VIETNAMESE;
            PAM_DIALOG = TextMessage.PAM_DIALOG_VIETNAMESE;
            lbl_Change_Color.Text = "Đổi màu nền";
        }
        private string logicSelectLanguage()
        {

            switch (CommonsVars.CURRENT_LANGUAGE)
            {
                case Constants.LOCATION_VIETNAM:
                    this.VietNamese();
                    return Constants.LOCATION_VIETNAM;

                case Constants.LOCATION_JAPAN:
                    this.Janpanese();
                    return Constants.LOCATION_JAPAN;

                case Constants.LOCATION_ENGLAND:
                    //this.ENGLAND();
                    return Constants.LOCATION_ENGLAND;

                default:
                    Console.WriteLine(Constants.LOCATION_VIETNAM);
                    return Constants.LOCATION_VIETNAM;
            }
        }
        private string getSelectedItemComboboxTxt(ComboBox cbo)
        {
            try
            {
                return cbo.SelectedItem.ToString();

            }
            catch (Exception ex)
            {
                return "";
            }

        }
        private string getSelectedTextCombobox(string inputText)
        {
            try
            {
                if (!inputText.Contains("Text")) return inputText;
                if (inputText.Equals("")) return "";
                string textSplit1 = "Text = ";
                string textSplit2 = ", Value";
                string inputText1 = inputText.Split(new[] { textSplit1 }, System.StringSplitOptions.None)[1];
                string inputText2 = inputText1.Split(new[] { textSplit2 }, System.StringSplitOptions.None)[0];
                return inputText2;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        //-----------------------------KhanhPD add new function to disable some items----------------------/

        private int getIndexFromStringSearchInDataBase(DataTable dt, string displayColumn, string stringContains, string stringNotContains = null)
        {
            bool checkContain = false;
            bool checkNotContain = false;
            List<string> method1 = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                method1.Add(row[displayColumn].ToString());
            }
            //-----------------------
            string[] method1_Array = method1.ToArray().Distinct().ToArray();
            for (int i=0;i< method1_Array.Length;i++)
            {
                checkContain = method1_Array[i].Contains(stringContains);
                if (stringNotContains == null || stringNotContains.Equals(String.Empty))
                {
                    checkNotContain = true;
                }
                else
                {
                    checkNotContain = !method1_Array[i].Contains(stringNotContains);
                }

                if(checkContain && checkNotContain)
                {
                    return i;
                }
                
            }

            return -1;
        }
        // KhanhPD add new function to disable some items 
        private void displayDataToComboBoxAndRemoveDuplicateItems(ComboBox cboBox, DataTable dt, string displayColumn, string valueColumn, string addBlankItem = "No")
        {
            cboBox.DataSource = null;
            cboBox.Items.Clear();
            List<object> items = new List<object>();
            cboBox.DisplayMember = "Text";
            cboBox.ValueMember = "Value";
            List<string> method1 = new List<string>();
            List<string> method2 = new List<string>();

            //================================
            // Add blank to combobox when addBlankItem contains Yes
            addBlankItem = addBlankItem.ToLower();
            if (addBlankItem.Contains("add_blank"))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
            }
            //===============================
            foreach (DataRow row in dt.Rows)
            {
                method1.Add(row[displayColumn].ToString());
                method2.Add(row[valueColumn].ToString());
            }
            //-----------------------
            string[] method1_Array = method1.ToArray().Distinct().ToArray();
            string[] method2_Array = method2.ToArray().Distinct().ToArray();

            //================================
            for (int k = 0; k < method1_Array.Length; k++)
            {
                if (!method1_Array[k].Equals(""))
                {
                    items.Add(new { Text = method1_Array[k], Value = method2_Array[k] });
                }
            }
            cboBox.DataSource = items;
            cboBox.SelectedIndex = -1;
        }
        private void displayDataToComboBox(ComboBox cboBox, DataTable dt, string displayColumn, string valueColumn, string addBlankItem = "No")
        {
            cboBox.DataSource = null;
            cboBox.Items.Clear();
            List<object> items = new List<object>();
            cboBox.DisplayMember = "Text";
            cboBox.ValueMember = "Value";
            //================================
            // Add blank to combobox when addBlankItem contains Yes
            addBlankItem = addBlankItem.ToLower();
            if (addBlankItem.Contains("add_blank"))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
            }
            //================================
            foreach (DataRow row in dt.Rows)
            {
                items.Add(new { Text = row[displayColumn], Value = row[valueColumn] });
            }
            cboBox.DataSource = items;
            cboBox.SelectedIndex = -1;
        }
        private void displayDataComboboxNoDataBase(ComboBox cbo, List<string> listDataAdd, string addBlankItem = "No")
        {
            cbo.DataSource = null;
            cbo.Items.Clear();
            if (addBlankItem.Contains("add_blank"))
            {
                listDataAdd.Insert(0, String.Empty);
            }
            for (int i = 0; i < listDataAdd.Count; i++)
            {
                cbo.Items.Insert(i, listDataAdd[i]);
            }
            //================================


        }
        /*************************************LOAD DATA TO COMBOBOX*****************************************/
        private void loadDataMaterial_Multil(int indexGroup)
        {
            ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            if (cboMaterial != null)
            {
                DataTable dt = manuTypeBus.getDataTable_From_RowName("MATERIAL_TO_SHAPE", "MATERIAL_NAME");
                this.displayDataToComboBoxAndRemoveDuplicateItems(cboMaterial, dt, "MATERIAL_NAME", "ID");
                cboMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
                cboMaterial.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboMaterial.AutoCompleteMode = AutoCompleteMode.Suggest;
            }

        }
        /*Finish Load Data When load Guid*/
        private void Rule_OnlyOne_CheckedCheckBox(int Column_Index)
        {
            for (int i = 1; i <= ROWCOUNTCOMPONENT; i++)
            {
                CheckBox checkbox1 = this.Controls.Find(String.Format("chk_A_{0}_{1}", Column_Index, i), true).FirstOrDefault() as CheckBox;
                CheckBox checkbox2 = this.Controls.Find(String.Format("chk_B_{0}_{1}", Column_Index, i), true).FirstOrDefault() as CheckBox;
                CheckBox checkbox3 = this.Controls.Find(String.Format("chk_C_{0}_{1}", Column_Index, i), true).FirstOrDefault() as CheckBox;
                CheckBox checkbox4 = this.Controls.Find(String.Format("chk_D_{0}_{1}", Column_Index, i), true).FirstOrDefault() as CheckBox;
                if (checkbox1 != null && checkbox2 != null && checkbox3 != null && checkbox4 != null)
                {
                    if (checkbox1.Checked == true)
                    {
                        checkbox2.Checked = false;
                        checkbox3.Checked = false;
                        checkbox4.Checked = false;
                        break;
                    }

                    if (checkbox2.Checked == true)
                    {
                        checkbox1.Checked = false;
                        checkbox3.Checked = false;
                        checkbox4.Checked = false;
                        break;
                    }

                    if (checkbox3.Checked == true)
                    {
                        checkbox2.Checked = false;
                        checkbox1.Checked = false;
                        checkbox4.Checked = false;
                        break;
                    }

                    if (checkbox4.Checked == true)
                    {
                        checkbox2.Checked = false;
                        checkbox3.Checked = false;
                        checkbox1.Checked = false;
                        break;
                    }
                }
            }
        }
        private void Rule_OnlyOne_CheckedCheckBox_Multiple(int Column_Index, int indexGroup)
        {
            for (int i = 1; i <= rowListPanel[indexGroup]; i++)
            {
                CheckBox checkbox1 = this.Controls.Find(String.Format("{0}chk_A_{1}_{2}", indexGroup, Column_Index, i), true).FirstOrDefault() as CheckBox;
                CheckBox checkbox2 = this.Controls.Find(String.Format("{0}chk_B_{1}_{2}", indexGroup, Column_Index, i), true).FirstOrDefault() as CheckBox;
                CheckBox checkbox3 = this.Controls.Find(String.Format("{0}chk_C_{1}_{2}", indexGroup, Column_Index, i), true).FirstOrDefault() as CheckBox;
                CheckBox checkbox4 = this.Controls.Find(String.Format("{0}chk_D_{1}_{2}", indexGroup, Column_Index, i), true).FirstOrDefault() as CheckBox;
                if (checkbox1 != null && checkbox2 != null && checkbox3 != null && checkbox4 != null)
                {
                    if (checkbox1.Checked == true)
                    {
                        checkbox2.Checked = false;
                        checkbox3.Checked = false;
                        checkbox4.Checked = false;
                        break;
                    }

                    if (checkbox2.Checked == true)
                    {
                        checkbox1.Checked = false;
                        checkbox3.Checked = false;
                        checkbox4.Checked = false;
                        break;
                    }

                    if (checkbox3.Checked == true)
                    {
                        checkbox2.Checked = false;
                        checkbox1.Checked = false;
                        checkbox4.Checked = false;
                        break;
                    }

                    if (checkbox4.Checked == true)
                    {
                        checkbox2.Checked = false;
                        checkbox3.Checked = false;
                        checkbox1.Checked = false;
                        break;
                    }
                }
            }


        }
        private void Funtion_All_Event_TextBox_KeyPress_Multil(object sender, KeyPressEventArgs e)
        {
            // Dertimine Textbox 
            TextBox txtBox = (TextBox)sender;
            string NametxtBox = txtBox.Name.ToString();
            string[] strArr = NametxtBox.Split(new[] { "txtBox_" }, System.StringSplitOptions.None);
            string integerDest = strArr[0];
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            int x = 0;
            Int32.TryParse(integerDest, out x);
            for (int i = 1; i <= 7; i++)
            {

                for (int j = 1; j <= rowListPanel[x]; j++)
                {
                    char ch = e.KeyChar;

                    if (ch == 46 && txtBox.Text.IndexOf('.') != -1) // 46 ASCII = (.)
                    {
                        e.Handled = true;
                        return;
                    }

                    if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
                    {
                        e.Handled = true;
                    }
                }
            }
        }
        private void Function_Event_DoNham_Multil(object sender, EventArgs e)
        {
            ComboBox cboDoNham = (ComboBox)sender;
            string nameCombobox = cboDoNham.Name;
            string ColumnIndex = splitString(nameCombobox, "_", 3);
            string indexGroup = splitString(nameCombobox, "cbo_DoNham", 0);
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            ComboBox cboGiaCong6 = this.Controls.Find(String.Format("{0}cbo_GiaCong_6_{1}", indexGroup, ColumnIndex), true).FirstOrDefault() as ComboBox;
            ComboBox cboGiaCong7 = this.Controls.Find(String.Format("{0}cbo_GiaCong_7_{1}", indexGroup, ColumnIndex), true).FirstOrDefault() as ComboBox;
            if (cboShape != null)
            {
                if( getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)).Contains("02") || 
                    getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)).Contains("32") ||
                    getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)).Contains("04") || 
                    getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)).Contains("34") ||
                    getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)).Contains("06") || 
                    getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)).Contains("36"))
                {

                    if (nameCombobox.Contains("cbo_DoNham_6") || nameCombobox.Contains("cbo_DoNham_7"))
                    {
                        if (getSelectedTextCombobox(getSelectedItemComboboxTxt(cboDoNham)).Equals("2"))
                        {

                            if (nameCombobox.Contains("6")) cboGiaCong6.Enabled = true;
                            if (nameCombobox.Contains("7")) cboGiaCong7.Enabled = true;
                            MessageBox.Show(this, "Bạn Cần Chọn Cách Thức Gia Công", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            if (nameCombobox.Contains("6"))
                            {
                                cboGiaCong6.Enabled = false;
                                cboGiaCong6.SelectedIndex = -1;
                            }
                            if (nameCombobox.Contains("7"))
                            {
                                cboGiaCong7.Enabled = false;
                                cboGiaCong7.SelectedIndex = -1;
                            }
                        }
                    }
                } 
                else
                {

                    if (nameCombobox.Contains("6"))
                    {
                        cboGiaCong6.Enabled = false;
                        cboGiaCong6.SelectedIndex = -1;
                    }
                    if (nameCombobox.Contains("7"))
                    {
                        cboGiaCong7.Enabled = false;
                        cboGiaCong7.SelectedIndex = -1;
                    }
                }
            }  

        }

        private double All_Execute_Shape_03(int indexGroup, int indexRow, TextBox textInPut1, TextBox textInPut2, TextBox textInPut_KTGC1, ComboBox cBoDoNham_1, ComboBox cBoDoNham_2,
            ComboBox cBoDoNham_KTGC1, TextBox textBoxOutPut1, TextBox textBoxOutPut2, TextBox textBoxOutPut_KTGC1)
        {
            double outputUltimate1 = -1;
            double outputUltimate2 = -1;
            double weighUltimate = -1;
            double outputUltimateKTGC1 = -1;

            // Dertimine 
            string RowStr = indexRow.ToString();
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            TextBox txt_Weigh = this.Controls.Find(String.Format("TXT_WEIGH_{0}", indexGroup), true).FirstOrDefault() as TextBox;
            ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", indexGroup), true).FirstOrDefault() as ComboBox;

            string cboMaterial_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial)), ":", 0);
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);

            //=======Function convert=======
            cboShape_StrIndex = cboShape_StrIndex.Replace("31", "01");

            string cboMaterial_Raw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial));
            double valueInput1 = convertStringToDouble(textInPut1.Text);
            double valueInput2 = convertStringToDouble(textInPut2.Text);
            double valueInput_KTGC1 = convertStringToDouble(textInPut_KTGC1.Text);
            

            double valueAjust1 = 0;
            double valueAjust2 = 0;
            double valueAjust1_KTGC1 = 0;

            /*Get Value Độ Nhám*/
            double valueDoNham1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_1)));
            double valueDoNham2 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_2)));
            double valueDoNham_KTGC1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_KTGC1)));

            // What điều này áp dụng khi độ nhám >0 thì ta sẽ làm tròn số theo KYOTO đưa ra ví dụ 1.01 = 2 ?? 0K
            if (valueDoNham1 > 0)
            {
                valueInput1 = ConvertStringToDouble_And_MathRoundRule(textInPut1.Text);
            }
            if (valueDoNham2 > 0)
            {
                valueInput2 = ConvertStringToDouble_And_MathRoundRule(textInPut2.Text);
            }
            if (valueDoNham_KTGC1 > 0)
            {
                valueInput_KTGC1 = ConvertStringToDouble_And_MathRoundRule(textInPut_KTGC1.Text);
            }

            //Must to fill độ nhám
            if (valueDoNham1 == -1 || valueDoNham2 == -1 || valueDoNham_KTGC1 == -1)
            {
                txtWarning.Text = "You can fill 0 But You can not set blank";
                txt_Weigh.Text = "ERROR";
                return -1;
            }

            //===========================================================================================================================
            if (textInPut1.Text.Equals(String.Empty) || textInPut2.Text.Equals(String.Empty) || textInPut_KTGC1.Text.Equals(String.Empty))
            {
                textBoxOutPut1.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut2.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut_KTGC1.Text = WAITING_CALCULATE_MESSAGE;
                return -1;
            }

            valueAjust1 = this.Excess_processing_Size_A_From_A_B_KTGC(cboMaterial, cboShape, valueInput1, valueInput2, valueInput_KTGC1, valueDoNham1);
            valueAjust2 = this.Excess_processing_Size_GiaCong_From_B_KTGC(cboMaterial, cboShape, valueInput2, valueInput_KTGC1, valueDoNham2);
            //===================================================================================================================
            DataTable dt_A = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1", "MATERIAL_TO_SHAPE", String.Format("{0}_{1}", cboMaterial_StrIndex, cboShape_StrIndex));
            string TARGETSTRING1 = dt_A.Rows.OfType<DataRow>().Select(k => k[2].ToString()).ToArray()[0];
            outputUltimate1 = this.TextRawForNumberTarget(TARGETSTRING1, textInPut1, textBoxOutPut1, valueAjust1, cboMaterial);

            DataTable dt_B = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1", "MATERIAL_TO_SHAPE", String.Format("{0}_{1}", cboMaterial_StrIndex, cboShape_StrIndex));
            string TARGETSTRING2 = dt_A.Rows.OfType<DataRow>().Select(k => k[2].ToString()).ToArray()[0];
            outputUltimate2 = this.TextRawForNumberTarget(TARGETSTRING2, textInPut2, textBoxOutPut2, valueAjust2, cboMaterial);

            // Lấy chung giá trị của giá trị lớn hơn
            outputUltimate1 = (outputUltimate1 >= outputUltimate2) ? outputUltimate1 : outputUltimate2;
            outputUltimate2 = outputUltimate1;
            
            // Tính KÍCH THƯỚC GIA CÔNG 1 - CALCULATE SIZE KTGC1
            /*~~~~~~~~~~~~~~~~~~~~~~~~-------------------------~~~~~~~~~~~~~~~~~~~~~~~*/
            if (cboShape_StrIndex.Equals("03") || cboShape_StrIndex.Equals("33"))
            {
                if (!cboMaterial_StrIndex.Equals("030")) // NGOẠI TRỪ A6063 CHO MÃ HÌNH DÁNG 03
                {
                    if (valueDoNham_KTGC1 == 1) valueAjust1_KTGC1 = 2; // cộng thêm 2mm KTGC, tham khảo mục số 4
                    if (valueDoNham_KTGC1 == 2) // với mã hình dáng là 03 và tất cả các vật liệu ngoại trừ nhôm （A6063）một mặt +2 và 2 mặt tra bảng lượng dư cộng vào chiều dài
                    {
                        if (valueInput1 <= 50) //~50
                        {
                            if (valueInput_KTGC1 <= 300) valueAjust1_KTGC1 = 3;
                            if (valueInput_KTGC1 > 300 && valueInput_KTGC1 <= 1400) valueAjust1_KTGC1 = 4;
                            if (valueInput_KTGC1 > 1400) valueAjust1_KTGC1 = 5;
                        }
                        if (valueInput1 > 50) // >50
                        {
                            if (valueInput_KTGC1 <= 200) valueAjust1_KTGC1 = 3;
                            if (valueInput_KTGC1 > 200 && valueInput_KTGC1 <= 1000) valueAjust1_KTGC1 = 4;
                            if (valueInput_KTGC1 > 1000) valueAjust1_KTGC1 = 5;
                        }

                    }
                }
                else
                {
                    if (valueDoNham_KTGC1 == 1) valueAjust1_KTGC1 = 2; // cộng thêm 2mm KTGC, tham khảo mục số 5
                    if (valueDoNham_KTGC1 == 2) // với mã hình dáng là 03 và tất cả các vật liệu ngoại trừ nhôm （A6063）một mặt +2 và 2 mặt tra bảng lượng dư cộng vào chiều dài
                    {
                        if (valueInput1 <= 70) //~70
                        {
                            if (valueInput_KTGC1 <= 500) valueAjust1_KTGC1 = 3;
                            if (valueInput_KTGC1 > 500 && valueInput_KTGC1 <= 1400) valueAjust1_KTGC1 = 4;
                            if (valueInput_KTGC1 > 1400) valueAjust1_KTGC1 = 5;
                        }

                    }
                }
            }
           
            // HIỂN THỊ KẾT QUẢ CHO KÍCH THƯỚC GIA CÔNG 1
            outputUltimateKTGC1 = valueInput_KTGC1 + valueAjust1_KTGC1;
            textBoxOutPut_KTGC1.Text = outputUltimateKTGC1.ToString();
            /// CALCULATE WEIGH
            DataTable dtQuery = manuTypeBus.get_Row_WHERE_To_Value_Row("MATERIAL_TO_SHAPE", "MATERIAL_NAME", "*", cboMaterial_Raw, true);
            string[] arrayResult = dtQuery.Rows.OfType<DataRow>().Select(k => k[3].ToString()).ToArray();
            double khoiluongRieng = this.convertStringToDouble(arrayResult[0]);
            if (outputUltimate1 != -1 && outputUltimate2 != -1 && outputUltimateKTGC1 != -1)
            {
                if (cboShape_StrIndex.Equals("03"))
                {
                    weighUltimate = (khoiluongRieng / 1000) * outputUltimate1 * outputUltimate2 * outputUltimateKTGC1;
                }
                
                // MathRoundingRule
                weighUltimate = this.MathRoundingRuleWeigh(weighUltimate);
                textBoxOutPut1.Text = outputUltimate1.ToString();
                textBoxOutPut2.Text = String.Empty;
                // Show result weigh
                return weighUltimate;
            }
            else
            {
                txt_Weigh.Text = "ERROR";
                return -1;
            }


        }

        private double All_Execute_A_vs_KTGC(int indexGroup, int indexRow, TextBox textInPut1, TextBox textInPut_KTGC1, ComboBox cBoDoNham_1,
            ComboBox cBoDoNham_KTGC1, ComboBox cboGiaCong6,  TextBox textBoxOutPut1, TextBox textBoxOutPut_KTGC1)
        {
            double outputUltimate1 = -1;
            double weighUltimate = -1;
            double outputUltimateKTGC1 = -1;
            // Dertimine 
            string RowStr = indexRow.ToString();
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            TextBox txt_Weigh = this.Controls.Find(String.Format("TXT_WEIGH_{0}", indexGroup), true).FirstOrDefault() as TextBox;
            ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", indexGroup), true).FirstOrDefault() as ComboBox;

            string cboMaterial_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial)), ":", 0);
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);

            //=======Function convert======
            cboShape_StrIndex = cboShape_StrIndex.Replace("32", "02").Replace("34", "04").Replace("35", "05");

            string cboMaterial_Raw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial));
            double valueInput1 = convertStringToDouble(textInPut1.Text);
            double valueInput_KTGC1 = convertStringToDouble(textInPut_KTGC1.Text);

            double valueAjust1 = 0;
            double valueAjust1_KTGC1 = 0;

            /*Get Value Độ Nhám*/
            double valueDoNham1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_1)));
            double valueDoNham_KTGC1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_KTGC1)));

            // What điều này áp dụng khi độ nhám >0 thì ta sẽ làm tròn số theo KYOTO đưa ra ví dụ 1.01 = 2 ?? 0K
            if (valueDoNham1 > 0)
            {
                valueInput1 = ConvertStringToDouble_And_MathRoundRule(textInPut1.Text);
            }

            if (valueDoNham_KTGC1 > 0)
            {
                valueInput_KTGC1 = ConvertStringToDouble_And_MathRoundRule(textInPut_KTGC1.Text);
            }

            //Must to fill độ nhám
            if (valueDoNham1 == -1 ||  valueDoNham_KTGC1 == -1)
            {
                txtWarning.Text = "You can fill 0 But You can not set blank";
                txt_Weigh.Text = "ERROR";
                return -1;
            }

            //===========================================================================================================================
            if (textInPut1.Text.Equals(String.Empty) || textInPut_KTGC1.Text.Equals(String.Empty))
            {
                textBoxOutPut1.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut_KTGC1.Text = WAITING_CALCULATE_MESSAGE;
                return -1;
            }
            // Calculate Lượng Dư cho kích thước A
            valueAjust1 = this.Excess_processing_Size_A_From_A_B_KTGC(cboMaterial, cboShape, valueInput1, 0, valueInput_KTGC1, valueDoNham1);

            //===================================================================================================================
            // Data analyzed
            DataTable dt_A = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1", "MATERIAL_TO_SHAPE", String.Format("{0}_{1}", cboMaterial_StrIndex, cboShape_StrIndex));
            string TARGETSTRING = dt_A.Rows.OfType<DataRow>().Select(k => k[2].ToString()).ToArray()[0];
            outputUltimate1 = this.TextRawForNumberTarget(TARGETSTRING, textInPut1, textBoxOutPut1, valueAjust1, cboMaterial);

            // Trường hợp mã là 02 hoặc 04 Tính lượng dư cho gia công 1
            valueAjust1_KTGC1 = this.Excess_processing_Size_GiaCong_From_B_KTGC(cboMaterial, cboShape, 0, valueAjust1_KTGC1, valueDoNham_KTGC1, cboGiaCong6);
            outputUltimateKTGC1 = valueInput_KTGC1 + valueAjust1_KTGC1;
            textBoxOutPut_KTGC1.Text = outputUltimateKTGC1.ToString();
            if (valueAjust1 == -1 || valueAjust1_KTGC1 == -1)
            {
                txt_Weigh.Text = "FILL AGAIN";
                return -1;
            }

            /// CALCULATE WEIGH
            DataTable dtQuery = manuTypeBus.get_Row_WHERE_To_Value_Row("MATERIAL_TO_SHAPE", "MATERIAL_NAME", "*", cboMaterial_Raw, true);
            string[] arrayResult = dtQuery.Rows.OfType<DataRow>().Select(k => k[3].ToString()).ToArray();
            double khoiluongRieng = this.convertStringToDouble(arrayResult[0]);
            if (outputUltimate1 != -1 && outputUltimateKTGC1 != -1 /*&& outputUltimateKTGC2 != -1*/)
            {
                if (cboShape_StrIndex.Equals("02"))
                {
                    weighUltimate = (khoiluongRieng/1000) *(PI_NUMBER/4) * outputUltimate1 * outputUltimate1 * outputUltimateKTGC1;
                }
                if (cboShape_StrIndex.Equals("04"))
                {
                    weighUltimate = (khoiluongRieng / 1000) * (PI_NUMBER / 4) * outputUltimate1 * outputUltimate1 * outputUltimateKTGC1 * 1.1;
                }
               
                // MathRoundingRule
                weighUltimate = this.MathRoundingRuleWeigh(weighUltimate);
                // Show result weigh
                return weighUltimate;
            }
            else
            {
                txt_Weigh.Text = "ERROR";
                return -1;
            }
        }

        // Create Duy Khanh
        // Return Value : Khối Lượng

        private double All_Execute_A_B_vs_KTGC(int indexGroup, int indexRow, TextBox textInPut1, TextBox textInPut2, TextBox textInPut_KTGC1, ComboBox cBoDoNham_1, ComboBox cBoDoNham_2,
            ComboBox cBoDoNham_KTGC1, ComboBox cboGiaCong6, ComboBox cboGiaCong7, TextBox textBoxOutPut1, TextBox textBoxOutPut2, TextBox textBoxOutPut_KTGC1)
        {
            double outputUltimate1 = -1;
            double outputUltimate2 = -1;
            double weighUltimate = -1;
            double outputUltimateKTGC1 = -1;

            // Dertimine 
            string RowStr = indexRow.ToString();
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            TextBox txt_Weigh = this.Controls.Find(String.Format("TXT_WEIGH_{0}", indexGroup), true).FirstOrDefault() as TextBox;
            ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", indexGroup), true).FirstOrDefault() as ComboBox;

            string cboMaterial_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial)), ":", 0);
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);

            //=======Function convert======
            cboShape_StrIndex = cboShape_StrIndex.Replace("31", "01").Replace("36", "06").Replace("37", "07");

            string cboMaterial_Raw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial));
            double valueInput1 = convertStringToDouble(textInPut1.Text);
            double valueInput2 = convertStringToDouble(textInPut2.Text);
            double valueInput_KTGC1 = convertStringToDouble(textInPut_KTGC1.Text);

            double valueAjust1 = 0;
            double valueAjust2 = 0;
            double valueAjust1_KTGC1 = 0;

            /*Get Value Độ Nhám*/
            double valueDoNham1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_1)));
            double valueDoNham2 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_2)));
            double valueDoNham_KTGC1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_KTGC1)));

            // What điều này áp dụng khi độ nhám >0 thì ta sẽ làm tròn số theo KYOTO đưa ra ví dụ 1.01 = 2 ?? 0K
            if (valueDoNham1 > 0)
            {
                valueInput1 = ConvertStringToDouble_And_MathRoundRule(textInPut1.Text);
            }

            if (valueDoNham2 > 0)
            {
                valueInput2 = ConvertStringToDouble_And_MathRoundRule(textInPut2.Text);
            }

            if (valueDoNham_KTGC1 > 0)
            {
                valueInput_KTGC1 = ConvertStringToDouble_And_MathRoundRule(textInPut_KTGC1.Text);
            }

            //Must to fill độ nhám
            if (valueDoNham1 == -1 || valueDoNham2 == -1 || valueDoNham_KTGC1 == -1)
            {
                txtWarning.Text = "You can fill 0 But You can not set blank";
                txt_Weigh.Text = "ERROR";
                return -1;
            }

            //===========================================================================================================================
            if (textInPut1.Text.Equals(String.Empty) || textInPut2.Text.Equals(String.Empty) || textInPut_KTGC1.Text.Equals(String.Empty))
            {
                textBoxOutPut1.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut2.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut_KTGC1.Text = WAITING_CALCULATE_MESSAGE;
                return -1;
            }
            valueAjust1 = this.Excess_processing_Size_A_From_A_B_KTGC(cboMaterial, cboShape, valueInput1, valueInput2, valueDoNham_KTGC1, valueDoNham1);

            // Tra Lượng dư cho kích thước B
            if (cboShape_StrIndex.Equals("01"))
            {
                if (valueDoNham2 == 1) // ĐỘ NHÁM  = 1
                {
                    valueAjust2 = 1;
                }
                //====================================
                if (valueDoNham2 == 2) // ĐỘ NHÁM  = 2
                {
                    valueAjust2 = 2;
                }
            }
            //===================================================================================================================
            // Data analyzed
            DataTable dt_A = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1_2", "MATERIAL_TO_SHAPE", String.Format("{0}_{1}", cboMaterial_StrIndex, cboShape_StrIndex));
            // Get Data From Textbox Kich Thuoc 1
            string tableKt1Ultimate = String.Empty;
            int checkPass1 = 0;
            for (int queryRun = 0; queryRun < dt_A.Rows.Count; queryRun++)
            {
                string array = dt_A.Rows.OfType<DataRow>().Select(k => k[1].ToString()).ToArray()[queryRun];
                double valueTableVL1 = convertStringToDouble(this.splitString(array, "_", 2));
                double valuePlus1 = valueInput1 + valueAjust1;
                if (valueTableVL1 >= valuePlus1)
                {
                    string textQuery = String.Format("{0}_{1}_{2}", cboMaterial_StrIndex, cboShape_StrIndex, valueTableVL1.ToString()).Replace(",", ".");
                    DataTable dt_A_1 = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1_2", "MATERIAL_TO_SHAPE", textQuery);
                    tableKt1Ultimate = dt_A_1.Rows.OfType<DataRow>().Select(k => k[2].ToString()).ToArray()[0];
                    outputUltimate1 = valueTableVL1;
                    // HIỂN THỊ KẾT QUẢ for KÍCH THƯỚC 1
                    textBoxOutPut1.Text = valueTableVL1.ToString();
                    textBoxOutPut1.BackColor = Color.LightGreen;
                    checkPass1 = 1;
                    break;
                }
            }
            //=====IF WITHOUT TABLE=====
            if (checkPass1 == 0)
            {
                textBoxOutPut1.Text = "NOT AVALABLE";
                textBoxOutPut1.BackColor = Color.Red;
                Console.WriteLine("NOT AVAIABLE");
            }
            // HIỂN THỊ KẾT QUẢ for KÍCH THƯỚC 2
            outputUltimate2 = this.TextRawForNumberTarget(tableKt1Ultimate, textInPut2, textBoxOutPut2, valueAjust2, cboMaterial);
            // Tính KÍCH THƯỚC GIA CÔNG 1 - CALCULATE SIZE KTGC1
            valueAjust1_KTGC1 = this.Excess_processing_Size_GiaCong_From_B_KTGC(cboMaterial, cboShape, valueInput2, valueInput_KTGC1, valueDoNham_KTGC1, cboGiaCong6);
            // HIỂN THỊ KẾT QUẢ CHO KÍCH THƯỚC GIA CÔNG 1
            outputUltimateKTGC1 = valueInput_KTGC1 + valueAjust1_KTGC1;
            textBoxOutPut_KTGC1.Text = outputUltimateKTGC1.ToString();
            /// CALCULATE WEIGH
            DataTable dtQuery = manuTypeBus.get_Row_WHERE_To_Value_Row("MATERIAL_TO_SHAPE", "MATERIAL_NAME", "*", cboMaterial_Raw, true);
            string[] arrayResult = dtQuery.Rows.OfType<DataRow>().Select(k => k[3].ToString()).ToArray();
            double khoiluongRieng = this.convertStringToDouble(arrayResult[0]);
            if (outputUltimate1 != -1 && outputUltimate2 != -1 && outputUltimateKTGC1 != -1)
            {
                if (cboShape_StrIndex.Equals("06"))
                {
                    weighUltimate = (khoiluongRieng/1000) * outputUltimateKTGC1 * (PI_NUMBER * outputUltimate1 - PI_NUMBER * (outputUltimate1 - 2 * outputUltimate2));
                }
                if (cboShape_StrIndex.Equals("07"))
                {
                    weighUltimate = (khoiluongRieng / 1000) * 4 * (outputUltimate1 - outputUltimate2);
                }
                if (cboShape_StrIndex.Equals("01"))
                {
                    weighUltimate = (khoiluongRieng / 1000) * outputUltimateKTGC1 * outputUltimate2 * outputUltimate1;
                }
                // MathRoundingRule
                weighUltimate = this.MathRoundingRuleWeigh(weighUltimate);
                // Show result weigh
                return weighUltimate;
            }
            else
            {
                txt_Weigh.Text = "ERROR";
                return -1;
            }

        }

        private double All_Execute_Shape_05(int indexGroup, int indexRow, TextBox textInPut1, TextBox textInPut_KTGC1, TextBox textInPut_KTGC2, ComboBox cBoDoNham_1, ComboBox cBoDoNham_KTGC1,
            ComboBox cBoDoNham_KTGC2, TextBox textBoxOutPut1, TextBox textBoxOutPut_KTGC1, TextBox textBoxOutPut_KTGC2)
        {
            double outputUltimate1 = -1;
            double weighUltimate = -1;
            double outputUltimateKTGC1 = -1;
            double outputUltimateKTGC2 = -1;

            // Dertimine 
            string RowStr = indexRow.ToString();
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            TextBox txt_Weigh = this.Controls.Find(String.Format("TXT_WEIGH_{0}", indexGroup), true).FirstOrDefault() as TextBox;
            ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", indexGroup), true).FirstOrDefault() as ComboBox;

            string cboMaterial_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial)), ":", 0);
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);
            string cboMaterial_Raw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial));

            //=======Function convert======
            cboShape_StrIndex = cboShape_StrIndex.Replace("31", "01").Replace("36", "06").Replace("37", "07");

           
            double valueInput1 = convertStringToDouble(textInPut1.Text);
            double valueInput_KTGC1 = convertStringToDouble(textInPut_KTGC1.Text);
            double valueInput_KTGC2 = convertStringToDouble(textInPut_KTGC2.Text);

            double valueAjust1 = 0;
            double valueAjust_KTGC1 = 0;
            double valueAjust_KTGC2 = 0;

            /*Get Value Độ Nhám*/
            double valueDoNham1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_1)));
            double valueDoNham_KTGC1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_KTGC1)));
            double valueDoNham_KTGC2 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_KTGC2)));

            // What điều này áp dụng khi độ nhám >0 thì ta sẽ làm tròn số theo KYOTO đưa ra ví dụ 1.01 = 2 ?? 0K
            if (valueDoNham1 > 0)
            {
                valueInput1 = ConvertStringToDouble_And_MathRoundRule(textInPut1.Text);
            }

            if (valueDoNham_KTGC1 > 0)
            {
                valueInput_KTGC1 = ConvertStringToDouble_And_MathRoundRule(textInPut_KTGC1.Text);
            }

            if (valueDoNham_KTGC2 > 0)
            {
                valueInput_KTGC2 = ConvertStringToDouble_And_MathRoundRule(textInPut_KTGC2.Text);
            }

            //===========================================================================================================================
            if (valueInput1.Equals(String.Empty) || valueInput_KTGC1.Equals(String.Empty) || valueInput_KTGC2.Equals(String.Empty))
            {
                textBoxOutPut1.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut_KTGC1.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut_KTGC2.Text = WAITING_CALCULATE_MESSAGE;
                return -1;
            }
            //===============Can fill độ nhám = 0 but You can not set blank ====================
            if (valueDoNham1 == -1 || valueDoNham_KTGC1 == -1 || valueDoNham_KTGC2 == -1)
            {
                txtWarning.Text = "You can fill 0 But You can not set blank";
                txt_Weigh.Text = "ERROR";
                return -1;
            }
            valueAjust1 = this.Excess_processing_Size_A_From_A_B_KTGC(cboMaterial, cboShape,valueInput1, valueInput_KTGC1, valueInput_KTGC2, valueDoNham1);
            if (valueAjust1 == -1)
            {
                txtWarning.Text      = "Yêu cầu nhập độ nhám";
                txtWarning.BackColor = Color.Red;
                txt_Weigh.Text = "ERROR";
                return -1;
            }
            
            //===================================================================================================================
            DataTable dt1 = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1", "MATERIAL_TO_SHAPE", String.Format("{0}_{1}", cboMaterial_StrIndex, cboShape_StrIndex));
            string TARGETSTRING1 = dt1.Rows.OfType<DataRow>().Select(k => k[2].ToString()).ToArray()[0];
            outputUltimate1 = this.TextRawForNumberTarget(TARGETSTRING1, textInPut1, textBoxOutPut1, valueAjust1, cboMaterial);

            // - CALCULATE SIZE KTGC1
            valueAjust_KTGC1 = Excess_processing_Size_GiaCong_From_B_KTGC(cboMaterial, cboShape, valueInput1, valueInput_KTGC2,valueDoNham_KTGC1);
            outputUltimateKTGC1 = valueInput_KTGC1 + valueAjust_KTGC1;
            
            // - CALCULATE SIZE KTGC2
            valueAjust_KTGC2 = Excess_processing_Size_GiaCong_From_B_KTGC(cboMaterial, cboShape, valueInput1, valueInput_KTGC2, valueDoNham_KTGC2);
            outputUltimateKTGC2 = valueInput_KTGC2 + valueAjust_KTGC2;

            // Show
            if(outputUltimateKTGC1 > outputUltimateKTGC2)
            {
                textBoxOutPut_KTGC1.Text = outputUltimateKTGC2.ToString();
                textBoxOutPut_KTGC2.Text = outputUltimateKTGC1.ToString();
            }
            else
            {
                textBoxOutPut_KTGC1.Text = outputUltimateKTGC1.ToString();
                textBoxOutPut_KTGC2.Text = outputUltimateKTGC2.ToString();
            }
            
            if (valueAjust_KTGC1 == -1 || valueAjust_KTGC2 == -1)
            {
                txtWarning.Text = "Yêu cầu nhập độ nhám";
                txt_Weigh.Text = "ERROR";
                return -1;
            }
            /// CALCULATE WEIGH

            DataTable dtQuery = manuTypeBus.get_Row_WHERE_To_Value_Row("MATERIAL_TO_SHAPE", "MATERIAL_NAME", "*", cboMaterial_Raw, true);
            string[] arrayResult = dtQuery.Rows.OfType<DataRow>().Select(k => k[3].ToString()).ToArray();
            double khoiluongRieng = this.convertStringToDouble(arrayResult[0]);
            if (outputUltimate1 != -1 && outputUltimateKTGC1 != -1 && outputUltimateKTGC2 != -1)
            {
                if (cboShape_StrIndex.Equals("05"))
                {
                    weighUltimate = (khoiluongRieng / 1000) * outputUltimate1 * outputUltimateKTGC1 * outputUltimateKTGC2;
                }
               
                // MathRoundingRule
                weighUltimate = this.MathRoundingRuleWeigh(weighUltimate);
                // Show result weigh
                return weighUltimate;
            }
            else
            {
                txt_Weigh.Text = "ERROR";
                return -1;
            }

        }

        private double All_Execute_A_B_C_vs_KTGC(int indexGroup, int indexRow, TextBox textInPut1, TextBox textInPut2, TextBox textInPut3, TextBox textInPut_KTGC1, ComboBox cBoDoNham_1,
            ComboBox cBoDoNham_2, ComboBox cBoDoNham_3, ComboBox cBoDoNham_KTGC1, ComboBox cboGiaCong6, ComboBox cboGiaCong7, TextBox textBoxOutPut1, TextBox textBoxOutPut2,
            TextBox textBoxOutPut3, TextBox textBoxOutPut_KTGC1)
        {
            double outputUltimate1 = -1;
            double outputUltimate2 = -1;
            double outputUltimate3 = -1;
            double weighUltimate = -1;
            double outputUltimateKTGC1 = -1;

            // Dertimine 
            string RowStr = indexRow.ToString();
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            TextBox txt_Weigh = this.Controls.Find(String.Format("TXT_WEIGH_{0}", indexGroup), true).FirstOrDefault() as TextBox;
            ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", indexGroup), true).FirstOrDefault() as ComboBox;

            string cboMaterial_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial)), ":", 0);
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);

            //=======Function convert======
            cboShape_StrIndex = cboShape_StrIndex.Replace("38", "08").Replace("41", "11").Replace("42", "12");

            string cboMaterial_Raw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial));
            double valueInput1 = convertStringToDouble(textInPut1.Text);
            double valueInput2 = convertStringToDouble(textInPut2.Text);
            double valueInput3 = convertStringToDouble(textInPut3.Text);
            double valueInput_KTGC1 = convertStringToDouble(textInPut_KTGC1.Text);

            double valueAjust1 = 0;
            double valueAjust2 = 0;
            double valueAjust3 = 0;
            double valueAjust1_KTGC1 = 0;

            /*Get Value Độ Nhám*/
            double valueDoNham1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_1)));
            double valueDoNham2 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_2)));
            double valueDoNham3 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_3)));
            double valueDoNham_KTGC1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_KTGC1)));

            // What điều này áp dụng khi độ nhám >0 thì ta sẽ làm tròn số theo KYOTO đưa ra ví dụ 1.01 = 2 ?? 0K
            if (valueDoNham1 > 0)
            {
                valueInput1 = ConvertStringToDouble_And_MathRoundRule(textInPut1.Text);
            }

            if (valueDoNham2 > 0)
            {
                valueInput2 = ConvertStringToDouble_And_MathRoundRule(textInPut2.Text);
            }

            if (valueDoNham3 > 0)
            {
                valueInput3 = ConvertStringToDouble_And_MathRoundRule(textInPut3.Text);
            }

            if (valueDoNham_KTGC1 > 0)
            {
                valueInput_KTGC1 = ConvertStringToDouble_And_MathRoundRule(textInPut_KTGC1.Text);
            }


            //===============Can fill độ nhám = 0 but You can not set blank ====================
            if (valueDoNham1 == -1 || valueDoNham2 == -1 || valueDoNham3 == -1 || valueDoNham_KTGC1 == -1)
            {
                txtWarning.Text = "You can fill 0 But You can not set blank";
                txt_Weigh.Text = "ERROR";
                return -1;
            }

            valueAjust1 = valueDoNham1;
            valueAjust2 = valueDoNham2;
            valueAjust3 = valueDoNham3;

            //===========================================================================================================================
            if (textInPut1.Text.Equals(String.Empty) || textInPut2.Text.Equals(String.Empty) || textInPut3.Text.Equals(String.Empty) || textInPut_KTGC1.Text.Equals(String.Empty))
            {
                textBoxOutPut1.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut2.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut3.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut_KTGC1.Text = WAITING_CALCULATE_MESSAGE;
            }
            //===================================================================================================================
            // Data analyzed
            DataTable dt1 = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1_2_3", "MATERIAL_TO_SHAPE", String.Format("{0}_{1}", cboMaterial_StrIndex, cboShape_StrIndex));
            //===================================================================================================================
            // Get Data From Textbox Kich Thuoc 1
            string TARGET_STRING = String.Empty;
            for (int queryRun1 = 0; queryRun1 < dt1.Rows.Count; queryRun1++)
            {
                string array_1 = dt1.Rows.OfType<DataRow>().Select(k => k[1].ToString()).ToArray()[queryRun1];
                double valueTable_1 = convertStringToDouble(this.splitString(array_1, "_", 2));
                double valuePlus_1 = valueInput1 + valueAjust1;
                if (valueTable_1 >= valuePlus_1)
                {
                    // OUTPUT KT1
                    outputUltimate1 = valueTable_1;
                    textBoxOutPut1.Text = valueTable_1.ToString();
                    string textQuery_1 = String.Format("{0}_{1}_{2}", cboMaterial_StrIndex, cboShape_StrIndex, valueTable_1.ToString()).Replace(",", ".");
                    DataTable dt2 = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1_2_3", "MATERIAL_TO_SHAPE", textQuery_1);
                    for (int queryRun2 = 0; queryRun2 < dt2.Rows.Count; queryRun2++)
                    {
                        string array_2 = dt2.Rows.OfType<DataRow>().Select(k => k[1].ToString()).ToArray()[queryRun2];
                        double valueTable_2 = convertStringToDouble(this.splitString(array_2, "_", 3));
                        double valuePlus_2 = valueInput2 + valueAjust2;
                        if (valueTable_2 >= valuePlus_2)
                        {
                            // OUTPUT KT2
                            outputUltimate2 = valueTable_2;
                            textBoxOutPut2.Text = valueTable_2.ToString();
                            string textQuery_2 = String.Format("{0}_{1}", textQuery_1, valueTable_2).Replace(",", ".");
                            DataTable dt3 = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1_2_3", "MATERIAL_TO_SHAPE", textQuery_2);
                            TARGET_STRING = dt3.Rows.OfType<DataRow>().Select(k => k[2].ToString()).ToArray()[0];
                            break;
                        }
                        else
                        {
                            if (queryRun2 == (dt2.Rows.Count - 1))
                            {
                                textBoxOutPut2.Text = "NA";
                                return -1;
                            }

                        }
                    }
                    // Quit when TARGETString have value
                    if (!TARGET_STRING.Equals(String.Empty))
                    {
                        break;
                    }
                }
                // Quit when TARGETString have value
                if (!TARGET_STRING.Equals(String.Empty))
                {
                    break;
                }
                else
                {
                    if (queryRun1 == (dt1.Rows.Count - 1))
                    {
                        // OUTPUT ERROR KT1
                        textBoxOutPut1.Text = "NA";
                        return -1;
                    }

                }
            }

            // HIỂN THỊ KẾT QUẢ for KÍCH THƯỚC 3
            outputUltimate3 = this.TextRawForNumberTarget(TARGET_STRING, textInPut3, textBoxOutPut3, valueAjust3, cboMaterial);

            // Tính KÍCH THƯỚC GIA CÔNG 1 - CALCULATE SIZE KTGC1
            /*Với tất cả các vật liệu có  mã hình dáng 01（ngoại trừ nhôm）,  07, 08 ,12, 13, 16.*/
            if (cboShape_StrIndex.Equals("08") || cboShape_StrIndex.Equals("38") ||
                cboShape_StrIndex.Equals("12") || cboShape_StrIndex.Equals("42") ||
                cboShape_StrIndex.Equals("13") || cboShape_StrIndex.Equals("43"))
            {
                if (!(cboMaterial_StrIndex.Equals("030") && cboShape_StrIndex.Equals("01"))) // NGOẠI TRỪ A6063 CHO MÃ HÌNH DÁNG 01
                {
                    if (valueDoNham_KTGC1 == 1) valueAjust1_KTGC1 = 2; // cộng thêm 2mm KTGC, tham khảo mục số 1

                    if (valueDoNham_KTGC1 == 2) // 2 MẶT Tra theo bảng 1, kết quả cộng KTGC
                    {
                        if (valueInput2 <= 125)
                        {
                            if (valueInput_KTGC1 <= 300) valueAjust1_KTGC1 = 3;
                            if (valueInput_KTGC1 > 300 && valueInput_KTGC1 <= 1400) valueAjust1_KTGC1 = 4;
                            if (valueInput_KTGC1 > 1400) valueAjust1_KTGC1 = 5;
                        }
                        if (valueInput2 > 125 && valueInput2 <= 200)
                        {
                            if (valueInput_KTGC1 <= 200) valueAjust1_KTGC1 = 3;
                            if (valueInput_KTGC1 > 200 && valueInput_KTGC1 <= 1000) valueAjust1_KTGC1 = 4;
                            if (valueInput_KTGC1 > 1400) valueAjust1_KTGC1 = 5;
                        }
                    }

                }
                else // A6063 CHO MÃ HÌNH DÁNG 01
                {
                    if (valueDoNham_KTGC1 == 1) valueAjust1_KTGC1 = 2; // cộng thêm 2mm KTGC, tham khảo mục số 2
                    if (valueDoNham_KTGC1 == 2) //Tra theo bảng 2, kết quả cộng KTGC
                    {
                        if (valueInput2 <= 125)
                        {
                            if (valueInput_KTGC1 <= 200) valueAjust1_KTGC1 = 3;
                            if (valueInput_KTGC1 > 200 && valueInput_KTGC1 <= 1000) valueAjust1_KTGC1 = 4;
                            if (valueInput_KTGC1 > 1400) valueAjust1_KTGC1 = 5;
                        }
                    }

                }

            }
            // HIỂN THỊ KẾT QUẢ CHO KÍCH THƯỚC GIA CÔNG 1
            outputUltimateKTGC1 = valueInput_KTGC1 + valueAjust1_KTGC1;
            textBoxOutPut_KTGC1.Text = outputUltimateKTGC1.ToString();
            /// CALCULATE WEIGH
            DataTable dtQuery = manuTypeBus.get_Row_WHERE_To_Value_Row("MATERIAL_TO_SHAPE", "MATERIAL_NAME", "*", cboMaterial_Raw, true);
            string[] arrayResult = dtQuery.Rows.OfType<DataRow>().Select(k => k[3].ToString()).ToArray();
            double khoiluongRieng = this.convertStringToDouble(arrayResult[0]);
            if (outputUltimate1 != -1 && outputUltimate2 != -1 && outputUltimate3 != -1 && outputUltimateKTGC1 != -1)
            {
                if (cboShape_StrIndex.Equals("08"))
                {
                    weighUltimate = (khoiluongRieng / 1000) * outputUltimate1 * outputUltimate2 * (outputUltimate1 - 2 * outputUltimate3) * outputUltimateKTGC1;
                }
                if (cboShape_StrIndex.Equals("12"))
                {
                    weighUltimate = (khoiluongRieng / 1000) * outputUltimateKTGC1 * (outputUltimate1 * outputUltimate2 - (outputUltimate1 - 2 * outputUltimate3) * (outputUltimate2 - outputUltimate3));
                }
                if (cboShape_StrIndex.Equals("13"))
                {
                    weighUltimate = (khoiluongRieng / 1000) * outputUltimateKTGC1 * (outputUltimate2 * outputUltimate3 - (outputUltimate2 - outputUltimate1) * (outputUltimate3 - outputUltimate1));
                }
                // MathRoundingRule
                weighUltimate = this.MathRoundingRuleWeigh(weighUltimate);
                // Show result weigh
                return weighUltimate;
            }
            else
            {
                txt_Weigh.Text = "ERROR";
                return -1;
            }
        }

        private double All_Execute_A_B_C_D_vs_KTGC(int indexGroup, int indexRow, TextBox textInPut1, TextBox textInPut2, TextBox textInPut3, TextBox textInPut4, TextBox textInPut_KTGC1,
            ComboBox cBoDoNham_1, ComboBox cBoDoNham_2, ComboBox cBoDoNham_3, ComboBox cBoDoNham_4, ComboBox cBoDoNham_KTGC1, ComboBox cboGiaCong6, ComboBox cboGiaCong7, TextBox textBoxOutPut1,
            TextBox textBoxOutPut2, TextBox textBoxOutPut3, TextBox textBoxOutPut4, TextBox textBoxOutPut_KTGC1)
        {
            double outputUltimate1 = -1;
            double outputUltimate2 = -1;
            double outputUltimate3 = -1;
            double outputUltimate4 = -1;
            double weighUltimate = -1;
            double outputUltimateKTGC1 = -1;

            // Dertimine 
            string RowStr = indexRow.ToString();
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            TextBox txt_Weigh = this.Controls.Find(String.Format("TXT_WEIGH_{0}", indexGroup), true).FirstOrDefault() as TextBox;
            ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", indexGroup), true).FirstOrDefault() as ComboBox;

            string cboMaterial_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial)), ":", 0);
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);

            //=======Function convert======
            cboShape_StrIndex = cboShape_StrIndex.Replace("46", "16");

            string cboMaterial_Raw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial));
            double valueInput1 = convertStringToDouble(textInPut1.Text);
            double valueInput2 = convertStringToDouble(textInPut2.Text);
            double valueInput3 = convertStringToDouble(textInPut3.Text);
            double valueInput4 = convertStringToDouble(textInPut4.Text);
            double valueInput_KTGC1 = convertStringToDouble(textInPut_KTGC1.Text);

            double valueAjust1 = 0;
            double valueAjust2 = 0;
            double valueAjust3 = 0;
            double valueAjust4 = 0;
            double valueAjust1_KTGC1 = 0;

            /*Get Value Độ Nhám*/
            double valueDoNham1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_1)));
            double valueDoNham2 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_2)));
            double valueDoNham3 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_3)));
            double valueDoNham4 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_4)));
            double valueDoNham_KTGC1 = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cBoDoNham_KTGC1)));


            // What điều này áp dụng khi độ nhám >0 thì ta sẽ làm tròn số theo KYOTO đưa ra ví dụ 1.01 = 2 ?? 0K
            if (valueDoNham1 > 0)
            {
                valueInput1 = ConvertStringToDouble_And_MathRoundRule(textInPut1.Text);
            }

            if (valueDoNham2 > 0)
            {
                valueInput2 = ConvertStringToDouble_And_MathRoundRule(textInPut2.Text);
            }

            if (valueDoNham3 > 0)
            {
                valueInput3 = ConvertStringToDouble_And_MathRoundRule(textInPut3.Text);
            }

            if (valueDoNham4 > 0)
            {
                valueInput4 = ConvertStringToDouble_And_MathRoundRule(textInPut4.Text);
            }

            if (valueDoNham_KTGC1 > 0)
            {
                valueInput_KTGC1 = ConvertStringToDouble_And_MathRoundRule(textInPut_KTGC1.Text);
            }

            //===============Can fill độ nhám = 0 but You can not set blank ====================
            if (valueDoNham1 == -1 || valueDoNham2 == -1 || valueDoNham3 == -1 || valueDoNham4 == -1 || valueDoNham_KTGC1 == -1)
            {
                txtWarning.Text = "You can fill 0 But You can not set blank";
                txt_Weigh.Text = "ERROR";
                return -1;
            }

            valueAjust1 = valueDoNham1;
            valueAjust2 = valueDoNham2;
            valueAjust3 = valueDoNham3;
            valueAjust3 = valueDoNham4;

            //===========================================================================================================================
            if (textInPut1.Text.Equals(String.Empty) || textInPut2.Text.Equals(String.Empty) || textInPut3.Text.Equals(String.Empty) ||
                textInPut4.Text.Equals(String.Empty) || textInPut_KTGC1.Text.Equals(String.Empty))
            {
                textBoxOutPut1.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut2.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut3.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut4.Text = WAITING_CALCULATE_MESSAGE;
                textBoxOutPut_KTGC1.Text = WAITING_CALCULATE_MESSAGE;
            }
            //===================================================================================================================
            // Data analyzed
            DataTable dt1 = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1_2_3_4", "MATERIAL_TO_SHAPE", String.Format("{0}_{1}", cboMaterial_StrIndex, cboShape_StrIndex));
            //===================================================================================================================
            // Get Data From Textbox Kich Thuoc 1
            string TARGET_STRING = String.Empty;
            for (int queryRun1 = 0; queryRun1 < dt1.Rows.Count; queryRun1++)
            {
                string array_1 = dt1.Rows.OfType<DataRow>().Select(k => k[1].ToString()).ToArray()[queryRun1];
                double valueTable_1 = convertStringToDouble(this.splitString(array_1, "_", 2));
                double valuePlus_1 = valueInput1 + valueAjust1;
                if (valueTable_1 >= valuePlus_1)
                {
                    // OUTPUT KT1
                    outputUltimate1 = valueTable_1;
                    textBoxOutPut1.Text = valueTable_1.ToString();
                    string textQuery_1 = String.Format("{0}_{1}_{2}", cboMaterial_StrIndex, cboShape_StrIndex, valueTable_1.ToString()).Replace(",", ".");
                    DataTable dt2 = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1_2_3_4", "MATERIAL_TO_SHAPE", textQuery_1);
                    for (int queryRun2 = 0; queryRun2 < dt2.Rows.Count; queryRun2++)
                    {
                        string array_2 = dt2.Rows.OfType<DataRow>().Select(k => k[1].ToString()).ToArray()[queryRun2];
                        double valueTable_2 = convertStringToDouble(this.splitString(array_2, "_", 3));
                        double valuePlus_2 = valueInput2 + valueAjust2;
                        if (valueTable_2 >= valuePlus_2)
                        {
                            // OUTPUT KT2
                            outputUltimate2 = valueTable_2;
                            textBoxOutPut2.Text = valueTable_2.ToString();
                            string textQuery_2 = String.Format("{0}_{1}", textQuery_1, valueTable_2).Replace(",", ".");
                            DataTable dt3 = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1_2_3_4", "MATERIAL_TO_SHAPE", textQuery_2);
                            for (int queryRun3 = 0; queryRun3 < dt3.Rows.Count; queryRun3++)
                            {
                                string array_3 = dt1.Rows.OfType<DataRow>().Select(k => k[1].ToString()).ToArray()[queryRun3];
                                double valueTable_3 = convertStringToDouble(this.splitString(array_3, "_", 4));
                                double valuePlus_3 = valueInput3 + valueAjust3;
                                if (valueTable_3 >= valuePlus_3)
                                {
                                    outputUltimate3 = valueTable_3;
                                    textBoxOutPut3.Text = valueTable_3.ToString();
                                    string textQuery_3 = String.Format("{0}_{1}", textQuery_2, valueTable_3).Replace(",", ".");
                                    DataTable dt4 = manuTypeBus.getDataTableWHERE_LIKE_VALUE("SIZE1_2_3_4", "MATERIAL_TO_SHAPE", textQuery_3);
                                    TARGET_STRING = dt4.Rows.OfType<DataRow>().Select(k => k[2].ToString()).ToArray()[0];
                                    break;
                                }
                                else
                                {
                                    if (queryRun3 == (dt3.Rows.Count - 1))
                                    {
                                        // OUTPUT ERROR KT3
                                        textBoxOutPut3.Text = "NA";
                                        return -1;
                                    }
                                }
                            }
                            // Quit when TARGETString have value
                            if (!TARGET_STRING.Equals(String.Empty))
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (queryRun2 == (dt2.Rows.Count - 1))
                            {
                                // OUTPUT ERROR KT2
                                textBoxOutPut2.Text = "NA";
                                return -1;
                            }
                        }
                    }
                }
                // Quit when TARGETString have value
                if (!TARGET_STRING.Equals(String.Empty))
                {
                    break;
                }
                else
                {
                    if (queryRun1 == (dt1.Rows.Count - 1))
                    {
                        // OUTPUT ERROR KT1
                        textBoxOutPut1.Text = "NA";
                        return -1;
                    }

                }
            }

            // HIỂN THỊ KẾT QUẢ for KÍCH THƯỚC 4
            outputUltimate4 = this.TextRawForNumberTarget(TARGET_STRING, textInPut4, textBoxOutPut4, valueAjust4, cboMaterial);

            // Tính KÍCH THƯỚC GIA CÔNG 1 - CALCULATE SIZE KTGC1
            /*Với tất cả các vật liệu có  mã hình dáng 01（ngoại trừ nhôm）,  07, 08 ,12, 13, 16.*/
            if (cboShape_StrIndex.Equals("08") || cboShape_StrIndex.Equals("38") ||
                cboShape_StrIndex.Equals("12") || cboShape_StrIndex.Equals("42") ||
                cboShape_StrIndex.Equals("13") || cboShape_StrIndex.Equals("43"))
            {
                if (!(cboMaterial_StrIndex.Equals("030") && cboShape_StrIndex.Equals("01"))) // NGOẠI TRỪ A6063 CHO MÃ HÌNH DÁNG 01
                {
                    if (valueDoNham_KTGC1 == 1) valueAjust1_KTGC1 = 2; // cộng thêm 2mm KTGC, tham khảo mục số 1

                    if (valueDoNham_KTGC1 == 2) // 2 MẶT Tra theo bảng 1, kết quả cộng KTGC
                    {
                        if (valueInput2 <= 125)
                        {
                            if (valueInput_KTGC1 <= 300) valueAjust1_KTGC1 = 3;
                            if (valueInput_KTGC1 > 300 && valueInput_KTGC1 <= 1400) valueAjust1_KTGC1 = 4;
                            if (valueInput_KTGC1 > 1400) valueAjust1_KTGC1 = 5;
                        }
                        if (valueInput2 > 125 && valueInput2 <= 200)
                        {
                            if (valueInput_KTGC1 <= 200) valueAjust1_KTGC1 = 3;
                            if (valueInput_KTGC1 > 200 && valueInput_KTGC1 <= 1000) valueAjust1_KTGC1 = 4;
                            if (valueInput_KTGC1 > 1400) valueAjust1_KTGC1 = 5;
                        }
                    }

                }
                else // A6063 CHO MÃ HÌNH DÁNG 01
                {
                    if (valueDoNham_KTGC1 == 1) valueAjust1_KTGC1 = 2; // cộng thêm 2mm KTGC, tham khảo mục số 2
                    if (valueDoNham_KTGC1 == 2) //Tra theo bảng 2, kết quả cộng KTGC
                    {
                        if (valueInput2 <= 125)
                        {
                            if (valueInput_KTGC1 <= 200) valueAjust1_KTGC1 = 3;
                            if (valueInput_KTGC1 > 200 && valueInput_KTGC1 <= 1000) valueAjust1_KTGC1 = 4;
                            if (valueInput_KTGC1 > 1400) valueAjust1_KTGC1 = 5;
                        }
                    }

                }

            }
            // HIỂN THỊ KẾT QUẢ CHO KÍCH THƯỚC GIA CÔNG 1
            outputUltimateKTGC1 = valueInput_KTGC1 + valueAjust1_KTGC1;
            textBoxOutPut_KTGC1.Text = outputUltimateKTGC1.ToString();
            /// CALCULATE WEIGH
            DataTable dtQuery = manuTypeBus.get_Row_WHERE_To_Value_Row("MATERIAL_TO_SHAPE", "MATERIAL_NAME", "*", cboMaterial_Raw, true);
            string[] arrayResult = dtQuery.Rows.OfType<DataRow>().Select(k => k[3].ToString()).ToArray();
            double khoiluongRieng = this.convertStringToDouble(arrayResult[0]);
            if (outputUltimate1 != -1 && outputUltimate2 != -1 && outputUltimate3 != -1 && outputUltimateKTGC1 != -1)
            {

                weighUltimate = (khoiluongRieng / 1000) * outputUltimateKTGC1 * (2 * outputUltimate4 * outputUltimate2 + (outputUltimate1 - 2 * outputUltimate4 * outputUltimate3));
                // MathRoundingRule
                weighUltimate = this.MathRoundingRuleWeigh(weighUltimate);
                // Show result weigh
                return weighUltimate;
            }
            else
            {
                txt_Weigh.Text = "ERROR";
                return -1;
            }
        }

        private double Excess_processing_Size_A_From_A_B_KTGC(ComboBox cboMaterial, ComboBox cboShape, double kichthuocA, double kichthuocB, double KTGC, double valueDoNham)
        {
            string cboMaterial_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial)), ":", 0);
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);
            string cboMaterial_Raw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial));

            if (valueDoNham == 0)
            {
                return 0;
            }

            //=======Function convert======
            cboShape_StrIndex = cboShape_StrIndex.Replace("30", "00").Replace("40", "10").Replace("50", "20").
                                      Replace("31", "01").Replace("41", "11").Replace("51", "21").
                                      Replace("32", "02").Replace("42", "12").Replace("52", "22").
                                      Replace("33", "03").Replace("43", "13").Replace("53", "23").
                                      Replace("34", "04").Replace("44", "14").Replace("54", "24").
                                      Replace("35", "05").Replace("45", "15").Replace("55", "25").
                                      Replace("36", "06").Replace("46", "16").Replace("56", "26").
                                      Replace("37", "07").Replace("47", "17").Replace("57", "27").
                                      Replace("38", "08").Replace("48", "18").Replace("58", "28").
                                      Replace("39", "09").Replace("49", "19").Replace("59", "29");
            if(kichthuocB != 0)
            {
                // Rule 1 SS400 , SS400-D , SUS304 , SUS304-CB SUS303 , S55C , C3604 , A6063 and Độ Nhám  = 1 + mã hình dáng  = 01
                if (cboMaterial_StrIndex.Equals("002") ||
                    cboMaterial_StrIndex.Equals("004") ||
                    cboMaterial_StrIndex.Equals("040") ||
                    cboMaterial_StrIndex.Equals("121") ||
                    cboMaterial_StrIndex.Equals("013") ||
                    cboMaterial_StrIndex.Equals("037") ||
                    cboMaterial_StrIndex.Equals("030"))
                {
                    if (cboShape_StrIndex.Equals("01")) // HÌNH DÁNG = 01
                    {
                        if (valueDoNham == 1) // ĐỘ NHÁM  = 1
                        {
                            if (kichthuocB <= 50)
                            {
                                if (KTGC <= 150) return 1;
                                if (KTGC > 150 && KTGC <= 500) return 2;
                                if (KTGC > 500) return 3;
                            }
                            //===================
                            if (kichthuocB > 50 && kichthuocB <= 100)
                            {
                                if (KTGC <= 150) return 1;
                                if (KTGC > 150 && KTGC <= 500) return 2;
                                if (KTGC > 500) return 3;
                            }
                            //===================
                            if (kichthuocB > 100 && kichthuocB <= 150)
                            {
                                if (KTGC > 100 && KTGC <= 500) return 2;
                                if (KTGC > 500) return 3;
                            }
                            //===================
                            if (kichthuocB > 150 && kichthuocB <= 200)
                            {
                                if (KTGC > 150 && KTGC <= 500) return 2;
                                if (KTGC > 500) return 3;
                            }
                        }
                        if (valueDoNham == 2) // ĐỘ NHÁM  = 2)
                        {
                            if (kichthuocB <= 50)
                            {
                                if (KTGC <= 200) return 2;
                                if (KTGC > 200 && KTGC <= 1000) return 3;
                                if (KTGC > 1000) return 4;
                            }
                            //===================
                            if (kichthuocB > 50 && kichthuocB <= 100)
                            {
                                if (KTGC <= 200) return 2;
                                if (KTGC > 200 && KTGC <= 1000) return 3;
                                if (KTGC > 1000) return 4;
                            }
                            //===================
                            if (kichthuocB > 100 && kichthuocB <= 150)
                            {

                                if (KTGC > 100 && KTGC <= 200) return 2;
                                if (KTGC > 200 && KTGC <= 1000) return 3;
                                if (KTGC > 1000) return 4;
                            }
                            //===================
                            if (kichthuocB > 150 && kichthuocB <= 200)
                            {
                                if (KTGC > 150 && KTGC <= 200) return 2;
                                if (KTGC > 200 && KTGC <= 1000) return 3;
                                if (KTGC > 1000) return 4;
                            }
                        }
                    }
                }
                // Rule 2 SS400 , SS400-D , SK4-D , SUS303 , SUS304,A5052
                if (cboMaterial_StrIndex.Equals("002") ||
                    cboMaterial_StrIndex.Equals("004") ||
                    cboMaterial_StrIndex.Equals("075") ||
                    cboMaterial_StrIndex.Equals("038") ||
                    cboMaterial_StrIndex.Equals("040") ||
                    cboMaterial_StrIndex.Equals("029"))
                {
                    if (cboShape_StrIndex.Equals("05")) // HÌNH DÁNG = 01
                    {
                        if (valueDoNham == 1 || valueDoNham == 2) // ĐỘ NHÁM  = 1
                        {
                            if (kichthuocB <= 50)
                            {
                                if (KTGC <= 200) return 2;
                                if (KTGC > 200 && KTGC <= 500) return 3;
                                if (KTGC > 500) return 4;
                            }

                            if (kichthuocB > 50 && kichthuocB <= 100)
                            {
                                if (KTGC <= 200) return 2;
                                if (KTGC > 200 && KTGC <= 500) return 3;
                                if (KTGC > 500) return 4;
                            }

                            if (kichthuocB > 100 && kichthuocB <= 150)
                            {
                                if (KTGC > 100 && KTGC <= 200) return 2;
                                if (KTGC > 200 && KTGC <= 500) return 3;
                                if (KTGC > 500) return 4;
                            }

                            if (kichthuocB > 150 && kichthuocB <= 200)
                            {
                                if (KTGC > 150 && KTGC <= 500) return 3;
                                if (KTGC > 500) return 4;
                            }

                            if (kichthuocB > 200 && kichthuocB <= 300)
                            {
                                if (KTGC > 200 && KTGC <= 500) return 3;
                                if (KTGC > 500) return 4;
                            }
                            if (kichthuocB > 300 && kichthuocB <= 500)
                            {
                                if (KTGC > 300 && KTGC <= 500) return 3;
                                if (KTGC > 500) return 4;
                            }
                            if (kichthuocB > 500 && kichthuocB <= 800)
                            {
                                if (KTGC > 500 && KTGC <= 1500) return 4;
                                if (KTGC > 1500) return 5;
                            }
                            if (kichthuocB > 800 && kichthuocB <= 1000)
                            {
                                if (KTGC > 500 && KTGC <= 1000) return 4;
                                if (KTGC > 1000) return 5;
                            }
                            if (kichthuocB > 1000 && kichthuocB <= 1500)
                            {
                                if (KTGC > 1000) return 5;
                            }
                        }
                    }
                }
            }
            

            if (cboShape_StrIndex.Equals("02") || cboShape_StrIndex.Equals("06"))
            {
                if (getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial)).ToLower().Contains("-d9"))
                {
                    return 1; // VỚI VẬT LIỆU CÓ TÊN CUỐI CÙNG LÀ -D9 THÌ LUÔN CỘNG LƯỢNG DƯ LÀ +1
                }
                else
                {
                    if (kichthuocA < 200) return 2;//VỚI PHÔI HÌNH TRỤ CÓ ĐƯỜNG KÍNH LÀ NHỎ HƠN 200 THÌ LƯỢNG DƯ LÀ +2mm
                    if (kichthuocA >= 200) return 3;//với đường kính từ φ200 trở lên thì lượng dư là +3mm
                }
            }
            return -1;
        }
        private double Excess_processing_Size_GiaCong_From_B_KTGC(ComboBox cboMaterial, ComboBox cboShape, double kichthuocB, double KTGC, double valueDoNham, ComboBox cboGiaCong = null)
        {
            string cboMaterial_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial)), ":", 0);
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);
            string cboMaterial_Raw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial));
            cboShape_StrIndex = cboShape_StrIndex.Replace("30", "00").Replace("40", "10").Replace("50", "20").
                                                  Replace("31", "01").Replace("41", "11").Replace("51", "21").
                                                  Replace("32", "02").Replace("42", "12").Replace("52", "22").
                                                  Replace("33", "03").Replace("43", "13").Replace("53", "23").
                                                  Replace("34", "04").Replace("44", "14").Replace("54", "24").
                                                  Replace("35", "05").Replace("45", "15").Replace("55", "25").
                                                  Replace("36", "06").Replace("46", "16").Replace("56", "26").
                                                  Replace("37", "07").Replace("47", "17").Replace("57", "27").
                                                  Replace("38", "08").Replace("48", "18").Replace("58", "28").
                                                  Replace("39", "09").Replace("49", "19").Replace("59", "29");
            // If độ nhám = 0 return 0;
            if (valueDoNham == 0)
            {
                return 0;
            }

            if(kichthuocB != 0)
            {
                /*Với tất cả các vật liệu có  mã hình dáng 01（ngoại trừ nhôm）,  07, 08 ,12, 13, 16.*/
                if (cboShape_StrIndex.Equals("01") || 
                    cboShape_StrIndex.Equals("07") || 
                    cboShape_StrIndex.Equals("08") || 
                    cboShape_StrIndex.Equals("12") || 
                    cboShape_StrIndex.Equals("13") || 
                    cboShape_StrIndex.Equals("16"))
                {
                    if (!(cboMaterial_StrIndex.Equals("030") && cboShape_StrIndex.Equals("01"))) // NGOẠI TRỪ A6063 CHO MÃ HÌNH DÁNG 01
                    {
                        if (valueDoNham == 1) return 2; // cộng thêm 2mm KTGC, tham khảo mục số 1

                        if (valueDoNham == 2) // 2 MẶT Tra theo bảng 1, kết quả cộng KTGC
                        {
                            if (kichthuocB <= 125)
                            {
                                if (KTGC <= 300) return 3;
                                if (KTGC > 300 && KTGC <= 1400) return 4;
                                if (KTGC > 1400) return 5;
                            }
                            if (kichthuocB > 125 && kichthuocB <= 200)
                            {
                                if (KTGC <= 200) return 3;
                                if (KTGC > 200 && KTGC <= 1000) return 4;
                                if (KTGC > 1400) return 5;
                            }
                        }

                    }
                    else // A6063 CHO MÃ HÌNH DÁNG 01
                    {
                        if (valueDoNham == 1) return 2; // cộng thêm 2mm KTGC, tham khảo mục số 2
                        if (valueDoNham == 2) //Tra theo bảng 2, kết quả cộng KTGC
                        {
                            if (kichthuocB <= 125)
                            {
                                if (KTGC <= 200) return 3;
                                if (KTGC > 200 && KTGC <= 1000) return 4;
                                if (KTGC > 1400) return 5;
                            }
                        }

                    }

                }

                /*~~~~~~~~~~~~~~~~~~~~~~~~-------------------------~~~~~~~~~~~~~~~~~~~~~~~*/
                if (cboShape_StrIndex.Equals("03") || cboShape_StrIndex.Equals("33"))
                {
                    if (!cboMaterial_StrIndex.Equals("030")) // NGOẠI TRỪ A6063 CHO MÃ HÌNH DÁNG 03
                    {
                        if (valueDoNham == 1) return 2; // cộng thêm 2mm KTGC, tham khảo mục số 4
                        if (valueDoNham == 2) // với mã hình dáng là 03 và tất cả các vật liệu ngoại trừ nhôm （A6063）một mặt +2 và 2 mặt tra bảng lượng dư cộng vào chiều dài
                        {
                            if (kichthuocB <= 50) //~50
                            {
                                if (KTGC <= 300) return 3;
                                if (KTGC > 300 && KTGC <= 1400) return 4;
                                if (KTGC > 1400) return 5;
                            }
                            if (kichthuocB > 50) // >50
                            {
                                if (KTGC <= 200) return 3;
                                if (KTGC > 200 && KTGC <= 1000) return 4;
                                if (KTGC > 1000) return 5;
                            }

                        }
                    }
                    else
                    {
                        if (valueDoNham == 1) return 2; // cộng thêm 2mm KTGC, tham khảo mục số 5
                        if (valueDoNham == 2) // với mã hình dáng là 03 và tất cả các vật liệu ngoại trừ nhôm （A6063）một mặt +2 và 2 mặt tra bảng lượng dư cộng vào chiều dài
                        {
                            if (kichthuocB <= 70) //~70
                            {
                                if (KTGC <= 500) return 3;
                                if (KTGC > 500 && KTGC <= 1400) return 4;
                                if (KTGC > 1400) return 5;
                            }

                        }
                    }
                }

                /*~~~~~~~~~~~~~~~~~~~~~~~~-------------------------~~~~~~~~~~~~~~~~~~~~~~~*/
                if (cboShape_StrIndex.Equals("05") || cboShape_StrIndex.Equals("35"))
                {
                    if (cboMaterial_StrIndex.Equals("002") || cboMaterial_StrIndex.Equals("013"))
                    {
                        if (valueDoNham == 1) return 3; // cộng thêm 3mm KTGC, tham khảo mục số 6
                        if (valueDoNham == 2)
                        {
                            if (kichthuocB <= 300) //~300
                            {
                                if (KTGC <= 500) return 5;
                                if (KTGC > 500) return 7;
                            }
                            if (kichthuocB > 300) //>300
                            {
                                if (KTGC > 300 && KTGC <= 500) return 6;
                                if (KTGC > 500) return 7;
                            }
                        }
                    }
                    //===========================================================================
                    if (cboMaterial_StrIndex.Equals("004") || cboMaterial_Raw.Equals("SUS") || cboMaterial_StrIndex.Equals("075"))
                    {
                        if (valueDoNham == 1) return 3; // cộng thêm 3mm KTGC, tham khảo mục số 7
                        if (valueDoNham == 2) return 5; // cộng thêm 5mm KTGC, tham khảo mục số 7

                    }
                    //==================== VẬT LIỆU NHÔM==========================//
                    if (cboMaterial_StrIndex.Equals("030"))
                    {
                        if (kichthuocB <= 30)
                        {
                            if (valueDoNham == 1)
                            {
                                return 3;
                            }
                            if (valueDoNham == 2)
                            {
                                if (KTGC <= 100)
                                {
                                    if (KTGC <= 200) return 3;
                                    if (KTGC > 200 && KTGC <= 1000) return 4;
                                    if (KTGC > 1000) return 5;
                                }
                                if (KTGC > 100 && KTGC <= 300)
                                {
                                    if (KTGC <= 1000) return 4;
                                    if (KTGC > 1000) return 5;
                                }
                                if (KTGC > 300)
                                {
                                    if (KTGC > 300) return 5;
                                }
                            }
                        }
                        //============30 <A≤90===========
                        if (kichthuocB > 30 && kichthuocB <= 90)
                        {
                            if (valueDoNham == 1) return 3;
                            if (valueDoNham == 2)
                            {
                                if (KTGC <= 100)
                                {
                                    if (KTGC <= 1000) return 4;
                                    if (KTGC > 1000) return 5;
                                }
                                if (KTGC > 100 && KTGC <= 300)
                                {
                                    if (KTGC <= 1000) return 4;
                                    if (KTGC > 1000) return 5;
                                }
                                if (KTGC > 300)
                                {
                                    if (KTGC > 300) return 5;
                                }
                            }

                        }
                        //============A >90 ===========
                        if (kichthuocB > 90)
                        {
                            if (valueDoNham == 1) return 3;
                            if (valueDoNham == 2) return 5;
                        }
                    }
                }
            }

            /*ĐỐI VỚI TẤT CẢ CÁC VẬT LIỆU CÓ MÃ HÌNH DÁNG LÀ 02, 04, 06 THÌ GIA CÔNG MỘT MẶT+2 VÀ GIA CÔNG 2 MẶT THÌ TRA BẢNG lượng dư cộng vào chiều dài*/
            if (cboShape_StrIndex.Equals("02") || 
                cboShape_StrIndex.Equals("04") || 
                cboShape_StrIndex.Equals("06"))
            {
                //Load Data cho trường công đoạn
                if (valueDoNham == 1) return 2; // cộng thêm 2mm KTGC, tham khảo mục số 2
                if (valueDoNham == 2) // GIA CÔNG 2 MẶT THÌ TRA BẢNG lượng dư cộng vào chiều dà
                {
                    string cbo_GiaCong_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboGiaCong)), ":", 0);
                    if (cbo_GiaCong_StrIndex.Equals("01"))
                    {
                        if (KTGC <= 1000) return 3;
                        if (KTGC > 1000 && KTGC <= 1400) return 4;
                        if (KTGC > 1400) return 5;
                    }
                    else if (cbo_GiaCong_StrIndex.Equals("02")) //có gia công bậc và rỗng giữa
                    {
                        if (KTGC <= 1000) return 4;
                        if (KTGC > 1000) return 5;
                    }
                    else if (cbo_GiaCong_StrIndex.Equals("03")) //có gia công bậc và rỗng giữa
                    {
                        return 5;
                    }
                    else
                    {
                        MessageBox.Show(this, "BẠN CẦN CHỌN CÁCH GIA CÔNG", "CẢNH BÁO", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return -1;
                    }
                }

            }

            return -1;

        }

        private double MathRoundingRuleWeigh(double value)
        {
            value = Math.Round(value, MidpointRounding.AwayFromZero);
            value = Math.Round(value / 10, MidpointRounding.AwayFromZero) * 10;
            return value;
        }

        private double ConvertStringToDouble_And_MathRoundRule(string stringIn)
        {
            try
            {
                if (stringIn.Equals(String.Empty))
                {
                    return -1;
                }
                if (!stringIn.Contains(".")) stringIn = stringIn + ".0";
                string intTxt1 = stringIn.Split(new[] { "." }, System.StringSplitOptions.None)[0];
                string intTxt2 = stringIn.Split(new[] { "." }, System.StringSplitOptions.None)[1];
                int x1 = 0;
                Int32.TryParse(intTxt1, out x1);
                int x2 = 0;
                Int32.TryParse(intTxt2, out x2);
                double xyz = (x2 / (Math.Pow(10, x2.ToString().Length)));
              
                if (xyz > 0) 
                {
                    return x1 + 1;
                }
                else
                {
                    return x1;
                }
            }
            catch
            {
                return -1;
            }
        }
        private double TextRawForNumberTarget(string stringAnalyze, TextBox TextBoxPointer, TextBox TextBoxOutPut, double valueAjust, ComboBox cboMaterialCheck)
        {
            int Count = stringAnalyze.Split(new[] { "||" }, System.StringSplitOptions.None).Length;
            double valueDoubleTextBox = this.convertStringToDouble(TextBoxPointer.Text) + valueAjust;
            string IndexGroup = splitString(TextBoxPointer.Name, "txtBox_", 0);
            string RowStr = splitString(TextBoxPointer.Name, "_", 2);
            string cboMaterialStringRaw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterialCheck)).Replace(" ", "");
            TextBox textBoxPAM_TC = this.Controls.Find(String.Format("{0}PAM_TIEUCHUAN_{1}", IndexGroup, RowStr), true).FirstOrDefault() as TextBox;
            int tieuChuanCount = count_SplitString(stringAnalyze, "_*");
            if (cboMaterialStringRaw.Contains("SUS") && !cboMaterialStringRaw.Contains("SUS304TKHL")) // Trường  hợp 2 với vật kiệu đặc biệt
            {
                if (tieuChuanCount == 0) return this.SearchIn__PAM(stringAnalyze, Count, valueDoubleTextBox, TextBoxOutPut, textBoxPAM_TC);
                if (tieuChuanCount > 0)
                {
                    for (int i = 0; i < Count - 1; i++)
                    {
                        if (this.splitString(stringAnalyze, "||", i).Contains("_*"))
                        {
                            double valueTableData = this.convertStringToDouble(this.splitString(this.splitString(stringAnalyze, "||", i), "_*", 0));
                            if ((valueTableData - valueDoubleTextBox) >= 0 && (valueTableData - valueDoubleTextBox) <= 15) // Khoảng cách nhỏ hơn 15
                            {
                                TextBoxOutPut.Text = valueTableData.ToString();
                                TextBoxOutPut.BackColor = Color.LightGreen;
                                textBoxPAM_TC.Text = TC;
                                textBoxPAM_TC.BackColor = Color.LightSkyBlue;
                                return valueTableData;// pass dk1 BANG TIEU CHUAN
                            }
                            else if ((valueTableData - valueDoubleTextBox) > 15) // Khoảng cách lớn hơn 15
                            {
                                for (int i1 = 0; i1 < Count - 1; i1++)
                                {
                                    if (!this.splitString(stringAnalyze, "||", i1).Contains("_*"))
                                    {
                                        valueTableData = this.convertStringToDouble(this.splitString(stringAnalyze, "||", i1));
                                        if (valueTableData >= valueDoubleTextBox && (valueTableData - valueDoubleTextBox) <= 15)
                                        {
                                            TextBoxOutPut.Text = valueTableData.ToString();
                                            TextBoxOutPut.BackColor = Color.LightGreen;
                                            textBoxPAM_TC.Text = PAM;
                                            textBoxPAM_TC.BackColor = Color.LightSalmon;
                                            return valueTableData; //pass dk2 BANG PAM
                                        }
                                        else if ((valueTableData - valueDoubleTextBox) > 15) // Khoảng cách lớn hơn 15 Quay về tiêu chuẩn
                                        {
                                            return this.SearchIn__TIEUCHUAN(stringAnalyze, Count, valueDoubleTextBox, TextBoxOutPut, textBoxPAM_TC);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                TextBoxOutPut.Text = "NA. OVER DATA INPUT";
                TextBoxOutPut.BackColor = Color.Red;
                return -1;
            }
            else if (cboMaterialStringRaw.Contains("S55C") || cboMaterialStringRaw.Contains("A2017") || cboMaterialStringRaw.Contains("SUS304TKHL")) // Trường  hợp 2 với vật kiệu đặc biệt
            {

                if (tieuChuanCount > 0)
                {
                    if (MessageBox.Show(this, Environment.NewLine + SELECTION_TC_PAM, PAM_DIALOG, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        return this.SearchIn__PAM(stringAnalyze, Count, valueDoubleTextBox, TextBoxOutPut, textBoxPAM_TC);
                    }
                    else
                    {
                        return this.SearchIn__TIEUCHUAN(stringAnalyze, Count, valueDoubleTextBox, TextBoxOutPut, textBoxPAM_TC);
                    }
                }
                else
                {
                    MessageBox.Show("KHÔNG TỒN TẠI TRONG BẢNG TIÊU CHUẨN");
                    return this.SearchIn__PAM(stringAnalyze, Count, valueDoubleTextBox, TextBoxOutPut, textBoxPAM_TC);
                }
            }
            else
            {
                double valueTargetTC = this.SearchIn__TIEUCHUAN(stringAnalyze, Count, valueDoubleTextBox, TextBoxOutPut, textBoxPAM_TC);
                double valueTargetPAM = this.SearchIn__TIEUCHUAN(stringAnalyze, Count, valueDoubleTextBox, TextBoxOutPut, textBoxPAM_TC);
                if (valueTargetTC == -1)
                {
                    if (valueTargetPAM == -1)
                    {
                        TextBoxOutPut.Text = "NA. OVER DATA INPUT";
                        TextBoxOutPut.BackColor = Color.Red;
                        return -1;
                    }
                    else
                    {
                        return valueTargetPAM;
                    }
                }
                else
                {
                    return valueTargetTC;
                }
            }
        }

        private double SearchIn__PAM(string stringAnalyze, int count_2_GachDung, double valueDoubleTextBox, TextBox TextBoxOutPut, TextBox textBoxPAM_TC)
        {
            for (int i = 0; i < count_2_GachDung - 1; i++)
            {
                if (!this.splitString(stringAnalyze, "||", i).Contains("_*"))
                {
                    double valueTableData = this.convertStringToDouble(this.splitString(stringAnalyze, "||", i));
                    if (valueTableData >= valueDoubleTextBox)
                    {
                        TextBoxOutPut.Text = valueTableData.ToString();
                        TextBoxOutPut.BackColor = Color.LightGreen;
                        textBoxPAM_TC.Text = PAM;
                        textBoxPAM_TC.BackColor = Color.LightSalmon;
                        return valueTableData; //pass dk2 BANG PAM
                    }
                }
            }
            return -1;
        }

        private double SearchIn__TIEUCHUAN(string stringAnalyze, int count_2_GachDung, double valueDoubleTextBox, TextBox TextBoxOutPut, TextBox textBoxPAM_TC)
        {
            for (int i = 0; i < count_2_GachDung - 1; i++)
            {
                if (this.splitString(stringAnalyze, "||", i).Contains("_*"))
                {
                    double valueTableData = this.convertStringToDouble(this.splitString(this.splitString(stringAnalyze, "||", i), "_*", 0));
                    if (valueTableData >= valueDoubleTextBox)
                    {
                        TextBoxOutPut.Text = valueTableData.ToString();
                        TextBoxOutPut.BackColor = Color.LightGreen;
                        textBoxPAM_TC.Text = TC;
                        textBoxPAM_TC.BackColor = Color.LightSkyBlue;
                        return valueTableData;// pass dk1 BANG TIEU CHUAN
                    }
                }
            }
            return -1;
        }

        private double convertStringToDouble(string stringIn)
        {
            try
            {
                if(stringIn.Equals(String.Empty))
                {
                    return -1;
                }
                if (!stringIn.Contains(".")) stringIn = stringIn + ".0";
                string intTxt1 = stringIn.Split(new[] { "." }, System.StringSplitOptions.None)[0];
                string intTxt2 = stringIn.Split(new[] { "." }, System.StringSplitOptions.None)[1];
                int x1 = 0;
                Int32.TryParse(intTxt1, out x1);
                int x2 = 0;
                Int32.TryParse(intTxt2, out x2);
                double xyz = (x2 / (Math.Pow(10, x2.ToString().Length)));
                double value = x1 + xyz;
                return value;

            }
            catch
            {
                return -1;
            }
        }
        //Create by Duy Khanh
        private int convertStringToInt(string stringInt)
        {
            try
            {
                int value = 0;
                Int32.TryParse(stringInt, out value);
                return value;
            }
            catch
            {
                return -1;
            }
        }
        private string splitString(string txtIn, string txtSplit, int indexGet)
        {
            try
            {
                string text1 = txtIn.Split(new[] { txtSplit }, System.StringSplitOptions.None)[indexGet];
                return text1;
            }
            catch
            {
                return String.Empty;
            }
        }
        private int count_SplitString(string txtIn, string txtSplit)
        {
            try
            {
                string[] text1 = txtIn.Split(new[] { txtSplit }, System.StringSplitOptions.None);
                return text1.Length;
            }
            catch
            {
                return 0;
            }
        }

        private void Function_All_Event_CheckBox(object sender, EventArgs e)
        {
            this.Rule_OnlyOne_CheckedCheckBox(1);
            this.Rule_OnlyOne_CheckedCheckBox(2);
            this.Rule_OnlyOne_CheckedCheckBox(3);
            this.Rule_OnlyOne_CheckedCheckBox(4);
            this.Rule_OnlyOne_CheckedCheckBox(5);
            this.Rule_OnlyOne_CheckedCheckBox(6);
            this.Rule_OnlyOne_CheckedCheckBox(7);
        }
        private void Function_All_Event_CheckBox_Multiple(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            string NameButton = checkbox.Name.ToString();
            string[] strArr = NameButton.Split(new[] { "chk_" }, System.StringSplitOptions.None);
            string integerDest = strArr[0];
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            int x = 0;
            Int32.TryParse(integerDest, out x);
            this.Rule_OnlyOne_CheckedCheckBox_Multiple(1, x);
            this.Rule_OnlyOne_CheckedCheckBox_Multiple(2, x);
            this.Rule_OnlyOne_CheckedCheckBox_Multiple(3, x);
            this.Rule_OnlyOne_CheckedCheckBox_Multiple(4, x);
            this.Rule_OnlyOne_CheckedCheckBox_Multiple(5, x);
            this.Rule_OnlyOne_CheckedCheckBox_Multiple(6, x);
            this.Rule_OnlyOne_CheckedCheckBox_Multiple(7, x);

        }
        private void RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(CheckBox checkboxMaster, int Column_Index, int indexGroup)
        {
            try
            {
                for (int i1 = 1; i1 <= rowListPanel[indexGroup]; i1++)
                {
                    ComboBox cbo = this.Controls.Find(String.Format("{0}cbo_DoNham_{1}_{2}", indexGroup, Column_Index, i1), true).FirstOrDefault() as ComboBox;
                    if (cbo != null)
                    {
                        cbo.Enabled = checkboxMaster.Checked;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(this,WARNING_NO_COMPONENT, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        private void Rule_ComboBoxSlave_All(int indexGroup)
        {
            CheckBox chkColum1 = this.Controls.Find(String.Format("{0}_chkMaster_1", indexGroup), true).FirstOrDefault() as CheckBox;
            CheckBox chkColum2 = this.Controls.Find(String.Format("{0}_chkMaster_2", indexGroup), true).FirstOrDefault() as CheckBox;
            CheckBox chkColum3 = this.Controls.Find(String.Format("{0}_chkMaster_3", indexGroup), true).FirstOrDefault() as CheckBox;
            CheckBox chkColum4 = this.Controls.Find(String.Format("{0}_chkMaster_4", indexGroup), true).FirstOrDefault() as CheckBox;
            CheckBox chkColum5 = this.Controls.Find(String.Format("{0}_chkMaster_5", indexGroup), true).FirstOrDefault() as CheckBox;
            CheckBox chkColum6 = this.Controls.Find(String.Format("{0}_chkMaster_6", indexGroup), true).FirstOrDefault() as CheckBox;
            CheckBox chkColum7 = this.Controls.Find(String.Format("{0}_chkMaster_7", indexGroup), true).FirstOrDefault() as CheckBox;

            RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(chkColum1, 1, indexGroup);
            RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(chkColum2, 2, indexGroup);
            RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(chkColum3, 3, indexGroup);
            RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(chkColum4, 4, indexGroup);
            RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(chkColum5, 5, indexGroup);
            RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(chkColum6, 6, indexGroup);
            RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(chkColum7, 7, indexGroup);
        }
        private void RuleEnable_TextBoxSlave_Follow_CheckboxMaster_Multil(CheckBox checkboxMaster, int Column_Index, int indexGroup)
        {
            try
            {
                for (int i = 1; i <= rowListPanel[indexGroup]; i++)
                {
                    TextBox textBox = this.Controls.Find(String.Format("{0}txtBox_{1}_{2}", indexGroup, Column_Index, i), true).FirstOrDefault() as TextBox;
                    if (textBox != null)
                    {
                        textBox.ReadOnly = !checkboxMaster.Checked;
                        textBox.BackColor = checkboxMaster.Checked ? Color.LightGreen : SystemColors.Control;
                    }
                }


            }
            catch (Exception ex)
            {
                //MessageBox.Show(this, WARNING_NO_COMPONENT, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }
        private void createNewRow_Input_Multil(int INDEX_GROUP)
        {
            try
            {
                Panel panel_Determine = this.Controls.Find(String.Format("INPUT_PANEL_{0}", INDEX_GROUP), true).FirstOrDefault() as Panel;
                if (panel_Determine != null)
                {
                    // Disable ScrollBar
                    panel_Determine.AutoScroll = false;
                    //----------------------------------
                    if (rowListPanel.Count <= INDEX_GROUP)
                    {
                        rowListPanel.Add(0);
                    }
                    // Increase Row
                    rowListPanel[INDEX_GROUP]++;
                    int ROW_COUNT = rowListPanel[INDEX_GROUP];
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    int Y_LOCATION_CBO_DONHAM = 27 + (ROW_COUNT - 1) * 50;
                    int Y_LOCATION_TEXTBOX = 50 + (ROW_COUNT - 1) * 50;
                    Console.WriteLine(Y_LOCATION_TEXTBOX + "_" + ROW_COUNT.ToString());

                    int TEXTBOX_HEIGHT = 20;
                    int TEXTBOX_WIDTH = 163;

                    //===========================================================
                    Label labelText = new Label();
                    labelText.Name = String.Format("{0}lableIndex_{1}", INDEX_GROUP, ROW_COUNT);
                    labelText.Text = ROW_COUNT.ToString();
                    labelText.BackColor = Color.FromArgb(192, 255, 192);
                    labelText.Font = new Font(labelText.Font, FontStyle.Bold);
                    labelText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    labelText.Location = new Point(10, Y_LOCATION_TEXTBOX);
                    labelText.Width = 32;
                    labelText.Height = 20;
                    panel_Determine.Controls.Add(labelText);
                    //============================================================

                    ComboBox cbo_DoNham_1 = new ComboBox();
                    ComboBox cbo_DoNham_2 = new ComboBox();
                    ComboBox cbo_DoNham_3 = new ComboBox();
                    ComboBox cbo_DoNham_4 = new ComboBox();
                    ComboBox cbo_DoNham_5 = new ComboBox();
                    ComboBox cbo_DoNham_6 = new ComboBox();
                    ComboBox cbo_DoNham_7 = new ComboBox();
                    ComboBox cbo_GiaCong_6 = new ComboBox();
                    ComboBox cbo_GiaCong_7 = new ComboBox();

                    cbo_DoNham_1.AutoSize = false;
                    cbo_DoNham_2.AutoSize = false;
                    cbo_DoNham_3.AutoSize = false;
                    cbo_DoNham_4.AutoSize = false;
                    cbo_DoNham_5.AutoSize = false;
                    cbo_DoNham_6.AutoSize = false;
                    cbo_DoNham_7.AutoSize = false;
                    cbo_GiaCong_6.AutoSize = false;
                    cbo_GiaCong_7.AutoSize = false;

                    cbo_DoNham_1.Name = String.Format("{0}cbo_DoNham_1_{1}", INDEX_GROUP, ROW_COUNT);
                    cbo_DoNham_2.Name = String.Format("{0}cbo_DoNham_2_{1}", INDEX_GROUP, ROW_COUNT);
                    cbo_DoNham_3.Name = String.Format("{0}cbo_DoNham_3_{1}", INDEX_GROUP, ROW_COUNT);
                    cbo_DoNham_4.Name = String.Format("{0}cbo_DoNham_4_{1}", INDEX_GROUP, ROW_COUNT);
                    cbo_DoNham_5.Name = String.Format("{0}cbo_DoNham_5_{1}", INDEX_GROUP, ROW_COUNT);
                    cbo_DoNham_6.Name = String.Format("{0}cbo_DoNham_6_{1}", INDEX_GROUP, ROW_COUNT);
                    cbo_DoNham_7.Name = String.Format("{0}cbo_DoNham_7_{1}", INDEX_GROUP, ROW_COUNT);

                    cbo_GiaCong_6.Name = String.Format("{0}cbo_GiaCong_6_{1}", INDEX_GROUP, ROW_COUNT);
                    cbo_GiaCong_7.Name = String.Format("{0}cbo_GiaCong_7_{1}", INDEX_GROUP, ROW_COUNT);

                    cbo_DoNham_1.Location = new Point(78, Y_LOCATION_CBO_DONHAM);
                    cbo_DoNham_2.Location = new Point(252, Y_LOCATION_CBO_DONHAM);
                    cbo_DoNham_3.Location = new Point(436, Y_LOCATION_CBO_DONHAM);
                    cbo_DoNham_4.Location = new Point(616, Y_LOCATION_CBO_DONHAM);
                    cbo_DoNham_5.Location = new Point(818, Y_LOCATION_CBO_DONHAM);
                    cbo_DoNham_6.Location = new Point(987, Y_LOCATION_CBO_DONHAM);
                    cbo_DoNham_7.Location = new Point(1170, Y_LOCATION_CBO_DONHAM);

                    cbo_GiaCong_6.Location = new Point(987 + 37, Y_LOCATION_CBO_DONHAM);
                    cbo_GiaCong_7.Location = new Point(1170 + 37, Y_LOCATION_CBO_DONHAM);

                    cbo_DoNham_1.Width = CBO_DONHAM_WIDTH;
                    cbo_DoNham_2.Width = CBO_DONHAM_WIDTH;
                    cbo_DoNham_3.Width = CBO_DONHAM_WIDTH;
                    cbo_DoNham_4.Width = CBO_DONHAM_WIDTH;
                    cbo_DoNham_5.Width = CBO_DONHAM_WIDTH;
                    cbo_DoNham_6.Width = CBO_DONHAM_WIDTH / 2 + 2;
                    cbo_DoNham_7.Width = CBO_DONHAM_WIDTH / 2 + 2;

                    cbo_GiaCong_6.Width = (CBO_DONHAM_WIDTH * 39) / 20;
                    cbo_GiaCong_7.Width = (CBO_DONHAM_WIDTH * 39) / 20;

                    cbo_DoNham_1.Height = CBO_DONHAM_HEIGHT;
                    cbo_DoNham_2.Height = CBO_DONHAM_HEIGHT;
                    cbo_DoNham_3.Height = CBO_DONHAM_HEIGHT;
                    cbo_DoNham_4.Height = CBO_DONHAM_HEIGHT;
                    cbo_DoNham_5.Height = CBO_DONHAM_HEIGHT;
                    cbo_DoNham_6.Height = CBO_DONHAM_HEIGHT;
                    cbo_DoNham_7.Height = CBO_DONHAM_HEIGHT;

                    cbo_GiaCong_6.Height = CBO_DONHAM_HEIGHT;
                    cbo_GiaCong_7.Height = CBO_DONHAM_HEIGHT;


                    cbo_DoNham_1.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbo_DoNham_2.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbo_DoNham_3.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbo_DoNham_4.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbo_DoNham_5.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbo_DoNham_6.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbo_DoNham_7.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbo_GiaCong_6.DropDownStyle = ComboBoxStyle.DropDownList;
                    cbo_GiaCong_7.DropDownStyle = ComboBoxStyle.DropDownList;

                    cbo_DoNham_1.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbo_DoNham_2.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbo_DoNham_3.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbo_DoNham_4.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbo_DoNham_5.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbo_DoNham_6.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbo_DoNham_7.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbo_GiaCong_6.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cbo_GiaCong_7.AutoCompleteSource = AutoCompleteSource.ListItems;

                    cbo_DoNham_1.AutoCompleteMode = AutoCompleteMode.Suggest;
                    cbo_DoNham_2.AutoCompleteMode = AutoCompleteMode.Suggest;
                    cbo_DoNham_3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    cbo_DoNham_4.AutoCompleteMode = AutoCompleteMode.Suggest;
                    cbo_DoNham_5.AutoCompleteMode = AutoCompleteMode.Suggest;
                    cbo_DoNham_6.AutoCompleteMode = AutoCompleteMode.Suggest;
                    cbo_DoNham_7.AutoCompleteMode = AutoCompleteMode.Suggest;
                    cbo_GiaCong_6.AutoCompleteMode = AutoCompleteMode.Suggest;
                    cbo_GiaCong_7.AutoCompleteMode = AutoCompleteMode.Suggest;

                    TextBox textBox1 = new TextBox();
                    TextBox textBox2 = new TextBox();
                    TextBox textBox3 = new TextBox();
                    TextBox textBox4 = new TextBox();
                    TextBox textBox5 = new TextBox();
                    TextBox textBox6 = new TextBox();
                    TextBox textBox7 = new TextBox();

                    TextBox PAM_TIEUCHUAN = new TextBox();
                    PAM_TIEUCHUAN.ReadOnly = true;

                    textBox1.Name = String.Format("{0}txtBox_1_{1}", INDEX_GROUP, ROW_COUNT);
                    textBox2.Name = String.Format("{0}txtBox_2_{1}", INDEX_GROUP, ROW_COUNT);
                    textBox3.Name = String.Format("{0}txtBox_3_{1}", INDEX_GROUP, ROW_COUNT);
                    textBox4.Name = String.Format("{0}txtBox_4_{1}", INDEX_GROUP, ROW_COUNT);
                    textBox5.Name = String.Format("{0}txtBox_5_{1}", INDEX_GROUP, ROW_COUNT);
                    textBox6.Name = String.Format("{0}txtBox_6_{1}", INDEX_GROUP, ROW_COUNT);
                    textBox7.Name = String.Format("{0}txtBox_7_{1}", INDEX_GROUP, ROW_COUNT);

                    PAM_TIEUCHUAN.Name = String.Format("{0}PAM_TIEUCHUAN_{1}", INDEX_GROUP, ROW_COUNT);


                    textBox1.Width = TEXTBOX_WIDTH;
                    textBox2.Width = TEXTBOX_WIDTH;
                    textBox3.Width = TEXTBOX_WIDTH;
                    textBox4.Width = TEXTBOX_WIDTH;
                    textBox5.Width = TEXTBOX_WIDTH;
                    textBox6.Width = TEXTBOX_WIDTH;
                    textBox7.Width = TEXTBOX_WIDTH;
                    PAM_TIEUCHUAN.Width = (TEXTBOX_WIDTH * 10) / 32;

                    textBox1.MaxLength = 10;
                    textBox2.MaxLength = 10;
                    textBox3.MaxLength = 10;
                    textBox4.MaxLength = 10;
                    textBox5.MaxLength = 10;
                    textBox6.MaxLength = 10;
                    textBox7.MaxLength = 10;
                    PAM_TIEUCHUAN.MaxLength = 10;

                    textBox1.Height = TEXTBOX_HEIGHT;
                    textBox2.Height = TEXTBOX_HEIGHT;
                    textBox3.Height = TEXTBOX_HEIGHT;
                    textBox4.Height = TEXTBOX_HEIGHT;
                    textBox5.Height = TEXTBOX_HEIGHT;
                    textBox6.Height = TEXTBOX_HEIGHT;
                    textBox7.Height = TEXTBOX_HEIGHT;
                    PAM_TIEUCHUAN.Height = TEXTBOX_HEIGHT;

                    textBox1.Location = new Point(44, Y_LOCATION_TEXTBOX);
                    textBox2.Location = new Point(228, Y_LOCATION_TEXTBOX);
                    textBox3.Location = new Point(413, Y_LOCATION_TEXTBOX);
                    textBox4.Location = new Point(601, Y_LOCATION_TEXTBOX);
                    textBox5.Location = new Point(790, Y_LOCATION_TEXTBOX);
                    textBox6.Location = new Point(978, Y_LOCATION_TEXTBOX);
                    textBox7.Location = new Point(1161, Y_LOCATION_TEXTBOX);

                    PAM_TIEUCHUAN.Location = new Point(1330, Y_LOCATION_TEXTBOX);

                    cbo_DoNham_1.SelectionChangeCommitted += new System.EventHandler(this.Function_Event_DoNham_Multil);
                    cbo_DoNham_2.SelectionChangeCommitted += new System.EventHandler(this.Function_Event_DoNham_Multil);
                    cbo_DoNham_3.SelectionChangeCommitted += new System.EventHandler(this.Function_Event_DoNham_Multil);
                    cbo_DoNham_4.SelectionChangeCommitted += new System.EventHandler(this.Function_Event_DoNham_Multil);
                    cbo_DoNham_5.SelectionChangeCommitted += new System.EventHandler(this.Function_Event_DoNham_Multil);
                    cbo_DoNham_6.SelectionChangeCommitted += new System.EventHandler(this.Function_Event_DoNham_Multil);
                    cbo_DoNham_7.SelectionChangeCommitted += new System.EventHandler(this.Function_Event_DoNham_Multil);

                    cbo_GiaCong_6.SelectionChangeCommitted += new System.EventHandler(this.Function_Event_DoNham_Multil);
                    cbo_GiaCong_7.SelectionChangeCommitted += new System.EventHandler(this.Function_Event_DoNham_Multil);

                    textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);

                    textBox1.Leave += new System.EventHandler(Funtion_All_Event_TextBox_txtBox_Leave_Multil); // Đổi vật liệu


                    //this.txtBox_1_1.Leave += new System.EventHandler(this.txtBox_1_1_Leave);

                    List<string> listProcess = new List<string>() { "01:Chỉ GC 2 mặt", "02:GC bậc và rỗng giữa", "03:GC ngoài" };
                    this.displayDataComboboxNoDataBase(cbo_GiaCong_6, listProcess, "add_blank");
                    this.displayDataComboboxNoDataBase(cbo_GiaCong_7, listProcess, "add_blank");
                    cbo_GiaCong_6.Enabled = false;
                    cbo_GiaCong_7.Enabled = false;

                    panel_Determine.Controls.Add(cbo_DoNham_1);
                    panel_Determine.Controls.Add(cbo_DoNham_2);
                    panel_Determine.Controls.Add(cbo_DoNham_3);
                    panel_Determine.Controls.Add(cbo_DoNham_4);
                    panel_Determine.Controls.Add(cbo_DoNham_5);
                    panel_Determine.Controls.Add(cbo_DoNham_6);
                    panel_Determine.Controls.Add(cbo_DoNham_7);
                    panel_Determine.Controls.Add(cbo_GiaCong_6);
                    panel_Determine.Controls.Add(cbo_GiaCong_7);

                    panel_Determine.Controls.Add(textBox1);
                    panel_Determine.Controls.Add(textBox2);
                    panel_Determine.Controls.Add(textBox3);
                    panel_Determine.Controls.Add(textBox4);
                    panel_Determine.Controls.Add(textBox5);
                    panel_Determine.Controls.Add(textBox6);
                    panel_Determine.Controls.Add(textBox7);

                    panel_Determine.Controls.Add(PAM_TIEUCHUAN);


                    //Rule for checkbox Enable and Disable
                    CheckBox checkbox1 = this.Controls.Find(String.Format("{0}_chkMaster_1", INDEX_GROUP), true).FirstOrDefault() as CheckBox;
                    if (checkbox1 != null)
                    {

                        this.RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(checkbox1, 1, INDEX_GROUP);
                        this.RuleEnable_TextBoxSlave_Follow_CheckboxMaster_Multil(checkbox1, 1, INDEX_GROUP);

                    }

                    CheckBox checkbox2 = this.Controls.Find(String.Format("{0}_chkMaster_2", INDEX_GROUP), true).FirstOrDefault() as CheckBox;
                    if (checkbox2 != null)
                    {

                        this.RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(checkbox2, 2, INDEX_GROUP);
                        this.RuleEnable_TextBoxSlave_Follow_CheckboxMaster_Multil(checkbox2, 2, INDEX_GROUP);


                    }

                    CheckBox checkbox3 = this.Controls.Find(String.Format("{0}_chkMaster_3", INDEX_GROUP), true).FirstOrDefault() as CheckBox;
                    if (checkbox3 != null)
                    {

                        this.RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(checkbox3, 3, INDEX_GROUP);
                        this.RuleEnable_TextBoxSlave_Follow_CheckboxMaster_Multil(checkbox3, 3, INDEX_GROUP);


                    }

                    CheckBox checkbox4 = this.Controls.Find(String.Format("{0}_chkMaster_4", INDEX_GROUP), true).FirstOrDefault() as CheckBox;
                    if (checkbox4 != null)
                    {

                        this.RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(checkbox4, 4, INDEX_GROUP);
                        this.RuleEnable_TextBoxSlave_Follow_CheckboxMaster_Multil(checkbox4, 4, INDEX_GROUP);


                    }

                    CheckBox checkbox5 = this.Controls.Find(String.Format("{0}_chkMaster_5", INDEX_GROUP), true).FirstOrDefault() as CheckBox;
                    if (checkbox5 != null)
                    {

                        this.RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(checkbox5, 5, INDEX_GROUP);
                        this.RuleEnable_TextBoxSlave_Follow_CheckboxMaster_Multil(checkbox5, 5, INDEX_GROUP);


                    }

                    CheckBox checkbox6 = this.Controls.Find(String.Format("{0}_chkMaster_6", INDEX_GROUP), true).FirstOrDefault() as CheckBox;
                    if (checkbox6 != null)
                    {

                        this.RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(checkbox6, 6, INDEX_GROUP);
                        this.RuleEnable_TextBoxSlave_Follow_CheckboxMaster_Multil(checkbox6, 6, INDEX_GROUP);


                    }

                    CheckBox checkbox7 = this.Controls.Find(String.Format("{0}_chkMaster_7", INDEX_GROUP), true).FirstOrDefault() as CheckBox;
                    if (checkbox7 != null)
                    {

                        this.RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(checkbox7, 7, INDEX_GROUP);
                        this.RuleEnable_TextBoxSlave_Follow_CheckboxMaster_Multil(checkbox7, 7, INDEX_GROUP);
                    }

                    panel_Determine.AutoScroll = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        
        private void createNewRow_Output_Multil(int INDEX_GROUP)
        {

            try
            {
                Panel panel_Determine = this.Controls.Find(String.Format("OUTPUT_PANEL_{0}", INDEX_GROUP), true).FirstOrDefault() as Panel;
                panel_Determine.AutoScroll = false;
                if (panel_Determine != null)
                {
                    int INDEX_ROW_OUTPUT = rowListPanel[INDEX_GROUP];
                    //-----------------------------------------------------------
                    int Y_LOCATION_TEXTBOX_OUT = 20 + (INDEX_ROW_OUTPUT - 1) * 35;

                    int TEXTBOX_HEIGHT = 20;
                    int TEXTBOX_WIDTH = 163;
                    //-----------------------------------------------------------

                    Label labelText = new Label();
                    labelText.Name = String.Format("{0}lableIndex_Output_{1}", INDEX_GROUP, INDEX_ROW_OUTPUT);
                    labelText.Text = INDEX_ROW_OUTPUT.ToString();
                    labelText.BackColor = Color.FromArgb(192, 255, 192);
                    labelText.Font = new Font(labelText.Font, FontStyle.Bold);
                    labelText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    labelText.Location = new Point(10, 20 + (INDEX_ROW_OUTPUT - 1) * 35);
                    labelText.Width = 32;
                    labelText.Height = 20;
                    panel_Determine.Controls.Add(labelText);
                    //============================================================
                    TextBox textBox1 = new TextBox();
                    TextBox textBox2 = new TextBox();
                    TextBox textBox3 = new TextBox();
                    TextBox textBox4 = new TextBox();
                    TextBox textBox5 = new TextBox();
                    TextBox textBox6 = new TextBox();
                    TextBox textBox7 = new TextBox();

                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    textBox3.ReadOnly = true;
                    textBox4.ReadOnly = true;
                    textBox5.ReadOnly = true;
                    textBox6.ReadOnly = true;
                    textBox7.ReadOnly = true;

                    textBox1.Name = String.Format("{0}txtBox_Output_1_{1}", INDEX_GROUP, INDEX_ROW_OUTPUT);
                    textBox2.Name = String.Format("{0}txtBox_Output_2_{1}", INDEX_GROUP, INDEX_ROW_OUTPUT);
                    textBox3.Name = String.Format("{0}txtBox_Output_3_{1}", INDEX_GROUP, INDEX_ROW_OUTPUT);
                    textBox4.Name = String.Format("{0}txtBox_Output_4_{1}", INDEX_GROUP, INDEX_ROW_OUTPUT);
                    textBox5.Name = String.Format("{0}txtBox_Output_5_{1}", INDEX_GROUP, INDEX_ROW_OUTPUT);
                    textBox6.Name = String.Format("{0}txtBox_Output_6_{1}", INDEX_GROUP, INDEX_ROW_OUTPUT);
                    textBox7.Name = String.Format("{0}txtBox_Output_7_{1}", INDEX_GROUP, INDEX_ROW_OUTPUT);


                    textBox1.Width = TEXTBOX_WIDTH;
                    textBox2.Width = TEXTBOX_WIDTH;
                    textBox3.Width = TEXTBOX_WIDTH;
                    textBox4.Width = TEXTBOX_WIDTH;
                    textBox5.Width = TEXTBOX_WIDTH;
                    textBox6.Width = TEXTBOX_WIDTH;
                    textBox7.Width = TEXTBOX_WIDTH;

                    textBox1.Height = TEXTBOX_HEIGHT;
                    textBox2.Height = TEXTBOX_HEIGHT;
                    textBox3.Height = TEXTBOX_HEIGHT;
                    textBox4.Height = TEXTBOX_HEIGHT;
                    textBox5.Height = TEXTBOX_HEIGHT;
                    textBox6.Height = TEXTBOX_HEIGHT;
                    textBox7.Height = TEXTBOX_HEIGHT;

                    textBox1.Location = new Point(44, Y_LOCATION_TEXTBOX_OUT);
                    textBox2.Location = new Point(228, Y_LOCATION_TEXTBOX_OUT);
                    textBox3.Location = new Point(413, Y_LOCATION_TEXTBOX_OUT);
                    textBox4.Location = new Point(601, Y_LOCATION_TEXTBOX_OUT);
                    textBox5.Location = new Point(790, Y_LOCATION_TEXTBOX_OUT);
                    textBox6.Location = new Point(978, Y_LOCATION_TEXTBOX_OUT);
                    textBox7.Location = new Point(1161, Y_LOCATION_TEXTBOX_OUT);

                    textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);
                    textBox7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(Funtion_All_Event_TextBox_KeyPress_Multil);

                    panel_Determine.Controls.Add(textBox1);
                    panel_Determine.Controls.Add(textBox2);
                    panel_Determine.Controls.Add(textBox3);
                    panel_Determine.Controls.Add(textBox4);
                    panel_Determine.Controls.Add(textBox5);
                    panel_Determine.Controls.Add(textBox6);
                    panel_Determine.Controls.Add(textBox7);
                    //Rule for checkbox Enable and Disable
                    Thread.Sleep(100);
                    panel_Determine.AutoScroll = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ERROR1, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void deleteLastRow_Multil(int INDEX_GROUP, bool ASK_BEFORE_DELETE = true)
        {
            try
            {
                int ROWCOUNTCOMPONENT = rowListPanel[INDEX_GROUP];
                Panel panel_Determine_Input = this.Controls.Find(String.Format("INPUT_PANEL_{0}", INDEX_GROUP), true).FirstOrDefault() as Panel;
                Panel panel_Determine_Output = this.Controls.Find(String.Format("OUTPUT_PANEL_{0}", INDEX_GROUP), true).FirstOrDefault() as Panel;
                if (ROWCOUNTCOMPONENT > 1)
                {
                    if (ASK_BEFORE_DELETE == true)
                    {
                        if (MessageBox.Show(this, String.Format(MESSAGE_WARNING_DELETE + " {0} ?", ROWCOUNTCOMPONENT), CommonsVars.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // Delete Label On Row
                            Label label = this.Controls.Find(String.Format("{0}lableIndex_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as Label;
                            ComboBox cboGiaCong6 = this.Controls.Find(String.Format("{0}cbo_GiaCong_6_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as ComboBox;
                            ComboBox cboGiaCong7 = this.Controls.Find(String.Format("{0}cbo_GiaCong_7_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as ComboBox;
                            TextBox txtPAM_TC = this.Controls.Find(String.Format("{0}PAM_TIEUCHUAN_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;

                            if (label != null) panel_Determine_Input.Controls.Remove(label);
                            if (cboGiaCong6 != null) panel_Determine_Input.Controls.Remove(cboGiaCong6);
                            if (cboGiaCong7 != null) panel_Determine_Input.Controls.Remove(cboGiaCong7);
                            if (txtPAM_TC != null) panel_Determine_Input.Controls.Remove(txtPAM_TC);
                            // Delete multil component
                            for (int i = 1; i <= 7; i++)
                            {
                                /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Delete INPUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                                // Delete All TextBox On Row
                                TextBox textBox = this.Controls.Find(String.Format("{0}txtBox_{1}_{2}", INDEX_GROUP, i, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;

                                if (textBox != null)
                                {
                                    panel_Determine_Input.Controls.Remove(textBox);

                                }
                                // Delete All CheckBox On Row
                                ComboBox cbo = this.Controls.Find(String.Format("{0}cbo_DoNham_{1}_{2}", INDEX_GROUP, i, ROWCOUNTCOMPONENT), true).FirstOrDefault() as ComboBox;

                                if (cbo != null) panel_Determine_Input.Controls.Remove(cbo);

                                /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Delete OUTPUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                                TextBox txtOut1 = this.Controls.Find(String.Format("{0}txtBox_Output_1_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                                TextBox txtOut2 = this.Controls.Find(String.Format("{0}txtBox_Output_2_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                                TextBox txtOut3 = this.Controls.Find(String.Format("{0}txtBox_Output_3_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                                TextBox txtOut4 = this.Controls.Find(String.Format("{0}txtBox_Output_4_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                                TextBox txtOut5 = this.Controls.Find(String.Format("{0}txtBox_Output_5_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                                TextBox txtOut6 = this.Controls.Find(String.Format("{0}txtBox_Output_6_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                                TextBox txtOut7 = this.Controls.Find(String.Format("{0}txtBox_Output_7_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;


                                if (txtOut1 != null) panel_Determine_Output.Controls.Remove(txtOut1);
                                if (txtOut2 != null) panel_Determine_Output.Controls.Remove(txtOut2);
                                if (txtOut3 != null) panel_Determine_Output.Controls.Remove(txtOut3);
                                if (txtOut4 != null) panel_Determine_Output.Controls.Remove(txtOut4);
                                if (txtOut5 != null) panel_Determine_Output.Controls.Remove(txtOut5);
                                if (txtOut6 != null) panel_Determine_Output.Controls.Remove(txtOut6);
                                if (txtOut7 != null) panel_Determine_Output.Controls.Remove(txtOut7);




                                Label labelOut = this.Controls.Find(String.Format("{0}lableIndex_Output_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as Label;
                                if (labelOut != null) panel_Determine_Output.Controls.Remove(labelOut);

                            }
                            rowListPanel[INDEX_GROUP] = rowListPanel[INDEX_GROUP] - 1;
                        }
                    }
                    else
                    {
                        // Delete Label On Row
                        Label label = this.Controls.Find(String.Format("{0}lableIndex_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as Label;
                        ComboBox cboGiaCong6 = this.Controls.Find(String.Format("{0}cbo_GiaCong_6_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as ComboBox;
                        ComboBox cboGiaCong7 = this.Controls.Find(String.Format("{0}cbo_GiaCong_7_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as ComboBox;
                        TextBox txtPAM_TC = this.Controls.Find(String.Format("{0}PAM_TIEUCHUAN_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;

                        if (label != null) panel_Determine_Input.Controls.Remove(label);
                        if (cboGiaCong6 != null) panel_Determine_Input.Controls.Remove(cboGiaCong6);
                        if (cboGiaCong7 != null) panel_Determine_Input.Controls.Remove(cboGiaCong7);
                        if (txtPAM_TC != null) panel_Determine_Input.Controls.Remove(txtPAM_TC);
                        // Delete multil component
                        for (int i = 1; i <= 7; i++)
                        {
                            /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Delete INPUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                            // Delete All TextBox On Row
                            TextBox textBox = this.Controls.Find(String.Format("{0}txtBox_{1}_{2}", INDEX_GROUP, i, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;

                            if (textBox != null)
                            {
                                panel_Determine_Input.Controls.Remove(textBox);

                            }
                            // Delete All CheckBox On Row
                            ComboBox cbo = this.Controls.Find(String.Format("{0}cbo_DoNham_{1}_{2}", INDEX_GROUP, i, ROWCOUNTCOMPONENT), true).FirstOrDefault() as ComboBox;

                            if (cbo != null) panel_Determine_Input.Controls.Remove(cbo);

                            /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Delete OUTPUT~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                            TextBox txtOut1 = this.Controls.Find(String.Format("{0}txtBox_Output_1_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                            TextBox txtOut2 = this.Controls.Find(String.Format("{0}txtBox_Output_2_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                            TextBox txtOut3 = this.Controls.Find(String.Format("{0}txtBox_Output_3_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                            TextBox txtOut4 = this.Controls.Find(String.Format("{0}txtBox_Output_4_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                            TextBox txtOut5 = this.Controls.Find(String.Format("{0}txtBox_Output_5_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                            TextBox txtOut6 = this.Controls.Find(String.Format("{0}txtBox_Output_6_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;
                            TextBox txtOut7 = this.Controls.Find(String.Format("{0}txtBox_Output_7_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as TextBox;


                            if (txtOut1 != null) panel_Determine_Output.Controls.Remove(txtOut1);
                            if (txtOut2 != null) panel_Determine_Output.Controls.Remove(txtOut2);
                            if (txtOut3 != null) panel_Determine_Output.Controls.Remove(txtOut3);
                            if (txtOut4 != null) panel_Determine_Output.Controls.Remove(txtOut4);
                            if (txtOut5 != null) panel_Determine_Output.Controls.Remove(txtOut5);
                            if (txtOut6 != null) panel_Determine_Output.Controls.Remove(txtOut6);
                            if (txtOut7 != null) panel_Determine_Output.Controls.Remove(txtOut7);




                            Label labelOut = this.Controls.Find(String.Format("{0}lableIndex_Output_{1}", INDEX_GROUP, ROWCOUNTCOMPONENT), true).FirstOrDefault() as Label;
                            if (labelOut != null) panel_Determine_Output.Controls.Remove(labelOut);

                        }
                        rowListPanel[INDEX_GROUP] = rowListPanel[INDEX_GROUP] - 1;
                    }
                }
                else
                {
                    MessageBox.Show(this, WARNING_DELETE_ROW_1, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show(this, "Have not row to remove", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
        private void checkBoxMaster_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            string name = checkbox.Name.ToString();
            string[] strArr1 = name.Split(new[] { "_chkMaster" }, System.StringSplitOptions.None);
            string integerDest1 = strArr1[0];
            string[] strArr2 = name.Split(new[] { "_" }, System.StringSplitOptions.None);
            string integerDest2 = strArr2[2];
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            int x_indexGroup = 0;
            Int32.TryParse(integerDest1, out x_indexGroup);

            int y_columnIndex = 0;
            Int32.TryParse(integerDest2, out y_columnIndex);
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            this.RuleEnable_ComboBoxSlave_Follow_CheckboxMaster_Multil(checkbox, y_columnIndex, x_indexGroup);
            this.RuleEnable_TextBoxSlave_Follow_CheckboxMaster_Multil(checkbox, y_columnIndex, x_indexGroup);

        }
        private void checkBoxMaster_Multil_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            string name = checkbox.Name.ToString();
            string[] strArr1 = name.Split(new[] { "_chkMaster" }, System.StringSplitOptions.None);
            string integerDest1 = strArr1[0];

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            int x_indexGroup = 0;
            Int32.TryParse(integerDest1, out x_indexGroup);
            this.selectOrDeSelectDoNham_Multil(x_indexGroup);

        }
        private void btn_AddNew_Group_Click(object sender, EventArgs e)
        {
            try
            {
                //===== Disable Scroll Panel Total
                panel_Total.AutoScroll = false;
                //--------------------------------
                COUNT_GROUP_BOX++;
                int X_LOCATION_GROUP_INPUT = 234;
                int Y_LOCATION_GROUP_INPUT = 60 + (379 - 60) * (COUNT_GROUP_BOX - 1);       // 319x

                int X_LOCATION_GROUP_OUTPUT = 234;
                int Y_LOCATION_GROUP_OUTPUT = 262 + (581 - 262) * (COUNT_GROUP_BOX - 1);      // 319x

                int Y_LOCATION_CHECKBOXMASTER = 46 + (319 + 46 - 46) * (COUNT_GROUP_BOX - 1);  // 319x

                int Y_LOCATION_BUTTON_ADD = 66 + (319 + 66 - 66) * (COUNT_GROUP_BOX - 1);  // 319x 
                int Y_LOCATION_BUTTON_REMOVE = 108 + (319 + 66 - 66) * (COUNT_GROUP_BOX - 1);  // 319x

                int X_LOCATION_COMBOBOX_MATERIAL = 4;
                int Y_LOCATION_COMBOBOX_MATERIAL = 66 + (319 + 66 - 66) * (COUNT_GROUP_BOX - 1);     // 319x ;

                int X_LOCATION_COMBOBOX_SHAPE = 4;
                int Y_LOCATION_COMBOBOX_SHAPE = 117 + (319 + 117 - 117) * (COUNT_GROUP_BOX - 1);        // 319x ;


                int X_LOCATION_COMBOBOX_PROCESS = 79;
                int Y_LOCATION_COMBOBOX_PROCESS = 269 + (319 + 269 - 269) * (COUNT_GROUP_BOX - 1);        // 319x ;

                int X_LOCATION_TEXTBOX_WEIGH = 79;
                int Y_LOCATION_TEXTBOX_WEIGH = 303 + (319 + 303 - 303) * (COUNT_GROUP_BOX - 1);        // 319x ;

                int X_LOCATION_PTRLINE = 3;
                int Y_LOCATION_PTRLINE = 336 + (319 + 336 - 336) * (COUNT_GROUP_BOX - 1);        // 319x ;

                int X_LBL_MATERIAL = 76;
                int Y_LBL_MATERIAL = 47 + (319 + 47 - 47) * (COUNT_GROUP_BOX - 1);        // 319x ;

                int X_LBL_SHAPE = 76;
                int Y_LBL_SHAPE = 101 + (319 + 101 - 101) * (COUNT_GROUP_BOX - 1);        // 319x ;

                int X_LBL_PROCESS = 3;
                int Y_LBL_PROCESS = 269 + (319 + 269 - 269) * (COUNT_GROUP_BOX - 1);        // 319x ;

                int X_LBL_WEIGH = 3;
                int Y_LBL_WEIGH = 303 + (319 + 303 - 303) * (COUNT_GROUP_BOX - 1);        // 319x ;

                int X_LOCATION_PANEL_INPUT = 6;
                int Y_LOCATION_PANEL_INPUT = 19;

                int X_LOCATION_PANEL_OUTPUT = 6;
                int Y_LOCATION_PANEL_OUTPUT = 19;

                /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Create PictureBox ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                PictureBox ptrLine = new PictureBox();

                ptrLine.Name = String.Format("PTRLINE_{0}", COUNT_GROUP_BOX);
                ptrLine.Width = 1780;
                ptrLine.Height = 2;
                ptrLine.BackColor = Color.DarkGreen;
                ptrLine.Location = new Point(X_LOCATION_PTRLINE, Y_LOCATION_PTRLINE);

                panel_Total.Controls.Add(ptrLine);

                /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Create Material and Shape Combobox~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                ComboBox cboMaterial = new ComboBox();
                ComboBox cboShape = new ComboBox();

                cboMaterial.Name = String.Format("CBO_MATERIAL_{0}", COUNT_GROUP_BOX);
                cboShape.Name = String.Format("CBO_SHAPE_{0}", COUNT_GROUP_BOX);

                cboMaterial.Height = 21;
                cboMaterial.Width = 218;

                cboMaterial.Font = new Font(cboMaterial.Font, FontStyle.Regular);
                cboShape.Font = new Font(cboShape.Font, FontStyle.Regular);
                cboShape.DropDownStyle = ComboBoxStyle.DropDownList;

                cboShape.Height = 21;
                cboShape.Width = 218;

                cboMaterial.Location = new Point(X_LOCATION_COMBOBOX_MATERIAL, Y_LOCATION_COMBOBOX_MATERIAL);
                cboShape.Location = new Point(X_LOCATION_COMBOBOX_SHAPE, Y_LOCATION_COMBOBOX_SHAPE);

                // Add Event
                cboMaterial.SelectionChangeCommitted += new System.EventHandler(this.cboMaterial_SelectionChangeCommitted);
                cboShape.SelectionChangeCommitted += new System.EventHandler(this.cboShape_SelectionChangeCommitted);
                //cboShape.SelectedIndexChanged += new System.EventHandler(this.cboShape_SelectionChangeCommitted);


                panel_Total.Controls.Add(cboMaterial);
                panel_Total.Controls.Add(cboShape);

                //lbl
                Label lblMaterial = new Label();
                Label lblShape = new Label();

                Label lblProcess = new Label();
                Label lblWeigh = new Label();

                lblMaterial.Name = String.Format("LBL_MATERIAL_{0}", COUNT_GROUP_BOX);
                lblShape.Name = String.Format("LBL_SHAPE_{0}", COUNT_GROUP_BOX);
                lblProcess.Name = String.Format("LBL_PROCESS_{0}", COUNT_GROUP_BOX);
                lblWeigh.Name = String.Format("LBL_WEIGH_{0}", COUNT_GROUP_BOX);

                lblMaterial.Text = TEXT_LBL_MATERIAL;
                lblShape.Text = TEXT_LBL_SHAPE;
                lblProcess.Text = TEXT_LBL_PROCESS;
                lblWeigh.Text = TEXT_LBL_WEIGH;

                lblMaterial.Height = 13;
                lblShape.Height = 13;
                lblProcess.Height = 13;
                lblWeigh.Height = 13;
                lblMaterial.Width = 61;
                lblShape.Width = 61;
                lblProcess.Width = 61;
                lblWeigh.Width = 61;

                lblMaterial.Location = new Point(X_LBL_MATERIAL, Y_LBL_MATERIAL);
                lblShape.Location = new Point(X_LBL_SHAPE, Y_LBL_SHAPE);
                lblProcess.Location = new Point(X_LBL_PROCESS, Y_LBL_PROCESS);
                lblWeigh.Location = new Point(X_LBL_WEIGH, Y_LBL_WEIGH);

                panel_Total.Controls.Add(lblMaterial);
                panel_Total.Controls.Add(lblShape);
                panel_Total.Controls.Add(lblProcess);
                panel_Total.Controls.Add(lblWeigh);

                /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Create Process Combobox and WEIGH TextBox OutPut ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                ComboBox cboProcess = new ComboBox();
                TextBox textBoxWeigh = new TextBox();

                cboProcess.Name = String.Format("CBO_PROCESS_{0}", COUNT_GROUP_BOX);
                textBoxWeigh.Name = String.Format("TXT_WEIGH_{0}", COUNT_GROUP_BOX);

                cboProcess.Height = 21;
                cboProcess.Width = 143;

                cboProcess.Height = 20;
                textBoxWeigh.Width = 143;

                cboProcess.DropDownStyle = ComboBoxStyle.DropDownList;

                cboProcess.Location = new Point(X_LOCATION_COMBOBOX_PROCESS, Y_LOCATION_COMBOBOX_PROCESS);
                textBoxWeigh.Location = new Point(X_LOCATION_TEXTBOX_WEIGH, Y_LOCATION_TEXTBOX_WEIGH);

                textBoxWeigh.ReadOnly = true;

                panel_Total.Controls.Add(cboProcess);
                panel_Total.Controls.Add(textBoxWeigh);

                /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Create 2 button Create and Remove~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                Button buttonAdd = new Button();
                Button buttonRemove = new Button();
                Button buttonCalculate = new Button();

                buttonAdd.BackColor = Color.LimeGreen;
                buttonRemove.BackColor = Color.LightCoral;
                buttonCalculate.BackColor = Color.AntiqueWhite;

                buttonAdd.Name = String.Format("Button_Add_{0}", COUNT_GROUP_BOX);
                buttonRemove.Name = String.Format("Button_Remove_{0}", COUNT_GROUP_BOX);
                buttonCalculate.Name = String.Format("Button_Calculate_{0}", COUNT_GROUP_BOX);

                buttonAdd.Text = BUTTON_ADD_TEXT;
                buttonRemove.Text = BUTTON_REMOVE_TEXT;
                buttonCalculate.Text = BUTTON_CALCULATE_TEXT;


                buttonAdd.Location = new Point(1647, Y_LOCATION_BUTTON_ADD);
                buttonRemove.Location = new Point(1647, Y_LOCATION_BUTTON_REMOVE);
                buttonCalculate.Location = new Point(1647, Y_LOCATION_BUTTON_REMOVE + 45);

                buttonAdd.Click += new System.EventHandler(this.Function_Event_Create_Row_Once_Group);
                buttonRemove.Click += new System.EventHandler(this.Function_Event_Remove_Row_Once_Group);
                buttonCalculate.Click += new System.EventHandler(this.Function_Calculate);

                buttonAdd.Enabled = false;

                panel_Total.Controls.Add(buttonAdd);
                panel_Total.Controls.Add(buttonRemove);
                panel_Total.Controls.Add(buttonCalculate);

                /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Create Loop CHECKBOX MASTER (7 checkbox) ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                CheckBox checkbox1 = new CheckBox();
                CheckBox checkbox2 = new CheckBox();
                CheckBox checkbox3 = new CheckBox();
                CheckBox checkbox4 = new CheckBox();
                CheckBox checkbox5 = new CheckBox();
                CheckBox checkbox6 = new CheckBox();
                CheckBox checkbox7 = new CheckBox();
                CheckBox checkboxAll = new CheckBox();

                checkbox1.AutoSize = false;
                checkbox2.AutoSize = false;
                checkbox3.AutoSize = false;
                checkbox4.AutoSize = false;
                checkbox5.AutoSize = false;
                checkbox6.AutoSize = false;
                checkbox7.AutoSize = false;
                checkboxAll.AutoSize = false;

                checkbox1.Height = CHECKBOX_HEIGHT;
                checkbox2.Height = CHECKBOX_HEIGHT;
                checkbox3.Height = CHECKBOX_HEIGHT;
                checkbox4.Height = CHECKBOX_HEIGHT;
                checkbox5.Height = CHECKBOX_HEIGHT;
                checkbox6.Height = CHECKBOX_HEIGHT;
                checkbox7.Height = CHECKBOX_HEIGHT;
                checkboxAll.Height = CHECKBOX_HEIGHT;

                //checkbox1.Width = CHECKBOX_WIDTH;
                //checkbox2.Width = CHECKBOX_WIDTH;
                //checkbox3.Width = CHECKBOX_WIDTH;
                //checkbox4.Width = CHECKBOX_WIDTH;
                //checkbox5.Width = CHECKBOX_WIDTH;
                //checkbox6.Width = CHECKBOX_WIDTH;
                //checkbox7.Width = CHECKBOX_WIDTH;
                //checkboxAll.Width = CHECKBOX_WIDTH;

                checkbox1.Name = String.Format("{0}_chkMaster_1", COUNT_GROUP_BOX);
                checkbox2.Name = String.Format("{0}_chkMaster_2", COUNT_GROUP_BOX);
                checkbox3.Name = String.Format("{0}_chkMaster_3", COUNT_GROUP_BOX);
                checkbox4.Name = String.Format("{0}_chkMaster_4", COUNT_GROUP_BOX);
                checkbox5.Name = String.Format("{0}_chkMaster_5", COUNT_GROUP_BOX);
                checkbox6.Name = String.Format("{0}_chkMaster_6", COUNT_GROUP_BOX);
                checkbox7.Name = String.Format("{0}_chkMaster_7", COUNT_GROUP_BOX);
                checkboxAll.Name = String.Format("{0}_chkMaster_All", COUNT_GROUP_BOX);

                checkbox1.Text = TEXT_ACTIVE_CHECKBOXMASTER;
                checkbox2.Text = TEXT_ACTIVE_CHECKBOXMASTER;
                checkbox3.Text = TEXT_ACTIVE_CHECKBOXMASTER;
                checkbox4.Text = TEXT_ACTIVE_CHECKBOXMASTER;
                checkbox5.Text = TEXT_ACTIVE_CHECKBOXMASTER;
                checkbox6.Text = TEXT_ACTIVE_CHECKBOXMASTER;
                checkbox7.Text = TEXT_ACTIVE_CHECKBOXMASTER;
                checkboxAll.Text = TEXT_ALL_DO_NHAM;

                checkbox1.Location = new Point(320, Y_LOCATION_CHECKBOXMASTER);
                checkbox2.Location = new Point(494, Y_LOCATION_CHECKBOXMASTER);
                checkbox3.Location = new Point(678, Y_LOCATION_CHECKBOXMASTER);
                checkbox4.Location = new Point(865, Y_LOCATION_CHECKBOXMASTER);
                checkbox5.Location = new Point(1060, Y_LOCATION_CHECKBOXMASTER);
                checkbox6.Location = new Point(1239, Y_LOCATION_CHECKBOXMASTER);
                checkbox7.Location = new Point(1426, Y_LOCATION_CHECKBOXMASTER);
                checkboxAll.Location = new Point(1561, Y_LOCATION_CHECKBOXMASTER);

                checkbox1.CheckedChanged += new System.EventHandler(this.checkBoxMaster_CheckedChanged);
                checkbox2.CheckedChanged += new System.EventHandler(this.checkBoxMaster_CheckedChanged);
                checkbox3.CheckedChanged += new System.EventHandler(this.checkBoxMaster_CheckedChanged);
                checkbox4.CheckedChanged += new System.EventHandler(this.checkBoxMaster_CheckedChanged);
                checkbox5.CheckedChanged += new System.EventHandler(this.checkBoxMaster_CheckedChanged);
                checkbox6.CheckedChanged += new System.EventHandler(this.checkBoxMaster_CheckedChanged);
                checkbox7.CheckedChanged += new System.EventHandler(this.checkBoxMaster_CheckedChanged);
                checkboxAll.CheckedChanged += new System.EventHandler(this.checkBoxMaster_Multil_CheckedChanged);

                panel_Total.Controls.Add(checkbox1);
                panel_Total.Controls.Add(checkbox2);
                panel_Total.Controls.Add(checkbox3);
                panel_Total.Controls.Add(checkbox4);
                panel_Total.Controls.Add(checkbox5);
                panel_Total.Controls.Add(checkbox6);
                panel_Total.Controls.Add(checkbox7);
                panel_Total.Controls.Add(checkboxAll);

                /******************************** Create new {GroupBoxInput[PanelInput]} ****************************************/
                GroupBox newGroupInput = new GroupBox();
                newGroupInput.Name = String.Format("INPUT{0}", COUNT_GROUP_BOX);
                newGroupInput.Text = String.Format("INPUT{0}", COUNT_GROUP_BOX);

                newGroupInput.Height = HEIGHT_GROUPBOX_INPUT;
                newGroupInput.Width = WIDTH_GROUPBOX_INPUT;

                newGroupInput.Name = String.Format("INPUT_GROUPBOX_{0}", COUNT_GROUP_BOX);

                newGroupInput.Location = new Point(X_LOCATION_GROUP_INPUT, Y_LOCATION_GROUP_INPUT);
                panel_Total.Controls.Add(newGroupInput);

                //--Create new Panel
                Panel panelInput = new Panel();
                panelInput.Name = String.Format("INPUT_PANEL_{0}", COUNT_GROUP_BOX);
                panelInput.Location = new Point(X_LOCATION_PANEL_INPUT, Y_LOCATION_PANEL_INPUT);
                panelInput.Height = HEIGHT_PANEL_INPUT;
                panelInput.Width = WIDTH_PANEL_INPUT;
                panelInput.Location = new Point(6, 19);
                panelInput.BorderStyle = BorderStyle.Fixed3D;
                newGroupInput.Controls.Add(panelInput);

                /******************************** Create new {GroupBoxOutput[PanelOutput]} ****************************************/
                GroupBox newGroupOutput = new GroupBox();
                newGroupOutput.Name = String.Format("OUTPUT{0}", COUNT_GROUP_BOX);
                newGroupOutput.Text = String.Format("OUTPUT{0}", COUNT_GROUP_BOX);

                newGroupOutput.Height = HEIGHT_GROUPBOX_OUTPUT;
                newGroupOutput.Width = WIDTH_GROUPBOX_OUTPUT;

                newGroupOutput.Name = String.Format("OUTPUT_GROUPBOX_{0}", COUNT_GROUP_BOX);

                newGroupOutput.Location = new Point(X_LOCATION_GROUP_OUTPUT, Y_LOCATION_GROUP_OUTPUT);
                panel_Total.Controls.Add(newGroupOutput);

                //--Create new Panel Output
                Panel panelOutput = new Panel();
                panelOutput.Name = String.Format("OUTPUT_PANEL_{0}", COUNT_GROUP_BOX);
                panelOutput.Location = new Point(X_LOCATION_PANEL_OUTPUT, Y_LOCATION_PANEL_OUTPUT);
                panelOutput.Height = HEIGHT_PANEL_OUTPUT;
                panelOutput.Width = WIDTH_PANEL_OUTPUT;
                panelOutput.Location = new Point(6, 19);
                panelOutput.BorderStyle = BorderStyle.FixedSingle;
                newGroupOutput.Controls.Add(panelOutput);

                // Create First Row
                this.createNewRow_Input_Multil(COUNT_GROUP_BOX);
                this.createNewRow_Output_Multil(COUNT_GROUP_BOX);
                this.loadDataMaterial_Multil(COUNT_GROUP_BOX);
                //===== Enable Scroll Panel Total
                panel_Total.AutoScroll = true;

            }
            catch (Exception ex)
            {
                COUNT_GROUP_BOX--;
                Console.WriteLine(ex);
            }
        }
        private void Function_Event_Create_Row_Once_Group(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string NameButton = button.Name.ToString();
            string[] strArr = NameButton.Split(new[] { "_" }, System.StringSplitOptions.None);
            string integerDest = strArr[2];
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            int x = 0;
            Int32.TryParse(integerDest, out x);
            //=================================
            this.createNewRow_Input_Multil(x);
            this.createNewRow_Output_Multil(x);
            this.selectOrDeSelectDoNham_Multil(x);
            this.loadDataForAllDoNham(x, rowListPanel[x]);
            this.Rule_ComboBoxSlave_All(x);
        }
        private void Function_Event_Remove_Row_Once_Group(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string NameButton = button.Name.ToString();
            string[] strArr = NameButton.Split(new[] { "_" }, System.StringSplitOptions.None);
            string integerDest = strArr[2];
            //=================================
            this.deleteLastRow_Multil(this.convertStringToInt(integerDest));
        }

        private void Function_Calculate(object sender, EventArgs e)
        {

            Button btn = (Button)sender;
            double totalWeigh = 0;
            int countZeroWeigh = 0;
            int indexGroup = this.convertStringToInt(splitString(btn.Name, "Button_Calculate_", 1));
            TextBox txtWeigh = this.Controls.Find(String.Format("TXT_WEIGH_{0}", indexGroup), true).FirstOrDefault() as TextBox;
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);
            double weighIndex = 0;
            for (int i = 1; i <= rowListPanel[indexGroup]; i++)
            {
                TextBox textInPut1 = this.Controls.Find(String.Format("{0}txtBox_1_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textInPut2 = this.Controls.Find(String.Format("{0}txtBox_2_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textInPut3 = this.Controls.Find(String.Format("{0}txtBox_3_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textInPut4 = this.Controls.Find(String.Format("{0}txtBox_3_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textInPut_KTGC1 = this.Controls.Find(String.Format("{0}txtBox_6_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textInPut_KTGC2 = this.Controls.Find(String.Format("{0}txtBox_7_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;

                ComboBox cBoDoNham_1 = this.Controls.Find(String.Format("{0}cbo_DoNham_1_{1}", indexGroup, i), true).FirstOrDefault() as ComboBox;
                ComboBox cBoDoNham_2 = this.Controls.Find(String.Format("{0}cbo_DoNham_2_{1}", indexGroup, i), true).FirstOrDefault() as ComboBox;
                ComboBox cBoDoNham_3 = this.Controls.Find(String.Format("{0}cbo_DoNham_3_{1}", indexGroup, i), true).FirstOrDefault() as ComboBox;
                ComboBox cBoDoNham_4 = this.Controls.Find(String.Format("{0}cbo_DoNham_4_{1}", indexGroup, i), true).FirstOrDefault() as ComboBox;
                ComboBox cBoDoNham_KTGC1 = this.Controls.Find(String.Format("{0}cbo_DoNham_6_{1}", indexGroup, i), true).FirstOrDefault() as ComboBox;
                ComboBox cBoDoNham_KTGC2 = this.Controls.Find(String.Format("{0}cbo_DoNham_7_{1}", indexGroup, i), true).FirstOrDefault() as ComboBox;

                ComboBox cboGiaCong6 = this.Controls.Find(String.Format("{0}cbo_GiaCong_6_{1}", indexGroup, i), true).FirstOrDefault() as ComboBox;
                ComboBox cboGiaCong7 = this.Controls.Find(String.Format("{0}cbo_GiaCong_7_{1}", indexGroup, i), true).FirstOrDefault() as ComboBox;

                TextBox textBoxOutPut1 = this.Controls.Find(String.Format("{0}txtBox_Output_1_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textBoxOutPut2 = this.Controls.Find(String.Format("{0}txtBox_Output_2_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textBoxOutPut3 = this.Controls.Find(String.Format("{0}txtBox_Output_3_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textBoxOutPut4 = this.Controls.Find(String.Format("{0}txtBox_Output_4_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textBoxOutPut_KTGC1 = this.Controls.Find(String.Format("{0}txtBox_Output_6_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;
                TextBox textBoxOutPut_KTGC2 = this.Controls.Find(String.Format("{0}txtBox_Output_7_{1}", indexGroup, i), true).FirstOrDefault() as TextBox;

                //==> ABCD
                if (cboShape_StrIndex.Equals("16") || cboShape_StrIndex.Equals("46"))
                {
                    weighIndex = this.All_Execute_A_B_C_D_vs_KTGC(indexGroup, i, textInPut1, textInPut2, textInPut3, textInPut4, textInPut_KTGC1, cBoDoNham_1, cBoDoNham_2,
                        cBoDoNham_3, cBoDoNham_4, cBoDoNham_KTGC1, cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut3, textBoxOutPut4, textBoxOutPut_KTGC1);
                }
                //==> ABC
                if (cboShape_StrIndex.Equals("08") || cboShape_StrIndex.Equals("38") ||
                    cboShape_StrIndex.Equals("12") || cboShape_StrIndex.Equals("42") ||
                    cboShape_StrIndex.Equals("13") || cboShape_StrIndex.Equals("43") ||
                    cboShape_StrIndex.Equals("18") || cboShape_StrIndex.Equals("48"))
                {
                    weighIndex = this.All_Execute_A_B_C_vs_KTGC(indexGroup, i, textInPut1, textInPut2, textInPut3, textInPut_KTGC1, cBoDoNham_1, cBoDoNham_2, cBoDoNham_3, cBoDoNham_KTGC1,
                                                                     cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut3, textBoxOutPut_KTGC1);
                }
                //==> AB
                if (cboShape_StrIndex.Equals("06") || cboShape_StrIndex.Equals("36") ||
                    cboShape_StrIndex.Equals("07") || cboShape_StrIndex.Equals("17"))
                {
                    weighIndex = this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut1, textInPut2, textInPut_KTGC1, cBoDoNham_1, cBoDoNham_2, cBoDoNham_KTGC1,
                    cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                }
                //==> 01
                // With Shape = 01 or 31. We have private for it. We will choose get MIN weigh to optimize and safe
                if (cboShape_StrIndex.Equals("01") || cboShape_StrIndex.Equals("31"))
                {
                    double weighIndex1 = this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut1, textInPut2, textInPut_KTGC1, cBoDoNham_1, cBoDoNham_2, cBoDoNham_KTGC1,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                    if (weighIndex1 <= 0) weighIndex1 = 999999999;

                    double weighIndex2 = this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut1, textInPut_KTGC1, textInPut2, cBoDoNham_1, cBoDoNham_KTGC1, cBoDoNham_2,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                    if (weighIndex2 <= 0) weighIndex2 = 999999999;

                    double weighIndex3 = this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut2, textInPut1, textInPut_KTGC1, cBoDoNham_2, cBoDoNham_1, cBoDoNham_KTGC1,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                    if (weighIndex3 <= 0) weighIndex3 = 999999999;

                    double weighIndex4 = this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut2, textInPut_KTGC1, textInPut1, cBoDoNham_2, cBoDoNham_KTGC1, cBoDoNham_1,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                    if (weighIndex4 <= 0) weighIndex4 = 999999999;

                    double weighIndex5 = this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut_KTGC1, textInPut1, textInPut2, cBoDoNham_KTGC1, cBoDoNham_1, cBoDoNham_2,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                    if (weighIndex5 <= 0) weighIndex5 = 999999999;

                    double weighIndex6 = this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut_KTGC1, textInPut2, textInPut1, cBoDoNham_KTGC1, cBoDoNham_2, cBoDoNham_1,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                    if (weighIndex6 <= 0) weighIndex6 = 999999999;

                    double[] array1 = { weighIndex1, weighIndex2, weighIndex3, weighIndex4, weighIndex5, weighIndex6 };
                    weighIndex = array1.Min();
                    int indexMin = -1;
                    if (weighIndex == weighIndex1) indexMin = 1;
                    if (weighIndex == weighIndex2) indexMin = 2;
                    if (weighIndex == weighIndex3) indexMin = 3;
                    if (weighIndex == weighIndex4) indexMin = 4;
                    if (weighIndex == weighIndex5) indexMin = 5;
                    if (weighIndex == weighIndex6) indexMin = 6;
                    switch (indexMin)
                    {

                        case 1:
                            this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut1, textInPut2, textInPut_KTGC1, cBoDoNham_1, cBoDoNham_2, cBoDoNham_KTGC1,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 2:
                            this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut1, textInPut_KTGC1, textInPut2, cBoDoNham_1, cBoDoNham_KTGC1, cBoDoNham_2,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 3:
                            this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut2, textInPut1, textInPut_KTGC1, cBoDoNham_2, cBoDoNham_1, cBoDoNham_KTGC1,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 4:
                            this.All_Execute_A_B_vs_KTGC(indexGroup, i, textInPut2, textInPut_KTGC1, textInPut2, cBoDoNham_2, cBoDoNham_KTGC1, cBoDoNham_1,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 5:
                            this.All_Execute_A_B_vs_KTGC(indexGroup, i, textBoxOutPut_KTGC1, textInPut1, textInPut2, cBoDoNham_KTGC1, cBoDoNham_1, cBoDoNham_2,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 6:
                            this.All_Execute_A_B_vs_KTGC(indexGroup, i, textBoxOutPut_KTGC1, textInPut2, textInPut1, cBoDoNham_KTGC1, cBoDoNham_2, cBoDoNham_1,
                                                           cboGiaCong6, cboGiaCong7, textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        default:
                            Console.WriteLine("unknown!");
                            break;
                    }
                }
                //==> A..02.04
                if (cboShape_StrIndex.Equals("02") || cboShape_StrIndex.Equals("32") ||
                    cboShape_StrIndex.Equals("04") || cboShape_StrIndex.Equals("34"))
                {
                    weighIndex = this.All_Execute_A_vs_KTGC(indexGroup, i, textInPut1, textInPut_KTGC1, cBoDoNham_1, cBoDoNham_KTGC1, cboGiaCong6, textBoxOutPut1, textBoxOutPut_KTGC1);
                }
                //==> A..05
                //Mã hình dáng đặc biệt 05
                if (cboShape_StrIndex.Equals("05") || cboShape_StrIndex.Equals("35"))
                    
                {
                    double weighIndex11 = this.All_Execute_Shape_05(indexGroup, i, textInPut1, textInPut_KTGC1, textInPut_KTGC2, cBoDoNham_1, cBoDoNham_KTGC1, cBoDoNham_KTGC2, 
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                    if (weighIndex11 <= 0) weighIndex11 = 999999999;

                    double weighIndex22 = this.All_Execute_Shape_05(indexGroup, i, textInPut1, textInPut_KTGC2, textInPut_KTGC1, cBoDoNham_1, cBoDoNham_KTGC2, cBoDoNham_KTGC1, 
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                    if (weighIndex22 <= 0) weighIndex22 = 999999999;

                    double weighIndex33 = this.All_Execute_Shape_05(indexGroup, i, textInPut_KTGC1, textInPut1, textInPut_KTGC2, cBoDoNham_KTGC1, cBoDoNham_1, cBoDoNham_KTGC2, 
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                    if (weighIndex33 <= 0) weighIndex33 = 999999999;

                    double weighIndex44 = this.All_Execute_Shape_05(indexGroup, i, textInPut_KTGC1, textInPut_KTGC2, textInPut1, cBoDoNham_KTGC1, cBoDoNham_KTGC2, cBoDoNham_1, 
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                    if (weighIndex44 <= 0) weighIndex44 = 999999999;

                    double weighIndex55 = this.All_Execute_Shape_05(indexGroup, i, textInPut_KTGC2, textInPut1, textInPut_KTGC1, cBoDoNham_KTGC2, cBoDoNham_1, cBoDoNham_KTGC1, 
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                    if (weighIndex55 <= 0) weighIndex55 = 999999999;

                    double weighIndex66 = this.All_Execute_Shape_05(indexGroup, i, textInPut_KTGC2, textInPut_KTGC1, textInPut1, cBoDoNham_KTGC2, cBoDoNham_KTGC1, cBoDoNham_1, 
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                    if (weighIndex66 <= 0) weighIndex66 = 999999999;

                    double[] array2 = { weighIndex11, weighIndex22, weighIndex33, weighIndex44, weighIndex55, weighIndex66 };
                    weighIndex = array2.Min();
                    int indexMin5 = -1;
                    if (weighIndex == weighIndex11) indexMin5 = 1;
                    if (weighIndex == weighIndex22) indexMin5 = 2;
                    if (weighIndex == weighIndex33) indexMin5 = 3;
                    if (weighIndex == weighIndex44) indexMin5 = 4;
                    if (weighIndex == weighIndex55) indexMin5 = 5;
                    if (weighIndex == weighIndex66) indexMin5 = 6;
                    switch (indexMin5)
                    {
                        case 1:
                            this.All_Execute_Shape_05(indexGroup, i, textInPut1, textInPut_KTGC1, textInPut_KTGC2, cBoDoNham_1, cBoDoNham_KTGC1, cBoDoNham_KTGC2,
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                            break;

                        case 2:
                            this.All_Execute_Shape_05(indexGroup, i, textInPut1, textInPut_KTGC2, textInPut_KTGC1, cBoDoNham_1, cBoDoNham_KTGC2, cBoDoNham_KTGC1,
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                            break;

                        case 3:
                            this.All_Execute_Shape_05(indexGroup, i, textInPut_KTGC1, textInPut1, textInPut_KTGC2, cBoDoNham_KTGC1, cBoDoNham_1, cBoDoNham_KTGC2,
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                            break;

                        case 4:
                            this.All_Execute_Shape_05(indexGroup, i, textInPut_KTGC1, textInPut_KTGC1, textInPut1, cBoDoNham_KTGC1, cBoDoNham_KTGC2, cBoDoNham_1,
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                            break;

                        case 5:
                            this.All_Execute_Shape_05(indexGroup, i, textInPut_KTGC2, textInPut1, textInPut_KTGC1, cBoDoNham_KTGC2, cBoDoNham_1, cBoDoNham_KTGC1,
                                                                                   textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                            break;

                        case 6:
                            this.All_Execute_Shape_05(indexGroup, i, textInPut_KTGC2, textInPut_KTGC1, textInPut1, cBoDoNham_KTGC2, cBoDoNham_KTGC1, cBoDoNham_1,
                                                                                  textBoxOutPut1, textBoxOutPut_KTGC1, textBoxOutPut_KTGC2);
                            break;

                        default:
                            Console.WriteLine("unknown!");
                            break;
                    }
                }
                //==> A..03
                // Mã hình dáng đặc biệt 03
                if (cboShape_StrIndex.Equals("03") || cboShape_StrIndex.Equals("33"))
                {
                    double weighIndex31 = this.All_Execute_Shape_03(indexGroup, i, textInPut1, textInPut2,textInPut_KTGC1,cBoDoNham_1, cBoDoNham_2,cBoDoNham_KTGC1,
                        textBoxOutPut1, textBoxOutPut2,textBoxOutPut_KTGC1);
                    double weighIndex32 = this.All_Execute_Shape_03(indexGroup, i, textInPut1, textInPut_KTGC1, textInPut2, cBoDoNham_1, cBoDoNham_KTGC1, cBoDoNham_2,
                        textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);

                    double weighIndex33 = this.All_Execute_Shape_03(indexGroup, i, textInPut2, textInPut1, textInPut_KTGC1, cBoDoNham_2, cBoDoNham_1, cBoDoNham_KTGC1,
                        textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                    double weighIndex34 = this.All_Execute_Shape_03(indexGroup, i, textInPut2, textInPut_KTGC1, textInPut1, cBoDoNham_2, cBoDoNham_KTGC1, cBoDoNham_1,
                        textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);

                    double weighIndex35 = this.All_Execute_Shape_03(indexGroup, i, textInPut_KTGC1, textInPut1, textInPut2, cBoDoNham_KTGC1, cBoDoNham_1, cBoDoNham_2,
                        textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                    double weighIndex36 = this.All_Execute_Shape_03(indexGroup, i, textInPut_KTGC1, textInPut2, textInPut1, cBoDoNham_KTGC1, cBoDoNham_2, cBoDoNham_1,
                        textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);

                    double[] array3 = { weighIndex31, weighIndex32, weighIndex33, weighIndex34, weighIndex35, weighIndex36 };
                    weighIndex = array3.Min();
                    int indexMin3 = -1;
                    if (weighIndex == weighIndex31) indexMin3 = 1;
                    if (weighIndex == weighIndex32) indexMin3 = 2;
                    if (weighIndex == weighIndex33) indexMin3 = 3;
                    if (weighIndex == weighIndex34) indexMin3 = 4;
                    if (weighIndex == weighIndex35) indexMin3 = 5;
                    if (weighIndex == weighIndex36) indexMin3 = 6;

                    switch (indexMin3)
                    {
                        case 1:
                            this.All_Execute_Shape_03(indexGroup, i, textInPut1, textInPut2, textInPut_KTGC1, cBoDoNham_1, cBoDoNham_2, cBoDoNham_KTGC1,
                        textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 2:
                            this.All_Execute_Shape_03(indexGroup, i, textInPut1, textInPut_KTGC1, textInPut2, cBoDoNham_1, cBoDoNham_KTGC1, cBoDoNham_2,
                        textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 3:
                            this.All_Execute_Shape_03(indexGroup, i, textInPut2, textInPut1, textInPut_KTGC1, cBoDoNham_2, cBoDoNham_1, cBoDoNham_KTGC1,
                       textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 4:
                            this.All_Execute_Shape_03(indexGroup, i, textInPut2, textInPut_KTGC1, textInPut1, cBoDoNham_2, cBoDoNham_KTGC1, cBoDoNham_1,
                        textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 5:
                            this.All_Execute_Shape_03(indexGroup, i, textInPut_KTGC1, textInPut1, textInPut2, cBoDoNham_KTGC1, cBoDoNham_1, cBoDoNham_2,
                       textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        case 6:
                            this.All_Execute_Shape_03(indexGroup, i, textInPut_KTGC1, textInPut2, textInPut1, cBoDoNham_KTGC1, cBoDoNham_2, cBoDoNham_1,
                        textBoxOutPut1, textBoxOutPut2, textBoxOutPut_KTGC1);
                            break;

                        default:
                            Console.WriteLine("unknown!");
                            break;
                    }
                }

                // Check Weigh
                if (weighIndex <= 0)
                {
                    countZeroWeigh += 1;
                }

                totalWeigh += weighIndex;
            }


            if (countZeroWeigh == 0)
            {
                txtWeigh.Text = totalWeigh.ToString() + " GRAM";
                txtWeigh.BackColor = Color.LimeGreen;
                txtWarning.Text = "OK!!!";
                txtWarning.BackColor = Color.LightGreen;
            }
            else
            {
                txtWarning.Text = "ERROR";
                txtWarning.BackColor = Color.Yellow;
                txtWeigh.Text = "FILL AGAIN";
                txtWeigh.BackColor = Color.Orange;
            }
            if (totalWeigh % 999999999 == 0)
            {
                txtWeigh.Text = "FILL AGAIN";
                MessageBox.Show(this,"ERROR.Fill Again Not Available","Lưu ý",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                txtWarning.Text = "ERROR";
                txtWarning.BackColor = Color.Orange;
                txtWeigh.BackColor = Color.Orange;
            }

        }


        private void btn_ChangeColor_Click(object sender, EventArgs e)
        {
            if (colorDialog_2.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                panel_Total.BackColor = colorDialog_2.Color;
            }
        }
        private void btn_ChangeColor2_Click(object sender, EventArgs e)
        {
            if (colorDialog_1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                this.BackColor = colorDialog_1.Color;
            }
        }
        private void btn_Delete_Group_Click(object sender, EventArgs e)
        {
            if (COUNT_GROUP_BOX > 1)
            {
                if (MessageBox.Show(this, String.Format(MESSAGE_WARNING_DELETE + " {0}", COUNT_GROUP_BOX), CommonsVars.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Delete multil component
                    for (int i = 1; i <= 7; i++)
                    {

                        // Delete All CheckBox On Row
                        CheckBox checkbox1 = this.Controls.Find(String.Format("{0}_chkMaster_1", COUNT_GROUP_BOX), true).FirstOrDefault() as CheckBox;
                        CheckBox checkbox2 = this.Controls.Find(String.Format("{0}_chkMaster_2", COUNT_GROUP_BOX), true).FirstOrDefault() as CheckBox;
                        CheckBox checkbox3 = this.Controls.Find(String.Format("{0}_chkMaster_3", COUNT_GROUP_BOX), true).FirstOrDefault() as CheckBox;
                        CheckBox checkbox4 = this.Controls.Find(String.Format("{0}_chkMaster_4", COUNT_GROUP_BOX), true).FirstOrDefault() as CheckBox;
                        CheckBox checkbox5 = this.Controls.Find(String.Format("{0}_chkMaster_5", COUNT_GROUP_BOX), true).FirstOrDefault() as CheckBox;
                        CheckBox checkbox6 = this.Controls.Find(String.Format("{0}_chkMaster_6", COUNT_GROUP_BOX), true).FirstOrDefault() as CheckBox;
                        CheckBox checkbox7 = this.Controls.Find(String.Format("{0}_chkMaster_7", COUNT_GROUP_BOX), true).FirstOrDefault() as CheckBox;
                        CheckBox checboxAll = this.Controls.Find(String.Format("{0}_chkMaster_All", COUNT_GROUP_BOX), true).FirstOrDefault() as CheckBox;

                        if (checkbox1 != null) panel_Total.Controls.Remove(checkbox1);
                        if (checkbox2 != null) panel_Total.Controls.Remove(checkbox2);
                        if (checkbox3 != null) panel_Total.Controls.Remove(checkbox3);
                        if (checkbox4 != null) panel_Total.Controls.Remove(checkbox4);
                        if (checkbox5 != null) panel_Total.Controls.Remove(checkbox5);
                        if (checkbox6 != null) panel_Total.Controls.Remove(checkbox6);
                        if (checkbox7 != null) panel_Total.Controls.Remove(checkbox7);
                        if (checkbox7 != null) panel_Total.Controls.Remove(checkbox7);
                        if (checboxAll != null) panel_Total.Controls.Remove(checboxAll);
                        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Delete combobox~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                        ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as ComboBox;
                        ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as ComboBox;
                        if (cboMaterial != null) panel_Total.Controls.Remove(cboMaterial);
                        if (cboShape != null) panel_Total.Controls.Remove(cboShape);
                        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Delete BUTTON~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                        Button buttonAdd = this.Controls.Find(String.Format("Button_Add_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as Button;
                        Button buttonRemove = this.Controls.Find(String.Format("Button_Remove_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as Button;
                        Button buttonCalculate = this.Controls.Find(String.Format("Button_Calculate_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as Button;
                        if (buttonAdd != null) panel_Total.Controls.Remove(buttonAdd);
                        if (buttonRemove != null) panel_Total.Controls.Remove(buttonRemove);
                        if (buttonCalculate != null) panel_Total.Controls.Remove(buttonCalculate);
                        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Delete GROUP~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                        GroupBox groupInput = this.Controls.Find(String.Format("INPUT_GROUPBOX_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as GroupBox;
                        GroupBox groupOutput = this.Controls.Find(String.Format("OUTPUT_GROUPBOX_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as GroupBox;
                        if (groupInput != null) panel_Total.Controls.Remove(groupInput);
                        if (groupOutput != null) panel_Total.Controls.Remove(groupOutput);

                        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                        ComboBox comboboxProcess = this.Controls.Find(String.Format("CBO_PROCESS_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as ComboBox;
                        TextBox txtWeigh = this.Controls.Find(String.Format("TXT_WEIGH_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as TextBox;

                        if (comboboxProcess != null) panel_Total.Controls.Remove(comboboxProcess);
                        if (txtWeigh != null) panel_Total.Controls.Remove(txtWeigh);
                        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Delete PTRLINE~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
                        PictureBox ptrLine = this.Controls.Find(String.Format("PTRLINE_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as PictureBox;
                        if (ptrLine != null) panel_Total.Controls.Remove(ptrLine);
                        //
                        Label lblMaterial = this.Controls.Find(String.Format("LBL_MATERIAL_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as Label;
                        Label lblShape = this.Controls.Find(String.Format("LBL_SHAPE_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as Label;
                        Label lblProcess = this.Controls.Find(String.Format("LBL_PROCESS_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as Label;
                        Label lblWeigh = this.Controls.Find(String.Format("LBL_WEIGH_{0}", COUNT_GROUP_BOX), true).FirstOrDefault() as Label;

                        if (lblMaterial != null) panel_Total.Controls.Remove(lblMaterial);
                        if (lblShape != null) panel_Total.Controls.Remove(lblShape);
                        if (lblProcess != null) panel_Total.Controls.Remove(lblProcess);
                        if (lblWeigh != null) panel_Total.Controls.Remove(lblWeigh);
                    }
                    rowListPanel.RemoveAt(COUNT_GROUP_BOX - 1);
                    COUNT_GROUP_BOX--;
                }
            }
            else
            {
                MessageBox.Show(this, WARNING_DELETE_ROW_1, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Funny event to beauty Guild
        private void pictureButton_Click(object sender, EventArgs e)
        {
            try
            {
                var bm = new Bitmap(Image.FromFile(CommonsVars.PATH_RUN_APP + "/ImageFolder/star1.png"), new Size(35, 35));
                if (funImage < 3)
                {
                    funImage++;
                }
                else
                {
                    funImage = 0;
                }
                if (funImage > 0)
                {
                    if (funImage == 1) bm = new Bitmap(Image.FromFile(CommonsVars.PATH_RUN_APP + "/ImageFolder/star1.png"), new Size(35, 35));
                    else if (funImage == 2) bm = new Bitmap(Image.FromFile(CommonsVars.PATH_RUN_APP + "/ImageFolder/star2.png"), new Size(35, 35));
                    else if (funImage == 3) bm = new Bitmap(Image.FromFile(CommonsVars.PATH_RUN_APP + "/ImageFolder/hammer.ico"), new Size(35, 35));

                    ptrBox1.Image = bm;
                    ptrBox2.Image = bm;
                    ptrBox3.Image = bm;
                    ptrBox4.Image = bm;
                    ptrBox5.Image = bm;
                    ptrBox6.Image = bm;
                    ptrBox7.Image = bm;
                }

                else
                {
                    ptrBox1.Image = null;
                    ptrBox2.Image = null;
                    ptrBox3.Image = null;
                    ptrBox4.Image = null;
                    ptrBox5.Image = null;
                    ptrBox6.Image = null;
                    ptrBox7.Image = null;
                }
            }
            catch
            {

            }
        }
        private void selectOrDeSelectDoNham_Multil(int index_Group)
        {
            try
            {
                CheckBox checkboxMaster = this.Controls.Find(String.Format("{0}_chkMaster_All", index_Group), true).FirstOrDefault() as CheckBox;
                // Choose All or No Choose All Độ Nhám
                for (int i = 1; i <= 7; i++)
                {
                    for (int j = 1; j <= rowListPanel[index_Group]; j++)
                    {
                        ComboBox cboDoNham = this.Controls.Find(String.Format("{0}cbo_DoNham_{1}_{2}", index_Group, i, j), true).FirstOrDefault() as ComboBox;
                        if (cboDoNham != null) cboDoNham.Enabled = checkboxMaster.Checked;
                        if (checkboxMaster.Checked == false) cboDoNham.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(this, WARNING_NO_COMPONENT, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

        }
        private void loadDataForAllDoNham(int index_Group, int RowIsLoaded)
        {
            List<string> listDoNham  = new List<string>();
            List<string> listDoNham1 = new List<string>() { "0", "1", "2" };
            List<string> listDoNham2 = new List<string>() { "0", "1", "2" };
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", index_Group), true).FirstOrDefault() as ComboBox;
            switch (splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0))
            {
                case "01":
                case "03":
                case "05":
                case "07":
                case "08":
                case "12":
                case "13":
                case "16":
                case "31":
                case "33":
                case "35":
                case "37":
                case "38":
                case "42":
                case "43":
                case "46":
                    listDoNham = listDoNham1;
                    break;
                case "02":
                case "04":
                case "06":
                case "32":
                case "34":
                case "36":
                    listDoNham = listDoNham2;
                    break;
            }
            for (int i = 1; i <= 7; i++)
            {

                ComboBox cboDoNham = this.Controls.Find(String.Format("{0}cbo_DoNham_{1}_{2}", index_Group, i, RowIsLoaded), true).FirstOrDefault() as ComboBox;
                if (cboDoNham != null)
                {
                    displayDataComboboxNoDataBase(cboDoNham, listDoNham);
                }

            }

        }
        private void ConditionShapeFunction(int indexGroup, ComboBox cboShape)
        {
            try
            {
                string valueShape = this.getSelectedTextCombobox(cboShape.SelectedItem.ToString());


                CheckBox chk_1 = this.Controls.Find(String.Format("{0}_chkMaster_1", indexGroup), true).FirstOrDefault() as CheckBox;
                CheckBox chk_2 = this.Controls.Find(String.Format("{0}_chkMaster_2", indexGroup), true).FirstOrDefault() as CheckBox;
                CheckBox chk_3 = this.Controls.Find(String.Format("{0}_chkMaster_3", indexGroup), true).FirstOrDefault() as CheckBox;
                CheckBox chk_4 = this.Controls.Find(String.Format("{0}_chkMaster_4", indexGroup), true).FirstOrDefault() as CheckBox;
                CheckBox chk_5 = this.Controls.Find(String.Format("{0}_chkMaster_5", indexGroup), true).FirstOrDefault() as CheckBox;
                CheckBox chk_6 = this.Controls.Find(String.Format("{0}_chkMaster_6", indexGroup), true).FirstOrDefault() as CheckBox;
                CheckBox chk_7 = this.Controls.Find(String.Format("{0}_chkMaster_7", indexGroup), true).FirstOrDefault() as CheckBox;

                Button btnAdd = this.Controls.Find(String.Format("Button_Add_{0}", indexGroup), true).FirstOrDefault() as Button;

                //====================
                chk_1.Enabled = false;
                chk_2.Enabled = false;
                chk_3.Enabled = false;
                chk_4.Enabled = false;
                chk_5.Enabled = false;
                chk_6.Enabled = false;
                chk_7.Enabled = false;
                if (valueShape.Contains("01:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;

                }
                if (valueShape.Contains("02:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("03:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("04:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("05:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = true;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("06:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("07:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("08:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("12:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("13:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("15:"))
                {
                    chk_1.Checked = false;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = false;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("16:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = true;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("18:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("20:"))
                {
                    chk_1.Checked = false;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = false;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = false;
                    btnAdd.BackColor = SystemColors.Control;
                }
                if (valueShape.Contains("31:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("32:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("33:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("34:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("35:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = true;
                    //====================
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("36:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    //====================
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("37:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("38:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("42:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("43:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("46:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = true;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("48:"))
                {
                    chk_1.Checked = true;
                    chk_2.Checked = true;
                    chk_3.Checked = true;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = true;
                    chk_7.Checked = false;
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
                if (valueShape.Contains("50:"))
                {
                    chk_1.Checked = false;
                    chk_2.Checked = false;
                    chk_3.Checked = false;
                    chk_4.Checked = false;
                    chk_5.Checked = false;
                    chk_6.Checked = false;
                    chk_7.Checked = false;
                    btnAdd.Enabled = true;
                    btnAdd.BackColor = Color.Green;
                }
            }
            catch
            {

            }
        }
        private void cboMaterial_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cboMaterial = (ComboBox)sender;
            string valueMaterial = this.getSelectedTextCombobox(cboMaterial.SelectedItem.ToString());

            string[] strArr = cboMaterial.Name.Split(new[] { "CBO_MATERIAL_" }, System.StringSplitOptions.None);
            string integerDest = strArr[1];
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            int x = 0;
            Int32.TryParse(integerDest, out x);
            //================================================================
            DataTable dt = manuTypeBus.get_Row_WHERE_To_Value_Row("MATERIAL_TO_SHAPE", "MATERIAL_NAME", "SHAPE", valueMaterial, true);
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", x), true).FirstOrDefault() as ComboBox;
            if (cboShape != null)
            {
                this.displayDataToComboBox(cboShape, dt, "SHAPE", "ID");
                cboShape.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboShape.AutoCompleteMode = AutoCompleteMode.Suggest;
            }
        }
        private void cboShape_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                ComboBox cboShape = (ComboBox)sender;
                string[] strArr = cboShape.Name.Split(new[] { "CBO_SHAPE_" }, System.StringSplitOptions.None);
                string integerDest = strArr[1];
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                int indexGroup = 0;
                Int32.TryParse(integerDest, out indexGroup);
                // Load Data for ComboBox Do Nham
                List<string> listDoNham = new List<string>();
                List<string> listDoNham1 = new List<string>() { "0", "1", "2" };
                List<string> listDoNham2 = new List<string>() { "0", "1", "2" };
                switch (splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0))
                {
                    case "01":
                    case "03":
                    case "05":
                    case "07":
                    case "08":
                    case "12":
                    case "13":
                    case "16":
                    case "31":
                    case "33":
                    case "35":
                    case "37":
                    case "38":
                    case "42":
                    case "43":
                    case "46":
                        listDoNham = listDoNham1;
                        break;
                    case "02":
                    case "04":
                    case "06":
                    case "32":
                    case "34":
                    case "36":
                        listDoNham = listDoNham2;
                        break;
                }
                // Delete Row > 1 when Shape <30
                switch (splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0))
                {
                    case "01":
                    case "02":
                    case "03":
                    case "04":
                    case "05":
                    case "06":
                    case "07":
                    case "08":
                    case "09":
                    case "10":
                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                    case "16":
                    case "17":
                    case "18":
                    case "19":
                    case "20":
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                    case "25":
                    case "26":
                    case "27":
                    case "28":
                    case "29":
                        if (rowListPanel[indexGroup] > 1)
                        {
                            for (int k = 1; k <= rowListPanel[indexGroup]; k++)
                            {
                                deleteLastRow_Multil(indexGroup, false);
                            }
                        }
                        break;
                }

                for (int i = 1; i <= rowListPanel[indexGroup]; i++)
                {
                    for (int j = 1; j <= 7; j++)
                    {
                        ComboBox cboDoNham = this.Controls.Find(String.Format("{0}cbo_DoNham_{2}_{1}", indexGroup, i, j), true).FirstOrDefault() as ComboBox;
                        if (cboDoNham != null)
                        {
                            displayDataComboboxNoDataBase(cboDoNham, listDoNham);
                        }
                    }

                }
                /*Reset All TextBox Input and TextBox OutPut*/
                for (int i1 = 1; i1 <= rowListPanel[indexGroup]; i1++)
                {
                    //Input
                    TextBox txtboxInput1 = this.Controls.Find(String.Format("{0}txtBox_1_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxInput2 = this.Controls.Find(String.Format("{0}txtBox_2_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxInput3 = this.Controls.Find(String.Format("{0}txtBox_3_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxInput4 = this.Controls.Find(String.Format("{0}txtBox_4_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxInput5 = this.Controls.Find(String.Format("{0}txtBox_5_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxInput6 = this.Controls.Find(String.Format("{0}txtBox_6_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxInput7 = this.Controls.Find(String.Format("{0}txtBox_7_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    //OutPut
                    TextBox txtboxOutPut1 = this.Controls.Find(String.Format("{0}txtBox_Output_1_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxOutPut2 = this.Controls.Find(String.Format("{0}txtBox_Output_2_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxOutPut3 = this.Controls.Find(String.Format("{0}txtBox_Output_3_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxOutPut4 = this.Controls.Find(String.Format("{0}txtBox_Output_4_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxOutPut5 = this.Controls.Find(String.Format("{0}txtBox_Output_5_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxOutPut6 = this.Controls.Find(String.Format("{0}txtBox_Output_6_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                    TextBox txtboxOutPut7 = this.Controls.Find(String.Format("{0}txtBox_Output_7_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;


                    TextBox txtWEIGH = this.Controls.Find(String.Format("TXT_WEIGH_{0}", indexGroup), true).FirstOrDefault() as TextBox;
                    ComboBox cboGiaCong6 = this.Controls.Find(String.Format("{0}cbo_GiaCong_6_{1}", indexGroup, i1), true).FirstOrDefault() as ComboBox;
                    ComboBox cboGiaCong7 = this.Controls.Find(String.Format("{0}cbo_GiaCong_7_{1}", indexGroup, i1), true).FirstOrDefault() as ComboBox;

                    //txtboxInput1.ResetText();
                    txtboxInput2.ResetText();
                    txtboxInput3.ResetText();
                    txtboxInput4.ResetText();
                    txtboxInput5.ResetText();
                    txtboxInput6.ResetText();
                    txtboxInput7.ResetText();

                    txtboxOutPut1.ResetText();
                    txtboxOutPut2.ResetText();
                    txtboxOutPut3.ResetText();
                    txtboxOutPut4.ResetText();
                    txtboxOutPut5.ResetText();
                    txtboxOutPut6.ResetText();
                    txtboxOutPut7.ResetText();
                    txtWEIGH.ResetText();

                    cboGiaCong6.SelectedIndex = -1;
                    cboGiaCong7.SelectedIndex = -1;
                    cboGiaCong6.Enabled = false;
                    cboGiaCong7.Enabled = false;

                    txtboxInput1.BackColor = !txtboxInput1.ReadOnly ? Color.LightGreen : SystemColors.Control;
                    txtboxInput2.BackColor = !txtboxInput2.ReadOnly ? Color.LightGreen : SystemColors.Control;
                    txtboxInput3.BackColor = !txtboxInput3.ReadOnly ? Color.LightGreen : SystemColors.Control;
                    txtboxInput4.BackColor = !txtboxInput4.ReadOnly ? Color.LightGreen : SystemColors.Control;
                    txtboxInput5.BackColor = !txtboxInput5.ReadOnly ? Color.LightGreen : SystemColors.Control;
                    txtboxInput6.BackColor = !txtboxInput6.ReadOnly ? Color.LightGreen : SystemColors.Control;
                    txtboxInput7.BackColor = !txtboxInput7.ReadOnly ? Color.LightGreen : SystemColors.Control;
                    txtboxInput1.BackColor = !txtboxInput1.ReadOnly ? Color.LightGreen : SystemColors.Control;

                    txtboxOutPut1.BackColor = SystemColors.Control;
                    txtboxOutPut2.BackColor = SystemColors.Control;
                    txtboxOutPut3.BackColor = SystemColors.Control;
                    txtboxOutPut4.BackColor = SystemColors.Control;
                    txtboxOutPut5.BackColor = SystemColors.Control;
                    txtboxOutPut6.BackColor = SystemColors.Control;
                    txtboxOutPut7.BackColor = SystemColors.Control;
                }
                //============Delete data of textBox when disable Checkbox master ============
                for (int k = 1; k <= 7; k++)
                {
                    CheckBox chk_Master = this.Controls.Find(String.Format("{0}_chkMaster_{1}", indexGroup, k), true).FirstOrDefault() as CheckBox;
                    if (chk_Master.Checked == false)
                    {
                        for (int i1 = 1; i1 <= rowListPanel[indexGroup]; i1++)
                        {
                            TextBox txtbox = this.Controls.Find(String.Format("{0}txtBox_1_{1}", indexGroup, i1), true).FirstOrDefault() as TextBox;
                            txtbox.Clear();
                        }

                    }
                }
                //================================================================
                this.ConditionShapeFunction(indexGroup, cboShape);
                //================================================================
                ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", indexGroup), true).FirstOrDefault() as ComboBox;

                string valueMat = this.getSelectedTextCombobox(cboMaterial.SelectedItem.ToString());
                string valuesShape = this.getSelectedTextCombobox(cboShape.SelectedItem.ToString());
                string[] strArr1 = valueMat.Split(new[] { ":" }, System.StringSplitOptions.None);
                string[] strArr2 = valuesShape.Split(new[] { ":" }, System.StringSplitOptions.None);
                string text = strArr1[0] + "_" + strArr2[0];
                DataTable dt = manuTypeBus.get_Row_WHERE_To_Value_Row("SIZE1", "MATERIAL_TO_SHAPE", "CODELOGIC", text, true);
                string[] array = dt.Rows.OfType<DataRow>().Select(k => k[2].ToString()).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ChangeComboBoxConditionRequest(int indexGroup, int indexRow,TextBox textBoxFocus,string comboboxStringFrom,double valueCompare,TypeCompare typeCompare,
            string comboboxStringTo,string notContainsStringFrom = null,
            string notContiansStringTo = null)
        {
            ComboBox cboShape = this.Controls.Find(String.Format("CBO_SHAPE_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            string cboShape_StrIndex = splitString(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboShape)), ":", 0);
            cboShape_StrIndex = cboShape_StrIndex.Replace("32", "02").Replace("36", "06");
            ComboBox cboDoNham = this.Controls.Find(String.Format("{0}cbo_DoNham_1_{1}", indexGroup, indexRow), true).FirstOrDefault() as ComboBox;
            ComboBox cboMaterial = this.Controls.Find(String.Format("CBO_MATERIAL_{0}", indexGroup), true).FirstOrDefault() as ComboBox;
            string cboMaterial_Raw = getSelectedTextCombobox(getSelectedItemComboboxTxt(cboMaterial));
            double valueDoNham = convertStringToDouble(getSelectedTextCombobox(getSelectedItemComboboxTxt(cboDoNham)));
            
            double valueSizeA = -1;
            if (valueDoNham > 0)
            {
                valueSizeA = ConvertStringToDouble_And_MathRoundRule(textBoxFocus.Text);
            }
            else
            {
                valueSizeA = convertStringToDouble(textBoxFocus.Text);
            }
            // Determine
            bool inputCompare = false;
            bool notContains = false;
            if (typeCompare == TypeCompare.BIGGER)
            {
                inputCompare = (valueSizeA > valueCompare);
            }

            if (typeCompare == TypeCompare.BIGGER_OR_AS)
            {
                inputCompare = (valueSizeA >= valueCompare);
            }

            if (typeCompare == TypeCompare.AS_TO_AS)
            {
                inputCompare = (valueSizeA == valueCompare);
            }

            if (typeCompare == TypeCompare.SMALLER)
            {
                inputCompare = (valueSizeA < valueCompare);
            }

            if (typeCompare == TypeCompare.SMALLER_OR_AS)
            {
                inputCompare = (valueSizeA <= valueCompare);
            }

            /*Xét trường hợp để hoán đổi*/
            if(notContainsStringFrom != null)
            {
                notContains = !cboMaterial_Raw.Contains(notContainsStringFrom);
            }
            else
            {
                notContains = true;
            }
            

            if(cboShape_StrIndex.Equals("02") || cboShape_StrIndex.Equals("06") || ( cboMaterial_Raw.Contains("SS400-D") && !cboMaterial_Raw.Contains("9")))
            {
                if (inputCompare)
                {
                    if (cboMaterial_Raw.Contains(comboboxStringFrom) && notContains)
                    {
                        if (MessageBox.Show(this, String.Format("Bạn muốn thay đổi từ vật liệu {0} sang vật liệu {1} không ?? ", comboboxStringFrom, comboboxStringTo), "Lưu ý", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (notContainsStringFrom != null)
                            {
                                DataTable dt = manuTypeBus.getDataTable_From_RowName("MATERIAL_TO_SHAPE", "MATERIAL_NAME");
                                int index = this.getIndexFromStringSearchInDataBase(dt, "MATERIAL_NAME", comboboxStringTo, notContiansStringTo);
                                cboMaterial.SelectedIndex = index;
                                cboShape.SelectedIndex = -1;
                            }
                        }

                    }
                }
            }
            
        }

        private void Funtion_All_Event_TextBox_txtBox_Leave_Multil(object sender, EventArgs e)
        {
            TextBox txtBox  = (TextBox)sender;
            int indexGroup  = this.convertStringToInt(this.splitString(txtBox.Name, "txtBox_1_", 0));
            int indexRow    = this.convertStringToInt(this.splitString(txtBox.Name, "txtBox_1_", 1));

            /*Xét trường hợp để hoán đổi*/
            //Thay dổi từ vật liệu SS400 – D sang SS400 trong trường hợp kích thước độ rộng lớn hơn 400mm.
            this.ChangeComboBoxConditionRequest(indexGroup, indexRow, txtBox, "SS400-D", 400, TypeCompare.BIGGER, "SS400", "9", "-");
            this.ChangeComboBoxConditionRequest(indexGroup, indexRow, txtBox, "SUS303", 45, TypeCompare.SMALLER_OR_AS, "SUS303-D9", "-", null);
            this.ChangeComboBoxConditionRequest(indexGroup, indexRow, txtBox, "SUS304", 45, TypeCompare.SMALLER_OR_AS, "SUS304-D9", "-", null);
            this.ChangeComboBoxConditionRequest(indexGroup, indexRow, txtBox, "SS400", 24, TypeCompare.SMALLER_OR_AS, "SS400-D9", "-", null);
            this.ChangeComboBoxConditionRequest(indexGroup, indexRow, txtBox, "S45C", 24, TypeCompare.SMALLER_OR_AS, "S45C-D9", "-", null);
            this.ChangeComboBoxConditionRequest(indexGroup, indexRow, txtBox, "A5056", 150, TypeCompare.SMALLER_OR_AS, "A5052", null, null);
            this.ChangeComboBoxConditionRequest(indexGroup, indexRow, txtBox, "A5052", 160, TypeCompare.SMALLER_OR_AS, "A5056", null, null);
        }

    }
}
