using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Kyoto.BUS;
using Kyoto.DAL;
using Kyoto.DTO;

namespace Kyoto.GUI
{
    public partial class frmOperation : Form
    {
        private const string SURFACE_MESSAGE = "割り増し計算が必要なため忘れないこと！";
        private const string SURFACE_MESSAGE_VN = "Không được quên thời gian bù！";
        private const string SURFACE_MESSAGE2 = "「{0}」があるから、手仕上げ3分を加算してください！";
        private const string SURFACE_MESSAGE2_VN = "Vì có「{0}」 nên hãy tính thêm thời gian Teshiage là 3 phút!！";
        private const string NAME_EXPORT_P_1ST = "[IMPORT_1]";
        private const string SPACE = " ";
        private const string WARNING_BAFU_LINK_MASKTEXTBOX_30 = "Chú ý BAFU >= 30";
        //private BUS_DRAWING drawingBUS;

        public int inputInfoId { get; set; }
        public string matFileName { get; set; }
        public string partFileName { get; set; }

        private bool bIsFormLoaded = false;

        private List<string> surfaceSpecialList = new List<string> { "052", "140", "079", "055", "029", "056", "059", "130", "077" };
        private List<string> surfaceSpecialMessageList = new List<string> { "001", "007", "008", "071", "111", "112", "114", "115", "132", "140" };
        private List<string> surfaceSpecialMessageList2 = new List<string> { "052", "059", "056", "029", "079", "130", "140", "055", "077" };

        private List<ComboBox> surefaceComboBoxList;
        private DataTable dtSurefaceProcess;

        //private List<string> surefaceComboBoxNameList = new List<string> { "cboSurfaceProcess1", "cboSurfaceProcess2", "cboSurfaceProcess3", "cboSurfaceProcess4", "cboSurfaceProcess5", "cboSurfaceProcess6", "cboSurfaceProcess7", "cboSurfaceProcess8", "cboSurfaceProcess9", "cboSurfaceProcess10" };
        List<Boolean> grvDrawingListDataCheckBox = new List<Boolean>();
        List<Boolean> grvDrawingListDataPinkColor = new List<Boolean>();
        TextBox columnTextBox;

        private bool bIsAllControlAreValidBeforeSaving = true;
        bool flagShape1 = false;
        bool flagShape2 = true;

        public frmOperation()
        {
            InitializeComponent();
            //drawingBUS = new BUS_DRAWING();

        }

        private void frmOperation_Load(object sender, EventArgs e)
        {
            bIsFormLoaded = false;
            surefaceComboBoxList = new List<ComboBox> { cboSurfaceProcess1, cboSurfaceProcess2, cboSurfaceProcess3, cboSurfaceProcess4, cboSurfaceProcess5, cboSurfaceProcess6, cboSurfaceProcess7, cboSurfaceProcess8, cboSurfaceProcess9, cboSurfaceProcess10 };
            this.LoadAllMasterDataToComboBox();
            this.LoadDataGridCombobox();
            btnSave.Enabled = false;
            bIsFormLoaded = true;
            grvMaterial.CurrentCellDirtyStateChanged += new EventHandler(grvMaterial1_CurrentCellDirtyStateChanged);
            //MessageBox.Show(CommonsVars.CURRENT_LANGUAGE);
            if (CommonsVars.CURRENT_LANGUAGE == "vi")
            {
                Delete.Text = "Xóa";
                btnColumn.Text = "Xóa";
            }
            //=========================================================
            for (int i = 0; i < 1000; i++)
            {
                grvDrawingListDataCheckBox.Add(false);
                grvDrawingListDataPinkColor.Add(false);
            }

            //=========================================================
        }

        private void LoadDataGridCombobox()
        {
            this.LoadMaterDivgrvCombobox();
            this.LoadgrvcboShapegrvCombobox();
            this.LoadgrvPhaseCombobox();
            this.LoadDrawingUserComboBox();
            this.LoadDrawingUser_CHECK_ComboBox();
        }

        private void LoadAllMasterDataToComboBox()
        {
            this.LoadManufactorDivisionComboBox();
            this.LoadManufactorTypeCombobox();
            this.LoadStandardRankCombobox();
            this.LoadGradeTypeCombobox();
            this.LoadSurfaceProCombobox();
            this.LoadManuInforRankCombobox();
            this.LoadPantDIvisionCombobox();
        }

