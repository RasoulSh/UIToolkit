using UnityEngine;
using UnityEngine.EventSystems;

namespace UIToolkit.InteractionHelpers
{
    public class MouseInputBehaviour : PointerInputBehaviour
    {
        [SerializeField] private MouseButton mouseButton = MouseButton.LeftClick;
        [SerializeField] private bool ignoreUI;

        public override bool IsPointerBlocked
        {
            get
            {
                if (SystemInfo.deviceType == DeviceType.Handheld)
                {
                    if (Input.touchCount > 0)
                    {
                        return ignoreUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
                    }


                }
                return ignoreUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

            }
        }

        public override Vector2 CurrentPointerPosition => Input.mousePosition;

        public enum MouseButton
        {
            LeftClick = 0,
            RightClick = 1
        }

        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown((int)mouseButton))
            {
                PushDownPointer();
            }
            if (Input.GetMouseButtonUp((int)mouseButton))
            {
                ReleaseUpPointer();
            }
        }
    }
}