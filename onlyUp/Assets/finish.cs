using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class finish : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Object;
    //gameObject.SetActive(false);
    void OnTriggerEnter(Collider other)
    {
        Object.SetActive(true);
        
    }
    private void OnTriggerExit(Collider other)
    {
        //Object.SetActive(false);
    }
}
