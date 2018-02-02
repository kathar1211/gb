using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonPress : MonoBehaviour
{
    public void ButtonClicked(string levelName)
    {
        print("hi");
        SceneManager.LoadScene("EasyLevel");
    }
}
