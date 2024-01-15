using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    public string nextSceneName = "YourNextSceneName";

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter: " + other.gameObject.name); // Add this line for debugging

        if (other.CompareTag("Enemy"))
        {
            // Switch scenes or perform any other action
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
