using CartoonFX;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PlatformGame
{
    public class ChatBalloon : MonoBehaviour
    {
        [SerializeField] UnityEvent OnShowEvent;
        [SerializeField] UnityEvent OnHideEvent;

        [SerializeField] TextMeshProUGUI UI;
        [SerializeField] SpriteRenderer E;
        [SerializeField] SpriteRenderer W;
        [SerializeField] SpriteRenderer S;
        [SerializeField] SpriteRenderer N;
        [SerializeField] SpriteRenderer C;
        [SerializeField] SpriteRenderer NE;
        [SerializeField] SpriteRenderer NW;
        [SerializeField] SpriteRenderer SE;
        [SerializeField] SpriteRenderer SW;
        [SerializeField] SpriteRenderer Arrow;

        [SerializeField] float mHeightUnit = 1;
        float mWidthUnit = 3;
        [Range(1f, 500f)]
        [SerializeField] float interval = 23;
        public bool IsShow { get; private set; }

        public void Show(bool bShow)
        {
            IsShow = bShow;
            UI.gameObject.SetActive(IsShow);
            E.gameObject.SetActive(IsShow);
            W.gameObject.SetActive(IsShow);
            S.gameObject.SetActive(IsShow);
            N.gameObject.SetActive(IsShow);
            C.gameObject.SetActive(IsShow);
            NE.gameObject.SetActive(IsShow);
            NW.gameObject.SetActive(IsShow);
            SE.gameObject.SetActive(IsShow);
            SW.gameObject.SetActive(IsShow);
            Arrow.gameObject.SetActive(IsShow);

            if (!bShow)
            {
                Clear();
                OnHideEvent.Invoke();
            }
            else
            {
                OnShowEvent.Invoke();
            }
        }

        public void SetText(string text)
        {
            UI.text = text;
        }

        public void Clear()
        {
            mWidthUnit = 3;
            PlaceParts();
            UI.text = "";
        }

        void PlaceParts()
        {
            N.transform.localScale = Vector3.up + Vector3.forward + Vector3.right * mWidthUnit;
            N.transform.localPosition = Vector3.up * C.sprite.bounds.extents.y * mHeightUnit;

            S.transform.localScale = N.transform.localScale;
            S.transform.localPosition = -N.transform.localPosition;


            E.transform.localScale = Vector3.right + Vector3.forward + Vector3.up * mHeightUnit;
            E.transform.localPosition = Vector3.right * C.sprite.bounds.extents.x * mWidthUnit;

            W.transform.localScale = E.transform.localScale;
            W.transform.localPosition = -E.transform.localPosition;

            C.transform.localScale = Vector3.right * mWidthUnit + Vector3.up * mHeightUnit + Vector3.forward;
            Arrow.transform.localPosition = S.transform.localPosition;

            NE.transform.localPosition = N.transform.localPosition + E.transform.localPosition;
            NW.transform.localPosition = N.transform.localPosition + W.transform.localPosition;
            SE.transform.localPosition = S.transform.localPosition + E.transform.localPosition;
            SW.transform.localPosition = S.transform.localPosition + W.transform.localPosition;
        }

        void Awake()
        {
            Show(false);
        }

        void Update()
        {
            mWidthUnit = 1 + UI.rectTransform.sizeDelta.x / interval;
            if (IsShow)
            {
                PlaceParts();
            }
        }

    }
}
