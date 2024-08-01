using System.Collections;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    [SerializeField] private int idItem;
    [SerializeField] private ParticleSystem muzzle;
    [SerializeField] private float startPlayMuzzle = 0.25f;

    public int GetIdItem()
    {
        return idItem;
    }
    public void StartPlayMuzzle()
    {
        StartCoroutine(PlayMuzzle());
    }

    private IEnumerator PlayMuzzle()
    {
        yield return new WaitForSeconds(startPlayMuzzle);
        if(muzzle != null)
            muzzle.Play();
    }
}
