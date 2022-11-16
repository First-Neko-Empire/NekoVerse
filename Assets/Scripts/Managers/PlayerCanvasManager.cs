using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StartCoroutine(HideCursor());
        }
    }

    private IEnumerator HideCursor()
    {
        yield return new WaitForEndOfFrame();

        Cursor.lockState = CursorLockMode.Locked;
    }
}
