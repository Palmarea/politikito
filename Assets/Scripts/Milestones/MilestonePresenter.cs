using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.Milestone
{
    public class MilestonePresenter : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private MilestoneSystem MainSystem;
        [SerializeField] private GameObject MilestoneCanvasUI;
        [SerializeField] private Image MilestoneImageUI;

        [Header("Milestone Images")]
        [SerializeField] private List<Sprite> MilestoneImages;

        [Header("Parameters")]
        [SerializeField] private float NotificationDuration = 3f;

        private Coroutine hideRoutine;

        private void Awake()
        {
            MilestoneCanvasUI.SetActive(false);
        }

        private void ShowMilestone(Milestone milestone)
        {
            MilestoneCanvasUI.SetActive(true);
            //MilestoneImageUI.sprite = MilestoneImages[milestone.level - 1];
            MilestoneImageUI.gameObject.SetActive(true);

            //NotificationTitle.text = milestone.title;
            //NotificationDescription.text = milestone.description;

            // Si ya hay una coroutine corriendo, la cancelamos
            if (hideRoutine != null)
                StopCoroutine(hideRoutine);

            hideRoutine = StartCoroutine(HideAfterDelay());
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(NotificationDuration);
            HideMilestone();
        }

        private void HideMilestone()
        {
            MilestoneCanvasUI.SetActive(false);
            MilestoneImageUI.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            MainSystem.OnMilestoneReached += ShowMilestone;
        }

        private void OnDisable()
        {
            MainSystem.OnMilestoneReached -= ShowMilestone;
        }
    }
}