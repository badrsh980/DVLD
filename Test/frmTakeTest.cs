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

namespace DVLD.Test
{

    
    public partial class frmTakeTest : Form
    {
       clsTest Test =new clsTest();
        private int TestAppointmentID=-1;
        clsTestType.enTestType TestTypeID;
        public frmTakeTest(int TestAppointmentID, clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
        }


        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            if (TestAppointmentID != -1)
            {
                ctrlScheduledTest1.TestTypeID = this.TestTypeID;
                ctrlScheduledTest1.LoadInfo(TestAppointmentID);
            }
            else
            {
                MessageBox.Show($"Error: No TestAppointmentID for {TestAppointmentID} ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Test.CreatedByUserID= clsGlobal.CurrentUser.UserID;
            Test.Notes = txtNotes.Text;
            Test.TestAppointmentID= TestAppointmentID;
            Test.TestID = TestAppointmentID;
            Test.TestResult = rbPass.Checked == true ? clsTest.Result.Pass : clsTest.Result.Fail;

            if (Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
