using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogTrigger : MonoBehaviour
{
    public Transform CameraPos;
    public float ShowTime;
    public string ShowText;
    public UnityEvent OnDisable;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            ShowDialog();
        }
    }

    private void ShowDialog()
    {
        StartCoroutine(ShowingCoroutine());
    }

    private IEnumerator ShowingCoroutine()
    {
        var camera = Camera.main.transform;
        CameraMovement.IsCameraMove = false;
        var timeElapsed = 0.0f;
        while (true)
        {
            yield return null;
            var t = timeElapsed;
            camera.position = Vector3.Lerp(camera.position, CameraPos.position, t);
            camera.rotation = Quaternion.Lerp(camera.rotation, CameraPos.rotation, t);
            timeElapsed += Time.deltaTime;
            if (t > 1)
            {
                UIText.Instance.SendText(ShowText, ShowTime);
                yield return new WaitForSeconds(ShowTime);
                CameraMovement.IsCameraMove = true;
                OnDisable?.Invoke();
                yield break;
            }
        }
    }
}
