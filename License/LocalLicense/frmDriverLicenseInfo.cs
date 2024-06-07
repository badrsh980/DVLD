using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.License.Controls
{
    public partial class frmDriverLicenseInfo : Form
    {
        int LicenseID;
        public frmDriverLicenseInfo(int LicenseID)
        {
            InitializeComponent();
            this.LicenseID = LicenseID;
        }

        private void frmDriverLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlLicenseInfo1.LoadData(LicenseID);
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
