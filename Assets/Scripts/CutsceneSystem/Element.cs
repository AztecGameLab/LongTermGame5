using System.Collections;
using UnityEngine;

namespace CutsceneSystem
{
    public class Element : MonoBehaviour
    {
        public float elementStartTime = 0;
        public float elementDuration = 2;

        public AnimationCurve positionAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public Vector2 startPosition;
        public Vector2 endPosition;

        public AnimationCurve rotationAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public float startRotation;
        public float endRotation;

        public AnimationCurve scaleAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        public Vector2 startScale;
        public Vector2 endScale;

        private void Reset()
        {
            SetStartTransform();
            SetEndTransform();
        }

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

        public void GoToTransform(float t)
        {
            GoToPosition(t);
            GoToRotation(t);
            GoToScale(t);
        }

        public void SetStartPosition() => startPosition = transform.localPosition;
        public void SetEndPosition() => endPosition = transform.localPosition;

        public void GoToPosition(float t) => transform.localPosition =
            Vector2.Lerp(startPosition, endPosition, positionAnimationCurve.Evaluate(t));

        public void SetStartRotation() => startRotation = transform.localEulerAngles.z;
        public void SetEndRotation() => endRotation = transform.localEulerAngles.z;

        public void GoToRotation(float t) =>
            transform.localRotation =
                Quaternion.Lerp(Quaternion.Euler(new Vector3(0, 0, startRotation)),
                    Quaternion.Euler(new Vector3(0, 0, endRotation)), t);

        public void SetStartScale() => startScale = transform.localScale;
        public void SetEndScale() => endScale = transform.localScale;

        public void GoToScale(float t) => transform.localScale =
            Vector2.Lerp(startScale, endScale, scaleAnimationCurve.Evaluate(t));


        public IEnumerator C_Animate()
        {
            GoToTransform(0);
            float currentTime = 0;

            yield return new WaitForSeconds(elementStartTime);
            while (currentTime <= elementDuration)
            {
                GoToTransform(currentTime / elementDuration);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}