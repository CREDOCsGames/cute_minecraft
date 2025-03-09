using Cinema;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Puzzle
{
    public class LevelEntryComponent : MonoBehaviour, IInstance, IPuzzleInstance
    {
        private IMediatorInstance _mediator;
        private CubePuzzleReader _puzzleData;
        private readonly List<Collider> _players = new();
        [SerializeField] private Image _icon;
        [SerializeField] private UnityEvent OnEnterBossStage;
        [SerializeField] private UnityEvent OnEnterBossMovie;
        public DataReader DataReader => new SystemReader();

        private void OnEnable()
        {
            _icon.gameObject.SetActive(0 < _players.Count);
        }
        private void Update()
        {
            if (_icon.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space))
            {
                _icon.gameObject.SetActive(false);
                GetComponent<Collider>().enabled = false;
                Movie.DoPlay(Movie.ENTER_BOSS);
                OnEnterBossMovie.Invoke();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }
            if (_players.Contains(other))
            {
                return;
            }
            _players.Add(other);
            if (_players.Count == 1)
            {
                _icon.gameObject.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            _players.Remove(other);
            if (_players.Count == 0)
            {
                _icon.gameObject.SetActive(false);
            }
        }
        public void InstreamData(byte[] data)
        {
            if (SystemReader.IsClearFace(data))
            {
                _puzzleData.OnRotated -= SendEntryMessage;
            }
            if (SystemReader.CLEAR_TOP_FACE.Equals(data))
            {
                _puzzleData.OnRotated += SendEntryMessage;
            }
            else
            if (SystemReader.CLEAR_LEFT_FACE.Equals(data))
            {
                _puzzleData.OnRotated += SendEntryMessage;
            }
            else
            if (SystemReader.CLEAR_FRONT_FACE.Equals(data))
            {
                _puzzleData.OnRotated += SendEntryMessage;
            }
            else
            if (SystemReader.CLEAR_RIGHT_FACE.Equals(data))
            {
                _puzzleData.OnRotated += SendEntryMessage;
            }
            else
            if (SystemReader.CLEAR_BACK_FACE.Equals(data))
            {
                Movie.ENTER_BOSS.OnEnd += SendEntryMessage;
                Movie.ENTER_BOSS.OnEnd += OnEnterBossStage.Invoke;
                Movie.ENTER_BOSS.OnEnd += () => Movie.ENTER_BOSS.OnEnd -= SendEntryMessage;
                Movie.ENTER_BOSS.OnEnd += () => Movie.ENTER_BOSS.OnEnd -= OnEnterBossStage.Invoke;
            }
            else
            if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                CoroutineRunner.instance.StartCoroutine(PlayEnding());
            }
        }
        public void InitInstance(CubePuzzleReader puzzleData)
        {
            _puzzleData = puzzleData;
        }
        public void SetMediator(IMediatorInstance mediator)
        {
            _mediator = mediator;
        }
        private void SendEntryMessage(Face preFace,Face playFace)
        {
            _mediator?.InstreamDataInstance<SystemReader>(SystemReader.READY_PLAYER);
        }
        private void SendEntryMessage()
        {
            _mediator?.InstreamDataInstance<SystemReader>(SystemReader.READY_PLAYER);
        }
        private IEnumerator PlayEnding()
        {
            yield return new WaitForSeconds(2f);
            Movie.DoPlay(Movie.EXIT_BOSS);
        }
    }

}
