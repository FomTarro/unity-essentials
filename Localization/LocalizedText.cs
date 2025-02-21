using TMPro;
using UnityEngine;

namespace Skeletom.Essentials.Localization
{
    public class LocalizedText : MonoBehaviour {

        [SerializeField]
        private string key = string.Empty;
        private ITextElement _text;

        private void Awake()
        {
            LocalizationManager.RegisterLocalizedText(this);
        }

        private void OnEnable()
        {
            Localize();
        }

        private void OnDestroy()
        {
            LocalizationManager.UnregisterLocalizedText(this);
        }

        /// <summary>
        /// Change the lookup key for the text and re-localize
        /// </summary>
        /// <param name="newKey"></param>
        public void ChangeKey(string newKey)
        {
            key = newKey;
            Localize();
        }

        /// <summary>
        /// Updates the display of the associated text object based on current language settings
        /// </summary>
        public void Localize()
        {   
            if(_text == null){
                _text = GetComponent<ITextElement>();
            }
            string val = LocalizationManager.Instance.GetString(key);
            _text.SetText(val);
        }
    }
}