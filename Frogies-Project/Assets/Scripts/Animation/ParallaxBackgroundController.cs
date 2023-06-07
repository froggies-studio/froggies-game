using Cinemachine;
using Core;
using UnityEngine;

namespace Animation
{
    public class ParallaxBackgroundController : MonoBehaviour
    {
        [SerializeField] private Transform[] parallaxBg;
        [SerializeField] private Transform[] staticBg;

        [SerializeField] private float characterOffset;
        [SerializeField] private float nearClipPlane;
        [SerializeField] private float farClipPlane = 50;

        [SerializeField] private float yFactor = 180f / 320f;

        private Vector3[] _startPositions;
        private Vector3 _lastCameraPosition;

        private void Start()
        {
            _startPositions = new Vector3[parallaxBg.Length];
            for (var i = 0; i < parallaxBg.Length; i++)
            {
                var pixelPosition = GlobalSceneManager.Instance.GlobalCamera.RoundToPixel(parallaxBg[i].position);
                _startPositions[i] = new Vector3(pixelPosition.x, pixelPosition.y, parallaxBg[i].position.z);
            }

            _lastCameraPosition = GlobalSceneManager.Instance.GlobalCamera.RoundToPixel(GlobalSceneManager.Instance.GlobalCamera.transform.position);
            CinemachineCore.CameraUpdatedEvent.AddListener(UpdateParallax);
        }

        private void UpdateParallax(CinemachineBrain arg0)
        {
            Vector3 cameraPosition = arg0.transform.position;
            Vector3 delta = GlobalSceneManager.Instance.GlobalCamera.RoundToPixel(cameraPosition - _lastCameraPosition);
            for (var i = 0; i < parallaxBg.Length; i++)
            {
                Vector3 bgPos = _startPositions[i];
                float distFromSubject = _startPositions[i].z - characterOffset;
                float clippingPlane = cameraPosition.z + (distFromSubject > 0 ? farClipPlane : nearClipPlane);
                float parallaxFactor = Mathf.Abs(distFromSubject) / clippingPlane;

                var parallaxPosition = GlobalSceneManager.Instance.GlobalCamera.RoundToPixel(new Vector2(
                    bgPos.x + delta.x * parallaxFactor,
                    bgPos.y + delta.y * parallaxFactor * yFactor));
                parallaxBg[i].position = new Vector3(parallaxPosition.x, parallaxPosition.y, _startPositions[i].z);
            }

            foreach (var bgTransform in staticBg)
            {
                bgTransform.position = cameraPosition;
            }
        }
    }
}
