using System;
using System.IO;

namespace VowelAServer.Server.Models
{
    public class Protocol
    {
        public static byte[] SerializeData(byte code, string json)
        {
            var protocol = new Protocol();
            return protocol.Serialize(code, json);
        }

        public static byte[] SerializeData(byte code, uint value)
        {
            var protocol = new Protocol();
            return protocol.Serialize(code, value);
        }

        private void InitWriter(int size)
        {
            m_buffer = new byte[size];
            m_stream = new MemoryStream(m_buffer);
            m_writer = new BinaryWriter(m_stream);
        }

        private void InitReader(byte[] buffer)
        {
            m_stream = new MemoryStream(buffer);
            m_reader = new BinaryReader(m_stream);
        }

        public byte[] Serialize(byte code, uint value)
        {
            const int bufSize = sizeof(byte) + sizeof(int);
            InitWriter(bufSize);
            m_writer.Write(code);
            m_writer.Write(value);
            return m_buffer;
        }

        public byte[] Serialize(byte code, string json)
        {
            var bufSize = sizeof(byte) + System.Text.Encoding.ASCII.GetByteCount(json) * sizeof(char);
            InitWriter(bufSize);
            m_writer.Write(code);
            m_writer.Write(json);
            return m_buffer;
        }

        public void Deserialize(byte[] buf, out byte code, out string json)
        {
            InitReader(buf);
            m_stream.Write(buf, 0, buf.Length);
            m_stream.Position = 0;
            code = m_reader.ReadByte();
            json = m_reader.ReadString();
        }

        private BinaryWriter m_writer;
        private BinaryReader m_reader;
        private MemoryStream m_stream;
        private byte[] m_buffer;
    }
}
