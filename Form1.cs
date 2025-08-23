using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private bool isUpdatingMode = false;
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
            cbWordsNumber.SelectedIndexChanged += cbWordsNumber_SelectedIndexChangedAsync;
            cBpunctuation.CheckedChanged += cBpunctuation_CheckedChanged;
            cbNumbers.CheckedChanged += cbNumbers_CheckedChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ApplyStyling();
            currentIndex = 0;
            timer = 0;               
            lbTimer.Text = "0:0";    
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

        private async void rtbText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!timerStarted)
            {
                timerStarted = true;

                if (cbTime.Checked)
                {
                    timer = Convert.ToInt32(cbTimeCounter.Items[cbTimeCounter.SelectedIndex]);
                    lbTimer.Text = String.Format("{0}:{1:D2}", timer / 60, timer % 60);
                }
                else
                {
                    timer = 0;
                    lbTimer.Text = "0:00";
                }

                timer1.Start();
            }


            char typedChar = e.KeyChar;

            if (typedChar == '\b') 
            {
                if (currentIndex <= 0) return;
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
            if (cbTime.Checked && currentIndex >= TextToGuess.Length && timer > 0)
            {
                await LoadNewText();
                currentIndex = 0;
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
            if (cbWordsNumber.Items.Count > 0)
            {
                cbWordsNumber.SelectedIndex = 0;
            }

            if (cbTimeCounter.Items.Count > 0)
            {
                cbTimeCounter.SelectedIndex = 0; 
            }


            await LoadNewText();

            rtbText.ReadOnly = true;
            rtbText.Text = TextToGuess;
            ApplyStyling();
            this.KeyPreview = true;

            cbNumbers.Visible = false;
            cBpunctuation.Visible = false;

            isUpdatingMode = false;
            cbWords.Checked = true;
            SelectMode(cbWords);
        }

        private async void btnNew_Click(object sender, EventArgs e)
        {
            await LoadNewText();

            currentIndex = 0;
            timer = 0;               
            lbTimer.Text = "0:0";   
            timerStarted = false;
            timer1.Stop();
        }
        private void FormatText()
        {
            
            rtbText.Font = new Font("Segoe UI", 18, FontStyle.Regular);

           
            rtbText.ForeColor = Color.Black;

          
            rtbText.BackColor = Color.WhiteSmoke;
            rtbText.SelectionAlignment = HorizontalAlignment.Left; 
        }
        private void UpdateCaretAndScroll()
        {
            rtbText.Select(currentIndex, 0);
            rtbText.SelectionStart = currentIndex;
            rtbText.SelectionLength = 0;
        }
        private void SelectMode(CheckBox selected)
        {
            if (isUpdatingMode) return;
            isUpdatingMode = true;

            foreach (var cb in new[] { cbTime, cbWords, cbQuote, cbZen })
            {
                if (cb != selected)
                    cb.Checked = false;
            }

            bool timeOrWordsSelected = cbTime.Checked || cbWords.Checked;
            cbNumbers.Visible = timeOrWordsSelected;
            cBpunctuation.Visible = timeOrWordsSelected;

            bool wordsSelected = cbWords.Checked;
            cbWordsNumber.Visible = wordsSelected;
            lbWordsNumber.Visible = wordsSelected;

            lbTimeCounter.Visible = cbTime.Checked;
            cbTimeCounter.Visible = cbTime.Checked;

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
                    cbTime.Checked = true;
            }
        }

        private void cbWords_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUpdatingMode)
            {
                if (cbWords.Checked)
                    SelectMode(cbWords);
                else
                    cbWords.Checked = true;
            }
        }

        private void cbQuote_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUpdatingMode)
            {
                if (cbQuote.Checked)
                    SelectMode(cbQuote);
                else
                    cbQuote.Checked = true;
            }
        }

        private void cbZen_CheckedChanged(object sender, EventArgs e)
        {
            if (!isUpdatingMode)
            {
                if (cbZen.Checked)
                    SelectMode(cbZen);
                else
                    cbZen.Checked = true; 
            }
        }
        private void ColorLetterWithoutScroll(int index, Color color)
        {
            int selStart = rtbText.SelectionStart;
            int selLength = rtbText.SelectionLength;

            SuspendDrawing(rtbText);

            rtbText.Select(index, 1);
            rtbText.SelectionColor = color;

            rtbText.Select(selStart, selLength);

            ResumeDrawing(rtbText);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cbTime.Checked)
            {
                timer -= 1;
                lbTimer.Text = String.Format("{0}:{1:D2}", timer / 60, timer % 60);

                if (timer <= 0)
                {
                    timer1.Stop();
                    timerStarted = false;

                    int selectedTime = Convert.ToInt32(cbTimeCounter.Items[cbTimeCounter.SelectedIndex]);
                    text.timeNeeded = selectedTime;
                    WPM = text.CalculateWPM();
                    lbWpm.Text = $"WPM: {WPM}";
                    lbWpm.Visible = true;

                    AddScore addScore = new AddScore(WPM, FileName, Type.Time);
                    addScore.ShowDialog();
                }
            }
            else
            {
                timer += 1;
                lbTimer.Text = String.Format("{0}:{1:D2}", timer / 60, timer % 60);
            }
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

            if (cbWords.Checked)
            {
                TextToGuess = await ExtractWords();
            }
            else if (cbTime.Checked)
            {
                TextToGuess = await ExtractWords(); 
            }
            else if (cbQuote.Checked) 
            {
                TextToGuess = await FetchQuoteAsync();
            }
            else if (cbZen.Checked) 
            {
                TextToGuess = await FetchZenQuoteAsync();
            }
            else
            {
                TextToGuess = await FetchParagraph();
            }

            if (cbWords.Checked || cbTime.Checked)
            {
                if (!cBpunctuation.Checked)
                    TextToGuess = new string(TextToGuess.Where(c => !char.IsPunctuation(c)).ToArray());
            }

            if (cbWords.Checked || cbTime.Checked)
                TextToGuess = FilterText(TextToGuess);

            text = new Text(TextToGuess);
            rtbText.ReadOnly = true;
            rtbText.Text = TextToGuess;
            ApplyStyling();
            FormatText();
        }

        private async Task<string> FetchParagraph()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://metaphorpsum.com/paragraphs/1");
                return result.Trim();
            }
        }
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

        private async Task<string> ExtractWords()
        {
            int numWords = Convert.ToInt32(cbWordsNumber.Items[cbWordsNumber.SelectedIndex]);
            List<string> collectedWords = new List<string>();
            Random rnd = new Random();

            while (collectedWords.Count < numWords)
            {
                var result = await FetchParagraph();

                if (!cBpunctuation.Checked)
                    result = new string(result.Where(c => !char.IsPunctuation(c)).ToArray());

                var words = result.Split(
                    new[] { ' ', '\t', '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries
                ).ToList();

                if (cbNumbers.Checked)
                {
                    for (int i = 0; i < words.Count; i++)
                    {
                        if (rnd.NextDouble() < 0.3)
                        {
                            int randomNumber = rnd.Next(0, 1000);
                            words[i] = words[i] + " " + randomNumber.ToString();
                        }
                    }
                }

                collectedWords.AddRange(words);
            }

            var selectedWords = collectedWords.Take(numWords);

            return string.Join(" ", selectedWords);
        }
        private string FilterText(string input)
        {
            string output = input;

            if (!cBpunctuation.Checked)
            {
                output = new string(output.Where(c => !char.IsPunctuation(c)).ToArray());
            }

            if (!cbNumbers.Checked)
            {
                output = new string(output.Where(c => !char.IsDigit(c)).ToArray());
            }

            return output;
        }


        private async void cbWordsNumber_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            if (cbWords.Checked)
            {
                await LoadNewText();
            }
        }

        private async void cBpunctuation_CheckedChanged(object sender, EventArgs e)
        {
            if (cbWords.Checked || cbTime.Checked)
            {
                await LoadNewText();
                currentIndex = 0;
            }
        }

        private async void cbNumbers_CheckedChanged(object sender, EventArgs e)
        {
            if (cbWords.Checked || cbTime.Checked)
            {
                await LoadNewText();
                currentIndex = 0;
            }
        }

    }
}
