using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    public ParticleSystem slasheffect;
    public int _defeated_enemies;
    public Database database;
    public Text good_misstext;
    public ParticleSystem catchanim;
    public int _defeated_color_enemies;
    public float move;
    public RectTransform maku;

    // Start is called before the first frame update
    void Start()
    {
        _defeated_enemies = database.defeated_enemies;
        _defeated_color_enemies = database.defeated_color_enemies;
        move = 1200f;
        maku.position = new Vector2(640, move);
        database = GameObject.Find("Database").GetComponent<Database>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_defeated_enemies < database.defeated_enemies)
        {
            StartCoroutine(effect());
        }
        if (_defeated_color_enemies < database.defeated_color_enemies)
        {
            StartCoroutine(catcheffect());
        }
        if (database.HP == 0)
        {
            
            move -= 1200f * Time.deltaTime;
            maku.position = new Vector2(640, move);
            if (move < 350f)
            {
                move = 350f;
                database.finish = true;
            }
        }
    }
    private IEnumerator effect()
    {
        _defeated_enemies = database.defeated_enemies;
        var x = Instantiate(slasheffect, new Vector2(-6, -2), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(x.gameObject);
    }

    public IEnumerator catcheffect()
    {
        _defeated_color_enemies = database.defeated_color_enemies;
        var y = Instantiate(catchanim, new Vector2(-6, -2), Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Destroy(y.gameObject);
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
