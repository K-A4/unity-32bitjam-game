using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    [ContextMenu("RstartSce")]
    public void RestartScene()
    {
        SceneManager.LoadScene("32BitJam", LoadSceneMode.Single);
    }

}
