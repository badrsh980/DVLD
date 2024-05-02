using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.Applications.TestTypes
{
    public partial class frmEditTestTypes : Form
    {
        private clsTestType.TestType ID;

        private clsTestType Test;




        public frmEditTestTypes(int ID)
        {
            InitializeComponent();
            this.ID = (clsTestType.TestType)ID;
        }


        private void frmEditTestTypes_Load(object sender, EventArgs e)
        {
            Test = clsTestType.Find(ID);

            if (Test != null)
            {
                lblTestTypeID.Text = ((int)ID).ToString();
                txtTitle.Text = Test.Title;
                txtDescription.Text = Test.Description;
                txtFees.Text = Test.Fees.ToString();
            }
            else
            {
                this.Close();
            }
        }






        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(txtTitle, "Title cannot be empty!");
            }
            else
            {
                errorProvider.SetError(txtTitle, null);
            };
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(txtTitle, "Title cannot be Description!");
            }
            else
            {
                errorProvider.SetError(txtTitle, null);
            };
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(txtFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider.SetError(txtFees, null);
            };

            if (!clsValidatoin.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(txtFees, "Invalid Number.");
            }
            else
            {
                errorProvider.SetError(txtFees, null);
            };
        }





        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Test.Title = txtTitle.Text;
            Test.Description = txtDescription.Text;
            Test.Fees = Convert.ToSingle(txtFees.Text);

            if (Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }


    }
}
