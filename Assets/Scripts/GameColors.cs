using UnityEngine;

public static class GameColors
{
    // ── HP Bar ──────────────────────────────
    public static readonly Color HPFill = HexToColor("#C8001E");
    public static readonly Color HPBG = HexToColor("#1A0005");

    // ── Text ────────────────────────────────
    public static readonly Color TextHP = HexToColor("#FFD0C0");
    public static readonly Color TextLevel = HexToColor("#E8C840");

    // ── Level Badge ─────────────────────────
    public static readonly Color BadgeBG = HexToColor("#1A3A6A");
    public static readonly Color BadgeText = HexToColor("#88CCFF");

    // ── HP theo ngưỡng ──────────────────────
    public static readonly Color HPHigh = HexToColor("#C8001E");
    public static readonly Color HPMid = HexToColor("#D95900");
    public static readonly Color HPLow = HexToColor("#FF2200");

    // ── Damage Text ─────────────────────────
    public static readonly Color DamageNormal = HexToColor("#F5F5F5");
    public static readonly Color DamageCrit = HexToColor("#FFD700");

    // ── Rarity ──────────────────────────────
    public static readonly string Common = "#C8C8C8";
    public static readonly string Uncommon = "#437A22";
    public static readonly string Rare = "#006494";
    public static readonly string Epic = "#7A39BB";
    public static readonly string Legendary = "#E8C840";
    public static readonly string SkillCardColor = "#8B0000";

    // ── Soul ──────────────────────────────
    public static readonly Color Soul = HexToColor("#8A7DFF");
    public static readonly Color SoulDark = HexToColor("#3C5CFF");
    public static readonly Color SoulCorrupted = HexToColor("#5A4B8A");
    public static readonly Color SoulEffect = HexToColor("#00FFFF");
    public static readonly Color SoulOrb = HexToColor("#7DE7FF");

    // ── Stats Panel ─────────────────────────
    public static readonly Color StatHeader = HexToColor("#1A0A00");
    public static readonly Color StatLabel = HexToColor("#2E1A0A");
    public static readonly Color StatValue = HexToColor("#8B1A1A");
    public static readonly Color StatBuffed = HexToColor("#E8C840");
    public static readonly Color StatDebuffed = HexToColor("#C8001E");

    // ── Helper ──────────────────────────────
    public static Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}