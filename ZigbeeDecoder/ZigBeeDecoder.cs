using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ZigbeeDecoder
{
    class ZigBeeDecoder
    {
        public class Conversion
        {
            public static byte[] Message2Bytes(string message)
            {
                var values = message.Split(' ');
                var bytes = new byte[values.Length];

                for(var i = 0; i < values.Length; ++i)
                {
                    if(byte.TryParse(values[i].Trim(), NumberStyles.HexNumber, null, out bytes[i]) == false)
                    {
                        bytes[i] = 0;
                    }
                }

                return bytes;
            }

            public static string ByteArrayToString(byte[] ba, int offset)
            {
                var hex = new StringBuilder(ba.Length * 2 - offset);

                for (var i = offset; i < ba.Length; ++i)
                {
                    hex.AppendFormat("{0:x2} ", ba[i]);
                }

                return hex.ToString();
            }

            protected static byte Big2Little8(IReadOnlyList<byte> data, ref int bitOffset)
            {
                var rtn = ((data[(bitOffset / 8) + 0] << 0));
                bitOffset += 8;
                return (byte)rtn;
            }

            protected static ushort Big2Little16(IReadOnlyList<byte> data, ref int bitOffset)
            {
                var rtn = ((data[(bitOffset / 8) + 1] << 8) |
                           (data[(bitOffset / 8) + 0] << 0));
                bitOffset += 16;
                return (ushort)rtn;
            }

            protected static uint Big2Little32(IReadOnlyList<byte> data, ref int bitOffset)
            {
                var rtn = ((data[(bitOffset / 8) + 3] << 24) |
                           (data[(bitOffset / 8) + 2] << 16) |
                           (data[(bitOffset / 8) + 1] << 8) |
                           (data[(bitOffset / 8) + 0] << 0));
                bitOffset += 32;
                return (uint)rtn;
            }

            protected static ulong Big2Little64(IReadOnlyList<byte> data, ref int bitOffset)
            {
                var rtn = ((data[(bitOffset / 8) + 7] << 56) |
                           (data[(bitOffset / 8) + 6] << 48) |
                           (data[(bitOffset / 8) + 5] << 40) |
                           (data[(bitOffset / 8) + 4] << 32) |
                           (data[(bitOffset / 8) + 3] << 24) |
                           (data[(bitOffset / 8) + 2] << 16) |
                           (data[(bitOffset / 8) + 1] << 8) |
                           (data[(bitOffset / 8) + 0] << 0));
                bitOffset += 64;
                return (ulong)rtn;
            }

            protected static float Big2LittleFloat(byte[] data, ref int bitOffset)
            {
                var i = (bitOffset / 8);
                byte[] b = new[] { data[i + 0], data[i + 1], data[i + 2], data[i + 3] };

                var rtn = BitConverter.ToSingle(b, 0);
                bitOffset += 32;
                return rtn;
            }
        }

        public class PacketSnifferHeader : Conversion
        {
            public byte Info { get; private set; }
            public uint PacketNumber { get; private set; }
            public ulong TimeStamp { get; private set; }
            public ushort Length { get; private set; }
            public const int Size = 15;

            public static bool TryParse(byte[] data, out PacketSnifferHeader header, int byteOffset = 0)
            {
                header = null;
                if (data.Length - byteOffset < Size) return false;

                var bitOffset = byteOffset * 8;
                header = new PacketSnifferHeader()
                {
                    Info = Big2Little8(data, ref bitOffset),
                    PacketNumber = Big2Little32(data, ref bitOffset),
                    TimeStamp = Big2Little64(data, ref bitOffset),
                    Length = Big2Little16(data, ref bitOffset),
                };

                return true;
            }

            public override string ToString()
            {
                return
                    $"Info = {Info},Packet Number = {PacketNumber},Time Stamp = {TimeStamp},Length = {Length}";
            }
        }

        public class ZigBeeHeader : Conversion
        {
            public byte Len { get;  set; }
            public ushort Pan { get;  set; }
            public byte SequenceNum { get;  set; }
            public ushort DestPan { get;  set; }
            public ushort DestAddr { get;  set; }
            public ushort Src { get;  set; }
            public const int Size = 10;

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public static bool operator ==(ZigBeeHeader hdr1, ZigBeeHeader hdr2)
            {
                if (ReferenceEquals(hdr1, hdr2))
                {
                    return true;
                }

                if (ReferenceEquals(hdr1, null))
                {
                    return false;
                }

                if (ReferenceEquals(hdr2, null))
                {
                    return false;
                }

                return hdr1.Len == hdr2.Len &&
                       hdr1.Pan == hdr2.Pan &&
                       hdr1.DestPan == hdr2.DestPan &&
                       hdr1.DestAddr == hdr2.DestAddr &&
                       hdr1.Src == hdr2.Src;
            }

            public static bool operator !=(ZigBeeHeader obj1, ZigBeeHeader obj2)
            {
                return !(obj1 == obj2);
            }

            public static bool TryParse(byte[] data, out ZigBeeHeader header, int byteOffset = 0)
            {
                header = null;
                if (data.Length - byteOffset < Size) return false;

                var bitOffset = byteOffset * 8;
                header = new ZigBeeHeader()
                {
                    Len = Big2Little8(data, ref bitOffset),
                    Pan = Big2Little16(data, ref bitOffset),
                    SequenceNum = Big2Little8(data, ref bitOffset),
                    DestPan = Big2Little16(data, ref bitOffset),
                    DestAddr = Big2Little16(data, ref bitOffset),
                    Src = Big2Little16(data, ref bitOffset),
                };

                return true;
            }

            public override string ToString()
            {
                return
                    $"Length = {Len},PAN ID = {Pan},Packet Number = {SequenceNum},Destination PAN Address = {DestPan},Destination Address = {DestAddr},Source Address = {Src}";
            }
        }
    }
}
