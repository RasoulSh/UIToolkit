using TweenerSystem.Enums;
using TweenerSystem.Tweeners;
using UnityEngine;

namespace UIToolkit.Billboard
{
    public class BillboardScaleEffect : BillboardEffect
    {
        [SerializeField] private Vector3 hoverScale = Vector3.one * 1.25f;
        [SerializeField] private LocalScaleTweener scaleTweener;
        public override float EffectDuration => scaleTweener.TotalDuration;

        public override void PlayEffect(Billboard.BillboardEffectType effectType, bool ignoreAnimation = false)
        {
            Vector3 targetScale = Vector3.one;
            switch (effectType)
            {
                case Billboard.BillboardEffectType.Hide:
                    targetScale = Vector3.zero;
                    break;
                case Billboard.BillboardEffectType.Hover:
                    targetScale = hoverScale;
                    break;
            }
            PlayScaleAnimation(targetScale, ignoreAnimation);
        }

        private void PlayScaleAnimation(Vector3 to, bool ignoreAnimation = false)
        {
            scaleTweener.From = transform.localScale;
            scaleTweener.To = to;
            scaleTweener.Play(TweenerDirection.Backward, true);
            scaleTweener.Play(TweenerDirection.Forward, ignoreAnimation);
        }
    }
}