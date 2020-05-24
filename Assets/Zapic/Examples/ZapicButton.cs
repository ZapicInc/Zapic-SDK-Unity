using UnityEngine;
using UnityEngine.UI;

public class ZapicButton : MonoBehaviour
{
    private void Awake()
    {
        var button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogWarning("ZapicButton must have a Button component");
            return;
        }

        button.onClick.AddListener(ShowZapicMenu);
    }

    private void ShowZapicChallengesMenu()
    {
        Zapic.ShowPage(ZapicPage.Challenges);
    }

    private void ShowZapicMenu()
    {
        Zapic.ShowDefaultPage();
    }
}
