using UnityEngine;
using UnityEngine.UI;

public static class UIUtils
{
    public static void UpdateHealthBar(Image healthBarFill, float currentVal, float maxVal)
    {
        float healthPercentage = currentVal / maxVal;
        healthBarFill.fillAmount = healthPercentage;
        healthBarFill.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    public static void UpdateHealthBar(Image healthBarFill, float currentPercentage)
    {
        healthBarFill.fillAmount = currentPercentage;
        healthBarFill.color = Color.Lerp(Color.red, Color.green, currentPercentage);
    }
}
