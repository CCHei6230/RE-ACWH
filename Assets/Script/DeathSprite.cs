using UnityEngine;
public class DeathSprite : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] m_deathSprite;

    public void SetSprite(SpriteRenderer _SR )
    {
        foreach (var tmp_SR in m_deathSprite)
        {
            tmp_SR.sprite =_SR.sprite;
            tmp_SR.gameObject.transform.localScale = _SR.gameObject.transform.localScale ;
            tmp_SR.gameObject.transform.localScale = new Vector3(tmp_SR.gameObject.transform.localScale.x * _SR.transform.root.transform.lossyScale.x, tmp_SR.gameObject.transform.localScale.y, tmp_SR.gameObject.transform.localScale.z);
        }
    }
}