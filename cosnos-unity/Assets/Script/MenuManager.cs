using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject savePanel;
    public GameObject itemPanel;
    public GameObject logPanel;
    public GameObject settingPanel;
    public GameObject exitPanel;

    public TabButtonEffect saveTab;
    public TabButtonEffect itemTab;
    public TabButtonEffect logTab;
    public TabButtonEffect settingTab;
    public TabButtonEffect exitTab;

    void HideAll()
    {
        savePanel.SetActive(false);
        itemPanel.SetActive(false);
        logPanel.SetActive(false);
        settingPanel.SetActive(false);
        exitPanel.SetActive(false);

        saveTab.ResetTab();
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
}
