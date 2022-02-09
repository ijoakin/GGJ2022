using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        //AudioManager.Instance.PlayMusicLoop(AudioManager.MusicId.TRANS);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
