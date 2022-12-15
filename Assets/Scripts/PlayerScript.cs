using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    Rigidbody Rb;

    [SerializeField] public GameObject RedH, FrontColl, BackColl;
    [SerializeField] public Transform RedHPos, LastChild;
    [SerializeField] public float Thrust, MaxSpeed, Point, PosY = 1, ChildCount;
    [SerializeField] public bool IsAvaible = true;
    [SerializeField] public TextMeshProUGUI PointTxt, TouchTxt, Touch_2Txt;
    public Vector3 jump;
    public float jumpForce = 2.0f;

    [Header("MoveScript")]
    [SerializeField] bool right, left;
    [SerializeField] Vector3 Vec_right,Vec_left; 
    [SerializeField] int PlayerSpeed=1;
    Vector3 FirstPos, LastPos;
    Touch touch;

    /// <Yap�lcaklar>
    /// D��en yada ayr�lan bir kare par�as� patlay�p(efektle) yok olmal�
    /// Duvara de�dikten sonra arkada kalan kareler yok olmal�
    /// Karenin alt�na h�zland�k�a artan bir efekt ekle
    /// Duvara �arp�nca objenin yeri daha ak��kan ve d�zg�n de�i�meli(hepsinin i�ine playerscript koyup ge�enlerden biri i�in aktif edilebilir)
    /// Karakterin h�zlan�nca �ekli bozuluyor yada �arp�nca
    /// 
    /// En son toplam kare say�s� kadar puan/�d�l kazan�lma 
    /// </summary>

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        ChildCount = this.transform.childCount;
       

    }
    private void FixedUpdate()
    {
        PlayerMove();
      
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "RoadT" && IsAvaible)
        {
            IsAvaible = false;
            //BackColl.GetComponent<BoxCollider>().enabled = false;
            Touch_2Txt = other.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            StartCoroutine(ChangePoint(Point,other.gameObject));
        }
        if (other.name == "Red" || other.tag=="Red" && IsAvaible)
        {
            IsAvaible = false;

            TouchTxt = other.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            Destroy(other.gameObject);


            StartCoroutine(PlusPoint(Point));
            PosY++;
            RedHPos.position = new Vector3(RedHPos.position.x, PosY, RedHPos.position.z);
            Vector3 vector3 = new Vector3(RedHPos.position.x, PosY, RedHPos.position.z);

            GameObject ChildGameObj = Instantiate(RedH, vector3, Quaternion.identity); //Oyuncunun kafas�na kare ekle
            ChildGameObj.transform.SetParent(this.transform);
            ChildCount = this.transform.childCount; //cocuk say�s�n� tutan float degere atad�k

        }
        else if(other.name=="Yellow" || other.tag =="Yellow" || other.name == "Blue")
        {
            Destroy(other.gameObject);
            //Oyuncunun kafas�ndan kare azalt
        }
        if (other.name == "Wall")
        {
            if(Point < 50)
            {
                Destroy(this.gameObject,2f);
                //End Game
            }
            else
            {
                Point -= 50;
                Destroy(other.gameObject);
            }
            //HoldColl.GetComponent<BoxCollider>().enabled = false;
            //this.transform.position = LastChild.position;
        }
        if(other.name =="FinishLine" || other.tag == "FL") //Make Thrust Zero
        {
           // Rb.isKinematic = true;
            Thrust = 0;
            Debug.Log("Worked");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        PointTxt.text = Point.ToString();

        if (other.name == "Red")
        {
            Debug.Log("Worked");
            IsAvaible = true;
        }
    }
    void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rb.AddForce(jump * jumpForce, ForceMode.Impulse);

        }


        if (Rb != null)
        {
            if (Input.GetMouseButtonDown(0)) //&& mouseIsNotOverUI eklenebilir ) // buras� ilk mousa t�klad���m�zda �al���r
            {
                FirstPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))//buras� s�rekli �al���r mouse t�klad�ktan sonra
            {
                LastPos = Input.mousePosition;
                float FarkX = LastPos.x - FirstPos.x;
                transform.Translate(FarkX * Time.deltaTime * PlayerSpeed / 250, 0, 0);

            }
            if (Input.GetMouseButtonUp(0))//buras� mousedan elimizi kald�r�nca �al���r
            {
                FirstPos = Vector3.zero;
                LastPos = Vector3.zero;
            }
            if (Rb.velocity.magnitude > MaxSpeed)
            {
                Rb.velocity = Rb.velocity.normalized * MaxSpeed;
                //transform.Translate(0, 0, MaxSpeed * Time.deltaTime);
            }
            Rb.AddForce(transform.forward * (Thrust * Time.deltaTime), ForceMode.Impulse);
            
        }
    }
    IEnumerator PlusPoint(float HoldPoint)
    {
        float TxtInt = float.Parse(TouchTxt.text);

        while (Point < HoldPoint + TxtInt) {
            
        Point += 1;
        yield return new WaitForSeconds(0.01f);
            PointTxt.text = Point.ToString();

        }
        

        yield break;
    }
    IEnumerator  MinusPoint(float HoldPoint)
    {
        float TxtInt = float.Parse(TouchTxt.text);
        
        while (Point >  HoldPoint - TxtInt)//-25 yerine textte yazan de�eri al
        {
            //Debug.Log("2");
            Point -= 1;
            yield return new WaitForSeconds(0.01f);
            PointTxt.text = Point.ToString();

        }
        
        yield break;
    }
    IEnumerator ChangePoint(float HoldPoint,GameObject TouchObj)
    {
        Debug.Log(Touch_2Txt.text.Length);
        float TxtInt = float.Parse(Touch_2Txt.text.Substring(1, Touch_2Txt.text.Length-1));
        Debug.Log(TxtInt);

        switch (Touch_2Txt.text.Substring(0, 1))
        {
            case "x":
                Point *= TxtInt;break;
            case "/":
                Point /= TxtInt; break;
            case "-":
                Point -= TxtInt; break;
            default:
                Point += TxtInt; break;
        }
        Destroy(TouchObj, 0.1f);



        PointTxt.text = Point.ToString();
        yield return new WaitForSeconds(1f);

        IsAvaible = true;
        yield break;
    }
    public void StrtCoroutine()
    {
        StartCoroutine(MinusPoint(Point));
    }

   
    
}
