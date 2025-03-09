using System;
using UnityEngine;
using UnityEngine.Events;

namespace Puzzle
{
    public class CubePuzzleEvent : ICore, IInstance
    {
        private Face _playingFace;
        public Action OnReady;
        public Action<Face> OnClearLevel;
        public Action<Face> OnStartLevel;
        public Action OnClearStage;
        public Action<Face, Face> OnRotated;
        public DataReader DataReader => SystemReader.Instance;
        private readonly Transform _puzzleTransform;
        public CubePuzzleEvent(Transform puzzleTransform, UnityEvent<Face> onStartLevel)
        {
            _puzzleTransform = puzzleTransform;
            onStartLevel.AddListener((f) => OnStartLevel?.Invoke(f));
        }
        private void OnRotateCube(byte[] data)
        {
            if (!data.Equals(SystemReader.ROTATE_CUBE))
            {
                return;
            }

            const float threshold = 0.98f;
            var up = _puzzleTransform.up;
            var right = _puzzleTransform.right;
            var forward = _puzzleTransform.forward;

            var playFace = Vector3.Dot(up, Vector3.up) > threshold ? Face.top :
                           Vector3.Dot(-up, Vector3.up) > threshold ? Face.bottom :
                           Vector3.Dot(right, Vector3.up) > threshold ? Face.right :
                           Vector3.Dot(-right, Vector3.up) > threshold ? Face.left :
                           Vector3.Dot(forward, Vector3.up) > threshold ? Face.front :
                           Vector3.Dot(-forward, Vector3.up) > threshold ? Face.back : Face.top;

            OnRotated.Invoke(_playingFace, playFace);
            _playingFace = playFace;
        }
        private void UpdateReady(byte[] data)
        {
            if (!data.Equals(SystemReader.READY_PLAYER))
            {
                return;
            }
            OnReady.Invoke();
        }


        public void InstreamData(byte[] data)
        {
            OnRotateCube(data);
            UpdateClearLevel(data);
            UpdateClearStage(data);
            UpdateReady(data);
        }
        private void UpdateClearLevel(byte[] data)
        {
            if (SystemReader.IsClearFace(data) && !SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                OnClearLevel?.Invoke((Face)(data[0] - 1));
            }
        }
        private void UpdateClearStage(byte[] data)
        {
            if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                OnClearStage?.Invoke();
            }
        }

        public void SetMediator(IMediatorCore mediator)
        {
        }
        public void SetMediator(IMediatorInstance mediator)
        {
        }
    }

}