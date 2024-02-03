using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBetweenOtherObjecs : MonoBehaviour
{
    [SerializeField]
    Transform obj1, obj2, targetToSet;

    [SerializeField]
    [Range(0, 1)]
    float betweeAB;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetToSet.position = Vector3.Lerp(obj1.position, obj2.position, betweeAB);
    }
}
