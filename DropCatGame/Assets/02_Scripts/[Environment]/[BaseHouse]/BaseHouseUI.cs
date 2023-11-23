using UnityEngine;
using UnityEngine.UI;

public class BaseHouseUI : MonoBehaviour
{
    private BaseHouseDurability baseHouseDurability;
    private Slider durabilitySlider;
    private TMPro.TextMeshProUGUI text;

    private int maxValue;
    private int value;

    private Color maxColorValue = new Color(255, 255, 255, 255);
    private Color minColorValue = new Color(255, 0, 0, 193);

    private void Awake()
    {
        GameObject baseHouse = GameObject.FindGameObjectWithTag("PLAYER_BaseHouse");
        baseHouseDurability = baseHouse.GetComponent<BaseHouseDurability>();
        durabilitySlider = this.GetComponent<Slider>();
        text = this.transform.GetChild(2).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Start()
    {
        maxValue = baseHouseDurability.Durability;
        durabilitySlider.maxValue = maxValue;
        value = maxValue;
    }

    public void ResetSliderValue()
    {
        value = maxValue;
        durabilitySlider.value = value;
        ChangeText();
    }

    private void Update()
    {
        UpdateValue();
        ChangeText();
    }

    private void UpdateValue()
    {
        value = baseHouseDurability.Durability;
        durabilitySlider.value = value;
    }

    private void ChangeText()
    {
        text.text = "HP " + value;
    }

    private bool ValueIsChanged()
    {
        return baseHouseDurability.Durability != value;
    }
}
