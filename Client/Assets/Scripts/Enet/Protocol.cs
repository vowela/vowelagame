using System;
using System.IO;

namespace Server
{
    public class Protocol
    {
        public static byte[] SerializeData(byte code, string json)                          => new Protocol().Serialize(code, json);
        public static byte[] SerializeData(byte code, uint value)                           => new Protocol().Serialize(code, value);
        public static byte[] SerializeData(byte code, string targetName, string methodName) => new Protocol().Serialize(code, targetName, methodName);

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
        
        public byte[] Serialize(byte code, string targetName, string methodName)
        {
            var bufSize = sizeof(byte) + System.Text.Encoding.ASCII.GetByteCount(targetName) * sizeof(char) + System.Text.Encoding.ASCII.GetByteCount(methodName) * sizeof(char);
            InitWriter(bufSize);
            m_writer.Write(code);
            m_writer.Write(targetName);
            m_writer.Write(methodName);
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

        public void Deserialize(byte[] buf, out byte code, out byte flag)
        {
            InitReader(buf);
            m_stream.Write(buf, 0, buf.Length);
            m_stream.Position = 0;
            code = m_reader.ReadByte();
            flag = m_reader.ReadByte();
        }

        private BinaryWriter m_writer;
        private BinaryReader m_reader;
        private MemoryStream m_stream;
        private byte[] m_buffer;
    }
}
