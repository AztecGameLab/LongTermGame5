using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class TextSystemManager : MonoBehaviour
{
    public GameObject BackgroundDefault;
    public GameObject BackgroundOracle;
    public GameObject BackgroundWindGod;
    public GameObject BackgroundFireGod;
    public GameObject BackgroundNatureGod;
    public GameObject BackgroundWaterGod;

    public bool textFullyRevealed;

    public TextMeshProUGUI textMeshProDefault;
    public TextMeshProUGUI textMeshProOracle;
    public TextMeshProUGUI textMeshProWindGod;
    public TextMeshProUGUI textMeshProFireGod;
    public TextMeshProUGUI textMeshProNatureGod;
    public TextMeshProUGUI textMeshProWaterGod;

    // Start is called before the first frame update
    void Start()
    {
        BackgroundDefault = GameObject.Find("BackgroundDefault");
        BackgroundOracle = GameObject.Find("BackgroundOracle");
        BackgroundWindGod = GameObject.Find("BackgroundWindGod");
        BackgroundFireGod = GameObject.Find("BackgroundFireGod");
        BackgroundNatureGod = GameObject.Find("BackgroundNatureGod");
        BackgroundWaterGod = GameObject.Find("BackgroundWaterGod");

        textMeshProDefault = GameObject.Find("TextDefault").GetComponent<TextMeshProUGUI>();
        textMeshProOracle = GameObject.Find("TextOracle").GetComponent<TextMeshProUGUI>();
        textMeshProWindGod = GameObject.Find("TextWindGod").GetComponent<TextMeshProUGUI>();
        textMeshProFireGod = GameObject.Find("TextFireGod").GetComponent<TextMeshProUGUI>();
        textMeshProNatureGod = GameObject.Find("TextNatureGod").GetComponent<TextMeshProUGUI>();
        textMeshProWaterGod = GameObject.Find("TextWaterGod").GetComponent<TextMeshProUGUI>();

        BackgroundDefault.SetActive(false);
        BackgroundOracle.SetActive(false);
        BackgroundWindGod.SetActive(false);
        BackgroundFireGod.SetActive(false);
        BackgroundNatureGod.SetActive(false);
        BackgroundWaterGod.SetActive(false);

        string dialog = "AAAAAAAAAAAAggggggggggggAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgggggggggggggggggggggggggggggggggggggggggggggggggggggAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAggggggggggAAAAAAAAAAAAAAGGGGGGGGGGGGGGGGGggggggggggggggggggggggggggggggggggg";

        //WriteDialog(DialogLook.Normal, dialog);
        //WriteDialog(DialogAesthetic.Oracle, dialog);
        //WriteDialog(DialogAesthetic.WindGod, dialog);
        //WriteDialog(DialogAesthetic.FireGod, dialog);
        //WriteDialog(DialogAesthetic.NatureGod, dialog);
        //WriteDialog(DialogAesthetic.WaterGod, dialog);
    }

    public Sprite normal, oracle, waves, sky, nature, ember, mortal;

    public Image dialogBackground;

    [TextArea(3, 10)] public string[] textsToShow;

    public int charactersPerSecond = 1;
    public int revealedCharacters;
    public int maxCharacterCount;

    public TextMeshPro dialogText;

    [SerializeField] DialogLook dialogLook;

    enum DialogLook
    {
        Normal,
        Oracle,
        Waves,
        Sky,
        Nature,
        Ember,
        Mortal
    };

    public void SetDialogLook()
    {
        if(dialogLook == DialogLook.Normal)
        {
            dialogBackground.sprite = normal;
        }
        else if (dialogLook == DialogLook.Oracle)
        {
            dialogBackground.sprite = oracle;
        }
        else if (dialogLook == DialogLook.Waves)
        {
            dialogBackground.sprite = waves;
        }
        else if (dialogLook == DialogLook.Sky)
        {
            dialogBackground.sprite = sky;
        }
        else if (dialogLook == DialogLook.Nature)
        {
            dialogBackground.sprite = nature;
        }
        else if (dialogLook == DialogLook.Ember)
        {
            dialogBackground.sprite = ember;
        }
        else if (dialogLook == DialogLook.Mortal)
        {
            dialogBackground.sprite = mortal;
        }
        else 
        {
            Debug.LogError("dialogBackground.sprite has an unexpected 'dialogLook.' \n" +
                           "This should never print");
        }
    }

    public void InitiateDialog(int dialogSet = 0)
    {

    }

    public void ContinueDialog()
    {

    }

    public void CloseDialog()
    {

    }

    IEnumerator ScrollText()
    {
        float timePerCharacter = 1 / charactersPerSecond;
        while (revealedCharacters < maxCharacterCount)
        {
            dialogText.maxVisibleCharacters = revealedCharacters;
            yield return new WaitForSeconds(timePerCharacter);
            revealedCharacters++;
        }
    }

    

    /* The Update function is used to allow the player to fullyReveal the text by pushing any button while text is 
     * on the screen
     * 
     * If the text has already been fully revealed, any button will instead reset the textboxes and close them
     * 
     *
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!textFullyRevealed)
            {
                foreach (Image rowToReveal in textHiderDefaultRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(rowToReveal.rectTransform.anchorMax.x, 0);
                }
                foreach (Image rowToReveal in textHiderOracleRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(rowToReveal.rectTransform.anchorMax.x, 0);
                }
                foreach (Image rowToReveal in textHiderWindGodRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(rowToReveal.rectTransform.anchorMax.x, 0);
                }
                foreach (Image rowToReveal in textHiderFireGodRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(rowToReveal.rectTransform.anchorMax.x, 0);
                }
                foreach (Image rowToReveal in textHiderNatureGodRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(rowToReveal.rectTransform.anchorMax.x, 0);
                }
                foreach (Image rowToReveal in textHiderWaterGodRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(rowToReveal.rectTransform.anchorMax.x, 0);
                }
            }
            else
            {
                foreach (Image rowToReveal in textHiderDefaultRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(0, 0);
                }
                foreach (Image rowToReveal in textHiderOracleRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(0, 0);
                }
                foreach (Image rowToReveal in textHiderWindGodRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(0, 0);
                }
                foreach (Image rowToReveal in textHiderFireGodRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(0, 0);
                }
                foreach (Image rowToReveal in textHiderNatureGodRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(0, 0);
                }
                foreach (Image rowToReveal in textHiderWaterGodRow)
                {
                    rowToReveal.rectTransform.anchorMin = new Vector2(0, 0);
                }
                BackgroundDefault.SetActive(false);
                BackgroundOracle.SetActive(false);
                BackgroundWindGod.SetActive(false);
                BackgroundFireGod.SetActive(false);
                BackgroundNatureGod.SetActive(false);
                BackgroundWaterGod.SetActive(false);
            }
        }
    }

    /* The write dialog method allows the user to write "dialog" they provide to the screen
     * The dialogAesthetic parameter controls how the text will look when written to the screen
     * 
     * Lastly the RevealText coroutine is called so the text is slowly revealed rather than immediate.
     *
    public void WriteDialog(DialogLook dialogAesthetic, string dialog)
    {
        if (dialogAesthetic == DialogLook.Normal)
        {
            BackgroundDefault.SetActive(true);
            textMeshProDefault.text = dialog;

            StartCoroutine(RevealText(textHiderDefaultRow));
        }
        else if (dialogAesthetic == DialogLook.Oracle)
        {
            BackgroundOracle.SetActive(true);
            textMeshProOracle.text = dialog;

            StartCoroutine(RevealText(textHiderOracleRow));
        }
        else if (dialogAesthetic == DialogLook.WindGod)
        {
            BackgroundWindGod.SetActive(true);
            textMeshProWindGod.text = dialog;

            StartCoroutine(RevealText(textHiderWindGodRow));
        }
        else if (dialogAesthetic == DialogLook.FireGod)
        {
            BackgroundFireGod.SetActive(true);
            textMeshProFireGod.text = dialog;

            StartCoroutine(RevealText(textHiderFireGodRow));
        }
        else if (dialogAesthetic == DialogLook.NatureGod)
        {
            BackgroundNatureGod.SetActive(true);
            textMeshProNatureGod.text = dialog;

            StartCoroutine(RevealText(textHiderNatureGodRow));
        }
        else if (dialogAesthetic == DialogLook.WaterGod)
        {
            BackgroundWaterGod.SetActive(true);
            textMeshProWaterGod.text = dialog;

            StartCoroutine(RevealText(textHiderWaterGodRow));
        }
    }

    /* This IEnumerator is called whenever text is to be revealed on the screen by WriteDialog()
     * 
     * textFullyRevealed is immediately set to false to prevent the player from closing dialog to early
     * 
     * Then, the "text" is slowly revealed by increasing the position of the hiderImage's anchorMin.x
     * It does this until the anchorMin.x is greater than the the anchorMax.x
     * At which point the next line is revealed
     * This is done for all four potential lines of dialog
     * 
     * Lastly WaitForSeconds is called to prevent the player from accedently skipping a line of dialog
     * Following this hesitation textFullyRevealed is set to true so the player can close the text dialog
     * 
     *
    private IEnumerator RevealText(Image[] hiderImages)
    {
        textFullyRevealed = false;
        for (int i = 0; i < hiderImages.Length; i++)
        {
            while (hiderImages[i].rectTransform.anchorMin.x < hiderImages[i].rectTransform.anchorMax.x)
            {
                hiderImages[i].rectTransform.anchorMin = new Vector2(hiderImages[i].rectTransform.anchorMin.x + textRevealSpeed, 0);
                yield return new WaitForSeconds(textRevealWaitTime);
            }
        }
        yield return new WaitForSeconds(doubleTapHesitation);
        textFullyRevealed = true;
    }
    */
}
