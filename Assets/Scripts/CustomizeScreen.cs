using UnityEngine;
using Gameplay.Data;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomizeScreen : MonoBehaviour
{
    public GameObject body;
    public GameObject rightEye;
    public GameObject leftEye;
    public Slider R;
    public Slider G;
    public Slider B;
    public Slider bodyHeightSlider;
    public Slider bodyWidthSlider;
    public Slider eyeSizeSlider;
    public Slider eyeOpenSlider;
    public Slider eyeSpacingSlider;
    public Slider irisSizeSlider;
    public Slider pupilSizeSlider;
    public Slider roughnessSlider;
    public Slider smileSlider;
    public Slider mouthWidthSlider;
    public Slider mouthOpenSlider;
    public Slider teethOpenSlider;
    public Slider eyesXSlider;
    public Slider eyesYSlider;

    public enum TargetColor
    {
        Body,
        Eye,
        Iris,
        Pupil
    }
    private TargetColor targetColor = TargetColor.Body;


    public void SetTargetColor(int color)
    {
        targetColor = (TargetColor)color;
        switch(targetColor) {
            case TargetColor.Body:
                R.value = GameSettings.bodyColor.r;
                G.value = GameSettings.bodyColor.g;
                B.value = GameSettings.bodyColor.b;
                break;
            case TargetColor.Eye:
                R.value = GameSettings.eyeColor.r;
                G.value = GameSettings.eyeColor.g;
                B.value = GameSettings.eyeColor.b;
                break;
            case TargetColor.Iris:
                R.value = GameSettings.irisColor.r;
                G.value = GameSettings.irisColor.g;
                B.value = GameSettings.irisColor.b;
                break;
            case TargetColor.Pupil:
                R.value = GameSettings.pupilColor.r;
                G.value = GameSettings.pupilColor.g;
                B.value = GameSettings.pupilColor.b;
                break;
        }
    }

    public void SetColorValueR(float value)
    {
        switch (targetColor)
        {
            case TargetColor.Body:
                GameSettings.bodyColor.r = value;
                break;
            case TargetColor.Eye:
                GameSettings.eyeColor.r = value;
                break;
            case TargetColor.Iris:
                GameSettings.irisColor.r = value;
                break;
            case TargetColor.Pupil:
                GameSettings.pupilColor.r = value;
                break;
        }
        updateValues();
    }

    public void SetColorValueG(float value)
    {
        switch (targetColor)
        {
            case TargetColor.Body:
                GameSettings.bodyColor.g = value;
                break;
            case TargetColor.Eye:
                GameSettings.eyeColor.g = value;
                break;
            case TargetColor.Iris:
                GameSettings.irisColor.g = value;
                break;
            case TargetColor.Pupil:
                GameSettings.pupilColor.g = value;
                break;
        }
        updateValues();
    }

    public void SetColorValueB(float value)
    {
        switch (targetColor)
        {
            case TargetColor.Body:
                GameSettings.bodyColor.b = value;
                break;
            case TargetColor.Eye:
                GameSettings.eyeColor.b = value;
                break;
            case TargetColor.Iris:
                GameSettings.irisColor.b = value;
                break;
            case TargetColor.Pupil:
                GameSettings.pupilColor.b = value;
                break;
        }
        updateValues();
    }

    public void BodyHeight(float value)
    {
        GameSettings.bodyHeight = value;
        updateValues();
    }
    public void BodyWidth(float value)
    {
        GameSettings.bodyWidth = value;
        updateValues();
    }
    public void EyeSize(float value)
    {
        GameSettings.eyeSize = value;
        updateValues();
    }
    public void EyeOpen(float value)
    {
        GameSettings.eyeOpen = value;
        updateValues();
    }
    public void EyeSpacing(float value)
    {
        GameSettings.eyeSpacing = value;
        updateValues();
    }
    public void IrisSize(float value)
    {
        GameSettings.irisSize = value;
        updateValues();
    }
    public void PupilSize(float value)
    {
        GameSettings.pupilSize = value;
        updateValues();
    }
    public void Roughness(float value)
    {
        GameSettings.roughness = value;
        updateValues();
    }
    public void Smile(float value)
    {
        GameSettings.smile = value;
        updateValues();
    }

    public void TeethOpen(float value)
    {
        GameSettings.teethOpen = value;
        updateValues();
    }

    public void MouthOpen(float value)
    {
        GameSettings.mouthOpen = value;
        updateValues();
    }

    public void SetSeed(string value)
    {
        GameSettings.seed = value.GetHashCode();
        updateValues();
    }

    public void EyeX(float value)
    {
        GameSettings.eyeX = value;
        updateValues();
    }

    public void EyeY(float value)
    {
        GameSettings.eyeY = value;
        updateValues();
    }

    public void MouthWidth(float value)
    {
        GameSettings.mouthWidth = value;
        updateValues();
    }

    private void updateValues()
    {
        Material _body = body.GetComponent<MeshRenderer>().material;
        Material _rightEye = rightEye.GetComponent<MeshRenderer>().material;
        Material _leftEye = leftEye.GetComponent<MeshRenderer>().material;
        // SkinColor
        _body.SetColor("Color_c9b0e0dfacb84c87a24618eea7b3d861", GameSettings.bodyColor);
        _rightEye.SetColor("Color_8cbb982647ed49f0a5c9f595711113f0", GameSettings.bodyColor);
        _leftEye.SetColor("Color_8cbb982647ed49f0a5c9f595711113f0", GameSettings.bodyColor);
        // SkinRoughness
        _body.SetFloat("Vector1_84b477cc3c2e4d089342a26b2a53ab9e", GameSettings.roughness);
        _rightEye.SetFloat("Vector1_549f0fee6d3c40239cb492f1c0dfe4df", GameSettings.roughness);
        _leftEye.SetFloat("Vector1_549f0fee6d3c40239cb492f1c0dfe4df", GameSettings.roughness);
        // EyeColor
        _rightEye.SetColor("Color_ca45d8a7905f40c291a1451df70014b0", GameSettings.eyeColor);
        _leftEye.SetColor("Color_ca45d8a7905f40c291a1451df70014b0", GameSettings.eyeColor);
        // Eye Open
        _rightEye.SetFloat("Vector1_4f08c76a1404422a8d66ae996bd2fbfa", GameSettings.eyeOpen);
        _leftEye.SetFloat("Vector1_4f08c76a1404422a8d66ae996bd2fbfa", GameSettings.eyeOpen);
        // IrisColor
        _rightEye.SetColor("Color_ac1f9112475c432685728afe941b7661", GameSettings.irisColor);
        _leftEye.SetColor("Color_ac1f9112475c432685728afe941b7661", GameSettings.irisColor);
        // PupilColor
        _rightEye.SetColor("Color_c0e17f4523ff42719aacaa10999c39c9", GameSettings.pupilColor);
        _leftEye.SetColor("Color_c0e17f4523ff42719aacaa10999c39c9", GameSettings.pupilColor);
        // IrisSize
        _rightEye.SetFloat("Vector1_eae4e3df392e4e06844152a91f4b5887", GameSettings.irisSize);
        _leftEye.SetFloat("Vector1_eae4e3df392e4e06844152a91f4b5887", GameSettings.irisSize);
        // PupilSize
        _rightEye.SetFloat("Vector1_572ab91db3d54d6c95239de619484396", GameSettings.pupilSize);
        _leftEye.SetFloat("Vector1_572ab91db3d54d6c95239de619484396", GameSettings.pupilSize);
        // LookX
        _rightEye.SetFloat("Vector1_9979475be7dd4d029c53f3b8d0bbb64a", GameSettings.eyeX);
        _leftEye.SetFloat("Vector1_9979475be7dd4d029c53f3b8d0bbb64a", GameSettings.eyeX);
        // LookY
        _rightEye.SetFloat("Vector1_bcb7e772ac2543f0a4f4ee289502ad17", GameSettings.eyeY);
        _leftEye.SetFloat("Vector1_bcb7e772ac2543f0a4f4ee289502ad17", GameSettings.eyeY);
        // Mouth
        _body.SetFloat("Vector1_6decf4ff65b849a3a10735c6af22a86c", GameSettings.mouthOpen);
        _body.SetFloat("Vector1_9785ac54f99345cdac2f47ee51317a62", GameSettings.mouthWidth);
        _body.SetFloat("Vector1_fe2e7edb90364604929ec17303e41edb", GameSettings.smile);
        _body.SetFloat("Vector1_a57ded8bb1e042dfbb2e2f2ab0e0bc3f", GameSettings.teethOpen);
        // BodyHeight
        body.gameObject.transform.localScale = new Vector3(
            GameSettings.bodyWidth,
            GameSettings.bodyHeight,
            GameSettings.bodyWidth
        );
        // EyeSize
        rightEye.gameObject.transform.localScale = new Vector3(
            GameSettings.eyeSize,
            GameSettings.eyeSize,
            GameSettings.eyeSize
        );
        leftEye.gameObject.transform.localScale = new Vector3(
            GameSettings.eyeSize,
            GameSettings.eyeSize,
            GameSettings.eyeSize
        );
        // EyeSpacing
        Vector3 directionOfTravelRight = -body.transform.right;
        Vector3 rightEyeDirection = directionOfTravelRight + directionOfTravelRight.normalized * GameSettings.eyeSpacing;
        rightEye.transform.position = body.transform.position + rightEyeDirection;

        Vector3 directionOfTravelLeft = body.transform.right;
        Vector3 leftEyeDirection = directionOfTravelLeft + directionOfTravelLeft.normalized * GameSettings.eyeSpacing;
        leftEye.transform.position = body.transform.position + leftEyeDirection;
    }
    
    private void Start() {
        bodyHeightSlider.value = GameSettings.bodyHeight;
        bodyWidthSlider.value = GameSettings.bodyWidth;
        eyeSizeSlider.value = GameSettings.eyeSize;
        eyeOpenSlider.value = GameSettings.eyeOpen;
        eyeSpacingSlider.value = GameSettings.eyeSpacing;
        irisSizeSlider.value = GameSettings.irisSize;
        pupilSizeSlider.value = GameSettings.pupilSize;
        roughnessSlider.value = GameSettings.roughness;
        smileSlider.value = GameSettings.smile;
        mouthWidthSlider.value = GameSettings.mouthWidth;
        mouthOpenSlider.value = GameSettings.mouthOpen;
        teethOpenSlider.value = GameSettings.teethOpen;
        eyesXSlider.value = GameSettings.eyeX;
        eyesYSlider.value = GameSettings.eyeY;
        R.value = GameSettings.bodyColor.r;
        G.value = GameSettings.bodyColor.g;
        B.value = GameSettings.bodyColor.b;
        updateValues();
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("ReminderScreen");
    }

}
