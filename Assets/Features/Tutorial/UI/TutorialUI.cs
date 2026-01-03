using System;
using Common.Utility;
using Features.Tutorial.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Tutorial.UI
{
    public sealed class TutorialUI : MonoBehaviour
    {
        public event Action Closed;

        [SerializeField, Required]
        private TMP_Text topicTitleText, chapterTitleText, descriptionText, chapterCountText;

        [SerializeField, Required]
        private Button leftButton, rightButton, closeButton;

        [SerializeField, Required]
        private CanvasGroup leftButtonGroup, rightButtonGroup;

        [SerializeField, Required]
        private RawImage tutorialImage;

        private TutorialTopicData _currentTopic;
        private int _currentChapterIndex = -1;
        private int _chapterCount;

        private void Awake()
        {
            leftButton.onClick.AddListener(OnLeftButtonClicked);
            rightButton.onClick.AddListener(OnRightButtonClicked);
            closeButton.onClick.AddListener(Close);
        }

        public void Setup(TutorialTopicData topicData)
        {
            _currentTopic = topicData;
            _chapterCount = topicData.Chapters.Count;
            topicTitleText.text = topicData.Title;
            SetChapter(0);
        }

        public void Reset()
        {
            _currentChapterIndex = -1;
            topicTitleText.text = string.Empty;
            _chapterCount = 0;
            _currentTopic = null;
        }

        public void Open()
        {
            if (gameObject.activeSelf)
                return;

            gameObject.SetActive(true);
        }

        public void Close()
        {
            if (!gameObject.activeSelf)
                return;

            gameObject.SetActive(false);

            Closed?.Invoke();
        }

        private void SetChapter(int index)
        {
            _currentChapterIndex = index.Clamp(0, _chapterCount - 1);
            var chapterData = _currentTopic.Chapters[index];
            descriptionText.text = chapterData.Description;
            chapterTitleText.text = chapterData.Title;
            chapterCountText.text = $"{_currentChapterIndex + 1}/{_chapterCount}";
            tutorialImage.texture = chapterData.Image;

            leftButtonGroup.alpha = _currentChapterIndex > 0 ? 1 : 0;
            rightButtonGroup.alpha = _currentChapterIndex < _chapterCount - 1 ? 1 : 0;
        }

        private void OnRightButtonClicked()
        {
            if (_currentChapterIndex >= _chapterCount - 1)
                return;

            SetChapter(_currentChapterIndex + 1);
        }

        private void OnLeftButtonClicked()
        {
            if (_currentChapterIndex <= 0)
                return;

            SetChapter(_currentChapterIndex - 1);
        }
    }
}