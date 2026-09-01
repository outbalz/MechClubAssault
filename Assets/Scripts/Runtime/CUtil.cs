using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CUtil
{
    public static string GetRandomName()
    {
        string familyName = GetRandomfamilyName();
        string givenName = GetRandomGivenName();
        return $"{familyName}{givenName}";
    }


    public static string GetRandomfamilyName()
    {
        string familyName;

        int randomIndex = Random.Range(0, 100);

        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 199, 199);

        randomIndex = Mathf.RoundToInt(curve.Evaluate(randomIndex));

        

        switch (randomIndex)
        {
            case 0: familyName = "김"; break;
            case 1: familyName = "이"; break;
            case 2: familyName = "박"; break;
            case 3: familyName = "최"; break;
            case 4: familyName = "정"; break;
            case 5: familyName = "강"; break;
            case 6: familyName = "조"; break;
            case 7: familyName = "윤"; break;
            case 8: familyName = "장"; break;
            case 9: familyName = "임"; break;
            case 10: familyName = "한"; break;
            case 11: familyName = "오"; break;
            case 12: familyName = "서"; break;
            case 13: familyName = "신"; break;
            case 14: familyName = "권"; break;
            case 15: familyName = "황"; break;
            case 16: familyName = "안"; break;
            case 17: familyName = "송"; break;
            case 18: familyName = "전"; break;
            case 19: familyName = "홍"; break;
            case 20: familyName = "유"; break;
            case 21: familyName = "고"; break;
            case 22: familyName = "문"; break;
            case 23: familyName = "양"; break;
            case 24: familyName = "손"; break;
            case 25: familyName = "배"; break;
            case 26: familyName = "조"; break;
            case 27: familyName = "백"; break;
            case 28: familyName = "허"; break;
            case 29: familyName = "유"; break;
            case 30: familyName = "남"; break;
            case 31: familyName = "심"; break;
            case 32: familyName = "노"; break;
            case 33: familyName = "정"; break;
            case 34: familyName = "하"; break;
            case 35: familyName = "곽"; break;
            case 36: familyName = "성"; break;
            case 37: familyName = "차"; break;
            case 38: familyName = "주"; break;
            case 39: familyName = "우"; break;
            case 40: familyName = "구"; break;
            case 41: familyName = "신"; break;
            case 42: familyName = "임"; break;
            case 43: familyName = "전"; break;
            case 44: familyName = "민"; break;
            case 45: familyName = "유"; break;
            case 46: familyName = "류"; break;
            case 47: familyName = "나"; break;
            case 48: familyName = "진"; break;
            case 49: familyName = "지"; break;
            case 50: familyName = "엄"; break;
            case 51: familyName = "채"; break;
            case 52: familyName = "원"; break;
            case 53: familyName = "천"; break;
            case 54: familyName = "방"; break;
            case 55: familyName = "공"; break;
            case 56: familyName = "강"; break;
            case 57: familyName = "현"; break;
            case 58: familyName = "함"; break;
            case 59: familyName = "변"; break;
            case 60: familyName = "염"; break;
            case 61: familyName = "양"; break;
            case 62: familyName = "변"; break;
            case 63: familyName = "여"; break;
            case 64: familyName = "추"; break;
            case 65: familyName = "노"; break;
            case 66: familyName = "도"; break;
            case 67: familyName = "소"; break;
            case 68: familyName = "신"; break;
            case 69: familyName = "석"; break;
            case 70: familyName = "선"; break;
            case 71: familyName = "설"; break;
            case 72: familyName = "마"; break;
            case 73: familyName = "길"; break;
            case 74: familyName = "주"; break;
            case 75: familyName = "연"; break;
            case 76: familyName = "방"; break;
            case 77: familyName = "위"; break;
            case 78: familyName = "표"; break;
            case 79: familyName = "명"; break;
            case 80: familyName = "기"; break;
            case 81: familyName = "반"; break;
            case 82: familyName = "라"; break;
            case 83: familyName = "왕"; break;
            case 84: familyName = "금"; break;
            case 85: familyName = "옥"; break;
            case 86: familyName = "육"; break;
            case 87: familyName = "인"; break;
            case 88: familyName = "맹"; break;
            case 89: familyName = "제"; break;
            case 90: familyName = "모"; break;
            case 91: familyName = "장"; break;
            case 92: familyName = "남궁"; break;
            case 93: familyName = "탁"; break;
            case 94: familyName = "국"; break;
            case 95: familyName = "여"; break;
            case 96: familyName = "진"; break;
            case 97: familyName = "어"; break;
            case 98: familyName = "은"; break;
            case 99: familyName = "편"; break;
            default: familyName = "김"; break;
        }

        //Debug.Log($"Random Index: {randomIndex}");
        return familyName;
    }


    public static string GetRandomGivenName()
    {
        string givenName;

        int randomIndex = Random.Range(0, 100);

        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 199, 199);

        randomIndex = Mathf.RoundToInt(curve.Evaluate(randomIndex));

        switch (randomIndex)
        {
            case 0: givenName = "서연"; break;
            case 1: givenName = "지우"; break;
            case 2: givenName = "서윤"; break;
            case 3: givenName = "민서"; break;
            case 4: givenName = "서현"; break;
            case 5: givenName = "하윤"; break;
            case 6: givenName = "유진"; break;
            case 7: givenName = "지민"; break;
            case 8: givenName = "윤서"; break;
            case 9: givenName = "하은"; break;
            case 10: givenName = "지원"; break;
            case 11: givenName = "수빈"; break;
            case 12: givenName = "지윤"; break;
            case 13: givenName = "지안"; break;
            case 14: givenName = "채원"; break;
            case 15: givenName = "지유"; break;
            case 16: givenName = "수아"; break;
            case 17: givenName = "민지"; break;
            case 18: givenName = "지아"; break;
            case 19: givenName = "은서"; break;
            case 20: givenName = "다은"; break;
            case 21: givenName = "서아"; break;
            case 22: givenName = "예은"; break;
            case 23: givenName = "예진"; break;
            case 24: givenName = "예원"; break;
            case 25: givenName = "수민"; break;
            case 26: givenName = "서영"; break;
            case 27: givenName = "하린"; break;
            case 28: givenName = "유나"; break;
            case 29: givenName = "예린"; break;
            case 30: givenName = "수연"; break;
            case 31: givenName = "소윤"; break;
            case 32: givenName = "채은"; break;
            case 33: givenName = "시아"; break;
            case 34: givenName = "아린"; break;
            case 35: givenName = "시은"; break;
            case 36: givenName = "윤아"; break;
            case 37: givenName = "은채"; break;
            case 38: givenName = "가은"; break;
            case 39: givenName = "이서"; break;
            case 40: givenName = "예서"; break;
            case 41: givenName = "아윤"; break;
            case 42: givenName = "소율"; break;
            case 43: givenName = "나은"; break;
            case 44: givenName = "유빈"; break;
            case 45: givenName = "지은"; break;
            case 46: givenName = "다연"; break;
            case 47: givenName = "지현"; break;
            case 48: givenName = "유주"; break;
            case 49: givenName = "연서"; break;
            case 50: givenName = "채윤"; break;
            case 51: givenName = "다인"; break;
            case 52: givenName = "서하"; break;
            case 53: givenName = "하연"; break;
            case 54: givenName = "주아"; break;
            case 55: givenName = "서은"; break;
            case 56: givenName = "예나"; break;
            case 57: givenName = "지수"; break;
            case 58: givenName = "윤지"; break;
            case 59: givenName = "현지"; break;
            case 60: givenName = "시연"; break;
            case 61: givenName = "소연"; break;
            case 62: givenName = "예지"; break;
            case 63: givenName = "혜원"; break;
            case 64: givenName = "나연"; break;
            case 65: givenName = "지연"; break;
            case 66: givenName = "수진"; break;
            case 67: givenName = "유림"; break;
            case 68: givenName = "채린"; break;
            case 69: givenName = "가연"; break;
            case 70: givenName = "예림"; break;
            case 71: givenName = "나현"; break;
            case 72: givenName = "나영"; break;
            case 73: givenName = "은지"; break;
            case 74: givenName = "민경"; break;
            case 75: givenName = "소현"; break;
            case 76: givenName = "다현"; break;
            case 77: givenName = "하영"; break;
            case 78: givenName = "도연"; break;
            case 79: givenName = "주은"; break;
            case 80: givenName = "가현"; break;
            case 81: givenName = "가영"; break;
            case 82: givenName = "유정"; break;
            case 83: givenName = "서희"; break;
            case 84: givenName = "채연"; break;
            case 85: givenName = "채영"; break;
            case 86: givenName = "주연"; break;
            case 87: givenName = "소희"; break;
            case 88: givenName = "유민"; break;
            case 89: givenName = "민채"; break;
            case 90: givenName = "정원"; break;
            case 91: givenName = "보민"; break;
            case 92: givenName = "세은"; break;
            case 93: givenName = "규리"; break;
            case 94: givenName = "아영"; break;
            case 95: givenName = "태희"; break;
            case 96: givenName = "주희"; break;
            case 97: givenName = "사랑"; break;
            case 98: givenName = "수정"; break;
            case 99: givenName = "소영"; break;
            default: givenName = "서연"; break;
        }

        //Debug.Log($"Random Index: {randomIndex}");
        return givenName;
    }

}
