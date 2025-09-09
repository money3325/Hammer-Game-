
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CustomizeableCharacter : MonoBehaviour
{

    [SerializeField]
    private CustomizedCharacter _character;
    
    public void StoreCustomizationInformation()
    {
        var elements = GetComponentsInChildren<CustomizableElement>();
        _character.Data.Clear();
        foreach (var element in elements)
        {
            _character.Data.Add(element.GetCustomizationDate());
        }
        SceneManager.LoadScene("SampleScene");
    }
}