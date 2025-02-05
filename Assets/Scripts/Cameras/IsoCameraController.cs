using Arcatech.Level;
using Arcatech.Managers;
using Arcatech.Units.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Scenes.Cameras
{
    public class IsoCameraController : MonoBehaviour, IManagedController
    {
        private AimingComponent _playerAimingComponent;
        private Vector3 _cameraTargetPoint;

        [SerializeField] bool DebugMessage = false;
        [Header("Camera settings")]
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector3 _desiredOffsetFromPlayer = Vector3.zero;
        [SerializeField, Range(0.1f, 3), Tooltip("How fast the camera moves")] private float _catchUpSpeed = 2f;
        [SerializeField,Range(1,5f),Tooltip("extra range when looking with mouse")] private float _lookDist = 1f;

        [Header("Fade out Settings")]
        [SerializeField,Tooltip("View radius of player for fade")] float _viewRadius = 5f;
        [SerializeField, Tooltip("the position of raycast target on the player")] Vector3 _targetAimOffset = Vector3.zero;


       // [SerializeField] string _fadingTag;
        [Range(0, 1f), SerializeField] float _targetFadedAlpha = 0.33f;
        [SerializeField] Material _fadedMaterial;
        [SerializeField] float _fadeSpeed = 0.5f;


        [Header("Fade out info")]
        [SerializeField] private List<FadingDecorComponent> _currentlyFadingList = new List<FadingDecorComponent>();
        //private Dictionary<FadingDecorComponent, Coroutine> _cors;
        private RaycastHit[] _hitsThisFrame;

        

        #region managed
        public void StartController()
        {
            _camera = GetComponent<Camera>();           
            _hitsThisFrame = new RaycastHit[50];
           // _cors = new Dictionary<FadingDecorComponent, Coroutine>();           

        }

        public void ControllerUpdate(float delta)
        {
            if (_playerAimingComponent == null)
            {
                _playerAimingComponent = FindObjectOfType<AimingComponent>();
                transform.position = _playerAimingComponent.transform.position + _desiredOffsetFromPlayer;
            }
            else
            {
                if (_playerAimingComponent.GetDistanceToTarget < _lookDist)
                {
                    _cameraTargetGizmoColor = Color.green;
                    if (DebugMessage)
                    {
                        Debug.Log($"Switching camera target to far look");
                    }

                    _cameraTargetPoint = _playerAimingComponent.GetLookTarget;
                }
                else
                {
                    _cameraTargetGizmoColor = Color.yellow;
                    if (DebugMessage)
                    {
                        Debug.Log($"Switching camera target to close look");
                    }
                    _cameraTargetPoint = _playerAimingComponent.transform.position + _playerAimingComponent.GetNormalizedDirectionToTaget * _lookDist;

                }

                transform.position = Vector3.Slerp(transform.position, _cameraTargetPoint + _desiredOffsetFromPlayer, Time.deltaTime * _catchUpSpeed);
                SphereCastForHiding();
            }


        }
        public void FixedControllerUpdate(float fixedDelta)
        {

        }
        public void StopController()
        {

        }

        #endregion
        #region FadeOut

        private void SphereCastForHiding()
        {
            Ray ray = new Ray(_camera.transform.position, _playerAimingComponent.transform.position + _targetAimOffset);
            float distance = (Vector3.Distance(_camera.transform.position, _playerAimingComponent.transform.position + _targetAimOffset));
            int hits = Physics.SphereCastNonAlloc(ray, _viewRadius, _hitsThisFrame, distance);

            //add relevant
            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    FadingDecorComponent fading = GetFadingDecorFromHit(_hitsThisFrame[i]);
                    // see if there are objects hitting the raycast target from camera

                    if (fading != null)
                    {
                        //Debug.Log($"Raycast on relevant object {fading.gameObject.name}");
                        if (!_currentlyFadingList.Contains(fading))
                        {
                            _currentlyFadingList.Add(fading);
                            fading.Fade(_fadeSpeed,_targetFadedAlpha,_fadedMaterial);
                        }
                    }
                }
                //remove irrelevant
                List<FadingDecorComponent> toRemove = new List<FadingDecorComponent>(_currentlyFadingList.Count);

                foreach (FadingDecorComponent decor in _currentlyFadingList)
                {
                    if (decor != null)
                    {
                        bool isDecorInFrameResults = false;
                        for (int i = 0; i < _hitsThisFrame.Length; i++)
                        {
                            FadingDecorComponent compThisFrame = GetFadingDecorFromHit(_hitsThisFrame[i]);
                            if (decor == compThisFrame)
                            {
                                isDecorInFrameResults = true;
                                break;
                                // comp in list still hit
                            }
                        }

                        if (!isDecorInFrameResults)
                        {
                            toRemove.Add(decor);
                        }
                    }
                }
                foreach (FadingDecorComponent decor in toRemove)
                {
                   // Debug.Log($"Remove from fading list {decor}");
                    _currentlyFadingList.Remove(decor);
                    decor.Unfade();
                }


                //clear hits storage
                System.Array.Clear(_hitsThisFrame, 0, _hitsThisFrame.Length);

            }
        }

        private FadingDecorComponent GetFadingDecorFromHit(RaycastHit hit)
        {
            return hit.collider != null ? hit.collider.GetComponent<FadingDecorComponent>() : null;
        }

        #endregion


        private Color _cameraTargetGizmoColor;

    }


}

