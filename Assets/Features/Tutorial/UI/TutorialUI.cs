using System;
using Features.Tutorial.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Tutorial.UI
{
    public sealed class TutorialUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text topicTitleText, chapterTitleText, descriptionText, chapterCountText;

        [SerializeField]
        private Button leftButton, rightButton;

        [SerializeField]
        private Image tutorialImage;

        private TutorialTopicData _currentTopic;
        private int _currentChapterIndex = -1;
        private int _chapterCount;

        private void Awake()
        {
            leftButton.onClick.AddListener(OnLeftButtonClicked);
            rightButton.onClick.AddListener(OnRightButtonClicked);
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
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void SetChapter(int index)
        {
            _currentChapterIndex = index;
            var chapterData = _currentTopic.Chapters[index];
            descriptionText.text = chapterData.Description;
            chapterTitleText.text = chapterData.Title;
            chapterCountText.text = $"{_currentChapterIndex}/{_chapterCount}";
            leftButton.enabled = _currentChapterIndex > 0;
            rightButton.enabled = _currentChapterIndex < _chapterCount;
        }

        private void OnRightButtonClicked()
        {
            if (_currentChapterIndex >= _chapterCount - 1)
                return;

            SetChapter(_chapterCount + 1);
        }

        private void OnLeftButtonClicked()
        {
            if (_currentChapterIndex <= 0)
                return;

            SetChapter(_chapterCount - 1);
        }
    }
}