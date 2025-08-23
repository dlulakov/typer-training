# Typing Speed Trainer (Windows Forms)

A simple Windows Forms desktop application for practicing fast and accurate typing, inspired by [Monkeytype](https://monkeytype.com/).  
The app allows users to take typing tests in English, measure typing speed (WPM) and accuracy, and automatically save results into a `.txt` file. A built-in highscore history helps users track their best performances over time.
The problem this appliation is trying to face is to make so everyone can practice typing and become better typer. The user will start the application and chose what mode he wants to use : words, time, quote, zen. Based on the mode he can start typing and the Text container and if he misspelles a character, the character will become red. And after he types all the words or the time runouts, the WPM will be calculated which is *word per minute*. After that the user can save his score so he can his progress in hte highscore tab.

To desing this systems I am using only the minimum forms, the main from, the highscore form and the add score form. The move and store the data at run-time, I am using *Text* class which represent the text that need to be typed and the information of the WPM, and for the Scores I am using a separte class which then I am using HighScore class which is using composition to store the list of Scores. And the HighScore object is stored in the "highscore.txt" to store scores.

In this project, I used [Chat GPT](https://chatgpt.com/) to resolve some problem and generate function.

---

## 📸 Screenshots
<!-- Add your screenshots here -->
- Main Menu  
  ![Main Menu](MainPage.PNG)

- Typing Test  
  ![Typing Test](Typing.PNG)

- Highscores  
  ![Highscores](HighScores.PNG)
  
- Add Score    
  ![Add Score](Add_Score.PNG)

---

## ✨ Features
- **Typing modes**: Choose between timed tests (e.g., 30/60 seconds) or fixed number of words.
- **Typing metrics**: Calculates Words Per Minute (WPM) and Accuracy.
- **Result saving**: Automatically stores test results in a `.txt` file.
- **Highscore history**: Keeps a record of the best scores for easy progress tracking.
- **Lightweight UI**: Minimalist Windows Forms design for focus on typing.

---

## 🛠️ Technology Stack
- **Language:** C#
- **Framework:** .NET (Windows Forms)
- **Storage:** Plain text file (`highscores.txt`)

---
