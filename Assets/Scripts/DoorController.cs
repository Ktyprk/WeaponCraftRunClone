using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public enum DoorType
{
    FireRate,
    FireRange,
    initYear
}

public class DoorController : MonoBehaviour
{
    public GameObject player;
    public DoorType doorType = DoorType.FireRate;
    public float doorFireRateIncrease = 10f, doorFireRangeIncrease = 10f, addYear = 10f;
    public TextMeshProUGUI fireRateText;
    private void Start()
    {
        player = PlayerController.Instance.gameObject;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            switch (doorType)
            {
                case DoorType.FireRate:
                    doorFireRateIncrease += 1f;
                    fireRateText.text = doorFireRateIncrease.ToString();
                    other.gameObject.SetActive(false);
                    break;
                case DoorType.FireRange:
                    doorFireRangeIncrease += 1f;
                    fireRateText.text = doorFireRangeIncrease.ToString();
                    other.gameObject.SetActive(false);
                    break;
                case DoorType.initYear:
                    // Gizli kapı için başka bir şey yap
                    break;
                default:
                    break;
            }
        }
        else if (other.CompareTag("Player"))
        {
            switch (doorType)
            {
                case DoorType.FireRate:
                    PlayerController.Instance.IncreaseFireRate(doorFireRateIncrease/10);
                    break;
                case DoorType.FireRange:
                    PlayerController.Instance.IncreaseFireRange(doorFireRateIncrease/10);
                    break;
                case DoorType.initYear:
                    PlayerController.Instance.IncreaseFireRange(addYear);
                    break;
                default:
                    break;
            }
        }
    }
}