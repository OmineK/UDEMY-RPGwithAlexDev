using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("Volume")]
    [SerializeField] UI_VolumeSlider[] volumeSettings;

    [Header("Death sceene")]
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] GameObject youDied;
    [SerializeField] GameObject restartButton;
    [Space]

    [SerializeField] GameObject characterUI;
    [SerializeField] GameObject skillTreeUI;
    [SerializeField] GameObject craftUI;
    [SerializeField] GameObject optionsUI;
    [SerializeField] GameObject exitUI;
    [SerializeField] GameObject inGameUI;

    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillToolTip;

    void Awake()
    {
        SwitchTo(skillTreeUI); //need this to assign skill tree events BEFORE skill scripts (to work properly)
        fadeScreen.gameObject.SetActive(true);
    }

    void Start()
    {
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKey(characterUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKey(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKey(craftUI);

        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchWithKey(optionsUI);
    }

    public void SwitchWithKey(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null) { return; }
        }

        SwitchTo(inGameUI);
    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

            if (!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            AudioManager.instance.PlaySFX(7, null);
            _menu.SetActive(true);
        }

        if (GameManager.instance != null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }

    public void SwitchToEndScreen()
    {
        fadeScreen.FadeOut();

        StartCoroutine(EndScreenCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(2f);
        youDied.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parametr == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parametr, item.slider.value);
        }
    }
}
