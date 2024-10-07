using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DGN_Robots
{
    public class ButtonFunctions : MonoBehaviour
    {
        public GameObject[] RobotsBlueOutline;
        public GameObject[] RobotsGreenOutline;
        public GameObject[] RobotsOrangeOutline;
        public GameObject[] RobotsPurpleOutline;
        public GameObject[] RobotsYellowOutline;
        public GameObject[] RobotsRedOutline;
        [Header("Rotaters")]
        public Rotater _rotaterRobots;
        [Header("10 Placer")]
        public GameObject _10placer;
        [Header("Color Placer")]
        public GameObject _ColorPlacer;

        public void RotaterOnOf()
        {
            if (_rotaterRobots.rotationSpeed > 15f)
                _rotaterRobots.rotationSpeed = 0f;
            else
                _rotaterRobots.rotationSpeed = 40f;
        }
        public void HandGunSwich()
        {
            foreach(GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>().HandGunSwich();
            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>().HandGunSwich();
            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>().HandGunSwich();
            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>().HandGunSwich();
            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>().HandGunSwich();
            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>().HandGunSwich();
            }
        }
        #region Color Placer
        public void ColorBlue()
        {
            _ColorPlacer.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        public void ColorGreen()
        {
            _ColorPlacer.transform.localPosition = new Vector3(-150f, 0f, 0f);
        }
        public void ColorOrange()
        {
            _ColorPlacer.transform.localPosition = new Vector3(-300f, 0f, 0f);
        }
        public void ColorPurple()
        {
            _ColorPlacer.transform.localPosition = new Vector3(-450f, 0f, 0f);
        }
        public void ColorYellow()
        {
            _ColorPlacer.transform.localPosition = new Vector3(-600f, 0f, 0f);
        }
        public void ColorRed()
        {
            _ColorPlacer.transform.localPosition = new Vector3(-750f, 0f, 0f);
        }
        #endregion
        #region 10 placer
        public void Robot1()
        {
            _10placer.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        public void Robot2()
        {
            _10placer.transform.localPosition = new Vector3(-15f, 0f, 0f);
        }
        public void Robot3()
        {
            _10placer.transform.localPosition = new Vector3(-30f, 0f, 0f);
        }
        public void Robot4()
        {
            _10placer.transform.localPosition = new Vector3(-45f, 0f, 0f);
        }
        public void Robot5()
        {
            _10placer.transform.localPosition = new Vector3(-60f, 0f, 0f);
        }
        public void Robot6()
        {
            _10placer.transform.localPosition = new Vector3(-75f, 0f, 0f);
        }
        public void Robot7()
        {
            _10placer.transform.localPosition = new Vector3(-90f, 0f, 0f);
        }
        public void Robot8()
        {
            _10placer.transform.localPosition = new Vector3(-105f, 0f, 0f);
        }
        public void Robot9()
        {
            _10placer.transform.localPosition = new Vector3(-120f, 0f, 0f);
        }
        public void Robot10()
        {
            _10placer.transform.localPosition = new Vector3(-135f, 0f, 0f);
        }
        #endregion
        #region Animations
        public void DeathAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Death";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Death";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Death";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Death";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Death";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Death";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }

        }
        public void FallAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fall";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fall";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fall";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fall";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fall";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fall";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            
        }
        public void Fire1Animation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
           
        }
        public void Fire1_LeftAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            
        }
        public void Fire1_RightAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire1_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            
        }
        public void Fire2Animation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            
        }
        public void Fire2_LeftAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Fire2_RightAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Fire2_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Get_Hit_HardAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Hard";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Hard";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Hard";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Hard";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Hard";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Hard";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Get_Hit_LightAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Light";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Light";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Light";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Light";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Light";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Hit_Light";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Get_UpAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Get_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Guns_HideAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Hide";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Hide";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Hide";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Hide";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Hide";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Hide";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Guns_ReadyAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Ready";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Ready";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Ready";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Ready";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Ready";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Guns_Ready";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void IdleAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Idle_AimingAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Idle_Look_LeftAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Idle_Look_Left_AimingAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Left_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Idle_Look_RightAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Idle_Look_Right_AimingAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Idle_Look_Right_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Jump_LandAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Land";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Land";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Land";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Land";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Land";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Land";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Jump_UpAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Jump_Up";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void JumpUp_And_DownAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "JumpUp_And_Down";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "JumpUp_And_Down";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "JumpUp_And_Down";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "JumpUp_And_Down";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "JumpUp_And_Down";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "JumpUp_And_Down";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void RunningAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Running_AimingAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Running_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void WalkingAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        public void Walking_AimingAnimation()
        {
            foreach (GameObject obj in RobotsBlueOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsGreenOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsOrangeOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsPurpleOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsYellowOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
            foreach (GameObject obj in RobotsRedOutline)
            {
                obj.GetComponent<Character_Controller>()._AnimationsName = "Walking_Aiming";
                obj.GetComponent<Character_Controller>().SetAnimation();

            }
        }
        #endregion
    }

}