using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        private Dictionary<string, int> taskCategoryCounts = new Dictionary<string, int>();
        private int pomodoroCount = 0;
        private int customNotesCount = 0;

        private Label titleLabel;
        private Panel leftPanel;
        private Panel rightPanel;
        private Label summaryLabel;

        public Form3()
        {
            InitializeComponent();
            LoadDataAndUpdateUI();
        }
        private void LoadDataAndUpdateUI()
        {
            Form1 form1 = Application.OpenForms["Form1"] as Form1;
            Form2 form2 = Application.OpenForms["Form2"] as Form2;

            taskCategoryCounts.Clear();
            pomodoroCount = 0;
            customNotesCount = 0;

            if (form1 != null)
            {
                var tasks = form1.GetAllTasks();
                foreach (var t in tasks)
                {
                    if (t.Contains("[工作任務]")) AddCount("工作");
                    else if (t.Contains("[個人任務]")) AddCount("個人");
                    else if (t.Contains("[學習任務]")) AddCount("學習");
                    else AddCount("其他");
                }
            }

            if (form2 != null)
            {
                var records = form2.GetPomodoroRecords();
                foreach (var r in records)
                {
                    if (r.Contains("✔️ 番茄完成")) pomodoroCount++;
                    else if (r.Contains("🗒 備註")) customNotesCount++;
                }
            }

            UpdateUIControls();
        }
        private void AddCount(string category)
        {
            if (taskCategoryCounts.ContainsKey(category)) taskCategoryCounts[category]++;
            else taskCategoryCounts[category] = 1;
        }
        private void UpdateUIControls()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            foreach (var kvp in taskCategoryCounts)
            {
                listBox1.Items.Add($"{kvp.Key} 任務：{kvp.Value}");
            }
            listBox2.Items.Add("✔ 完成首個任務：" + (taskCategoryCounts.Values.Sum() >= 1 ? "已解鎖" : "未解鎖"));
            listBox2.Items.Add("✔ 完成 10 個任務：" + (taskCategoryCounts.Values.Sum() >= 10 ? "已解鎖" : "未解鎖"));
            listBox2.Items.Add("✔ 完成 5 次番茄鐘：" + (pomodoroCount >= 5 ? "已解鎖" : "未解鎖"));
            listBox2.Items.Add("✔ 添加 3 次備註：" + (customNotesCount >= 3 ? "已解鎖" : "未解鎖"));
            label2.Text = $"總任務數：{taskCategoryCounts.Values.Sum()} ｜ 番茄鐘完成：{pomodoroCount} ｜ 備註數：{customNotesCount}";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            LoadDataAndUpdateUI();
            MessageBox.Show("資料已重新載入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            MessageBox.Show("成就列表已清空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = Application.OpenForms["Form1"] as Form1;
            if (form1 == null)
            {
                MessageBox.Show("Form1 未開啟，無法取得任務。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var tasks = form1.GetAllTasks();
            if (tasks.Count == 0)
            {
                MessageBox.Show("沒有任務資料。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string taskDetails = string.Join(Environment.NewLine, tasks);
            MessageBox.Show(taskDetails, "任務詳細清單", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = Application.OpenForms["Form2"] as Form2;
            if (form2 == null)
            {
                MessageBox.Show("Form2 未開啟，無法取得番茄鐘記錄。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var records = form2.GetPomodoroRecords();
            if (records.Count == 0)
            {
                MessageBox.Show("沒有番茄鐘記錄。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string recordDetails = string.Join(Environment.NewLine, records);
            MessageBox.Show(recordDetails, "番茄鐘歷史記錄", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
