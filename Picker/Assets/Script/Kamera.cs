using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 Target_offset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate() // daha soft bir gecis istedigimiz icin
    {
        transform.position = Vector3.Lerp(transform.position, Target.position + Target_offset, .125f); 
    }
}
