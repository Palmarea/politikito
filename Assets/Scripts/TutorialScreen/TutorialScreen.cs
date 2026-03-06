using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialScreen : MonoBehaviour
{
    public TMP_Text homeworkTitleLabel;
    public TMP_Text homeworkDescriptionLabel;
    
    public List<Toggle> tutorialStepToggles;
    public List<TMP_Text> tutorialStepLabels;
    
    public Button nextButton;

    private void Start()
    {
        StartCoroutine(StartSequence());
        nextButton.onClick.AddListener(HandleOnNextButtonClick);
    }

    private IEnumerator StartSequence()
    {
        homeworkTitleLabel.gameObject.SetActive(false);
        homeworkDescriptionLabel.gameObject.SetActive(false);
        tutorialStepLabels[0].gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        homeworkTitleLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        homeworkDescriptionLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        tutorialStepToggles[0].gameObject.SetActive(true);
        tutorialStepToggles[0].transform.localScale = Vector3.zero;
        tutorialStepToggles[0].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        tutorialStepLabels[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        TweenNextButton();
    }

    private void TweenNextButton()
    {
        nextButton.gameObject.SetActive(true);
        RectTransform nextButtonRectTransform = nextButton.GetComponent<RectTransform>();
        Vector2 originalPosition = nextButtonRectTransform.anchoredPosition;
        
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.5f);
        mySequence.Append(nextButtonRectTransform.DOPunchAnchorPos(Vector2.down * 25f, 1f, 1, 0.1f));
        mySequence.AppendInterval(0.5f);
        mySequence.SetLoops(-1, LoopType.Restart).Play();
    }
    
    private void HandleOnNextButtonClick()
    {
    }
}
