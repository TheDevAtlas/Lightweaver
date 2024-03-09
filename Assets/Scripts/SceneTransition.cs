using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int targetLevel;
    public float sceneChangeTime = 3f;
    public Animator animator;

    public void ChangeLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        animator.SetTrigger("EndScene");

        yield return new WaitForSeconds(sceneChangeTime);

        SceneManager.LoadScene(targetLevel);
    }
}
