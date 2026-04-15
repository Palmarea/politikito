using Game.Systems.Milestone;
using Game.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Systems.Ending
{
    public class FinalSequenceTransition : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private MilestonePresenter MilestonePresenter;
        [SerializeField] private TransitionHandler TransitionHandler;

        [SerializeField] private GameObject Character;
        [SerializeField] private GameObject HUD;
        [SerializeField] private GameObject DetailObjectsParent;

        [Header("Scene")]
        [SerializeField] private string finalSceneName;

        [Header("Parameters")]
        [SerializeField] private float waitTime = 60f;

        [Header("Debug")]
        [SerializeField] private bool triggerFromInspector;

        private AsyncOperation asyncLoad;

        private void Start()
        {
            MilestonePresenter.OnLastMilestoneShown += StartSequence;

            if (triggerFromInspector)
                StartSequence();
        }

        private void OnDestroy()
        {
            MilestonePresenter.OnLastMilestoneShown -= StartSequence;
        }

        private void StartSequence()
        {
            // 1. Desactivar elementos
            Character.SetActive(false);
            HUD.SetActive(false);
            DetailObjectsParent.SetActive(false);

            // 2. Comenzar carga async
            asyncLoad = SceneManager.LoadSceneAsync(finalSceneName);
            asyncLoad.allowSceneActivation = false;

            // 3. Escuchar fin de transición a negro
            TransitionHandler.OnTransBlackEnded += OnBlackScreen;

            // 4. Iniciar secuencia
            StartCoroutine(SequenceCoroutine());
        }

        private IEnumerator SequenceCoroutine()
        {
            // Esperar el tiempo de exploración
            yield return new WaitForSeconds(waitTime);

            // Esperar a que la escena esté cargada al 90%
            yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);

            // Pedir transición a negro SOLO cuando ya está lista
            TransitionHandler.RequestTransitionTB();
        }

        private void OnBlackScreen()
        {
            // Activar escena cuando ya está completamente negro
            asyncLoad.allowSceneActivation = true;
        }
    }
}