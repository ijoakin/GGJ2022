using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("History");
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }
}
