using TMPro;
using UnityEngine;

public class MonnyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coin;
    [SerializeField] private TextMeshProUGUI _soul;

    public void SetCoin(int coin)
    {
        _coin.text = coin.ToString();
    }

    public void SetSoul(int soul)
    {
        _soul.text = soul.ToString();
    }
}
