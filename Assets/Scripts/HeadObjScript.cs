using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadObjScript : MonoBehaviour
{
   [SerializeField] public PlayerScript playerScript;

    private void Awake()
    {
        playerScript = GameObject.Find("PlayerCube").GetComponent<PlayerScript>();
        
    }
    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "GhostColl" || other.name =="Plane" )
        {
            // StartCoroutine(MinusPoint(playerScript.Point));
            playerScript.StrtCoroutine();
            Destroy(this.gameObject, 0.5f);//patlama efekti ve zamanlayýcýsý ekle
        }
        
    }
    /*  IEnumerator MinusPoint(float HoldPoint)
    {
        Debug.Log("1");
        while (playerScript.Point >= playerScript.Point-25 )
        {
            Debug.Log("2");
            playerScript.Point -= 1;
            yield return new WaitForSeconds(0.01f);
            playerScript.PointTxt.text = playerScript.Point.ToString();

        }
        Destroy(this.gameObject, 5f);
        yield break;
    }*/
  
}
