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
using System.Runtime.InteropServices.ComTypes;

namespace Typer_Training
{
    public partial class AddScore : Form
    {
        public HighScores highScores { get; set; }
        public int WPM { get; set; }
        public string FileName { get; set; }
        public Type Type { get; set; }
        public AddScore(int wpm, string fileName, Type type)
        {
            InitializeComponent();
            deserializeScores(fileName);
            WPM = wpm;
            FileName = fileName;
            lbAddScoreWpm.Text = $"WPM: {wpm}";
            lbAddScoreWpm.Visible = true;
            Type = type;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            highScores.addScore(new Score(txtUserName.Text, WPM, Type));
            serializeScore(FileName);
            this.DialogResult = DialogResult.OK;
            this.Close();
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


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbWpm_Click(object sender, EventArgs e)
        {

        }
    }
}
