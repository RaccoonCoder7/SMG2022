using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event : MonoBehaviour
{
    [SerializeField]
    string EventName;

    [SerializeField]
    bool EventChk = false;

    // Start is called before the first frame update
    void Start()
    {
        EventName = this.gameObject.name;
    }

    private void Update()
    {
        if(EventChk)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("asd");

                SendEventNum();

                //SceneManager.LoadScene("")
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            EventChk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            EventChk = false;
        }
    }

    void SendEventNum()
    {
        int SecondIdx = 1;

        string[] EventNumberArr = EventName.Split('_');

        GameMgr.In.EventNumber = int.Parse(EventNumberArr[SecondIdx]);
    }
}
