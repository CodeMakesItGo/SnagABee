using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;


namespace ZigbeeDecoder
{
    public partial class Form1 : Form
    {
        private readonly UdpClient _udpServer = new UdpClient(5000);
        private DataTable _decodeDataTable;
        private DataTable _messageDataTable;

        private bool _pauseUpdates;
        private bool _threadRunning = true;
        private Thread _updThread;

        public Form1()
        {
            InitializeComponent();
            CreateMessageTable();
            CreateDecodeTable();

            //Remove when using real data
            CreateTestData();
        }

        private byte[] SelectedMessage { get; set; }
        private ZigBeeDecoder.ZigBeeHeader SelectedMessageHeader { get; set; }
        private List<YuneecDecoder.DecoderGroup> DecoderGroups { get; } = new List<YuneecDecoder.DecoderGroup>();
        private string DecoderFileName { get; set; }

        private void CreateMessageTable()
        {
            _messageDataTable = new DataTable("MessageTable");

            _messageDataTable.AddNewColumn("Count");
            _messageDataTable.AddNewColumn("Time");
            _messageDataTable.AddNewColumn("Message");

            dataGridView_messages.DataSource = _messageDataTable;
        }

        private void CreateDecodeTable()
        {
            //Reload table data here with saved decode values
            _decodeDataTable = new DataTable("DecodeTable");

            _decodeDataTable.AddNewColumn("Name");
            _decodeDataTable.AddNewColumn("Start Bit");
            _decodeDataTable.AddNewColumn("Bit Length");
            _decodeDataTable.AddNewColumn("Start Byte");
            _decodeDataTable.AddNewColumn("Byte Length");
            _decodeDataTable.AddNewColumn("Signed");
            _decodeDataTable.AddNewColumn("Factor");
            _decodeDataTable.AddNewColumn("Unit");
            _decodeDataTable.AddNewColumn("Notes");
            _decodeDataTable.AddNewColumn("Raw Data");
            _decodeDataTable.AddNewColumn("Value");

            dataGridView_decoded.DataSource = _decodeDataTable;
        }

        private void CreateTestData()
        {         
            var dr = _messageDataTable.NewRow();
            dr["Count"] = "0";
            dr["Time"] = "120";
            dr["Message"] = "22 41 88 84 94 e1 fa 9e 4e 8b 01 02 74 00 3b ff 04 66 00 c4 ff 45 00 32 f3 ff 61 f5 10 05 00 4d e5 ff f3";
            _messageDataTable.Rows.Add(dr);

            dr = _messageDataTable.NewRow();
            dr["Count"] = "0";
            dr["Time"] = "120";
            dr["Message"] = "34 41 88 c4 94 e1 4e 8b fa 9e 34 31 80 08 00 80 08 00 d5 48 00 3a ab b5 88 85 de 14 e6 dd 46 17 eb 90 2b d2 00 00 44 42 bf 00 01 00 ed ff 04 00 00 00 00";
            _messageDataTable.Rows.Add(dr);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBoxLabel_len.SetName("Length");
            textBoxLabel_seq.SetName("Sequence Number");
            textBoxLabel_srcadd.SetName("Source Address");
            textBoxLabel_srcpan.SetName("Source PAN ID");
            textBoxLabel_destpan.SetName("Destination PAN ID");
            textBoxLabel_destadd.SetName("Destination Address");

            _updThread = new Thread(UpdClientThread);
            _updThread.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _threadRunning = false;
            _udpServer.Close();
            _updThread.Abort();
        }

