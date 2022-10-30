using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public virtual void SetMoving(bool move) { }

    public virtual void SetLaying(bool lay, float layTime) { }
}
