using UnityEngine;

namespace Platformer.Datum
{
    [CreateAssetMenu(fileName = "new SkillCastSetting", menuName = "Platformer/ScriptableObjects/SkillCastSetting")]
    public class SkillCastSetting : ScriptableObject
    {
        public int targetMax; // �ִ� Ÿ���� ��
        public LayerMask targetMask; // Ÿ�� ���� ����ũ 
        public float damageGain; // ���� ���
        public Vector2 castCenter; // Ÿ�� ���� ����(�簢��) ���� �߽�
        public Vector2 castSize; // Ÿ�� ���� ���� ũ��
        public float castDistance; // Ÿ�� ���� ���� �� �Ÿ� 
    }
}