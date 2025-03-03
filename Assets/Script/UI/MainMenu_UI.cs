using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private string sceneName = "MainScene";

        public void ContinueGame()
        {
            SceneManager.LoadScene(sceneName);
        }

        public void NewGame()
        {
            SceneManager.LoadScene(sceneName);
        }

        public void ExitGame()
        {
            Debug.Log("Exit");
            //Application.Quit();
        }
    }
}