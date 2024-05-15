using UnityEngine;

public class ScrollTextMessageBehaviour : ScrollMessageBehaviour
{
    private TMPro.TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = this.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void SetText(string textContents)
    {
        textMeshPro.text = textContents;
    }
}
