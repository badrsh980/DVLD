using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Test.ctrl
{



    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;
        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;


        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsLocalDrivingApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;


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

     



    }
}
