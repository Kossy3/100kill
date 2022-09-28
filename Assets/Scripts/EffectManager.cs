using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public ParticleSystem slasheffect;
    private int _defeated_enemies;
    public Database database;
    public Text good_misstext;
    public Animator catchanim;
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

    public IEnumerator goodtext()
    {
        good_misstext.text = "巧";
        good_misstext.color = new Color(192f / 255f, 115f / 255f, 57f / 255f, 255f / 255f);
        yield return new WaitForSeconds(0.2f);
        good_misstext.text = null;
    }

    public IEnumerator misstext()
    {
        good_misstext.text = "拙";
        good_misstext.color = new Color(57f / 255f, 74f / 255f, 192f / 255f, 255f / 255f);
        yield return new WaitForSeconds(0.2f);
        good_misstext.text = null;
    }
}
