using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject savePanel;
    public GameObject itemPanel;
    public GameObject logPanel;
    public GameObject settingPanel;
    public GameObject exitPanel;
    public GameObject menuPanel;
    public NovelGameManager gameManager;
    public TabButtonEffect saveTab;
    public TabButtonEffect itemTab;
    public TabButtonEffect logTab;
    public TabButtonEffect settingTab;
    public TabButtonEffect exitTab;
    public GameObject loadPanel;
    public TabButtonEffect loadTab;
    
    void Start()
    {
        menuPanel.SetActive(false);
    }
    void HideAll()
    {
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        itemPanel.SetActive(false);
        logPanel.SetActive(false);
        settingPanel.SetActive(false);
        exitPanel.SetActive(false);

        saveTab.ResetTab();
        loadTab.ResetTab();
        itemTab.ResetTab();
        logTab.ResetTab();
        settingTab.ResetTab();
        exitTab.ResetTab();
    }

    public void ShowSave()
    {
        HideAll();
        savePanel.SetActive(true);
        saveTab.SelectTab();
    }

    public void ShowLoad()
    {
        HideAll();
        loadPanel.SetActive(true);
        loadTab.SelectTab();
    }
    public void ShowItem()
    {
        HideAll();
        itemPanel.SetActive(true);
        itemTab.SelectTab();
    }

    public void ShowLog()
    {
        HideAll();
        logPanel.SetActive(true);
        logTab.SelectTab();
    }

    public void ShowSetting()
    {
        HideAll();
        settingPanel.SetActive(true);
        settingTab.SelectTab();
    }

    public void ShowExit()
    {
        HideAll();
        exitPanel.SetActive(true);
        exitTab.SelectTab();
    }
    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        gameManager.isMenuOpen = true;
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);

        Debug.Log("menuPanel active: " + menuPanel.activeSelf);
    }
}
