using TMPro;
using UnityEngine;

namespace Skeletom.Essentials.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPTextElement : MonoBehaviour, ITextElement
    {
        private TMP_Text _text;

        public string GetText()
        {
            if(_text == null){
                OnValidate();
            }
            return _text.text;
        }

        public void SetText(string value)
        {
            if(_text == null){
                OnValidate();
            }
            _text.text = value;
        }

        private void OnValidate()
        {
            _text = GetComponent<TMP_Text>();
        }
    }
}