        private void processEnterKey(KeyEventArgs e, CheckBox chkCurrent, MaskedTextBox txtCurrent)
        {
            if (e.KeyCode == Keys.Enter)
            {
                List<Control[]> manuControlList = new List<Control[]> {
                new Control[] { chkManuFries, txtManuFriesTime },
                new Control[] { chkManuDrill, txtManuDrillTime },
                new Control[] { chkManuLathe, txtManuLatheTime },
                new Control[] { chkManuTarepan, txtManuTarepanTime },
                new Control[] { chkManuBend, txtManuBendTime },
                new Control[] { chkManuWeld, txtManuWeldTime },
                new Control[] { chkManuOther, txtManuOtherTime },
                new Control[] { chkManuMC, txtManuMCTime },
                new Control[] { chkManuPolish, txtManuPolishTime },
                new Control[] { chkManuAdhesion, txtManuAdhesionTime }
            };

                for (int i = 0; i < manuControlList.Count; i++)
                {
                    if (((CheckBox)manuControlList[i][0]).Name.Equals(chkCurrent.Name))
                    {
                        bool bFound = false;
                        for (int j = i + 1; j < manuControlList.Count; j++)
                        {
                            if (((CheckBox)manuControlList[j][0]).Checked)
                            {
                                bFound = true;
                                ((MaskedTextBox)manuControlList[j][1]).Focus();
                                break;
                            }
                        }
                        if (!bFound)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (((CheckBox)manuControlList[j][0]).Checked)
                                {
                                    ((MaskedTextBox)manuControlList[j][1]).Focus();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void LoadManufactorDivisionComboBox()
        {
            BUS_ManufactorDivision manDivBus = new BUS_ManufactorDivision();
            DataTable dt = manDivBus.getManufactorDivisionList();
            this.displayDataToComboBox(cboManuDivision, dt, "MANU_NOTATION", "MANU_CODE");
        }

        private void LoadDrawingUserComboBox()
        {
            if (CommonsVars.USER_ROLE == Constants.USER_ROLE_MANAGER || CommonsVars.USER_ROLE == Constants.USER_ROLE_LEADER)
            {
                BUS_User userBus = new BUS_User();
                DataTable dt = userBus.getUserList(false);
                this.DisplayDataToGrvCombobox(grvDrawingListUserComboBox, dt, "USER_SHORTNAME", "USER_ID");
            }
        }
        //=====================================
        private void LoadDrawingUser_CHECK_ComboBox()
        {
            if (CommonsVars.USER_ROLE == Constants.USER_ROLE_MANAGER || CommonsVars.USER_ROLE == Constants.USER_ROLE_LEADER)
            {
                BUS_User userBus = new BUS_User();
                DataTable dt = userBus.getUserList(false);
                this.DisplayDataToGrvCombobox(userCheck, dt, "USER_SHORTNAME", "USER_ID");
            }
        }

        private void LoadManufactorTypeCombobox()
        {
            BUS_ManufactorType manuTypeBus = new BUS_ManufactorType();
            DataTable dt = manuTypeBus.getManufactorTypeList();
            //this.displayDataToComboBox(cboManufactorType, dt, "MANU_TYPE_NOTATION", "MANU_TYPE_CODE","Add_Blank");
            this.displayDataToComboBox(cboManufactorType, dt, "MANU_TYPE_NOTATION", "MANU_TYPE_CODE");
        }
        private void LoadStandardRankCombobox()
        {
            BUS_StandardRank manuTypeBus = new BUS_StandardRank();
            DataTable dt = manuTypeBus.getStandardRankList();
            //this.displayDataToComboBox(cboStandardRank, dt, "STD_NOTATION", "STD_CODE", "Add_Blank");
            this.displayDataToComboBox(cboStandardRank, dt, "STD_NOTATION", "STD_CODE");
        }

        private void LoadGradeTypeCombobox()
        {
            BUS_GradeType gradeBUS = new BUS_GradeType();
            DataTable dt = gradeBUS.getGradeTypeList();
            //this.displayDataToComboBox(cboGradeType, dt, "GRADE_NOTATION", "GRADE_CODE", "Add_Blank");
            this.displayDataToComboBox(cboGradeType, dt, "GRADE_NOTATION", "GRADE_CODE");
        }
        private void LoadSurfaceProCombobox()
        {
            BUS_SurfaceProcess sur_proBUS = new BUS_SurfaceProcess();
            dtSurefaceProcess = sur_proBUS.getSurfaceProcessList();

            //Insert the Default Item to DataTable.
            // Duy Khanh add blank items to cboSurfaceProcessX 1 times when add "add_one_item_blank" to function displayDataToComboBox
            this.displayDataToComboBox(cboSurfaceProcess1, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE", "Add_Blank");
            //this.displayDataToComboBox(cboSurfaceProcess1, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");
            this.displayDataToComboBox(cboSurfaceProcess2, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");
            this.displayDataToComboBox(cboSurfaceProcess3, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");
            this.displayDataToComboBox(cboSurfaceProcess4, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");
            this.displayDataToComboBox(cboSurfaceProcess5, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");
            this.displayDataToComboBox(cboSurfaceProcess6, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");
            this.displayDataToComboBox(cboSurfaceProcess7, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");
            this.displayDataToComboBox(cboSurfaceProcess8, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");
            this.displayDataToComboBox(cboSurfaceProcess9, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");
            this.displayDataToComboBox(cboSurfaceProcess10, dtSurefaceProcess, "SUR_PRO_NOTATION", "SUR_PRO_CODE");

        }

        private void LoadManuInforRankCombobox()
        {
            BUS_ManuInforRank inforrankBUS = new BUS_ManuInforRank();
            DataTable dt = inforrankBUS.getManuInforRankList();
            this.displayDataToComboBox(cboManuFriesRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
            this.displayDataToComboBox(cboManuDrillRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
            this.displayDataToComboBox(cboManuLatheRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
            this.displayDataToComboBox(cboManuTarepanRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
            this.displayDataToComboBox(cboManuBendRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
            this.displayDataToComboBox(cboManuWeldRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
            this.displayDataToComboBox(cboManuOtherRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
            this.displayDataToComboBox(cboManuMCRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
            this.displayDataToComboBox(cboManuPolishRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
            this.displayDataToComboBox(cboManuAdhesionRank, dt, "MANU_INFO_NOTATION", "MANU_INFO_CODE");
        }

        private void LoadPantDIvisionCombobox()
        {
            BUS_PaintDivision paintBUS = new BUS_PaintDivision();
            DataTable dt = paintBUS.getPaintDivisionList();
            this.displayDataToComboBox(cboPaintDivision1, dt, "PAINT_DIV_NOTATION", "PAINT_DIV_CODE");
            this.displayDataToComboBox(cboPaintDivision2, dt, "PAINT_DIV_NOTATION", "PAINT_DIV_CODE");
        }


        private void LoadMaterDivgrvCombobox()
        {
            BUS_MaterialDivision masterdivtBUS = new BUS_MaterialDivision();
            DataTable dt = masterdivtBUS.getManuInforRankList();
            this.DisplayDataToGrvCombobox(grvcboMaterial, dt, "MAT_NOTATION", "MAT_DIV_CODE", "Add_Blank");
        }

        //private void LoadgrvcboShapegrvCombobox(DataGridViewComboBoxCell grvComboCell, string materialCode)
        //{                            
        //    BUS_ShapeDivision shapeBUS = new BUS_ShapeDivision();
        //    DataTable dt = shapeBUS.getShapeListByMaterialCode(materialCode);
        //    this.DisplayDataToGrvCombobox(grvComboCell, dt, "SHAPE_NOTATION", "SHAPE_DIV_CODE");                            
        //}
        private void LoadgrvcboShapegrvCombobox()
        {
            BUS_ShapeDivision shapeBUS = new BUS_ShapeDivision();
            DataTable dt = shapeBUS.getShapeListVietnamese();
            this.DisplayDataToGrvCombobox(grvcboShape, dt, "SHAPE_NOTATION", "SHAPE_DIV_CODE", "Add_Blank");
        }
        private void LoadgrvPhaseCombobox()
        {
            BUS_PhaseType phaseBUS = new BUS_PhaseType();
            DataTable dt = phaseBUS.getPhaseList();
            this.DisplayDataToGrvCombobox(grvcboPhase, dt, "PHASE_NOTATION", "PHASE_CODE");
        }

        private void cmbDrawingNoProgress_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void displayDataToComboBox(ComboBox cboBox, DataTable dt, string displayColumn, string valueColumn, string addBlankItem = "No")
        {
            cboBox.DataSource = null;
            //cboBox.Items.Clear();
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

        private void DisplayDataToGrvCombobox(DataGridViewComboBoxColumn grvcombobox, DataTable dt, string varColumnNameDisplay, string varColumnNameValue, string addBlankItem = "No")
        {
            grvcombobox.DataSource = null;
            grvcombobox.Items.Clear();
            List<object> items = new List<object>();
            grvcombobox.DisplayMember = "Text";
            grvcombobox.ValueMember = "Value";
            grvcombobox.DataPropertyName = "Value";
            //======================================
            // Add blank to combobox when addBlankItem contains Yes
            addBlankItem = addBlankItem.ToLower();
            if (addBlankItem.Contains("add_blank"))
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
            }
            // End
            //======================================
            foreach (DataRow row in dt.Rows)
            {
                items.Add(new { Text = row[varColumnNameDisplay], Value = row[varColumnNameValue] });
            }
            grvcombobox.DataSource = items;
        }


        // KhanhPD add new function to disable some items 
        private void displayDataToComboBoxAndRemoveItems(ComboBox cboBox, DataTable dt, string displayColumn, string valueColumn, List<string> itemsRemove, string addBlankItem = "No")
        {
            cboBox.DataSource = null;
            //cboBox.Items.Clear();
            List<object> items = new List<object>();
            cboBox.DisplayMember = "Text";
            cboBox.ValueMember = "Value";
            List<string> method1 = new List<string>();
            List<string> method2 = new List<string>(); ;
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
            //================================
            for (int k = 0; k < method1.Count; k++)
            {
                bool check = true;
                for (int i = 0; i < itemsRemove.Count; i++)
                {

                    if ((method2[k].Contains(itemsRemove[i])))
                    {
                        check = false;
                        break;
                    }
                }
                if (check == true)
                {
                    items.Add(new { Text = method1[k], Value = method2[k] });
                }

            }
            cboBox.DataSource = items;
            cboBox.SelectedIndex = -1;
        }

        // Duy Khanh add new Function to add and remove some items in combobox GridView

        private void DisplayDataToGrvComboboxAndRemoveSomeItems(DataGridViewComboBoxColumn grvcombobox, DataTable dt, string varColumnNameDisplay, string varColumnNameValue, List<string> itemsRemove, string addBlankItem = "No")
        {
            try
            {
                grvcombobox.DataSource = null;
                //grvcombobox.Items.Clear();
                List<object> items = new List<object>();
                grvcombobox.DisplayMember = "Text";
                grvcombobox.ValueMember = "Value";
                grvcombobox.DataPropertyName = "Value";
                //======================================
                List<string> method1 = new List<string>();
                List<string> method2 = new List<string>(); ;
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
                    method1.Add(row[varColumnNameDisplay].ToString());
                    method2.Add(row[varColumnNameValue].ToString());
                }
                //================================
                for (int k = 0; k < method1.Count; k++)
                {
                    bool check = true;
                    for (int i = 0; i < itemsRemove.Count; i++)
                    {

                        if ((method2[k].Contains(itemsRemove[i])))
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check == true)
                    {
                        items.Add(new { Text = method1[k], Value = method2[k] });
                    }

                }
                grvcombobox.DataSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error: " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisplayDataToGrvCombobox(DataGridViewComboBoxCell grvcombobox, DataTable dt, string varColumnNameDisplay, string varColumnNameValue)
        {
            grvcombobox.DataSource = null;
            grvcombobox.Items.Clear();
            List<object> items = new List<object>();
            grvcombobox.DisplayMember = "Text";
            grvcombobox.ValueMember = "Value";
            //items.Add(new { Text = "", Value = "" });
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new { Text = row[varColumnNameDisplay], Value = row[varColumnNameValue] });
                }
                grvcombobox.DataSource = items;
            }
        }

        private void btnSelectInputInfo_Click(object sender, EventArgs e)
        {
            frmSelectInputInfo frmSelect = new frmSelectInputInfo();
            frmSelect.ShowDialog(this);
            this.AllActionWithGridViewData(); // Duy Khanh Add code
            this.SetComboboxRankStandard();
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess1, false);
            //===============================================================
            txtMessageGRVMATERIAL.Text = "Làm ơn check lại Mã Vật Liệu";
            txtMessageGRVMATERIAL.BackColor = Color.Orange;
        }

        public void LoadDrawingListOfInputData(int inputInfoId)
        {
            txtMaterialFileName.Text = this.matFileName.Replace("IN", "RE");
            txtPartFileName.Text = this.partFileName.Replace("IN", "RE");

            this.resetInputItems();

            this.reloadDrawingList(inputInfoId);
        }

        private void reloadDrawingList(int inputInfoId)
        {
            try
            {
                BUS_DrawingInfor drawingBUS = new BUS_DrawingInfor();

                DataTable dt = drawingBUS.getDrawingInfoByInputID(this.inputInfoId);

                grvDrawingList.RowCount = 0;
                int i = 1;
                foreach (DataRow row in dt.Rows)
                {
                    DataGridViewRow gridRow = new DataGridViewRow();
                    gridRow.CreateCells(grvDrawingList);

                    gridRow.Cells[0].Value = i.ToString();
                    gridRow.Cells[1].Value = row["PART_CODE"].ToString();
                    if (Constants.PROGRESS_DONE.Equals(row["PROGRESS_STATUS"].ToString()))
                    {
                        gridRow.Cells[2].Value = Constants.PROGRESS_DONE;
                        gridRow.Cells[2].Style.BackColor = Color.Yellow;
                    }
                    else if (Constants.PROGRESS_CHECKED.Equals(row["PROGRESS_STATUS"].ToString()))
                    {
                        gridRow.Cells[2].Value = Constants.PROGRESS_CHECKED;
                        gridRow.Cells[2].Style.BackColor = Color.White;
                    }
                    else if (Constants.PROGRESS_DOING.Equals(row["PROGRESS_STATUS"].ToString()))
                    {
                        gridRow.Cells[2].Value = Constants.PROGRESS_DOING;
                        gridRow.Cells[2].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        gridRow.Cells[2].Value = Constants.PROGRESS_NOT_STARTED;
                        gridRow.Cells[2].Style.BackColor = Color.Gray;
                    }
                    gridRow.Cells[3].Value = row["USER_ID"].ToString();
                    gridRow.Cells[5].Value = row["USER_ID_CHECK"].ToString();

                    grvDrawingList.Rows.Add(gridRow);
                    i++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error LoadDrawingListOfInputData(): " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangeSaveButtonStatus(bool bEnable)
        {
            if ((bIsFormLoaded) && (!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())) && (grvDrawingList.CurrentRow != null) && (grvDrawingList.CurrentRow.Index >= 0))
            {
                btnSave.Enabled = bEnable;
                //if (CommonsVars.LOGIN_USER_ID.Equals(grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex].Value.ToString()))
                //{
                //    btnSave.Enabled = bEnable;
                //}
                //else if (CommonsVars.USER_ROLE <= Constants.USER_ROLE_OPERATOR)
                //{
                //    MessageBox.Show(this, "Drawing [" + grvDrawingList[1, grvDrawingList.CurrentRow.Index].Value.ToString() + " ] is not your task! Please contact your Leader/Sub Leader.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
                //else
                //{
                //    MessageBox.Show(this, "Drawing [" + grvDrawingList[1, grvDrawingList.CurrentRow.Index].Value.ToString() + " ] is not your task! Please see the assignment list.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
                //Yêu cầu từ Mr Khả
            }
            else
            {
                btnSave.Enabled = false;
            }

        }

        private void highlightPairTextBox(ComboBox cboHighlight, MaskedTextBox txtHighlight, CheckBox chkCheck)
        {
            if ((!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())) && (cboHighlight.SelectedIndex >= 0))
            {
                txtHighlight.BackColor = (int.Parse("0" + txtHighlight.Text.Trim()) == 0) ? Color.Yellow : Color.White;
                cboHighlight.BackColor = Color.White;
                chkCheck.Checked = true;
            }
            else if (!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim()))
            {
                txtHighlight.BackColor = chkCheck.Checked ? Color.Yellow : Color.White;
                cboHighlight.BackColor = chkCheck.Checked ? Color.Yellow : Color.White;
            }
            else
            {
                txtHighlight.BackColor = Color.White;
            }
        }

        private void highlightPairTextBox(ComboBox cboHighlight, MaskedTextBox txtHighlight, MaskedTextBox txtHighlight2, bool bIsMoneyItem = false)
        {
            if (!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim()))
            {
                txtHighlight2.Text = bIsMoneyItem ? txtHighlight2.Text : Constants.EMPTY_STRING;
                if (cboHighlight.SelectedIndex >= 0)
                {
                    txtHighlight.BackColor = (int.Parse("0" + txtHighlight.Text.Trim()) == 0) ? Color.Yellow : Color.White;
                    txtHighlight2.BackColor = (bIsMoneyItem) && (int.Parse("0" + txtHighlight2.Text.Trim()) == 0) ? Color.YellowGreen : Color.White;
                    cboHighlight.BackColor = Color.White;
                }
                else
                {
                    cboHighlight.BackColor = ((int.Parse("0" + txtHighlight.Text.Trim()) != 0) || (int.Parse("0" + txtHighlight2.Text.Trim()) != 0)) ? Color.Yellow : Color.White;
                    txtHighlight.BackColor = (int.Parse("0" + txtHighlight2.Text.Trim()) != 0) ? Color.Yellow : Color.White;
                    txtHighlight2.BackColor = (bIsMoneyItem) && (int.Parse("0" + txtHighlight.Text.Trim()) != 0) ? Color.YellowGreen : Color.White;
                }
            }
            else
            {
                cboHighlight.BackColor = Color.White;
                txtHighlight.BackColor = Color.White;
                txtHighlight2.BackColor = Color.White;
            }
        }

        private void highlightPairTextBox(ComboBox cboHighlight, MaskedTextBox txtHighlight)
        {
            if ((!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())) && (cboHighlight.SelectedIndex >= 0))
            {
                txtHighlight.BackColor = (int.Parse("0" + txtHighlight.Text.Trim()) == 0) ? Color.Yellow : Color.White;
                cboHighlight.BackColor = Color.White;
            }
            else
            {
                txtHighlight.BackColor = Color.White;
            }
        }

        private void highlightPaintPairTextBox(ComboBox cboHighlight, MaskedTextBox txtHighlight)
        {
            if ((!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())) && ("1".Equals(cboHighlight.SelectedValue + "")))
            {
                txtHighlight.BackColor = (int.Parse("0" + txtHighlight.Text.Trim()) == 0) ? Color.Yellow : Color.White;
                cboHighlight.BackColor = Color.White;
            }
            else
            {
                txtHighlight.BackColor = Color.White;
            }
        }

        private void highlightPairComboBox(ComboBox cboHighlight, MaskedTextBox txtHighlight)
        {
            if ((!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())) && (int.Parse("0" + txtHighlight.Text.Trim()) != 0))
            {
                cboHighlight.BackColor = cboHighlight.SelectedIndex < 0 ? Color.Yellow : Color.White;
                txtHighlight.BackColor = Color.White;
            }
            else
            {
                cboHighlight.BackColor = Color.White;
            }
        }

        private void highlightPairComboBox(ComboBox cboHighlight, MaskedTextBox txtHighlight, MaskedTextBox txtHighlight2)
        {
            if ((!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())))
            {
                if (int.Parse("0" + txtHighlight.Text.Trim()) != 0)
                {
                    cboHighlight.BackColor = cboHighlight.SelectedIndex < 0 ? Color.Yellow : Color.White;
                    txtHighlight2.BackColor = (int.Parse("0" + txtHighlight2.Text.Trim()) == 0) ? Color.Yellow : Color.White;
                    txtHighlight.BackColor = Color.White;
                }
                else
                {
                    txtHighlight.BackColor = ((cboHighlight.SelectedIndex >= 0) || (int.Parse("0" + txtHighlight2.Text.Trim()) != 0)) ? Color.Yellow : Color.White;
                    cboHighlight.BackColor = (int.Parse("0" + txtHighlight2.Text.Trim()) != 0) ? Color.Yellow : Color.White;
                    txtHighlight2.BackColor = ((cboHighlight.SelectedIndex >= 0) || (int.Parse("0" + txtHighlight.Text.Trim()) != 0)) ? Color.Yellow : Color.White;
                }
            }
            else
            {
                cboHighlight.BackColor = Color.White;
                txtHighlight.BackColor = Color.White;
                txtHighlight2.BackColor = Color.White;
            }
        }

        private void highlightPairComboBox(ComboBox cboHighlight, MaskedTextBox txtHighlight, CheckBox chkCheck)
        {
            if ((!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())) && (int.Parse("0" + txtHighlight.Text.Trim()) != 0))
            {
                cboHighlight.BackColor = cboHighlight.SelectedIndex < 0 ? Color.Yellow : Color.White;
                txtHighlight.BackColor = Color.White;
                chkCheck.Checked = true;
            }
            else if (!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim()))
            {
                cboHighlight.BackColor = chkCheck.Checked ? Color.Yellow : Color.White;
                txtHighlight.BackColor = chkCheck.Checked ? Color.Yellow : Color.White;
            }
            else
            {
                cboHighlight.BackColor = Color.White;
                txtHighlight.BackColor = Color.White;
            }
        }

        private void highlightPairTextBoxvsTextBox(MaskedTextBox txtHighlight1, MaskedTextBox txtHighlight2)
        {
            if ((bIsFormLoaded) && (int.Parse("0" + txtHighlight1.Text.Trim()) != 0))
            {
                txtHighlight2.BackColor = (int.Parse("0" + txtHighlight2.Text.Trim()) == 0) ? Color.Yellow : Color.White;
                txtHighlight1.BackColor = Color.White;
            }
            else
            {
                txtHighlight2.BackColor = Color.White;
            }
        }

        #region Change event
        bool flag = false;
        private void cboSurfaceProcess1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();

            flag = true;

            highlightPairTextBoxNew(cboSurfaceProcess1, txtSurefaceWeight1);

        }

        private void cboSurfaceProcess2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();
            flag = true;
            highlightPairTextBoxNew(cboSurfaceProcess2, txtSurefaceWeight2);
        }

        private void cboSurfaceProcess3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();
            flag = true;
            highlightPairTextBoxNew(cboSurfaceProcess3, txtSurefaceWeight3);
        }

        private void cboSurfaceProcess4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();
            flag = true;
            highlightPairTextBoxNew(cboSurfaceProcess4, txtSurefaceWeight4);
        }

        private void cboSurfaceProcess5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();
            flag = true;
            highlightPairTextBoxNew(cboSurfaceProcess5, txtSurefaceWeight5);
        }

        private void cboSurfaceProcess6_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();
            flag = true;
            highlightPairTextBoxNew(cboSurfaceProcess6, txtSurefaceWeight6);
        }

        private void cboSurfaceProcess7_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();
            flag = true;
            highlightPairTextBoxNew(cboSurfaceProcess7, txtSurefaceWeight7);

        }

        private void cboSurfaceProcess8_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();
            flag = true;
            highlightPairTextBoxNew(cboSurfaceProcess8, txtSurefaceWeight8);
        }

        private void cboSurfaceProcess9_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();
            flag = true;
            highlightPairTextBoxNew(cboSurfaceProcess9, txtSurefaceWeight9);
        }

        private void cboSurfaceProcess10_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
            //fillSurefaceProcessAuto();
            this.ExecuteCondition();
            highlightPairTextBoxNew(cboSurfaceProcess10, txtSurefaceWeight10);
        }

        private void txtSurefaceQuantity1_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceQuantity2_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceQuantity3_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceQuantity4_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceQuantity5_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceQuantity6_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceQuantity7_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceQuantity8_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceQuantity9_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceQuantity10_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney1_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney2_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney3_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney4_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney5_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney6_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney7_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney8_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney9_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtSurefaceMoney10_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManufactorType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboStandardRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);

            // Require from Mr NamNV

            this.SetComboboxRankStandard();
        }

        private void cboGradeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuFriesTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuFriesRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuDrillTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuDrillRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuLatheTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuLatheRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuTarepanTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuTarepanRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuBendTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuBendRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuWeldTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuWeldRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuOtherTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuOtherRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuMCTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuMCRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuPolishTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuPolishRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuAdhesionTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboManuAdhesionRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuToothCuttingCost_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuQuenchingCost_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuPreparationTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuPreparationStamp_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuPreparationColorCheck_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuKeyCost_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuTapeTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuWoodCost_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuEvidenceCost_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtManuScrewTime_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboPaintDivision1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtPaintSquare1_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void cboPaintDivision2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        private void txtPaintSquare2_TextChanged(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }
        #endregion Change event

        private void btnReloadDrawingStatus_Click(object sender, EventArgs e)
        {
            if (btnSave.Enabled)
            {
                if ((MessageBox.Show(this, "Data has been changed, do you want to save before reloading status?", CommonsVars.APP_NAME, MessageBoxButtons.YesNoCancel) == DialogResult.Yes))
                {
                    this.saveCurrentData();
                    completeDrawingData(Constants.PROGRESS_DOING);
                }
            }

            this.reloadDrawingList(this.inputInfoId);
        }

        private void saveCurrentData()
        {
            BUS_MaterialEst materialEstBUS = new BUS_MaterialEst();
            BUS_PartEst partEstBUS = new BUS_PartEst();
            clearEmptyBuyingMaterials();
            List<DTO_MasterialEst> materialList = new List<DTO_MasterialEst>();
            int id = 1;

            foreach (DataGridViewRow row in grvMaterial.Rows)
            {
                DTO_MasterialEst materialEst = new DTO_MasterialEst(this.inputInfoId, txtDrawingCode.Text, id.ToString(), row.Cells[1].Value + "", row.Cells[2].Value + "", cboManuDivision.SelectedValue + "", row.Cells[3].Value + "",
                    row.Cells[4].Value + "", row.Cells[5].Value + "", row.Cells[6].Value + "", row.Cells[7].Value + "", row.Cells[8].Value + "", row.Cells[9].Value + "", row.Cells[10].Value + "", row.Cells[11].Value + "");
                materialList.Add(materialEst);
                id++;
                if (grvMaterial.Rows.Count == id) break;
            }
            //Figbug 181218
            #region
            string txtPaintDivision1 = "";
            string txtPaintDivision2 = "";
            {

                if (cboPaintDivision1.SelectedIndex == 0) txtPaintDivision1 = "1";
                if (cboPaintDivision2.SelectedIndex == 0) txtPaintDivision1 = "1";

            }
            #endregion

            List<DTO_PartEst> partEstList = new List<DTO_PartEst>();
            DTO_PartEst partEst = new DTO_PartEst(this.inputInfoId, txtDrawingCode.Text,
                cboSurfaceProcess1.SelectedValue + "", txtSurefaceWeight1.Text, txtSurefaceMoney1.Text,
                cboSurfaceProcess2.SelectedValue + "", txtSurefaceWeight2.Text, txtSurefaceMoney2.Text,
                cboSurfaceProcess3.SelectedValue + "", txtSurefaceWeight3.Text, txtSurefaceMoney3.Text,
                cboSurfaceProcess4.SelectedValue + "", txtSurefaceWeight4.Text, txtSurefaceMoney4.Text,
                cboSurfaceProcess5.SelectedValue + "", txtSurefaceWeight5.Text, txtSurefaceMoney5.Text,
                cboSurfaceProcess6.SelectedValue + "", txtSurefaceWeight6.Text, txtSurefaceMoney6.Text,
                cboSurfaceProcess7.SelectedValue + "", txtSurefaceWeight7.Text, txtSurefaceMoney7.Text,
                cboSurfaceProcess8.SelectedValue + "", txtSurefaceWeight8.Text, txtSurefaceMoney8.Text,
                cboSurfaceProcess9.SelectedValue + "", txtSurefaceWeight9.Text, txtSurefaceMoney9.Text,
                cboSurfaceProcess10.SelectedValue + "", txtSurefaceWeight10.Text, txtSurefaceMoney10.Text,

                //cboPaintDivision1.SelectedValue + "", txtPaintSquare1.Text,
                //cboPaintDivision2.SelectedValue + "", txtPaintSquare2.Text,

                txtPaintDivision1 + "", txtPaintSquare1.Text,
                txtPaintDivision2 + "", txtPaintSquare2.Text,

                this.getSelectedIndexCombobox(this.getSelectedItem(cboManufactorType)) + "",
                cboStandardRank.SelectedValue + "",

                cboManuFriesRank.SelectedValue + "", txtManuFriesTime.Text,
                cboManuDrillRank.SelectedValue + "", txtManuDrillTime.Text,
                cboManuLatheRank.SelectedValue + "", txtManuLatheTime.Text,
                cboManuTarepanRank.SelectedValue + "", txtManuTarepanTime.Text,
                cboManuBendRank.SelectedValue + "", txtManuBendTime.Text,
                cboManuWeldRank.SelectedValue + "", txtManuWeldTime.Text,
                cboManuOtherRank.SelectedValue + "", txtManuOtherTime.Text,
                cboManuMCRank.SelectedValue + "", txtManuMCTime.Text,
                cboManuPolishRank.SelectedValue + "", txtManuPolishTime.Text,
                cboManuAdhesionRank.SelectedValue + "", txtManuAdhesionTime.Text,

                txtManuToothCuttingCost.Text,
                txtManuQuenchingCost.Text,
                txtManuPreparationTime.Text,
                txtManuPreparationStamp.Text,
                txtManuPreparationColorCheck.Text,
                txtManuKeyCost.Text,
                txtManuTapeTime.Text,
                txtManuEviPerDrawing.Text,
                txtManuEviOneDrawing.Text,
                txtManuScrewTime.Text,

                grvBuyMaterial.Rows[0].Cells[1].Value + "", grvBuyMaterial.Rows[0].Cells[2].Value + "", grvBuyMaterial.Rows[0].Cells[3].Value + "", grvBuyMaterial.Rows[0].Cells[4].Value + "", grvBuyMaterial.Rows[0].Cells[5].Value + "", grvBuyMaterial.Rows[0].Cells[6].Value + "",
                grvBuyMaterial.Rows[1].Cells[1].Value + "", grvBuyMaterial.Rows[1].Cells[2].Value + "", grvBuyMaterial.Rows[1].Cells[3].Value + "", grvBuyMaterial.Rows[1].Cells[4].Value + "", grvBuyMaterial.Rows[1].Cells[5].Value + "", grvBuyMaterial.Rows[1].Cells[6].Value + "",
                grvBuyMaterial.Rows[2].Cells[1].Value + "", grvBuyMaterial.Rows[2].Cells[2].Value + "", grvBuyMaterial.Rows[2].Cells[3].Value + "", grvBuyMaterial.Rows[2].Cells[4].Value + "", grvBuyMaterial.Rows[2].Cells[5].Value + "", grvBuyMaterial.Rows[2].Cells[6].Value + "",
                grvBuyMaterial.Rows[3].Cells[1].Value + "", grvBuyMaterial.Rows[3].Cells[2].Value + "", grvBuyMaterial.Rows[3].Cells[3].Value + "", grvBuyMaterial.Rows[3].Cells[4].Value + "", grvBuyMaterial.Rows[3].Cells[5].Value + "", grvBuyMaterial.Rows[3].Cells[6].Value + "",
                grvBuyMaterial.Rows[4].Cells[1].Value + "", grvBuyMaterial.Rows[4].Cells[2].Value + "", grvBuyMaterial.Rows[4].Cells[3].Value + "", grvBuyMaterial.Rows[4].Cells[4].Value + "", grvBuyMaterial.Rows[4].Cells[5].Value + "", grvBuyMaterial.Rows[4].Cells[6].Value + "",
                grvBuyMaterial.Rows[5].Cells[1].Value + "", grvBuyMaterial.Rows[5].Cells[2].Value + "", grvBuyMaterial.Rows[5].Cells[3].Value + "", grvBuyMaterial.Rows[5].Cells[4].Value + "", grvBuyMaterial.Rows[5].Cells[5].Value + "", grvBuyMaterial.Rows[5].Cells[6].Value + "",
                grvBuyMaterial.Rows[6].Cells[1].Value + "", grvBuyMaterial.Rows[6].Cells[2].Value + "", grvBuyMaterial.Rows[6].Cells[3].Value + "", grvBuyMaterial.Rows[6].Cells[4].Value + "", grvBuyMaterial.Rows[6].Cells[5].Value + "", grvBuyMaterial.Rows[6].Cells[6].Value + "",
                grvBuyMaterial.Rows[7].Cells[1].Value + "", grvBuyMaterial.Rows[7].Cells[2].Value + "", grvBuyMaterial.Rows[7].Cells[3].Value + "", grvBuyMaterial.Rows[7].Cells[4].Value + "", grvBuyMaterial.Rows[7].Cells[5].Value + "", grvBuyMaterial.Rows[7].Cells[6].Value + "",
                cboGradeType.SelectedValue + "", txtRadiusBendEffort.Text, cboSideFinish.Text + "");
            partEstList.Add(partEst);

            try
            {
                // Save Material
                // Move code 
                materialEstBUS.updateBatchMaterialEst(materialList);
                // Move code 
                //SavePartEst

                partEstBUS.updateBatchPartEst(partEstList);

                btnSave.Enabled = false;

                MessageBox.Show(this, "Data has been saved successfully!", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error btnSave_ClickUser(): " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void grvDrawingList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if ((grvDrawingList.CurrentRow != null) && (grvDrawingList.CurrentRow.Index >= 0) && (grvDrawingList.CurrentCell != null))
            //{
            //    if (grvDrawingList.CurrentCell.ColumnIndex == 1)
            //    {
            //        if (Constants.PROGRESS_NOT_STARTED.Equals(grvDrawingList.CurrentCell.Value.ToString()))
            //        {
            //            grvDrawingList[0, grvDrawingList.CurrentCell.RowIndex].Style.BackColor = Color.Gray;
            //        }
            //        else if (Constants.PROGRESS_DOING.Equals(grvDrawingList.CurrentCell.Value.ToString()))
            //        {
            //            grvDrawingList[0, grvDrawingList.CurrentCell.RowIndex].Style.BackColor = Color.Yellow;
            //        }
            //        else
            //        {
            //            grvDrawingList[0, grvDrawingList.CurrentCell.RowIndex].Style.BackColor = Color.Green;
            //        }
            //    }
            //    else if (grvDrawingList.CurrentCell.ColumnIndex == 2)
            //    {
            //        if(!Constants.EMPTY_STRING.Equals(grvDrawingList.CurrentCell.Value + ""))
            //        {
            //            //this.assignTaskForUsers(grvDrawingList.CurrentCell.Value + "");
            //        }
            //    }
            //}
        }

        //private void assignTaskForUsers(string userId)
        //{
        //    if (bIsFormLoaded && (rdoEmptyRowAssign.Checked || rdoReassignAll.Checked))
        //    {
        //        bool bReassignAll = false;
        //        if(rdoReassignAll.Checked)
        //        {
        //            if (!bReassignAll && (MessageBox.Show(this, "Do you want to assign all task with user = [" + userId + "]?", CommonsVars.APP_NAME, MessageBoxButtons.YesNo) == DialogResult.Yes))
        //            {
        //                bReassignAll = true;
        //            }
        //        }
        //        bIsFormLoaded = false;
        //        foreach (DataGridViewRow row in grvDrawingList.Rows)
        //        {
        //            if(rdoEmptyRowAssign.Checked && Constants.EMPTY_STRING.Equals(row.Cells[2].Value + ""))
        //            {
        //                row.Cells[2].Value = userId;
        //            }
        //            else
        //            {
        //                if (bReassignAll)
        //                {
        //                    row.Cells[2].Value = userId;
        //                }
        //            }
        //        }
        //        bIsFormLoaded = true;
        //    }
        //}

        private void controlHighlightGrvMaterial(string shapeCode, DataGridViewRow currentRow, bool isFirstLoad = false)
        {
            //Default
            for (int i = 4; i <= 11; i++)
            {
                if (!isFirstLoad)
                {
                    currentRow.Cells[i].Value = "";
                }
                //currentRow.Cells[i].ReadOnly = true;
                currentRow.Cells[i].Style.BackColor = Color.Gray;
            }

            if ("01,06,07".IndexOf(shapeCode) >= 0)
            {
                currentRow.Cells[4].ReadOnly = true;
                currentRow.Cells[7].ReadOnly = true;
                currentRow.Cells[8].ReadOnly = true;
                currentRow.Cells[9].ReadOnly = true;
                currentRow.Cells[11].ReadOnly = true;

                //Size 1
                //currentRow.Cells[5].ReadOnly = false;
                currentRow.Cells[5].Style.BackColor = isEmptyOrZero(currentRow.Cells[5].Value + "") ? Color.Yellow : Color.White;
                //Size 2
                //currentRow.Cells[6].ReadOnly = false;
                currentRow.Cells[6].Style.BackColor = isEmptyOrZero(currentRow.Cells[6].Value + "") ? Color.Yellow : Color.White;
                // Cutting size 1
                //currentRow.Cells[10].ReadOnly = false;
                currentRow.Cells[10].Style.BackColor = isEmptyOrZero(currentRow.Cells[10].Value + "") ? Color.Yellow : Color.White;

            }
            else if ("02,03,04".IndexOf(shapeCode) >= 0)
            {
                currentRow.Cells[4].ReadOnly = true;
                currentRow.Cells[6].ReadOnly = true;
                currentRow.Cells[7].ReadOnly = true;
                currentRow.Cells[8].ReadOnly = true;
                currentRow.Cells[9].ReadOnly = true;
                currentRow.Cells[11].ReadOnly = true;

                //Size 1
                currentRow.Cells[5].ReadOnly = false;
                currentRow.Cells[5].Style.BackColor = isEmptyOrZero(currentRow.Cells[5].Value + "") ? Color.Yellow : Color.White;
                // Cutting size 1
                currentRow.Cells[10].ReadOnly = false;
                currentRow.Cells[10].Style.BackColor = isEmptyOrZero(currentRow.Cells[10].Value + "") ? Color.Yellow : Color.White;
            }
            else if ("05".IndexOf(shapeCode) >= 0)
            {
                currentRow.Cells[4].ReadOnly = true;
                currentRow.Cells[6].ReadOnly = true;
                currentRow.Cells[7].ReadOnly = true;
                currentRow.Cells[8].ReadOnly = true;
                currentRow.Cells[9].ReadOnly = true;

                //Size 1
                currentRow.Cells[5].ReadOnly = false;
                currentRow.Cells[5].Style.BackColor = isEmptyOrZero(currentRow.Cells[5].Value + "") ? Color.Yellow : Color.White;
                // Cutting size 1
                currentRow.Cells[10].ReadOnly = false;
                currentRow.Cells[10].Style.BackColor = isEmptyOrZero(currentRow.Cells[10].Value + "") ? Color.Yellow : Color.White;
                // Cutting size 2
                currentRow.Cells[11].ReadOnly = false;
                currentRow.Cells[11].Style.BackColor = isEmptyOrZero(currentRow.Cells[11].Value + "") ? Color.Yellow : Color.White;
            }
            else if ("08,12,13,18".IndexOf(shapeCode) >= 0)
            {
                currentRow.Cells[4].ReadOnly = true;
                currentRow.Cells[7].ReadOnly = true;
                currentRow.Cells[8].ReadOnly = true;
                currentRow.Cells[9].ReadOnly = true;
                currentRow.Cells[11].ReadOnly = true;

                //Size 1
                currentRow.Cells[5].ReadOnly = false;
                currentRow.Cells[5].Style.BackColor = isEmptyOrZero(currentRow.Cells[5].Value + "") ? Color.Yellow : Color.White;
                //Size 2
                currentRow.Cells[6].ReadOnly = false;
                currentRow.Cells[6].Style.BackColor = isEmptyOrZero(currentRow.Cells[6].Value + "") ? Color.Yellow : Color.White;
                //Size 3
                currentRow.Cells[7].ReadOnly = false;
                currentRow.Cells[7].Style.BackColor = isEmptyOrZero(currentRow.Cells[7].Value + "") ? Color.Yellow : Color.White;
                // Cutting size 1
                currentRow.Cells[10].ReadOnly = false;
                currentRow.Cells[10].Style.BackColor = isEmptyOrZero(currentRow.Cells[10].Value + "") ? Color.Yellow : Color.White;
            }
            else if ("16".IndexOf(shapeCode) >= 0)
            {
                currentRow.Cells[4].ReadOnly = true;
                currentRow.Cells[9].ReadOnly = true;
                currentRow.Cells[11].ReadOnly = true;

                //Size 1
                currentRow.Cells[5].ReadOnly = false;
                currentRow.Cells[5].Style.BackColor = isEmptyOrZero(currentRow.Cells[5].Value + "") ? Color.Yellow : Color.White;
                //Size 2
                currentRow.Cells[6].ReadOnly = false;
                currentRow.Cells[6].Style.BackColor = isEmptyOrZero(currentRow.Cells[6].Value + "") ? Color.Yellow : Color.White;
                //Size 3
                currentRow.Cells[7].ReadOnly = false;
                currentRow.Cells[7].Style.BackColor = isEmptyOrZero(currentRow.Cells[7].Value + "") ? Color.Yellow : Color.White;
                //Size 4
                currentRow.Cells[8].ReadOnly = false;
                currentRow.Cells[8].Style.BackColor = isEmptyOrZero(currentRow.Cells[8].Value + "") ? Color.Yellow : Color.White;
                // Cutting size 1
                currentRow.Cells[10].ReadOnly = false;
                currentRow.Cells[10].Style.BackColor = isEmptyOrZero(currentRow.Cells[10].Value + "") ? Color.Yellow : Color.White;
            }
            else if ("00,20".IndexOf(shapeCode) >= 0)
            {
                //Default
                for (int i = 4; i <= 11; i++)
                {
                    currentRow.Cells[i].ReadOnly = true;
                }
            }
            else if ("15,31,32,33,34,35,36,37,38,42,43,46,48,50".IndexOf(shapeCode) >= 0)
            {
                for (int i = 5; i <= 11; i++)
                {
                    currentRow.Cells[i].ReadOnly = true;
                }

                //Quantity
                currentRow.Cells[4].ReadOnly = false;
                currentRow.Cells[4].Style.BackColor = isEmptyOrZero(currentRow.Cells[4].Value + "") ? Color.Yellow : Color.White;
            }
        }

        private void grvMaterial_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((grvMaterial.CurrentRow != null) && (grvMaterial.CurrentCell != null))
            {
                if (grvMaterial.CurrentCell.ColumnIndex == 1)
                {
                    fillSurefaceProcessAuto();
                }

                if (grvMaterial.CurrentCell.ColumnIndex == 2)
                {
                    controlHighlightGrvMaterial(grvMaterial.CurrentCell.Value + "", grvMaterial.CurrentRow);
                    grvMaterial.CurrentCell.Style.BackColor = Constants.EMPTY_STRING.Equals(grvMaterial.CurrentCell.Value + "") ? Color.Yellow : Color.White;
                }

                if ((grvMaterial.CurrentCell.ColumnIndex >= 4) && (grvMaterial.CurrentCell.ColumnIndex <= 11) && (!grvMaterial.CurrentCell.ReadOnly))
                {
                    Decimal number = 0;
                    if (!(isEmptyOrZero(grvMaterial.CurrentCell.Value + "") || Decimal.TryParse(grvMaterial.CurrentCell.Value + "", out number)))
                    {
                        MessageBox.Show(this, "You just input wrong value! Please input a number at this item.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        grvMaterial.CurrentCell.Value = Constants.EMPTY_STRING;
                    }
                    grvMaterial.CurrentCell.Style.BackColor = Constants.EMPTY_STRING.Equals(grvMaterial.CurrentCell.Value + "") ? Color.Yellow : Color.White;
                }
                //===============================================================
                if (grvMaterial.CurrentCell.RowIndex == grvMaterial.Rows.Count - 1)
                {
                    grvMaterial.CurrentCell.Style.BackColor = Color.White;
                }

            }
        }

        private void grvMaterial_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //e.Cancel = true;
            //MessageBox.Show("Error happened " + e.Context.ToString());

            //if (e.Context == DataGridViewDataErrorContexts.Commit)
            //{
            //    MessageBox.Show("Commit error");
            //}
            //if (e.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            //{
            //    MessageBox.Show("Cell change");
            //}
            //if (e.Context == DataGridViewDataErrorContexts.Parsing)
            //{
            //    MessageBox.Show("parsing error");
            //}
            //if (e.Context == DataGridViewDataErrorContexts.LeaveControl)
            //{
            //    MessageBox.Show("leave control error");
            //}

            //if ((e.Exception) is ConstraintException)
            //{
            //    DataGridView view = (DataGridView)sender;
            //    view.Rows[e.RowIndex].ErrorText = "an error";
            //    view.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "an error";

            //    e.ThrowException = false;
            //}
        }

        private void highlightPairTextBoxByComboValue(MaskedTextBox txtHighlight, List<string> specialValueList)
        {
            bool bIsExistSpecialValue = false;
            txtManuPreparationTime.BackColor = Color.White;
            if (!isEmptyOrZero(txtDrawingCode.Text)) // (bIsFormLoaded)
            {
                foreach (ComboBox cbo in this.surefaceComboBoxList)
                {
                    cbo.BackColor = Color.White;
                    if (cbo.SelectedIndex >= 0)
                    {
                        foreach (string itemValue in specialValueList)
                        {
                            if (itemValue.Equals(cbo.SelectedValue))
                            {
                                bIsExistSpecialValue = true;
                                txtManuPreparationTime.BackColor = ((bIsExistSpecialValue) && isEmptyOrZero(txtManuPreparationTime.Text)) ? Color.Yellow : Color.White;
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void unhighlightComboExceptTextBox(MaskedTextBox targetTextBox)
        {
            if (!Constants.EMPTY_STRING.Equals(targetTextBox.Text.Trim()))
            {
                targetTextBox.BackColor = Color.White;
                int i = 1;
                foreach (ComboBox cbo in this.surefaceComboBoxList)
                {
                    if (cbo.SelectedIndex >= 0)
                    {
                        foreach (string specialValue in this.surfaceSpecialList)
                        {
                            if (specialValue.Equals(cbo.SelectedValue))
                            {
                                Control[] controls = this.Controls.Find("txtSurefaceQuantity" + i.ToString(), true);
                                foreach (Control ctl in controls)
                                {
                                    if (!Constants.EMPTY_STRING.Equals(ctl.Text.Trim()))
                                    {
                                        cbo.BackColor = Color.White;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    i++;
                }
            }
        }

        private void controlMessage1Display(ComboBox cboCurrent)
        {
            if (this.bIsFormLoaded && (cboCurrent.SelectedIndex >= 0))
            {
                bool bFound = false;
                foreach (string surefaceCode in this.surfaceSpecialMessageList)
                {
                    if (surefaceCode.Equals(cboCurrent.SelectedValue))
                    {
                        bFound = true;
                        break;
                    }
                }
                if (bFound)
                {
                    txtSurefaceMessage.Text = CommonsVars.CURRENT_LANGUAGE.Equals(Constants.LOCATION_JAPAN) ? SURFACE_MESSAGE : SURFACE_MESSAGE_VN;
                }
                else
                {
                    txtSurefaceMessage.Text = Constants.EMPTY_STRING;
                }

            }
            else
            {
                txtSurefaceMessage.Text = "";
            }
        }

        private bool isMoneyItem_SurfaceProcess(string surfaceProcessCode)
        {
            if (this.dtSurefaceProcess != null)
            {
                foreach (DataRow row in this.dtSurefaceProcess.Rows)
                {
                    if ((surfaceProcessCode.Equals(row["SUR_PRO_CODE"] + "")) && ((row["SUR_PRO_UNIT"] + "").Equals("円")))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void setDefaultValueForSurefaceRelatedItems(ComboBox cbo, MaskedTextBox txtQuantity, MaskedTextBox txtMoney, out bool bIsMoney)
        {
            bIsMoney = false;
            if (cbo.SelectedIndex >= 0)
            {
                string selectedValue = cbo.SelectedValue + "";
                bIsMoney = isMoneyItem_SurfaceProcess(selectedValue);
                if (bIsMoney)
                {
                    if (int.Parse("0" + txtQuantity.Text.Trim()) == 0)
                    {
                        txtQuantity.Text = "0";
                    }
                    if (int.Parse("0" + txtMoney.Text.Trim()) == 0)
                    {
                        txtMoney.Text = "1";
                    }
                }
                //if (this.dtSurefaceProcess != null)
                //{
                //    foreach(DataRow row in this.dtSurefaceProcess.Rows)
                //    {
                //        if((selectedValue.Equals(row["SUR_PRO_CODE"] + "")) && ((row["SUR_PRO_UNIT"] + "").Equals("円")))
                //        {
                //            if(int.Parse("0" + txtQuantity.Text.Trim()) == 0)
                //            {
                //                txtQuantity.Text = "1";
                //            }
                //            if (int.Parse("0" + txtMoney.Text.Trim()) == 0)
                //            {
                //                txtMoney.Text = "1";
                //            }
                //            bIsMoney = true;
                //            break;
                //        }
                //    }
                //}
            }
            else
            {
                txtQuantity.Text = Constants.EMPTY_STRING;
                txtMoney.Text = Constants.EMPTY_STRING;
            }
        }

        private void displaySurfaceMessage2()
        {
            if (bIsFormLoaded)
            {
                string surfaceName = "";

                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess1);
                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess2);
                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess3);
                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess4);
                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess5);
                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess6);
                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess7);
                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess8);
                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess9);
                surfaceName = surfaceName + getNameOfSpecialSurfaceComboBox(cboSurfaceProcess10);

                txtManuMessage.Text = Constants.EMPTY_STRING.Equals(surfaceName) ? Constants.EMPTY_STRING :
                    string.Format(CommonsVars.CURRENT_LANGUAGE.Equals(Constants.LOCATION_JAPAN) ? SURFACE_MESSAGE2 : SURFACE_MESSAGE2_VN, surfaceName.Substring(0, surfaceName.Length - 1));
            }
            else
            {
                txtManuMessage.Text = Constants.EMPTY_STRING;
            }
        }

        private string getNameOfSpecialSurfaceComboBox(ComboBox cbo)
        {
            if (cbo.SelectedIndex < 0)
            {
                return Constants.EMPTY_STRING;
            }
            else
            {
                foreach (string surfaceCode in surfaceSpecialMessageList2)
                {
                    if (surfaceCode.Equals(cbo.SelectedValue))
                    {
                        return cbo.Text + "、";
                    }
                }

            }
            return Constants.EMPTY_STRING;
        }

        private void cboSurfaceProcess1_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess1, txtSurefaceWeight1, txtSurefaceMoney1, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess1, txtSurefaceWeight1, txtSurefaceMoney1, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);

            this.controlMessage1Display((ComboBox)sender);

            displaySurfaceMessage2();
        }

        private void cboSurfaceProcess2_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess2, txtSurefaceWeight2, txtSurefaceMoney2, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess2, txtSurefaceWeight2, txtSurefaceMoney2, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            this.controlMessage1Display((ComboBox)sender);
            displaySurfaceMessage2();
        }

        private void cboSurfaceProcess3_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess3, txtSurefaceWeight3, txtSurefaceMoney3, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess3, txtSurefaceWeight3, txtSurefaceMoney3, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            this.controlMessage1Display((ComboBox)sender);
            displaySurfaceMessage2();
        }

        private void cboSurfaceProcess4_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess4, txtSurefaceWeight4, txtSurefaceMoney4, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess4, txtSurefaceWeight4, txtSurefaceMoney4, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            this.controlMessage1Display((ComboBox)sender);
            displaySurfaceMessage2();
        }

        private void cboSurfaceProcess5_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess5, txtSurefaceWeight5, txtSurefaceMoney5, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess5, txtSurefaceWeight5, txtSurefaceMoney5, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            this.controlMessage1Display((ComboBox)sender);
            displaySurfaceMessage2();
        }

        private void cboSurfaceProcess6_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess6, txtSurefaceWeight6, txtSurefaceMoney6, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess6, txtSurefaceWeight6, txtSurefaceMoney6, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            this.controlMessage1Display((ComboBox)sender);
            displaySurfaceMessage2();
        }

