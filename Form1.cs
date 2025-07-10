using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Typer_Training
{
    public partial class Form1 : Form
    {
        public static String TextToGuess { get; set; }
        public Text text { get; set; }
        public int currentIndex { get; set; }
        public int timer { get; set; }
        public int WPM { get; set; }
        public bool timerStarted { get; set; }
        public Form1()
        {
            InitializeComponent();
            timerStarted = false;
            currentIndex = 0;
            timer1.Interval = 1000;
            timer = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplyStyling();
            currentIndex = 0;
            timer = 0;
            timerStarted = false;
            timer1.Stop();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTime.Checked)
            {
                cbNumbers.Visible = true;
                cBpunctuation.Visible = true;
                cbQuote.Checked = false;
                cbWords.Checked = false;
                cbZen.Checked = false;
            }
            else
            {
                cbNumbers.Visible = false;
                cBpunctuation.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cBpunctuation_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void cbLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbWords_CheckedChanged(object sender, EventArgs e)
        {
            if (cbWords.Checked)
            {
                cbNumbers.Visible = true;
                cBpunctuation.Visible = true;
                cbQuote.Checked = false;
                cbTime.Checked = false;
                cbZen.Checked = false;
            }
            else
            {
                cbNumbers.Visible = false;
                cBpunctuation.Visible = false;
            }
        }

        private void cbQuote_CheckedChanged(object sender, EventArgs e)
        {
            if (cbQuote.Checked)
            {
                cbZen.Checked = false;
                cbTime.Checked = false;
                cbWords.Checked = false;
            }
        }

        private void cbZen_CheckedChanged(object sender, EventArgs e)
        {
            if (cbZen.Checked)
            {
                cbQuote.Checked = false;
                cbTime.Checked = false;
                cbWords.Checked = false;
            }
        }

        private void rtbText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (currentIndex >= TextToGuess.Length)
            {
                text.timeNeeded = timer;
                WPM = text.CalculateWPM();
                lbWpm.Text = $"WPM: {WPM}";
                lbWpm.Visible = true;
                timer1.Stop();
                timer = 0;
                timerStarted = false;
                return;
            }
            if (currentIndex < 0)
            {
                return;
            }
            if (!timerStarted)
            {
                timerStarted = true;
                timer1.Start();
            }

            char expectedChar = TextToGuess[currentIndex];
            char typedChar = e.KeyChar;

            if (typedChar == '\b')
            {

                ColorLetter(--currentIndex, Color.Gray);
                text.CharactersTyped++;
                return;
            }

            if (typedChar == expectedChar)
            {
                ColorLetter(currentIndex, Color.Green);
            }
            else
            {
                ColorLetter(currentIndex, Color.Red);
                text.Mistakes++;
            }
            text.CharactersTyped++;
            currentIndex++;
        }
        private void ColorLetter(int index, Color color)
        {
            rtbText.Select(index, 1);
            rtbText.SelectionColor = color;
            rtbText.Select(rtbText.TextLength, 0);
        }

        private void ApplyStyling()
        {
            rtbText.SelectAll();
            rtbText.SelectionColor = Color.Gray;
            rtbText.DeselectAll();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadNewText();
            rtbText.ReadOnly = true;
            rtbText.Text = TextToGuess;
            ApplyStyling();
            this.KeyPreview = true;
            cBpunctuation.Visible = false;
            cbNumbers.Visible = false;
        }

        private async void btnNew_Click(object sender, EventArgs e)
        {
            await LoadNewText();
            timerStarted = false;
            timer1.Stop();
            timer = 0;
        }
        private async Task LoadNewText()
        {
            currentIndex = 0;
            TextToGuess = await FetchParagraph();
            text = new Text(TextToGuess);
            rtbText.ReadOnly = true;
            rtbText.Text = TextToGuess;
            ApplyStyling();
        }
        private async Task<string> FetchParagraph()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://metaphorpsum.com/paragraphs/1");
                return result.Trim();
            }
        }

        private async Task<string> FetchRandomQuoteAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync("http://api.quotable.io/random");
                JObject json = JObject.Parse(response);
                return json["content"].ToString();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer += 1;
            lbTimer.Text = String.Format("{0}:{1}", timer / 60, timer % 60);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
