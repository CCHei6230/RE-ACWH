using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] SceneObject m_nextScene;
    [SerializeField] GameObject[] m_otherObj;

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            //GetComponent<Animator>().SetBool("NextScene", true);
            //FindObjectOfType<SoundManager>().PlayAudioOneShot("SE_Title01");
            SceneManager.LoadScene(m_nextScene);
        }
    }
     void NextScene()
    {
            SceneManager.LoadScene(m_nextScene);
    }
}
