using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TransformMenu : EditorMenuAbstract
{
    public static TransformMenu instance;
    [SerializeField] Transform mousePlacementContainer;
    [SerializeField] TMP_InputField _posXField;
    [SerializeField] TMP_InputField _posYField;
    [SerializeField] TMP_InputField _posZField;

    [SerializeField] TMP_InputField _rotXField;
    [SerializeField] TMP_InputField _rotYField;
    [SerializeField] TMP_InputField _rotZField;

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
        SetFields(mousePlacementContainer);
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
                    tempValues = mousePlacementContainer.rotation.eulerAngles;
                    tempValues.x = newValue;
                    mousePlacementContainer.rotation = Quaternion.Euler(tempValues);
                    break;
                case "RY":
                    tempValues = mousePlacementContainer.rotation.eulerAngles;
                    tempValues.y = newValue;
                    mousePlacementContainer.rotation = Quaternion.Euler(tempValues);
                    break;
                case "RZ":
                    tempValues = mousePlacementContainer.rotation.eulerAngles;
                    tempValues.z = newValue;
                    mousePlacementContainer.rotation = Quaternion.Euler(tempValues);
                    break;

                case "PX":
                    tempValues = mousePlacementContainer.position;
                    tempValues.x = newValue;
                    mousePlacementContainer.position = tempValues;
                    break;
                case "PY":
                    tempValues = mousePlacementContainer.position;
                    tempValues.y = newValue;
                    mousePlacementContainer.position = tempValues;
                    break;
                case "PZ":
                    tempValues = mousePlacementContainer.position;
                    tempValues.z = newValue;
                    mousePlacementContainer.position = tempValues;
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
        SelectionCounter.text = string.Format("{0} OBJECTS SELECTED", mousePlacementContainer.childCount);
        print(string.Format("{0} OBJECTS SELECTED", mousePlacementContainer.childCount));
    }

    public void CloneObject()
    {
        ObjectPlacer.instance.CloneObject();
        SetFields(mousePlacementContainer);
    }
}
