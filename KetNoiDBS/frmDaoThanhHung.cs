using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KetNoiDBS
{
    public partial class frmDaoThanhHung : Form
    {
        DataSet dset = new DataSet();
        SqlConnection conn;
        
       

        public frmDaoThanhHung()
        {
            InitializeComponent();
            conn = getConnection();
        }

        
        private SqlConnection getConnection()
        {
            string strConn = "Data Source=LAPTOP-60391V64\\SQLEXPRESS;Initial Catalog=QuanLi_Sinhvien;Integrated Security=True";
            SqlConnection conn = new SqlConnection(strConn);
            conn.Open();
            return conn;
        }

        private void frmDaoThanhHung_Load(object sender, EventArgs e)
        {

            this.loadCmbKhoa();
            this.loadLop();
            this.binding();
        }

        private void loadCmbKhoa()
        {
            string query = "select * from Khoa";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            adapter.Fill(dset, "Khoa");

            cmbKhoa.DataSource = dset.Tables["Khoa"];
            cmbKhoa.DisplayMember = "TenKhoa";
            cmbKhoa.ValueMember = "MaKhoa";
        }

        private void loadLop()
        {

            if (dset.Tables.Contains("Lop"))
                datagrdviewKhoa.DataSource = dset.Tables["Lop"];
            else
            {
                string query = "select * from Lop";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.Fill(dset, "Lop");
                datagrdviewKhoa.DataSource = dset.Tables["Lop"];
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string maKhoa = cmbKhoa.SelectedValue.ToString();
            string maLop = txtMaLop.Text;
            string tenLop = txtTenLop.Text;

            string query = "select * from Lop";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

            if (!dset.Tables.Contains("Lop"))
            {
                adapter.Fill(dset, "Lop");
                

            }

            DataRow newRow = dset.Tables["Lop"].NewRow();
            newRow["MaLop"] = maLop;
            newRow["TenLop"] = tenLop;
            newRow["MaKhoa"] = maKhoa;

            dset.Tables["Lop"].Rows.Add(newRow);

            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(dset, "Lop");

            this.loadLop();

            txtMaLop.Text = "";
            txtTenLop.Text = "";

            txtMaLop.Focus();


        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
           
            string query = "select * from Lop";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            string id = datagrdviewKhoa.SelectedCells[0].OwningRow.Cells[0].Value.ToString();

            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dset.Tables["Lop"].Columns["MaLop"];

            dset.Tables["Lop"].PrimaryKey = keyColumns;
            DataRow dr = dset.Tables["Lop"].Rows.Find(id);


            if (dr != null)
            {
                dr.Delete();
            }

            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(dset, "Lop");

            this.loadLop();

        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            string query = "select * from Lop";
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
         
            string id = datagrdviewKhoa.SelectedCells[0].OwningRow.Cells[0].Value.ToString();

            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dset.Tables["Lop"].Columns["MaLop"];
     

            dset.Tables["Lop"].PrimaryKey = keyColumns;
            DataRow dr = dset.Tables["Lop"].Rows.Find(id);
            
            if (dr != null)
            {
                dr["TenLop"] = txtTenLop.Text;
                dr["MaKhoa"] = cmbKhoa.SelectedValue;
            }
            

            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(dset, "Lop");
            this.loadLop();

        }

        private void binding()
        {
           
            txtMaLop.DataBindings.Clear();
            txtMaLop.DataBindings.Add("Text", datagrdviewKhoa.DataSource, "MaLop");
            txtTenLop.DataBindings.Clear();
            txtTenLop.DataBindings.Add("Text", datagrdviewKhoa.DataSource, "TenLop");
            cmbKhoa.DataBindings.Clear();
            cmbKhoa.DataBindings.Add("DisplayMember", datagrdviewKhoa.DataSource, "MaKhoa");
        }

        private void frmDaoThanhHung_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
            conn.Dispose();
            dset.Dispose();
            
        }
    }
}
