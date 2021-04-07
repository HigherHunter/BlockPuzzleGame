using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        private AudioSource _mainAudioSource;
        [SerializeField] private AudioClip startGameAudioClip;
        [SerializeField] private AudioClip buttonPressAudioClip;
        [SerializeField] private AudioClip blockPlaceAudioClip;
        [SerializeField] private AudioClip blockMissAudioClip;
        [SerializeField] private AudioClip blocksSpawnAudioClip;
        [SerializeField] private AudioClip newHighScoreAudioClip;
        [SerializeField] private AudioClip gameOverAudioClip;
        [SerializeField] private AudioClip gameOverHighScoreAudioClip;
        [SerializeField] private AudioClip[] oneClearSuccessAudioClips;
        [SerializeField] private AudioClip[] twoAndThreeClearSuccessAudioClips;
        [SerializeField] private AudioClip[] fourClearSuccessAudioClips;
        [SerializeField] private AudioClip fiveAndSixClearSuccessAudioClip;

        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _mainAudioSource = GetComponent<AudioSource>();
        }

        public void PlayStartSound() => _mainAudioSource.PlayOneShot(startGameAudioClip);

        public void PlayButtonPressSound() => _mainAudioSource.PlayOneShot(buttonPressAudioClip);

        public void PlayPlaceSound() => _mainAudioSource.PlayOneShot(blockPlaceAudioClip);

        public void PlayMissSound() => _mainAudioSource.PlayOneShot(blockMissAudioClip);

        public void PlayBlocksSpawnSound() => _mainAudioSource.PlayOneShot(blocksSpawnAudioClip);

        public void PlayNewHighScoreSound() => _mainAudioSource.PlayOneShot(newHighScoreAudioClip);

        public void PlayGameOverSound() => _mainAudioSource.PlayOneShot(gameOverAudioClip);

        public void PlayHighScoreGameOverSound() => _mainAudioSource.PlayOneShot(gameOverHighScoreAudioClip);

        public void PlayOneClearRandomSuccessSound()
        {
            int i = Random.Range(0, oneClearSuccessAudioClips.Length);

            if (oneClearSuccessAudioClips[i])
                _mainAudioSource.PlayOneShot(oneClearSuccessAudioClips[i]);
        }

        public void PlayTwoOrThreeClearRandomSuccessSound()
        {
            int i = Random.Range(0, twoAndThreeClearSuccessAudioClips.Length);

            if (twoAndThreeClearSuccessAudioClips[i])
                _mainAudioSource.PlayOneShot(twoAndThreeClearSuccessAudioClips[i]);
        }

        public void PlayFourClearRandomSuccessSound()
        {
            int i = Random.Range(0, fourClearSuccessAudioClips.Length);

            if (fourClearSuccessAudioClips[i])
                _mainAudioSource.PlayOneShot(fourClearSuccessAudioClips[i]);
        }

        public void PlayFiveOrSixClearRandomSuccessSound() => _mainAudioSource.PlayOneShot(fiveAndSixClearSuccessAudioClip);

        public void Mute()
        {
            _mainAudioSource.mute = !_mainAudioSource.mute;
        }

        public bool IsPlaying()
        {
            return _mainAudioSource.isPlaying;
        }
    }
}