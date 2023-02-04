using UnityEngine;

namespace UIToolkit.Billboard
{
    public abstract class BillboardEffect : MonoBehaviour
    {
        public abstract float EffectDuration { get; }
        public abstract void PlayEffect(UIToolkit.Billboard.Billboard.BillboardEffectType effectType, bool ignoreAnimation = false);
    }
}