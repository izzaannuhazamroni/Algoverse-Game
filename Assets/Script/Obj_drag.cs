using UnityEngine;
using UnityEngine.Events;

public class Obj_drag : MonoBehaviour
{
    [HideInInspector]public Vector2 SavePos;
    [HideInInspector] private float distance;
    [HideInInspector] public bool isAbovePlacement;

    Transform SaveOb;
    public int ID;
    public UnityEvent OnDragRight;
    void Start()
    {
        SavePos = transform.position;
    }

    void Update()
    {

    }
    private void OnMouseUp()
    {
        if (isAbovePlacement)
        {
            int ID_Placement = SaveOb.GetComponent<ID_Placement>().ID;
            if (ID.Equals(ID_Placement))
            {
                if(SaveOb.childCount == 0)
                {
                    transform.SetParent(SaveOb);
                    transform.localPosition = Vector3.zero;
                    transform.localScale = SaveOb.localScale;
                    SaveOb.GetComponent<SpriteRenderer>().enabled = false;

                    SaveOb.GetComponent<Rigidbody2D>().simulated = false;
                    SaveOb.GetComponent<BoxCollider2D>().enabled = false;

                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    OnDragRight.Invoke();

                    GameSystem.instance.DataC++; ;
                    DataGame.DataScore += 10;
                }
                else
                {
                    transform.position = SavePos;
                }
            }
            else
            {
                transform.position = SavePos;
                DataGame.DataHealth--;
                DataGame.DataTimes -= 20;
            }
        }
        else
        {
            transform.position = SavePos;
        }
    }
    private void OnMouseDown()
    {
        //if (rb != null) {
        //    rb.isKinematic = false;
        //}
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
    }
    private void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;
        //Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = Pos;
    }
    private void OnTriggerStay2D(Collider2D trig)
    {
        if(trig.gameObject.CompareTag("Placement"))
        {
            isAbovePlacement = true;
            SaveOb = trig.gameObject.transform;
        }

    }
    private void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Placement"))
        {
            isAbovePlacement = false;
        }
    }
}
