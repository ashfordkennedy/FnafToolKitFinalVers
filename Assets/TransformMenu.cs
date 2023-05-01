using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TransformMenu : EditorMenuAbstract
{
    public static TransformMenu instance;
    [SerializeField] Transform container;
    [SerializeField] TMP_InputField _posXField;
    [SerializeField] TMP_InputField _posYField;
    [SerializeField] TMP_InputField _posZField;

    [SerializeField] TMP_InputField _rotXField;
    [SerializeField] TMP_InputField _rotYField;
    [SerializeField] TMP_InputField _rotZField;

    public float SelectedXposition { get => container.position.x;  set => container.position = new Vector3(value, container.position.y,container.position.z);}
    public float SelectedYposition { get => container.position.y; set => container.position = new Vector3(container.position.x, value, container.position.z); }
    public float SelectedZposition { get => container.position.z; set => container.position = new Vector3(container.position.x, container.position.y, value); }

    [SerializeField] ToggleGroup _snapToggleGroup;
    [SerializeField] TMP_Text SelectionCounter;

    private void Awake()
    {
        instance = this;
    }


    public override void CloseMenu()
    {
        
        base.CloseMenu();

        if (ObjectPlacer.instance.MenuOpen == true)
        {
            ObjectPlacer.instance.CloseMenu();
        }
    }

    public override void OpenMenu()
    {
        base.OpenMenu();
        SetSelectCounter();
        SetFields(container);
    }

    public void DeleteObjects()
    {
        ObjectPlacer.instance.DeleteObjects();
    }

    public void SetFields(Transform transform)
    {
        _posXField.SetTextWithoutNotify("" + transform.position.x);
        _posYField.SetTextWithoutNotify("" + transform.position.y);
        _posZField.SetTextWithoutNotify("" + transform.position.z);

        _rotXField.SetTextWithoutNotify("" + transform.rotation.eulerAngles.x);
        _rotYField.SetTextWithoutNotify("" + transform.rotation.eulerAngles.y);
        _rotZField.SetTextWithoutNotify("" + transform.rotation.eulerAngles.z);
    }

    public void ConfirmInputValue(TMP_InputField input)
    {
        string tag = input.gameObject.name;
        float newValue;
        if(ValidateValue(input.text,out newValue) == true)
        {
            Vector3 tempValues;
            switch (tag)
            {
                case "RX":
                    tempValues = container.rotation.eulerAngles;
                    tempValues.x = newValue;
                    container.rotation = Quaternion.Euler(tempValues);
                    break;
                case "RY":
                    tempValues = container.rotation.eulerAngles;
                    tempValues.y = newValue;
                    container.rotation = Quaternion.Euler(tempValues);
                    break;
                case "RZ":
                    tempValues = container.rotation.eulerAngles;
                    tempValues.z = newValue;
                    container.rotation = Quaternion.Euler(tempValues);
                    break;

                case "PX":
                    tempValues = container.position;
                    tempValues.x = newValue;
                    container.position = tempValues;
                    break;
                case "PY":
                    tempValues = container.position;
                    tempValues.y = newValue;
                    container.position = tempValues;
                    break;
                case "PZ":
                    tempValues = container.position;
                    tempValues.z = newValue;
                    container.position = tempValues;
                    break;
            }

        }
        else
        {

        }


       


    }

    public void SetSnapSize(float value)
    {
        ObjectPlacer.instance.SetSnapValue(value);      
    }

    public void SetRotateSnapSize(float value)
    {
        ObjectPlacer.instance.SetRotationDegrees(value);
    }

    public bool ValidateValue(string value, out float correctedValue)
    {
        correctedValue = 0f;
        if (float.TryParse(value, out correctedValue))
        {
            return true;
        }
        else
        {
            correctedValue = 0f;
            return false;
        }
    }

    public void SetSelectCounter()
    {
        SelectionCounter.text = string.Format("{0} OBJECTS SELECTED", container.childCount);
        print(string.Format("{0} OBJECTS SELECTED", container.childCount));
    }

    public void CloneObject()
    {
        ObjectPlacer.instance.CloneObject();
        SetFields(container);
    }
}
