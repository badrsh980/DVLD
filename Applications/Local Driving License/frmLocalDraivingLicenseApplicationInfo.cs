﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Local_Driving_License
{
    public partial class frmLocalDraivingLicenseApplicationInfo : Form
    {
        public frmLocalDraivingLicenseApplicationInfo(int LocalDraivingLicenseApplication)
        {
            InitializeComponent();
            ctrlLocalDraivingLicenseApplicationInfo1.LoadLocalDraivingLicenseApplicationInfo(LocalDraivingLicenseApplication);
        }

        private void frmLocalDraivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
