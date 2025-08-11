using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int phaseID; // Phase mới khi player chạm checkpoint này
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (triggered) return; // Đã kích hoạt rồi thì bỏ qua

        if (col.CompareTag("Player"))
        {
            triggered = true; // Đánh dấu đã kích hoạt
            PhaseManager.Instance.SwitchToPhase(phaseID);

            // Tuỳ chọn: tắt collider để không bị trigger lại
            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null) collider.enabled = false;

            // Hoặc tắt script luôn
            // enabled = false;
        }
    }
}
