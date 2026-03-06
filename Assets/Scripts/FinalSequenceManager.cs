using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Systems.Milestone;
using Game.Character;
using Game.Character.StateMachine.States;

public class FinalSequenceController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private MilestonePresenter milestonePresenter;
    [SerializeField] private TamaCharacterController character;
    [SerializeField] private TamaCharacterAnimation characterAnimator;
    [SerializeField] private GameObject TransitionUI;

    [Header("Final Visuals")]
    [SerializeField] private AnimatorOverrideController finalAnimatorOverride;
    [SerializeField] private Transform finalPosition;

    [Header("Objects To Hide")]
    [SerializeField] private List<GameObject> objectsToHide;

    [Header("Clickable")]
    [SerializeField] private GameObject fullScreenCollider;

    [Header("Closing Animation")]
    [SerializeField] private Animator closingAnimator;
    [SerializeField] private string closingTrigger = "Close";

    [Header("Scene")]
    [SerializeField] private string finalSceneName;

    [Header("Parameters")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float textDuration = 5f;

    [Header("Debug")]
    [SerializeField] private bool triggerFromInspector;

    private int milestoneCounter = 0;
    private bool sequenceStarted = false;
    private bool clickTriggered = false;

    private AsyncOperation sceneLoading;

    private void OnEnable()
    {
        milestonePresenter.OnMilestoneShown += OnMilestone;
    }

    private void OnDisable()
    {
        milestonePresenter.OnMilestoneShown -= OnMilestone;
    }

    private void OnMilestone(int level)
    {
        if (sequenceStarted) return;

        milestoneCounter++;

        if (milestoneCounter >= 4)
        {
            StartCoroutine(StartFinalSequence());
        }
    }

    private void Update()
    {
        // Debug trigger desde inspector
        if (triggerFromInspector && !sequenceStarted)
        {
            //triggerFromInspector = false;
            StartCoroutine(StartFinalSequence());
        }
    }

    private IEnumerator StartFinalSequence()
    {
        sequenceStarted = true;

        // Cargar escena final async
        sceneLoading = SceneManager.LoadSceneAsync(finalSceneName);
        sceneLoading.allowSceneActivation = false;

        // Ocultar objetos
        foreach (var obj in objectsToHide)
            obj.SetActive(false);

        // Cambiar animación del personaje
        characterAnimator.RequestAnimatorOverride(finalAnimatorOverride);

        // Mover personaje
        yield return StartCoroutine(MoveCharacter());

        // Activar collider clickable
        fullScreenCollider.SetActive(true);
    }

    private IEnumerator MoveCharacter()
    {  
        while (Vector3.Distance(character.transform.position, finalPosition.position) > 0.05f)
        {
            character.transform.position = Vector3.MoveTowards(
                character.transform.position,
                finalPosition.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        character.ChangeState(new FrozenState(character));
    }

    public void OnScreenClicked()
    {
        if (clickTriggered) return;

        clickTriggered = true;
        fullScreenCollider.SetActive(false);
        TransitionUI.SetActive(true);
        closingAnimator.SetTrigger(closingTrigger);
    }

    // Llamado con Animation Event
    public void OnClosingAnimationFinished()
    {
        sceneLoading.allowSceneActivation = true;
    }
}