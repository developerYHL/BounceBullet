using ClientLibrary;
using UnityEngine;
using UnityEngine.UI; // UI 관련 코드
using Photon.Pun;

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity
{
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더
    public GunCtrl theGun;
    public PlayerController playerCtl;
    private Animator playerAnimator; // 플레이어의 애니메이터

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerCtl = FindObjectOfType<PlayerController>();
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        // 체력 슬라이더 활성화
        healthSlider.gameObject.SetActive(true);
        // 체력 슬라이더의 최대값을 기본 체력값으로 변경
        healthSlider.maxValue = startingHealth;
        // 체력 슬라이더의 값을 현재 체력값으로 변경
        healthSlider.value = health;
    }

    // 체력 회복
    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);
        // 체력 갱신
        healthSlider.value = health;
    }


    // 데미지 처리
    [PunRPC]
    public override void OnDamage(float damage)
    {
        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage);
        // 갱신된 체력을 체력 슬라이더에 반영
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        // 체력 슬라이더 비활성화
        healthSlider.gameObject.SetActive(false);

        // 애니메이터의 Die 트리거를 발동시켜 사망 애니메이션 재생
        playerAnimator.SetTrigger("Die");

        theGun.gunState = GunCtrl.State.Die;
        playerCtl.enabled = false;

        // 5초 뒤에 리스폰
        Invoke("Respawn", 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        // 사망하지 않은 경우에만 아이템 사용가능
        if (!dead)
        {
            // 충돌한 상대방으로 부터 Item 컴포넌트를 가져오기 시도
            IItem item = other.GetComponent<IItem>();

            // 충돌한 상대방으로부터 Item 컴포넌트가 가져오는데 성공했다면
            if (item != null)
            {
                // 호스트만 아이템 직접 사용 가능
                // 호스트에서는 아이템을 사용 후, 사용된 아이템의 효과를 모든 클라이언트들에게 동기화시킴
                if (PhotonNetwork.IsMasterClient)
                {
                    // Use 메서드를 실행하여 아이템 사용
                    item.Use(gameObject);
                }
            }
        }
    }

    // 부활 처리
    public void Respawn()
    {
        // 로컬 플레이어만 직접 위치를 변경 가능
        if (photonView.IsMine)
        {
            Transform respawnPos = ServerManager.instance.playerSpawn[photonView.OwnerActorNr - 1];
            transform.position = respawnPos.position;
        }

        // 컴포넌트들을 리셋하기 위해 게임 오브젝트를 잠시 껐다가 다시 켜기
        // 컴포넌트들의 OnDisable(), OnEnable() 메서드가 실행됨
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        playerCtl.enabled = true;
    }
}