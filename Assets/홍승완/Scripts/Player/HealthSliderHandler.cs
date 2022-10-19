using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HealthSliderHandler : MonoBehaviourPun
{
    // ###############################################
    //             NAME : HongSW                      
    //             MAIL : gkenfktm@gmail.com         
    // ###############################################

    //[SerializeField] Camera _cam;

    [SerializeField] Stats _stats;
    Slider _hpSlider;

    private void Awake()
    {
        _hpSlider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        _hpSlider.maxValue = _stats.maxHealth;
    }

    // lateUpdate에서 처리할것
    private void LateUpdate()
    {
        // 체력 게이지는 일정높이로 로컬플레이어를 따라 다닌다
        if (!photonView.IsMine)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = PlayerBehaviour.CurrentPlayerPos + new Vector3(0, 3, 0);
        transform.Rotate(0, 0, 0);

        //_hpSlider.value = _stats.health;
        photonView.RPC("UpdateHealthBar", RpcTarget.All);
    }

    [PunRPC]
    private void UpdateHealthBar()
    {
        _hpSlider.value = _stats.health;
    }
}
