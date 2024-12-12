using Controller;
using Movement;
using Puzzle;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace NW
{
    public class CubePresentation : IPresentation
    {
        private readonly MovementComponent _movement;
        public CubePresentation(MovementComponent movementComponent)
        {
            _movement = movementComponent;
        }
        public void InstreamData(byte[] data)
        {
            if (SystemReader.CLEAR_RIGHT_FACE.Equals(data))
            {
                Command("MovementAction/RotateLeft");
            }
            else if (SystemReader.CLEAR_LEFT_FACE.Equals(data))
            {
                Command("MovementAction/RotateLeft");
            }
            else if (SystemReader.CLEAR_FRONT_FACE.Equals(data))
            {
                Command("MovementAction/RotateLeft");
            }
            else if (SystemReader.CLEAR_BACK_FACE.Equals(data))
            {
                Command("MovementAction/RotateBackward");
            }
            else if (SystemReader.CLEAR_TOP_FACE.Equals(data))
            {
                Command("MovementAction/RotateBackward");
            }
            else if (SystemReader.CLEAR_BOTTOM_FACE.Equals(data))
            {
                Command("MovementAction/RotateLeft");
            }

        }

        private void Command(string path)
        {
            Util.CoroutineRunner.Instance.StartCoroutine(Action(path));
        }

        private IEnumerator Action(string path)
        {
            var character = GameObject.FindAnyObjectByType<CharacterComponent>()._character;
            while (character != null && character.State != "Idle")
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            var action = Resources.Load<MovementAction>(path);
            character?.ChangeState(new JumpState());
            if (action != null)
            {
                _movement.PlayMovement(action);
            }
        }

    }
    [CreateAssetMenu(menuName = "Custom/CubeInstance")]
    public class CubeInstance : PuzzleInstance, IDataLink
    {
        private MovementComponent _movement;
        public IMediatorInstance Mediator { get; set; }
        public event Action<byte[]> OnInteraction;
        protected override void Instantiate(CubeMapReader puzzleData)
        {
            if (!puzzleData.BaseTransform.TryGetComponent(out _movement))
            {
                _movement = puzzleData.BaseTransform.AddComponent<MovementComponent>();
                puzzleData.BaseTransform.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        protected override void SetDataLink(out IDataLink dataLink)
        {
            dataLink = this;
        }

        protected override void SetDataReader(out DataReader reader)
        {
            reader = new SystemReader();
        }

        protected override void SetPresentation(out IPresentation presentation)
        {
            presentation = new CubePresentation(_movement);
        }
    }

}
