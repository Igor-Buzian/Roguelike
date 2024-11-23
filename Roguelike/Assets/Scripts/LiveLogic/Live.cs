using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Live : MonoBehaviour
{
    public void PlayerDeath()
    {
        StartCoroutine(LoadCurrentScene());
    }
    public void EnemyDeath()
    {
        StartCoroutine(ClearBody());
    }
    IEnumerator LoadCurrentScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator ClearBody()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

}
