using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Load the next level using the buildIndex of the active scene
    /// </summary>
    public static void LoadNextLevel()
    {
        int curSceneIndex = SceneManager.GetActiveScene().buildIndex; //Get active scene build index

        string nextLevelPath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(curSceneIndex + 1); //Get the next scene path
        string levelName; //Used to store the level name to load

        if (!string.IsNullOrEmpty(nextLevelPath)) //If the next level path is not null or empty, the next level exist
        {
            //Get the next level name using it's path
            levelName = Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(curSceneIndex + 1));
        }
        else //If there is not a next level, open the scene wich tell the player, there is no more level
        {
            levelName = "NoMoreLevelScene";
        }

        SceneManager.LoadScene(levelName); //Load the given scene using it's name
    }

    /// <summary>
    /// Simmply reload the active scene using it's buildingIndex
    /// </summary>
    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
