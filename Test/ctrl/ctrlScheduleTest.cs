using DVLD.Classes;
using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Windows.Forms;

namespace DVLD.Test.ctrl
{

    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;


        private clsLocalDrivingApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        public clsTestType.enTestType TestTypeID
        {
            get { return _TestTypeID; }

            set
            {

                _TestTypeID = value;

                switch (_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        {
                            gbTestTupeName.Text = "Vision Test";
                            pbScheduleTest.Image = Resources.Vision_512;
                            break;
                        }
                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbTestTupeName.Text = "Written Test";
                            pbScheduleTest.Image = Resources.Written_Test_32;
                            break;
                        }

                    case clsTestType.enTestType.PracticalTest:
                        {
                            gbTestTupeName.Text = "Practical Test";
                            pbScheduleTest.Image = Resources.driving_test_512;
                            break;
                        }
                }
            }
        }


        public ctrlScheduleTest()
        {
            InitializeComponent();
        }



        public void LoadData(int LocalDrivingLicenseApplicationID, int TestAppointmentID = -1)
        {
            if (TestAppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _TestAppointmentID = TestAppointmentID;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _LocalDrivingLicenseApplication = clsLocalDrivingApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Driving License Application with ID = " + _LocalDrivingLicenseApplicationID, " Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.FindForm().Close();
                btnSave.Enabled = false;
                return;
            }

            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;


            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                float ReTakeTestFees = clsApplicationType.Find((int)clsApplication.enApplicationType.Retaketest).Fees;
                lblRetakeFees.Text = ReTakeTestFees.ToString();
                gbRetakeTest.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblReTakeTestAppID.Text = "0";
                dtpTestDate.MinDate = DateTime.Now;

            }
            else 
            {
                lblRetakeFees.Text = "0";
                gbRetakeTest.Enabled = false;
                lblTitle.Text = "Schedule Retake Test";
                lblReTakeTestAppID.Text = "N/A";
                dtpTestDate.MinDate = DateTime.Now;

            }

            lblDLAppID.Text = _LocalDrivingLicenseApplication.ApplicationID.ToString();
            lblDraiverClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.PersonFullName;
            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTralsPerTest(_TestTypeID).ToString();


            if (_Mode == enMode.AddNew)
            {
                lblTestFees.Text = clsTestType.Find(_TestTypeID).Fees.ToString();
                dtpTestDate.MinDate = DateTime.Now;
                lblReTakeTestAppID.Text = "N/A";
                _TestAppointment = new clsTestAppointment();
            }
            else
            {

                if (!_LoadAppointmentData())
                    return;

            }
            lblTotalFees.Text = (Convert.ToSingle(lblTestFees.Text) + Convert.ToSingle(lblRetakeFees.Text)).ToString();


            if (!_HandleActiveTestAppointmentConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePrviousTestConstraint())
                return;


        }
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && clsLocalDrivingApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, _TestTypeID))
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }

            return true;
        }

        private bool _HandlePrviousTestConstraint()
        {

            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    lblUserMessage.Visible = false;

                    return true;

                case clsTestType.enTestType.WrittenTest:

                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }


                    return true;

                case clsTestType.enTestType.PracticalTest:

                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }


                    return true;

            }
            return true;

        }



        private bool _HandleAppointmentLockedConstraint()
        {

            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment loacked.";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;

            }
            else
                lblUserMessage.Visible = false;

            return true;
        }

        private bool _HandleRetakeApplication()
        {

            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {


                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.Retaketest;
                Application.ApplicationStatus = clsApplication.enStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.Retaketest).Fees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }else
                {

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
                }


            }
            return true;
        }


        private bool _LoadAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("No TestAppointment with ID = " + _TestAppointmentID, " Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.FindForm().Close();

                return false;
            }
            lblTestFees.Text = _TestAppointment.PaidFees.ToString();
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) > 0)
            {
                dtpTestDate.MinDate = _TestAppointment.AppointmentDate;
                dtpTestDate.Value = _TestAppointment.AppointmentDate;

            }
            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRetakeFees.Text = "0";
                lblReTakeTestAppID.Text = "N/A";

            }
            else
            {
                gbRetakeTest.Enabled = true;

                lblRetakeFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
                lblReTakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
                lblTitle.Text = "Schedule Retake Test";

            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblTestFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm().Close(); 
        }

        private void ctrlScheduleTest_Load(object sender, EventArgs e)
        {

        }
    }
}
