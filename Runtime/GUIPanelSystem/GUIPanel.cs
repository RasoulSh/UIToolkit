using TweenerSystem;
using UnityEngine;

namespace UIToolkit.GUIPanelSystem
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GUIPanel : MonoBehaviour
    {
        [SerializeField] private bool isShown = true;
        [SerializeField] private bool initializeOnStart = true;
        [SerializeField] private Tweener tweener;
        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>(); 
        public bool IsShown => isShown;
#if UNITY_EDITOR
        /// <summary>
        /// Important: Only callable in editor
        /// </summary>
        public bool InitialShown
        {
            set
            {
                if (UnityEditor.EditorApplication.isPlaying)
                {
                    Debug.LogError("You can only set initial shown in editor mode");
                    return;
                }
                isShown = value;
            }
        }
        /// <summary>
        /// Important: Only callable in editor
        /// </summary>
        public bool InitializeOnStart
        {
            set
            {
                if (UnityEditor.EditorApplication.isPlaying)
                {
                    Debug.LogError("You can only set initialize on start in editor mode");
                    return;
                }

                initializeOnStart = value;
            }
        }
#endif
        public bool IsInitialized { get; private set; }
        public event GUIPanelDelegate OnToggle;
        public delegate void GUIPanelDelegate(GUIPanel panel);
        protected virtual void Start()
        {
            if (IsInitialized)
            {
                return;
            }
            if (initializeOnStart)
            {
                Initialize();   
            }
        }

        public bool Initialize()
        {
            if (IsInitialized)
            {
                Debug.LogWarning("The GUI panel has been initialized already. You cannot initialize it twice");
                return false;
            }
            IsInitialized = true;
            OnInitialize();
            tweener ??= GetComponent<Tweener>();
            if (tweener != null)
            {
                tweener.Play(isShown, true);
                tweener.Delegation.onFinishPlaying.AddListener(OnTweenerFinishedPlaying);   
            }
            gameObject.SetActive(isShown);
            return true;
        }

        private void OnTweenerFinishedPlaying()
        {
            gameObject.SetActive(isShown);
        }

        public bool IsInteractable
        {
            get => CanvasGroup.interactable;
            set => CanvasGroup.interactable = value;
        }

        public void Show(bool ignoreAnimate = false) => Toggle(true, ignoreAnimate);
        public void Hide(bool ignoreAnimate = false) => Toggle(false, ignoreAnimate);

        public void Toggle(bool isShown, bool ignoreAnimate = false)
        {
            if (IsInitialized == false)
            {
                Initialize();
            }
            if (this.isShown == isShown)
            {
                return;
            }
            this.isShown = isShown;
            if(isShown)
                OnShow();
            else
                OnHide();
            OnToggle?.Invoke(this);
            if (tweener == null)
            {
                gameObject.SetActive(isShown);
                return;
            }
            if (isShown)
            {
                tweener.Play(false, true);
                gameObject.SetActive(true);
            }
            tweener.Play(isShown, ignoreAnimate);
        }

        protected virtual void OnShow(){}
        protected virtual void OnHide(){}
        protected virtual void OnInitialize(){}
    }
}