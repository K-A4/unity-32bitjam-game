using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private float distance;
    [SerializeField] private float openSpeed;
    private bool opened;
    private bool opening;

    [ContextMenu("Test")]
    public void Interact()
    {
        if (!opening)
        {
            StartCoroutine(Coroutine());
        }
    }

    public IEnumerator Coroutine()
    {
        opened = !opened;
        opening = true;
        var startPos = transform.position;
        var right = opened ? transform.right : - transform.right;
        var endPos = startPos + (right * distance);
        var timeElapsed = 0.0f;

        while (true)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed);
            timeElapsed += (openSpeed / distance) * Time.deltaTime;

            if (timeElapsed > 1)
            {
                opening = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
