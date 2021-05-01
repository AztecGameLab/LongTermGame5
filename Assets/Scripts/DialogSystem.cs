using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EasyButtons;
using FMODUnity;
using UnityEngine.InputSystem;

public class DialogSystem : MonoBehaviour
{
    public static bool isDialoging;
    
    public float charPerSecond = 1;

    [EventRef] public string greetingSound;
    public bool hasGreetingSound;

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

    public void StartDialog()
    {
        if (_currentDialogIndex >= 0)
            return;

        if (hasGreetingSound)
            RuntimeManager.PlayOneShot(greetingSound);
        
        isDialoging = true;
        ListenForInput();
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
            _secondsPerChar = 0.0001f;
        else if (++_currentDialogIndex < dialog.Length)
        {
            if (_currentRevealString != null)
                StopCoroutine(_currentRevealString);
            _currentRevealString = StartCoroutine(RevealString(dialog[_currentDialogIndex]));
        }
        else
            CloseDialog();
    }

    public Action finishedDialog;
    private void CloseDialog()
    {
        isDialoging = false;
        StopListenForInput();
        _currentDialogIndex = -1;
        finishedDialog?.Invoke();
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

        while (visibleCount <= totalCharacters)
        {
            _textComponent.maxVisibleCharacters = visibleCount;

            visibleCount++;

            yield return new WaitForSeconds(_secondsPerChar);
        }

        _secondsPerChar = _defaultSecondsPerChar;
        _textRevealing = false;
        _currentRevealString = null;
    }



    void ListenForInput()
    {
        PlatformerController.instance.Inputs.Player.Get().FindAction("Interact", true).performed += Performed;
    }

    void StopListenForInput()
    {
        PlatformerController.instance.Inputs.Player.Get().FindAction("Interact", true).performed -= Performed;
    }
    

    void Performed(InputAction.CallbackContext context)
    {
        InteractDialog();
    }
}