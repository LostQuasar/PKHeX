using System;
using System.Collections.Generic;
using System.Text;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PKHeX.Core;

public sealed class TrainerCard8 : SaveBlock<SAV8SWSH>
{
    public TrainerCard8(SAV8SWSH sav, SCBlock block) : base (sav, block.Data) { }

    private Span<byte> OT_Trash => Data.AsSpan(0x00, 0x1A);

    public string OT
    {
        get => SAV.GetString(OT_Trash);
        set => SAV.SetString(OT_Trash, value.AsSpan(), SAV.OTLength, StringConverterOption.ClearZero);
    }

    public byte Language
    {
        get => Data[0x1B];
        set => Data[0x1B] = value; // languageID
    }

    public int TrainerID
    {
        get => ReadInt32LittleEndian(Data.AsSpan(0x1C));
        set => WriteInt32LittleEndian(Data.AsSpan(0x1C), value);
    }

    public ushort PokeDexOwned
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(Offset + 0x20));
        set => WriteUInt16LittleEndian(Data.AsSpan(Offset + 0x20), value);
    }

    public ushort ShinyPokemonFound
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(Offset + 0x22));
        set => WriteUInt16LittleEndian(Data.AsSpan(Offset + 0x22), value);
    }

    public byte Game
    {
        get => Data[0x24];
        set => Data[0x24] = value; // 0 = Sword, 1 = Shield
    }

    public byte Starter
    {
        get => Data[0x25];
        set => Data[0x25] = value; // Grookey=0, Scorbunny=1, Sobble=2
    }

    public ushort CurryTypesOwned
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(Offset + 0x26));
        set => WriteUInt16LittleEndian(Data.AsSpan(Offset + 0x26), value);
    }

    public const int RotoRallyScoreMax = 99_999;

    public int RotoRallyScore
    {
        get => ReadInt32LittleEndian(Data.AsSpan(0x28));
        set
        {
            if (value > RotoRallyScoreMax)
                value = RotoRallyScoreMax;
            WriteInt32LittleEndian(Data.AsSpan(0x28), value);
            // set to the other block since it doesn't have an accessor
            SAV.SetValue(SaveBlockAccessor8SWSH.KRotoRally, (uint)value);
        }
    }

    public const int MaxPokemonCaught = 99_999;

    public int CaughtPokemon
    {
        get => ReadInt32LittleEndian(Data.AsSpan(0x2C));
        set
        {
            if (value > MaxPokemonCaught)
                value = MaxPokemonCaught;
            WriteInt32LittleEndian(Data.AsSpan(0x2C), value);
        }
    }

    public bool PokeDexComplete
    {
        get => Data[Offset + 0x30] == 1;
        set => Data[Offset + 0x30] = value ? (byte)1 : (byte)0;
    }

    public bool ArmorDexComplete
    {
        get => Data[Offset + 0x1B4] == 1;
        set => Data[Offset + 0x1B4] = value ? (byte)1 : (byte)0;
    }

    public bool CrownDexComplete
    {
        get => Data[Offset + 0x1B5] == 1;
        set => Data[Offset + 0x1B5] = value ? (byte)1 : (byte)0;
    }

    public int Gender
    {
        get => Data[0x38];
        set => Data[0x38] = (byte)value;
    }

    public string Number
    {
        get => Encoding.ASCII.GetString(Data, 0x39, 3);
        set
        {
            for (int i = 0; i < 3; i++)
                Data[0x39 + i] = (byte) (value.Length > i ? value[i] : '\0');
            SAV.State.Edited = true;
        }
    }

    public ulong Skin // aka the base model
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x40));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x40), value);
    }

    public ulong Hair
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x48));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x48), value);
    }

    public ulong Brow
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x50));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x50), value);
    }

    public ulong Lashes
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x58));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x58), value);
    }

    public ulong Contacts
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x60));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x60), value);
    }

    public ulong Lips
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x68));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x68), value);
    }

    public ulong Glasses
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x70));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x70), value);
    }

    public ulong Hat
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x78));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x78), value);
    }

    public ulong Jacket
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x80));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x80), value);
    }

    public ulong Top
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x88));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x88), value);
    }

    public ulong Bag
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x90));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x90), value);
    }

    public ulong Gloves
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0x98));
        set => WriteUInt64LittleEndian(Data.AsSpan(0x98), value);
    }

    public ulong BottomOrDress
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0xA0));
        set => WriteUInt64LittleEndian(Data.AsSpan(0xA0), value);
    }

    public ulong Sock
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0xA8));
        set => WriteUInt64LittleEndian(Data.AsSpan(0xA8), value);
    }

    public ulong Shoe
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0xB0));
        set => WriteUInt64LittleEndian(Data.AsSpan(0xB0), value);
    }

    public ulong MomSkin // aka the base model
    {
        get => ReadUInt64LittleEndian(Data.AsSpan(0xC0));
        set => WriteUInt64LittleEndian(Data.AsSpan(0xC0), value);
    }

    // Trainer Card Pokemon
    // 0xC8 - 0xE3 (0x1C)
    // 0xE4
    // 0x100
    // 0x11C
    // 0x138
    // 0x154 - 0x16F

    /// <summary>
    /// Gets an object that exposes the data of the corresponding party <see cref="index"/>.
    /// </summary>
    public TrainerCard8Poke ViewPoke(int index)
    {
        if ((uint) index >= 6)
            throw new ArgumentOutOfRangeException(nameof(index));
        return new TrainerCard8Poke(Data, Offset + 0xC8 + (index * TrainerCard8Poke.SIZE));
    }

    /// <summary>
    /// Applies the current <see cref="SaveFile.PartyData"/> to the block.
    /// </summary>
    public void SetPartyData() => LoadTeamData(SAV.PartyData);

    public void LoadTeamData(IList<PKM> party)
    {
        for (int i = 0; i < party.Count; i++)
            ViewPoke(i).LoadFrom(party[i]);
        for (int i = party.Count; i < 6; i++)
            ViewPoke(i).Clear();
    }

    public ushort StartedYear
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(Offset + 0x170));
        set => WriteUInt16LittleEndian(Data.AsSpan(Offset + 0x170), value);
    }

    public byte StartedMonth
    {
        get => Data[Offset + 0x172];
        set => Data[Offset + 0x172] = value;
    }

    public byte StartedDay
    {
        get => Data[Offset + 0x173];
        set => Data[Offset + 0x173] = value;
    }

    public uint TimestampPrinted
    {
        // should this be a ulong?
        get => ReadUInt32LittleEndian(Data.AsSpan(Offset + 0x1A8));
        set => WriteUInt32LittleEndian(Data.AsSpan(Offset + 0x1A8), value);
    }
}

