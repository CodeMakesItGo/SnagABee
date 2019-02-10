using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ZigbeeDecoder
{
    internal class YuneecDecoder
    {
        private List<DecoderGroup> DecoderGroups { get; } = new List<DecoderGroup>();

        public bool GetValue(ushort sourceId, string name, out float value)
        {
            value = 0.0f;
            foreach (var decoderGroup in DecoderGroups)
            {
                if (decoderGroup.Header.Src != sourceId)
                {
                    continue;
                }

                foreach (var decoderInfo in decoderGroup.DecoderInfoList)
                {
                    if (!decoderInfo.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }

                    value = decoderInfo.DecoderValue.Value;
                    return true;
                }
            }

            return false;
        }

        public void DecodeMessage(byte[] message, ZigBeeDecoder.ZigBeeHeader header)
        {
            foreach (var decoderGroup in DecoderGroups)
            {
                if (!decoderGroup.Header.Equals(header))
                {
                    continue;
                }

                foreach (var decoderInfo in decoderGroup.DecoderInfoList)
                {
                    DecodeValue(message, decoderInfo);
                }

                break;
            }
        }

        private void DecodeValue(byte[] bytes, DecoderInfo decoderInfo)
        {
            uint rtn = 0;

            var bitLength = decoderInfo.BitLength;
            var bitOffset = decoderInfo.StartBit;
            var signed = decoderInfo.Signed;
            var factor = decoderInfo.Factor;

            //Pass the number of bits over to our rtn value
            for (var i = 0; i < bitLength; ++i)
            {
                var b = bytes[(bitOffset + i) / 8];

                if ((b & (1 << (7 - (bitOffset + i) % 8))) > 0)
                {
                    rtn |= (uint) 1 << (bitLength - 1 - i);
                }
            }

            decoderInfo.DecoderValue.Raw = rtn;

            //byte swap for big endian
            if (bitLength % 8 == 0 && bitLength > 8)
            {
                uint byteSwapped = 0;
                var swaps = bitLength / 8;

                for (var i = 0; i < swaps; ++i)
                {
                    byteSwapped += rtn & 0xFF;
                    rtn = rtn >> 8;

                    if (i < swaps - 1)
                    {
                        byteSwapped = byteSwapped << 8;
                    }
                }

                rtn = byteSwapped;
            }

            if (signed)
            {
                if (bitLength == 8)
                {
                    decoderInfo.DecoderValue.Value = (sbyte) rtn * factor;
                }

                if (bitLength == 16)
                {
                    decoderInfo.DecoderValue.Value = (short) rtn * factor;
                }

                if (bitLength == 32)
                {
                    decoderInfo.DecoderValue.Value = (int) rtn * factor;
                }
            }
            else
            {
                decoderInfo.DecoderValue.Value = rtn * factor;
            }
        }

        public bool LoadSettings()
        {
            var ofd = new OpenFileDialog {Filter = @"*.csv|*.csv", Multiselect = false};

            DecoderGroups.Clear();

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return DecoderGroups.Count > 0;
            }

            DecoderGroup dg = null;

            using (var sr = new StreamReader(ofd.FileName))
            {
                while (sr.EndOfStream == false)
                {
                    var line = sr.ReadLine();
                    if (line == null)
                    {
                        continue;
                    }

                    var items = line.Split(',');

                    if (items.Length == 6) //New Decode group
                    {
                        if (dg != null)
                        {
                            DecoderGroups.Add(dg);
                        }

                        //"Length,Sequence Number,Source Address,Source PAN ID,Destination PAN ID,Destination Address"
                        dg = new DecoderGroup
                        {
                            Header = new ZigBeeDecoder.ZigBeeHeader
                            {
                                Len = byte.Parse(items[0]),
                                SequenceNum = byte.Parse(items[1]),
                                Src = ushort.Parse(items[2]),
                                Pan = ushort.Parse(items[3]),
                                DestPan = ushort.Parse(items[4]),
                                DestAddr = ushort.Parse(items[5])
                            }
                        };

                    }

                    if (items.Length == 9) //new Decode info
                    {
                        //"Name,Start Bit,Bit Length,Start Byte,Byte Length,Signed,Factor,Unit,Notes"
                        var di = new DecoderInfo
                        {
                            Name = items[0],
                            StartBit = int.Parse(items[1]),
                            BitLength = int.Parse(items[2]),
                            StartByte = int.Parse(items[3]),
                            ByteLength = float.Parse(items[4]),
                            Signed = bool.Parse(items[5]),
                            Factor = float.Parse(items[6]),
                            Unit = items[7],
                            Notes = items[8]
                        };

                        dg?.DecoderInfoList.Add(di);
                    }
                }

                DecoderGroups.Add(dg);
            }

            return DecoderGroups.Count > 0;
        }

        public class DecoderGroup
        {
            public ZigBeeDecoder.ZigBeeHeader Header { get; set; }
            public List<DecoderInfo> DecoderInfoList { get; } = new List<DecoderInfo>();
        }

        public class DecoderInfo
        {
            public readonly DecoderValue DecoderValue = new DecoderValue();
            public string Name { get; set; }
            public int StartBit { get; set; }
            public int BitLength { get; set; }
            public int StartByte { get; set; }
            public float ByteLength { get; set; }
            public bool Signed { get; set; }
            public float Factor { get; set; }
            public string Unit { get; set; }
            public string Notes { get; set; }
        }

        public class DecoderValue
        {
            public float Value { get; set; }
            public uint Raw { get; set; }
            public string RawString => $"0x{Raw:X2}";
        }
    }
}