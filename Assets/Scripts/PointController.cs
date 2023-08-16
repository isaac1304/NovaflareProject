using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointController : MonoBehaviour
{
    [SerializeField]
    public static int points = 0;

    [SerializeField]
    TextMeshProUGUI pointsText;

    public void AddPoint()
    {
        points++;
        pointsText.text = points.ToString();
    }
}
