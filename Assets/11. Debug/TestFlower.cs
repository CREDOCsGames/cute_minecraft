using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Random = UnityEngine.Random;

public class TestFlower : MonoBehaviour
{
    static readonly List<TestFlower> mInstances = new(6);
    public MatrixBool Puzzle;
    public Button Button;
    public GridLayoutGroup Grid;
    public TextMeshProUGUI UI;
    public int index = 0;
    Action<Button> ButtonEvent;

    void Cross(Button button)
    {
        if (!IsActive(button))
        {
            return;
        }
        var data = Resources.Load<FlowerPuzzleData>("Flower Puzzle Data");
        mInstances.ForEach(x =>
        {
            if (data.Links[index].Contains(x.index))
            {
                x.Flower(int.Parse(button.name));
            }
        });

        CheckClear();
    }
    void Dot(Button button)
    {
        if (!IsActive(button))
        {
            return;
        }
        if (!mInstances.Any(x => x.index == index))
        {
            return;
        }
        ReverseColor(int.Parse(button.name));
        CheckClear();
    }
    void Create(Button button)
    {
        if (IsActive(button))
        {
            return;
        }

        button.GetComponent<Image>().color = Random.Range(0, 2) % 2 == 0 ? Color.red : Color.green;
    }
    void Flower(int i)
    {
        if (!IsActive(transform.GetChild(i).GetComponent<Button>())) return;

        ReverseColor(i);

        if (i % Puzzle.Column != 0)
        {
            ReverseColor(i - 1);
        }

        if (i % Puzzle.Column != Puzzle.Column - 1)
        {
            ReverseColor(i + 1);
        }

        if (i / Puzzle.Column != 0)
        {
            ReverseColor(i - Puzzle.Column);
        }

        if (i / Puzzle.Column < Puzzle.Row - 1)
        {
            ReverseColor(i + Puzzle.Column);
        }
    }
    void ReverseColor(int i)
    {
        var child = transform.GetChild(i);
        if (IsActive(child.GetComponent<Button>()))
        {
            var image = child.GetComponent<Image>();
            image.color = image.color == Color.red ? Color.green : Color.red;
        }
    }
    bool IsActive(Button button)
    {
        var color = button.GetComponent<Image>().color;
        return color == Color.red || Color.green == color;
    }
    void CheckClear()
    {
        if (!IsEnd())
        {
            return;
        }

        var data = Resources.Load<FlowerPuzzleData>("Flower Puzzle Data");
        if (data.UseClearThanLock.Equals("True"))
        {
            LockIfClear();
        }

        if (data.UseClearConditionThanBaseFace.Equals("True"))
        {
            ClearIfBaseFaceEnd();
        }

        if (data.UseLinkedClear.Equals("True"))
        {
            ClearIfLinkedFaceEnd();
        }

        if (mInstances.All(x => x.IsEnd()))
        {
            LockAll();
        }
    }
    void LockIfClear()
    {
        if (IsEnd())
        {
            foreach (var flower in GetComponentsInChildren<Button>())
            {
                flower.interactable = false;
            }
        }
    }
    void ClearIfBaseFaceEnd()
    {
        var baseFace = mInstances.Where(x => x.index == 0).First();

        if (baseFace.IsEnd())
        {
            LockAll();
        }
    }
    void ClearIfLinkedFaceEnd()
    {
        var data = Resources.Load<FlowerPuzzleData>("Flower Puzzle Data");
        var linked = mInstances.Where(x => data.Links[index].Any(y => y == x.index));
        if (linked.All(x => x.IsEnd()))
        {
            LockIfClear();
        }

    }
    void LockAll()
    {
        mInstances.ForEach(puzzle => puzzle.GetComponentsInChildren<Button>().ToList().ForEach(button => button.interactable = false));
    }
    bool IsEnd()
    {
        var flowers = GetComponentsInChildren<Image>().Where(x => x.color == Color.red || x.color == Color.green);
        return flowers.All(x => x.color == flowers.First().color);
    }
    void Awake()
    {
        Grid.constraintCount = Puzzle.Column;
        for (int i = 0; i < Puzzle.Column * Puzzle.Row; i++)
        {
            var button = Instantiate(Button, transform);
            button.name = i.ToString();
            button.onClick.AddListener(() => ButtonEvent.Invoke(button));
            if (!Puzzle.Matrix[i / Puzzle.Column].List[i % Puzzle.Column] || Random.Range(0, 4) == 0) continue;
            button.GetComponent<Image>().color = Random.Range(0, 2) % 2 == 0 ? Color.red : Color.green;
        }
        ButtonEvent = Cross;
        UI.text = ButtonEvent.Method.Name;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ButtonEvent = ButtonEvent == Cross ? Dot : ButtonEvent == Dot ? Create : Cross;
            UI.text = ButtonEvent.Method.Name;
        }
    }
    void OnEnable()
    {
        mInstances.Add(this);
    }
    void OnDisable()
    {
        mInstances.Remove(this);
    }
}
