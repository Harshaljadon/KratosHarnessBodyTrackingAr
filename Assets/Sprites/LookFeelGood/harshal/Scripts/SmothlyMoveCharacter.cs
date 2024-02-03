using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmothlyMoveCharacter : MonoBehaviour
{
    public Transform characterPositionTransform, objMovePos; // Reference to the game object you want to move
    public float moveDistance = 1f; // Distance to move forward and backward
    public float duration = 1f; // Duration of the movement in seconds
    public Animator charcterAnime, objectMoveAnime;
    public Transform poleA, poleB;

    public bool turnsAB =  true, arMode, forwardBackword ;
    private void OnEnable()
    {
        Mirror();
        objectMoveAnime.enabled = true;
        //StartCoroutine(MoveObject());
    }


    private void Update()
    {
        if (turnsAB)
        {
            moveDistance = Vector3.Distance(characterPositionTransform.position, poleA.position);
            forwardBackword = turnsAB;
            objectMoveAnime.SetBool("forwardBackword", forwardBackword);

        }
        else
        {
            moveDistance = Vector3.Distance(characterPositionTransform.position, poleB.position);
            forwardBackword = turnsAB;
            objectMoveAnime.SetBool("forwardBackword", forwardBackword);

        }
        if (moveDistance < .01f)
        {
            //forwardBackword = !forwardBackword;
            charcterAnime.SetBool("Ideal", true); //forwardBackword
            arMode = false;
        }
    }

    private void LateUpdate()
    {
        
        characterPositionTransform.position = objMovePos.transform.position;
    }
    //private IEnumerator MoveObject()
    //{


    //    float elapsedTime = 0f;
    //    Vector3 startPosition = characterPositionTransform.position;
    //    Vector3 targetPosition = startPosition + characterPositionTransform.forward.normalized * moveDistance;

    //    while (elapsedTime < duration)
    //    {
    //        float t = elapsedTime / duration;
    //        characterPositionTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }


    //}
    public bool mirrorToggle;


    private void OnDisable()
    {
        if (!arMode)
        {
        turnsAB = !turnsAB;

        }
        //objectMoveAnime.enabled = false;
        charcterAnime.SetBool("Ideal", false);

        
    }

    void Mirror()
    {
        
        mirrorToggle = !mirrorToggle;
        charcterAnime.SetBool("ChangeMirror", mirrorToggle);

    }

    public void StopMoving()
    {
        arMode = turnsAB = mirrorToggle = true;
        //turnsAB = true;
        //mirrorToggle = true;
        StopAllCoroutines();
    }
}
