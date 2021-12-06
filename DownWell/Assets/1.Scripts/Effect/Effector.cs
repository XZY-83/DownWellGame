using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effector : MonoBehaviour
{
    public List<Effect> effects;

    public void Generate(string name)
    {
        var fx = effects.Find(f => f.name == name);
        if(fx != null) Instantiate(fx.fx, fx.transform.position, Quaternion.Euler(0, 0, fx.angle));
    }
}

[System.Serializable]
public class Effect
{
    public string name;
    public Transform transform;
    public GameObject fx;
    public float angle = 0;
}
