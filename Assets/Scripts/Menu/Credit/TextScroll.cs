using System;
using System.Collections.Generic;
using Event;
using UnityEngine;

namespace Menu.Credit
{
    public class TextScroll : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed;
        [SerializeField] private Transform top;
        [SerializeField] private Transform center;
        [SerializeField] private Transform bottom;
        [SerializeField] private Transform[] text;
        private List<Vector2> _textPositions;
        private float _cd;

        private void OnEnable()
        {
            if(EventManager.Instance)
                EventManager.Instance.RegisterEventHandlersFromAttributes(this);
        }

        private void OnDisable()
        {
            if(EventManager.Instance)
                EventManager.Instance.UnregisterAllEventsForObject(this);
        }
        
        private void Start()
        {
            _textPositions = new List<Vector2>();
            foreach (var t in text)
            {
                _textPositions.Add(t.localPosition);
            }
            gameObject.SetActive(false);
        }

        [EventSubscribe("OpenCredit")]
        public object ResetPosition(string anyway)
        {
            for (var i = 0; i < text.Length; i++)
            {
                text[i].localPosition = _textPositions[i];
            }
            return null;
        }

        public float ScrollSpeed
        {
            get => scrollSpeed;
            set => scrollSpeed = value;
        }

        private void Update()
        {
            foreach (var variable in text)
            {
                var temp = variable.localPosition;
                if (!(variable.gameObject.name.Equals("3Q") && variable.localPosition.y > center.localPosition.y))
                {
                    temp.y += scrollSpeed * Time.deltaTime;
                }
                if (temp.y > top.localPosition.y || temp.y < bottom.localPosition.y)
                {
                    variable.gameObject.SetActive(false);
                }
                else
                {
                    variable.gameObject.SetActive(true);
                }

                variable.localPosition = temp;
            }
        }
    }
}