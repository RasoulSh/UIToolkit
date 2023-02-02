using UnityEngine;

namespace UIToolkit.GUIPanelSystem
{
    public class GUIPanelInitializer : MonoBehaviour
    {
        [SerializeField] private GUIPanel[] panels;

        private void Start()
        {
            foreach (var panel in panels)
            {
                if (panel.IsInitialized == false)
                {
                    panel.Initialize();   
                }
            }
        }
    }
}