using DVLD.Properties;
using DVLD_Buisness;
using System.IO;
using System.Windows.Forms;

namespace DVLD.License.Controls
{
    public partial class ctrlLicenseInfo : UserControl
    {
        private clsLicense License;
        private int _LicenseID;
        public ctrlLicenseInfo()
        {
            InitializeComponent();
        }
        public int LicenseID
        {
            get { return _LicenseID; }
        }
        public clsLicense SelectedLicenseInfo
        { get { return License; } }


        public void LoadData(int LicenseID)
        {
            _LicenseID = LicenseID;
            License = clsLicense.Find(_LicenseID);
            if (License== null)
            {
                MessageBox.Show($"No License with ID ApplicationID = {LicenseID}");
                return;
            }


            lblClass.Text = License.LicenseClassinfo.ClassName;
            lblFullName.Text= License.DriverInfo.personInfo.FullName;
            lblLicenseID.Text= License.LicenseID.ToString();
            lblNationalNO.Text= License.DriverInfo.personInfo.NationalNo;
            lblGendor.Text = (License.DriverInfo.personInfo.Gender == 0) ? "Male" : "Femail";
            lblIssueDate.Text = License.IssueDate.ToShortDateString();
            lblIssueReason.Text = License.IssueReasonText;
            lblNotes.Text= License.Notes.ToString();
            lblIsActive.Text= License.IsActive==true ? "Yes" :"No";
            lblDateOfBirth.Text = License.DriverInfo.personInfo.DateOfBirth.ToShortDateString();
            lblDriverID.Text = License.DriverID.ToString();
            lblExpiraionDate.Text=License.ExpirationDate.ToShortDateString();

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
            lblIsDetained.Text = License.IsLicenseDetained() ?"Yes":"no";
        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }
    }
}
