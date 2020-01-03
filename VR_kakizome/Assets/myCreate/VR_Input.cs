using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Input : MonoBehaviour
{
    [SerializeField]
    private GameObject sphere;

    private OVRInput.Controller controller;

    void Start()
    {
        // OVRControllerHelperから左右どっちかを取得する
        controller = GetComponent<OVRControllerHelper>().m_controller;
    }

    void Update()
    {
    if (OVRInput.GetDown(OVRInput.Button.One, controller)) {            
            Instantiate(sphere, transform.position, transform.rotation);
        }
    }
}
