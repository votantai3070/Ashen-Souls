using UnityEngine;

public static class GameColors
{
    // ── HP Bar ──────────────────────────────
    public static readonly Color HPFill = HexToColor("#C8001E"); // đỏ máu
    public static readonly Color HPBG = HexToColor("#1A0005"); // đen đỏ

    // ── Text ────────────────────────────────
    public static readonly Color TextHP = HexToColor("#FFD0C0"); // trắng ấm
    public static readonly Color TextLevel = HexToColor("#E8C840"); // vàng

    // ── Level Badge ─────────────────────────
    public static readonly Color BadgeBG = HexToColor("#1A3A6A"); // xanh thép đậm
    public static readonly Color BadgeText = HexToColor("#88CCFF"); // xanh sáng nhạt

    // ── HP theo ngưỡng ──────────────────────
    public static readonly Color HPHigh = HexToColor("#C8001E"); // > 50% — đỏ máu
    public static readonly Color HPMid = HexToColor("#D95900"); // 25–50% — cam tối
    public static readonly Color HPLow = HexToColor("#FF2200"); // < 25% — đỏ cam báo động

    // ── Rarity (giữ nguyên hệ thống cũ) ────
    public static readonly Color Common = HexToColor("#C8C8C8");
    public static readonly Color Uncommon = HexToColor("#437A22");
    public static readonly Color Rare = HexToColor("#006494");
    public static readonly Color Epic = HexToColor("#7A39BB");
    public static readonly Color Legendary = HexToColor("#E8C840");

    // ── Helper ──────────────────────────────
    private static Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}