using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{

    /// <summary>
    /// Called the first frame the object is targeted.
    /// </summary>
    void OnLookStart(out string name);

    /// <summary>
    /// Called every frame that the object is being looked at.
    /// </summary>
    void OnLook();

    /// <summary>
    /// Called when the object is no longer being targeted.
    /// </summary>
    void OnLookEnd();

    /// <summary>
    /// Called once when the object is targeted and submit is pressed.
    /// </summary>
    void OnLookInteract();
  

}
