using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class BasicSceneLoader: MonoBehaviour
    {
        [SerializeField] protected SceneAsset nextScene;

        protected void LoadScene()
        {
            SceneManager.LoadScene(nextScene.name);
        }
    }
}