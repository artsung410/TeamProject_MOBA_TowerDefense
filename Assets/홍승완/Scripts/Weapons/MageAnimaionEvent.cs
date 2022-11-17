using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MageAnimaionEvent : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    public PlayerBehaviour _playerScript;
    public Stats _stats;
    public Transform EnergyGem; // 투사체 생성 위치
    public GameObject Bullet; // 생성할 프리팹

    GameObject bulletGo; // 투사체 정보 담는곳

    #region 애니메이션 이벤트 적용

    public void FireEnergyBall()
    {
        if (photonView.IsMine)
        {
            // 총알 발사
            //Debug.Log($"총알생성");
            // TODO : 총알을 오브젝트풀로 관리하는것을 고려해볼것
            bulletGo = PhotonNetwork.Instantiate(Bullet.name, EnergyGem.transform.position, Quaternion.identity);

            //Debug.Log($"총알발사 : {bulletGo.name}");

            if (bulletGo != null)
            {
                // 총알에 PlayerBehaviour컴포넌트 정보를 넘겨줌
                bulletGo.GetComponent<EnergyBall>().GetTargetObject(_playerScript);
                bulletGo.GetComponent<EnergyBall>().GetStatData(_stats);

                //Debug.Log($"총알이 스크립트를 가졌는가? : {bulletGo.GetComponent<EnergyBall>()}");
            }
        }
    }

    #endregion
}
