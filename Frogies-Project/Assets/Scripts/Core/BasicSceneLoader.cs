using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public class BasicSceneLoader: MonoBehaviour
    {
        [SerializeField] private SceneAsset nextScene;
        [SerializeField] private Collider2D entityCollider;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.Equals(entityCollider))
            {
                LoadScene();
            }
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(nextScene.name);
        }
    }
}