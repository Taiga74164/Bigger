using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class AlphaClickBlocker : MonoBehaviour
{
    private void Start() => GetComponent<Image>().alphaHitTestMinimumThreshold = 1.0f;
}
