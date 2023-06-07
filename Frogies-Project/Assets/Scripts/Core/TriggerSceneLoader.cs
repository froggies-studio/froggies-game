using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerSceneLoader : BasicSceneLoader
    {
        [SerializeField] private Collider2D entityCollider;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.Equals(entityCollider))
            {
                LoadScene();
            }
        }
    }
}