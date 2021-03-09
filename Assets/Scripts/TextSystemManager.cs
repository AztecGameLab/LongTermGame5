using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSystemManager : MonoBehaviour
{
    public enum DialogAesthetic {Default, Oracle, WindGod, FireGod, NatureGod, WaterGod};

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

        DialogAesthetic DefaultAestetic = DialogAesthetic.Default;
        DialogAesthetic OracleAestetic = DialogAesthetic.Oracle;
        DialogAesthetic WindGodAestetic = DialogAesthetic.WindGod;
        DialogAesthetic FireGodAestetic = DialogAesthetic.FireGod;
        DialogAesthetic NatureGodAestetic = DialogAesthetic.NatureGod;
        DialogAesthetic WaterGodAestetic = DialogAesthetic.WaterGod;
        string dialog = "AAAAAAAAAAAAggggggggggggAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgggggggggggggggggggggggggggggggggggggggggggggggggggggAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAggggggggggAAAAAAAAAAAAAAGGGGGGGGGGGGGGGGGggggggggggggggggggggggggggggggggggg";

        //WriteDialog(DefaultAestetic, dialog);
        //WriteDialog(OracleAestetic, dialog);
        //WriteDialog(WindGodAestetic, dialog);
        //WriteDialog(FireGodAestetic, dialog);
        //WriteDialog(NatureGodAestetic, dialog);
        WriteDialog(WaterGodAestetic, dialog);

    }

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

    private IEnumerator RevealText(Image[] hiderImages)
    {
        for (int i = 0; i < hiderImages.Length; i++)
        {
            while (hiderImages[i].rectTransform.anchorMin.x < hiderImages[i].rectTransform.anchorMax.x)
            {
                hiderImages[i].rectTransform.anchorMin = new Vector2(hiderImages[i].rectTransform.anchorMin.x + .002f, 0);
                yield return new WaitForSeconds(.01f);
            }
        }
    }
}
