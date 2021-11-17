using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusDiagnoster.Model.Converters
{
    public enum VarType
    {
        Decimal,
        Integer,
        Hexadecimal,
        Binary,
        BigEndianFloat,
        LittleEndianFloat
    }
    public class VariableType
    {

        //Decimal
        public static string convertToDec(ushort reg1)
        {

            return reg1.ToString();
        }

        public static string convertToInt16(ushort reg1)
        {

            Int16 res = BitConverter.ToInt16(BitConverter.GetBytes(reg1),0);
            return res.ToString();
        }

        public static string convertToHex(ushort reg1)
        {

            string res =BitConverter.ToString( BitConverter.GetBytes(reg1));
            return res;
        }


        public static string convertToBin(ushort reg1)
        {

            string res = Convert.ToString(reg1,2);
            return res;
        }
        //Big Endian
        public static string convertToFloatBE(ushort reg1, ushort reg2)
        {
            string result = "";

            byte[] bytes1 = BitConverter.GetBytes(reg1);
            byte[] bytes2 = BitConverter.GetBytes(reg2);

            byte[] combinedBytes = {bytes1[0],bytes1[1],bytes2[0],bytes2[1] };

            float res = BitConverter.ToSingle(combinedBytes, 0);

            result = res.ToString();


            return result;
        }

        //Little Endian
        public static string convertToFloatLE(ushort reg1, ushort reg2)
        {
            string result = "";

            byte[] bytes1 = BitConverter.GetBytes(reg1);
            byte[] bytes2 = BitConverter.GetBytes(reg2);

            byte[] combinedBytes = { bytes2[0], bytes2[1], bytes1[0], bytes1[1] };

            float res = BitConverter.ToSingle(combinedBytes, 0);

            result = res.ToString();


            return result;
        }

    }
}
