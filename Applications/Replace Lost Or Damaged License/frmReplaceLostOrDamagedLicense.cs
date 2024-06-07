using DVLD.Classes;
using DVLD.License.Controls;
using DVLD_Buisness;
using System;
using System.Windows.Forms;

namespace DVLD.Applications.Replace_Lost_Or_Damaged_License
{
    public partial class frmReplaceLostOrDamagedLicense : Form
    {
        private clsLicense License = null;
        private int LicenseID = -1;
        private clsLicense NewLicense = null;

        public frmReplaceLostOrDamagedLicense()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Damaged License";
            lblApplicationFees.Text = clsApplicationType.Find((int)_IsLostOrDamagedLicense()).Fees.ToString();

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Lost License";
            lblApplicationFees.Text = clsApplicationType.Find((int)_IsLostOrDamagedLicense()).Fees.ToString();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            LicenseID = obj; 

            if (LicenseID != -1)
            {
                License = clsLicense.Find(LicenseID);
                if (License != null)
                {
                    if (License.IsActive)
                    {
                        lblOldLicenseID.Text = License.LicenseID.ToString();
                        btnReplace.Enabled = true;
                    }
                    else
                    {
                        // The license is found but is not active, show a message box.
                        MessageBox.Show("The license is not active.", "Inactive License", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // No license is found with the given ID, show a message box.
                    MessageBox.Show("No license found with the provided ID.", "License Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmReplaceLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplacementforaDamagedDrivingLicense).Fees.ToString();
        }

        private clsLicense.enIssueReason _IsLostOrDamagedLicense()
        {
            return rbDamagedLicense.Checked ? clsLicense.enIssueReason.ReplacementForDamaged : clsLicense.enIssueReason.ReplacementForLost;
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Replace the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            NewLicense = License.Replace(_IsLostOrDamagedLicense(), clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Replace the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            llblShowLicenseInfo.Enabled = true;
            btnReplace.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            lblLicenseReplacementApplicationID.Text = NewLicense.ApplicationID.ToString();
            lblReplacedLicens.Text = NewLicense.LicenseID.ToString();
        }

        private void llblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(NewLicense.LicenseID);
            frm.ShowDialog();
        }
    }
}
