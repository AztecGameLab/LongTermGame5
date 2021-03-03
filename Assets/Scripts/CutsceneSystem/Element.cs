using System;
using Unity.Mathematics;
using UnityEngine;

namespace CutsceneSystem
{
    public class Element : MonoBehaviour
    {
        public AnimationCurve positionAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public Vector2 startPosition;
        public Vector2 endPosition;

        public AnimationCurve rotationAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public float startRotation;
        public float endRotation;

        public AnimationCurve scaleAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public Vector2 startScale;
        public Vector2 endScale;

        public void SetStartTransform()
        {
            SetStartPosition();
            SetStartRotation();
            SetStartScale();
        }

        public void SetEndTransform()
        {
            SetEndPosition();
            SetEndRotation();
            SetEndScale();
        }

        public void GoToStartTransform()
        {
            GoToPosition(0);
            GoToRotation(0);
            GoToScale(0);
        }

        public void GoToEndTransform()
        {
            GoToPosition(1);
            GoToRotation(1);
            GoToScale(1);
        }

        public void SetStartPosition() => startPosition = transform.localPosition;
        public void SetEndPosition() => endPosition = transform.localPosition;
        public void GoToPosition(float t) => transform.localPosition =
            Vector2.Lerp(startPosition, endPosition, positionAnimationCurve.Evaluate(t));

        public void SetStartRotation() => startRotation = transform.localEulerAngles.z;
        public void SetEndRotation() => endRotation = transform.localEulerAngles.z;
        public void GoToRotation(float t) =>
            transform.localEulerAngles = new Vector3(0, 0,
                Mathf.Lerp(startRotation, endRotation, rotationAnimationCurve.Evaluate(t)));

        public void SetStartScale() => startScale = transform.localScale;
        public void SetEndScale() => endScale = transform.localScale;
        public void GoToScale(float t) => transform.localScale =
            Vector2.Lerp(startScale, endScale, scaleAnimationCurve.Evaluate(t));


        private void Reset()
        {
            SetStartTransform();
            SetEndTransform();
        }
    }
}