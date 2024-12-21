/*FileName:		Singleton
 *Author:		Zhang Xiaotao
 *CreateTime:	2020-03-05-21:52:10
 *UnityVersion:	2019.3.2f1
 *Version:		1.0
 *Description:	
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// where T : new() -- 表示类型必须具有公共的无参数构造函数。
public class Singleton<T> : IDisposable where T : new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    public virtual void Dispose()
    {
        
    }
}
