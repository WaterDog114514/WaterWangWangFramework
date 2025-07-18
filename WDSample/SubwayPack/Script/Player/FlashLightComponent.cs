//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FlashLightComponent : MonoBehaviour
//{

//    public GameObject FlashLight;
//    public AudioSource audioSource;
//    public AudioClip clip;
//    private bool isOn;
//    // Update is called once per frame
//    private void Awake()
//    {
//        //audio = GetComponent<AudioSource>();
//        FlashLight.SetActive(false);
//    }
//    void Update()
//    {

//        if (Input.GetKeyDown(KeyCode.F))
//        {
//            isOn = !isOn;
//            if (isOn)
//                FlashLight.SetActive(true);
//            else
//                FlashLight.SetActive(false);
//            audio.PlayOneShot(clip);
//        }
//    }
//}
