using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterBehaviour.Attack;
using Spine.Unity;

public class ComboCountUI : MonoBehaviour
{
    private SkeletonGraphic skeletonGraphic = null;
    private TMPro.TextMeshProUGUI comboText = null;
    private TMPro.TextMeshProUGUI additionalComboScoreText = null;

    private PlayerBasicAttackLogger playerBasicAttackLog = null;
    private Spine.TrackEntry trackEntry = null;

    private void Awake()
    {
        GameObject playerAttackObj = GameObject.FindGameObjectWithTag("PLAYER_Character").transform.GetChild(0).gameObject;
        playerBasicAttackLog = playerAttackObj.GetComponent<PlayerBasicAttackLogger>();
        skeletonGraphic = this.transform.GetChild(0).GetComponent<SkeletonGraphic>();
        comboText = this.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        additionalComboScoreText = this.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        additionalComboScoreText.text = "";
    }

    private void Update()
    {
        if(playerBasicAttackLog.ShouldDisplayUIFeedback())
        {
            DisplayComboFeedback();
            UpdateComboCountText();
        }
        else
        {
            skeletonGraphic.gameObject.SetActive(false);
            comboText.gameObject.SetActive(false);
            additionalComboScoreText.text = "";
            trackEntry = null;
        }
    }

    private void DisplayComboFeedback()
    {
        skeletonGraphic.gameObject.SetActive(true);
        comboText.gameObject.SetActive(true);
        if (trackEntry == null)
        {
            trackEntry = skeletonGraphic.AnimationState.SetAnimation(0, "InitCombo", false);
            trackEntry = skeletonGraphic.AnimationState.AddAnimation(0, "ContinueCombo", true, 0f);
        }
    }

    private void UpdateComboCountText()
    {
        comboText.text = playerBasicAttackLog.BasicAttackComboCount.ToString();
        if(5 <= playerBasicAttackLog.BasicAttackComboCount)
        {
            additionalComboScoreText.text = "Score + " + playerBasicAttackLog.CurrentAdditionalComboScore.ToString();
        }
        else
        {
            additionalComboScoreText.text = "";
        }
    }
}
