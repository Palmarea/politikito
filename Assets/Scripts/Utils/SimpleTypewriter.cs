using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Utils
{
    [RequireComponent(typeof(TMP_Text))]
    public class SimpleTypewriter : MonoBehaviour
    {
        [SerializeField] private float charactersPerSecond = 30f;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip typeSound;

        private TMP_Text textBox;
        private Coroutine typingCoroutine;

        private void Awake()
        {
            textBox = GetComponent<TMP_Text>();
        }

        public void ShowText(string message)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeRoutine(message));
        }

        private IEnumerator TypeRoutine(string message)
        {
            textBox.text = message;
            textBox.maxVisibleCharacters = 0;

            float delay = 1f / charactersPerSecond;

            for (int i = 0; i < message.Length; i++)
            {
                textBox.maxVisibleCharacters++;

                if (typeSound != null && audioSource != null)
                    audioSource.PlayOneShot(typeSound);

                yield return new WaitForSeconds(delay);
            }
        }
    }
}