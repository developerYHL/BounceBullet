using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Photon.Pun;
using Cinemachine;

public class projectileActor : MonoBehaviourPun {

    public Transform spawnLocator; 
    public Transform spawnLocatorMuzzleFlare;
    public Transform shellLocator;
    public Animator recoilAnimator;

    public AudioClip gunSound;

    public Transform[] shotgunLocator;

    [System.Serializable]
    public class projectile
    {
        public string name;
        public Rigidbody bombPrefab;
        public GameObject muzzleflare;
        public float min, max;
        public bool rapidFire;
        public float rapidFireCooldown;   

        public bool shotgunBehavior;
        public int shotgunPellets;
        public GameObject shellPrefab;
        public bool hasShells;
    }
    public projectile[] bombList;


    string FauxName;
    public Text UiText;

    public bool UImaster = true;
    public bool CameraShake = true;
    public float rapidFireDelay;
    public CameraShakeProjectile CameraShakeCaller;

    float firingTimer;
    public bool firing;
    public int bombType = 0;

   // public ParticleSystem muzzleflare;

    public bool swarmMissileLauncher = false;
    int projectileSimFire = 1;

    public bool Torque = false;
    public float Tor_min, Tor_max;

    public bool MinorRotate;
    public bool MajorRotate = false;
    int seq = 0;


	// Use this for initialization
	void Start ()
    {
        if (UImaster)
        {
            UiText.text = bombList[bombType].name.ToString();
        }
        if (swarmMissileLauncher)
        {
            projectileSimFire = 5;
        }
        if (photonView.IsMine)
        {
            CameraShakeProjectile followcam = FindObjectOfType<CameraShakeProjectile>();
            CameraShakeCaller = followcam;
            //followcam.LookAt = transform;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        //BULLETS
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Switch(-1);
        }
        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown(KeyCode.E))
        {
            Switch(1);
        }

	    if(Input.GetButtonDown("Fire1"))
        {
            //GetComponent<AudioSource>().clip = gunSound;
            GetComponent<AudioSource>().Play();
            firing = true;
            Fire();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            GetComponent<AudioSource>().Stop();
            firing = false;
            firingTimer = 0;
        }

        if (bombList[bombType].rapidFire && firing)
        {
            if(firingTimer > bombList[bombType].rapidFireCooldown+rapidFireDelay)
            {
                Fire();
                firingTimer = 0;
            }
        }

        if(firing)
        {
            firingTimer += Time.deltaTime;
        }
	}

    public void Switch(int value)
    {
            bombType += value;
            if (bombType < 0)
            {
              bombType = bombList.Length;
              bombType--;
            }
            else if (bombType >= bombList.Length)
            {
                bombType = 0;
            }
        if (UImaster)
        {
            UiText.text = bombList[bombType].name.ToString();
        }
    }


    public void Fire()
    {
        if(CameraShake)
        {
            CameraShakeCaller.ShakeCamera();
        }
        spawnLocator.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


        PhotonNetwork.Instantiate(bombList[bombType].muzzleflare.name, spawnLocatorMuzzleFlare.position, spawnLocatorMuzzleFlare.rotation);
        //Instantiate(bombList[bombType].muzzleflare, spawnLocatorMuzzleFlare.position, spawnLocatorMuzzleFlare.rotation);
        //   bombList[bombType].muzzleflare.Play();

        if (bombList[bombType].hasShells)
        {
            PhotonNetwork.Instantiate(bombList[bombType].shellPrefab.name, shellLocator.position, shellLocator.rotation);
            //Instantiate(bombList[bombType].shellPrefab, shellLocator.position, shellLocator.rotation);
        }
        //recoilAnimator.SetTrigger("recoil_trigger");

        GameObject rocketInstance;
        rocketInstance = PhotonNetwork.Instantiate(bombList[bombType].bombPrefab.name, spawnLocator.position, spawnLocator.rotation);
        //rocketInstance = Instantiate(bombList[bombType].bombPrefab, spawnLocator.position, spawnLocator.rotation) as Rigidbody;
        //spawnLocator.eulerAngles = new Vector3(0, spawnLocator.eulerAngles.y, spawnLocator.localRotation.z);
        //print(rocketInstance.rotation + ", " + (spawnLocator.eulerAngles * new Vector3(0,1,1)));
        //transform.eulerAngles = new Vector3(transform.rotation.x, 0.0f, transform.rotation.z)

        // Quaternion.Euler(0,90,0)
        rocketInstance.GetComponent<Rigidbody>().AddForce(spawnLocator.forward * Random.Range(bombList[bombType].min, bombList[bombType].max));

        if (bombList[bombType].shotgunBehavior)
        {
            for(int i = 0; i < bombList[bombType].shotgunPellets ;i++ )
            {
                GameObject rocketInstanceShotgun;
                rocketInstanceShotgun = PhotonNetwork.Instantiate(bombList[bombType].bombPrefab.name, shotgunLocator[i].position, shotgunLocator[i].rotation);
                // Quaternion.Euler(0,90,0)
                rocketInstanceShotgun.GetComponent<Rigidbody>().AddForce(shotgunLocator[i].forward * Random.Range(bombList[bombType].min, bombList[bombType].max));
            }
        }

        if (Torque)
        {
            rocketInstance.GetComponent<Rigidbody>().AddTorque(spawnLocator.up * Random.Range(Tor_min, Tor_max));
        }
        if (MinorRotate)
        {
            RandomizeRotation();
        }
        if (MajorRotate)
        {
            Major_RandomizeRotation();
        }
    }


    void RandomizeRotation()
    {
        if (seq == 0)
        {
            seq++;
            transform.Rotate(0, 1, 0);
        }
      else if (seq == 1)
        {
            seq++;
            transform.Rotate(1, 1, 0);
        }
      else if (seq == 2)
        {
            seq++;
            transform.Rotate(1, -3, 0);
        }
      else if (seq == 3)
        {
            seq++;
            transform.Rotate(-2, 1, 0);
        }
       else if (seq == 4)
        {
            seq++;
            transform.Rotate(1, 1, 1);
        }
       else if (seq == 5)
        {
            seq = 0;
            transform.Rotate(-1, -1, -1);
        }
    }

    void Major_RandomizeRotation()
    {
        if (seq == 0)
        {
            seq++;
            transform.Rotate(0, 25, 0);
        }
        else if (seq == 1)
        {
            seq++;
            transform.Rotate(0, -50, 0);
        }
        else if (seq == 2)
        {
            seq++;
            transform.Rotate(0, 25, 0);
        }
        else if (seq == 3)
        {
            seq++;
            transform.Rotate(25, 0, 0);
        }
        else if (seq == 4)
        {
            seq++;
            transform.Rotate(-50, 0, 0);
        }
        else if (seq == 5)
        {
            seq = 0;
            transform.Rotate(25, 0, 0);
        }
    }
}
