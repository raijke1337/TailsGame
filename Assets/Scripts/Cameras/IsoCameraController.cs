using Arcatech.Level;
using Arcatech.Managers;
using Arcatech.Units.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Arcatech.Scenes.Cameras
{
    [RequireComponent(typeof(Camera))]
    public class IsoCameraController : LoadedManagerBase
    {
        private AimingComponent _playerAimingComponent;
        private Vector3 _cameraTargetPoint;


        [Header("Camera settings")]
        [SerializeField] private Camera _camera;
        [SerializeField] private Vector3 _desiredOffsetFromPlayer = Vector3.zero;
        [SerializeField, Range(0.1f, 3), Tooltip("How fast the camera moves")] private float _catchUpSpeed = 2f;
        [SerializeField,Range(1,5f),Tooltip("extra range when looking with mouse")] private float _lookDist = 1f;

        [Header("Fade out Settings")]
        [SerializeField,Tooltip("View radius of player for fade")] float _viewRadius = 5f;
        [SerializeField, Tooltip("the position of raycast target on the player")] Vector3 _targetAimOffset = Vector3.zero;


        [SerializeField] LayerMask _layerMask;
        [Range(0, 1f), SerializeField] float _targetFadedAlpha = 0.33f;
        [SerializeField] bool _keepShadows = true;
        [SerializeField] float _fadeSpeed = 0.5f;


        [Header("Fade out info")]
        [SerializeField] private List<FadingDecorComponent> _currentlyFadingList = new List<FadingDecorComponent>();
        private Dictionary<FadingDecorComponent, Coroutine> _cors;
        private RaycastHit[] _hitsThisFrame;


        #region managed
        public override void Initiate()
        {
            _camera = GetComponent<Camera>();
            if (_playerAimingComponent == null) _playerAimingComponent = GameManager.Instance.GetGameControllers.UnitsManager.GetPlayerUnit.GetInputs<InputsPlayer>().Aiming;

            _hitsThisFrame = new RaycastHit[50];
            _cors = new Dictionary<FadingDecorComponent, Coroutine>();
            transform.position = _playerAimingComponent.transform.position + _desiredOffsetFromPlayer;

             //Debug.Log($"Camera initiate, target is {_playerTarget.gameObject}");
        }

        public override void RunUpdate(float delta)
        {
            if (_playerAimingComponent.GetDistanceToTarget < _lookDist)
            {
                _cameraTargetGizmoColor = Color.green;
                if (ShowDebug)                
                {
                    Debug.Log($"Switching camera target to far look");
                }

                _cameraTargetPoint = _playerAimingComponent.GetLookTarget;
            }
            else
            {
                _cameraTargetGizmoColor = Color.yellow;
                if (ShowDebug)
                {
                    Debug.Log($"Switching camera target to close look");
                }
                _cameraTargetPoint = _playerAimingComponent.transform.position + _playerAimingComponent.GetNormalizedDirectionToTaget * _lookDist;

            }

            transform.position = Vector3.Slerp(transform.position, _cameraTargetPoint + _desiredOffsetFromPlayer, Time.deltaTime* _catchUpSpeed);
            SphereCastForHiding();

        }

        public override void Stop()
        {
            StopAllCoroutines();
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
                            if (_cors.ContainsKey(fading))
                            {
                               // Debug.Log($"Stop coroutine (expected show) for {fading}");
                                StopCoroutine(_cors[fading]);
                            }
                            //Debug.Log($"Add {fading} to list adn start hiding coroutine");
                            _currentlyFadingList.Add(fading);
                            _cors[fading] = StartCoroutine(HideObject(fading));
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
                    if (_cors.ContainsKey(decor) && _cors[decor] != null)
                    {
                     //   Debug.Log($"Stop coroutine (expected hide) for {decor} and starting show");
                        StopCoroutine(_cors[decor]);
                    }
                    var c = StartCoroutine(ShowObject(decor));
                    _cors[decor] = c; 
                }


                //clear hits storage
                System.Array.Clear(_hitsThisFrame, 0, _hitsThisFrame.Length);

            }
        }
            private FadingDecorComponent GetFadingDecorFromHit(RaycastHit hit)
            {
                return hit.collider != null ? hit.collider.GetComponent<FadingDecorComponent>() : null;
            }

            private IEnumerator HideObject(FadingDecorComponent f)
            {
          //  Debug.Log($"Start Coroutine Hide for {f}");
            foreach (Material m in f.Materials)
            {
                if ((m.HasProperty("_BaseColor")) || (m.HasProperty("_Color")))
                {
                    // shader magic goes here (see debug inspector for opaque vs transparent materials values)
                    // prepare materials
                    m.SetFloat("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    m.SetFloat("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    m.SetFloat("_ZWrite", 0);

                    // transparent surface type or not
                    m.SetFloat("_Surface", 1);

                    m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                    m.SetShaderPassEnabled("DepthOnly", false);
                    m.SetShaderPassEnabled("SHADOWCASTER", _keepShadows);

                    m.SetOverrideTag("RenderType", "Transparent");

                    m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                    m.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                }
            }

            if ((f.Materials[0].HasProperty("_BaseColor")) || (f.Materials[0].HasProperty("_Color"))) 
            {
                float t = 0;
                // check for custom shaders from asset store, should only affect the outline

                while (f.Materials[0].GetColor("_BaseColor").a > _targetFadedAlpha)
                {
                    foreach (Material m in f.Materials)
                    {
                        if ((m.HasProperty("_BaseColor")) || !(m.HasProperty("_Color")))
                        {

                            if (m.HasProperty("_BaseColor"))
                            {
                                Color c = new Color(
   m.GetColor("_BaseColor").r, m.GetColor("_BaseColor").g, m.GetColor("_BaseColor").b,
      Mathf.Lerp(f.InitialAlpha, _targetFadedAlpha, t * _fadeSpeed));
                                m.SetColor("_BaseColor", c);
                            }

                            if (m.HasProperty("_Color"))
                            {
                                Color c = new Color(
                                m.GetColor("_Color").r, m.GetColor("_Color").g, m.GetColor("_Color").b,
      Mathf.Lerp(f.InitialAlpha, _targetFadedAlpha, t * _fadeSpeed));
                                m.SetColor("_Color", c);
                            }
                        }
                    }

                    t += Time.deltaTime;
                    yield return null;
                }
            }



           // Debug.Log($"Complete Coroutine Hide for {f}");
            yield return null;
        }

            private IEnumerator ShowObject(FadingDecorComponent f)
            {
           // Debug.Log($"Start Coroutine Show for {f}");
            float t = 0;
            while (f.Materials[0].GetColor("_BaseColor").a < f.InitialAlpha)
            {
                foreach (Material m in f.Materials)
                {

                    Color c = new(
                            m.GetColor("_BaseColor").r, m.GetColor("_BaseColor").g, m.GetColor("_BaseColor").b,
                            Mathf.Lerp(_targetFadedAlpha, f.InitialAlpha, t * _fadeSpeed));

                    if (m.HasProperty("_BaseColor"))
                    {
                        m.SetColor("_BaseColor", c);
                    }
                    if (m.HasProperty("_Color"))
                    {
                        m.SetColor("_Color", c);
                    }
                }
                t += Time.deltaTime;
                yield return null;
            }

            foreach (Material m in f.Materials)
            {
               //set back to opaque

                    m.SetFloat("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                m.SetFloat("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                m.SetFloat("_ZWrite", 1);
                m.SetFloat("_Surface", 0);

                m.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

                m.SetShaderPassEnabled("DepthOnly", true);
                m.SetShaderPassEnabled("SHADOWCASTER", true);

                m.SetOverrideTag("RenderType", "Opaque");

                m.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            //Debug.Log($"Complete Coroutine Show for {f}");
            yield return null;  
        }

        #endregion


        private Color _cameraTargetGizmoColor;
        private void OnDrawGizmos()
        {

            if (_camera == null) return;
            Gizmos.color = _cameraTargetGizmoColor;
            Gizmos.DrawWireSphere(_cameraTargetPoint, 0.15f);
            Gizmos.DrawWireSphere(_playerAimingComponent.GetLookTarget, 0.1f);
            Gizmos.DrawLine(_playerAimingComponent.transform.position, 
                _playerAimingComponent.transform.position + _playerAimingComponent.GetNormalizedDirectionToTaget);

        }
    }


}

