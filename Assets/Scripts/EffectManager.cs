using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public ParticleSystem slasheffect;
    private int _defeated_enemies;
    public Database database;
    // Start is called before the first frame update
    void Start()
    {
        _defeated_enemies = database.defeated_enemies;
    }

    // Update is called once per frame
    void Update()
    {
        if (_defeated_enemies < database.defeated_enemies)
        {
            StartCoroutine(effect());
        }
    }
    private IEnumerator effect()
    {
        _defeated_enemies = database.defeated_enemies;
        var x = Instantiate(slasheffect, new Vector2(-6, -2), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(x.gameObject);
    }
}
