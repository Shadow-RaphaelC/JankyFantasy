using UnityEngine;
using UnityEngine.UI;

public class HealthBarLink : MonoBehaviour
{
    private Combattant target;
    private Slider slider;

    public void Setup(Combattant target, Slider slider)
    {
        this.target = target;
        this.slider = slider;

        if (slider != null)
        {
            slider.maxValue = target.MaxHP;
            slider.value = target.CurrentHP;
        }
    }

    private void Update()
    {
        if (target == null || slider == null) return;
        slider.value = target.CurrentHP;
    }
}
