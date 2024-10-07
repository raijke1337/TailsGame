using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGN_Robots
{
    public class Character_Controller : MonoBehaviour
    {
        [HideInInspector] public string _AnimationsName;
        [Header("Hands")]
        [SerializeField] private GameObject Hand_L;
        [SerializeField] private GameObject Hand_R;
        [Header("Hands")]
        [SerializeField] private GameObject Gun_L;
        [SerializeField] private GameObject Gun_R;
        [Header("Fire Particles")]
        public ParticleSystem _FireLeft;
        public ParticleSystem _FireRight;
        public void Start()
        {
            ActiveHands();
        }
        public void Update()
        {
            
        }
        public void SetAnimation()
        {
            GetComponent<Animator>().Play(_AnimationsName);

        }
        public void ActiveHands()
        {
            Hand_L.SetActive(true);
            Hand_R.SetActive(true);
            Gun_L.SetActive(false);
            Gun_R.SetActive(false);
        }
        public void ActiveGuns()
        {
            Hand_L.SetActive(false);
            Hand_R.SetActive(false);
            Gun_L.SetActive(true);
            Gun_R.SetActive(true);
        }
        public void HandGunSwich()
        {
            if (Hand_L.activeInHierarchy)
                ActiveGuns();

            else
                ActiveHands();
        }
        public void ActiveLeftGun()
        {
            Hand_L.SetActive(false);
            Gun_L.SetActive(true);
        }
        public void ActiveRightGun()
        {
            Hand_R.SetActive(false);
            Gun_R.SetActive(true);
        }
        public void FireLeft()
        {
            _FireLeft.Play();
        }
        public void FireRight()
        {
            _FireRight.Play();
        }
        public void Fire()
        {
            FireLeft();
            FireRight();
        }
    }
}