        private void cboSurfaceProcess7_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess7, txtSurefaceWeight7, txtSurefaceMoney7, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess7, txtSurefaceWeight7, txtSurefaceMoney7, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            this.controlMessage1Display((ComboBox)sender);
            displaySurfaceMessage2();
        }

        private void cboSurfaceProcess8_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess8, txtSurefaceWeight8, txtSurefaceMoney8, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess8, txtSurefaceWeight8, txtSurefaceMoney8, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            this.controlMessage1Display((ComboBox)sender);
            displaySurfaceMessage2();
        }

        private void cboSurfaceProcess9_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess9, txtSurefaceWeight9, txtSurefaceMoney9, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess9, txtSurefaceWeight9, txtSurefaceMoney9, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            this.controlMessage1Display((ComboBox)sender);
            displaySurfaceMessage2();
        }

        private void cboSurfaceProcess10_TextChanged(object sender, EventArgs e)
        {
            bool bIsMoneyItem = false;
            setDefaultValueForSurefaceRelatedItems(cboSurfaceProcess10, txtSurefaceWeight10, txtSurefaceMoney10, out bIsMoneyItem);
            highlightPairTextBox(cboSurfaceProcess10, txtSurefaceWeight10, txtSurefaceMoney10, bIsMoneyItem);
            highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            this.controlMessage1Display((ComboBox)sender);
            displaySurfaceMessage2();
        }

        private void highlightPairComboBox_SurfaceProcess(ComboBox cboSurface, MaskedTextBox txtQuantity, MaskedTextBox txtMoney)
        {
            if ((!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())))
            {
                bool bIsMoneyItem = isMoneyItem_SurfaceProcess(cboSurface.SelectedValue + "");
                txtMoney.Text = bIsMoneyItem ? txtMoney.Text : Constants.EMPTY_STRING;

                if (int.Parse("0" + txtQuantity.Text.Trim()) != 0)
                {
                    cboSurface.BackColor = cboSurface.SelectedIndex < 0 ? Color.Yellow : Color.White;
                    txtMoney.BackColor = bIsMoneyItem && (int.Parse("0" + txtMoney.Text.Trim()) == 0) ? Color.YellowGreen : Color.White;
                    txtQuantity.BackColor = Color.White;
                }
                else
                {
                    txtQuantity.BackColor = ((cboSurface.SelectedIndex >= 0) || (int.Parse("0" + txtMoney.Text.Trim()) != 0)) ? Color.Yellow : Color.White;
                    cboSurface.BackColor = (int.Parse("0" + txtMoney.Text.Trim()) != 0) ? Color.Yellow : Color.White;
                    txtMoney.BackColor = bIsMoneyItem && ((cboSurface.SelectedIndex >= 0) || (int.Parse("0" + txtQuantity.Text.Trim()) != 0)) ? Color.YellowGreen : Color.White;
                }
                // Request From HUY
                if (!cboSurface.Text.Equals(""))
                {
                    if (checkSurfaceCondition_ToGenerate_0_1(cboSurface))
                    {
                        txtQuantity.BackColor = Color.GreenYellow;
                    }
                }

            }
            else
            {
                cboSurface.BackColor = Color.White;
                txtQuantity.BackColor = Color.White;
                txtMoney.BackColor = Color.White;
            }
        }

        private void txtSurefaceQuantity1_TextChanged_1(object sender, EventArgs e)
        {
            //highlightPairComboBox(cboSurfaceProcess1, txtSurefaceWeight1, txtSurefaceMoney1);
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess1, txtSurefaceWeight1, txtSurefaceMoney1);
            ChangeSaveButtonStatus(true);


        }

        private void txtSurefaceQuantity2_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess2, txtSurefaceWeight2, txtSurefaceMoney2);
            ChangeSaveButtonStatus(true);

        }

        private void txtSurefaceQuantity3_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess3, txtSurefaceWeight3, txtSurefaceMoney3);
            ChangeSaveButtonStatus(true);

        }

        private void txtSurefaceQuantity4_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess4, txtSurefaceWeight4, txtSurefaceMoney4);
            ChangeSaveButtonStatus(true);

        }

        private void txtSurefaceQuantity5_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess5, txtSurefaceWeight5, txtSurefaceMoney5);
            ChangeSaveButtonStatus(true);

        }

        private void txtSurefaceQuantity6_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess6, txtSurefaceWeight6, txtSurefaceMoney6);
            ChangeSaveButtonStatus(true);

        }

        private void txtSurefaceQuantity7_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess7, txtSurefaceWeight7, txtSurefaceMoney7);
            ChangeSaveButtonStatus(true);

        }

        private void txtSurefaceQuantity8_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess8, txtSurefaceWeight8, txtSurefaceMoney8);
            ChangeSaveButtonStatus(true);

        }

        private void txtSurefaceQuantity9_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess9, txtSurefaceWeight9, txtSurefaceMoney9);
            ChangeSaveButtonStatus(true);

        }

        private void txtSurefaceQuantity10_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess(cboSurfaceProcess10, txtSurefaceWeight10, txtSurefaceMoney10);
            ChangeSaveButtonStatus(true);

        }

        private void cboPaintDivision1_TextChanged(object sender, EventArgs e)
        {
            highlightPaintPairTextBox(cboPaintDivision1, txtPaintSquare1);
            ChangeSaveButtonStatus(true);
        }

        private void cboPaintDivision2_TextChanged(object sender, EventArgs e)
        {
            highlightPaintPairTextBox(cboPaintDivision2, txtPaintSquare2);
            ChangeSaveButtonStatus(true);
        }

        private void txtPaintSquare1_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboPaintDivision1, txtPaintSquare1);
            cboPaintDivision1.SelectedIndex = Constants.EMPTY_STRING.Equals(txtPaintSquare1.Text.Trim()) ? 1 : 0;
            ChangeSaveButtonStatus(true);
        }

        private void txtPaintSquare2_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboPaintDivision2, txtPaintSquare2);
            cboPaintDivision2.SelectedIndex = Constants.EMPTY_STRING.Equals(txtPaintSquare2.Text.Trim()) ? 1 : 0;
            ChangeSaveButtonStatus(true);
        }

        private void txtManuFriesTime_TextChanged_1(object sender, EventArgs e)
        {
            //ChangeSaveButtonStatus(true);
            highlightPairComboBox(cboManuFriesRank, txtManuFriesTime, chkManuFries);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuDrillTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboManuDrillRank, txtManuDrillTime, chkManuDrill);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuLatheTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboManuLatheRank, txtManuLatheTime, chkManuLathe);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuTarepanTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboManuTarepanRank, txtManuTarepanTime, chkManuTarepan);
            highlightPairTextBoxvsTextBox(txtManuTarepanTime, txtManuTapeTime);
            controlRelationOfBendAndTarepanTime((MaskedTextBox)sender);
            ChangeSaveButtonStatus(true);
            if (bIsFormLoaded)
            {
                if (int.Parse("0" + txtManuTarepanTime.Text.Trim()) != 0)
                {
                    cboGradeType.BackColor = cboGradeType.SelectedIndex >= 0 ? Color.White : Color.Yellow;
                }
                else
                {
                    cboGradeType.BackColor = Color.White;
                }
            }
        }

        private void txtManuBendTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboManuBendRank, txtManuBendTime, chkManuBend);
            controlRelationOfBendAndTarepanTime((MaskedTextBox)sender);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuWeldTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboManuWeldRank, txtManuWeldTime, chkManuWeld);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuOtherTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboManuOtherRank, txtManuOtherTime, chkManuOther);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuMCTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboManuMCRank, txtManuMCTime, chkManuMC);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuPolishTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboManuPolishRank, txtManuPolishTime, chkManuPolish);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuAdhesionTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox(cboManuAdhesionRank, txtManuAdhesionTime, chkManuAdhesion);
            ChangeSaveButtonStatus(true);
        }

        private void cboManuFriesRank_TextChanged(object sender, EventArgs e)
        {

            highlightPairTextBox(cboManuFriesRank, txtManuFriesTime, chkManuFries);
            ChangeSaveButtonStatus(true);
        }

        private void cboManuDrillRank_TextChanged(object sender, EventArgs e)
        {
            highlightPairTextBox(cboManuDrillRank, txtManuDrillTime, chkManuDrill);
            ChangeSaveButtonStatus(true);
        }

        private void cboManuLatheRank_TextChanged(object sender, EventArgs e)
        {
            highlightPairTextBox(cboManuLatheRank, txtManuLatheTime, chkManuLathe);
            ChangeSaveButtonStatus(true);
        }

        private void cboManuTarepanRank_TextChanged(object sender, EventArgs e)
        {
            highlightPairTextBox(cboManuTarepanRank, txtManuTarepanTime, chkManuTarepan);
            highlightPairTextBox(cboManuTarepanRank, txtManuTapeTime, chkManuTarepan);
            if (bIsFormLoaded && (cboManuTarepanRank.SelectedIndex >= 0))
            {
                cboGradeType.BackColor = cboGradeType.SelectedIndex >= 0 ? Color.White : Color.Yellow;
            }
            ChangeSaveButtonStatus(true);
        }

        private void cboManuBendRank_TextChanged(object sender, EventArgs e)
        {
            highlightPairTextBox(cboManuBendRank, txtManuBendTime, chkManuBend);
            ChangeSaveButtonStatus(true);
        }

        private void cboManuWeldRank_TextChanged(object sender, EventArgs e)
        {
            highlightPairTextBox(cboManuWeldRank, txtManuWeldTime, chkManuWeld);
            ChangeSaveButtonStatus(true);
        }

        private void cboManuOtherRank_TextChanged(object sender, EventArgs e)
        {
            highlightPairTextBox(cboManuOtherRank, txtManuOtherTime, chkManuOther);
            ChangeSaveButtonStatus(true);
        }

        private void cboManuMCRank_TextChanged(object sender, EventArgs e)
        {
            highlightPairTextBox(cboManuMCRank, txtManuMCTime, chkManuMC);
            ChangeSaveButtonStatus(true);
        }

        private void cboManuPolishRank_TextChanged(object sender, EventArgs e)
        {
            highlightPairTextBox(cboManuPolishRank, txtManuPolishTime, chkManuPolish);
            ChangeSaveButtonStatus(true);
        }

        private void cboManuAdhesionRank_TextChanged(object sender, EventArgs e)
        {
            highlightPairTextBox(cboManuAdhesionRank, txtManuAdhesionTime, chkManuAdhesion);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuPreparationTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuPreparationTime, chk3);
            unhighlightComboExceptTextBox(txtManuPreparationTime);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuToothCuttingCost_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuToothCuttingCost, chk1);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuQuenchingCost_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuQuenchingCost, chk2);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuPreparationStamp_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuPreparationStamp, chk4);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuPreparationColorCheck_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuPreparationColorCheck, chk5);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuKeyCost_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuKeyCost, chk6);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuTapeTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuTapeTime, chk7);
            ChangeSaveButtonStatus(true);
            if (!Constants.EMPTY_STRING.Equals(txtManuTapeTime.Text.Trim()))
            {
                txtManuTapeTime.BackColor = Color.White;
            }
            else
            {
                if ((!Constants.EMPTY_STRING.Equals(txtManuTarepanTime.Text.Trim())) || (cboManuTarepanRank.SelectedIndex >= 0))
                {
                    txtManuTapeTime.BackColor = Color.Yellow;
                }
            }
        }

        private void txtManuWoodCost_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuEviPerDrawing, chk8);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuEvidenceCost_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuEviOneDrawing, chk9);
            ChangeSaveButtonStatus(true);
        }

        private void txtManuScrewTime_TextChanged_1(object sender, EventArgs e)
        {
            highlightMaskedTextBoxvsCheckBox(txtManuScrewTime, chk10);
            ChangeSaveButtonStatus(true);
        }

        private void grvBuyMaterial_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (grvBuyMaterial.Rows.Count > 0)
            {
                if ((grvBuyMaterial.CurrentCell.FormattedValue.ToString().Length > 0) && (grvBuyMaterial.CurrentCell.Style.BackColor == Color.LightPink))
                {
                    grvBuyMaterial.CurrentCell.Style.BackColor = Color.White;
                }

            }

            //=====================================================================================
            if ((grvBuyMaterial.CurrentCell != null) && (grvBuyMaterial.CurrentCell.RowIndex >= 0))
            {
                if (grvBuyMaterial.CurrentCell.ColumnIndex == 4)
                {
                    if (!Constants.EMPTY_STRING.Equals(grvBuyMaterial.CurrentCell.Value + ""))
                    {
                        grvBuyMaterial[5, grvBuyMaterial.CurrentCell.RowIndex].Value = 1;
                        if ("1".Equals(grvBuyMaterial.CurrentCell.Value + ""))
                        {
                            grvBuyMaterial[6, grvBuyMaterial.CurrentCell.RowIndex].Value = 2;
                        }
                        else
                        {
                            grvBuyMaterial[6, grvBuyMaterial.CurrentCell.RowIndex].Value = Constants.EMPTY_STRING;
                        }
                    }
                    else
                    {
                        grvBuyMaterial[5, grvBuyMaterial.CurrentCell.RowIndex].Value = Constants.EMPTY_STRING;
                    }
                }
                ChangeSaveButtonStatus(true);
            }
        }

        private void grvBuyMaterial_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            e.Control.KeyPress -= new KeyPressEventHandler(Column_4_5_6_KeyPress);
            if (grvBuyMaterial.CurrentCell.ColumnIndex == 4 || grvBuyMaterial.CurrentCell.ColumnIndex == 5 || grvBuyMaterial.CurrentCell.ColumnIndex == 6) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column_4_5_6_KeyPress);
                }
            }

            if ((grvBuyMaterial.CurrentCell != null) && (grvBuyMaterial.CurrentCell.RowIndex >= 0) && (grvBuyMaterial.CurrentCell.ColumnIndex >= 6))
            {
                columnTextBox = e.Control as TextBox;
                if (columnTextBox != null)
                {
                    columnTextBox.KeyPress += TextBox_KeyPress;
                }
            }

        }
        //Rule fill Data
        private void Column_4_5_6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void grvBuyMaterial_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((grvBuyMaterial.CurrentCell != null) && (grvBuyMaterial.CurrentCell.RowIndex >= 0))
            {
                if ((columnTextBox != null) && (grvBuyMaterial.CurrentCell.ColumnIndex >= 4))
                {
                    columnTextBox.KeyPress -= TextBox_KeyPress;
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var textBox = (TextBox)sender;

            if ((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }

            if ((!char.IsControl(e.KeyChar)) && (textBox.Text + "").Length >= 7)
            {
                e.Handled = true;
            }
        }

        private void TextBox_KeyPress_DecimalAccept(object sender, KeyPressEventArgs e)
        {
            var textBox = (TextBox)sender;

            if ((!char.IsControl(e.KeyChar)) && (!char.IsDigit(e.KeyChar) && e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((!char.IsControl(e.KeyChar)) && (textBox.Text + "").Length >= 7)
            {
                e.Handled = true;
            }
        }

        private void loadDataIntoGrvMaterialInfor(int inputInfoId, string drawingCode)
        {

            BUS_MaterialEst materInfor = new BUS_MaterialEst();
            DataTable dt = materInfor.getMateriaInfolByInputIdAndDrawingCode(inputInfoId, drawingCode);

            grvMaterial.RowCount = 1;


            if (dt.Rows.Count > 0)
            {
                // Display info on TextBox, ComboBox
                txtDrawingCode.Text = drawingCode;
                txtOldSubCode.Text = dt.Rows[0]["OLD_SUB_CODE"] + "";
                cboManuDivision.Text = dt.Rows[0]["MANU_CODE"] + "";


                // Display info on Material grid
                int i = 1;
                foreach (DataRow dtRow in dt.Rows)
                {
                    DataGridViewRow grvRow = new DataGridViewRow();
                    grvRow.CreateCells(grvMaterial);
                    grvRow.Cells[0].Value = i.ToString();
                    grvRow.Cells[1].Value = dtRow["MATERIAL_CODE"] + "";
                    //LoadgrvcboShapegrvCombobox((DataGridViewComboBoxCell)grvRow.Cells[2], dtRow["MATERIAL_CODE"] + "");
                    grvRow.Cells[2].Value = dtRow["SHAPE_DIV_CODE"] + "";
                    grvRow.Cells[2].Style.BackColor = Constants.EMPTY_STRING.Equals(grvRow.Cells[2].Value + "") ? Color.Yellow : Color.White;
                    //grvRow.Cells[2].Style.BackColor = Color.Yellow;
                    grvRow.Cells[3].Value = dtRow["PHASE_CODE"] + "";
                    grvRow.Cells[4].Value = dtRow["WEIGHT"] + "";
                    grvRow.Cells[5].Value = dtRow["SHAPE_SIZE_A"] + "";
                    grvRow.Cells[6].Value = dtRow["SHAPE_SIZE_B"] + "";
                    grvRow.Cells[7].Value = dtRow["SHAPE_SIZE_C"] + "";
                    grvRow.Cells[8].Value = dtRow["SHAPE_SIZE_D"] + "";
                    grvRow.Cells[9].Value = dtRow["SHAPE_SIZE_E"] + "";
                    grvRow.Cells[10].Value = dtRow["CUTTING_SIZE_A"] + "";
                    grvRow.Cells[11].Value = dtRow["CUTTING_SIZE_B"] + "";
                    controlHighlightGrvMaterial(grvRow.Cells[2].Value + "", grvRow, true);
                    grvMaterial.Rows.Add(grvRow);
                    i++;
                }

            }

        }

        private void loadDataIntoGrvPartEst(int inputInfoId, string drawingCode)
        {

            BUS_PartEst partEstBUS = new BUS_PartEst();
            DataTable dt = partEstBUS.getDrawingInfoFromPartEstById(inputInfoId, drawingCode);

            grvBuyMaterial.RowCount = 0;

            if (dt.Rows.Count > 0)
            {
                // Display surfaceProcess
                txtDrawingCode.Text = drawingCode;
                cboSurfaceProcess1.SelectedValue = dt.Rows[0]["SUR_PRO_CODE1"] + "";
                txtSurefaceWeight1.Text = dt.Rows[0]["SUR_PRO1_QNTY"] + "";
                txtSurefaceMoney1.Text = dt.Rows[0]["SUR_PRO1_AMOUNT"] + "";


                cboSurfaceProcess2.SelectedValue = dt.Rows[0]["SUR_PRO_CODE2"] + "";
                txtSurefaceWeight2.Text = dt.Rows[0]["SUR_PRO2_QNTY"] + "";
                txtSurefaceMoney2.Text = dt.Rows[0]["SUR_PRO2_AMOUNT"] + "";

                cboSurfaceProcess3.SelectedValue = dt.Rows[0]["SUR_PRO_CODE3"] + "";
                txtSurefaceWeight3.Text = dt.Rows[0]["SUR_PRO3_QNTY"] + "";
                txtSurefaceMoney3.Text = dt.Rows[0]["SUR_PRO3_AMOUNT"] + "";

                cboSurfaceProcess4.SelectedValue = dt.Rows[0]["SUR_PRO_CODE4"] + "";
                txtSurefaceWeight4.Text = dt.Rows[0]["SUR_PRO4_QNTY"] + "";
                txtSurefaceMoney4.Text = dt.Rows[0]["SUR_PRO4_AMOUNT"] + "";

                cboSurfaceProcess5.SelectedValue = dt.Rows[0]["SUR_PRO_CODE5"] + "";
                txtSurefaceWeight5.Text = dt.Rows[0]["SUR_PRO5_QNTY"] + "";
                txtSurefaceMoney5.Text = dt.Rows[0]["SUR_PRO5_AMOUNT"] + "";

                cboSurfaceProcess6.SelectedValue = dt.Rows[0]["SUR_PRO_CODE6"] + "";
                txtSurefaceWeight6.Text = dt.Rows[0]["SUR_PRO6_QNTY"] + "";
                txtSurefaceMoney6.Text = dt.Rows[0]["SUR_PRO6_AMOUNT"] + "";

                cboSurfaceProcess7.SelectedValue = dt.Rows[0]["SUR_PRO_CODE7"] + "";
                txtSurefaceWeight7.Text = dt.Rows[0]["SUR_PRO7_QNTY"] + "";
                txtSurefaceMoney7.Text = dt.Rows[0]["SUR_PRO7_AMOUNT"] + "";

                cboSurfaceProcess8.SelectedValue = dt.Rows[0]["SUR_PRO_CODE8"] + "";
                txtSurefaceWeight8.Text = dt.Rows[0]["SUR_PRO8_QNTY"] + "";
                txtSurefaceMoney8.Text = dt.Rows[0]["SUR_PRO8_AMOUNT"] + "";

                cboSurfaceProcess9.SelectedValue = dt.Rows[0]["SUR_PRO_CODE9"] + "";
                txtSurefaceWeight9.Text = dt.Rows[0]["SUR_PRO9_QNTY"] + "";
                txtSurefaceMoney9.Text = dt.Rows[0]["SUR_PRO9_AMOUNT"] + "";

                cboSurfaceProcess10.SelectedValue = dt.Rows[0]["SUR_PRO_CODE10"] + "";
                txtSurefaceWeight10.Text = dt.Rows[0]["SUR_PRO10_QNTY"] + "";
                txtSurefaceMoney10.Text = dt.Rows[0]["SUR_PRO10_AMOUNT"] + "";
                //Paint
                txtPaintSquare1.Text = dt.Rows[0]["PAINT_AREA1"] + "";
                txtPaintSquare2.Text = dt.Rows[0]["PAINT_AREA2"] + "";

                cboPaintDivision1.SelectedValue = dt.Rows[0]["PAINT_CODE1"] + "";
                cboPaintDivision2.SelectedValue = dt.Rows[0]["PAINT_CODE2"] + "";

                // 
                cboManufactorType.SelectedValue = dt.Rows[0]["MANU_CODE"] + "";
                cboStandardRank.SelectedValue = dt.Rows[0]["MANU_RANK_CODE"] + "";

                //
                txtManuFriesTime.Text = dt.Rows[0]["FRIES_MANU_TIME"] + "";
                cboManuFriesRank.SelectedValue = dt.Rows[0]["FRIES_RANK_CODE"] + "";

                txtManuDrillTime.Text = dt.Rows[0]["DRILL_MANU_TIME"] + "";
                cboManuDrillRank.SelectedValue = dt.Rows[0]["DRILL_RANK_CODE"] + "";

                txtManuLatheTime.Text = dt.Rows[0]["LATHE_MANU_TIME"] + "";
                cboManuLatheRank.SelectedValue = dt.Rows[0]["LATHE_RANK_CODE"] + "";

                txtManuTarepanTime.Text = dt.Rows[0]["TAREPAN_MANU_TIME"] + "";
                cboManuTarepanRank.SelectedValue = dt.Rows[0]["TAREPAN_RANK_CODE"] + "";

                txtManuBendTime.Text = dt.Rows[0]["BEND_MANU_TIME"] + "";
                cboManuBendRank.SelectedValue = dt.Rows[0]["BEND_RANK_CODE"] + "";

                txtManuWeldTime.Text = dt.Rows[0]["WELD_MANU_TIME"] + "";
                cboManuWeldRank.SelectedValue = dt.Rows[0]["WELD_RANK_CODE"] + "";

                txtManuOtherTime.Text = dt.Rows[0]["OTHER_MANU_TIME"] + "";
                cboManuOtherRank.SelectedValue = dt.Rows[0]["OTHER_RANK_CODE"] + "";

                txtManuMCTime.Text = dt.Rows[0]["MC_MANU_TIME"] + "";
                cboManuMCRank.SelectedValue = dt.Rows[0]["MC_RANK_CODE"] + "";

                txtManuPolishTime.Text = dt.Rows[0]["POLISH_MANU_TIME"] + "";
                cboManuPolishRank.SelectedValue = dt.Rows[0]["POLISH_RANK_CODE"] + "";

                txtManuAdhesionTime.Text = dt.Rows[0]["ADHESION_MANU_TIME"] + "";
                cboManuAdhesionRank.SelectedValue = dt.Rows[0]["ADHESION_RANK_CODE"] + "";
                //combobox 加工種別 

                // show 3kyu & R uốn
                cboGradeType.SelectedValue = dt.Rows[0]["LEVEL3"] + "";
                txtRadiusBendEffort.Text = dt.Rows[0]["RADIUS_BEND_EFFORT"] + "";

                //ManuInfor Rank. 
                txtManuToothCuttingCost.Text = dt.Rows[0]["EST_COST_OF_TOOTH_CUTTING"] + "";
                txtManuQuenchingCost.Text = dt.Rows[0]["EST_COST_OF_QUENCHING"] + "";
                txtManuPreparationTime.Text = dt.Rows[0]["EST_TIME_OF_PRE_MANU"] + "";
                txtManuPreparationStamp.Text = dt.Rows[0]["EST_TIME_OF_PRE_STAMP"] + "";
                txtManuPreparationColorCheck.Text = dt.Rows[0]["EST_TIME_OF_PRE_COLOR_CHECK"] + "";
                txtManuKeyCost.Text = dt.Rows[0]["EST_COST_OF_KEY"] + "";
                txtManuTapeTime.Text = dt.Rows[0]["EST_TIME_OF_TAPE"] + "";
                txtManuEviPerDrawing.Text = dt.Rows[0]["EVIDENCE_PER_DRAWING"] + "";
                txtManuEviOneDrawing.Text = dt.Rows[0]["EVIDENCE_ONE_DRAWING"] + "";
                txtManuScrewTime.Text = dt.Rows[0]["SCREW"] + "";

                const int MAX_PURCHASED_PART = 8;

                for (int i = 1; i <= MAX_PURCHASED_PART; i++)
                {
                    DataGridViewRow grvRow = new DataGridViewRow();
                    grvRow.CreateCells(grvBuyMaterial);
                    grvRow.Cells[0].Value = i.ToString();
                    grvRow.Cells[1].Value = dt.Rows[0]["PURCHASED_PART_CODE" + i.ToString()] + "";
                    grvRow.Cells[2].Value = dt.Rows[0]["PURCHASED_PART_MAKER_NAME" + i.ToString()] + "";
                    grvRow.Cells[3].Value = dt.Rows[0]["PURCHASED_PART_MODEL" + i.ToString()] + "";
                    grvRow.Cells[4].Value = dt.Rows[0]["PURCHASED_PART_PRICE" + i.ToString()] + "";
                    grvRow.Cells[5].Value = dt.Rows[0]["NUMBER_OF_PARTS_PURCHASED" + i.ToString()] + "";
                    grvRow.Cells[6].Value = dt.Rows[0]["PURCHASED_PART_RATE_CODE" + i.ToString()] + "";
                    grvBuyMaterial.Rows.Add(grvRow);
                }

                cboSideFinish.Text = dt.Rows[0]["SIDE_FINISH"] + "";
            }
        }

        private int getIntegerValueOfString(string strText)
        {
            return Constants.EMPTY_STRING.Equals(strText.Trim()) ? 0 : int.Parse(strText.Trim());
        }

        private void controlManufactoringTypeAndRankType(MaskedTextBox currentTextBox, ComboBox targetComboBox, bool isNotGradeType = true)
        {
            if (chkManuTarepan.CheckState == CheckState.Checked) //  2018/09/13: || chkManuBend.CheckState == CheckState.Checked
            {
                if (!Constants.EMPTY_STRING.Equals(txtManuTarepanTime.Text))
                {
                    int totalTime = this.calculateManufaturingTime();
                    if (totalTime == 0)
                    {
                        return;
                    }
                    int terapanTime = int.Parse(txtManuTarepanTime.Text);
                    int raduisTime = Constants.EMPTY_STRING.Equals(txtManuBendTime.Text.Trim()) ? 0 : int.Parse(txtManuBendTime.Text.Trim());
                    int raduisBendEffort = Constants.EMPTY_STRING.Equals(txtRadiusBendEffort.Text.Trim()) ? 0 : int.Parse(txtRadiusBendEffort.Text.Trim());
                    decimal percentManuRate = 0;
                    percentManuRate = ((decimal)(terapanTime + raduisTime - raduisBendEffort) * 100) / totalTime;

                    if (percentManuRate < 25)
                    {
                        cboManufactorType.SelectedValue = "2";
                    }
                    else if ((percentManuRate >= 25) && (percentManuRate < 65))
                    {
                        cboManufactorType.SelectedValue = "6";
                    }
                    else if ((percentManuRate >= 65) && (percentManuRate < 80))
                    {
                        cboManufactorType.SelectedValue = "7";
                    }
                    else
                    {
                        if ("1".Equals(cboGradeType.SelectedValue) || "2".Equals(cboGradeType.SelectedValue))
                        {
                            cboManufactorType.SelectedValue = "7";
                        }
                        else
                        {
                            cboManufactorType.SelectedValue = "8";
                        }
                    }

                }

                if (isNotGradeType && (!Constants.EMPTY_STRING.Equals(currentTextBox.Text)) && (targetComboBox.SelectedIndex < 0))
                {
                    targetComboBox.SelectedIndex = cboStandardRank.SelectedIndex;
                }

            }
            //else
            //{
            //    targetComboBox.SelectedIndex = cboStandardRank.SelectedIndex;
            //}


        }

        // 加工種別 
        private int calculateManufaturingTime()
        {
            int tempTotal = 0;

            tempTotal = getIntegerValueOfString(txtManuFriesTime.Text)
                + getIntegerValueOfString(txtManuDrillTime.Text)
                + getIntegerValueOfString(txtManuLatheTime.Text)
                + getIntegerValueOfString(txtManuTarepanTime.Text)
                + getIntegerValueOfString(txtManuBendTime.Text)
                + getIntegerValueOfString(txtManuWeldTime.Text)
                + getIntegerValueOfString(txtManuOtherTime.Text)
                + getIntegerValueOfString(txtManuMCTime.Text)
                + getIntegerValueOfString(txtManuPolishTime.Text)
                + getIntegerValueOfString(txtManuAdhesionTime.Text);


            return tempTotal;

        }

        private void txtManuFriesTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuFriesRank);
        }

        private void txtManuDrillTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuDrillRank);
        }

        private void txtManuLatheTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuLatheRank);
        }

        private void txtManuTarepanTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuTarepanRank);
        }

        private void txtManuBendTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuBendRank);
        }

        private void txtManuWeldTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuWeldRank);
        }

        private void txtManuOtherTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuOtherRank);
        }

        private void txtManuMCTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuMCRank);
        }

        private void txtManuPolishTime_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtManuPolishTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuPolishRank);
        }

        private void txtManuAdhesionTime_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            this.controlManufactoringTypeAndRankType((MaskedTextBox)sender, cboManuAdhesionRank);
        }

        private void grvDrawingList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            //MessageBox.Show("Error happened " + e.Context.ToString());
        }

        private void btnUpdateAssign_Click(object sender, EventArgs e)
        {
            try
            {
                BUS_DrawingInfor drawingInfoBUS = new BUS_DrawingInfor();
                drawingInfoBUS.updateDrawingInfo(getDrawingInfoList());

                MessageBox.Show(this, "Assignment data has been saved successfully!", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error btnUpdateAssign_Click(): " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void btnUpdateAssign_Click(object sender, EventArgs e)
        //{
        //    if ((CommonsVars.USER_ROLE == Constants.USER_ROLE_OPERATOR))
        //    {

        //        MessageBox.Show(this, "You don't have permission Update Assignment", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);

        //    }
        //    else
        //    {
        //        BUS_DrawingInfor drawingInfoBUS = new BUS_DrawingInfor();
        //        drawingInfoBUS.updateBatchDrawingInfo(getDrawingInfoList());
        //        MessageBox.Show(this, "Assignment data has been saved successfully!", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }

        //}

        private void btnUpdateDrawingProgress_Click(object sender, EventArgs e)
        {
            try
            {
                BUS_DrawingInfor drawingInfoBUS = new BUS_DrawingInfor();
                drawingInfoBUS.updateDrawingInfo(getDrawingInfoList());

                MessageBox.Show(this, "Drawing progress data has been saved successfully!", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error btnUpdateDrawingProgress_Click(): " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<DTO_DrawingInfor> getDrawingInfoList()
        {
            List<DTO_DrawingInfor> drawingInfoList = new List<DTO_DrawingInfor>();
            foreach (DataGridViewRow row in grvDrawingList.Rows)
            {
                DTO_DrawingInfor drawingInfo = new DTO_DrawingInfor(0, row.Cells[1].Value + "", this.inputInfoId, row.Cells[3].Value + "", "Update assignment", row.Cells[2].Value + "", DateTime.Now, row.Cells[5].Value + "");

                drawingInfoList.Add(drawingInfo);
            }
            return drawingInfoList;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.saveCurrentDataPinkColor();
            this.saveCurrentDataCheckbox();
            this.saveCurrentData();
            this.completeDrawingData(Constants.PROGRESS_DOING);
            this.updateViewDrawingStatusOnDrawingList(Constants.PROGRESS_DOING);
        }

        private void grvMaterial_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if ((grvMaterial.CurrentCell != null) && (grvMaterial.CurrentCell.RowIndex >= 0) && ((grvMaterial.CurrentCell.ColumnIndex == 2) || (grvMaterial.CurrentCell.ColumnIndex >= 4)))
            {
                columnTextBox = e.Control as TextBox;
                if (columnTextBox != null)
                {
                    columnTextBox.KeyPress += TextBox_KeyPress_DecimalAccept;
                }
            }
            //==========================================
            // duy khanh add new event SelectionChangeCommitted
            if (e.Control.GetType() == typeof(DataGridViewComboBoxEditingControl))
            {
                ComboBox cmb = (ComboBox)e.Control;
                cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                cmb.SelectionChangeCommitted -= new EventHandler(grv_cmb_SelectionChangeCommitted);
                cmb.SelectionChangeCommitted += new EventHandler(grv_cmb_SelectionChangeCommitted);
            }
        }

        private void chkManuFries_CheckedChanged(object sender, EventArgs e)
        {
            txtManuFriesTime.Enabled = chkManuFries.Checked;
            cboManuFriesRank.Enabled = chkManuFries.Checked;
            //txtManuFriesTime.Text = chkManuFries.Checked ? txtManuFriesTime.Text : Constants.EMPTY_STRING;
            txtManuFriesTime.Text = chkManuFries.Checked ? txtManuFriesTime.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            cboManuFriesRank.SelectedIndex = chkManuFries.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuFriesRank.BackColor = (chkManuFries.Checked && (cboManuFriesRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuFriesTime.BackColor = (chkManuFries.Checked && isEmptyOrZero(txtManuFriesTime.Text)) ? Color.Yellow : Color.White;
        }

        private void chkManuDrill_CheckedChanged(object sender, EventArgs e)
        {
            txtManuDrillTime.Enabled = chkManuDrill.Checked;
            cboManuDrillRank.Enabled = chkManuDrill.Checked;
            // txtManuDrillTime.Text = chkManuDrill.Checked ? txtManuDrillTime.Text : Constants.EMPTY_STRING;
            txtManuDrillTime.Text = chkManuDrill.Checked ? txtManuDrillTime.Text : Constants.ZERO_STRING;
            cboManuDrillRank.SelectedIndex = chkManuDrill.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuDrillRank.BackColor = (chkManuDrill.Checked && (cboManuDrillRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuDrillTime.BackColor = (chkManuDrill.Checked && isEmptyOrZero(txtManuDrillTime.Text)) ? Color.Yellow : Color.White;
        }

        private void chkManuLathe_CheckedChanged(object sender, EventArgs e)
        {
            txtManuLatheTime.Enabled = chkManuLathe.Checked;
            cboManuLatheRank.Enabled = chkManuLathe.Checked;
            //txtManuLatheTime.Text = chkManuLathe.Checked ? txtManuLatheTime.Text : Constants.EMPTY_STRING;
            txtManuLatheTime.Text = chkManuLathe.Checked ? txtManuLatheTime.Text : Constants.ZERO_STRING;
            cboManuLatheRank.SelectedIndex = chkManuLathe.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuLatheRank.BackColor = (chkManuLathe.Checked && (cboManuLatheRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuLatheTime.BackColor = (chkManuLathe.Checked && isEmptyOrZero(txtManuLatheTime.Text)) ? Color.Yellow : Color.White;
        }

        private void chkManuTarepan_CheckedChanged(object sender, EventArgs e)
        {
            txtManuTarepanTime.Enabled = chkManuTarepan.Checked;
            cboManuTarepanRank.Enabled = chkManuTarepan.Checked;
            //txtManuTarepanTime.Text = chkManuTarepan.Checked ? txtManuTarepanTime.Text : Constants.EMPTY_STRING;
            txtManuTarepanTime.Text = chkManuTarepan.Checked ? txtManuTarepanTime.Text : Constants.ZERO_STRING;
            cboManuTarepanRank.SelectedIndex = chkManuTarepan.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuTarepanRank.BackColor = (chkManuTarepan.Checked && (cboManuTarepanRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuTarepanTime.BackColor = (chkManuTarepan.Checked && isEmptyOrZero(txtManuTarepanTime.Text)) ? Color.Yellow : Color.White;

            // Require Edit from Nam.Nv 10/12/2019
            this.SetComboboxRankStandard();

            if (bIsFormLoaded && chkManuTarepan.Checked)
            {
                cboGradeType.BackColor = cboGradeType.SelectedIndex >= 0 ? Color.White : Color.Yellow;
                // Check when 1kyu
                if ((cboGradeType.SelectedIndex == 0) && (isEmptyOrZero(txtManuPreparationTime.Text)))
                {
                    txtManuPreparationTime.BackColor = Color.Yellow;
                }
                //else
                //{
                //    txtManuPreparationTime.BackColor = Color.White;
                //}
            }
            else
            {
                cboGradeType.BackColor = Color.White;
                //txtManuPreparationTime.BackColor = Color.White;
                highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            }
        }

        private void chkManuBend_CheckedChanged(object sender, EventArgs e)
        {
            txtManuBendTime.Enabled = chkManuBend.Checked;
            cboManuBendRank.Enabled = chkManuBend.Checked;
            //txtManuBendTime.Text = chkManuBend.Checked ? txtManuBendTime.Text : Constants.EMPTY_STRING;
            txtManuBendTime.Text = chkManuBend.Checked ? txtManuBendTime.Text : Constants.ZERO_STRING;
            cboManuBendRank.SelectedIndex = chkManuBend.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuBendRank.BackColor = (chkManuBend.Checked && (cboManuBendRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuBendTime.BackColor = (chkManuBend.Checked && isEmptyOrZero(txtManuBendTime.Text)) ? Color.Yellow : Color.White;
        }

        private void chkManuWeld_CheckedChanged(object sender, EventArgs e)
        {
            txtManuWeldTime.Enabled = chkManuWeld.Checked;
            cboManuWeldRank.Enabled = chkManuWeld.Checked;
            //txtManuWeldTime.Text = chkManuWeld.Checked ? txtManuWeldTime.Text : Constants.EMPTY_STRING;
            txtManuWeldTime.Text = chkManuWeld.Checked ? txtManuWeldTime.Text : Constants.ZERO_STRING;
            cboManuWeldRank.SelectedIndex = chkManuWeld.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuWeldRank.BackColor = (chkManuWeld.Checked && (cboManuWeldRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuWeldTime.BackColor = (chkManuWeld.Checked && isEmptyOrZero(txtManuWeldTime.Text)) ? Color.Yellow : Color.White;
            if (bIsFormLoaded)
            {
                this.fillSurefaceProcessAuto();
            }
        }

        private void chkManuOther_CheckedChanged(object sender, EventArgs e)
        {
            txtManuOtherTime.Enabled = chkManuOther.Checked;
            cboManuOtherRank.Enabled = chkManuOther.Checked;
            //txtManuOtherTime.Text = chkManuOther.Checked ? txtManuOtherTime.Text : Constants.EMPTY_STRING;
            txtManuOtherTime.Text = chkManuOther.Checked ? txtManuOtherTime.Text : Constants.ZERO_STRING;
            cboManuOtherRank.SelectedIndex = chkManuOther.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuOtherRank.BackColor = (chkManuOther.Checked && (cboManuOtherRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuOtherTime.BackColor = (chkManuOther.Checked && isEmptyOrZero(txtManuOtherTime.Text)) ? Color.Yellow : Color.White;
        }

        private void chkManuMC_CheckedChanged(object sender, EventArgs e)
        {
            txtManuMCTime.Enabled = chkManuMC.Checked;
            cboManuMCRank.Enabled = chkManuMC.Checked;
            // txtManuMCTime.Text = chkManuMC.Checked ? txtManuMCTime.Text : Constants.EMPTY_STRING;
            txtManuMCTime.Text = chkManuMC.Checked ? txtManuMCTime.Text : Constants.ZERO_STRING;
            cboManuMCRank.SelectedIndex = chkManuMC.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuMCRank.BackColor = (chkManuMC.Checked && (cboManuMCRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuMCTime.BackColor = (chkManuMC.Checked && isEmptyOrZero(txtManuMCTime.Text)) ? Color.Yellow : Color.White;
        }

        private void chkManuPolish_CheckedChanged(object sender, EventArgs e)
        {
            txtManuPolishTime.Enabled = chkManuPolish.Checked;
            cboManuPolishRank.Enabled = chkManuPolish.Checked;
            //txtManuPolishTime.Text = chkManuPolish.Checked ? txtManuPolishTime.Text : Constants.EMPTY_STRING;
            txtManuPolishTime.Text = chkManuPolish.Checked ? txtManuPolishTime.Text : Constants.ZERO_STRING;
            cboManuPolishRank.SelectedIndex = chkManuPolish.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuPolishRank.BackColor = (chkManuPolish.Checked && (cboManuPolishRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuPolishTime.BackColor = (chkManuPolish.Checked && isEmptyOrZero(txtManuPolishTime.Text)) ? Color.Yellow : Color.White;
        }

        private void chkManuAdhesion_CheckedChanged(object sender, EventArgs e)
        {
            txtManuAdhesionTime.Enabled = chkManuAdhesion.Checked;
            cboManuAdhesionRank.Enabled = chkManuAdhesion.Checked;
            //txtManuAdhesionTime.Text = chkManuAdhesion.Checked ? txtManuAdhesionTime.Text : Constants.EMPTY_STRING;
            txtManuAdhesionTime.Text = chkManuAdhesion.Checked ? txtManuAdhesionTime.Text : Constants.ZERO_STRING;
            cboManuAdhesionRank.SelectedIndex = chkManuAdhesion.Checked ? cboStandardRank.SelectedIndex : -1;
            cboManuAdhesionRank.BackColor = (chkManuAdhesion.Checked && (cboManuAdhesionRank.SelectedIndex < 0)) ? Color.Yellow : Color.White;
            txtManuAdhesionTime.BackColor = (chkManuAdhesion.Checked && isEmptyOrZero(txtManuAdhesionTime.Text)) ? Color.Yellow : Color.White;
        }

        private void fillSurefaceProcessAuto()
        {
            foreach (DataGridViewRow row in grvMaterial.Rows)
            {
                DataGridViewComboBoxCell cboCell = (DataGridViewComboBoxCell)row.Cells[1];
                string cboText = cboCell.FormattedValue.ToString();
                string cboValue = cboCell.Value + "";

                #region 材質種別１「ステンレス」+溶接　→　表面処理「108」
                if (cboText.Contains("SUS") && (chkManuWeld.Checked) && (!this.isExistSurefaceProcessCode("108")))
                {
                    this.setValueToTopSurefaceProcessComboBox("108");
                }
                #endregion 材質種別１「ステンレス」+溶接　→　表面処理「108」

                #region 材質「180」又は「151」+溶接　→　表面処理「108」「008」
                if ((cboValue.Equals("180") || cboValue.Equals("109") || cboValue.Equals("174") || cboValue.Equals("214") || cboValue.Equals("151")) && (chkManuWeld.Checked))
                {
                    if (!this.isExistSurefaceProcessCode("108"))
                    {
                        this.setValueToTopSurefaceProcessComboBox("108");
                    }
                    if (!this.isExistSurefaceProcessCode("008"))
                    {
                        this.setValueToTopSurefaceProcessComboBox("008");
                    }

                }

                //if (cboText.Contains("SUS") && (!chkManuWeld.Checked) && (this.isExistSurefaceProcessCode("108")))
                //{
                //    this.clearValueToTopSurefaceProcessComboBox("108");
                //}

                //fixbug12
                //if ((cboValue.Equals("180") || cboValue.Equals("109") || cboValue.Equals("174") || cboValue.Equals("214") || cboValue.Equals("151")) && (!chkManuWeld.Checked))
                //{
                //    this.clearValueToTopSurefaceProcessComboBox("108");
                //    this.clearValueToTopSurefaceProcessComboBox("008");
                //}



                #endregion 材質「180」又は「151」+溶接　→　表面処理「108」「008」

                #region 材質種別2「黒皮」 +メッキ
                // TODO: confirm with Kha for using Contains or EndWith; 並仕上げ??
                if ((cboValue.Equals("002") || cboValue.Equals("019") || cboValue.Equals("011") || cboValue.Equals("013") || cboValue.Equals("024")
                    || cboValue.Equals("074") || cboValue.Equals("144") || cboValue.Equals("124") || cboValue.Equals("130") || cboValue.Equals("097")
                     || cboValue.Equals("017") || cboValue.Equals("016") || cboValue.Equals("013"))
                    && (!this.isExistSurefaceProcessCode("132"))
                     && (cboSideFinish.SelectedIndex == 1)
                     && (this.isExistAtLeastSurefaceProcess())) // 2018/09/18 Add condition
                {
                    this.setValueToTopSurefaceProcessComboBox("132");
                }
                #endregion 材質種別2「黒皮」 +メッキ

            }
        }

        private bool isExistSurefaceProcessCode(string strValue)
        {
            return (((cboSurfaceProcess1.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess1.SelectedValue))) || ((cboSurfaceProcess2.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess2.SelectedValue)))
                    || ((cboSurfaceProcess3.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess3.SelectedValue))) || ((cboSurfaceProcess4.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess4.SelectedValue)))
                     || ((cboSurfaceProcess5.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess5.SelectedValue))) || ((cboSurfaceProcess6.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess6.SelectedValue)))
                     || ((cboSurfaceProcess7.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess7.SelectedValue))) || ((cboSurfaceProcess8.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess8.SelectedValue)))
                     || ((cboSurfaceProcess9.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess9.SelectedValue))) || ((cboSurfaceProcess10.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess10.SelectedValue))));
        }

        private bool isExistAtLeastSurefaceProcess()
        {
            return (((cboSurfaceProcess1.SelectedIndex >= 0)) || ((cboSurfaceProcess2.SelectedIndex >= 0))
                    || ((cboSurfaceProcess3.SelectedIndex >= 0)) || ((cboSurfaceProcess4.SelectedIndex >= 0))
                     || ((cboSurfaceProcess5.SelectedIndex >= 0)) || ((cboSurfaceProcess6.SelectedIndex >= 0))
                     || ((cboSurfaceProcess7.SelectedIndex >= 0)) || ((cboSurfaceProcess8.SelectedIndex >= 0))
                     || ((cboSurfaceProcess9.SelectedIndex >= 0)) || ((cboSurfaceProcess10.SelectedIndex >= 0)));
        }

        private void setValueToTopSurefaceProcessComboBox(string strValue)
        {
            if (cboSurfaceProcess1.SelectedIndex < 0)
            {
                cboSurfaceProcess1.SelectedValue = strValue;
            }
            else if (cboSurfaceProcess2.SelectedIndex < 0)
            {
                cboSurfaceProcess2.SelectedValue = strValue;
            }
            else if (cboSurfaceProcess3.SelectedIndex < 0)
            {
                cboSurfaceProcess3.SelectedValue = strValue;
            }
            else if (cboSurfaceProcess4.SelectedIndex < 0)
            {
                cboSurfaceProcess4.SelectedValue = strValue;
            }
            else if (cboSurfaceProcess5.SelectedIndex < 0)
            {
                cboSurfaceProcess5.SelectedValue = strValue;
            }
            else if (cboSurfaceProcess6.SelectedIndex < 0)
            {
                cboSurfaceProcess6.SelectedValue = strValue;
            }
            else if (cboSurfaceProcess7.SelectedIndex < 0)
            {
                cboSurfaceProcess7.SelectedValue = strValue;
            }
            else if (cboSurfaceProcess8.SelectedIndex < 0)
            {
                cboSurfaceProcess8.SelectedValue = strValue;
            }
            else if (cboSurfaceProcess9.SelectedIndex < 0)
            {
                cboSurfaceProcess9.SelectedValue = strValue;
            }
            else if (cboSurfaceProcess10.SelectedIndex < 0)
            {
                cboSurfaceProcess10.SelectedValue = strValue;
            }
        }

        private void highlightPairComboBox_SurfaceProcess_Money(ComboBox cboSurface, MaskedTextBox txtMoney, MaskedTextBox txtQuantity)
        {
            if ((!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())))
            {
                bool bIsMoneyItem = isMoneyItem_SurfaceProcess(cboSurface.SelectedValue + "");
                txtMoney.Text = bIsMoneyItem ? txtMoney.Text : Constants.EMPTY_STRING;

                if (int.Parse("0" + txtMoney.Text.Trim()) != 0)
                {
                    cboSurface.BackColor = cboSurface.SelectedIndex < 0 ? Color.Yellow : Color.White;
                    txtQuantity.BackColor = (int.Parse("0" + txtQuantity.Text.Trim()) == 0) ? Color.Yellow : Color.White;
                    txtMoney.BackColor = Color.White;
                }
                else
                {
                    txtMoney.BackColor = bIsMoneyItem && ((cboSurface.SelectedIndex >= 0) || (int.Parse("0" + txtQuantity.Text.Trim()) != 0)) ? Color.YellowGreen : Color.White;
                    cboSurface.BackColor = (cboSurface.SelectedIndex < 0) && (int.Parse("0" + txtQuantity.Text.Trim()) != 0) ? Color.Yellow : Color.White;
                    txtQuantity.BackColor = ((cboSurface.SelectedIndex >= 0) && (int.Parse("0" + txtQuantity.Text.Trim()) == 0)) ? Color.Yellow : Color.White;
                }
            }
            else
            {
                cboSurface.BackColor = Color.White;
                txtMoney.BackColor = Color.White;
                txtQuantity.BackColor = Color.White;
            }
        }

        private void txtSurefaceMoney1_TextChanged_1(object sender, EventArgs e)
        {
            //highlightPairComboBox(cboSurfaceProcess1, txtSurefaceMoney1, txtSurefaceWeight1);
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess1, txtSurefaceMoney1, txtSurefaceWeight1);
        }

        private void txtSurefaceMoney2_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess2, txtSurefaceMoney2, txtSurefaceWeight2);
        }

        private void txtSurefaceMoney3_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess3, txtSurefaceMoney3, txtSurefaceWeight3);
        }

        private void txtSurefaceMoney4_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess4, txtSurefaceMoney4, txtSurefaceWeight4);
        }

        private void txtSurefaceMoney5_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess5, txtSurefaceMoney5, txtSurefaceWeight5);
        }

        private void txtSurefaceMoney6_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess6, txtSurefaceMoney6, txtSurefaceWeight6);
        }

        private void txtSurefaceMoney7_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess7, txtSurefaceMoney7, txtSurefaceWeight7);
        }

        private void txtSurefaceMoney8_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess8, txtSurefaceMoney8, txtSurefaceWeight8);
        }

        private void txtSurefaceMoney9_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess9, txtSurefaceMoney9, txtSurefaceWeight9);
        }

        private void txtSurefaceMoney10_TextChanged_1(object sender, EventArgs e)
        {
            highlightPairComboBox_SurfaceProcess_Money(cboSurfaceProcess10, txtSurefaceMoney10, txtSurefaceWeight10);
        }

        private void resetInputItems()
        {
            btnSave.Enabled = false;
            btnComplete.Enabled = false;
            btnCompleteChecking.Enabled = false;

            // Material
            txtDrawingCode.Text = Constants.EMPTY_STRING;
            txtOldSubCode.Text = Constants.EMPTY_STRING;
            cboManuDivision.SelectedIndex = -1;
            cboSideFinish.SelectedIndex = -1;
            grvMaterial.RowCount = 1;

            // Parts
            txtManuMessage.Text = Constants.EMPTY_STRING;
            txtSurefaceMessage.Text = Constants.EMPTY_STRING;
            grvBuyMaterial.RowCount = 0;

            cboManufactorType.SelectedIndex = -1;
            cboStandardRank.SelectedIndex = -1;
            cboGradeType.SelectedIndex = -1;
            txtRadiusBendEffort.Text = Constants.EMPTY_STRING;

            chkManuFries.Checked = false;
            txtManuFriesTime.Text = Constants.EMPTY_STRING;
            cboManuFriesRank.SelectedIndex = -1;

            chkManuDrill.Checked = false;
            txtManuDrillTime.Text = Constants.EMPTY_STRING;
            cboManuDrillRank.SelectedIndex = -1;

            chkManuLathe.Checked = false;
            txtManuLatheTime.Text = Constants.EMPTY_STRING;
            cboManuLatheRank.SelectedIndex = -1;

            chkManuTarepan.Checked = false;
            txtManuTarepanTime.Text = Constants.EMPTY_STRING;
            cboManuTarepanRank.SelectedIndex = -1;

            chkManuBend.Checked = false;
            txtManuBendTime.Text = Constants.EMPTY_STRING;
            cboManuBendRank.SelectedIndex = -1;

            chkManuWeld.Checked = false;
            txtManuWeldTime.Text = Constants.EMPTY_STRING;
            cboManuWeldRank.SelectedIndex = -1;

            chkManuOther.Checked = false;
            txtManuOtherTime.Text = Constants.EMPTY_STRING;
            cboManuOtherRank.SelectedIndex = -1;

            chkManuMC.Checked = false;
            txtManuMCTime.Text = Constants.EMPTY_STRING;
            cboManuMCRank.SelectedIndex = -1;

            chkManuPolish.Checked = false;
            txtManuPolishTime.Text = Constants.EMPTY_STRING;
            cboManuPolishRank.SelectedIndex = -1;

            chkManuAdhesion.Checked = false;
            txtManuAdhesionTime.Text = Constants.EMPTY_STRING;
            cboManuAdhesionRank.SelectedIndex = -1;

            txtManuPreparationTime.BackColor = Color.White;

            cboSurfaceProcess1.SelectedIndex = -1;
            txtSurefaceWeight1.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney1.Text = Constants.EMPTY_STRING;

            cboSurfaceProcess2.SelectedIndex = -1;
            txtSurefaceWeight2.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney2.Text = Constants.EMPTY_STRING;

            cboSurfaceProcess3.SelectedIndex = -1;
            txtSurefaceWeight3.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney3.Text = Constants.EMPTY_STRING;

            cboSurfaceProcess4.SelectedIndex = -1;
            txtSurefaceWeight4.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney4.Text = Constants.EMPTY_STRING;

            cboSurfaceProcess5.SelectedIndex = -1;
            txtSurefaceWeight5.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney5.Text = Constants.EMPTY_STRING;

            cboSurfaceProcess6.SelectedIndex = -1;
            txtSurefaceWeight6.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney6.Text = Constants.EMPTY_STRING;

            cboSurfaceProcess7.SelectedIndex = -1;
            txtSurefaceWeight7.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney7.Text = Constants.EMPTY_STRING;

            cboSurfaceProcess8.SelectedIndex = -1;
            txtSurefaceWeight8.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney8.Text = Constants.EMPTY_STRING;

            cboSurfaceProcess9.SelectedIndex = -1;
            txtSurefaceWeight9.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney9.Text = Constants.EMPTY_STRING;

            cboSurfaceProcess10.SelectedIndex = -1;
            txtSurefaceWeight10.Text = Constants.EMPTY_STRING;
            txtSurefaceMoney10.Text = Constants.EMPTY_STRING;

            cboPaintDivision1.SelectedIndex = 0;
            txtPaintSquare1.Text = Constants.EMPTY_STRING;

            cboPaintDivision2.SelectedIndex = 0;
            txtPaintSquare2.Text = Constants.EMPTY_STRING;

            // List checkbox2
            chk1.Checked = false;
            txtManuToothCuttingCost.Text = Constants.EMPTY_STRING;

            chk2.Checked = false;
            txtManuQuenchingCost.Text = Constants.EMPTY_STRING;

            chk3.Checked = false;
            txtManuPreparationTime.Text = Constants.EMPTY_STRING;

            chk4.Checked = false;
            txtManuPreparationStamp.Text = Constants.EMPTY_STRING;

            chk5.Checked = false;
            txtManuPreparationColorCheck.Text = Constants.EMPTY_STRING;

            chk6.Checked = false;
            txtManuKeyCost.Text = Constants.EMPTY_STRING;

            chk7.Checked = false;
            txtManuTapeTime.Text = Constants.EMPTY_STRING;

            chk8.Checked = false;
            txtManuEviPerDrawing.Text = Constants.EMPTY_STRING;

            chk9.Checked = false;
            txtManuEviOneDrawing.Text = Constants.EMPTY_STRING;

            chk10.Checked = false;
            txtManuScrewTime.Text = Constants.EMPTY_STRING;
            // end
        }

        private void grvMaterial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            grvMaterial.CurrentCell.ToolTipText = "aaaaaaaaaaa";
            if (!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim()))
            {
                if (grvMaterial.CurrentCell != null)
                {
                    if (grvMaterial.CurrentCell.ColumnIndex == 12)
                    {
                        if ((grvMaterial.CurrentCell.RowIndex > 0) && (grvMaterial.CurrentCell.RowIndex < grvMaterial.Rows.Count - 1))
                        {
                            if (MessageBox.Show(this, "Are you sure you want to delete Material [" + grvMaterial[1, grvMaterial.CurrentCell.RowIndex].Value + "]?", CommonsVars.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                grvMaterial.Rows.RemoveAt(grvMaterial.CurrentCell.RowIndex);
                                for (int i = 1; i < grvMaterial.Rows.Count - 1; i++)
                                {
                                    grvMaterial[0, i - 1].Value = i.ToString();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Can not delete the first material! Each part needs at least one material.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
        }

        private void grvDrawingList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            #region Duy Khanh add code 
            this.CheckShapeGridView();
            this.loadDataCheckbox();
            this.loadDataLightPinkColor();
            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (btnSave.Enabled && MessageBox.Show(this, "Drawing [" + grvDrawingList[1, grvDrawingList.CurrentRow.Index].Value + "] has been change." + Environment.NewLine + "Do you want to save before closing?", CommonsVars.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.saveCurrentData();
            }

            this.Close();
        }

        private void updateViewDrawingStatusOnDrawingList(string progressInfo)
        {
            if ((grvDrawingList.CurrentRow != null) && (grvDrawingList.CurrentRow.Index >= 0))
            {
                grvDrawingList[2, grvDrawingList.CurrentRow.Index].Value = progressInfo;

                if (Constants.PROGRESS_DONE.Equals(progressInfo))
                {
                    grvDrawingList[2, grvDrawingList.CurrentRow.Index].Style.BackColor = Color.Yellow;
                }
                else if (Constants.PROGRESS_CHECKED.Equals(progressInfo))
                {
                    grvDrawingList[2, grvDrawingList.CurrentRow.Index].Style.BackColor = Color.White;
                }
                else if (Constants.PROGRESS_DOING.Equals(progressInfo))
                {
                    grvDrawingList[2, grvDrawingList.CurrentRow.Index].Style.BackColor = Color.Red;
                }
                else
                {
                    grvDrawingList[2, grvDrawingList.CurrentRow.Index].Style.BackColor = Color.Gray;
                }
            }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            if ((grvDrawingList.CurrentRow != null) && (grvDrawingList.CurrentRow.Index >= 0))
            {
                try
                {
                    if (btnSave.Enabled)
                    {
                        this.saveCurrentData();
                    }
                    bIsAllControlAreValidBeforeSaving = true;
                    if (AreAllDataValid(this))
                    {
                        completeDrawingData(Constants.PROGRESS_DONE);
                        this.updateViewDrawingStatusOnDrawingList(Constants.PROGRESS_DONE);
                        MessageBox.Show(this, "Drawing data has been saved successfully!", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error btnComplete_Click(): " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void completeDrawingData(string progressInfo)
        {
            List<DTO_DrawingInfor> drawingInfoList = new List<DTO_DrawingInfor>();
            DataGridViewRow row = grvDrawingList.CurrentRow;
            DTO_DrawingInfor drawingInfo = new DTO_DrawingInfor(0, row.Cells[1].Value + "", this.inputInfoId, row.Cells[3].Value + "", "Completed", progressInfo, DateTime.Now, row.Cells[5].Value + "");
            drawingInfoList.Add(drawingInfo);

            BUS_DrawingInfor drawingInfoBUS = new BUS_DrawingInfor();
            drawingInfoBUS.updateDrawingInfo(drawingInfoList);
        }

        private bool AreAllDataValid(Control ctl)
        {
            foreach (Control child in ctl.Controls)
            {
                if (((child is TextBox) || (child is MaskedTextBox) || (child is ComboBox)) && (child.BackColor == Color.Yellow))
                {
                    child.Focus();
                    MessageBox.Show(this, "An(some) item(s) with yellow backcolor required data! Please check before completing.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    bIsAllControlAreValidBeforeSaving = false;
                    return false;
                }
                else if (child.Name.Equals("grvMaterial"))
                {
                    foreach (DataGridViewRow row in grvMaterial.Rows)
                    {
                        for (int i = 4; i <= 11; i++)
                        {
                            if (row.Cells[i].Style.BackColor == Color.Yellow)
                            {
                                MessageBox.Show(this, "An(some) item(s) of Material with yellow backcolor required data! Please check before completing.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                row.Cells[i].Selected = true;
                                bIsAllControlAreValidBeforeSaving = false;
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    if (bIsAllControlAreValidBeforeSaving) AreAllDataValid(child);
                }
            }
            return bIsAllControlAreValidBeforeSaving;
        }

        private void grvDrawingList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if ((CommonsVars.USER_ROLE == Constants.USER_ROLE_OPERATOR))
            {
                MessageBox.Show(this, "You don't have permission Update Assignment", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                if ((grvDrawingList.CurrentCell != null) && (grvDrawingList.CurrentCell.ColumnIndex == 3) && (grvDrawingList.CurrentCell.RowIndex > 0))
                {
                    if (!Constants.EMPTY_STRING.Equals(grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex - 1].Value + ""))
                    {
                        if (Constants.EMPTY_STRING.Equals(grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex].Value + ""))
                        {
                            grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex].Value = grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex - 1].Value;
                        }
                        else
                        {
                            if ((!(grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex].Value + "").Equals(grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex - 1].Value + ""))
                                && (MessageBox.Show(this, "Current drawing has been assigned to [" + grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex].Value + "]." +
                                "Do you want to change to [" + grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex - 1].Value + "]?" + Environment.NewLine + "Do you want to save before closing?", CommonsVars.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
                            {
                                grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex].Value = grvDrawingList[3, grvDrawingList.CurrentCell.RowIndex - 1].Value;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Above drawing has no assingment data!" + Environment.NewLine + "Program can only copy assignment information from above drawing to current drawing.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            chkManuFries.Checked = false;
            chkManuDrill.Checked = false;
            chkManuLathe.Checked = false;
            chkManuTarepan.Checked = false;
            chkManuBend.Checked = false;
            chkManuWeld.Checked = false;
            chkManuOther.Checked = false;
            chkManuMC.Checked = false;
            chkManuPolish.Checked = false;
            chkManuAdhesion.Checked = false;

            txtManuToothCuttingCost.ResetText();
            txtManuQuenchingCost.ResetText();
            txtManuPreparationTime.ResetText();
            txtManuPreparationStamp.ResetText();
            txtManuPreparationColorCheck.ResetText();
            txtManuKeyCost.ResetText();
            txtManuTapeTime.ResetText();
            txtManuEviPerDrawing.ResetText();
            txtManuEviOneDrawing.ResetText();
            txtManuScrewTime.ResetText();

            chk1.Checked = false;
            chk2.Checked = false;
            chk3.Checked = false;
            chk4.Checked = false;
            chk5.Checked = false;
            chk6.Checked = false;
            chk7.Checked = false;
            chk8.Checked = false;
            chk9.Checked = false;
            chk10.Checked = false;
            txtManuToothCuttingCost.ResetText();
            txtManuQuenchingCost.ResetText();
            txtManuPreparationTime.ResetText();
            txtManuPreparationStamp.ResetText();
            txtManuPreparationColorCheck.ResetText();
            txtManuKeyCost.ResetText();
            txtManuTapeTime.ResetText();
            txtManuTapeTime.BackColor = System.Drawing.Color.White;
            txtManuEviPerDrawing.ResetText();
            txtManuEviOneDrawing.ResetText();
            txtManuScrewTime.ResetText();
        }

        private void cboGradeType_TextChanged(object sender, EventArgs e)
        {
            findIndexByText((ComboBox)sender);
            if (chkManuTarepan.Checked)
            {
                if (cboGradeType.SelectedIndex == 0)
                {
                    cboGradeType.BackColor = Color.White;
                    if (int.Parse("0" + txtManuPreparationTime.Text.Trim()) == 0)
                    {
                        txtManuPreparationTime.BackColor = Color.Yellow;
                    }
                    controlManufactoringTypeAndRankType(null, null, false);
                }
                else if (cboGradeType.SelectedIndex > 0)
                {
                    cboGradeType.BackColor = Color.White;
                    //txtManuPreparationTime.BackColor = Color.White;
                    controlManufactoringTypeAndRankType(null, null, false);
                    highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
                }
                else
                {
                    cboGradeType.BackColor = Color.Yellow;
                    highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
                }
            }
            else
            {
                cboGradeType.BackColor = Color.White;
                //txtManuPreparationTime.BackColor = Color.White;
                highlightPairTextBoxByComboValue(txtManuPreparationTime, this.surfaceSpecialList);
            }
            ChangeSaveButtonStatus(true);
        }


        private void grvBuyMaterial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim()))
            {
                if (grvBuyMaterial.CurrentCell != null)
                {
                    if (grvBuyMaterial.CurrentCell.ColumnIndex == 7)
                    {
                        if ((grvBuyMaterial.CurrentCell.RowIndex >= 0))
                        {
                            int currentIndex = grvBuyMaterial.CurrentCell.RowIndex;
                            if (Constants.EMPTY_STRING.Equals(grvBuyMaterial[1, currentIndex].Value + "")
                                && Constants.EMPTY_STRING.Equals(grvBuyMaterial[2, currentIndex].Value + "")
                                && Constants.EMPTY_STRING.Equals(grvBuyMaterial[3, currentIndex].Value + "")
                                && Constants.EMPTY_STRING.Equals(grvBuyMaterial[4, currentIndex].Value + "")
                                && Constants.EMPTY_STRING.Equals(grvBuyMaterial[5, currentIndex].Value + "")
                                && Constants.EMPTY_STRING.Equals(grvBuyMaterial[6, currentIndex].Value + ""))//fixbug cho phep nhap khi gia khac 1: 2018/12/19
                            {
                                grvBuyMaterial.Rows.RemoveAt(currentIndex);
                                grvBuyMaterial.Rows.Add();
                                setIndexOfBuyMaterialGrid();
                            }
                            else if (MessageBox.Show(this, "Are you sure you want to delete this record?", CommonsVars.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                grvBuyMaterial.Rows.RemoveAt(currentIndex);
                                grvBuyMaterial.Rows.Add();
                                setIndexOfBuyMaterialGrid();
                            }
                        }
                    }
                }
            }
        }

        private void setIndexOfBuyMaterialGrid()
        {
            for (int i = 1; i < grvBuyMaterial.Rows.Count - 1; i++)
            {
                grvBuyMaterial[0, i - 1].Value = i.ToString();
            }
        }

        private void clearEmptyBuyingMaterials()
        {
            if (!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim()))
            {
                if ((grvBuyMaterial.CurrentCell.RowIndex > 0))
                {
                    for (int i = grvBuyMaterial.RowCount - 1; i >= 0; i--)
                    {
                        if ((Constants.EMPTY_STRING.Equals(grvBuyMaterial[1, i].Value + "")
                            && Constants.EMPTY_STRING.Equals(grvBuyMaterial[2, i].Value + "")
                            && Constants.EMPTY_STRING.Equals(grvBuyMaterial[3, i].Value + "")
                            && Constants.EMPTY_STRING.Equals(grvBuyMaterial[4, i].Value + "")
                            && Constants.EMPTY_STRING.Equals(grvBuyMaterial[5, i].Value + "")
                            && Constants.EMPTY_STRING.Equals(grvBuyMaterial[6, i].Value + "")))//fixbug cho phep nhap khi gia khac 1: 2018/12/19
                        {
                            grvBuyMaterial.Rows.RemoveAt(i);
                            grvBuyMaterial.Rows.Add();
                            grvBuyMaterial[0, i].Value = (i + 1).ToString();
                        }
                    }
                }

            }
        }

        private void btnCompleteChecking_Click(object sender, EventArgs e)
        {
            if ((grvDrawingList.CurrentRow != null) && (grvDrawingList.CurrentRow.Index >= 0))
            {
                try
                {
                    if (btnSave.Enabled)
                    {
                        this.saveCurrentData();
                    }
                    bIsAllControlAreValidBeforeSaving = true;
                    if (AreAllDataValid(this))
                    {
                        //this.saveCurrentData();
                        completeDrawingData(Constants.PROGRESS_CHECKED);
                        this.updateViewDrawingStatusOnDrawingList(Constants.PROGRESS_CHECKED);
                        MessageBox.Show(this, "Drawing data has been saved successfully!", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error btnCompleteChecking_Click(): " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //if ((grvDrawingList.CurrentRow != null) && (grvDrawingList.CurrentRow.Index >= 0))
            //{
            //    try
            //    {
            //        if (btnSave.Enabled)
            //        {
            //            this.saveCurrentData();
            //        }
            //        bIsAllControlAreValidBeforeSaving = true;
            //        if (AreAllDataValid(this))
            //        {
            //            completeDrawingData(Constants.PROGRESS_CHECKED);
            //            this.updateViewDrawingStatusOnDrawingList(Constants.PROGRESS_CHECKED);
            //            MessageBox.Show(this, "Drawing data has been saved successfully!", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(this, "Error btnCompleteChecking_Click(): " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        private void frmOperation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.Enabled && MessageBox.Show(this, "Drawing [" + grvDrawingList[1, grvDrawingList.CurrentRow.Index].Value + "] has been change." + Environment.NewLine + "Do you want to save before closing?", CommonsVars.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bIsAllControlAreValidBeforeSaving = true;
                if (AreAllDataValid(this))
                {
                    this.saveCurrentData();
                    this.completeDrawingData(Constants.PROGRESS_DOING);
                }
            }
        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtManuFriesTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuFries, txtManuFriesTime);
        }

        // Function convert String to selected Index
        private int ConvertStringToSelectedIndex(string textString)
        {
            textString = textString.ToUpper();
            switch (textString)
            {
                case null:
                    return -1;
                    break;
                case "A":
                    return 0;
                    break;
                case "B":
                    return 1;
                    break;
                case "C":
                    return 2;
                    break;
                case "D":
                    return 3;
                    break;
                case "E":
                    return 4;
                    break;
                case "F":
                    return 5;
                    break;
                case "G":
                    return 6;
                    break;

                default:
                    return -1;
                    break;
            }
        }


        private void cboStandardRank_TextChanged(object sender, EventArgs e)
        {

            //this.SetComboboxRank();
            if (bIsFormLoaded)
            {
                findIndexByText((ComboBox)sender);
                if (cboStandardRank.SelectedIndex >= 0)
                {
                    int index = -1;
                    if (cboStandardRank.SelectedItem.ToString() != null)
                    {
                        index = ConvertStringToSelectedIndex(getSelectedTextCombobox(cboStandardRank.SelectedItem.ToString()));
                    }
                    else
                    {
                        index = -1;
                    }
                    if (chkManuFries.Checked)
                    {
                        cboManuFriesRank.SelectedIndex = index;
                    }
                    if (chkManuDrill.Checked)
                    {
                        cboManuDrillRank.SelectedIndex = index;
                    }
                    if (chkManuLathe.Checked)
                    {
                        cboManuLatheRank.SelectedIndex = index;
                    }
                    if (chkManuTarepan.Checked)
                    {
                        cboManuTarepanRank.SelectedIndex = index;
                    }
                    if (chkManuBend.Checked)
                    {
                        cboManuBendRank.SelectedIndex = index;
                    }
                    if (chkManuWeld.Checked)
                    {
                        cboManuWeldRank.SelectedIndex = index;
                    }
                    if (chkManuOther.Checked)
                    {
                        cboManuOtherRank.SelectedIndex = index;
                    }
                    if (chkManuMC.Checked)
                    {
                        cboManuMCRank.SelectedIndex = index;
                    }
                    if (chkManuPolish.Checked)
                    {
                        cboManuPolishRank.SelectedIndex = index;
                    }
                    if (chkManuAdhesion.Checked)
                    {
                        cboManuAdhesionRank.SelectedIndex = index;
                    }
                }
                cboStandardRank.BackColor = cboStandardRank.SelectedIndex >= 0 ? Color.White : Color.Yellow;
            }
        }

        private void txtRadiusBendEffort_TextChanged(object sender, EventArgs e)
        {
            controlRelationOfBendAndTarepanTime((MaskedTextBox)sender);
            controlManufactoringTypeAndRankType(null, null, false);
            ChangeSaveButtonStatus(true);
        }

        private void controlRelationOfBendAndTarepanTime(MaskedTextBox currentTextBox)
        {
            if (bIsFormLoaded && (!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text)) && (!Constants.EMPTY_STRING.Equals(currentTextBox.Text.Trim())))
            {
                int radiusBendTime = int.Parse("0" + txtRadiusBendEffort.Text.Trim());
                int bendAndTarepanTime = int.Parse("0" + txtManuBendTime.Text.Trim()) + int.Parse("0" + txtManuTarepanTime.Text.Trim());

                if ((radiusBendTime >= bendAndTarepanTime))
                {
                    MessageBox.Show(this, "[" + label12.Text + "] must be less than [" + chkManuTarepan.Text + "] + [" + chkManuBend.Text + "]!", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    if (currentTextBox.Name.Equals("txtRadiusBendEffort"))
                    {
                        currentTextBox.Text = Constants.EMPTY_STRING;
                    }
                    currentTextBox.Focus();
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (grvDrawingList.RowCount <= 0)
            {
                MessageBox.Show(this, "No drawing data! At least one drawing required before exporting.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (btnSave.Enabled)
            {
                MessageBox.Show(this, "You are updating data! Data must be saved and completed before exporting.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //if (!isAllDrawingCompleted())
            //{
            //    MessageBox.Show(this, "There is (are some) drawing has not been completed yet! All drawing must be completed or checked before exporting.", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            if (Directory.Exists(CommonsVars.LAST_EXPORT_FOLDER))
            {
                exportFolderBrowser.SelectedPath = CommonsVars.LAST_EXPORT_FOLDER;
            }

            if (exportFolderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                string outputFolder = exportFolderBrowser.SelectedPath;
                try
                {
                    #region Duy Khanh add Code
                    //===========Delete Exist File==================
                    if (File.Exists(outputFolder + "\\" + this.txtMaterialFileName.Text))
                    {
                        File.Delete(outputFolder + "\\" + this.txtMaterialFileName.Text);
                    }

                    if (File.Exists(outputFolder + "\\" + this.txtPartFileName.Text))
                    {
                        File.Delete(outputFolder + "\\" + this.txtPartFileName.Text);
                    }
                    #endregion
                    // Output Material
                    BUS_MaterialEst materialBUS = new BUS_MaterialEst();

                    materialBUS.exportPartData(this.inputInfoId, outputFolder + "\\" + this.txtMaterialFileName.Text);

                    // Output Part
                    BUS_PartEst partBUS = new BUS_PartEst();

                    partBUS.exportPartData(this.inputInfoId, outputFolder + "\\" + this.txtPartFileName.Text);
                    //partBUS.exportPartData1(this.inputInfoId, outputFolder + "\\" + this.txtPartFileName.Text);

                    this.exportDrawingProgressInfo(outputFolder + "\\" + (this.txtPartFileName.Text).Substring(0, (this.txtPartFileName.Text).Length - 4) + "_Progress.txt");

                    MessageBox.Show(this, "Completed exporting data!" + Environment.NewLine + "Please check at folder: [" + exportFolderBrowser.SelectedPath + "]", CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //================
                    //DUY KHANH FIX EXPORT 00--00--00
                    this.FixIssue_104_112_115(outputFolder + "\\" + this.txtPartFileName.Text);

                    string[] strArr = this.txtPartFileName.Text.Split(new[] { ".txt" }, System.StringSplitOptions.None);
                    string destFileName = NAME_EXPORT_P_1ST + ".txt";
                    this.Copy_File(outputFolder, outputFolder, this.txtPartFileName.Text, destFileName);

                    string folderDestfileFull = outputFolder + "\\" + destFileName;
                    // Check exist
                    if (File.Exists(folderDestfileFull))
                    {
                        File.Delete(folderDestfileFull);
                        this.Copy_File(outputFolder, outputFolder, this.txtPartFileName.Text, destFileName);

                    }
                    /*------------------------------Fix Export Data and add new file-------------------------------------------*/

                    string[] linesdata = File.ReadAllLines(outputFolder + "\\" + destFileName);
                    if (linesdata.Length > 1)
                    {
                        /*---------------------------------*/
                        List<int> listInt = new List<int>();
                        /*---------------------------------*/
                        for (int i = 0; i < linesdata.Length - 1; ++i)
                        {
                            string stringNumberPrice = linesdata[i].Split(new[] { "VTN,," }, System.StringSplitOptions.None)[1].Split(new[] { "," }, System.StringSplitOptions.None)[0];
                            listInt.Add(this.ConvertStringToInt(stringNumberPrice) ?? 0);
                        }

                        int maxValue = listInt.Max();
                        int maxIndex = listInt.ToList().IndexOf(maxValue);
                        if (maxIndex != 0)
                        {
                            this.InsertAndRemoveLineIntoFile(folderDestfileFull, linesdata[maxIndex], 1, maxIndex);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error: " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        /*------------------------------------------------------------------------*/
        public void InsertAndRemoveLineIntoFile(string Path, string Data, int LineInsert, int LineRemove)
        {
            try
            {
                var listData = new List<string>();
                FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite);
                int lines = 0;
                using (StreamReader sr = new StreamReader(fs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (lines == (LineInsert - 1))
                        {
                            listData.Add(Data);
                        }

                        else if (lines == LineRemove)
                        {
                            lines++;
                            continue;
                        }

                        listData.Add(line);
                        lines++;
                    }
                }
                using (StreamWriter sw = new StreamWriter(Path))
                {
                    foreach (var item in listData)
                    {
                        sw.WriteLine(item.Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error by InsertAndRemoveLineIntoFile: " + ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /*------------------------------------------------------------------------*/
        private int? ConvertStringToInt(string intString)
        {
            int i = 0;
            return (Int32.TryParse(intString, out i) ? i : (int?)null);
        }

        // 2018/09/13 Add  exportDrawingProgressInfo
        private void exportDrawingProgressInfo(string outputFilePath)
        {
            if (grvDrawingList.Rows.Count > 0)
            {
                string outputString = Constants.EMPTY_STRING;
                using (StreamWriter sw = new StreamWriter(outputFilePath, false, Encoding.UTF8))
                {
                    foreach (DataGridViewRow row in grvDrawingList.Rows)
                    {
                        outputString = row.Cells[0].Value + "\t" + row.Cells[1].Value + "\t" + row.Cells[2].Value;
                        sw.WriteLine(outputString);
                    }
                }
            }
        }

        private bool isAllDrawingCompleted()
        {
            bool bCheckValid = true;
            foreach (DataGridViewRow row in grvDrawingList.Rows)
            {
                if (!(Constants.PROGRESS_CHECKED.Equals(row.Cells[2].Value + "") || Constants.PROGRESS_DONE.Equals(row.Cells[2].Value + "")))
                {
                    bCheckValid = false;
                    break;
                }
            }

            return bCheckValid;
        }

        private void txtManuToothCuttingCost_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
        }

        private void trimTextBox(object sender)
        {
            MaskedTextBox textBox = (MaskedTextBox)sender;
            textBox.Text = textBox.Text.Trim();
        }

        private void txtManuQuenchingCost_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtManuPreparationTime_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtManuPreparationStamp_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtManuPreparationColorCheck_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtManuKeyCost_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtManuTapeTime_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtManuEviPerDrawing_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtManuEviOneDrawing_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtManuScrewTime_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight1_Leave(object sender, EventArgs e)
        {
            /*Duy Khanh add code from request from Mr Nam*/
            this.ComboboxWithRule(cboSurfaceProcess1, "008", txtSurefaceWeight1, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess1, false);
        }

        private void txtSurefaceMoney1_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight2_Leave(object sender, EventArgs e)
        {
            // Request from MrNamNV
            this.ComboboxWithRule(cboSurfaceProcess2, "008", txtSurefaceWeight2, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess2, false);
        }

        private void txtSurefaceMoney2_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight3_Leave(object sender, EventArgs e)
        {
            // Request from MrNamNV
            this.ComboboxWithRule(cboSurfaceProcess3, "008", txtSurefaceWeight3, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess3, false);
        }

        private void txtSurefaceMoney3_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight4_Leave(object sender, EventArgs e)
        {
            // Request from MrNamNV
            this.ComboboxWithRule(cboSurfaceProcess4, "008", txtSurefaceWeight4, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess4, false);
        }

        private void txtSurefaceMoney4_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight5_Leave(object sender, EventArgs e)
        {
            // Request from MrNamNV
            this.ComboboxWithRule(cboSurfaceProcess5, "008", txtSurefaceWeight5, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess5, false);
        }

        private void txtSurefaceMoney5_Leave(object sender, EventArgs e)
        {

            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight6_Leave(object sender, EventArgs e)
        {
            // Request from MrNamNV
            this.ComboboxWithRule(cboSurfaceProcess6, "008", txtSurefaceWeight6, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess6, false);
        }

        private void txtSurefaceMoney6_Leave(object sender, EventArgs e)
        {

            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight7_Leave(object sender, EventArgs e)
        {
            // Request from MrNamNV
            this.ComboboxWithRule(cboSurfaceProcess7, "008", txtSurefaceWeight7, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess7, false);
        }

        private void txtSurefaceMoney7_Leave(object sender, EventArgs e)
        {

            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight8_Leave(object sender, EventArgs e)
        {
            // Request from MrNamNV
            this.ComboboxWithRule(cboSurfaceProcess8, "008", txtSurefaceWeight8, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess8, false);
        }

        private void txtSurefaceMoney8_Leave(object sender, EventArgs e)
        {

            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight9_Leave(object sender, EventArgs e)
        {
            // Request from MrNamNV
            this.ComboboxWithRule(cboSurfaceProcess9, "008", txtSurefaceWeight9, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess9, false);
        }

        private void txtSurefaceMoney9_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceWeight10_Leave(object sender, EventArgs e)
        {
            // Request from MrNamNV
            this.ComboboxWithRule(cboSurfaceProcess10, "008", txtSurefaceWeight10, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            selectAllTextBox(sender);
            trimTextBox(sender);
            // Request from Nam.NV
            this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess10, false);
        }

        private void txtSurefaceMoney10_Leave(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtPaintSquare1_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            if (Constants.EMPTY_STRING.Equals(txtPaintSquare1.Text.Trim()))
            {
                string strRaw = txtPaintSquare1.Text.Trim();
                if (strRaw != "")
                {
                    double valueDouble = Double.Parse(strRaw);
                    valueDouble = (Math.Round(valueDouble / 10, MidpointRounding.AwayFromZero) * 10);
                    txtPaintSquare1.Text = valueDouble.ToString();

                }
            }
        }

        private void txtPaintSquare2_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
            if (Constants.EMPTY_STRING.Equals(txtPaintSquare2.Text.Trim()))
            {
                string strRaw = txtPaintSquare2.Text.Trim();
                if (strRaw != "")
                {
                    double valueDouble = Double.Parse(strRaw);
                    valueDouble = (Math.Round(valueDouble / 10, MidpointRounding.AwayFromZero) * 10);
                    txtPaintSquare2.Text = valueDouble.ToString();

                }
            }
        }

        private void txtRadiusBendEffort_Leave(object sender, EventArgs e)
        {
            trimTextBox(sender);
        }

        private void selectAllTextBox(object sender)
        {
            MaskedTextBox textBox = (MaskedTextBox)sender;
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
            textBox.SelectAll();
        }

        private void txtRadiusBendEffort_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtRadiusBendEffort_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuFriesTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuFriesTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuDrillTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuDrillTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuLatheTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuLatheTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuTarepanTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuTarepanTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuBendTime_Enter(object sender, EventArgs e)
        {

            selectAllTextBox(sender);
        }

        private void txtManuBendTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuWeldTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuWeldTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuOtherTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuOtherTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuMCTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuMCTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuPolishTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuPolishTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuAdhesionTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuAdhesionTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuToothCuttingCost_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuToothCuttingCost_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuQuenchingCost_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuQuenchingCost_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuPreparationTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuPreparationTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuPreparationStamp_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuPreparationStamp_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuPreparationColorCheck_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtManuPreparationColorCheck_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuKeyCost_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuKeyCost_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuTapeTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuTapeTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuEviPerDrawing_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuEviPerDrawing_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuEviOneDrawing_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuEviOneDrawing_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuScrewTime_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtManuScrewTime_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight1_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight1_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney1_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney1_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight2_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtSurefaceWeight2_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney2_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney2_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight3_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtSurefaceWeight3_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney3_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney3_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight4_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtSurefaceWeight4_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight5_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtSurefaceWeight5_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney5_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney5_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight6_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtSurefaceWeight6_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney6_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney6_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight7_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtSurefaceWeight7_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney7_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney7_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight8_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtSurefaceWeight8_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney8_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney8_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight9_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtSurefaceWeight9_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney9_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney9_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceWeight10_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);

        }

        private void txtSurefaceWeight10_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney10_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtSurefaceMoney10_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtPaintSquare1_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            if (txtPaintSquare1.Text.Trim() != "")
            {
                string strRaw = txtPaintSquare1.Text.Trim();
                if (strRaw != "")
                {
                    double valueDouble = Double.Parse(strRaw);
                    valueDouble = (Math.Round(valueDouble / 10, MidpointRounding.AwayFromZero) * 10);
                    txtPaintSquare1.Text = valueDouble.ToString();


                }
            }
        }

        private void txtPaintSquare1_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void txtPaintSquare2_Enter(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            if (txtPaintSquare2.Text.Trim() != "")
            {
                string strRaw = txtPaintSquare2.Text.Trim();
                if (strRaw != "")
                {
                    double valueDouble = Double.Parse(strRaw);
                    valueDouble = (Math.Round(valueDouble / 10, MidpointRounding.AwayFromZero) * 10);
                    txtPaintSquare2.Text = valueDouble.ToString();
                }
            }
        }

        private void txtPaintSquare2_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
        }

        private void grvMaterial_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && (grvMaterial.CurrentCell != null) && (grvMaterial.CurrentCell.RowIndex >= 0) && ((grvMaterial.CurrentCell.ColumnIndex == 2) || (grvMaterial.CurrentCell.ColumnIndex >= 4)))
            {
                grvMaterial.CurrentCell.Value = Constants.EMPTY_STRING;
            }
            if ((e.KeyCode == Keys.Tab) && (grvMaterial.CurrentCell != null) && (grvMaterial.CurrentCell.RowIndex >= 0) && (grvMaterial.CurrentCell.ColumnIndex >= 3))
            {
                bool bFoundYellowCell = false;
                for (int i = grvMaterial.CurrentCell.ColumnIndex + 1; i <= 11; i++)
                {
                    if ((grvMaterial.CurrentRow.Cells[i].Style.BackColor == Color.Yellow) || (grvMaterial.CurrentRow.Cells[i].Style.BackColor == Color.White))
                    {
                        bFoundYellowCell = true;
                        grvMaterial.CurrentCell = grvMaterial.CurrentRow.Cells[i - 1];
                        grvMaterial.CurrentCell.Selected = true;
                        break;
                    }
                }
                if (!bFoundYellowCell)
                {
                    for (int i = 4; i <= grvMaterial.CurrentCell.ColumnIndex - 1; i++)
                    {
                        if ((grvMaterial.CurrentRow.Cells[i].Style.BackColor == Color.Yellow) || (grvMaterial.CurrentRow.Cells[i].Style.BackColor == Color.White))
                        {
                            grvMaterial.CurrentCell = grvMaterial.CurrentRow.Cells[i - 1];
                            grvMaterial.CurrentCell.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        private void cboSideFinish_TextChanged(object sender, EventArgs e)
        {
            fillSurefaceProcessAuto();
            findIndexByText((ComboBox)sender);
            cboSideFinish.BackColor = cboSideFinish.SelectedIndex >= 0 ? Color.White : Color.Green;
            ChangeSaveButtonStatus(true);
        }

        private void findIndexByText(ComboBox cbo)
        {
            string strText = cbo.Text + Constants.EMPTY_STRING;
            for (int i = 0; i < cbo.Items.Count; i++)
            {
                if (strText.ToLower().Equals(cbo.Items[i].ToString().ToLower()))
                {
                    cbo.SelectedIndex = i;
                    break;
                }
            }
        }

        private void grvDrawingList_SelectionChanged(object sender, EventArgs e)
        {
            if ((grvDrawingList.CurrentRow != null) && (grvDrawingList.CurrentRow.Index >= 0))
            {
                if ((grvDrawingList.CurrentCell != null))
                {
                    grvDrawingList.CurrentRow.Selected = true;
                    //grvDrawingList.BeginEdit(true);
                    if (btnSave.Enabled)
                    {
                        if (MessageBox.Show(this, "Drawing [" + grvDrawingList[1, grvDrawingList.CurrentRow.Index].Value + "] has been change." + Environment.NewLine + "Do you want to save before changing to another drawing?", CommonsVars.APP_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.saveCurrentData();
                        }
                    }
                    this.bIsFormLoaded = false;
                    this.resetInputItems();
                    if (this.inputInfoId > 0)
                    {
                        // Load info from MATERIAL_EST
                        loadDataIntoGrvMaterialInfor(this.inputInfoId, grvDrawingList[1, grvDrawingList.CurrentCell.RowIndex].Value.ToString());
                        // Load info from PART_EST
                        loadDataIntoGrvPartEst(this.inputInfoId, grvDrawingList[1, grvDrawingList.CurrentCell.RowIndex].Value.ToString());

                        //cboSideFinish.SelectedIndex = -1;
                        //cboManuDivision.BackColor = cboManuDivision.SelectedIndex >= 0 ? Color.White : Color.Yellow;
                        cboSideFinish.BackColor = cboSideFinish.SelectedIndex >= 0 ? Color.White : Color.Yellow;
                        cboManufactorType.BackColor = cboManufactorType.SelectedIndex >= 0 ? Color.White : Color.Yellow;
                        cboStandardRank.BackColor = cboStandardRank.SelectedIndex >= 0 ? Color.White : Color.Yellow;
                        btnComplete.Enabled = true;
                        btnCompleteChecking.Enabled = true;
                    }
                    this.bIsFormLoaded = true;
                }
            }
            //else if ((grvDrawingList.CurrentCell != null) && (grvDrawingList.CurrentCell.ColumnIndex == 3))
            //{
            //    grvDrawingList.CurrentRow.Selected = true;
        }

        private void cboManufactorType_TextChanged(object sender, EventArgs e)
        {
            findIndexByText((ComboBox)sender);
            cboManufactorType.BackColor = cboManufactorType.SelectedIndex >= 0 ? Color.White : Color.Yellow;
            ChangeSaveButtonStatus(true);
        }

        private void txtManuDrillTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuDrill, txtManuDrillTime);
        }

        private void txtManuBendTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuBend, txtManuBendTime);
        }

        private void txtManuLatheTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuLathe, txtManuLatheTime);
        }

        private void txtManuTarepanTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuTarepan, txtManuTarepanTime);
        }

        private void txtManuWeldTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuWeld, txtManuWeldTime);
        }

        private void txtManuOtherTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuOther, txtManuOtherTime);
        }

        private void txtManuMCTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuMC, txtManuMCTime);
        }

        private void txtManuPolishTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuPolish, txtManuPolishTime);
        }

        private void txtManuAdhesionTime_KeyDown(object sender, KeyEventArgs e)
        {
            processEnterKey(e, chkManuAdhesion, txtManuAdhesionTime);
        }

        private void grvMaterial_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((grvBuyMaterial.CurrentCell != null) && (grvBuyMaterial.CurrentCell.RowIndex >= 0))
            {
                ChangeSaveButtonStatus(true);
            }
        }

        private bool isEmptyOrZero(string str)
        {
            str = (str + "").Trim();
            return Constants.EMPTY_STRING.Equals(str) || "0".Equals(str);
        }

        private void cboManuDivision_TextChanged(object sender, EventArgs e)
        {
            fillSurefaceProcessAuto();
            ChangeSaveButtonStatus(true);
        }

        private void cboManuDivision_TextChanged_1(object sender, EventArgs e)
        {
            ChangeSaveButtonStatus(true);
        }

        //add function fixbug 12 2018.11.07
        private void clearValueToTopSurefaceProcessComboBox(string strValue)
        {
            if ((cboSurfaceProcess1.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess1.SelectedValue)))
            {
                //cboSurfaceProcess1.SelectedValue = strValue;
                cboSurfaceProcess1.SelectedIndex = -1;
            }
            else if ((cboSurfaceProcess2.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess2.SelectedValue)))
            {
                //cboSurfaceProcess2.SelectedValue = strValue;
                cboSurfaceProcess2.SelectedIndex = -1;
            }
            else if ((cboSurfaceProcess3.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess3.SelectedValue)))
            {
                //cboSurfaceProcess3.SelectedValue = strValue;
                cboSurfaceProcess3.SelectedIndex = -1;
            }
            else if ((cboSurfaceProcess4.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess4.SelectedValue)))
            {
                //cboSurfaceProcess4.SelectedValue = strValue;
                cboSurfaceProcess4.SelectedIndex = -1;
            }
            else if ((cboSurfaceProcess5.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess5.SelectedValue)))
            {
                //cboSurfaceProcess5.SelectedValue = strValue;
                cboSurfaceProcess5.SelectedIndex = -1;
            }
            else if ((cboSurfaceProcess6.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess6.SelectedValue)))
            {
                //cboSurfaceProcess6.SelectedValue = strValue;
                cboSurfaceProcess6.SelectedIndex = -1;
            }
            else if ((cboSurfaceProcess7.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess7.SelectedValue)))
            {
                //cboSurfaceProcess7.SelectedValue = strValue;
                cboSurfaceProcess7.SelectedIndex = -1;
            }
            else if ((cboSurfaceProcess8.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess8.SelectedValue)))
            {
                //cboSurfaceProcess8.SelectedValue = strValue;
                cboSurfaceProcess8.SelectedIndex = -1;
            }
            else if ((cboSurfaceProcess9.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess9.SelectedValue)))
            {
                //cboSurfaceProcess9.SelectedValue = strValue;
                cboSurfaceProcess9.SelectedIndex = -1;
            }
            else if ((cboSurfaceProcess10.SelectedIndex >= 0) && (strValue.Equals(cboSurfaceProcess10.SelectedValue)))
            {
                //cboSurfaceProcess10.SelectedValue = strValue;
                cboSurfaceProcess10.SelectedIndex = -1;
            }
        }

        private void grvBuyMaterial_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        #region Duy Khanh add Code
        private void highlightMaskedTextBoxvsCheckBox(MaskedTextBox txtHighlight, CheckBox chkCheck)
        {
            try
            {
                if ((!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim())) && (int.Parse("0" + txtHighlight.Text.Trim()) != 0))
                {
                    txtHighlight.BackColor = Color.White;
                    chkCheck.Checked = true;
                }
                else if (!Constants.EMPTY_STRING.Equals(txtDrawingCode.Text.Trim()))
                {
                    txtHighlight.BackColor = chkCheck.Checked ? Color.Yellow : Color.White;
                }
                else
                {
                    txtHighlight.BackColor = Color.White;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }


        //===================================
        private void saveCurrentDataCheckbox()
        {
            int currentDrawingList = 0;
            try
            {
                currentDrawingList = grvDrawingList.CurrentCell.RowIndex;

                grvDrawingListDataCheckBox[currentDrawingList * 10 + 0] = chk1.Checked;
                grvDrawingListDataCheckBox[currentDrawingList * 10 + 1] = chk2.Checked;
                grvDrawingListDataCheckBox[currentDrawingList * 10 + 2] = chk3.Checked;
                grvDrawingListDataCheckBox[currentDrawingList * 10 + 3] = chk4.Checked;
                grvDrawingListDataCheckBox[currentDrawingList * 10 + 4] = chk5.Checked;
                grvDrawingListDataCheckBox[currentDrawingList * 10 + 5] = chk6.Checked;
                grvDrawingListDataCheckBox[currentDrawingList * 10 + 6] = chk7.Checked;
                grvDrawingListDataCheckBox[currentDrawingList * 10 + 7] = chk8.Checked;
                grvDrawingListDataCheckBox[currentDrawingList * 10 + 8] = chk9.Checked;
                grvDrawingListDataCheckBox[currentDrawingList * 10 + 9] = chk10.Checked;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        // Save Pink Color
        private void saveCurrentDataPinkColor()
        {
            int currentDrawingList = 0;
            try
            {
                currentDrawingList = grvDrawingList.CurrentCell.RowIndex;
                grvDrawingListDataPinkColor[currentDrawingList * 10 + 0] = grvBuyMaterial.Rows[0].Cells[4].Style.BackColor == Color.LightPink ? true : false;
                grvDrawingListDataPinkColor[currentDrawingList * 10 + 1] = grvBuyMaterial.Rows[1].Cells[4].Style.BackColor == Color.LightPink ? true : false;
                grvDrawingListDataPinkColor[currentDrawingList * 10 + 2] = grvBuyMaterial.Rows[2].Cells[4].Style.BackColor == Color.LightPink ? true : false;
                grvDrawingListDataPinkColor[currentDrawingList * 10 + 3] = grvBuyMaterial.Rows[3].Cells[4].Style.BackColor == Color.LightPink ? true : false;
                grvDrawingListDataPinkColor[currentDrawingList * 10 + 4] = grvBuyMaterial.Rows[4].Cells[4].Style.BackColor == Color.LightPink ? true : false;
                grvDrawingListDataPinkColor[currentDrawingList * 10 + 5] = grvBuyMaterial.Rows[5].Cells[4].Style.BackColor == Color.LightPink ? true : false;
                grvDrawingListDataPinkColor[currentDrawingList * 10 + 6] = grvBuyMaterial.Rows[6].Cells[4].Style.BackColor == Color.LightPink ? true : false;
                grvDrawingListDataPinkColor[currentDrawingList * 10 + 7] = grvBuyMaterial.Rows[7].Cells[4].Style.BackColor == Color.LightPink ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        //==================================
        private void loadDataCheckbox()
        {
            int currentDrawingList = 0;
            try
            {
                currentDrawingList = grvDrawingList.CurrentCell.RowIndex;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 0]) chk1.Checked = true;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 1]) chk2.Checked = true;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 2]) chk3.Checked = true;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 3]) chk4.Checked = true;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 4]) chk5.Checked = true;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 5]) chk6.Checked = true;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 6]) chk7.Checked = true;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 7]) chk8.Checked = true;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 8]) chk9.Checked = true;
                if (grvDrawingListDataCheckBox[currentDrawingList * 10 + 9]) chk10.Checked = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "ERROR loadDataCheckbox ");
            }

        }
        //====================================================================================
        private void loadDataLightPinkColor()
        {
            int currentDrawingList = 0;
            try
            {
                currentDrawingList = grvDrawingList.CurrentCell.RowIndex;
                if (grvDrawingListDataPinkColor.Count >= 8)
                {
                    if (grvDrawingListDataPinkColor[currentDrawingList * 10 + 0]) grvBuyMaterial.Rows[0].Cells[4].Style.BackColor = Color.LightPink;
                    if (grvDrawingListDataPinkColor[currentDrawingList * 10 + 1]) grvBuyMaterial.Rows[1].Cells[4].Style.BackColor = Color.LightPink;
                    if (grvDrawingListDataPinkColor[currentDrawingList * 10 + 2]) grvBuyMaterial.Rows[2].Cells[4].Style.BackColor = Color.LightPink;
                    if (grvDrawingListDataPinkColor[currentDrawingList * 10 + 3]) grvBuyMaterial.Rows[3].Cells[4].Style.BackColor = Color.LightPink;
                    if (grvDrawingListDataPinkColor[currentDrawingList * 10 + 4]) grvBuyMaterial.Rows[4].Cells[4].Style.BackColor = Color.LightPink;
                    if (grvDrawingListDataPinkColor[currentDrawingList * 10 + 5]) grvBuyMaterial.Rows[5].Cells[4].Style.BackColor = Color.LightPink;
                    if (grvDrawingListDataPinkColor[currentDrawingList * 10 + 6]) grvBuyMaterial.Rows[6].Cells[4].Style.BackColor = Color.LightPink;
                    if (grvDrawingListDataPinkColor[currentDrawingList * 10 + 7]) grvBuyMaterial.Rows[7].Cells[4].Style.BackColor = Color.LightPink;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "ERROR loadDataCheckbox ");
            }

        }

        public void CheckShapeGridView()
        {
            #region Duy Khanh add code
            // Check condition all Shape != 00
            if (this.checkContainsTextAllColumn("combobox", 2, "00") == false)
            {
                for (int i = 0; i < grvBuyMaterial.Rows.Count; i++)
                {
                    grvBuyMaterial.Rows[i].ReadOnly = true;
                }
            }
            else
            {
                for (int i = 0; i < grvBuyMaterial.Rows.Count; i++)
                {
                    grvBuyMaterial.Rows[i].ReadOnly = false;
                }
            }
            #endregion
        }
        private bool checkContainsTextAllColumn(string stypeData, int indexColumn, string ContainsText, string modeCheck = "single")
        {

            //Define variable counter
            int counter = 0;
            int rowCounter = 0;
            foreach (DataGridViewRow row in grvMaterial.Rows)
            {
                rowCounter += 1;
                string objectText = "";
                stypeData = stypeData.ToLower();
                modeCheck = modeCheck.ToLower();
                if (stypeData == "textbox")
                {
                    DataGridViewTextBoxCell objectCell = (DataGridViewTextBoxCell)row.Cells[indexColumn];
                    objectText = objectCell.FormattedValue.ToString();
                    string objectValue = objectCell.Value + "";
                }
                else if (stypeData == "combobox")
                {
                    DataGridViewComboBoxCell objectCell = (DataGridViewComboBoxCell)row.Cells[indexColumn];
                    objectText = objectCell.FormattedValue.ToString();
                    string objectValue = objectCell.Value + "";
                }
                else
                {
                    DataGridViewComboBoxCell objectCell = (DataGridViewComboBoxCell)row.Cells[indexColumn];
                    objectText = objectCell.FormattedValue.ToString();
                    string objectValue = objectCell.Value + "";
                }

                if (objectText.Contains(ContainsText))
                {
                    counter += 1;
                }
            }
            // modeCheck = All
            if (modeCheck == "all")
            {
                if (counter == rowCounter - 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else // mode check = single
            {
                if (counter != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        //private void DisableItems35()
        //{
        //    int row = grvMaterial.CurrentCell.RowIndex;

        //    if (this.GetDataTextGridViewStyleSelect("combobox", row, 1).Contains("35"))
        //    {
        //        this.SetDataComboboxGridView(row, 2, -1);
        //    }
        //}
        //=======================================================================
        void grvMaterial1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {

            if (grvMaterial.IsCurrentCellDirty)
            {
                txtMessageGRVMATERIAL.ResetText();
                txtMessageGRVMATERIAL.BackColor = Color.White;
                // This fires the cell value changed handler below
                grvMaterial.CommitEdit(DataGridViewDataErrorContexts.Commit);
                this.AllActionWithGridViewData();
                if (flagShape1)
                {
                    this.highlightPinkColorgrvBuyMaterial();
                    flagShape1 = false;
                    flagShape2 = false;
                }
                else
                {
                    flagShape2 = true;
                }
            }
            ChangeSaveButtonStatus(true);
        }
        //========================================================================
        private void AllActionWithGridViewData()
        {
            /*Xử lý điều kiện đầu vào của  Vật liệu và Độ Xử Lý bề mặt để quyết định đầu ra cho Độ nhóm gia công*/
            this.ExecuteCondition();
            // 1 loại Vật Liệu  + Hình dáng bằng 00
            int counterRow = this.CountObjectHaveValueOfColumn("combobox", 1);
            bool contain1 = this.CheckDataTextGridViewStyleSelect("combobox", 0, 2, "00", false);// Hình dáng = 00
            bool contain2 = this.CheckDataTextGridViewStyleSelect("textbox", 0, 4, "", true);
            bool checkAllTextRow2 = this.checkContainsTextAllColumn("combobox", 2, "00", "all");
            int countWeightHaveValue = this.CountObjectHaveValueOfColumn("textbox", 4, true);
            bool haveValue00 = this.checkContainsTextAllColumn("combobox", 2, "00");
            int countWeightDoNotNeedFillValue = this.CountObjectHaveValueOfColumn("combobox", 2, true, 30, 2);
            if ((counterRow == 1) && contain1)
            {
                cboManuDivision.SelectedIndex = -1;
                cboManuDivision.SelectedIndex = 1;
            }
            // 1 loại Vật Liệu  + Hình dáng khác 00 + Trường vật liệu có giá trị

            if ((counterRow == 1) && !contain1 && contain2)
            {
                cboManuDivision.SelectedIndex = -1;
                cboManuDivision.SelectedIndex = 0;
            }
            // 1 loại Vật Liệu  + Hình dáng khác 00 + Trường vật liệu không có giá trị
            if ((counterRow == 1) && !contain1 && !contain2)
            {
                cboManuDivision.SelectedIndex = -1;
            }
            // > 2 loại Vật Liệu  + Tất cả Hình dáng là 00 ==> Phân loại gia công = K
            if ((counterRow > 1) && checkAllTextRow2)
            {
                cboManuDivision.SelectedIndex = -1;
                cboManuDivision.SelectedIndex = 1;
            }
            // > 2 loại Vật Liệu  +  Có 1 vật liệu trong số các vật liệu có khối lượng khác 0 ==> Phân loại gia công = B
            else if ((counterRow > 1) && (countWeightHaveValue == 1))
            {
                cboManuDivision.SelectedIndex = -1;
                cboManuDivision.SelectedIndex = 0;
            }
            // > 2 loại Vật Liệu  +  Có vật liệu mã hình dáng  [00] và có vật liệu không phải tính khối lượng thì [ phân loại gia công ] để trống
            else if ((counterRow > 1) && haveValue00 && (countWeightDoNotNeedFillValue > 1))
            {
                cboManuDivision.SelectedIndex = -1;
            }
            /******Giá******/
            this.CheckShapeGridView();

            // 10/12/2019 add new Required form Nam.NV Khi tích vào ô Laze đối với vật liệu 040: SUS304 khi nhập vào ô(Hình dáng) không được phép nhập mã hình dáng 35
            List<string> itemsRemove = new List<string>();
            itemsRemove.Add("35");
            BUS_ShapeDivision shapeBUS = new BUS_ShapeDivision();
            DataTable dt = shapeBUS.getShapeList();
            if (chkManuTarepan.Checked == true)
            {

                for (int i = 0; i < grvBuyMaterial.Rows.Count - 1; i++)
                {
                    if (this.GetDataTextGridViewStyleSelect("combobox", i, 1).Contains("040"))
                    {
                        if (this.GetDataTextGridViewStyleSelect("combobox", i, 2).Contains("35"))
                        {
                            string message = "Không thể chọn Shape = 35 . Khi vật liệu = 040 và Laze được chọn";
                            MessageBox.Show(this, message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }


            }

        }
        //=============================================================
        private void highlightPinkColorgrvBuyMaterial()
        {
            try
            {
                if (grvMaterial.CurrentCell.ColumnIndex == 2)
                {
                    // Check condition all Shape != 00
                    int counterPink = 0;
                    int counterNotBlank = 0;
                    for (int i = 0; i < 8; i++)
                    {
                        bool checkBlank = grvBuyMaterial.Rows[i].Cells[4].FormattedValue.ToString() != "";
                        bool checkPink = grvBuyMaterial.Rows[i].Cells[4].Style.BackColor == Color.LightPink;
                        if (checkPink)
                        {
                            counterPink += 1;
                        }

                        if (checkBlank) counterNotBlank += 1;

                    }

                    String objectText = grvMaterial.CurrentRow.Cells[2].FormattedValue + "";
                    if (objectText.Contains("00"))
                    {
                        if (counterPink == 0)
                        {
                            grvBuyMaterial.Rows[counterNotBlank].Cells[4].Style.BackColor = Color.LightPink;
                            // Ignor Focus
                            //grvBuyMaterial.Focus();
                            //grvBuyMaterial.CurrentCell = grvBuyMaterial.Rows[counterNotBlank].Cells[4];
                        }
                        else
                        {
                            grvBuyMaterial.Rows[counterNotBlank + counterPink].Cells[4].Style.BackColor = Color.LightPink;
                            // Ignor Focus
                            //grvBuyMaterial.Focus();
                            //grvBuyMaterial.CurrentCell = grvBuyMaterial.Rows[counterNotBlank + counterPink].Cells[4];
                        }
                    }

                    //===============
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        //================================
        private void ExecuteCondition()
        {
            if (this.checkDoubleCondition())
            {
                Random rd = new Random();
                int rand_num;
                rand_num = rd.Next(0, 1);
                cboSideFinish.Enabled = true;
                cboSideFinish.SelectedIndex = rand_num;
                cboSideFinish.BackColor = System.Drawing.Color.GreenYellow;
            }
            else
            {
                cboSideFinish.BackColor = System.Drawing.Color.White;
                cboSideFinish.Enabled = false;
                cboSideFinish.SelectedIndex = 0;
            }
        }
        //==============================
        /*Counter all cell have value of column */
        private int CountObjectHaveValueOfColumn(string styleSelect, int indexColumnSelected, bool IsCompareValue = false, float valueCompare = 0, int modeCompare = 1)
        {
            //Define variable counter
            int counter = 0;
            string objectText = "";
            styleSelect = styleSelect.ToLower();
            foreach (DataGridViewRow row in grvMaterial.Rows)
            {

                if (styleSelect == "textbox")
                {
                    DataGridViewTextBoxCell objectCell = (DataGridViewTextBoxCell)row.Cells[indexColumnSelected];
                    objectText = objectCell.FormattedValue.ToString();
                }
                else if (styleSelect == "combobox")
                {
                    DataGridViewComboBoxCell objectCell = (DataGridViewComboBoxCell)row.Cells[indexColumnSelected];
                    objectText = objectCell.FormattedValue.ToString();

                }
                else
                {
                    DataGridViewComboBoxCell objectCell = (DataGridViewComboBoxCell)row.Cells[indexColumnSelected];
                    objectText = objectCell.FormattedValue.ToString();
                }

                if (objectText.Length > 1)
                {
                    if (IsCompareValue)
                    {
                        // modeCompare = 1 is ">"
                        // modeCompare = 2 is "<"
                        // modeCompare = 3 is "=="
                        // modeCompare = 4 is ">="
                        // modeCompare = 5 is "<="
                        bool check = false;
                        float valueToFloat = 0;
                        if (styleSelect == "combobox")
                        {
                            if (objectText != "00")
                            {
                                valueToFloat = float.Parse(objectText.Remove(2).Replace(" ", ""));
                            }
                            else
                            {
                                valueToFloat = float.Parse(objectText);
                            }

                        }
                        else // else textbox
                        {
                            valueToFloat = float.Parse(objectText);
                        }
                        switch (modeCompare)
                        {
                            case 1:
                                check = valueToFloat > valueCompare;
                                break;
                            case 2:
                                check = valueToFloat < valueCompare;
                                break;
                            case 3:
                                check = valueToFloat == valueCompare;
                                break;
                            case 4:
                                check = valueToFloat >= valueCompare;
                                break;
                            case 5:
                                check = valueToFloat <= valueCompare;
                                break;
                            default:
                                check = valueToFloat > valueCompare;
                                break;
                        }
                        if (check)
                        {
                            counter += 1;
                        }
                    }
                    else
                    {
                        counter += 1;
                    }
                }
            }
            return counter;
        }
        //================================
        /*Check Text contains in TextBox GridView*/
        private bool CheckDataTextGridViewStyleSelect(string styleSelect, int indexRowSelected, int indexColumnSelected, string content, bool modeCheckNull)
        {
            try
            {
                DataGridViewRow row = grvMaterial.Rows[indexRowSelected];
                styleSelect.ToLower();
                if (row != null)
                {
                    string objectText = "";
                    if (styleSelect == "textbox")
                    {
                        DataGridViewTextBoxCell objectCell = (DataGridViewTextBoxCell)row.Cells[indexColumnSelected];
                        objectText = objectCell.FormattedValue.ToString();
                        string objectValue = objectCell.Value + "";
                    }
                    else if (styleSelect == "combobox")
                    {
                        DataGridViewComboBoxCell objectCell = (DataGridViewComboBoxCell)row.Cells[indexColumnSelected];
                        objectText = objectCell.FormattedValue.ToString();
                        string objectValue = objectCell.Value + "";
                    }
                    else
                    {
                        DataGridViewComboBoxCell objectCell = (DataGridViewComboBoxCell)row.Cells[indexColumnSelected];
                        objectText = objectCell.FormattedValue.ToString();
                        string objectValue = objectCell.Value + "";
                    }
                    // Check content cell textbox different NULL
                    // modeCheckNull = true
                    if (modeCheckNull == true)
                    {
                        if (objectText.Length > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    // modeCheckNull = false switch to mode check Contains text
                    else
                    {
                        if (objectText.Contains(content))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                else
                {
                    return false;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e + "It is not combobox in ListView or Combobox");
                return false;
            }
        }

        //-------------New Request When "Xử Lý Bề Mặt" to Lượng và Tiền = 0 , 1
        private bool checkSurfaceCondition_ToGenerate_0_1(ComboBox cbo)
        {
            if (this.checkComboboxSelectedTextContains(cbo, "074") ||
               this.checkComboboxSelectedTextContains(cbo, "077") ||
               this.checkComboboxSelectedTextContains(cbo, "078") ||
               this.checkComboboxSelectedTextContains(cbo, "079") ||
               this.checkComboboxSelectedTextContains(cbo, "080") ||
               this.checkComboboxSelectedTextContains(cbo, "082") ||
               this.checkComboboxSelectedTextContains(cbo, "083") ||
               this.checkComboboxSelectedTextContains(cbo, "088") ||
               this.checkComboboxSelectedTextContains(cbo, "090") ||
               this.checkComboboxSelectedTextContains(cbo, "092") ||
               this.checkComboboxSelectedTextContains(cbo, "095") ||
               this.checkComboboxSelectedTextContains(cbo, "096") ||
               this.checkComboboxSelectedTextContains(cbo, "097") ||
               this.checkComboboxSelectedTextContains(cbo, "098") ||
               this.checkComboboxSelectedTextContains(cbo, "117") ||
               this.checkComboboxSelectedTextContains(cbo, "118") ||
               this.checkComboboxSelectedTextContains(cbo, "119") ||
               this.checkComboboxSelectedTextContains(cbo, "120") ||
               this.checkComboboxSelectedTextContains(cbo, "121") ||
               this.checkComboboxSelectedTextContains(cbo, "122") ||
               this.checkComboboxSelectedTextContains(cbo, "123") ||
               this.checkComboboxSelectedTextContains(cbo, "124") ||
               this.checkComboboxSelectedTextContains(cbo, "127") ||
               this.checkComboboxSelectedTextContains(cbo, "128") ||
               this.checkComboboxSelectedTextContains(cbo, "999")
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool checkComboboxSelectedTextContains(ComboBox cbo, string txtContains)
        {
            if (cbo.SelectedValue.ToString().Contains(txtContains))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //==============================
        // Check điều kiện kép của vật liệu và Xử lý bề mặt
        private bool checkDoubleCondition()
        {
            return (this.IsContainsMaterialSpecific() && this.isExistAtLeastSurefaceProcess(false));
        }
        //==============================
        private bool IsContainsMaterialSpecific()
        {
            foreach (DataGridViewRow row in grvMaterial.Rows)
            {
                if (row != null)
                {
                    DataGridViewComboBoxCell cboCell = (DataGridViewComboBoxCell)row.Cells[1];
                    string cboText = cboCell.FormattedValue.ToString();
                    string cboValue = cboCell.Value + "";
                    // SS400, STKR400, S45C, S55C, SK3, SK4, SK5, SKD11, SKH51,STKM 13A
                    if (cboText.Contains("002") || cboText.Contains("STKR400") ||
                        cboText.Contains("S45C") || cboText.Contains("S55C") ||
                        cboText.Contains("SK3") || cboText.Contains("SK4") ||
                        cboText.Contains("STKR400") || cboText.Contains("SK5") ||
                        cboText.Contains("SKD11") || cboText.Contains("SKH51") ||
                        cboText.Contains("STKM 13A") || cboText.Contains("STPG370") ||
                        cboText.Contains("017") || cboText.Contains("141"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //====================================
        private bool isExistAtLeastSurefaceProcess(bool allowSelectedIndexIsZero = true)
        {
            if (allowSelectedIndexIsZero)
            {
                return (((cboSurfaceProcess1.SelectedIndex >= 0)) || ((cboSurfaceProcess2.SelectedIndex >= 0))
                    || ((cboSurfaceProcess3.SelectedIndex >= 0)) || ((cboSurfaceProcess4.SelectedIndex >= 0))
                     || ((cboSurfaceProcess5.SelectedIndex >= 0)) || ((cboSurfaceProcess6.SelectedIndex >= 0))
                     || ((cboSurfaceProcess7.SelectedIndex >= 0)) || ((cboSurfaceProcess8.SelectedIndex >= 0))
                     || ((cboSurfaceProcess9.SelectedIndex >= 0)) || ((cboSurfaceProcess10.SelectedIndex >= 0)));
            }
            else
            {
                return (((cboSurfaceProcess1.SelectedIndex > 0)) || ((cboSurfaceProcess2.SelectedIndex > 0))
                    || ((cboSurfaceProcess3.SelectedIndex > 0)) || ((cboSurfaceProcess4.SelectedIndex > 0))
                    || ((cboSurfaceProcess5.SelectedIndex > 0)) || ((cboSurfaceProcess6.SelectedIndex > 0))
                    || ((cboSurfaceProcess7.SelectedIndex > 0)) || ((cboSurfaceProcess8.SelectedIndex > 0))
                    || ((cboSurfaceProcess9.SelectedIndex > 0)) || ((cboSurfaceProcess10.SelectedIndex > 0)));
            }


        }
        //=======================================
        public void grv_cmb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            flagShape1 = true;
            if (flagShape2)
            {
                this.highlightPinkColorgrvBuyMaterial();
            }

        }

        //==========================================
        private void highlightPairTextBoxNew(ComboBox cboHighlight, MaskedTextBox txtHighlight)
        {
            if (cboHighlight.SelectedIndex == 0)
            {
                txtHighlight.BackColor = Color.White;
                txtHighlight.Text = "";
            }
            else if (cboHighlight.SelectedIndex > 0 && int.Parse("0" + txtHighlight.Text.Trim()) == 0)
            {
                txtHighlight.BackColor = Color.Yellow;
                cboHighlight.BackColor = Color.White;
            }
            else if (cboHighlight.SelectedIndex > 0 && int.Parse("0" + txtHighlight.Text.Trim()) != 0)
            {
                txtHighlight.BackColor = Color.White;
                cboHighlight.BackColor = Color.White;
            }
            else
            {
                txtHighlight.BackColor = Color.White;
                txtHighlight.Text = "";
            }
            // Request From HUY
            if (!cboHighlight.Text.Equals(""))
            {
                if (checkSurfaceCondition_ToGenerate_0_1(cboHighlight))
                {
                    txtHighlight.BackColor = Color.GreenYellow;
                }
            }
        }

        //===========================================
        private void cboSurfaceProcess1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            highlightPairTextBoxNew(cboSurfaceProcess1, txtSurefaceWeight1);

            if (flag)
            {
                cboSurfaceProcess2.Focus();
                flag = false;
            }

            CheckListComboboxRequireFromNamNV(cboSurfaceProcess1);
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess1))
            {
                txtSurefaceWeight1.Text = "0";
                txtSurefaceWeight1.BackColor = Color.GreenYellow;
                txtSurefaceMoney1.Text = "1";
                //txtSurefaceMoney1.ReadOnly = true;
                //txtSurefaceWeight1.ReadOnly = false;
            }
        }
        private void cboSurfaceProcess2_SelectionChangeCommitted(object sender, EventArgs e)
        {

            highlightPairTextBoxNew(cboSurfaceProcess2, txtSurefaceWeight2);
            if (flag)
            {
                cboSurfaceProcess3.Focus();
                flag = false;
            }
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess2))
            {
                txtSurefaceWeight2.Text = "0";
                txtSurefaceWeight2.BackColor = Color.GreenYellow;
                txtSurefaceMoney2.Text = "1";

            }
            // Check 
            CheckListComboboxRequireFromNamNV(cboSurfaceProcess2);
        }

        private void cboSurfaceProcess3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            highlightPairTextBoxNew(cboSurfaceProcess3, txtSurefaceWeight3);
            if (flag)
            {
                cboSurfaceProcess4.Focus();
                flag = false;
            }
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess3))
            {
                txtSurefaceWeight3.Text = "0";
                txtSurefaceWeight3.BackColor = Color.GreenYellow;
                txtSurefaceMoney3.Text = "1";

            }
            // Check 
            CheckListComboboxRequireFromNamNV(cboSurfaceProcess3);
        }

        private void cboSurfaceProcess4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            highlightPairTextBoxNew(cboSurfaceProcess4, txtSurefaceWeight4);
            if (flag)
            {
                cboSurfaceProcess5.Focus();
                flag = false;
            }
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess4))
            {
                txtSurefaceWeight4.Text = "0";
                txtSurefaceWeight4.BackColor = Color.GreenYellow;
                txtSurefaceMoney4.Text = "1";

            }
            // Check 
            CheckListComboboxRequireFromNamNV(cboSurfaceProcess4);
        }

        private void cboSurfaceProcess5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            highlightPairTextBoxNew(cboSurfaceProcess5, txtSurefaceWeight5);
            if (flag)
            {
                cboSurfaceProcess6.Focus();
                flag = false;
            }
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess5))
            {
                txtSurefaceWeight5.Text = "0";
                txtSurefaceWeight5.BackColor = Color.GreenYellow;
                txtSurefaceMoney5.Text = "1";

            }
            // Check 
            CheckListComboboxRequireFromNamNV(cboSurfaceProcess5);
        }

        private void cboSurfaceProcess6_SelectionChangeCommitted(object sender, EventArgs e)
        {

            highlightPairTextBoxNew(cboSurfaceProcess6, txtSurefaceWeight6);
            if (flag)
            {
                cboSurfaceProcess7.Focus();
                flag = false;
            }
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess6))
            {
                txtSurefaceWeight6.Text = "0";
                txtSurefaceWeight6.BackColor = Color.GreenYellow;
                txtSurefaceMoney6.Text = "1";

            }
            // Check 
            CheckListComboboxRequireFromNamNV(cboSurfaceProcess6);
        }

        private void cboSurfaceProcess7_SelectionChangeCommitted(object sender, EventArgs e)
        {

            highlightPairTextBoxNew(cboSurfaceProcess7, txtSurefaceWeight7);
            if (flag)
            {
                cboSurfaceProcess8.Focus();
                flag = false;
            }
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess7))
            {
                txtSurefaceWeight7.Text = "0";
                txtSurefaceWeight7.BackColor = Color.GreenYellow;
                txtSurefaceMoney7.Text = "1";

            }
            // Check 
            CheckListComboboxRequireFromNamNV(cboSurfaceProcess7);
        }

        private void cboSurfaceProcess8_SelectionChangeCommitted(object sender, EventArgs e)
        {

            highlightPairTextBoxNew(cboSurfaceProcess8, txtSurefaceWeight8);
            if (flag)
            {
                cboSurfaceProcess9.Focus();
                flag = false;
            }
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess8))
            {
                txtSurefaceWeight8.Text = "0";
                txtSurefaceWeight8.BackColor = Color.GreenYellow;
                txtSurefaceMoney8.Text = "1";

            }
            // Check 
            CheckListComboboxRequireFromNamNV(cboSurfaceProcess8);
        }

        private void cboSurfaceProcess9_SelectionChangeCommitted(object sender, EventArgs e)
        {

            highlightPairTextBoxNew(cboSurfaceProcess9, txtSurefaceWeight9);
            if (flag)
            {
                cboSurfaceProcess10.Focus();
                flag = false;
            }
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess9))
            {
                txtSurefaceWeight9.Text = "0";
                txtSurefaceWeight9.BackColor = Color.GreenYellow;
                txtSurefaceMoney9.Text = "1";

            }
            // Check 
            CheckListComboboxRequireFromNamNV(cboSurfaceProcess9);
        }

        private void cboSurfaceProcess10_SelectionChangeCommitted(object sender, EventArgs e)
        {
            highlightPairTextBoxNew(cboSurfaceProcess10, txtSurefaceWeight10);
            // Check 
            // Request "Khi người dùng chọn surface đặc biệt thì tiền có giá trị 1"
            if (this.checkSurfaceCondition_ToGenerate_0_1(cboSurfaceProcess10))
            {
                txtSurefaceWeight10.BackColor = Color.GreenYellow;
                txtSurefaceWeight10.Text = "0";
                txtSurefaceMoney10.Text = "1";

            }
            CheckListComboboxRequireFromNamNV(cboSurfaceProcess10);
        }

        //====================================================
        #region Duy Khanh add code fix issue Gia công Cell 204 112 and 115 all have value is 0
        // Duy Khanh add code to fix issue Gia Công
        public void replaceStringDataInLine(string splitString, int indexFocus, string stringReplace, int lineEdit, string fileFolder)
        {
            // read the data
            int line_to_edit = lineEdit; // Warning: 1-based indexing!
            string sourceFile = fileFolder;
            string destinationFile = fileFolder;

            // Read the appropriate line from the file.
            string lineToWrite = null;
            using (StreamReader reader = new StreamReader(sourceFile))
            {
                for (int i = 1; i <= line_to_edit; ++i)
                    lineToWrite = reader.ReadLine();
            }

            if (lineToWrite == null)
                throw new InvalidDataException("Line does not exist in " + sourceFile);

            // Read the old file.
            string[] linesdata = File.ReadAllLines(destinationFile);
            string dataLineNeedEdit = linesdata[line_to_edit - 1].ToString();
            string[] strArr = dataLineNeedEdit.Split(new[] { splitString }, System.StringSplitOptions.None);
            string StringComplete = "";
            //===================================
            for (int i = 0; i < strArr.Length; i++)
            {
                string addStr = "";
                if (i == indexFocus - 1)
                {
                    if (i != strArr.Length - 1)
                    {
                        addStr = stringReplace + ",";
                    }
                    else
                    {
                        addStr = stringReplace;
                    }
                }
                else
                {
                    if (i != strArr.Length - 1)
                    {
                        addStr = strArr[i] + ",";
                    }
                    else
                    {
                        addStr = strArr[i];
                    }
                }
                StringComplete += addStr;
            }

            // Write the new file over the old file.
            using (StreamWriter writer = new StreamWriter(destinationFile))
            {
                for (int currentLine = 1; currentLine <= linesdata.Length; ++currentLine)
                {
                    if (currentLine == line_to_edit)
                    {
                        writer.WriteLine(StringComplete);
                    }
                    else
                    {
                        writer.WriteLine(linesdata[currentLine - 1]);
                    }
                }
            }

        }
        // Function related to Export
        public void FixIssue_104_112_115(string fileFolder)
        {
            string text = System.IO.File.ReadAllText(fileFolder);
            int count1 = System.Text.RegularExpressions.Regex.Matches(text, "\n").Count;

            // Fix cell 104 (offset - 14 = 90)
            for (int i = 1; i <= count1; i++)
            {
                this.replaceStringDataInLine(",", 90, "0", i, fileFolder);
            }
            // Fix cell 112 (offset - 14 = 98)
            for (int i = 1; i <= count1; i++)
            {
                this.replaceStringDataInLine(",", 98, "0", i, fileFolder);
            }
            // Fix cell 115 (offset - 14 = 101)
            for (int i = 1; i <= count1; i++)
            {
                this.replaceStringDataInLine(",", 101, "0", i, fileFolder);
            }


        }

        private void Copy_File(string sourceDir, string backupDir, string sourceName, string desfileName)
        {

            try
            {

                string[] txtList = Directory.GetFiles(sourceDir, "*.txt");



                // Copy text files.
                foreach (string f in txtList)
                {

                    // Remove path from the file name.
                    string fName = sourceName;

                    try
                    {
                        // Will not overwrite if the destination file already exists.
                        File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, desfileName));
                    }

                    // Catch exception if the file was already copied.
                    catch (IOException copyError)
                    {
                        Console.WriteLine(copyError.Message);
                    }
                }

                // Delete source files that were copied.
            }

            catch (DirectoryNotFoundException dirNotFound)
            {
                Console.WriteLine(dirNotFound.Message);
            }

        }

        #endregion

        private void grvBuyMaterial_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && (grvBuyMaterial.CurrentCell != null))
            {
                grvBuyMaterial.CurrentCell.Value = Constants.EMPTY_STRING;
                if (grvBuyMaterial.CurrentCell.Style.BackColor == Color.LightPink)
                {
                    grvBuyMaterial.CurrentCell.Style.BackColor = Color.White;
                }
            }
        }

        private void grvBuyMaterial_KeyDown_1(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete) && (grvBuyMaterial.CurrentCell != null))
            {
                grvBuyMaterial.CurrentCell.Value = Constants.EMPTY_STRING;
                if (grvBuyMaterial.CurrentCell.Style.BackColor == Color.LightPink)
                {
                    grvBuyMaterial.CurrentCell.Style.BackColor = Color.White;
                }
            }
        }

        private void chk1_CheckedChanged(object sender, EventArgs e)
        {

            txtManuToothCuttingCost.Enabled = chk1.Checked;
            txtManuToothCuttingCost.Text = chk1.Checked ? txtManuToothCuttingCost.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuToothCuttingCost.BackColor = (chk1.Checked && isEmptyOrZero(txtManuToothCuttingCost.Text)) ? Color.Yellow : Color.White;
        }
        #endregion

        private void chk2_CheckedChanged(object sender, EventArgs e)
        {
            txtManuQuenchingCost.Enabled = chk2.Checked;
            txtManuQuenchingCost.Text = chk2.Checked ? txtManuQuenchingCost.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuQuenchingCost.BackColor = (chk2.Checked && isEmptyOrZero(txtManuQuenchingCost.Text)) ? Color.Yellow : Color.White;
        }

        private void chk3_CheckedChanged(object sender, EventArgs e)
        {
            txtManuPreparationTime.Enabled = chk3.Checked;
            txtManuPreparationTime.Text = chk3.Checked ? txtManuPreparationTime.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuPreparationTime.BackColor = (chk3.Checked && isEmptyOrZero(txtManuPreparationTime.Text)) ? Color.Yellow : Color.White;
        }

        private void chk4_CheckedChanged(object sender, EventArgs e)
        {
            txtManuPreparationStamp.Enabled = chk4.Checked;
            txtManuPreparationStamp.Text = chk4.Checked ? txtManuPreparationStamp.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuPreparationStamp.BackColor = (chk4.Checked && isEmptyOrZero(txtManuPreparationStamp.Text)) ? Color.Yellow : Color.White;
        }

        private void chk5_CheckedChanged(object sender, EventArgs e)
        {
            txtManuPreparationColorCheck.Enabled = chk5.Checked;
            txtManuPreparationColorCheck.Text = chk5.Checked ? txtManuPreparationColorCheck.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuPreparationColorCheck.BackColor = (chk5.Checked && isEmptyOrZero(txtManuPreparationColorCheck.Text)) ? Color.Yellow : Color.White;
        }

        private void chk6_CheckedChanged(object sender, EventArgs e)
        {
            txtManuKeyCost.Enabled = chk6.Checked;
            txtManuKeyCost.Text = chk6.Checked ? txtManuKeyCost.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuKeyCost.BackColor = (chk6.Checked && isEmptyOrZero(txtManuKeyCost.Text)) ? Color.Yellow : Color.White;
        }

        private void chk7_CheckedChanged(object sender, EventArgs e)
        {
            txtManuTapeTime.Enabled = chk7.Checked;
            txtManuTapeTime.Text = chk7.Checked ? txtManuTapeTime.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuTapeTime.BackColor = (chk7.Checked && isEmptyOrZero(txtManuTapeTime.Text)) ? Color.Yellow : Color.White;
        }

        private void chk8_CheckedChanged(object sender, EventArgs e)
        {
            txtManuEviPerDrawing.Enabled = chk8.Checked;
            txtManuEviPerDrawing.Text = chk8.Checked ? txtManuEviPerDrawing.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuEviPerDrawing.BackColor = (chk8.Checked && isEmptyOrZero(txtManuEviPerDrawing.Text)) ? Color.Yellow : Color.White;
        }

        private void chk9_CheckedChanged(object sender, EventArgs e)
        {
            txtManuEviOneDrawing.Enabled = chk9.Checked;
            txtManuEviOneDrawing.Text = chk9.Checked ? txtManuEviOneDrawing.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuEviOneDrawing.BackColor = (chk9.Checked && isEmptyOrZero(txtManuEviOneDrawing.Text)) ? Color.Yellow : Color.White;
        }

        private void chk10_CheckedChanged(object sender, EventArgs e)
        {
            txtManuScrewTime.Enabled = chk10.Checked;
            txtManuScrewTime.Text = chk10.Checked ? txtManuScrewTime.Text : Constants.ZERO_STRING; // update set 0 when unchecked
            txtManuScrewTime.BackColor = (chk10.Checked && isEmptyOrZero(txtManuScrewTime.Text)) ? Color.Yellow : Color.White;
        }

        private void cboStandardRank_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboStandardRank.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess1.SelectedIndex = -1;
            }
        }

        private void cboGradeType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboGradeType.SelectedIndex = -1;
            }
        }

        private void cboManufactorType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboManufactorType.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess2.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess3.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess4.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess5.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess6.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess7.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess8.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess9.SelectedIndex = -1;
            }
        }

        private void cboSurfaceProcess10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                cboSurfaceProcess10.SelectedIndex = -1;
            }
        }



        // Rounding Number 10
        private void roundNumberTen(MaskedTextBox txtBox)
        {
            if (txtBox.Text.Trim() != "")
            {
                string strRaw = txtBox.Text.Trim();
                if (strRaw != "")
                {
                    double valueDouble = Double.Parse(strRaw);
                    valueDouble = (Math.Round(valueDouble / 10, MidpointRounding.AwayFromZero) * 10);
                    txtBox.Text = valueDouble.ToString();
                }
            }
        }
        //


        private void SetComboboxRankStandard()
        {
            string message = "When Laze is checked. So Rank can not have A or B value.\n Please fill again";
            if (chkManuTarepan.Checked == true)
            {
                string text = this.getSelectedTextCombobox(this.getSelectedItem(cboStandardRank));
                if (text.Contains("A") | text.Contains("B"))
                {
                    cboStandardRank.SelectedIndex = -1;

                    MessageBox.Show(this, message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void txtPaintSquare1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                roundNumberTen(txtPaintSquare1);
            }
        }
        private void txtPaintSquare2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                roundNumberTen(txtPaintSquare2);
            }

        }

        private void txtPaintSquare1_MouseLeave(object sender, EventArgs e)
        {
            roundNumberTen(txtPaintSquare1);
        }
        private void txtPaintSquare2_MouseLeave(object sender, EventArgs e)
        {
            roundNumberTen(txtPaintSquare2);
        }
        private string getSelectedTextCombobox(string inputText)
        {
            if (inputText == null || inputText.Equals(""))
            {
                return "";
            }
            else
            {
                string textSplit1 = "Text = ";
                string textSplit2 = ", Value";
                string inputText1 = inputText.Split(new[] { textSplit1 }, System.StringSplitOptions.None)[1];
                string inputText2 = inputText1.Split(new[] { textSplit2 }, System.StringSplitOptions.None)[0];
                return inputText2;
            }
        }

        private string getSelectedIndexCombobox(string inputText)
        {
            if (inputText == null || inputText.Equals(""))
            {
                return "";
            }
            else
            {
                string textSplit1 = "Value =";
                string textSplit2 = "}";
                string inputText1 = inputText.Split(new[] { textSplit1 }, System.StringSplitOptions.None)[1];
                string inputText2 = inputText1.Split(new[] { textSplit2 }, System.StringSplitOptions.None)[0];
                inputText2 = inputText2.Replace(" ", "");
                return inputText2;
            }
        }


        //Get GetDataTextGridViewStyleSelect
        private string GetDataTextGridViewStyleSelect(string styleSelect, int indexRowSelected, int indexColumnSelected)
        {
            try
            {
                string objectText = "";
                DataGridViewRow row = grvMaterial.Rows[indexRowSelected];
                styleSelect.ToLower();
                if (row != null)
                {

                    if (styleSelect == "textbox")
                    {
                        DataGridViewTextBoxCell objectCell = (DataGridViewTextBoxCell)row.Cells[indexColumnSelected];
                        objectText = objectCell.FormattedValue.ToString();
                        string objectValue = objectCell.Value + "";
                    }
                    else if (styleSelect == "combobox")
                    {
                        DataGridViewComboBoxCell objectCell = (DataGridViewComboBoxCell)row.Cells[indexColumnSelected];
                        objectText = objectCell.FormattedValue.ToString();
                        string objectValue = objectCell.Value + "";
                    }
                    else
                    {
                        DataGridViewComboBoxCell objectCell = (DataGridViewComboBoxCell)row.Cells[indexColumnSelected];
                        objectText = objectCell.FormattedValue.ToString();
                        string objectValue = objectCell.Value + "";
                    }

                }
                return objectText;

            }

            catch (Exception e)
            {
                Console.WriteLine(e + "It is not combobox in ListView or Combobox");
                return "";
            }
        }

        //SetDataTextBoxGridView
        private void SetDataTextBoxGridView(int indexRowSelected, int indexColumnSelected, string contentTextBox)
        {
            try
            {
                DataGridViewRow row = grvMaterial.Rows[indexRowSelected];
                if (row != null)
                {
                    DataGridViewTextBoxCell objectCell = (DataGridViewTextBoxCell)row.Cells[indexColumnSelected];
                    objectCell.Value = contentTextBox;

                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e + "Error when set textbox" + indexRowSelected + "-" + indexColumnSelected);
            }
        }

        //SetDataComboboxGridView
        private void SetDataComboboxGridView(int indexRowSelected, int indexColumnSelected/*, int ComboboxValue*/)
        {
            try
            {
                DataGridViewRow row = grvMaterial.Rows[indexRowSelected];
                if (row != null)
                {
                    DataGridViewComboBoxCell objectCell = (DataGridViewComboBoxCell)row.Cells[indexColumnSelected].EditedFormattedValue;
                    //objectCell.Value = ComboboxValue;
                    //objectCell.ToolTipText = "AAAAAAAAA";
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e + "Error when set combobox" + indexRowSelected + "-" + indexColumnSelected);
            }
        }

        private void txtSurefaceWeight1_KeyPress(object sender, KeyPressEventArgs e)
        {

            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from Nam.NV
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess1, false);
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess1, "008", txtSurefaceWeight1, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);

            }

        }

        private void txtSurefaceWeight2_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from Nam.NV
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess2, false);
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess2, "008", txtSurefaceWeight2, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
            }

        }

        private void txtSurefaceWeight3_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess3, "008", txtSurefaceWeight3, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
                // Request from Nam.NV
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess3, false);
            }
        }

        private void txtSurefaceWeight4_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess4, "008", txtSurefaceWeight4, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess3, false);
            }
        }

        private void txtSurefaceWeight5_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess5, "008", txtSurefaceWeight5, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess3, false);
            }
        }

        private void txtSurefaceWeight6_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess6, "008", txtSurefaceWeight6, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess3, false);
            }
        }

        private void txtSurefaceWeight7_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess7, "008", txtSurefaceWeight7, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess3, false);
            }
        }

        private void txtSurefaceWeight8_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess8, "008", txtSurefaceWeight8, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess3, false);
            }
        }

        private void txtSurefaceWeight9_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess9, "008", txtSurefaceWeight9, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess3, false);
            }
        }

        private void txtSurefaceWeight10_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
            //-----------------------------------------
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                // Request from MrNamNV
                this.ComboboxWithRule(cboSurfaceProcess10, "008", txtSurefaceWeight10, 30, txtSurefaceMessage, WARNING_BAFU_LINK_MASKTEXTBOX_30);
                this.CheckListComboboxRequireFromNamNV(cboSurfaceProcess3, false);
            }
        }

        private void txtSurefaceMoney1_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney2_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney3_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney4_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney5_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney6_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney7_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney8_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney9_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney10_KeyPress(object sender, KeyPressEventArgs e)
        {
            System.Globalization.CultureInfo c = System.Globalization.CultureInfo.CurrentUICulture;
            char dot = (char)c.NumberFormat.NumberDecimalSeparator[0];
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !e.KeyChar.Equals(dot) && !e.KeyChar.Equals(SPACE))
                e.Handled = true;
        }

        private void txtSurefaceMoney1_Enter_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney1_Leave_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney2_Enter_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney2_Leave_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney3_Enter_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney3_Leave_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney5_Enter_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender); selectAllTextBox(sender);
        }

        private void txtSurefaceMoney5_Leave_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney6_Enter_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney6_Leave_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney7_Enter_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney7_Leave_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney8_Enter_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney8_Leave_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney9_Enter_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney9_Leave_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney10_Enter_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney10_Leave_1(object sender, EventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney1_MouseClick_1(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney3_MouseClick_1(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney4_MouseClick(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney5_MouseClick_1(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney6_MouseClick_1(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney7_MouseClick_1(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney8_MouseClick_1(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney9_MouseClick_1(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }

        private void txtSurefaceMoney10_MouseClick_1(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }
        // Compare combobox with one Value and show MessageBox
        private void ComboboxWithRule(ComboBox cboCheck, string contain_String, MaskedTextBox maskTextBox, int valueCompare, TextBox textbox, string messageTextBoxAndMessagebox)
        {
            try
            {
                string allText = cboCheck.SelectedItem.ToString();
                string selectedText = this.getSelectedTextCombobox(allText);
                if (selectedText.Contains(contain_String))
                {
                    int valueInt;
                    bool check;
                    check = Int32.TryParse(maskTextBox.Text, out valueInt);
                    if (valueInt >= valueCompare)
                    {

                    }
                    else
                    {
                        textbox.ResetText();
                        textbox.Text = messageTextBoxAndMessagebox;
                        MessageBox.Show(this, messageTextBoxAndMessagebox, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        maskTextBox.ResetText();
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private string getSelectedItem(ComboBox cbo)
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

        private string GetDataMaskTextBoxMathCombobox(int indexCombobox)
        {
            switch (indexCombobox + 1)
            {
                case 1:
                    return txtSurefaceWeight1.Text;
                    break;
                case 2:
                    return txtSurefaceWeight2.Text;
                    break;
                case 3:
                    return txtSurefaceWeight3.Text;
                    break;
                case 4:
                    return txtSurefaceWeight4.Text;
                    break;
                case 5:
                    return txtSurefaceWeight5.Text;
                    break;
                case 6:
                    return txtSurefaceWeight6.Text;
                    break;
                case 7:
                    return txtSurefaceWeight7.Text;
                    break;
                case 8:
                    return txtSurefaceWeight8.Text;
                    break;
                case 9:
                    return txtSurefaceWeight9.Text;
                    break;
                case 10:
                    return txtSurefaceWeight10.Text;
                    break;

                default:
                    return "";
                    break;
            }
        }
        private void CheckListComboboxRequireFromNamNV(ComboBox currentComboboxCheck, bool modeResetComboboxDuplicate = true)
        {
            try
            {
                string cboCheck1 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess1));
                string cboCheck2 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess2));
                string cboCheck3 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess3));
                string cboCheck4 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess4));
                string cboCheck5 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess5));
                string cboCheck6 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess6));
                string cboCheck7 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess7));
                string cboCheck8 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess8));
                string cboCheck9 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess9));
                string cboCheck10 = this.getSelectedTextCombobox(getSelectedItem(cboSurfaceProcess10));

                List<string> listCombobox = new List<string>();
                List<string> value_008 = new List<string>();
                List<string> value_108 = new List<string>();

                int count1 = 0;
                int count2 = 0;
                listCombobox.Add(cboCheck1);
                listCombobox.Add(cboCheck2);
                listCombobox.Add(cboCheck3);
                listCombobox.Add(cboCheck4);
                listCombobox.Add(cboCheck5);
                listCombobox.Add(cboCheck6);
                listCombobox.Add(cboCheck7);
                listCombobox.Add(cboCheck8);
                listCombobox.Add(cboCheck9);
                listCombobox.Add(cboCheck10);
                //=================================
                for (int i = 0; i < listCombobox.Count; i++)
                {
                    if (listCombobox[i].Contains("008"))
                    {
                        value_008.Add(this.GetDataMaskTextBoxMathCombobox(i));
                        count1++;
                    }
                    if (listCombobox[i].Contains("108"))
                    {
                        value_108.Add(this.GetDataMaskTextBoxMathCombobox(i)); ;
                        count2++;
                    }
                }
                //==================================
                if (count1 > 1)
                {
                    string message1 = "Chỉ được nhập 1 lần xử lý bề mặt bằng 008";
                    if (modeResetComboboxDuplicate) currentComboboxCheck.ResetText();
                    MessageBox.Show(this, message1, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtSurefaceMessage.Text = message1;
                    count1 -= 1;
                }

                if (count2 > 1)
                {
                    string message1 = "Chỉ được nhập 1 lần xử lý bề mặt bằng 108";
                    if (modeResetComboboxDuplicate) currentComboboxCheck.ResetText();
                    MessageBox.Show(this, message1, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtSurefaceMessage.Text = message1;
                    count2 -= 1;
                }
                // Check condition compare value between 008 combobox and 108 combobox
                // Kiểm tra điều kiện giữa combobox 008 và 108 giá trị combobox 108 phải lớn hơn 009 ở trường Lượng
                if (count1 == 1 & count2 == 1)
                {

                    string message2 = "Chú ý bạn phải nhập Lượng ở 008 (Xử lý bề mặt) > 108(Xử lý bề mặt)";
                    string text__008 = "";
                    string text__108 = "";
                    int x__008 = 0;
                    /*************/
                    if (value_008[0].Equals("") | value_008 == null)
                    {
                        text__008 = "0";
                    }
                    else
                    {
                        text__008 = value_008[0];
                    }
                    //
                    if (value_108[0].Equals("") | value_108 == null)
                    {
                        text__108 = "0";
                    }
                    else
                    {
                        text__108 = value_108[0];
                    }
                    /*************/
                    Int32.TryParse(text__008, out x__008);
                    int x__108 = 0;
                    Int32.TryParse(text__108, out x__108);
                    if (x__008 < x__108)
                    {
                        MessageBox.Show(this, message2, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtSurefaceMessage.Text = message2;
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, CommonsVars.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }


        private void txtSurefaceMoney1_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceMoney2_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceMoney3_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceMoney4_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceMoney5_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceMoney6_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceMoney7_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceMoney8_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceMoney9_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceMoney10_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void txtSurefaceWeight8_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnSTATISTICAL_Click(object sender, EventArgs e)
        {
            frmSTATISTICAL frmStatistical = new frmSTATISTICAL();
            frmStatistical.ShowDialog(this);
        }

        private void txtSurefaceMoney2_MouseClick_1(object sender, MouseEventArgs e)
        {
            selectAllTextBox(sender);
            trimTextBox(sender);
        }
    }
}
