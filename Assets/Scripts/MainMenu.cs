using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        //AudioManager.Instance.PlayMusicLoop(AudioManager.MusicId.TRANS);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("History");
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ExitButton()
    {
        // TODO: Confirmation dialog
        Application.Quit();
    }
}
