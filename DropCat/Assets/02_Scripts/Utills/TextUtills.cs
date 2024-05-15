using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextUtills : MonoBehaviour
{
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.MiddleCenter, TextAlignment textAlignment = TextAlignment.Center, int sortingOrder = 0)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }
    public static TextMesh CreateWorldText(Transform parent, string text, Vector2 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("Grid_World_Text", typeof(TextMesh));
        gameObject.tag = "GRIDWORLDTEXT";
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    
    public static void DestoryWorldText()
    {
        foreach (GameObject text in GameObject.FindGameObjectsWithTag("GRIDWORLDTEXT"))
        {
            Destroy(text.gameObject);
        }
    }
}
