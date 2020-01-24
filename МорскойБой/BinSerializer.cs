using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace МорскойБой
{
    class BinSerializer<T> where T : class, new()
{
   public static unsafe byte[] ToArray(T data)

   {

      int size = Marshal.SizeOf(data);

      byte[] buff = new byte[size];

 

      fixed (byte* p = buff)

      {

         Marshal.StructureToPtr(data, (IntPtr)p, false);

      }

 

      return buff;

   }

 

   public static unsafe T FromArray(byte[] buff)

   {

      if (Marshal.SizeOf(typeof(T)) != buff.Length)
        throw new Exception("Size mismatch");

       

      T result = new T();

       

      fixed (byte* p = buff)

      {

         Marshal.PtrToStructure((IntPtr)p, result);

      }
            return result;
   }

};

}
