using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Data;
using System.Windows.Forms;
using static DVLD_Buisness.clsTestType;

namespace DVLD.Test
{
    public partial class frmTestAppointments : Form
    {

        private int _LocalDrivingLicenseApplicationID;

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private DataTable _dtLicenseTestAppointments;

        public frmTestAppointments(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeID = TestType;

        }
        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestTypeID)
            {

                case clsTestType.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }

                case clsTestType.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestType.enTestType.PracticalTest:
                    {
                        lblTitle.Text = "Street Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
            }
        }

        private void frmTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();


            ctrlLocalDraivingLicenseApplicationInfo1.LoadLocalDraivingLicenseApplicationInfo(_LocalDrivingLicenseApplicationID);
            _dtLicenseTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestTypeID);
            dgvLicenseTestAppointments.DataSource = _dtLicenseTestAppointments;
            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();

            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;

                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            clsLocalDrivingApplication localDrivingLicenseApplication = clsLocalDrivingApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);


            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            
            //---
            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestTypeID);

            if (LastTest == null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
                frm1.ShowDialog();
                frmTestAppointments_Load(null, null);
                return;
            }
        
            //if person already passed the test s/he cannot retak it.
            if (LastTest.TestResult == clsTest.Result.Pass)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest
                (LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestTypeID);
            frm2.ShowDialog();
            frmTestAppointments_Load(null, null);
            //---
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID , TestAppointmentID);
            frm.ShowDialog();
            frmTestAppointments_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            //frm frm = new frmTakeTest(TestAppointmentID, _TestTypeID );
            //frm.ShowDialog();
            //frmListTestAppointments_Load(null, null);

        }
    }
}
