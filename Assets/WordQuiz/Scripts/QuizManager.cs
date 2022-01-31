using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public static QuizManager instance;
    [SerializeField] GameObject gameover;
    [SerializeField] private QuizDataScriptable questionData;
    [SerializeField] private Image questionImage;
    [SerializeField] private WordData[] answerWordArray;
    [SerializeField] private WordData[] optionsWordArray;
    [SerializeField] char[] AlphaBeth;

    char[] charArray = new char[12];
    int currentAnswerIndex = 0;
    bool correctAnswer = true;
    List<int> selectedWordIndex;
    int currentQuestionIndex = 0;
    GameStatus gameStatus = GameStatus.Playing;
    string answerWord;
    int score = 0;
    public static int LevelPassed;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        selectedWordIndex = new List<int>();
    }

    private char randomPersianChar()
    {
        char[] persianLetters = new char[32]{
        'ا',
        'ب',
        'پ',
        'ت',
        'ث',
        'ج',
        'چ',
        'ح',
        'خ',
        'د',
        'ذ',
        'ر',
        'ز',
        'ژ',
        'س',
        'ش',
        'ص',
        'ض',
        'ط',
        'ظ',
        'ع',
        'غ',
        'ف',
        'ق',
        'ک',
        'گ',
        'ل',
        'م',
        'ن',
        'و',
        'ه',
        'ی'
        };
        return persianLetters[(int)Random.Range(0, persianLetters.Length)];
    }

    private void Start()
    {
        SetQuestion();
    }
    void SetQuestion()
    {
        currentAnswerIndex = 0;
        selectedWordIndex.Clear();
        questionImage.sprite = questionData.questions[currentQuestionIndex].questionImage;
        answerWord = questionData.questions[currentQuestionIndex].answer;
        ResetQuestion();

        for (int i = 0; i < answerWord.Length; i++)
        {
            charArray[i] = char.ToUpper(answerWord[i]);
        }
        for (int i = answerWord.Length; i < optionsWordArray.Length; i++)
        {
            charArray[i] = randomPersianChar();
        }
        charArray = ShuffleList.ShuffleListItems<char>(charArray.ToList()).ToArray();
        for (int i = 0; i < optionsWordArray.Length; i++)
        {
            optionsWordArray[i].SetChar(charArray[i]);
        }
        currentQuestionIndex++;
        gameStatus = GameStatus.Playing;
    }
    public void SelectedOption(WordData wordData)
    {
        if (gameStatus == GameStatus.Next || currentAnswerIndex >= answerWord.Length)
        {
            return;
        }
        selectedWordIndex.Add(wordData.transform.GetSiblingIndex());
        answerWordArray[currentAnswerIndex].SetChar(wordData.charValue);
        wordData.gameObject.SetActive(false);
        currentAnswerIndex++;

        if (currentAnswerIndex >= answerWord.Length)
        {
            correctAnswer = true;
            for (int i = 0; i < answerWord.Length; i++)
            {
                if (char.ToUpper(answerWord[i]) != char.ToUpper(answerWordArray[i].charValue))
                {
                    correctAnswer = false;
                    break;
                }
            }
            if (correctAnswer)
            {
                gameStatus = GameStatus.Next;
                score++;
                if (currentQuestionIndex < questionData.questions.Count)
                {
                    Invoke("SetQuestion", 0.5f);
                }
                else
                {
                    gameover.SetActive(true);
                    LevelPassed = PlayerPrefs.GetInt("LevelPassed", LevelPassed);
                    if (SceneManager.GetActiveScene().buildIndex > LevelPassed)
                    {
                        LevelPassed = SceneManager.GetActiveScene().buildIndex;
                        PlayerPrefs.SetInt("LevelPassed", LevelPassed);
                    }
                }
            }
            else if (!correctAnswer)
            {
                print("False");
            }
        }
    }
    private void ResetQuestion()
    {
        for (int i = 0; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(true);
            answerWordArray[i].SetChar('_');
        }
        for (int i = answerWord.Length; i < answerWordArray.Length; i++)
        {
            answerWordArray[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < optionsWordArray.Length; i++)
        {
            optionsWordArray[i].gameObject.SetActive(true);
        }
    }
    public void ResetLastWord()
    {
        if (selectedWordIndex.Count > 0)
        {
            int index = selectedWordIndex[selectedWordIndex.Count - 1];
            optionsWordArray[index].gameObject.SetActive(true);
            selectedWordIndex.RemoveAt(selectedWordIndex.Count - 1);
            currentAnswerIndex--;
            answerWordArray[currentAnswerIndex].SetChar('_');
        }
    }
}

[System.Serializable]
public class QuestionData
{
    public Sprite questionImage;
    public string answer;
}
public enum GameStatus
{
    Playing,
    Next
}