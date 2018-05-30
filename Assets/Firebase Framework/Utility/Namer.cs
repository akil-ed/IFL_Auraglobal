using UnityEngine;
using UnityEngine.UI;

public class Namer : MonoBehaviour
{
    public string Name;
    string _name;
    Text textField;

    private void OnValidate()
    {
        if (Name != _name)
        {
            gameObject.name = Name;
            if (textField == null)
            {
                if (transform.childCount > 0)
                {
                    textField = transform.GetChild(0).GetComponent<Text>();
                }
                else
                {
                    textField = GetComponent<Text>();
                }
            }
            textField.text = Name;
            _name = Name;
        }
    }
}