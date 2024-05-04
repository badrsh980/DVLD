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
            _Mode = enMode.Update;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
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
                btnSave.Enabled = true;
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

             int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;


            if (clsApplication.DoesPersonHaveActiveApplication(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {
                MessageBox.Show("Person already has an active application ID =  for this license class. Choose another license class.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
