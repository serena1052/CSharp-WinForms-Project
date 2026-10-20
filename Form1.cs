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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(new string[] { "低", "中", "高", "緊急" });
            comboBox1.SelectedIndex = 1; 
        }
        public List<string> GetAllTasks()
        {
            return listBox1.Items.Cast<string>().ToList();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    listBox1.Items.Add($"{fileName} - 已接收");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            richTextBox1.Clear();
            comboBox1.SelectedIndex = 1;
            dateTimePicker1.Value = DateTime.Now;
            radioButton5.Checked = true;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            listBox1.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("請輸入任務標題！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string category = radioButton5.Checked ? "工作任務" :
                              radioButton6.Checked ? "個人任務" :
                              radioButton7.Checked ? "學習任務" : "其他";

            string priority = comboBox1.SelectedItem?.ToString() ?? "中";

            string important = checkBox1.Checked ? "是" : "否";
            string repeatable = checkBox2.Checked ? "是" : "否";
            string reminder = checkBox3.Checked ? "是" : "否";

            string dueDate = dateTimePicker1.Value.ToShortDateString();

            string taskSummary = $"[{category}] {textBox1.Text} (優先級:{priority}, 到期:{dueDate}, 重要:{important}, 重複:{repeatable}, 提醒:{reminder})\n描述：{richTextBox1.Text}";
            listBox1.Items.Add(taskSummary);

            label1.Text = "任務已加入";
            textBox1.Clear();
            richTextBox1.Clear();
            comboBox1.SelectedIndex = 1;
            dateTimePicker1.Value = DateTime.Now;
            radioButton5.Checked = true;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("請先拖放或選擇檔案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string item = listBox1.Items[i].ToString();
                if (item.Contains("已接收"))
                {
                    listBox1.Items[i] = item.Replace("已接收", "已處理");
                }
            }
            label1.Text = $"已成功處理 {listBox1.Items.Count} 個檔案";
            MessageBox.Show("文件處理完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 離開ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 說明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SmartFileTaskManager 範例程式\n\n功能包含拖放文件、任務管理、檔案處理等。", "說明", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 匯入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (var file in ofd.FileNames)
                {
                    string fileName = Path.GetFileName(file);
                    listBox1.Items.Add($"{fileName} - 已接收");
                }
            }
        }
        private void 匯出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("沒有任務可匯出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文字檔案 (*.txt)|*.txt";
            sfd.FileName = "TaskReport.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (var sw = new StreamWriter(sfd.FileName))
                {
                    foreach (var item in listBox1.Items)
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
                MessageBox.Show("任務已匯出！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("這是一個示範訊息框！", "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("沒有任務可產生報告！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string report = "任務報告:\n\n";

            foreach (var item in listBox1.Items)
            {
                report += item.ToString() + "\n";
            }

            MessageBox.Show(report, "任務報告", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }
    }
}
