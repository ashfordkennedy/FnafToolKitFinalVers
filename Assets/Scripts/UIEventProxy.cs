using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventProxy : MonoBehaviour
{
    public void ToggleObjectActive(GameObject target)
    {
        target.SetActive(!target.activeInHierarchy);
    }
}
