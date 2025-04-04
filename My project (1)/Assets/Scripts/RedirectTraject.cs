using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectTraject : MonoBehaviour
{
     void Start()
    {
        string plan = PlayerPrefs.GetString("plan");

        Debug.Log(plan);
        if(plan == "A")
        {
            SceneManager.LoadScene("behandeltrajectA");
        }
        else
        {
            SceneManager.LoadScene("behandeltrajectB");
        }

    }

     
}
