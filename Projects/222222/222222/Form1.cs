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
using System.Globalization;

namespace _222222
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-FBKFN4T\SQLEXPRESS;Initial Catalog=TEST;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
            savein.Visible = false;
            saveup.Visible = false;
            allDisabled();
            
        }

        //tất cả input tools disabled
        private void allDisabled()
        {
            hoten.Enabled = false;
            mssv.Enabled = false;
            lop.Enabled = false;
            dateTimePicker1.Enabled = false;
            namrbtn.Enabled = false;
            nurbtn.Enabled = false;
            cxdrbtn.Enabled = false;
            comboBox1.Enabled = false;
            sdt.Enabled = false;
            email.Enabled = false;
        }

        //tất cả input tools enabled
        private void allEnabled()
        {
            hoten.Enabled = true;
            mssv.Enabled = true;
            lop.Enabled = true;
            dateTimePicker1.Enabled = true;
            namrbtn.Enabled = true;
            nurbtn.Enabled = true;
            cxdrbtn.Enabled = true;
            comboBox1.Enabled = true;
            sdt.Enabled = true;
            email.Enabled = true;
        }

        //làm input tools empty 
        private void clearData()
        {
            hoten.Text = "";
            mssv.Text = "";
            lop.Text = "";
            dateTimePicker1.Text = "";
            namrbtn.Checked = false;
            nurbtn.Checked = false;
            cxdrbtn.Checked = false;
            comboBox1.Text = "+84";
            sdt.Text = "";
            email.Text = "";
        }

        //làm cho sđt chỉ nhập được số
        private void sdt_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
                MessageBox.Show("Chỉ nhập được số vào ô này.");
            }
        }

        //lấy string cho radio button
        string gioitinh;
        private void namrbtn_CheckedChanged(object sender, EventArgs e)
        {
            gioitinh = "Nam";
        }
        private void nurbtn_CheckedChanged(object sender, EventArgs e)
        {
            gioitinh = "Nu";
        }
        private void cxdrbtn_CheckedChanged(object sender, EventArgs e)
        {
            gioitinh = "Chua xac dinh";
        }

        //check vao o radio button
        private void checkGioitinh()
        {
            if (dataGridView1.SelectedRows[0].Cells[4].Value.ToString() == "Nam")
                namrbtn.Checked = true;
            if (dataGridView1.SelectedRows[0].Cells[4].Value.ToString() == "Nu")
                nurbtn.Checked = true;
            if (dataGridView1.SelectedRows[0].Cells[4].Value.ToString() == "Chua xac dinh")
                cxdrbtn.Checked = true;
        }
        //lấy data từ datagridview
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                hoten.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                mssv.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                lop.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                dateTimePicker1.Text =dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                checkGioitinh();
                comboBox1.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                sdt.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                email.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
        }

        //hiển thị data
       
        private void display()
        {
            string Query = " select * from Table_1";
            SqlDataAdapter SDA = new SqlDataAdapter(Query, conn);
            DataTable dt = new DataTable();
            SDA.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        //INSERT
        //click "Thêm" 
        private void insert_Click(object sender, EventArgs e)
        {
            insert.Visible = false;
            update.Visible = false;
            delete.Visible = false;
            savein.Visible = true;
            clearData();
            allEnabled();
        }
        //làm chữ hoa
        private string CapitalizeFirstLetters()
        {
            char[] array = hoten.Text.ToCharArray();
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        //check xem điền đúng sdt chưa
        private int check_sdt()
        {
            char[] array = sdt.Text.ToCharArray();
            if (array[0] == '0')
            {
                return 0;
            }
            return 1;
        }
        private int check_email()
        {
           SqlCommand  cmd = new SqlCommand("select * from Table_1 where Email='"+email.Text+"'",conn);
           SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0)
                return 0;
            else return 1;
        }
        //click "Lưu" của insert
        private void savein_Click(object sender, EventArgs e)
        {
            int checkSDT=check_sdt();
            int checkEmail = check_email();
            if (namrbtn.Checked == false && nurbtn.Checked == false && cxdrbtn.Checked == false)
            {
                cxdrbtn.Checked = true;
            }
            if (checkEmail == 0)
            {
                MessageBox.Show("Email bị trùng. Mời bạn kiểm tra và điền lại đầy đủ.");
                email.Clear();
            }
            else { 
                if (checkSDT == 0)
                {
                    MessageBox.Show("Số điện thoại không được có số 0 ở đầu. Mời bạn kiểm tra và điền lại đầy đủ.");
                    sdt.Clear();
                }
                else
                {
                    if (hoten.Text == "" || mssv.Text == "" || lop.Text == "" || sdt.Text == "" || email.Text == "")
                    {
                        MessageBox.Show("Bạn điền thiếu thông tin. Mời bạn kiểm tra và điền lại đầy đủ.");
                    }
                    else
                    {
                        hoten.Text = CapitalizeFirstLetters();

                        try
                        {
                            conn.Open();
                            string query = "insert into Table_1 (HoTen, MSSV, Lop, NgaySinh, GioiTinh, MaVung, SDT,Email) values ('" + hoten.Text + "','" + mssv.Text + "','" + lop.Text + "', '" + dateTimePicker1.Text + "', '" + gioitinh + "', '" + comboBox1.Text + "', '" + sdt.Text + "','" + email.Text + "')";
                            SqlDataAdapter SDA = new SqlDataAdapter(query, conn);
                            SDA.SelectCommand.ExecuteNonQuery();
                        }
                        catch
                        {
                            MessageBox.Show("MSSV bị trùng. Mời bạn điền lại form mới.");
                            mssv.Clear();
                        }
                        conn.Close();
                        display();
                        clearData();
                        allDisabled();
                        insert.Visible = true;
                        update.Visible = true;
                        delete.Visible = true;
                        savein.Visible = false;
                    }
                }
            }
        }
        
        //DELETE
        //click "Xóa"
        private void delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = "delete from Table_1 where mssv= '"+mssv.Text+"' ";
            SqlDataAdapter SDA = new SqlDataAdapter(query, conn);
            SDA.SelectCommand.ExecuteNonQuery();
            display();
            conn.Close();
        }

        //UPDATE
        //click "Sửa"
        private void update_Click(object sender, EventArgs e)
        {
            allEnabled();
            update.Visible = false;
            insert.Visible = false;
            delete.Visible = false;
            saveup.Visible = true;
        }
        private int check_mssv()
        {
            SqlCommand cmd = new SqlCommand("select * from Table_1 where MSSV='" + mssv.Text + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int i = ds.Tables[0].Rows.Count;
            if (i > 0)
                return 0;
            else return 1;
        }
        //click "Lưu" của update
        private void saveup_Click(object sender, EventArgs e)
        {
                hoten.Text = CapitalizeFirstLetters();
                conn.Open();
                string query = "update Table_1 set HoTen='" + hoten.Text + "', MSSV= '" + mssv.Text + "', Lop= '" + lop.Text + "', NgaySinh= '" + dateTimePicker1.Text + "', GioiTinh= '" + gioitinh + "', MaVung= '" + comboBox1.Text + "', SDT='" + sdt.Text + "', Email='" + email.Text + "' where mssv= '" + mssv.Text + "' ";
                SqlDataAdapter SDA = new SqlDataAdapter(query, conn);
                SDA.SelectCommand.ExecuteNonQuery();
                display();
                conn.Close();
                allDisabled();
                update.Visible = true;
                insert.Visible = true;
                delete.Visible = true;
                saveup.Visible = false;
        }
    }
}
