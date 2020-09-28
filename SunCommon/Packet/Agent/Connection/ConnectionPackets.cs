﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using KaymakNetwork;
using NetworkCommsDotNet.Connections;

namespace SunCommon
{
    public static class ConnectionPackets
    {
        public class C2SAskEnterCharSelect :ConnectionPacket
        {
            private byte[] unk1;
            public int userID;
            public string username;
            public C2SAskEnterCharSelect(ByteBuffer buffer, NetworkCommsDotNet.Connections.Connection connection) :
                base(118, connection)
            {
                //TODO figure out packet struct
                unk1 = buffer.ReadBlock(2);
                userID= BitConverter.ToInt32(buffer.ReadBlock(5), 0);
                var uname = buffer.ReadBlock(50);
                for (int i = 0; i < uname.Length; i++)
                {
                    if (uname[i] == 0)
                    {
                        byte[] help = new byte[i];
                        Array.Copy(uname, help, i);
                        username = Encoding.ASCII.GetString(help);
                        break;
                    }
                }
            }

            public override void OnReceive()
            {
                ////TODO link userIDs
                //var userID = ByteUtils.ToByteArray(33, 4);
                //var packet = new S2CAnsEnterCharSelect(userID);
                //packet.Send(Connection);
            }
        }

        public class S2CAnsEnterCharSelect : ConnectionPacket
        {
            private byte[] userID;
            private byte charCount;
            private byte unk2 = 00;
            private byte[] characterInfobytes;
            public S2CAnsEnterCharSelect(int userID,int charCount,byte[] characterInfobytes) : base(152)
            {

                this.userID = ByteUtils.ToByteArray(userID, 4);
                this.charCount = (byte)charCount;
                this.unk2 = this.charCount;
                this.characterInfobytes = characterInfobytes;
            }

            public new void Send(Connection connection)
            {
                var sb = getSendableBytes(userID, new byte[] {charCount, unk2},characterInfobytes);
                connection.SendUnmanagedBytes(sb);
            }
        }


    }
}
