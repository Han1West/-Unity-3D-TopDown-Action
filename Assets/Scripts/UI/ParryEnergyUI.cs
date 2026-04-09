using UnityEngine;
using UnityEngine.UI;

public class ParryEnergyUI : MonoBehaviour
{
    [SerializeField] Image[] parryEnergyImages;
    [SerializeField] PlayerGuard playerGuard;

    void Start()
    {
        playerGuard.OnParryEnergyChanged += UpdateParryEnergy;
    }

    void OnDestroy()
    {
        playerGuard.OnParryEnergyChanged -= UpdateParryEnergy;
    }

    public void UpdateParryEnergy(int current, int max)
    {
        for(int i =0; i < max; ++i)
        {
            if(i < current)
                parryEnergyImages[i].color = Color.yellow;
            else
                parryEnergyImages[i].color = Color.white;
        }
    }
}
