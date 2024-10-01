using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class ChatBalloonComponent : MonoBehaviour
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
        [Range(1f, 500f)] [SerializeField] float interval = 23;
        bool IsShow { get; set; }

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

        void Clear()
        {
            mWidthUnit = 3;
            PlaceParts();
            UI.text = "";
        }

        void PlaceParts()
        {
            var transform1 = N.transform;
            transform1.localScale = Vector3.up + Vector3.forward + Vector3.right * mWidthUnit;
            var sprite = C.sprite;
            var localPosition = mHeightUnit * sprite.bounds.extents.y * Vector3.up;
            transform1.localPosition = localPosition;

            var transform2 = S.transform;
            transform2.localScale = transform1.localScale;
            var localPosition1 = -localPosition;
            transform2.localPosition = localPosition1;


            var transform3 = E.transform;
            transform3.localScale = Vector3.right + Vector3.forward + Vector3.up * mHeightUnit;
            var position = mWidthUnit * sprite.bounds.extents.x * Vector3.right;
            transform3.localPosition = position;

            var transform4 = W.transform;
            transform4.localScale = transform3.localScale;
            var position1 = -position;
            transform4.localPosition = position1;

            C.transform.localScale = Vector3.right * mWidthUnit + Vector3.up * mHeightUnit + Vector3.forward;
            Arrow.transform.localPosition = localPosition1;

            NE.transform.localPosition = localPosition + position;
            NW.transform.localPosition = localPosition + position1;
            SE.transform.localPosition = localPosition1 + position;
            SW.transform.localPosition = localPosition1 + position1;
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