public sealed class TrainerCard8Poke : ISpeciesForm
{
    public const int SIZE = 0x1C;
    private readonly byte[] Data;
    private readonly int Offset;

    public TrainerCard8Poke(byte[] data, int offset)
    {
        Data = data;
        Offset = offset;
    }

    public ushort Species
    {
        get => ReadUInt16LittleEndian(Data.AsSpan(Offset + 0x00));
        set => WriteInt32LittleEndian(Data.AsSpan(Offset + 0x00), value);
    }

    public byte Form
    {
        get => Data[Offset + 0x04];
        set => WriteInt32LittleEndian(Data.AsSpan(Offset + 0x04), value);
    }

    public int Gender
    {
        get => ReadInt32LittleEndian(Data.AsSpan(Offset + 0x08));
        set => WriteInt32LittleEndian(Data.AsSpan(Offset + 0x08), value);
    }

    public bool IsShiny
    {
        get => Data[Offset + 0xC] != 0;
        set => Data[Offset + 0xC] = value ? (byte)1 : (byte)0;
    }

    public uint EncryptionConstant
    {
        get => ReadUInt32LittleEndian(Data.AsSpan(Offset + 0x10));
        set => WriteUInt32LittleEndian(Data.AsSpan(Offset + 0x10), value);
    }

    public uint Unknown
    {
        get => ReadUInt32LittleEndian(Data.AsSpan(Offset + 0x14));
        set => WriteUInt32LittleEndian(Data.AsSpan(Offset + 0x14), value);
    }

    public int FormArgument
    {
        get => ReadInt32LittleEndian(Data.AsSpan(Offset + 0x18));
        set => WriteInt32LittleEndian(Data.AsSpan(Offset + 0x18), value);
    }

    public void Clear() => Array.Clear(Data, Offset, SIZE);

    public void LoadFrom(PKM pk)
    {
        Species = pk.Species;
        Form = pk.Form;
        Gender = pk.Gender;
        IsShiny = pk.IsShiny;
        EncryptionConstant = pk.EncryptionConstant;
        FormArgument = pk is IFormArgument f && pk.Species == (int) Core.Species.Alcremie ? (int)f.FormArgument : -1;
    }

    public void LoadFrom(TitleScreen8Poke pk)
    {
        Species = pk.Species;
        Form = pk.Form;
        Gender = pk.Gender;
        IsShiny = pk.IsShiny;
        EncryptionConstant = pk.EncryptionConstant;
        FormArgument = pk.FormArgument;
    }
}
