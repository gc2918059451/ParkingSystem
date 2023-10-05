using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace parking
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //退出按钮
            DialogResult result = MessageBox.Show("确定关闭吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sql sql = new Sql();//创建sql对象
            //1.验证用户名不能为空
            if (this.Username.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入用户名", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Username.Focus();//光标归位到用户名
                return;
            }
            //2.验证密码不能为空
            if (this.Password.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入密码", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Password.Focus();//光标归位到密码
                return;
            }
            //设置搜索语句
            string strsql = "select count(*) from admin where username='" + Username.Text + "' and password='" + Password.Text + "'";
            //连接数据库查询，若搜索到元素返回值>0
            int count = sql.Conn(strsql);
            if (count > 0)
            {
                MessageBox.Show("登录成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("用户名或密码不正确", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            menu menu = new menu();
            /*this.Hide();*/
            menu.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
