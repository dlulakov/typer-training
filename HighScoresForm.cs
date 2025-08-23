using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Typer_Training
{
    public partial class HighScoresForm : Form
    {

        public HighScores highScores { get; set; }
        public string Filename { get; }

        public HighScoresForm(string filename)
        {
            InitializeComponent();
            Filename = filename;
            deserializeScores(Filename);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lbTimer_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ShowAllScores(); 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
        }

        private void serializeScore(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, highScores);
            }
        }

        private void deserializeScores(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {

                    IFormatter formatter = new BinaryFormatter();
                    if (fs.Length > 0)
                    {
                        highScores = (HighScores)formatter.Deserialize(fs);
                    }
                    else
                    {
                        fs.Close();
                        highScores = new HighScores();
                        serializeScore(path);
                    }
                }
            }
            else
            {
                highScores = new HighScores();
                serializeScore(path);
            }
        }

        private void FilterScores(Type scoreType)
        {
            if (highScores != null && highScores.scores.Count > 0)
            {
                var filtered = highScores.scores
                    .Where(s => s.ScoreType == scoreType)
                    .OrderByDescending(s => s.WPM)
                    .Select((s, index) => new
                    {
                        Rank = index + 1,
                        Name = s.Name,
                        WPM = s.WPM
                    })
                    .ToList();

                dataGridView1.DataSource = filtered;
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        private void zenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterScores(Type.Zen);
            SetCheckedMenuItem(zenToolStripMenuItem);
        }

        private void wordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterScores(Type.Words);
            SetCheckedMenuItem(wordsToolStripMenuItem);
        }

        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterScores(Type.Time);
            SetCheckedMenuItem(timeToolStripMenuItem);
        }

        private void quoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FilterScores(Type.Quote);
            SetCheckedMenuItem(quoteToolStripMenuItem);
        }

        private void alllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAllScores();
            SetCheckedMenuItem(alllToolStripMenuItem);
        }

        private void ShowAllScores()
        {
            if (highScores != null && highScores.scores.Count > 0)
            {
                var allScores = highScores.scores
                    .OrderByDescending(s => s.WPM)
                    .Select((s, index) => new
                    {
                        Rank = index + 1,
                        Name = s.Name,
                        WPM = s.WPM,
                        Type = s.ScoreType.ToString() 
                    })
                    .ToList();

                dataGridView1.DataSource = allScores;
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }
        private void SetCheckedMenuItem(ToolStripMenuItem selectedItem)
        {
            foreach (ToolStripMenuItem item in menuStrip1.Items)
            {
                if (item.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem subItem in item.DropDownItems)
                    {
                        subItem.Checked = (subItem == selectedItem);
                    }
                }
            }
        }

    }
}
