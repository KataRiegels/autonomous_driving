#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
public class GameControl : MonoBehaviour
{
    public static int episode = 1; // Track the episode count
    public static int succession = 0;

    // Update is called once per frame
    void Start()
    {
        Debug.Log("episode: " + episode);
    }
    void Update()
    {
        // Check if the succession count has reached 10 or more
        if (succession >= 10)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        #if UNITY_EDITOR
        // This will exit Play Mode in the Unity Editor
        EditorApplication.ExitPlaymode();
        #endif
        Debug.Log("Game Over. Succession reached 10.");
    }

    // Method to increment the episode count
    public static void IncrementEpisode()
    {
        episode++;
        Debug.Log("episode: " + episode);
    }
    public static void IncrementSuccession()
    {
        succession++;
        Debug.Log("Succession: " + succession);
    }
    public static void ResetSuccession()
    {
        succession = 0;
        Debug.Log("Succession: " + succession);
    }
}
