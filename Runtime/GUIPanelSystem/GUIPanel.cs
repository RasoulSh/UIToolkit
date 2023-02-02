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
        public bool IsInitialized { get; private set; }

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

        public virtual bool Initialize()
        {
            if (IsInitialized)
            {
                Debug.LogWarning("The GUI panel has been initialized already. You cannot initialize it twice");
                return false;
            }
            IsInitialized = true;
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
    }
}