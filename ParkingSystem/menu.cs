using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;//和SQL相关的命名空间


namespace parking
{
    public partial class menu : Form
    {
        Time1 time = new Time1();//创建时间对象
        const string Sqllink = @"Data Source=LAPTOP-GLILAS4P;Initial Catalog=parking;User Id=sa;Password=123456";
        Sql sql = new Sql();//创建数据库对象
        //创建车牌号、车主姓名、车主电话、入库时间、出库时间、车位总数变量
        /*private char carnumber;
        private char carname;
        private char carphone;
        private char intime;
        private char outtime;
        */
        private int parkcount;//车库车位数量
        private int kongxian;//空闲车位数量
        public menu()
        {
            InitializeComponent();
        }

        public void garage_judge()//判断车库空闲车位数量
        {
            string strsql = "select parkcount from parkcount";
            string strsql2 = "select count(*) from garage";
            SqlConnection conn = new SqlConnection(Sqllink);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给parkcount
            {
                parkcount = Convert.ToInt32(read[0].ToString());
            }
            conn.Close();//关闭
            int count = sql.Conn(strsql2);//查询车库表中车辆数量
            kongxian = parkcount - count;//计算空余车位数量
        }

        public void gengxinchewei()
        {
            garage_judge();
            labparkcount.Text = kongxian.ToString() + "/" + parkcount.ToString();
        }

