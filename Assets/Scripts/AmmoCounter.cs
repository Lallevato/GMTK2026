using TMPro;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{

    public static AmmoCounter instance;

    public TMP_Text boltText;
    public TMP_Text grenadeText;
    public int boltCurrentValue;
    public int grenadeCurrentValue;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boltText.text = "Silver Bolts:" + boltCurrentValue.ToString();
        grenadeText.text = "Holy Water:" + grenadeCurrentValue.ToString();
    }

    public void IncreaseSilverBolts(int v)
    {
        boltCurrentValue += v;
        boltText.text = "Silver Bolts:" + boltCurrentValue.ToString();
    }

    public void DecreaseSilverBolts(int v)
    {
        boltCurrentValue -= v;
        boltText.text = "Silver Bolts:" + boltCurrentValue.ToString();
    }

    public void IncreaseGrenades(int v)
    {
        grenadeCurrentValue += v;
        grenadeText.text = "Holy Water:" + grenadeCurrentValue.ToString();
    }

    public void DecreaseGrenades(int v)
    {
        grenadeCurrentValue -= v;
        grenadeText.text = "Holy Water:" + grenadeCurrentValue.ToString();
    }
}
