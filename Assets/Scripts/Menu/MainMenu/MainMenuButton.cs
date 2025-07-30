using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.MainMenu
{
    public class MainMenuButton : MonoBehaviour
    {
        public void GameStart()
        {
            SceneManager.LoadSceneAsync("GamePlay");
        }

        public void GameQuit()
        {
            if (runInEditMode)
            {
                
            }
            else
            {
                Application.Quit();
            }
            
        }

        public void OpenCreditMenu()
        {
            SceneManager.LoadSceneAsync("Credit");
        }
    }
}
