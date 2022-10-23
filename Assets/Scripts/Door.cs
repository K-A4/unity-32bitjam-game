using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    [ContextMenu("Test")]
    public void Interact()
    {
        anim.SetTrigger("OpenClose");
    }

}
