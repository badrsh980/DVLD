using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.License.Controls
{
    public partial class ctrlLicenseInfo : UserControl
    {
        clsLicense License;
        clsLocalDrivingApplication LocalDrivingApplication;
        public ctrlLicenseInfo()
        {
            InitializeComponent();
        }
        public void LoadData(int LocalDrivingLicesnse)
        {
            LocalDrivingApplication= clsLocalDrivingApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicesnse);
            License  = clsLicense.FindByApplicationID(LocalDrivingApplication.ApplicationID);
            lblClass.Text = LocalDrivingApplication.LicenseClassInfo.ClassName;
            lblFullName.Text= License.DriverInfo.personInfo.FullName;
            lblLicenseID.Text= License.LicenseID.ToString();
            lblNationalNO.Text= License.DriverInfo.personInfo.NationalNo;
            lblGendor.Text = (License.DriverInfo.personInfo.Gender == 0) ? "Male" : "Femail";
            lblIssueDate.Text = License.IssueDate.ToShortTimeString();
            lblIssueReason.Text = License.IssueReasonText;
            lblNotes.Text= License.Notes.ToString();
            lblIsActive.Text= License.IsActive==true ? "Yes" :"No";
            lblDateOfBirth.Text = License.DriverInfo.personInfo.DateOfBirth.ToShortTimeString();
            lblDriverID.Text = License.DriverID.ToString();
            lblExpiraionDate.Text=License.ExpirationDate.ToShortTimeString();
            if (License.DriverInfo.personInfo.Gender == 0)
            {
                pbPerson.Image = Resources.Male_512;
                pbGendor.Image = Resources.Man_32;

            }
            else
            {
                pbPerson.Image = Resources.Female_512;
                pbGendor.Image = Resources.Woman_32;

            }
            string ImagePath = License.DriverInfo.personInfo.ImagePath;
            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPerson.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //NEED TO WORK LEATER
            lblIsDetained.Text = "NOT WORKING NEED TO WORK FOR Detained CLASS";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
