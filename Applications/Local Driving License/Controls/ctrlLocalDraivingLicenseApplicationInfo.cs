using DVLD.People;
using DVLD_Buisness;
using System;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.LinkLabel;

namespace DVLD.Applications.Local_Driving_License.Controls
{
    public partial class ctrlLocalDraivingLicenseApplicationInfo : UserControl
    {
        private clsLocalDrivingApplication _LocalDrivingApplication;


        public void LoadLocalDraivingLicenseApplicationInfo(int DrivingLicenseApplicationID)
        {
            _LocalDrivingApplication = clsLocalDrivingApplication.FindByLocalDrivingLicenseApplicationID(DrivingLicenseApplicationID);
            if (_LocalDrivingApplication == null)
            {
                ResetDrivingLicenseApplicationInfo();
                MessageBox.Show("No Person with PersonID = " + DrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillPersonInfo();
        }


        private void ResetDrivingLicenseApplicationInfo()
        {
            lblAppID.Text = "[????]";
            lblApplicantName.Text = "[????]";
            lblCreatedBy.Text = "[????]";
            lblDate.Text = "[????]";
            lblDLAppID.Text = "[????]";
            lblFees.Text = "[????]";
            lblPassedTest.Text = "[????]";
            lblStatus.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblType.Text = "[????]";
            AppliedForLicense.Text = "[????]";

        }


        public void _FillPersonInfo()
        {
            lblAppID.Text = _LocalDrivingApplication.ApplicationID.ToString();


            lblDate.Text = _LocalDrivingApplication.ApplicationDate.ToShortDateString();
            lblDLAppID.Text = _LocalDrivingApplication.LocalDrivingLicenseApplicationID.ToString();
            lblFees.Text = _LocalDrivingApplication.PaidFees.ToString();
            lblPassedTest.Text ="3/"+_LocalDrivingApplication.PassedTestCount.ToString();
            lblStatus.Text = _LocalDrivingApplication.StatusText;
            lblStatusDate.Text = _LocalDrivingApplication.LastStatusDate.ToShortDateString();



            AppliedForLicense.Text = _LocalDrivingApplication.LicenseClassInfo.ClassName;

             lblApplicantName.Text = _LocalDrivingApplication.PersonFullName;
             lblCreatedBy.Text = _LocalDrivingApplication.CreatedByUserInfo.UserName;
             lblType.Text = _LocalDrivingApplication.ApplicationTypeInfo.Title;
             llblShowLicenseInfo.Enabled = _LocalDrivingApplication.IsLicenseIssued();

        }

        public ctrlLocalDraivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void ctrlLocalDraivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {}

        private void llblViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_LocalDrivingApplication.ApplicantPerson.PersonID);
            frm.ShowDialog();


        }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}