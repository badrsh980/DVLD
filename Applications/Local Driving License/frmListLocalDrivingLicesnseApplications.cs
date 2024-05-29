using DVLD.Applications;
using DVLD.Applications.Local_Driving_License;
using DVLD.Test;
using DVLD_Buisness;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class frmListLocalDrivingLicesnseApplications : Form
    {
        private static DataTable _dtAllDrivingApplication = clsLocalDrivingApplication.GetAllLocalDrivingLincense();


        private void _RefreshPeoplList()
        {
            _dtAllDrivingApplication = clsLocalDrivingApplication.GetAllLocalDrivingLincense();
            dgvLocalDrivingLicenseApplications.DataSource = _dtAllDrivingApplication;
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }


        public frmListLocalDrivingLicesnseApplications()
        {
            InitializeComponent();

        }


        private void frmListLocalDrivingLicesnseApplications_Load(object sender, EventArgs e)
        {
            _RefreshPeoplList();
            dgvLocalDrivingLicenseApplications.DataSource = _dtAllDrivingApplication;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();

            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {
                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "Local Driving License ApplicationID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width = 200;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Class Name";
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 300;

                dgvLocalDrivingLicenseApplications.Columns[2].Width = 100;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 300;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 200;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Test Count";
                dgvLocalDrivingLicenseApplications.Columns[5].Width = 100;

                dgvLocalDrivingLicenseApplications.Columns[6].HeaderText = "Status";
                dgvLocalDrivingLicenseApplications.Columns[6].Width = 150;
                dgvLocalDrivingLicenseApplications.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvLocalDrivingLicenseApplications_CellFormatting);

            }


        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                case "Class Name":
                    FilterColumn = "ClassName";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Status":
                    FilterColumn = "Status";
                    break;
                default:
                    FilterColumn = "None";
                    break;

            }
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllDrivingApplication.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
                return;
            }

            if (FilterColumn == "LocalDrivingLicenseApplicationID")
            {
                _dtAllDrivingApplication.DefaultView.RowFilter = string.Format("[{0}]={1}", FilterColumn, txtFilterValue.Text.Trim());


            }
            else
            {
                _dtAllDrivingApplication.DefaultView.RowFilter = string.Format("[{0}]Like'{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            }
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDraivingLicenseApplicationInfo frm = new frmLocalDraivingLicenseApplicationInfo((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeoplList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeoplList();

        }

        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                clsLocalDrivingApplication LocalDrivingApplication = clsLocalDrivingApplication.FindByLocalDrivingLicenseApplicationID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
                //Perform Delele and refresh
                if (LocalDrivingApplication != null)
                {
                    if (LocalDrivingApplication.Delete())
                    {
                        MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _RefreshPeoplList();
                    }

                    else
                        MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CancelApplicaitonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Cancel Local Driving Application [" + dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value + "]", "Confirm Cancel", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                clsLocalDrivingApplication LocalDrivingApplication = clsLocalDrivingApplication.FindByLocalDrivingLicenseApplicationID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
                //Perform Delele and refresh
                if (LocalDrivingApplication != null)
                {
                    if (LocalDrivingApplication.SetCancel())
                    {
                        MessageBox.Show("Application Cancel Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _RefreshPeoplList();
                    }

                    else
                        MessageBox.Show("Person was not Cancel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



        }


        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            issueDriverLicenseForTheFirstTime frm = new issueDriverLicenseForTheFirstTime((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);

            frm.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication();
            frm.ShowDialog();
            _RefreshPeoplList();
        }

        private void frmListLocalDrivingLicesnseApplications_Activated(object sender, EventArgs e)
        {
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmTestAppointments frm = new frmTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.VisionTest);
            frm.ShowDialog();
            _RefreshPeoplList();

        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmTestAppointments frm = new frmTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.WrittenTest);
            frm.ShowDialog();
            _RefreshPeoplList();

        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmTestAppointments frm = new frmTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.PracticalTest);
            frm.ShowDialog();
            _RefreshPeoplList();

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmsApplications_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int numberOfPassTests = clsLocalDrivingApplication.PassedTestCounts((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            clsLocalDrivingApplication LocalDrivingLicenseApplication = clsLocalDrivingApplication.FindByLocalDrivingLicenseApplicationID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);


            int TotalPassedTests = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;

            bool LicenseExists = LocalDrivingLicenseApplication.IsLicenseIssued();

            //Enabled only if person passed all tests and Does not have license. 
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;

            showLicenseToolStripMenuItem.Enabled = LicenseExists;

            editToolStripMenuItem.Enabled = !LicenseExists && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enStatus.New);
            ScheduleTestsMenue.Enabled = !LicenseExists;

            //Enable/Disable Cancel Menue Item
            //We only canel the applications with status=new.
            CancelApplicaitonToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enStatus.New);

            //Enable/Disable Delete Menue Item
            //We only allow delete incase the application status is new not complete or Cancelled.
            DeleteApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enStatus.New);



            //Enable Disable Schedule menue and it's sub menue
            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest); ;
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.PracticalTest);

            ScheduleTestsMenue.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enStatus.New);

            if (ScheduleTestsMenue.Enabled == true)
            {
                //To Allow Schdule vision test, Person must not passed the same test before.
                scheduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                //To Allow Schdule written test, Person must pass the vision test and must not passed the same test before.
                scheduleWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                //To Allow Schdule steet test, Person must pass the vision * written tests, and must not passed the same test before.
                scheduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;

            }


        }

        private void dgvLocalDrivingLicenseApplications_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                if (e.Value != null)
                {
                    string cellValue = e.Value.ToString();

                    // Change color based on the value
                    if (cellValue == "Completed")
                    {
                        e.CellStyle.ForeColor = Color.Green; // Text color
                    }
                    else if (cellValue == "Cancelled")
                    {
                        e.CellStyle.ForeColor = Color.Red; // Text color
                    }
                    else if (cellValue == "New")
                    {
                        e.CellStyle.ForeColor = Color.Gray; // Text color
                    }
                }
            }
        }
    }
}