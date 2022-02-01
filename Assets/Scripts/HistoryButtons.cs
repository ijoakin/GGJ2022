using UnityEngine;

public class HistoryButtons : MonoBehaviour
{
    public void MenuButton()
    {
        History.Instance.Menu();
    }

    public void NextButton()
    {
        History.Instance.Next();
    }

    public void PrevButton()
    {
        History.Instance.Prev();
    }

    public void SkipButton()
    {
        History.Instance.Skip();
    }
}
