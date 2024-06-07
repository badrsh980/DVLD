using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Buisness.clsLicense;

namespace DVLD.Applications.Local_Driving_License
{

    
    public partial class frmIssueDriverLicenseForTheFirstTime : Form
    {
        private int DrivingLicenseApplicationID=-1;
        clsLocalDrivingApplication _LocalDrivingLicenseApplication;
        public frmIssueDriverLicenseForTheFirstTime(int DrivingLicenseApplicationID)
        {
            InitializeComponent();

            this.DrivingLicenseApplicationID = DrivingLicenseApplicationID;
            ctrlLocalDraivingLicenseApplicationInfo1.LoadLocalDraivingLicenseApplicationInfo(DrivingLicenseApplicationID);
            _LocalDrivingLicenseApplication = clsLocalDrivingApplication.FindByLocalDrivingLicenseApplicationID(this.DrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Applicaiton with ID=" + DrivingLicenseApplicationID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (!_LocalDrivingLicenseApplication.PassedAllTests())
            {

                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            if (LicenseID != -1)
            {

                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            int LicenseID = _LocalDrivingLicenseApplication.IssueLicenseForTheFirtTime(txtBox.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show("License Issued Successfully with License ID = " + LicenseID.ToString(),
                    "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            else
            {
                MessageBox.Show("License Was not Issued ! ",
                 "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void issueDriverLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
        }

        private void ctrlLocalDraivingLicenseApplicationInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