        // Runs on its own thread to get UDP packets from the Texas Instrument broadcaster
        private void UpdClientThread()
        {
            _threadRunning = true;
            while (_threadRunning)
            {
                var remoteEp = new IPEndPoint(IPAddress.Any, 11000);

                var data = _udpServer.Receive(ref remoteEp);

                if (ZigBeeDecoder.PacketSnifferHeader.TryParse(data, out var psh) == false)
                {
                    return;
                }

                if (ZigBeeDecoder.ZigBeeHeader.TryParse(data, out var header, ZigBeeDecoder.PacketSnifferHeader.Size) == false)
                {
                    return;
                }

                var message = ZigBeeDecoder.Conversion.ByteArrayToString(data, ZigBeeDecoder.PacketSnifferHeader.Size);

                //Add new message to group
                if (DecoderGroups.Count(x => x.Header == header) == 0)
                {
                    var dg = new YuneecDecoder.DecoderGroup
                    {
                        Header = header
                    };
                    DecoderGroups.Add(dg);

                    var dr = _messageDataTable.NewRow();
                    dr["Count"] = psh.PacketNumber;
                    dr["Time"] = DateTime.Now.ToString("HH:mm:ss:fff");
                    dr["Message"] = message;
                    _messageDataTable.Rows.Add(dr);
                }

                //Update existing message
                else
                {
                    for (var i = 0; i < _messageDataTable.Rows.Count; i++)
                    {
                        var dr = _messageDataTable.Rows[i];
                        var bytes = ZigBeeDecoder.Conversion.Message2Bytes(dr["Message"] as string);

                        if (ZigBeeDecoder.ZigBeeHeader.TryParse(bytes, out var msgHeader) == false)
                        {
                            continue;
                        }

                        if (header != msgHeader)
                        {
                            continue;
                        }

                        dr["Count"] = psh.PacketNumber;
                        dr["Time"] = DateTime.Now.ToString("HH:mm:ss:fff");
                        dr["Message"] = message;
                        break;
                    }
                }

                if (SelectedMessageHeader != null &&
                    SelectedMessageHeader == header)
                {
                    SelectedMessage = ZigBeeDecoder.Conversion.Message2Bytes(message);
                    UpdateDecodeValues(SelectedMessage);
                }
            }
        }

        #region Update Methods
        //Update the message PAN addresses, common zigbee headers
        private void UpdateHeaderInfo(string message)
        {
            var bytes = ZigBeeDecoder.Conversion.Message2Bytes(message);
            if (ZigBeeDecoder.ZigBeeHeader.TryParse(bytes, out var header) == false)
            {
                return;
            }

            SelectedMessage = bytes;
            SelectedMessageHeader = header;

            textBoxLabel_len.SetValue(header.Len.ToString());
            textBoxLabel_seq.SetValue(header.SequenceNum.ToString());
            textBoxLabel_srcadd.SetValue(header.Src.ToString("X4"));
            textBoxLabel_srcpan.SetValue(header.Pan.ToString("X4"));
            textBoxLabel_destpan.SetValue(header.DestPan.ToString("X4"));
            textBoxLabel_destadd.SetValue(header.DestAddr.ToString("X4"));

            UpdateDecodeInfo(SelectedMessageHeader);
            UpdateDecodeValues(SelectedMessage);
        }

        private void UpdateDecodeInfo(ZigBeeDecoder.ZigBeeHeader header)
        {
            //something has changed, clear old data
            _decodeDataTable.Clear();

            //No decode info is available, create defaults
            if (DecoderGroups.Count(x => x.Header == header) == 0)
            {
                SetDecodeDefaults(header.Len);
            }
            else
            {
                var dg = DecoderGroups.First(x => x.Header == header);

                //No decode info is available, create defaults
                if (dg.DecoderInfoList.Count == 0)
                {
                    SetDecodeDefaults(header.Len);
                }
                else
                {
                    foreach (var di in dg.DecoderInfoList)
                    {
                        var dr = _decodeDataTable.NewRow();
                        dr["Name"] = di.Name;
                        dr["Start Bit"] = di.StartBit;
                        dr["Bit Length"] = di.BitLength;
                        dr["Start Byte"] = di.StartByte;
                        dr["Byte Length"] = di.ByteLength;
                        dr["Signed"] = di.Signed;
                        dr["Factor"] = di.Factor;
                        dr["Unit"] = di.Unit;
                        dr["Notes"] = di.Notes;
                        dr["Raw Data"] = "";
                        dr["Value"] = "";
                        _decodeDataTable.Rows.Add(dr);
                    }
                }
            }
        }

        private void SetDecodeDefaults(byte length)
        {
            for (var i = ZigBeeDecoder.ZigBeeHeader.Size; i < length; ++i)
            {
                var dr = _decodeDataTable.NewRow();
                dr["Name"] = "";
                dr["Start Bit"] = i * 8;
                dr["Bit Length"] = 8;
                dr["Start Byte"] = i;
                dr["Byte Length"] = 1;
                dr["Signed"] = "False";
                dr["Factor"] = 1.0;
                dr["Unit"] = "";
                dr["Notes"] = "";
                dr["Raw Data"] = "";
                dr["Value"] = "";
                _decodeDataTable.Rows.Add(dr);
            }
        }

