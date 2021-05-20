using UnityEngine;

public class PowerUpWall : MonoBehaviour
{
    public enum TypePowerUp
    {
        BigExplosion,
        MoreBombs,
        PlusLife
    }
    [SerializeField]public TypePowerUp myType;

    private void Start()
    {
       int randomBetweenTheThree = Random.Range(0, 100);

       if (randomBetweenTheThree < 25)
           myType = TypePowerUp.PlusLife;
       else if(randomBetweenTheThree > 25 && randomBetweenTheThree < 50)
           myType = TypePowerUp.BigExplosion;
       else if(randomBetweenTheThree > 50 && randomBetweenTheThree < 100)
           myType = TypePowerUp.MoreBombs;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player refPj = other.gameObject.GetComponent<Player>();
            if (refPj != null)
            {
                switch (myType)
                {
                    case TypePowerUp.BigExplosion:

                        refPj.radiusMyBombs++;

                        break;
                    case TypePowerUp.MoreBombs:

                        refPj.maxAmountBombs++;

                        break;
                    case TypePowerUp.PlusLife:

                        refPj.lifes++;

                        break;
                }
                refPj.passMyDataToTheUI?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
