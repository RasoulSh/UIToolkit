using UnityEngine;

namespace UIToolkit.Billboard
{
    public class SimpleBillboard : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        private Transform _transform;
        private Transform Transform => _transform ??= transform;
        private RectTransform _rectTransform;
        private RectTransform RectTransform => _rectTransform ??= Transform as RectTransform;
        private Canvas _canvas;
        private Canvas Canvas => _canvas ??= Transform.GetComponentInParent<Canvas>();
        private Transform CurrentCameraTransform { get; set; }
        public Vector3 CurrentPositionOffset { get; private set; } = Vector3.zero;


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

        private void Update()
        {
            if (Target == null)
            {
                return;
            }
            Camera currentCamera = Camera.main;
            if (currentCamera == null)
            {
                return;
            }
            CurrentCameraTransform = currentCamera.transform;
            Vector3 position = Target.position + Offset;
            float difference = Vector3.Angle(position - CurrentCameraTransform.position, CurrentCameraTransform.forward);
            bool isVisible = currentCamera != null && difference < 90f && -90f < difference;
            if (isVisible == false)
            {
                return;
            }
            position = currentCamera.WorldToScreenPoint(position);
            position.z = Canvas.transform.position.z;
            Vector3 pos = position;
            pos += CurrentPositionOffset;
            RectTransform.position = pos;
        }
    }
}