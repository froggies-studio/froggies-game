using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class BasicSceneLoader: MonoBehaviour
    {
        [SerializeField] [CanBeNull] protected SceneAsset nextScene;

        public void LoadScene()
        {
            if (!nextScene)
            {
                Debug.Log("Quiting Game!");
                Application.Quit();
                return;
            }

            SceneManager.LoadScene(nextScene.name);
        }
    }
}