using System;
using Battle;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{

    public class ButtonLink : IDataLink<Button>
    {
        public event Action<byte[]> OnInteraction;

        public void Link(Button flower, byte[] data)
        {
            flower.onClick.AddListener(() => Convert2Vector4Byte(flower, data));
        }

        void Convert2Vector4Byte(Button button, byte[] data)
        {
            OnInteraction.Invoke(data);
        }
    }

    public class TestFlower : PuzzleInstance<Button>
    {
        public GridLayoutGroup Grid;
        public Button Button;
        public byte Width;
        private Transform _parent;


        protected override void Instantiate(out CubeMap<Button> cubeMap)
        {
            CreateParent();

            var instantiator = new Instantiator<Button>(Button);
            cubeMap = new CubeMap<Button>(Width, instantiator);

            foreach (var index in cubeMap.GetIndex())
            {
                var x = index[0];
                var y = index[1];
                var face = index[2];
                var flower = cubeMap.GetElements(x, y, face);

                flower.transform.SetParent(_parent.GetChild(face));
            }
        }

        protected override void SetDataLink(out IDataLink<Button> dataLink)
        {
            throw new NotImplementedException();
        }

        protected override void SetPresentation(out IPresentation<Button> presentation)
        {
            presentation = new ButtonPresentation();
        }

        private void CreateParent()
        {
            if (_parent = null)
            {
                _parent = new GameObject().transform;
            }

            for (int i = 0; i < _parent.childCount; i++)
            {
                GameObject.Destroy(_parent.GetChild(i));
            }

            for (byte i = 0; i < 6; i++)
            {
                var obj = new GameObject();
                obj.transform.SetParent(_parent);
                var grid = obj.AddComponent<GridLayoutGroup>();
                grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                grid.constraintCount = Width;
                var dir = i switch
                {
                    (byte)Face.front =>
                        Vector3.zero,
                    (byte)Face.back =>
                        Vector3.right * 2,
                    (byte)Face.right =>
                        Vector3.right,
                    (byte)Face.left =>
                        Vector3.left,
                    (byte)Face.top =>
                        Vector3.up,
                    (byte)Face.bottom =>
                        Vector3.down,
                    _ => throw new NotImplementedException($"Out of range : {i}")
                };

                obj.transform.position = dir * Width;
            }
        }
    }

    public class ButtonPresentation : IPresentation<Button>
    {
        public void InstreamData(Button elements, byte data)
        {
            elements.interactable = true;
            switch (data)
            {
                case (byte)Flower.FlowerType.Red:
                    elements.image.color = Color.red;
                    break;
                case (byte)Flower.FlowerType.Green:
                    elements.image.color = Color.green;
                    break;
                default:
                    elements.interactable = false;
                    break;
            }
        }

    }



}
