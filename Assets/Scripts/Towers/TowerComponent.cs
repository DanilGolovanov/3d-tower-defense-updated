using System.Collections;
using System.Collections.Generic;
using TowerDefence.Towers;
using UnityEngine;

public class TowerComponent : MonoBehaviour
{
    private ParticleSystem particles;

    private TowerType tower;

    public LineRenderer bulletLine;

    private void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        if (!GetComponent<TowerType>())
        {
            tower = GetComponentInParent<TowerType>();
        }
        else
        {
            tower = GetComponent<TowerType>();
        }
    }

    public void PlayAttackVisuals()
    {
        particles.Play();
    }

    public void DrawBulletLineCo()
    {
        StartCoroutine("DrawBulletLine");
    }

    IEnumerator DrawBulletLine()
    {
        bulletLine.gameObject.SetActive(true);
        bulletLine.positionCount = 2;
        bulletLine.SetPosition(0, transform.position);
        bulletLine.SetPosition(1, new Vector3(tower.closeEnemies[0].transform.position.x, 
                                            tower.closeEnemies[0].transform.position.y + tower.closeEnemies[0].GetComponent<BoxCollider>().size.y / 2, 
                                            tower.closeEnemies[0].transform.position.z));
        yield return new WaitForSeconds(.1f);
        bulletLine.gameObject.SetActive(false);
    }
}
