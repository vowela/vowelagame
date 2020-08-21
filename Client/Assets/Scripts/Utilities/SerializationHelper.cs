using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Changes to these script are sensitive! Due to using IO and runtime formatter libs it's not possible to put
// this script into shared project, so this script is copied into Server. Changes to serializer are required for both!
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
        var formatter = new BinaryFormatter();
        var stream = new MemoryStream();
        formatter.Serialize(stream, item);
        stream.Seek(0, SeekOrigin.Begin);
        return stream.ToArray();
    }

    public static object DeserializeFromBytes(byte[] bytes)
    {
        var formatter = new BinaryFormatter();
        var stream = new MemoryStream(bytes);
        return formatter.Deserialize(stream);
    }

    private static SerializationData InitWriter(int size)
    {
        var serializationData = new SerializationData {Buffer = new byte[size]};
        serializationData.Stream = new MemoryStream(serializationData.Buffer);
        serializationData.Writer = new BinaryWriter(serializationData.Stream);
        return serializationData;
    }

    private static SerializationData InitReader(byte[] buffer)
    {
        var serializationData = new SerializationData {Stream = new MemoryStream(buffer)};
        serializationData.Reader = new BinaryReader(serializationData.Stream);
        return serializationData;
    }

    // RPC Serialization
    public static byte[] Serialize(byte code, string methodName, string targetName = "", int objectId = -1, params object[] args)
    {
        // Calculate sizes
        var bufSize = PlayerPrefs.GetString(AuthController.SessionID).Length * sizeof(char) + sizeof(byte) +
                      (objectId == -1 ? targetName.Length * sizeof(char) : sizeof(int)) +
                      methodName.Length * sizeof(char) + sizeof(int);
        foreach (var arg in args)
        {
            bufSize += sizeof(int);                  // Argument data length
            bufSize += SerializeToBytes(arg).Length; // Argument data
        }
        
        // Write data to binary writer
        var data = InitWriter(bufSize);
        // Session id goes first
        data.Writer.Write(PlayerPrefs.GetString(AuthController.SessionID));
        // Then Network Event Code
        data.Writer.Write(code);
        // Target name for static rpc or object id for dynamic
        if (objectId == -1) data.Writer.Write(targetName);
        else                data.Writer.Write(objectId);
        // Method name goes next
        data.Writer.Write(methodName);
        // Arguments list here
        data.Writer.Write(args.Length);
        foreach (var arg in args)
        {
            var argData = SerializeToBytes(arg);
            data.Writer.Write(argData.Length);
            data.Writer.Write(argData);
        }
        return data.Buffer;
    }
}