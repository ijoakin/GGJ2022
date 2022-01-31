using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("FinalLevel");
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }
}
