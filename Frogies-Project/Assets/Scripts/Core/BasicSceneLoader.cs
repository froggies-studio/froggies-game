using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class BasicSceneLoader: MonoBehaviour
    {
        [SerializeField] protected string nextSceneName;

        public void LoadScene()
        {
            if (nextSceneName.Equals("Quit"))
            {
                Debug.Log("Quiting Game!");
                Application.Quit();
                return;
            }

            SceneManager.LoadScene(nextSceneName);
        }
    }
}