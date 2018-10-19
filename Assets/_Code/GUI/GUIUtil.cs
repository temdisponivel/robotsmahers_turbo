using System;
using GamepadInput;
using RobotSmashers.Robots;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RobotSmashers.GUI {
    public static class GUIUtil {
        public static ComponentImageByType FindComponentImage(ComponentImageByType[] list, ComponentType type) {
            for (int i = 0; i < list.Length; i++) {
                ComponentImageByType image = list[i];
                if (image.Component == type) {
                    return image;
                }
            }

            return null;
        }
        
        public static ButtonImage FindButtonImages(ButtonImage[] list, GamePad.Button button) {
            for (int i = 0; i < list.Length; i++) {
                ButtonImage image = list[i];
                if (image.Button == button) {
                    return image;
                }
            }

            return null;
        }
        
        public static void UpdateHealthBar(Robot[] robots, HealthBar healthBars) {
            UnityEngine.Debug.Assert(robots.Length == healthBars.HealthBarImages.Length, "There's not enough health bars images. There need to be at least: " + robots.Length);
            
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];
                float normalizedHp = robot.CurrentHP / robot.DefaultHP;

                Text robotNameText = healthBars.RobotNames[i];
                robotNameText.text = robot.Name;

                Image image = healthBars.HealthBarImages[i];
                image.fillAmount = 1 - normalizedHp;

                image.color = robot.Color;
            }
        }
        
        public static void SetupButtonGUI(Robot[] robots, GameplayGUIMaster master) {
            ButtonBindingsGUI gui = master.ButtonBindings;
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];
                HorizontalLayoutGroup group = gui.Group[i];

                for (int j = 0; j < robot.Chassi.AllSlots.Length; j++) {
                    ComponentSlot slot = robot.Chassi.AllSlots[j];
                    
                    ComponentImageByType image = FindComponentImage(master.Images, slot.Component);
                    UnityEngine.Debug.Assert(image != null, slot.Component);
                    if (image == null) {
                        continue;
                    }
                    ButtonImage buttonImage = FindButtonImages(master.ButtonImages, slot.UseButton);
                    UnityEngine.Debug.Assert(buttonImage != null, slot.UseButton);
                    if (buttonImage == null) {
                        continue;
                    }
                    
                    ButtonGUI buttonGUI = UnityEngine.Object.Instantiate(gui.ButtonPrefab, group.transform);
                    
                    buttonGUI.ComponentImage.sprite = image.Image;
                    buttonGUI.ButtonImage.sprite = buttonImage.Image;
                }
            }
        }

        public static void SetupStartRoundGUI(StartRoundGUI gui) {
            gui.CurrentStartMatchCooldown = gui.DefaultStartMatchCooldown;
        }

        public static void UpdateStartRoundGUI(GameplayGUIMaster master, Match match) {
            master.StartRoundGUI.CurrentStartMatchCooldown -= Time.deltaTime;
            if (master.StartRoundGUI.CurrentStartMatchCooldown <= 0) {
                if (master.StartRoundGUI.CurrentStartMatchCooldown > -.5f) {
                    master.StartRoundGUI.CountdownText.text = "GO";
                } else {
                    master.StartRoundGUI.CountdownText.text = Mathf.CeilToInt(master.StartRoundGUI.CurrentStartMatchCooldown).ToString();
                    ChangeState(master, match, GameplayGUIState.PLAYING);
                }
            } else {
                master.StartRoundGUI.CountdownText.text = Mathf.CeilToInt(master.StartRoundGUI.CurrentStartMatchCooldown).ToString();
            }
        }

        public static void SetupEndRoundGUI(EndRoundGUI gui) {
            gui.CurrentReloadArenaCooldown = gui.DefaultReloadArenaCooldown;
        }

        public static void UpdateEndRoundGUI(GameplayGUIMaster master, Match match) {
            master.EndRoundGUI.CurrentReloadArenaCooldown -= Time.deltaTime;
            if (master.EndRoundGUI.CurrentReloadArenaCooldown <= 0) {
                GameplayLoopManager.ReloadSceneAndChangeState(GameplayGUIState.ROUND_START, false);
            }
        }

        public static void SetupEndMatchGUI(EndMatchGUI gui, Match match) {
            gui.WinnerText.text = match.Winner.Name;
        }

        public static void UpdateEndMatchGUI(GameplayGUIMaster master, Match match) {
            if (master.EndMatchGUI.RestartClicked) {
                SceneManager.LoadScene("_Title");
            }
        }
        
        public static void UpdateGUI(GameplayGUIMaster master, Match match) {
            switch (master.CurrentState) {
                case GameplayGUIState.PLAYING:
                    UpdateHealthBar(match.Robots, master.HealthBar);
                    break;
                case GameplayGUIState.ROUND_START:
                    UpdateStartRoundGUI(master, match);
                    break;
                case GameplayGUIState.ROUND_ENDED:
                    UpdateEndRoundGUI(master, match);
                    break;
                case GameplayGUIState.MATCH_ENDED:
                    UpdateEndMatchGUI(master, match);
                    break;
            }
        }

        public static void ChangeState(GameplayGUIMaster master, Match match, GameplayGUIState state) {
            master.CurrentState = state;
            Debug.Log("Changing state to: " + state);
            
            switch (master.CurrentState) {
                case GameplayGUIState.PLAYING:
                    master.HealthBar.gameObject.SetActive(true);
                    master.ButtonBindings.gameObject.SetActive(true);
                    master.EndMatchGUI.gameObject.SetActive(false);
                    master.EndRoundGUI.gameObject.SetActive(false);
                    master.StartRoundGUI.gameObject.SetActive(false);
                    
                    SetupButtonGUI(match.Robots, master);
                    break;
                case GameplayGUIState.ROUND_START:
                    master.HealthBar.gameObject.SetActive(false);
                    master.ButtonBindings.gameObject.SetActive(false);
                    master.EndMatchGUI.gameObject.SetActive(false);
                    master.EndRoundGUI.gameObject.SetActive(false);
                    master.StartRoundGUI.gameObject.SetActive(true);
                    
                    SetupStartRoundGUI(master.StartRoundGUI);
                    break;
                case GameplayGUIState.ROUND_ENDED:
                    master.HealthBar.gameObject.SetActive(false);
                    master.ButtonBindings.gameObject.SetActive(false);
                    master.EndMatchGUI.gameObject.SetActive(false);
                    master.EndRoundGUI.gameObject.SetActive(true);
                    master.StartRoundGUI.gameObject.SetActive(false);
                    
                    SetupEndRoundGUI(master.EndRoundGUI);
                    break;
                case GameplayGUIState.MATCH_ENDED:
                    master.HealthBar.gameObject.SetActive(false);
                    master.ButtonBindings.gameObject.SetActive(false);
                    master.EndMatchGUI.gameObject.SetActive(true);
                    master.EndRoundGUI.gameObject.SetActive(false);
                    master.StartRoundGUI.gameObject.SetActive(false);
                    
                    SetupEndMatchGUI(master.EndMatchGUI, match);
                    break;
            }
        }
    }
}