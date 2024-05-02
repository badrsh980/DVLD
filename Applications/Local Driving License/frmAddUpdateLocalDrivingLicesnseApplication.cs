using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD.Applications
{


    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };

        clsLocalDrivingApplication clsLocalDrivingApplication;
        private enMode _Mode = enMode.AddNew;


        private int _ApplicantPersonID;
        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            clsLocalDrivingApplication = new clsLocalDrivingApplication();
        }

        public frmAddUpdateLocalDrivingLicesnseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            clsLocalDrivingApplication.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }

        private void _ResetDefualtValues()
        {
            //this will initialize the reset the defaule values
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Local Driving License Application";
                this.Text = "Add New Local Driving License Application";
                ctrlPersonCardWithFilter1.FilterFocus();
                btnSave.Enabled = true;

            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

            }
        }

        private void _LoadData()
        {

            ctrlPersonCardWithFilter1.FilterEnabled = false;
            clsLocalDrivingApplication = clsLocalDrivingApplication.FindByApplicationID(clsLocalDrivingApplication.LocalDrivingLicenseApplicationID);
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
            cbLicenseClass.SelectedText= clsLocalDrivingApplication.LicenseClass.ClassName; 
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblFees.Text= clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalDrivingLicenseService).Fees.ToString();
            
        }

        private void _FillCompoBox()
        {
            foreach (DataRow row in clsLicenseClass.GetAllLicenseClass().Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }







        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
            _FillCompoBox();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }



        }







        private void btnSave_Click(object sender, EventArgs e)
        {

            clsLocalDrivingApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            clsLocalDrivingApplication.ApplicationDate = DateTime.Now;
            clsLocalDrivingApplication.ApplicationTypeID = clsApplication.enApplicationType.NewLocalDrivingLicenseService;
            clsLocalDrivingApplication.ApplicationStatus = clsLocalDrivingApplication.enStatus.New;
            clsLocalDrivingApplication.LastStatusDate = DateTime.Now;
            clsLocalDrivingApplication.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalDrivingLicenseService).Fees;
            clsLocalDrivingApplication.UserID = clsGlobal.CurrentUser.UserID;
            clsLocalDrivingApplication.LicenseClassID = (int)clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;



            

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
            _ApplicantPersonID = obj;
        }
    }
}
