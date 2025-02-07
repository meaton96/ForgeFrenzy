using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame(int index) => UnityEngine.SceneManagement.SceneManager.LoadScene(index);

    public void QuitGame() => Application.Quit();
}
