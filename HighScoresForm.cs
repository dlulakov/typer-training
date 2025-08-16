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
            if (highScores != null && highScores.scores.Count > 0)
            {
                // use your entity's getSorted method
                var sorted = highScores.getSorted()
                    .Select((s, index) => new
                    {
                        Rank = index + 1,
                        Name = s.Name, 
                        WPM = s.WPM
                    })
                    .ToList();

                dataGridView1.DataSource = sorted;
            }
            else
            {
                dataGridView1.DataSource = null;
            }

            // optional formatting
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
                // Create empty high scores and save to file
                highScores = new HighScores();
                serializeScore(path);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
