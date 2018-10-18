using GamepadInput;
using RobotSmashers.Robots;
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
                image.fillAmount = normalizedHp;
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
    }
}