        private void UpdateDecodeValues(byte[] bytes)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDecodeValuesDelegate(() => UpdateDecodeValues(bytes)));
                return;
            }

            //skip updating the values if the user is changing a value
            if (_pauseUpdates)
            {
                return;
            }

            try
            {
                foreach (DataRow dr in _decodeDataTable.Rows)
                {
                    if (int.TryParse(dr["Start Bit"] as string, out var bitOffset) == false)
                    {
                        return;
                    }

                    if (int.TryParse(dr["Bit Length"] as string, out var bitLength) == false)
                    {
                        return;
                    }

                    if (bool.TryParse(dr["Signed"] as string, out var signed) == false)
                    {
                        return;
                    }

                    if (float.TryParse(dr["Factor"] as string, out var factor) == false)
                    {
                        return;
                    }

                    uint rtn = 0;

                    //Pass the number of bits over to our rtn value
                    for (var i = 0; i < bitLength; ++i)
                    {
                        var b = bytes[(bitOffset + i) / 8];

                        if ((b & (1 << (7 - (bitOffset + i) % 8))) > 0)
                        {
                            rtn |= (uint) 1 << (bitLength - 1 - i);
                        }
                    }

                    dr["Raw Data"] = $"0x{rtn:X2}";

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
                            dr["Value"] = (string) dr["Factor"] == "1" ? $"{(sbyte) rtn}" : $"{(sbyte) rtn * factor}";
                        }

                        if (bitLength == 16)
                        {
                            dr["Value"] = (string) dr["Factor"] == "1" ? $"{(short) rtn}" : $"{(short) rtn * factor}";
                        }

                        if (bitLength == 32)
                        {
                            dr["Value"] = (string) dr["Factor"] == "1" ? $"{(int) rtn}" : $"{(int) rtn * factor}";
                        }
                    }
                    else
                    {
                        dr["Value"] = (string) dr["Factor"] == "1" ? $"{rtn}" : $"{rtn * factor}";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        #endregion

        #region MenuItems
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(DecoderFileName))
            {
                CreateDecoderFile();
            }

            SaveDecoderInfo();

            if (string.IsNullOrEmpty(DecoderFileName)) return;

            using (var sw = new StreamWriter(DecoderFileName))
            {
                foreach (var dg in DecoderGroups)
                {
                    sw.WriteLine($"{dg.Header.Len},{dg.Header.SequenceNum},{dg.Header.Src},{dg.Header.Pan},{dg.Header.DestPan},{dg.Header.DestAddr}");

                    foreach (var di in dg.DecoderInfoList)
                        sw.WriteLine(
                            $"{di.Name},{di.StartBit},{di.BitLength},{di.StartByte},{di.ByteLength},{di.Signed},{di.Factor},{di.Unit},{di.Notes}");
                }
            }
        }

        //Update our memory of the decoder info
        private void SaveDecoderInfo()
        {
            YuneecDecoder.DecoderGroup dg;

            if (DecoderGroups.Count(x => x.Header == (SelectedMessageHeader)) == 0)
            {
                dg = new YuneecDecoder.DecoderGroup
                {
                    Header = SelectedMessageHeader
                };
                DecoderGroups.Add(dg);
            }
            else
            {
                dg = DecoderGroups.First(x => x.Header == (SelectedMessageHeader));
                dg.DecoderInfoList.Clear();
            }

            for (var i = 0; i < _decodeDataTable.Rows.Count; i++)
            {
                var dr = _decodeDataTable.Rows[i];

                var di = new YuneecDecoder.DecoderInfo
                {
                    Name = dr["Name"] == DBNull.Value ? "" : (string) dr["Name"],
                    StartBit = int.Parse((string) dr["Start Bit"]),
                    BitLength = int.Parse((string) dr["Bit Length"]),
                    StartByte = int.Parse((string) dr["Start Byte"]),
                    ByteLength = float.Parse((string) dr["Byte Length"]),
                    Signed = bool.Parse((string) dr["Signed"]),
                    Factor = float.Parse((string) dr["Factor"]),
                    Unit = (string) dr["Unit"],
                    Notes = (string) dr["Notes"]
                };

                dg.DecoderInfoList.Add(di);
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog {Filter = @"*.csv|*.csv", Multiselect = false};

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DecoderGroups.Clear();
            YuneecDecoder.DecoderGroup dg = null;

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
                        dg = new YuneecDecoder.DecoderGroup
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
                        var di = new YuneecDecoder.DecoderInfo
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

            MessageSelectionUpdate(dataGridView_messages);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateDecoderFile();
        }

        private void CreateDecoderFile()
        {
            var sfd = new SaveFileDialog {Filter = @".csv|*.csv"};

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                DecoderFileName = sfd.FileName;
                Name = sfd.FileName;
            }
        }
        #endregion

        #region DataGrid Events
        private void dataGridView_decoded_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _pauseUpdates = true;
        }

        private void dataGridView_decoded_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            _pauseUpdates = false;
            UpdateDecodeValues(SelectedMessage);
        }

        //The user has updated one of the cells, recalculate the table and save it
        private void dataGridView_decoded_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            for (var i = 0; i < _decodeDataTable.Rows.Count - 1; ++i)
            {
                var dr1 = _decodeDataTable.Rows[i];
                var dr2 = _decodeDataTable.Rows[i + 1];

                if (int.TryParse(dr1["Start Bit"] as string, out var bitOffset1) == false)
                {
                    return;
                }

                if (int.TryParse(dr1["Bit Length"] as string, out var bitLength1) == false)
                {
                    return;
                }


                if (int.TryParse(dr2["Start Bit"] as string, out var bitOffset2) == false)
                {
                    return;
                }

                if (int.TryParse(dr2["Bit Length"] as string, out var bitLength2) == false)
                {
                    return;
                }


                //delete row
                if (bitOffset1 + bitLength1 >= bitOffset2 + bitLength2)
                {
                    _decodeDataTable.Rows.RemoveAt(i + 1);
                    --i;
                    continue;
                }


                //insert row
                if (bitOffset1 + bitLength1 < bitOffset2)
                {
                    var startbit = bitOffset1 + bitLength1;
                    var bitLength = bitOffset2 - startbit;

                    var dr = _decodeDataTable.NewRow();
                    dr["Name"] = "";
                    dr["Start Bit"] = startbit;
                    dr["Bit Length"] = bitLength;
                    dr["Start Byte"] = startbit / 8;
                    dr["Byte Length"] = bitLength / 8.0;
                    dr["Signed"] = "False";
                    dr["Factor"] = 1.0;
                    dr["Unit"] = "";
                    dr["Notes"] = "";
                    dr["Raw Data"] = "";
                    dr["Value"] = "";
                    _decodeDataTable.Rows.InsertAt(dr, i + 1);
                }

                //Split row
                if (bitOffset1 + bitLength1 > bitOffset2 &&
                    bitOffset1 + bitLength1 < bitOffset2 + bitLength2)
                {
                    var startbit = bitOffset1 + bitLength1;
                    var bitLength = startbit - bitOffset2;

                    dr2["Start Bit"] = startbit;
                    dr2["Bit Length"] = bitLength;
                    dr2["Start Byte"] = startbit / 8;
                    dr2["Byte Length"] = bitLength / 8.0;
                }
            }

            //Readjust all rows
            var lastBit = 0;
            for (var i = 0; i < _decodeDataTable.Rows.Count; ++i)
            {
                var dr = _decodeDataTable.Rows[i];

                if (int.TryParse(dr["Start Bit"] as string, out var bitOffset) == false)
                {
                    return;
                }

                if (int.TryParse(dr["Bit Length"] as string, out var bitLength) == false)
                {
                    return;
                }


                dr["Start Bit"] = bitOffset;
                dr["Bit Length"] = bitLength;
                dr["Start Byte"] = bitOffset / 8;
                dr["Byte Length"] = bitLength / 8.0;

                lastBit = bitOffset + bitLength;
            }

            //Add rows ass needed
            while (lastBit / 8 < SelectedMessageHeader.Len)
            {
                var startBit = lastBit;
                var bitLength = SelectedMessageHeader.Len * 8 - startBit;
                lastBit += bitLength;

                var dr = _decodeDataTable.NewRow();
                dr["Name"] = "";
                dr["Start Bit"] = startBit;
                dr["Bit Length"] = bitLength;
                dr["Start Byte"] = startBit / 8;
                dr["Byte Length"] = bitLength / 8.0;
                dr["Signed"] = "False";
                dr["Factor"] = 1.0;
                dr["Unit"] = "";
                dr["Notes"] = "";
                dr["Raw Data"] = "";
                dr["Value"] = "";
                _decodeDataTable.Rows.Add(dr);
            }

            SaveDecoderInfo();
        }

        //message selection has changed, update the decoder grid view
        private void dataGridView_messages_SelectionChanged(object sender, EventArgs e)
        {
            if (!(sender is DataGridView dgv))
            {
                return;
            }

            MessageSelectionUpdate(dgv);
        }

        private void MessageSelectionUpdate(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                return;
            }

            var dgvr = dgv.SelectedRows[0].Cells["Message"];

            if (string.IsNullOrEmpty(dgvr.Value as string))
            {
                return;
            }

            UpdateHeaderInfo((string)dgvr.Value);
        }
#endregion

       

        private delegate void UpdateDecodeValuesDelegate();
    }

    public static class DataTableExtension
    {
        public static void AddNewColumn(this DataTable dt, string name)
        {
            var c0 = new DataColumn(name);
            dt.Columns.Add(c0);
        }
    }
}