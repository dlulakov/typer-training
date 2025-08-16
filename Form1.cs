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
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace Typer_Training
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_SETREDRAW = 0x0b;

        public static String TextToGuess { get; set; }
        public Text text { get; set; }
        public int currentIndex { get; set; }
        public int timer { get; set; }
        public int WPM { get; set; }
        private bool isUpdatingMode = false; // guard flag
        public bool timerStarted { get; set; }
        public static readonly string FileName = "higscore.txt";
        public Form1()
        {
            InitializeComponent();
            timerStarted = false;
            currentIndex = 0;
            timer1.Interval = 1000;
            timer = 0;

            cBpunctuation.Visible = false;
            cbNumbers.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplyStyling();
            currentIndex = 0;
            timer = 0;               // reset internal timer
            lbTimer.Text = "0:0";    // reset label
            timerStarted = false;
            timer1.Stop();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            timer = 0;
            lbTimer.Text = "0:0";
            timerStarted = false;

            HighScoresForm form2 = new HighScoresForm(FileName);
            form2.ShowDialog();
        }

        private void rtbText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!timerStarted)
            {
                timerStarted = true;
                timer1.Start();
            }

            char typedChar = e.KeyChar;

            if (typedChar == '\b') // backspace
            {
                if (currentIndex <= 0) return; // nothing to backspace
                currentIndex--;
                ColorLetterWithoutScroll(currentIndex, Color.Gray);
                text.CharactersTyped++;
            }
            else
            {
                char expectedChar = TextToGuess[currentIndex];

                if (typedChar == expectedChar)
                {
                    ColorLetterWithoutScroll(currentIndex, typedChar == expectedChar ? Color.Green : Color.Red);
                }
                else
                {
                    ColorLetterWithoutScroll(currentIndex, typedChar == expectedChar ? Color.Green : Color.Red);
                    text.Mistakes++;
                }

                text.CharactersTyped++;
                currentIndex++;

                // Check immediately if the text is fully typed
                if (currentIndex >= TextToGuess.Length)
                {
                    text.timeNeeded = timer;
                    WPM = text.CalculateWPM();
                    lbWpm.Text = $"WPM: {WPM}";
                    lbWpm.Visible = true;
                    timer1.Stop();
                    timer = 0;
                    timerStarted = false;

                    Type scoreType;
                    if (cbWords.Checked)
                        scoreType = Type.Words;
                    else if (cbTime.Checked)
                        scoreType = Type.Time;
                    else if (cbQuote.Checked)
                        scoreType = Type.Quote;
                    else 
                        scoreType = Type.Zen;

                    AddScore addScore = new AddScore(WPM, FileName, scoreType);
                    addScore.ShowDialog();
                    return;
                }

            }

            UpdateCaretAndScroll();
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

            // Make Numbers and Punctuation hidden initially
            cbNumbers.Visible = false;
            cBpunctuation.Visible = false;

            // Preselect Words and update visibility correctly
            isUpdatingMode = false; // Make sure the guard is off
            cbWords.Checked = true; // preselect
            SelectMode(cbWords);    // call SelectMode to update visibility
        }


        private async void btnNew_Click(object sender, EventArgs e)
        {
            await LoadNewText();

            currentIndex = 0;
            timer = 0;               // reset internal timer
            lbTimer.Text = "0:0";    // reset label
            timerStarted = false;
            timer1.Stop();
        }
        //private async Task LoadNewText()
        //{
        //    currentIndex = 0;
        //    TextToGuess = await FetchParagraph();
        //    text = new Text(TextToGuess);
        //    rtbText.ReadOnly = true;
        //    rtbText.Text = TextToGuess;
        //    ApplyStyling();
        //    FormatText(); // Apply nicer formatting
        //}
        private void FormatText()
        {
            // Set a nicer, larger font
            rtbText.Font = new Font("Segoe UI", 18, FontStyle.Regular);

            // Optional: set color for the text
            rtbText.ForeColor = Color.Black;

            // Optional: improve readability
            rtbText.BackColor = Color.WhiteSmoke;
            rtbText.SelectionAlignment = HorizontalAlignment.Left; // text alignment
        }
        private void UpdateCaretAndScroll()
        {
            rtbText.Select(currentIndex, 0);
            rtbText.SelectionStart = currentIndex;
            rtbText.SelectionLength = 0;
        }

        //private async Task<string> FetchParagraph()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string result = await client.GetStringAsync("http://metaphorpsum.com/paragraphs/1");
        //        return result.Trim();
        //    }
        //}

        //private async Task<string> FetchRandomQuoteAsync()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        string response = await client.GetStringAsync("http://api.quotable.io/random");
        //        JObject json = JObject.Parse(response);
        //        return json["content"].ToString();
        //    }
        //}
        private void SelectMode(CheckBox selected)
        {
            if (isUpdatingMode) return;

            isUpdatingMode = true;

            // Uncheck all other mode checkboxes
            foreach (var cb in new[] { cbTime, cbWords, cbQuote, cbZen })
            {
                if (cb != selected)
                    cb.Checked = false;
            }

            // Show Numbers and Punctuation only for Time or Words modes
            bool timeOrWordsSelected = cbTime.Checked || cbWords.Checked;
            cbNumbers.Visible = timeOrWordsSelected;
            cBpunctuation.Visible = timeOrWordsSelected;

            // Show Time counter controls only if Time mode is selected
            bool isTimeSelected = cbTime.Checked;
            lbTimeCounter.Visible = isTimeSelected;
            cbTimeCounter.Visible = isTimeSelected;

            isUpdatingMode = false;
            LoadNewText();
        }

        private void cbTime_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUpdatingMode)
            {
                if (cbTime.Checked)
                    SelectMode(cbTime);
                else
                    cbTime.Checked = true; // prevent unchecking
            }
        }

        private void cbWords_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUpdatingMode)
            {
                if (cbWords.Checked)
                    SelectMode(cbWords);
                else
                    cbWords.Checked = true; // prevent unchecking
            }
        }

        private void cbQuote_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUpdatingMode)
            {
                if (cbQuote.Checked)
                    SelectMode(cbQuote);
                else
                    cbQuote.Checked = true; // prevent unchecking
            }
        }

        private void cbZen_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUpdatingMode)
            {
                if (cbZen.Checked)
                    SelectMode(cbZen);
                else
                    cbZen.Checked = true; // prevent unchecking
            }
        }
        private void ColorLetterWithoutScroll(int index, Color color)
        {
            // Save current selection
            int selStart = rtbText.SelectionStart;
            int selLength = rtbText.SelectionLength;

            SuspendDrawing(rtbText);

            rtbText.Select(index, 1);
            rtbText.SelectionColor = color;

            // Restore selection
            rtbText.Select(selStart, selLength);

            ResumeDrawing(rtbText);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer += 1;
            lbTimer.Text = String.Format("{0}:{1}", timer / 60, timer % 60);
        }
        private void SuspendDrawing(Control c)
        {
            SendMessage(c.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        private void ResumeDrawing(Control c)
        {
            SendMessage(c.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            c.Refresh();
        }


        private async Task LoadNewText()
        {
            currentIndex = 0;

            if (cbWords.Checked) // Words mode
            {
                TextToGuess = await FetchParagraph();
            }
            else if (cbQuote.Checked) // Quote mode
            {
                TextToGuess = await FetchQuoteAsync();
            }
            else if (cbZen.Checked) // Zen mode
            {
                TextToGuess = await FetchZenQuoteAsync();
            }
            else // default fallback
            {
                TextToGuess = await FetchParagraph();
            }

            text = new Text(TextToGuess);
            rtbText.ReadOnly = true;
            rtbText.Text = TextToGuess;
            ApplyStyling();
            FormatText(); // Apply nicer formatting
        }

        // Existing method for paragraphs
        private async Task<string> FetchParagraph()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://metaphorpsum.com/paragraphs/1");
                return result.Trim();
            }
        }

        // Existing method for quote
        private async Task<string> FetchQuoteAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync("https://zenquotes.io/api/random");
                JArray jsonArray = JArray.Parse(response);
                string quote = jsonArray[0]["q"].ToString();
                string author = jsonArray[0]["a"].ToString();
                return $"{quote} — {author}";
            }
        }

        // New method for Zen quotes
        private async Task<string> FetchZenQuoteAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string response = await client.GetStringAsync("https://zenquotes.io/api/random");
                JArray jsonArray = JArray.Parse(response);
                string quote = jsonArray[0]["q"].ToString();
                string author = jsonArray[0]["a"].ToString();
                return $"{quote} — {author}";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lbTimeCounter_Click(object sender, EventArgs e)
        {

        }
    }
}
