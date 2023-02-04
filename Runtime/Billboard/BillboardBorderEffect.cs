using TweenerSystem;
using TweenerSystem.Enums;
using TweenerSystem.Tweeners;
using UnityEngine;

namespace UIToolkit.Billboard
{
    public class BillboardBorderEffect : BillboardEffect
    {
        [SerializeField] private Tweener showTweener;
        [SerializeField] private GraphicAlphaTweener borderAlphaAnim;
        [SerializeField] private LocalScaleTweener borderScaleAnim;


        public override float EffectDuration
        {
            get
            {
                return Mathf.Max(showTweener.TotalDuration, borderAlphaAnim.TotalDuration,
                    borderScaleAnim.TotalDuration);
            }
        }
        public override void PlayEffect(UIToolkit.Billboard.Billboard.BillboardEffectType effectType, bool ignoreAnimation = false)
        {
            switch (effectType)
            {
                case UIToolkit.Billboard.Billboard.BillboardEffectType.Show:
                    showTweener.Play(TweenerDirection.Forward, ignoreAnimation);
                    break;
                case UIToolkit.Billboard.Billboard.BillboardEffectType.Hide:
                    borderAlphaAnim.Play(TweenerDirection.Backward, ignoreAnimation);
                    borderScaleAnim.Play(TweenerDirection.Backward, ignoreAnimation);
                    showTweener.Play(TweenerDirection.Backward, ignoreAnimation);
                    break;
                case UIToolkit.Billboard.Billboard.BillboardEffectType.Hover:
                    borderAlphaAnim.Play(TweenerDirection.Forward, ignoreAnimation);
                    borderScaleAnim.Play(TweenerDirection.Forward, ignoreAnimation);
                    break;
                case UIToolkit.Billboard.Billboard.BillboardEffectType.Blur:
                    borderAlphaAnim.Play(TweenerDirection.Backward, ignoreAnimation);
                    borderScaleAnim.Play(TweenerDirection.Backward, ignoreAnimation);
                    break;
            }
        }
    }
}