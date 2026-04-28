using Game.Systems.Milestone;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.UI;

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
        [SerializeField] private SoundUpdater soundUpdater;

        [Header("Scene")]
        [SerializeField] private string finalSceneName;

        [Header("Parameters")]
        [SerializeField] private float waitTime = 60f;
        [SerializeField] private float audioFadeDuration = 5f;

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
            Character.SetActive(false);
            HUD.SetActive(false);
            DetailObjectsParent.SetActive(false);

            if (soundUpdater != null)
                soundUpdater.FadeOutVolumes(audioFadeDuration);

            asyncLoad = SceneManager.LoadSceneAsync(finalSceneName);
            asyncLoad.allowSceneActivation = false;

            TransitionHandler.OnTransBlackEnded += OnBlackScreen;

            StartCoroutine(SequenceCoroutine());
        }

        private IEnumerator SequenceCoroutine()
        {
            yield return new WaitForSeconds(waitTime);

            yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);

            TransitionHandler.RequestTransitionTB();
        }

        private void OnBlackScreen()
        {
            asyncLoad.allowSceneActivation = true;
        }
    }
}