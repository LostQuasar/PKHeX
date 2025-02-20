﻿using System;
using static PKHeX.Core.LanguageID;

namespace PKHeX.Core;

/// <summary>
/// Logic relating to <see cref="LanguageID"/> values.
/// </summary>
public static class Language
{
    private static readonly byte[] Languages =
    {
        (byte)Japanese,
        (byte)English,
        (byte)French,
        (byte)German,
        (byte)Spanish,
        (byte)Italian,

        (byte)Korean, // GS

        (byte)ChineseS,
        (byte)ChineseT,
    };

    // check Korean for the VC case, never possible to match string outside of this case
    private static readonly byte[] Languages_GB = Languages.AsSpan(0, 7).ToArray(); // [..KOR]
    private static readonly byte[] Languages_3  = Languages.AsSpan(0, 6).ToArray(); // [..KOR)
    private const LanguageID SafeLanguage = English;

    public static ReadOnlySpan<byte> GetAvailableGameLanguages(int generation = PKX.Generation) => generation switch
    {
        1           => Languages_3, // No KOR
        2           => Languages_GB,
        3           => Languages_3, // No KOR
        4 or 5 or 6 => Languages_GB,
        _           => Languages,
    };

    private static bool HasLanguage(byte[] permitted, byte language)
    {
        int index = Array.IndexOf(permitted, language);
        return index != -1;
    }

    public static LanguageID GetSafeLanguage(int generation, LanguageID prefer, GameVersion game = GameVersion.Any) => generation switch
    {
        1 when game == GameVersion.BU => Japanese,
        1           => HasLanguage(Languages_3,  (byte)prefer) ? prefer : SafeLanguage,
        2           => HasLanguage(Languages_GB, (byte)prefer) && (prefer != Korean || game == GameVersion.C) ? prefer : SafeLanguage,
        3           => HasLanguage(Languages_3 , (byte)prefer) ? prefer : SafeLanguage,
        4 or 5 or 6 => HasLanguage(Languages_GB, (byte)prefer) ? prefer : SafeLanguage,
        _           => HasLanguage(Languages,    (byte)prefer) ? prefer : SafeLanguage,
    };

    public static string GetLanguage2CharName(this LanguageID language) => language switch
    {
        Japanese => "ja",
        French => "fr",
        Italian => "it",
        German => "de",
        Spanish => "es",
        Korean => "ko",
        ChineseS or ChineseT => "zh",
        _ => GameLanguage.DefaultLanguage,
    };

    /// <summary>
    /// Gets the Main Series language ID from a GameCube (C/XD) language ID.
    /// </summary>
    /// <param name="value">GameCube (C/XD) language ID.</param>
    /// <returns>Main Series language ID.</returns>
    /// <remarks>If no conversion is possible or maps to the same value, the input <see cref="value"/> is returned.</remarks>
    public static byte GetMainLangIDfromGC(byte value) => (LanguageGC)value switch
    {
        LanguageGC.German =>   (byte)German,
        LanguageGC.French =>   (byte)French,
        LanguageGC.Italian =>  (byte)Italian,
        LanguageGC.Spanish =>  (byte)Spanish,
        LanguageGC.UNUSED_6 => (byte)UNUSED_6,
        _ => value,
    };

    /// <summary>
    /// Gets the GameCube (C/XD) language ID from a Main Series language ID.
    /// </summary>
    /// <param name="value">Main Series language ID.</param>
    /// <returns>GameCube (C/XD) language ID.</returns>
    /// <remarks>If no conversion is possible or maps to the same value, the input <see cref="value"/> is returned.</remarks>
    public static byte GetGCLangIDfromMain(byte value) => (LanguageID)value switch
    {
        French =>   (byte)LanguageGC.French,
        Italian =>  (byte)LanguageGC.Italian,
        German =>   (byte)LanguageGC.German,
        UNUSED_6 => (byte)LanguageGC.UNUSED_6,
        Spanish =>  (byte)LanguageGC.Spanish,
        _ => value,
    };
}
