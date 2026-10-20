using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private Timer timer;
        private int totalTime; 
        private int remainingTime;
        private enum PomodoroPhase { Focus, ShortBreak, LongBreak }
        private PomodoroPhase currentPhase = PomodoroPhase.Focus;
        private int pomodoroCount = 0;
        public Form2()
        {
            InitializeComponent();
            InitTimer();

            comboBox1.Items.AddRange(new string[] { "Beep", "Asterisk", "Exclamation" });
            comboBox1.SelectedIndex = 0;

            radioButton1.Checked = true;
            label1.Text = "請選擇並開始計時";
            label3.Text = "剩餘時間：00:00";
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            numericUpDown1.Value = 25;
            numericUpDown2.Value = 5;
            numericUpDown3.Value = 15;
        }
        public List<string> GetPomodoroRecords()
        {
            return listBox1.Items.Cast<string>().ToList();
        }
        private void InitTimer()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingTime > 0)
            {
                remainingTime--;
                UpdateUI();
            }
            else
            {
                timer.Stop();
                PlaySound();

                switch (currentPhase)
                {
                    case PomodoroPhase.Focus:
                        pomodoroCount++;
                        listBox1.Items.Add($"✔️ 番茄完成 - {DateTime.Now:HH:mm}");
                        currentPhase = (pomodoroCount % 4 == 0) ? PomodoroPhase.LongBreak : PomodoroPhase.ShortBreak;
                        break;
                    case PomodoroPhase.ShortBreak:
                    case PomodoroPhase.LongBreak:
                        currentPhase = PomodoroPhase.Focus;
                        break;
                }

                if (checkBox1.Checked)
                    StartPhase();
                else
                    label1.Text = "已完成，請手動開始下一階段";
            }
        }
        private void StartPhase()
        {
            switch (currentPhase)
            {
                case PomodoroPhase.Focus:
                    totalTime = (int)numericUpDown1.Value * 60;
                    label1.Text = "專注時間";
                    break;
                case PomodoroPhase.ShortBreak:
                    totalTime = (int)numericUpDown2.Value * 60;
                    label1.Text = "短休息";
                    break;
                case PomodoroPhase.LongBreak:
                    totalTime = (int)numericUpDown3.Value * 60;
                    label1.Text = "長休息";
                    break;
            }

            remainingTime = totalTime;
            progressBar1.Maximum = totalTime;
            progressBar1.Value = 0;
            timer.Start();
            UpdateUI();
        }

        private void UpdateUI()
        {
            int minutes = remainingTime / 60;
            int seconds = remainingTime % 60;
            this.Text = $"{label1.Text} - {minutes:D2}:{seconds:D2}";
            label3.Text = $"剩餘時間：{minutes:D2}:{seconds:D2}";
            int value = totalTime - remainingTime;
            progressBar1.Value = Math.Min(value, progressBar1.Maximum);
        }
        private void PlaySound()
        {
            if (!checkBox2.Checked) return;

            switch (comboBox1.SelectedItem?.ToString())
            {
                case "Beep":
                    SystemSounds.Beep.Play();
                    break;
                case "Asterisk":
                    SystemSounds.Asterisk.Play();
                    break;
                case "Exclamation":
                    SystemSounds.Exclamation.Play();
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                currentPhase = PomodoroPhase.Focus;
            else if (radioButton2.Checked)
                currentPhase = PomodoroPhase.ShortBreak;
            else if (radioButton3.Checked)
                currentPhase = PomodoroPhase.LongBreak;

            StartPhase();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer.Stop();
            remainingTime = 0;
            progressBar1.Value = 0;
            label1.Text = "已重設";
            label3.Text = "剩餘時間：00:00";
            this.Text = "Pomodoro Timer";
            numericUpDown1.Value = 25;
            numericUpDown2.Value = 5;  
            numericUpDown3.Value = 15;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add($"📝 自訂紀錄 - {DateTime.Now:HH:mm}");
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                listBox1.Items.Add($"🗒 備註 - {textBox1.Text} ({DateTime.Now:HH:mm})");
                textBox1.Clear();
            }
        }
    }
}
