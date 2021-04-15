using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EasyButtons;

public class DialogSystem : MonoBehaviour
{
    public float charPerSecond = 1;

    [TextArea(5, 10)]
    public string[] dialog;

    private float _defaultSecondsPerChar;
    private float _secondsPerChar;
    private TMP_Text _textComponent;
    private bool _textRevealing;
    private int _currentDialogIndex;

    private void Start()
    {
        _currentDialogIndex = -1;
        _defaultSecondsPerChar = 1 / charPerSecond;
        _secondsPerChar = _defaultSecondsPerChar;
    }

    [Button]
    public void InteractDialog()
    {
        if(_currentDialogIndex < 0)
            StartDialog();
        else
            NextDialog();
    }

    private void StartDialog()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        _textComponent = GetComponentInChildren<TMP_Text>();

        _currentDialogIndex = 0;
        if (_currentRevealString != null)
            StopCoroutine(_currentRevealString);
        _currentRevealString = StartCoroutine(RevealString(dialog[_currentDialogIndex]));
    }

    private void NextDialog()
    {
        if (_textRevealing)
            _secondsPerChar = 0.001f;
        else if (++_currentDialogIndex < dialog.Length)
        {
            if (_currentRevealString != null)
                StopCoroutine(_currentRevealString);
            _currentRevealString = StartCoroutine(RevealString(dialog[_currentDialogIndex]));
        }
        else
            CloseDialog();
    }

    private void CloseDialog()
    {
        _currentDialogIndex = -1;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private Coroutine _currentRevealString;
    IEnumerator RevealString(string text)
    {
        _textRevealing = true;

        _textComponent.text = text;

        _textComponent.ForceMeshUpdate();

        int totalCharacters = _textComponent.textInfo.characterCount;
        int visibleCount = 0;

        while (visibleCount < totalCharacters)
        {
            _textComponent.maxVisibleCharacters = visibleCount;

            visibleCount++;

            yield return new WaitForSeconds(_secondsPerChar);
        }

        _secondsPerChar = _defaultSecondsPerChar;
        _textRevealing = false;
        _currentRevealString = null;
    }
}