        private void menu_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
            gengxinchewei();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //parknum不能为空
            if (this.parknum.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车位数量", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.parknum.Focus();//光标归位到车牌号
                return;
            }
            int num = Convert.ToInt32(parknum.Text);//设置num=更改数量
            string strsql1 = "select count(*) from garage";
            string strsql2 = "update parkcount set parkcount=" + Convert.ToInt32(parknum.Text) + "";
            int count = sql.Conn(strsql1);//统计车库内车辆的数目
            if(num < count)//若更改数量小于库内车辆数目，则提示
            {
                MessageBox.Show("车位数量不足，请等待车辆出库后在进行更改", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.parknum.Focus();//光标归位到车牌号
                return;
            }
            else//若更改数量>=库内车辆数目，则进行更改，并刷新空闲车位数目
            {
                if (Convert.ToInt32(sql.Comm(strsql2)) > 0)
                {
                    MessageBox.Show("修改成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gengxinchewei();
                    parknum.Text = "";
                }
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //车牌号不能为空
            if (this.crcarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.crcarnumber.Focus();//光标归位到车牌号
                return;
            }
            //搜索并展示记录
            dataGridView2.Rows.Clear();
            string cph, czxm, czdh, rksj, cksj, tcfy, cllx;
            string strsql1 = "select carnumber,carname,carphone,intime,outtime,cost,cartype from history where carnumber='" + crcarnumber.Text + "'";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql1, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cph = read[0].ToString();
                czxm = read[1].ToString();
                czdh = read[2].ToString();
                rksj = read[3].ToString();
                cksj = read[4].ToString();
                tcfy = read[5].ToString();
                cllx = read[6].ToString();
                string[] table = { cph, czxm, czdh, rksj, cksj, tcfy, cllx };
                dataGridView2.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //验证车牌不能为空
            if (this.rucarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.rucarnumber.Focus();//光标归位到车牌号
                return;
            }
            //正确车牌号
            if (rucarnumber.Text.Length != 7)
            {
                MessageBox.Show("请输入7位正确车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //查询车辆信息
            //设置数据库查询语句
            string strsql1 = "select count(*) from car where carnumber='" + rucarnumber.Text + "'";
            string strsql2 = "select count(*) from garage where carnumber='" + rucarnumber.Text + "'";
            string strsql3 = "select carname,cartype,carphone from car where carnumber='" + rucarnumber.Text + "'";
            //从数据库中按车牌号查询是否有记录
            int count1 = sql.Conn(strsql1);
            int count2 = sql.Conn(strsql2);
            //若车辆表无记录则提示新车辆然后继续输入车主姓名、车主电话
            if (count1 <= 0)
            {
                MessageBox.Show("新车辆，请继续输入", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.rucarnumber.Focus();//光标归位到车牌号
                return;
            }
            //若车辆表有记录且车库表有记录则提示车辆已入库
            else if (count1 > 0 && count2 > 0)
            {
                MessageBox.Show("该车辆已入库", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.rucarnumber.Focus();//光标归位到车牌号
                return;
            }
            //若车辆表有记录，车库表无记录，则则自动填充姓名、电话，提示查询成功
            else if (count1 > 0 && count2 <= 0)
            {
                string connStr = Sqllink;//设置数据库连接方式
                SqlConnection conn = new SqlConnection(connStr);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql3, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给姓名、电话text
                {
                    rucarname.Text = read[0].ToString();
                    rucartype.Text = read[1].ToString();
                    rucarphone.Text = read[2].ToString();
                }
                conn.Close();//关闭
                MessageBox.Show("查询成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void ruku_Click(object sender, EventArgs e)
        {
            //车牌号不能为空
            if (this.rucarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.rucarnumber.Focus();//光标归位到车牌号
                return;
            }
            //车辆类型不能为空
            if (this.rucartype.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请选择车辆类型", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.rucartype.Focus();//光标归位到车主姓名
                return;
            }
            //车主姓名不能为空
            if (this.rucarname.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主姓名", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.rucarname.Focus();//光标归位到车主姓名
                return;
            }
            //车主电话不能为空
            if (this.rucarphone.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主电话", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.rucarphone.Focus();//光标归位到车主电话
                return;
            }
            //入库时间不能为空
            if (this.ruintime.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入入库时间", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ruintime.Focus();//光标归位到入库时间
                return;
            }
            //正确车牌号
            if(rucarnumber.Text.Length != 7)
            {
                MessageBox.Show("请输入7位正确车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //正确入库时间
            if(ruintime.Text.Length != 12)
            {
                MessageBox.Show("请输入12位日期格式", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //判断车库是否有空余车位
            if (kongxian == 0)//车位已满
            {
                MessageBox.Show("车位已满", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //查看库内是否有该车辆
            string strsql1 = "select count(*) from garage where carnumber='" + rucarnumber.Text + "'";
            int count = sql.Conn(strsql1);
            if (count > 0)//车库表有车辆
            {
                MessageBox.Show("该车辆已入库", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else//车库表无车辆，车库未满
            {
                string strsql2 = "select count(*) from car where carnumber='" + rucarnumber.Text + "'";
                int chongfu = sql.Conn(strsql2);
                if (chongfu > 0)
                {
                    //如果车辆表有车辆，则先更改车辆表中车辆信息，再在车库表中添加车辆信息，历史记录中添加信息
                    string strsql3 = "update car set carname='" + rucarname.Text + "',cartype='" + rucartype.Text + "', carphone='" + rucarphone.Text + "' where carnumber='" + rucarnumber.Text + "'";
                    string strsql4 = "insert into garage(carnumber,carname,carphone,intime,cartype) values('" + rucarnumber.Text + "','" + rucarname.Text + "','" + rucarphone.Text + "','" + ruintime.Text + "','" + rucartype.Text + "')";
                    string strsql5 = "insert into history(carnumber,carname,carphone,intime,cartype) values('" + rucarnumber.Text + "','" + rucarname.Text + "','" + rucarphone.Text + "','" + ruintime.Text + "','" + rucartype.Text + "')";
                    if (Convert.ToInt32(sql.Comm(strsql3)) > 0 && Convert.ToInt32(sql.Comm(strsql4)) > 0 && Convert.ToInt32(sql.Comm(strsql5)) > 0)
                    {
                        MessageBox.Show("入库成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gengxinchewei();
                        rucarnumber.Text = "";
                        rucartype.Text = "";
                        rucarname.Text = "";
                        rucarphone.Text = "";
                        ruintime.Text = "";
                    }
                }
                else//若果车辆表无车辆，则将车辆信息插入到车辆表，再在车库表中添加车辆信息，历史记录中添加信息
                {
                    string strsql3 = "insert into car(carnumber,carname,carphone,cartype) values('" + rucarnumber.Text + "','" + rucarname.Text + "','" + rucarphone.Text + "','" + rucartype.Text + "')";
                    string strsql4 = "insert into garage(carnumber,carname,carphone,intime,cartype) values('" + rucarnumber.Text + "','" + rucarname.Text + "','" + rucarphone.Text + "','" + ruintime.Text + "','" + rucartype.Text + "')";
                    string strsql5 = "insert into history(carnumber,carname,carphone,intime,cartype) values('" + rucarnumber.Text + "','" + rucarname.Text + "','" + rucarphone.Text + "','" + ruintime.Text + "','" + rucartype.Text + "')";
                    if (Convert.ToInt32(sql.Comm(strsql3)) > 0 && Convert.ToInt32(sql.Comm(strsql4)) > 0 && Convert.ToInt32(sql.Comm(strsql5)) > 0)
                    {
                        MessageBox.Show("入库成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gengxinchewei();
                        rucarnumber.Text = "";
                        rucartype.Text = "";
                        rucarname.Text = "";
                        rucarphone.Text = "";
                        ruintime.Text = "";
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //清空输入框内数据
            rucarnumber.Text = "";
            rucartype.Text = "";
            rucarname.Text = "";
            rucarphone.Text = "";
            ruintime.Text = "";
        }

        private void chuku_Click(object sender, EventArgs e)
        {
            //车牌号不能为空
            if (this.chucarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.chucarnumber.Focus();//光标归位到车牌号
                return;
            }
            //车辆类型不能为空
            if (this.chucartype.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车辆类型", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.chucartype.Focus();//光标归位到车辆类型
                return;
            }
            //出库时间不能为空
            if (this.chuouttime.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入出库时间", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.chuouttime.Focus();//光标归位到出库时间
                return;
            }
            //正确车牌号
            if (chucarnumber.Text.Length != 7)
            {
                MessageBox.Show("请输入7位正确车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //正确入库时间
            if (chuouttime.Text.Length != 12)
            {
                MessageBox.Show("请输入12位日期格式", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //通过检查入库时间、费用确保入库时间、费用更新
            if (this.chuintime.Text.Trim() == "点击查询获取入库时间" || this.chucost.Text.Trim() == "点击以获取费用")
            {
                //若未更新则提示
                MessageBox.Show("请更新入库时间或费用", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //通过车牌搜索数据库，若不存在则提示
            string strsql1 = "select count(*) from garage where carnumber='" + chucarnumber.Text + "'";
            int count = sql.Conn(strsql1);
            if(count <= 0)
            {
                MessageBox.Show("该车辆不在库中", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else//若存在，则根据车牌搜索数据库，比较车辆类型、入库时间是否一致
            {
                string type = "";
                string intime = "";
                //获取车辆类型、入库时间
                string strsql2 = "select cartype,intime from garage where carnumber='" + chucarnumber.Text + "'";
                SqlConnection conn = new SqlConnection(Sqllink);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql2, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋
                {
                    type = read[0].ToString();
                    intime = read[1].ToString();
                }
                conn.Close();//关闭

                string czxm = "";
                string czdh = "";
                string strsql01 = "select carname,carphone from car where carnumber='" + chucarnumber.Text + "'";
                SqlConnection conn1 = new SqlConnection(Sqllink);//连接数据库
                conn1.Open();//打开数据库
                SqlCommand comm1 = new SqlCommand(strsql01, conn1);//查询
                SqlDataReader read1 = comm1.ExecuteReader();
                while (read1.Read())//将查询结果赋
                {
                    czxm = read1[0].ToString();
                    czdh = read1[1].ToString();
                }
                conn1.Close();//关闭

                if (chucartype.Text != type || chuintime.Text != intime)//若不一致则提示
                {
                    MessageBox.Show("车量信息有误", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else//一致则进行出库操作，车库表中删除车辆信息、历史记录增加信息、更新空闲车位、置空车牌号、类型、intime、outime、cost
                {
                    
                    string strsql02 = "delete from garage where carnumber='" + chucarnumber.Text + "'";
                    string strsql03 = "insert into history(carnumber,carname,carphone,intime,outtime,cost,cartype) values('" + chucarnumber.Text + "','" + czxm + "','" + czdh + "','" + chuintime.Text + "','" + chuouttime.Text + "','" + chucost.Text + "','" + chucartype.Text +"')";
                    DialogResult result = MessageBox.Show("停车费用为：" + chucost.Text + "元\t确认出库吗？","提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        if (Convert.ToInt32(sql.Comm(strsql02)) > 0 && Convert.ToInt32(sql.Comm(strsql03)) > 0)
                        {
                            MessageBox.Show("出库成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            gengxinchewei();
                            chucarnumber.Text = "";
                            chucartype.Text = "";
                            chuintime.Text = "点击查询获取入库时间";
                            chuouttime.Text = "";
                            chucost.Text = "点击以获取费用";
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //查看库内所有车辆
            dataGridView1.Rows.Clear();
            string cph, czxm, czdh, rksj, cllx;
            string strsql = "select carnumber,carname,carphone,intime,cartype from garage";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cph = read[0].ToString();
                czxm = read[1].ToString();
                czdh = read[2].ToString();
                rksj = read[3].ToString();
                cllx = read[4].ToString();
                string[] table = { cph, czxm, czdh, rksj, cllx };
                dataGridView1.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //指定车辆车牌号不能为空
            if (this.ckcarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ckcarnumber.Focus();//光标归位到车牌号
                return;
            }
            //按车牌号搜索数据库

            /*//若无该车辆，提示未查到该车辆
            if(count <= 0)
            {
                MessageBox.Show("未查到该车辆", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }*/
            else//若有该车辆，则展示车辆信息
            {
                /*string strsql1 = "select count(*) from garage where carnumber='" + ckcarnumber.Text + "'";
                int count = sql.Conn(strsql1);*/
                dataGridView1.Rows.Clear();
                string cph, czxm, czdh, rksj, cllx;
                string strsql2 = "select carnumber,carname,carphone,intime,cartype from garage where carnumber='" + ckcarnumber.Text + "'";
                string connStr = Sqllink;//设置数据库连接方式
                SqlConnection conn = new SqlConnection(connStr);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql2, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间、车辆类型
                {
                    cph = read[0].ToString();
                    czxm = read[1].ToString();
                    czdh = read[2].ToString();
                    rksj = read[3].ToString();
                    cllx = read[4].ToString();
                    string[] table = { cph, czxm, czdh, rksj, cllx };
                    dataGridView1.Rows.Add(table);
                }
                conn.Close();//关闭
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //查看所有出入记录
            dataGridView2.Rows.Clear();
            string cph, czxm, czdh, rksj, cksj, tcfy, cllx;
            string strsql1 = "select carnumber,carname,carphone,intime,outtime,cost,cartype from history";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql1, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cph = read[0].ToString();
                czxm = read[1].ToString();
                czdh = read[2].ToString();
                rksj = read[3].ToString();
                cksj = read[4].ToString();
                tcfy = read[5].ToString();
                cllx = read[6].ToString();
                string[] table = { cph, czxm, czdh, rksj, cksj, tcfy, cllx };
                dataGridView2.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //车主电话不能为空
            if (this.crcarphone.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主电话", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.crcarphone.Focus();//光标归位到车主电话
                return;
            }
            //搜索并展示记录
            dataGridView2.Rows.Clear();
            string cph, czxm, czdh, rksj, cksj, tcfy, cllx;
            string strsql1 = "select carnumber,carname,carphone,intime,outtime,cost,cartype from history where carphone='" + crcarphone.Text + "'";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql1, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cph = read[0].ToString();
                czxm = read[1].ToString();
                czdh = read[2].ToString();
                rksj = read[3].ToString();
                cksj = read[4].ToString();
                tcfy = read[5].ToString();
                cllx = read[6].ToString();
                string[] table = { cph, czxm, czdh, rksj, cksj, tcfy, cllx };
                dataGridView2.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //判断车牌、姓名、电话不能为空
            //车牌号不能为空
            if (this.zccarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.zccarnumber.Focus();//光标归位到车牌号
                return;
            }
            //车主姓名不能为空
            if (this.zccarname.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主姓名", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.zccarname.Focus();//光标归位到车主姓名
                return;
            }
            //车主电话不能为空
            if (this.zccarphone.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主电话", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.zccarphone.Focus();//光标归位到车主电话
                return;
            }
            //判断是否已是会员
            string strsql1 = "select count(*) from member where carnumber='" + zccarnumber.Text + "'";
            string strsql2 = "insert into member(carnumber,carname,carphone) values('" + zccarnumber.Text + "','" + zccarname.Text + "','" + zccarphone.Text + "')";
            int count = sql.Conn(strsql1);
            if (count > 0)//如果已注册（会员表中有车牌信息）则提示已注册
            {
                MessageBox.Show("该车辆已注册会员", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else//如果未注册，则将信息导入会员表
            {
                if (Convert.ToInt32(sql.Comm(strsql2)) > 0)
                {
                    MessageBox.Show("注册成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    zccarnumber.Text = "";
                    zccarname.Text = "";
                    zccarphone.Text = "";
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //验证车牌号不能为空
            if (this.zxcarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.zxcarnumber.Focus();//光标归位到车牌号
                return;
            }
            //验证姓名、电话不能为空
            if (this.zxcarname.Text.Trim() == "点击查找获取车主姓名" || this.zxcarphone.Text.Trim() == "点击查找获取车主电话")
            {
                MessageBox.Show("请先进行查询操作", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //车牌、姓名、电话不为空后进行注销操作，注销后从会员表中删除车辆信息
            string strsql1 = "delete from member where carnumber='" + zxcarnumber.Text + "'";
            DialogResult result = MessageBox.Show("确认注销吗？", "提示信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                if (Convert.ToInt32(sql.Comm(strsql1)) > 0)
                {
                    MessageBox.Show("注销成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    zxcarnumber.Text = "";
                    zxcarname.Text = "点击查找获取车主姓名";
                    zxcarphone.Text = "点击查找获取车主电话";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //验证车牌号不能为空
            if (this.zxcarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.zxcarnumber.Focus();//光标归位到车牌号
                return;
            }
            //根据车牌号查找会员信息
            string strsql1 = "select count(*) from member where carnumber='" + zxcarnumber.Text + "'";
            string strsql2 = "select carname,carphone from member where carnumber='" + zxcarnumber.Text + "'";
            int count = sql.Conn(strsql1);
            //若无该车辆，提示未查到该车辆
            if (count > 0)//若找到，则将姓名、电话数据导入lab.text
            {
                string connStr = Sqllink;//设置数据库连接方式
                SqlConnection conn = new SqlConnection(connStr);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql2, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给姓名、电话text
                {
                    zxcarname.Text = read[0].ToString();
                    zxcarphone.Text = read[1].ToString();
                }
                conn.Close();//关闭
                MessageBox.Show("查询成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else//若未找到则提示
            {
                MessageBox.Show("未查到该车辆", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //查看会员表中所有会员信息
            dataGridView3.Rows.Clear();
            string cph, czxm, czdh;
            string strsql1 = "select carnumber,carname,carphone from member";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql1, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cph = read[0].ToString();
                czxm = read[1].ToString();
                czdh = read[2].ToString();
                string[] table = { cph, czxm, czdh };
                dataGridView3.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //通过车牌号查找会员信息
            //车牌号不能为空
            if (this.hycarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.hycarnumber.Focus();//光标归位到车牌号
                return;
            }
            dataGridView3.Rows.Clear();
            string cph, czxm, czdh;
            string strsql1 = "select carnumber,carname,carphone from member where carnumber='" + hycarnumber.Text + "'";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql1, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cph = read[0].ToString();
                czxm = read[1].ToString();
                czdh = read[2].ToString();
                string[] table = { cph, czxm, czdh };
                dataGridView3.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            //通过电话查找会员信息
            //车主电话不能为空
            if (this.hycarphone.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主电话", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.hycarphone.Focus();//光标归位到车牌号
                return;
            }
            dataGridView3.Rows.Clear();
            string cph, czxm, czdh;
            string strsql1 = "select carnumber,carname,carphone from member where carphone='" + hycarphone.Text + "'";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql1, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cph = read[0].ToString();
                czxm = read[1].ToString();
                czdh = read[2].ToString();
                string[] table = { cph, czxm, czdh };
                dataGridView3.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //通过姓名查找会员信息
            //车主姓名不能为空
            if (this.hycarname.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主姓名", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.hycarname.Focus();//光标归位到车牌号
                return;
            }
            dataGridView3.Rows.Clear();
            string cph, czxm, czdh;
            string strsql1 = "select carnumber,carname,carphone from member where carname='" + hycarname.Text + "'";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql1, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cph = read[0].ToString();
                czxm = read[1].ToString();
                czdh = read[2].ToString();
                string[] table = { cph, czxm, czdh };
                dataGridView3.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //指定车辆姓名不能为空
            if (this.ckcarname.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主姓名", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ckcarname.Focus();//光标归位到车牌号
                return;
            }
            else//若有该车辆，则展示车辆信息
            {
                dataGridView1.Rows.Clear();
                string cph, czxm, czdh, rksj, cllx;
                string strsql2 = "select carnumber,carname,carphone,intime,cartype from garage where carname='" + ckcarname.Text + "'";
                string connStr = Sqllink;//设置数据库连接方式
                SqlConnection conn = new SqlConnection(connStr);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql2, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
                {
                    cph = read[0].ToString();
                    czxm = read[1].ToString();
                    czdh = read[2].ToString();
                    rksj = read[3].ToString();
                    cllx = read[4].ToString();
                    string[] table = { cph, czxm, czdh, rksj, cllx };
                    dataGridView1.Rows.Add(table);
                }
                conn.Close();//关闭
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //指定车辆电话不能为空
            if (this.ckcarphone.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主电话", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ckcarphone.Focus();//光标归位到车牌号
                return;
            }
            else//若有该车辆，则展示车辆信息
            {
                dataGridView1.Rows.Clear();
                string cph, czxm, czdh, rksj, cllx;
                string strsql2 = "select carnumber,carname,carphone,intime,cartype from garage where carphone='" + ckcarphone.Text + "'";
                string connStr = Sqllink;//设置数据库连接方式
                SqlConnection conn = new SqlConnection(connStr);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql2, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
                {
                    cph = read[0].ToString();
                    czxm = read[1].ToString();
                    czdh = read[2].ToString();
                    rksj = read[3].ToString();
                    cllx = read[4].ToString();
                    string[] table = { cph, czxm, czdh, rksj, cllx };
                    dataGridView1.Rows.Add(table);
                }
                conn.Close();//关闭
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //车主姓名不能为空
            if (this.crcarname.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车主姓名", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.crcarname.Focus();//光标归位到车主电话
                return;
            }
            //搜索并展示记录
            dataGridView2.Rows.Clear();
            string cph, czxm, czdh, rksj, cksj, tcfy, cllx;
            string strsql1 = "select carnumber,carname,carphone,intime,outtime,cost,cartype from history where carname='" + crcarname.Text + "'";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql1, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cph = read[0].ToString();
                czxm = read[1].ToString();
                czdh = read[2].ToString();
                rksj = read[3].ToString();
                cksj = read[4].ToString();
                tcfy = read[5].ToString();
                cllx = read[6].ToString();
                string[] table = { cph, czxm, czdh, rksj, cksj, tcfy, cllx };
                dataGridView2.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void button18_Click(object sender, EventArgs e)
        {
            //指定车辆入库时间不能为空
            if (this.ckintime.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入入库时间", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ckintime.Focus();//光标归位到车牌号
                return;
            }
            else//若有该车辆，则展示车辆信息
            {
                dataGridView1.Rows.Clear();
                string cph, czxm, czdh, rksj, cllx;
                string strsql2 = "select carnumber,carname,carphone,intime,cartype from garage where intime like '" + ckintime.Text + "%'";
                string connStr = Sqllink;//设置数据库连接方式
                SqlConnection conn = new SqlConnection(connStr);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql2, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
                {
                    cph = read[0].ToString();
                    czxm = read[1].ToString();
                    czdh = read[2].ToString();
                    rksj = read[3].ToString();
                    cllx = read[4].ToString();
                    string[] table = { cph, czxm, czdh, rksj, cllx };
                    dataGridView1.Rows.Add(table);
                }
                conn.Close();//关闭
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            //指定车辆入库时间不能为空
            if (this.crintime.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入入库时间", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.crintime.Focus();//光标归位到入库时间
                return;
            }
            else//若有该车辆，则展示车辆信息
            {
                dataGridView2.Rows.Clear();
                string cph, czxm, czdh, rksj, cksj, tcfy, cllx;
                string strsql1 = "select carnumber,carname,carphone,intime,outtime,cost,cartype from history where intime like '" + crintime.Text + "%'";
                string connStr = Sqllink;//设置数据库连接方式
                SqlConnection conn = new SqlConnection(connStr);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql1, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
                {
                    cph = read[0].ToString();
                    czxm = read[1].ToString();
                    czdh = read[2].ToString();
                    rksj = read[3].ToString();
                    cksj = read[4].ToString();
                    tcfy = read[5].ToString();
                    cllx = read[6].ToString();
                    string[] table = { cph, czxm, czdh, rksj, cksj, tcfy, cllx };
                    dataGridView2.Rows.Add(table);
                }
                conn.Close();//关闭
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            //指定车辆出库时间不能为空
            if (this.crouttime.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入出库时间", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.crouttime.Focus();//光标归位到出库时间
                return;
            }
            else//若有该车辆，则展示车辆信息
            {
                dataGridView2.Rows.Clear();
                string cph, czxm, czdh, rksj, cksj, tcfy, cllx;
                string strsql1 = "select carnumber,carname,carphone,intime,outtime,cost,cartype from history where outtime like '" + crouttime.Text + "%'";
                string connStr = Sqllink;//设置数据库连接方式
                SqlConnection conn = new SqlConnection(connStr);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql1, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
                {
                    cph = read[0].ToString();
                    czxm = read[1].ToString();
                    czdh = read[2].ToString();
                    rksj = read[3].ToString();
                    cksj = read[4].ToString();
                    tcfy = read[5].ToString();
                    cllx = read[6].ToString();
                    string[] table = { cph, czxm, czdh, rksj, cksj, tcfy, cllx };
                    dataGridView2.Rows.Add(table);
                }
                conn.Close();//关闭
            }
        }

        private void chuintime_Click(object sender, EventArgs e)
        {

        }

        private void chukuchaxun_Click(object sender, EventArgs e)
        {
            //车牌号不能为空
            if (this.chucarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.chucarnumber.Focus();//光标归位到车牌号
                return;
            }
            //设置查询语句，按车牌号搜索车库表中车辆的车辆类型、入库时间
            string strsql1 = "select count(*) from garage where carnumber='" + chucarnumber.Text + "'";
            string strsql2 = "select cartype,intime from garage where carnumber='"+ chucarnumber.Text + "'";
            int count = sql.Conn(strsql1);
            if(count <= 0)//车库表中无该车辆
            {
                MessageBox.Show("车库中未找到该车辆", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.chucarnumber.Focus();//光标归位到车牌号
                return;
            }
            else//车库表中有该车辆，则将车辆类型、入库时间显示到入库时间lab标签中
            {
                SqlConnection conn = new SqlConnection(Sqllink);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql2, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给chucartype、chuintime
                {
                    chucartype.Text = read[0].ToString();
                    chuintime.Text = read[1].ToString();
                }
                conn.Close();//关闭
            }
        }

        /*private void button21_Click(object sender, EventArgs e)
        {
            
        }*/

        private void chucost_Click(object sender, EventArgs e)
        {
            //车牌号不能为空
            if (this.chucarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.chucarnumber.Focus();//光标归位到车牌号
                return;
            }
            //车辆类型不能为空
            if (this.chucartype.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车辆类型", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.chucartype.Focus();//光标归位到车辆类型
                return;
            }
            //入库时间不能为空
            if (this.chuintime.Text.Trim() == "点击查询获取入库时间")
            {
                MessageBox.Show("请更新入库时间", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //出库时间不能为空
            if (this.chuouttime.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入出库时间", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.chuouttime.Focus();//光标归位到出库时间
                return;
            }
            //正确车牌号
            if (chucarnumber.Text.Length != 7)
            {
                MessageBox.Show("请输入7位正确车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //正确入库时间
            if (chuouttime.Text.Length != 12)
            {
                MessageBox.Show("请输入12位日期格式", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //判断出库时间是否大于入库时间
            if (Convert.ToDouble(this.chuouttime.Text) < Convert.ToDouble(this.chuintime.Text))
            {
                MessageBox.Show("请确定出库时间大于入库时间", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.chuouttime.Focus();//光标归位到出库时间
                return;
            }
            //检查车辆是否在库内
            string strsql3 = "select count(*) from garage where carnumber='" + chucarnumber.Text + "'";
            int count1 = sql.Conn(strsql3);
            if(count1 <= 0)
            {
                MessageBox.Show("车库内未找到该车辆", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //确保收费标准已设置
            string strsql4 = "select count(*) from cost where cartype='" + chucartype.Text + "'";
            int count2 = sql.Conn(strsql4);
            if(count2 <= 0)
            {
                MessageBox.Show("请先设置"+ chucartype.Text + "类车收费标准", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //计算停车时间
            long intime = long.Parse(chuintime.Text);//将入库时间转为long类型
            long outtime = long.Parse(chuouttime.Text);//将出库时间转为long类型
            //计算天数间隔
            long inyear = intime / 100000000;
            long inmonth = intime % 100000000 / 1000000;
            long inday = intime % 1000000 / 10000;
            string ruday = inyear.ToString() + "-" + inmonth.ToString() + "-" + inday.ToString();
            long outyear = outtime / 100000000;
            long outmonth = outtime % 100000000 / 1000000;
            long outday = outtime % 1000000 / 10000;
            string chuday = outyear.ToString() + "-" + outmonth.ToString() + "-" + outday.ToString();
            DateTime dt1 = Convert.ToDateTime(ruday);
            DateTime dt2 = Convert.ToDateTime(chuday);
            TimeSpan span = dt2.Subtract(dt1);
            int dayDiff = span.Days;
            /*long day = outtime % 1000000 / 10000 - intime % 1000000 / 10000;*/
            long hour = outtime % 10000 / 100 - intime % 10000 / 100;
            long min = outtime % 100 - intime % 100;
            double biaozhun = 0;//收费标准
            double member = 1.0;//定义会员折扣
            double feiyong = 0;//定义费用
            double free_time = 0;
            //根据车辆类型获取收费标准、免费时间
            string strsql1 = "select free,charge,discount from cost where cartype='" + chucartype.Text + "'";
            SqlConnection conn = new SqlConnection(Sqllink);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql1, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给免费时长、收费标准、会员折扣
            {
                free_time = double.Parse(read[0].ToString());
                biaozhun = double.Parse(read[1].ToString());
                member = double.Parse(read[2].ToString());
            }
            conn.Close();//关闭
            //计算标准费用
            if(dayDiff == 0 && ((hour * 60 + min) < free_time))
            {
                feiyong = 0;
            }
            else
            {
                //测试天数
                //MessageBox.Show(dayDiff.ToString(), "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //费用计算
                feiyong = (dayDiff * 1440 + hour * 60 + min - free_time) / 60 * biaozhun;
            }
            //计算会员费用
            string strsql2 = "select count(*) from member where carnumber='" + chucarnumber.Text + "'";
            int count = sql.Conn(strsql2);
            if(count > 0)//是会员进行折扣
            {
                feiyong = feiyong * member;
            }
            //输出费用
            chucost.Text = feiyong.ToString("0.00");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            chucarnumber.Text = "";
            chucartype.Text = "";
            chuintime.Text = "点击查询获取入库时间";
            chuouttime.Text = "";
            chucost.Text = "点击以获取费用";
        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nowtime.Text = DateTime.Now.ToString();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            ruintime.Text = time.Gettime();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            chuouttime.Text = time.Gettime();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            //验证车牌不能为空
            if (this.zccarnumber.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入车牌号", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.zccarnumber.Focus();//光标归位到车牌号
                return;
            }
            string strsql1 = "select count(*) from car where carnumber='" + zccarnumber.Text + "'";
            string strsql3 = "select carname,carphone from car where carnumber='" + zccarnumber.Text + "'";
            //从数据库中按车牌号查询是否有记录
            int count1 = sql.Conn(strsql1);
            //若车辆表无记录则提示新车辆然后继续输入车主姓名、车主电话
            if (count1 <= 0)
            {
                MessageBox.Show("新车辆，请继续输入", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //若车辆表有记录则则自动填充姓名、电话
            else
            {
                string connStr = Sqllink;//设置数据库连接方式
                SqlConnection conn = new SqlConnection(connStr);//连接数据库
                conn.Open();//打开数据库
                SqlCommand comm = new SqlCommand(strsql3, conn);//查询
                SqlDataReader read = comm.ExecuteReader();
                while (read.Read())//将查询结果赋给姓名、电话text
                {
                    zccarname.Text = read[0].ToString();
                    zccarphone.Text = read[1].ToString();
                }
                conn.Close();//关闭
                return;
            }
        }

        private void label38_Click_1(object sender, EventArgs e)
        {
        }

        private void button21_Click_1(object sender, EventArgs e)
        {
            //车辆类型不能为空
            if (this.sfcartype.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请选择车辆类型", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.sfcartype.Focus();//光标归位到车牌号
                return;
            }
            //免费时段不能为空
            if (this.freetime.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入免费时段", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.freetime.Focus();//光标归位到车牌号
                return;
            }
            //收费标准不能为空
            if (this.charge.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入收费标准", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.charge.Focus();//光标归位到车牌号
                return;
            }
            //会员折扣不能为空
            if (this.discount.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入会员折扣", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.discount.Focus();//光标归位到车牌号
                return;
            }
            //限制收费标准
            if (Convert.ToInt32(this.charge.Text) < 0 || Convert.ToInt32(this.charge.Text) > 10)
            {
                MessageBox.Show("请输入合理的收费标准", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.charge.Focus();//光标归位到收费标准
                return;
            }
            //限制收费标准
            if (Convert.ToDouble(this.discount.Text) <= 0 || Convert.ToDouble(this.discount.Text) >= 1)
            {
                MessageBox.Show("请输入合理的会员折扣", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.discount.Focus();//光标归位到车牌号
                return;
            }
            //查看收费表中有无该车辆类型的数据
            string strsql1 = "select count(*) from cost where cartype='" + sfcartype.Text + "'";
            string strsql2 = "update cost set free=" + Convert.ToInt32(freetime.Text) + ",charge=" + Convert.ToInt32(charge.Text) + ",discount='" + discount.Text + "' where cartype='" + sfcartype.Text + "'";
            string strsql3 = "insert into cost(cartype,free,charge,discount) values('" + sfcartype.Text + "'," + Convert.ToInt32(freetime.Text) + "," + Convert.ToInt32(charge.Text) + ",'" + discount.Text + "')";
            int count = sql.Conn(strsql1);
            if (count > 0)
            {
                //若已存在则进行修改
                if (Convert.ToInt32(sql.Comm(strsql2)) > 0)
                {
                    MessageBox.Show("更改成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                //若不存在则进行添加
                if (Convert.ToInt32(sql.Comm(strsql3)) > 0)
                {
                    MessageBox.Show("更改成功", "输入提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            //查看所有收费标准
            dataGridView4.Rows.Clear();
            string cllx, mfsc, sfbz, hyzk;
            string strsql = "select cartype,free,charge,discount from cost";
            string connStr = Sqllink;//设置数据库连接方式
            SqlConnection conn = new SqlConnection(connStr);//连接数据库
            conn.Open();//打开数据库
            SqlCommand comm = new SqlCommand(strsql, conn);//查询
            SqlDataReader read = comm.ExecuteReader();
            while (read.Read())//将查询结果赋给车牌号、姓名、电话、入库时间
            {
                cllx = read[0].ToString();
                mfsc = read[1].ToString();
                sfbz = read[2].ToString();
                hyzk = read[3].ToString();
                string[] table = { cllx, mfsc, sfbz, hyzk };
                dataGridView4.Rows.Add(table);
            }
            conn.Close();//关闭
        }

        private void chucarnumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage11_Click(object sender, EventArgs e)
        {

        }
    }
}
