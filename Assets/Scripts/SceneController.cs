using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public AudioSource audioSourceMusicMenu;
    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
    private void Start()
    {
        audioSourceMusicMenu.Play();
    }
    private void OnDestroy()
    {
        audioSourceMusicMenu.Stop();
    }
}

/*public class SceneController : MonoBehaviour
{
    private void Awake()
    {
        var noDestruirEntreEscenas = FindObjectOfType<SceneController>();
        if(noDestruirEntreEscenas.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(
    }

}*/
