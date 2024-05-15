using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TimeAttack : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI timerTxt;
    [SerializeField] private float selectCountdown;
    private float startCountdown = 8f;
    private ObjectSpawnManager objectSpawnManager = null;
    private float timeLimit = 100f;
    public float TimeLimit { get { return timeLimit; } }
    public float SelectCountdown { get { return Mathf.Floor(selectCountdown); } }
    private Transform playerObj = null;
    private PlayerBasicAttackLogger playerBasicAttackLogger = null;

    private void Awake()
    {
        objectSpawnManager = GameObject.Find("[ObjectSpawnManager]").GetComponent<ObjectSpawnManager>();
        timeLimit = selectCountdown;

        playerObj = GameObject.Find("[Player]").transform; 
        playerBasicAttackLogger = playerObj.GetChild(0).GetComponent<PlayerBasicAttackLogger>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timerTxt != null)
        {
            if (startCountdown > 0)
            {
                startCountdown -= Time.deltaTime;

                timerTxt.text = Mathf.Floor(selectCountdown).ToString();
            }
            else if (selectCountdown > 0)
            {
                selectCountdown -= Time.deltaTime;
                timerTxt.text = Mathf.Floor(selectCountdown).ToString();
            }
            else if (selectCountdown < 1)
            {
                timerTxt.text = "0";
                if (objectSpawnManager.GetModeName == ObjectSpawnManager.ModeName.DjClimbing)
                {
                    GameOverEventManager.current.GameOverTriggerEnter();
                }
            }
        }
    }
}
