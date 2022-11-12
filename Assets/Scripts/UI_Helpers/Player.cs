using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                gameObject.transform.position -= Vector3.one * Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                gameObject.transform.position += Vector3.one * Time.deltaTime;
            }
        }
    }
}
