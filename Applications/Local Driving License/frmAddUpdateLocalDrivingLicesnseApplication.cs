using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Data;
using System.Windows.Forms;
using static DVLD_Buisness.clsLicenseClass;

namespace DVLD.Applications
{


    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };

        clsLocalDrivingApplication clsLocalDrivingApplication;
        private enMode _Mode = enMode.AddNew;


        private int LocalDrivingLicenseApplicationID= -1;


        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            clsLocalDrivingApplication = new clsLocalDrivingApplication();
        }

        public frmAddUpdateLocalDrivingLicesnseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _Mode = enMode.Update;
        }

        private void _ResetDefualtValues()
        {
            //this will initialize the reset the defaule values
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                ctrlPersonCardWithFilter1.FilterFocus();
                cbLicenseClass.SelectedIndex = 2;
                lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalDrivingLicenseService).Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                if (LocalDrivingLicenseApplicationID != -1)
                {
                    clsLocalDrivingApplication = clsLocalDrivingApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
                    lblTitle.Text = "Update Local Driving License Application";
                    this.Text = "Update Local Driving License Application";
                    _LoadData();
                }
         

            }
        }

        private void _LoadData()
        {

            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (clsLocalDrivingApplication == null)
            {
                MessageBox.Show("No Local Driving Application with ID = " + clsLocalDrivingApplication, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            //the following code will not be executed if the person was not found
            ctrlPersonCardWithFilter1.LoadPersonInfo(clsLocalDrivingApplication.ApplicantPersonID);
            lblLocalDrivingLicebseApplicationID.Text= clsLocalDrivingApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsLocalDrivingApplication.ApplicationDate.ToString();
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLocalDrivingApplication.LicenseClassInfo.ClassName);
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblFees.Text= clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalDrivingLicenseService).Fees.ToString();
            
        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = -1;
             LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(ctrlPersonCardWithFilter1.PersonID, clsApplication.enApplicationType.NewLocalDrivingLicenseService, LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }


            clsLocalDrivingApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            clsLocalDrivingApplication.ApplicationDate = DateTime.Now;
            clsLocalDrivingApplication.ApplicationTypeID = 1;

            clsLocalDrivingApplication.ApplicationStatus = clsApplication.enStatus.New;
            clsLocalDrivingApplication.LastStatusDate = DateTime.Now;
            clsLocalDrivingApplication.PaidFees = Convert.ToSingle(lblFees.Text);

            clsLocalDrivingApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            clsLocalDrivingApplication.LicenseClassID = LicenseClassID;



            

            if (clsLocalDrivingApplication.Save())
            {


                lblLocalDrivingLicebseApplicationID.Text = clsLocalDrivingApplication.LocalDrivingLicenseApplicationID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;

                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            //_ApplicantPersonID = obj;
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {


            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }


            //incase of add new mode.
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];

            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
               
            }
        }

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {
        
        
        
        
        }
        



        private void frmAddUpdateLocalDrivingLicesnseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
