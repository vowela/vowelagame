using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializationData
{
    public BinaryWriter Writer;
    public BinaryReader Reader;
    public MemoryStream Stream;
    public byte[] Buffer;
}

public class SerializationHelper
{
    public static byte[] SerializeToBytes<T>(T item)
    {
        var formatter    = new BinaryFormatter();
        using (var stream = new MemoryStream())
        {
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            return stream.ToArray();
        }
    }
    
    public static object DeserializeFromBytes(byte[] bytes)
    {
        var formatter    = new BinaryFormatter();
        using (var stream = new MemoryStream(bytes)) return formatter.Deserialize(stream);
    }
    
    private static SerializationData InitWriter(int size)
    {
        var serializationData    = new SerializationData { Buffer = new byte[size] };
        serializationData.Stream = new MemoryStream(serializationData.Buffer);
        serializationData.Writer = new BinaryWriter(serializationData.Stream);
        return serializationData;
    }
    
    private static SerializationData InitReader(byte[] buffer)
    {
        var serializationData    = new SerializationData { Stream = new MemoryStream(buffer) };
        serializationData.Reader = new BinaryReader(serializationData.Stream);
        return serializationData;
    }
    
    public static byte[] Serialize(byte code, string targetName, string methodName, params object[] args)
    {
        // Calculate sizes
        var bufSize = sizeof(byte) + targetName.Length * sizeof(char) + methodName.Length * sizeof(char);
        foreach (var arg in args)
        {
            bufSize += SerializeToBytes(arg).Length;
        }
        
        // Write data to binary writer
        var data = InitWriter(bufSize);
        data.Writer.Write(code);
        data.Writer.Write(targetName);
        data.Writer.Write(methodName);
        data.Writer.Write(args.Length);
        foreach (var arg in args)
        {
            var argData = SerializeToBytes(arg);
            data.Writer.Write(argData.Length);
            data.Writer.Write(argData);
        }
        return data.Buffer;
    }

    public static void Deserialize(byte[] buf, out byte code, out string json)
    {
        var data = InitReader(buf);
        data.Stream.Write(buf, 0, buf.Length);
        data.Stream.Position = 0;
        code = data.Reader.ReadByte();
        json = data.Reader.ReadString();
    }
}
