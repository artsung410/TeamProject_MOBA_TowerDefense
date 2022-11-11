using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Wand : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public PlayerBehaviour _playerScript;
    public Transform EnergyGem;
    public GameObject Bullet;

    GameObject bulletGo;

    #region 애니메이션 이벤트 적용

    public void FireEnergyBall()
    {
        if (photonView.IsMine)
        {
            // 총알 발사
            Debug.Log($"총알생성");
            bulletGo = PhotonNetwork.Instantiate(Bullet.name, EnergyGem.transform.position, Quaternion.identity);
            Debug.Log($"총알발사 : {bulletGo.name}");
            if (bulletGo != null)
            {
                // 총알에 PlayerBehaviour컴포넌트 정보를 넘겨줌
                bulletGo.GetComponent<EnergyBall>().GetTargetObject(_playerScript);
                Debug.Log($"총알이 스크립트를 가졌는가? : {bulletGo.GetComponent<EnergyBall>()}");
            }
        }
    }

    #endregion
}
