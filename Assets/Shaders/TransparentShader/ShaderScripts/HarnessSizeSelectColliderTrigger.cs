using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarnessSizeSelectColliderTrigger : MonoBehaviour
{ 
    [SerializeField]
    int indexSize;
    HarnessMasserManagerUI harnessMasserManagerUI;
    // Start is called before the first frame update
    void Start()
    {
        harnessMasserManagerUI = FindObjectOfType<HarnessMasserManagerUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LeftHand") || other.gameObject.CompareTag("RightHand"))
        {
            harnessMasserManagerUI.AdjustHarnessSizeHandInteraction(indexSize);
        }
    }
}
