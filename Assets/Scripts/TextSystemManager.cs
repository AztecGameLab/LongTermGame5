using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSystemManager : MonoBehaviour
{
    public enum DialogAesthetic {Default, Oracle, WindGod, FireGod, NatureGod, WaterGod};

    public float textRevealSpeed = .002f;
    public float textRevealWaitTime = .01f;
    public float doubleTapHesitation = 1f;

    public GameObject BackgroundDefault;
    public GameObject BackgroundOracle;
    public GameObject BackgroundWindGod;
    public GameObject BackgroundFireGod;
    public GameObject BackgroundNatureGod;
    public GameObject BackgroundWaterGod;

    public Image[] textHiderDefaultRow = new Image[4];

    public Image[] textHiderOracleRow = new Image[4];

    public Image[] textHiderWindGodRow = new Image[4];

    public Image[] textHiderFireGodRow = new Image[4];

    public Image[] textHiderNatureGodRow = new Image[4];

    public Image[] textHiderWaterGodRow = new Image[4];

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

        textHiderDefaultRow[0] = GameObject.Find("DefaultTextCoverRow1").GetComponent<Image>();
        textHiderDefaultRow[1] = GameObject.Find("DefaultTextCoverRow2").GetComponent<Image>();
        textHiderDefaultRow[2] = GameObject.Find("DefaultTextCoverRow3").GetComponent<Image>();
        textHiderDefaultRow[3] = GameObject.Find("DefaultTextCoverRow4").GetComponent<Image>();
        
        textHiderOracleRow[0] = GameObject.Find("OracleTextCoverRow1").GetComponent<Image>();
        textHiderOracleRow[1] = GameObject.Find("OracleTextCoverRow2").GetComponent<Image>();
        textHiderOracleRow[2] = GameObject.Find("OracleTextCoverRow3").GetComponent<Image>();
        textHiderOracleRow[3] = GameObject.Find("OracleTextCoverRow4").GetComponent<Image>();
        
        textHiderWindGodRow[0] = GameObject.Find("WindGodTextCoverRow1").GetComponent<Image>();
        textHiderWindGodRow[1] = GameObject.Find("WindGodTextCoverRow2").GetComponent<Image>();
        textHiderWindGodRow[2] = GameObject.Find("WindGodTextCoverRow3").GetComponent<Image>();
        textHiderWindGodRow[3] = GameObject.Find("WindGodTextCoverRow4").GetComponent<Image>();
        
        textHiderFireGodRow[0] = GameObject.Find("FireGodTextCoverRow1").GetComponent<Image>();
        textHiderFireGodRow[1] = GameObject.Find("FireGodTextCoverRow2").GetComponent<Image>();
        textHiderFireGodRow[2] = GameObject.Find("FireGodTextCoverRow3").GetComponent<Image>();
        textHiderFireGodRow[3] = GameObject.Find("FireGodTextCoverRow4").GetComponent<Image>();
        
        textHiderNatureGodRow[0] = GameObject.Find("NatureGodTextCoverRow1").GetComponent<Image>();
        textHiderNatureGodRow[1] = GameObject.Find("NatureGodTextCoverRow2").GetComponent<Image>();
        textHiderNatureGodRow[2] = GameObject.Find("NatureGodTextCoverRow3").GetComponent<Image>();
        textHiderNatureGodRow[3] = GameObject.Find("NatureGodTextCoverRow4").GetComponent<Image>();

        textHiderWaterGodRow[0] = GameObject.Find("WaterGodTextCoverRow1").GetComponent<Image>();
        textHiderWaterGodRow[1] = GameObject.Find("WaterGodTextCoverRow2").GetComponent<Image>();
        textHiderWaterGodRow[2] = GameObject.Find("WaterGodTextCoverRow3").GetComponent<Image>();
        textHiderWaterGodRow[3] = GameObject.Find("WaterGodTextCoverRow4").GetComponent<Image>();

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

        WriteDialog(DialogAesthetic.Default, dialog);
        //WriteDialog(DialogAesthetic.Oracle, dialog);
        //WriteDialog(DialogAesthetic.WindGod, dialog);
        //WriteDialog(DialogAesthetic.FireGod, dialog);
        //WriteDialog(DialogAesthetic.NatureGod, dialog);
        //WriteDialog(DialogAesthetic.WaterGod, dialog);
    }

    /* The Update function is used to allow the player to fullyReveal the text by pushing any button while text is 
     * on the screen
     * 
     * If the text has already been fully revealed, any button will instead reset the textboxes and close them
     * 
     */
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
     */
    public void WriteDialog(DialogAesthetic dialogAesthetic, string dialog)
    {
        if (dialogAesthetic == DialogAesthetic.Default)
        {
            BackgroundDefault.SetActive(true);
            textMeshProDefault.text = dialog;

            StartCoroutine(RevealText(textHiderDefaultRow));
        }
        else if (dialogAesthetic == DialogAesthetic.Oracle)
        {
            BackgroundOracle.SetActive(true);
            textMeshProOracle.text = dialog;

            StartCoroutine(RevealText(textHiderOracleRow));
        }
        else if (dialogAesthetic == DialogAesthetic.WindGod)
        {
            BackgroundWindGod.SetActive(true);
            textMeshProWindGod.text = dialog;

            StartCoroutine(RevealText(textHiderWindGodRow));
        }
        else if (dialogAesthetic == DialogAesthetic.FireGod)
        {
            BackgroundFireGod.SetActive(true);
            textMeshProFireGod.text = dialog;

            StartCoroutine(RevealText(textHiderFireGodRow));
        }
        else if (dialogAesthetic == DialogAesthetic.NatureGod)
        {
            BackgroundNatureGod.SetActive(true);
            textMeshProNatureGod.text = dialog;

            StartCoroutine(RevealText(textHiderNatureGodRow));
        }
        else if (dialogAesthetic == DialogAesthetic.WaterGod)
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
     */
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
}
