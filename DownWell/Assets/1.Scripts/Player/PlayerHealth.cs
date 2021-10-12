using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] int currentHealth = 0;

    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int CurrentHealth { get { return currentHealth; } }

    #region Event Declare
    //[Header("Event")]
    public event System.Action OnChangedHealth;
    public UnityEvent OnDie;
    #endregion

    //int addHP = 0;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// ���� ü�� ����
    /// </summary>
    /// <returns>currentHealth / maxHealth</returns>
    public float GetCurrentHealthRatio()
    {
        return (float)currentHealth / maxHealth;
    }

    public void GainHealth(int amount)
    {
        currentHealth += Mathf.Clamp(amount, 0, maxHealth);

        if (OnChangedHealth != null) OnChangedHealth.Invoke();

        //var additionalHP = amount - maxHealth;

        //if (additionalHP > 0) addHP += additionalHP;
    }

    public void LoseHealth()
    {
        currentHealth--;
        Debug.Log("Lose hp");

        if(OnChangedHealth != null) OnChangedHealth.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene(0);
    }
}