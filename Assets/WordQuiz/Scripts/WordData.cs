using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordData : MonoBehaviour
{
    [SerializeField] private Text charText;
    [HideInInspector] public char charValue;
    private Button ButtonObj;

    private void Awake()
    {
        ButtonObj = GetComponent<Button>();
        if (ButtonObj)
        {
            ButtonObj.onClick.AddListener(() => CharSelected());
        }
    }

    public void SetChar(char value)
    {
        charText.text = value + "";
        charValue = value;
    }

    private void CharSelected()
    {
        QuizManager.instance.SelectedOption(this);
    }
}
