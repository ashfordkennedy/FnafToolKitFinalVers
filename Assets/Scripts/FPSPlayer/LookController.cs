using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif
/// <summary>
/// Created by - James Jordan 25-11-2021
/// Functionality for player mouse/stick look controls.
/// Based on the unity standard assets system (but nicer)
/// </summary>

[System.Serializable]
public class LookController
{
    public float XSensitivity = 5f;
    public float YSensitivity = 5f;

    public float MinimumXRot = -90f;
    public float MaximumX = 90f;

    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;

    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private bool m_cursorIsLocked = true;

    [Header("Interaction System")]
    public bool interactionEnabled = true;
    public float lookLength = 5f;
    public World_Interactable lookTarget = null;
    public Transform lookTargetTransform = null;
    public bool displayTextPrompt = true;
    public TMP_Text interactionTextPrompt;


    public void Initialise(Transform target, Transform camera)
    {
        m_CharacterTargetRot = target.localRotation;
        m_CameraTargetRot = camera.localRotation;
        
    }


    public void LookRotation(Transform target, Transform camera, Vector2 input)
    {
        input.x = input.x * XSensitivity;
        input.y = input.y * YSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, input.x, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-input.y, 0f, 0f);

        m_CameraTargetRot = ClampRotationXAxis(m_CameraTargetRot);

        target.localRotation = Quaternion.Slerp(target.localRotation, m_CharacterTargetRot, smoothTime * Time.fixedDeltaTime);
        camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot, smoothTime * Time.fixedDeltaTime);
 

       // target.localRotation = m_CharacterTargetRot;
       // camera.localRotation = m_CameraTargetRot;
    }

    private Quaternion ClampRotationXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumXRot, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }


    public void InteractionRaycast(Camera camera)
    {
        if (interactionEnabled) {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit, lookLength))
            {
                if (hit.collider == null)
                {
                    interactionTextPrompt.text = "";
                    lookTarget = null;
                    //  lookTarget

                }
                else
                {
                    // if target has changed
                    if (lookTargetTransform != hit.collider.transform)
                    {
                        if (lookTarget != null) {
                            lookTarget.OnLookEnd();
                        }
                        // something is hit but isnt an interactable
                        else
                        {
                            interactionTextPrompt.text = "";
                            lookTarget = null;
                            lookTargetTransform = null;
                        }


                        // get new target if possible
                        if (hit.collider.gameObject.TryGetComponent<World_Interactable>(out lookTarget))
                        {
                            lookTargetTransform = hit.collider.transform;
                            // trigger OnLook() on the object and receive the name.
                            lookTarget.OnLookStart(out string interactableName);
                            if (displayTextPrompt == true)
                            {
                                interactionTextPrompt.text = interactableName;
                            }
                        }
                    }
                    else if (lookTarget != null)
                    {
                        // same object Try to run OnLook()
                        lookTarget.OnLook();
                    }


                }

            }
            else
            {
                if (lookTarget != null)
                {
                    lookTarget.OnLookEnd();
                }
                interactionTextPrompt.text = "";
                lookTarget = null;
                lookTargetTransform = null;

            }


            //debug look thing. remove when you dont want to see the cube anymore
           // lookbox.position = hit.point;

        }
    }
}
