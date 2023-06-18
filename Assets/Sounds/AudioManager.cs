using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudiQ8
{
    public List<AudioClip> Audios;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Istance; 
    [SerializeField] private List<AudiQ8> Audios;
    private void Start()
    {
        Istance = this;
    }

    public AudioClip GetRandomSound(int index)
    {
        var r = Random.value;
        var rindex = (int)(Audios[index].Audios.Count * r);
        return (Audios[index].Audios[rindex]);
    }
}
