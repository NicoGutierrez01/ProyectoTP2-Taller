using UnityEngine;
using UnityEngine.SceneManagement;

public class BackLevelSelector : MonoBehaviour
{
    public void GoToSelector()
    {
        SceneManager.LoadScene(SceneData.nivelselect);
    }
}
