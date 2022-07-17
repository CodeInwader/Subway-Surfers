using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField InputField;

    public void LoadScene()
    {
       
        string text = InputField.GetComponent<TMP_InputField>().text;
        GlobalLeadboard.currentName = text;

        SceneManager.LoadScene("Main");
    }
}
