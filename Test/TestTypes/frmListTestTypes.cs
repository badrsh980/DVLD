using DVLD_Buisness;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLD.Applications.TestTypes
{
    public partial class frmListTestTypes : Form
    {

        DataTable _dtAllTestTypes;

        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private void ListTestTypes_Load(object sender, EventArgs e)
        {
            _dtAllTestTypes = clsTestType.GetAllApplicationTypes();
            dgvTestTypes.DataSource = _dtAllTestTypes;
            lblRecordsCount.Text = dgvTestTypes.RowCount.ToString();

            if (dgvTestTypes.Rows.Count > 0)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width = 110;
                dgvTestTypes.Columns[0].Visible = false;

                dgvTestTypes.Columns[1].HeaderText = "Title";
                dgvTestTypes.Columns[1].Width = 200;

                dgvTestTypes.Columns[2].HeaderText = "Description";
                dgvTestTypes.Columns[2].Width = 650;

                dgvTestTypes.Columns[3].HeaderText = "Fees";
                dgvTestTypes.Columns[3].Width = 100;
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditTestTypes frmEditTestTypes = new frmEditTestTypes((int)dgvTestTypes.SelectedCells[0].Value);
            frmEditTestTypes.ShowDialog();
            ListTestTypes_Load(null, null);
        }
    }
}
