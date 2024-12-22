using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProtoBuffEncoding
{
    /// <summary>
    ///  序列化protobuf
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static byte[] Serialize(IMessage message)
    {
        return message.ToByteArray();
    }

    /// <summary>
    /// 反序列化protobuf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static T Deserialize<T>(byte[] bytes) where T : class, IMessage, new()
    {
        T message = new T();
        try
        {
            message = message.Descriptor.Parser.ParseFrom(bytes) as T;
        }
        catch (Exception e)
        {
            throw e;
        }
        return message;
    }

    /// <summary>
    /// 反序列化protobuf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static T Deserialize<T>(Stream stream) where T : class, Google.Protobuf.IMessage, new()
    {
        T message = new T();
        try
        {
            message = message.Descriptor.Parser.ParseDelimitedFrom(stream) as T;
        }
        catch (Exception e)
        {
            throw e;
        }
        return message;
    }

    /// <summary>
    /// 反序列化protobuf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <returns></returns>
    public static T Deserialize<T>(string str) where T : class, Google.Protobuf.IMessage, new()
    {
        T message = new T();
        try
        {
            message = message.Descriptor.Parser.ParseJson(str) as T;
        }
        catch (Exception e)
        {
            throw e;
        }
        return message;
    }

}
