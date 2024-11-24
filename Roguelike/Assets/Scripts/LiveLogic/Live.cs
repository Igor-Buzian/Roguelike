using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Live : MonoBehaviour
{
    /// <summary>
    /// It creates a delay before restarting the scene
    /// </summary>
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
    /// <summary>
    /// It creates a delay before Clearing enemy body
    /// </summary>
    IEnumerator ClearBody()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

}
