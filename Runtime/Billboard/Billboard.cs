using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIToolkit.Billboard
{
    public class Billboard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private BillboardConfig config;
        [SerializeField] private BillboardEffect effect;
        [SerializeField] private Button button;
        public bool ShowOnAwake { get; set; }
        private bool? _isShown = null;
        private float _lastHideTime;
        private bool _canShow = false;
        private Transform _transform;
        private Transform Transform => _transform ??= transform;
        private RectTransform _rectTransform;
        private RectTransform RectTransform => _rectTransform ??= Transform as RectTransform;
        private Canvas _canvas;
        private Canvas Canvas => _canvas ??= Transform.GetComponentInParent<Canvas>();
        public GameObject RelatedAsset { get; set; }
        private Transform CurrentCameraTransform { get; set; }

        public bool CanShow
        {
            set
            {
                _canShow = value;
                if (button)
                {
                    button.interactable = _canShow;
                }
            }
        }

        private void Awake()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            if (effect == null) return;
            effect.PlayEffect(BillboardEffectType.Hide, true);
        }

        private void Start()
        {
            if (ShowOnAwake)
            {
                var canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup)
                {
                    canvasGroup.interactable = true;
                }
            }
        }

        public BillboardConfig Config
        {
            get => config;
            set => config = value;
        }

        public Vector3 CurrentPositionOffset { get; private set; } = Vector3.zero;

        private void Update()
        {
            if (config.Target == null)
            {
                return;
            }
            Camera currentCamera = Camera.main;
            if (currentCamera == null)
            {
                return;
            }
            CurrentCameraTransform = currentCamera.transform;
            Vector3 position = config.Target.position + config.Offset;
            float difference = Vector3.Angle(position - CurrentCameraTransform.position, CurrentCameraTransform.forward);
            bool isShown = currentCamera != null && difference < 90f && -90f < difference;
            if (config.MaxDistance > 0f)
            {
                isShown = isShown && _canShow && Vector3.Distance(CurrentCameraTransform.position, position) <= config.MaxDistance;
            }

            if (_isShown != isShown)
            {
                if (effect != null)
                {
                    effect.PlayEffect(isShown ? BillboardEffectType.Show : BillboardEffectType.Hide, this._isShown == null);
                }
                _isShown = isShown;
                if (isShown == false)
                {
                    _lastHideTime = Time.time;
                }
            }

            var effectDuration = 0f;
            if (effect != null)
            {
                effectDuration = effect.EffectDuration;
            }
            if (isShown || _lastHideTime > Time.time - effectDuration)
            {
                position = currentCamera.WorldToScreenPoint(position);
                position.z = Canvas.transform.position.z;
                Vector3 pos = position;
                if (config.AutoFit)
                {
                    Vector3 prevPos = pos;
                    var pixelRect = Canvas.pixelRect;
                    pos.x = Mathf.Clamp(pos.x, pixelRect.width, pixelRect.width);
                    pos.y = Mathf.Max(pos.y, RectTransform.rect.height);
                    CurrentPositionOffset = pos - prevPos;
                }
                else
                {
                    pos += CurrentPositionOffset;
                }
                RectTransform.position = pos;
                if (config.RotationOffset != 0f)
                {
                    Vector3 rot = RectTransform.eulerAngles;
                    rot.x = config.RotationOffset - CurrentCameraTransform.eulerAngles.x;
                    RectTransform.eulerAngles = rot;
                }
            }
        }

        [Serializable]
        public class BillboardConfig
        {
            [SerializeField] private Transform target;
            [SerializeField] private Vector3 offset;
            [SerializeField] private bool autoFit;
            [SerializeField] private float maxDistance = 500f;
            [SerializeField] private LayerMask collidingLayers;
            [SerializeField] private float rotationOffset;

            public Transform Target
            {
                get => target;
                set => target = value;
            }

            public Vector3 Offset
            {
                get => offset;
                set => offset = value;
            }

            public bool AutoFit
            {
                get => autoFit;
                set => autoFit = value;
            }

            public float MaxDistance
            {
                get => maxDistance;
                set => maxDistance = value;
            }

            public LayerMask CollidingLayers
            {
                get => collidingLayers;
                set => collidingLayers = value;
            }

            public float RotationOffset
            {
                get => rotationOffset;
                set => rotationOffset = value;
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isShown == false)
            {
                return;
            }
            if (effect == null) return;
            effect.PlayEffect(BillboardEffectType.Hover);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isShown == false)
            {
                return;
            }
            if (effect == null) return;
            effect.PlayEffect(BillboardEffectType.Blur);
        }


        public void HideBillboard()
        {
            _canShow = false;
            if (_isShown == false)
            {
                return;
            }
            _isShown = false;
            if (button)
            {
                button.interactable = false;
            }

            if (effect != null)
            {
                effect.PlayEffect(BillboardEffectType.Hide);   
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        public void ShowBillboard()
        {
            if (_isShown == true)
            {
                return;
            }
            _canShow = true;
            if (button)
            {
                button.interactable = true;
            }

            if (effect != null)
            {
                effect.PlayEffect(BillboardEffectType.Show);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        public enum BillboardEffectType
        {
            Show = 0,
            Hide = 1,
            Hover = 2,
            Blur = 3
        }
    }
}