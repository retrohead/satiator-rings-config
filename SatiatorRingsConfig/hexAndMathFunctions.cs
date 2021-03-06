using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace SatiatorRingsConfig
{
    public class hexAndMathFunctions
    {
        public static string stringToProper(string str)
        {
            bool flag = true;
            string str1 = "";
            for (int startIndex = 0; startIndex < str.Length; ++startIndex)
            {
                string str2 = str.Substring(startIndex, 1);
                if (flag)
                {
                    flag = false;
                    str1 += str2.ToUpper();
                }
                else
                    str1 += str2;
                if (str2 == " ")
                    flag = true;
            }
            return str1;
        }

        public static int getPercentage(int thisval, int targetVal) => (int)Decimal.Round((Decimal)thisval / (Decimal)targetVal * 100M, 0);

        public static string decimalByteConvert(int byte_decimal, string return_type)
        {
            string str = string.Format("{0:X2}", (object)byte_decimal);
            int int32 = Convert.ToInt32(str, 16);
            return return_type == "hex" ? str : char.ConvertFromUtf32(int32);
        }

        public static string trimByteString(string str)
        {
            if(str.Contains("\0"))
            {
                str = str.Substring(0, str.IndexOf("\0"));
            }
            return str;
        }

        public static string reversehex(string hex, int len, bool isCharString = false)
        {
            string str1 = "";
            if (hex != null)
            {
                while (hex.Length < len)
                    if(isCharString)
                        hex = hex + "0";
                    else
                        hex = "0" + hex;
                for (int startIndex = len - 2; startIndex >= 0; startIndex -= 2)
                {
                    string str2 = hex.Substring(startIndex, 2);
                    str1 += str2;
                }
            }
            else
                str1 = hex;
            while (str1.Length < len)
                str1 += "0";
            return str1;
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string intToTimeString(int time)
        {
            int hour = time / 3600;
            int sec = time - hour * 3600;
            int min = sec / 60;
            sec = sec - min * 60;

            return hour.ToString("D2") + ":" + min.ToString("D2") + ":" + sec.ToString("D2");
        }
        public static string reversestring(string str)
        {
            string str1 = "";
            for (int startIndex = str.Length - 1; startIndex >= 0; --startIndex)
                str1 += str.Substring(startIndex, 1);
            return str1;
        }

        public static string halfByteSwap(string hex)
        {
            string str = "";
            if (((Decimal)hex.Length / 2M).ToString() != (hex.Length / 2).ToString())
            {
                MessageBox.Show("Trying to halfByte swap an uneven (" + (object)hex.Length + ") amount of bytes!");
                return "";
            }
            for (int index = 0; index < hex.Length / 2; ++index)
                str = str + hex.Substring(1 + index * 2, 1) + hex.Substring(index * 2, 1);
            return str;
        }

        public static int hexToInt(string hex)
        {
            hex = reversehex(hex, hex.Length);
            return int.Parse(hex, NumberStyles.HexNumber);
        }

        public static string hex2binary(string hexvalue) => Convert.ToString(Convert.ToInt32(hexvalue, 16), 2);

        public static string stringToHexadecimal(string Data, int bytes)
        {
            string str = "";
            foreach (char ch in Data.ToCharArray())
            {
                string hex = string.Format("{0:X4}", (object)Convert.ToUInt32(ch));
                str += reversehex(hex, 4);
            }
            while (str.Length / 2 < bytes)
                str += "00";
            return str;
        }

        public static string addCommasToHex(string hex)
        {
            string str = "";
            int num = 0;
            for (int startIndex = 0; startIndex < hex.Length; ++startIndex)
            {
                str += hex.Substring(startIndex, 1);
                if (startIndex != hex.Length - 1)
                {
                    ++num;
                    if (num == 2)
                    {
                        num = 0;
                        str += ",";
                    }
                }
            }
            return str;
        }

        public static string convertHexToEncryptionKey(string hex)
        {
            if (hex.Length < 16)
            {
                MessageBox.Show("The application encoded key is incorrect");
                Environment.Exit(0);
            }
            string str = "";
            hex = addCommasToHex(hex);
            string[] strArray = hex.Split(',');
            for (int index = 0; index < 8; ++index)
            {
                uint num = uint.Parse(strArray[index], NumberStyles.HexNumber);
                str += Encoding.UTF32.GetString(BitConverter.GetBytes(num));
            }
            return str;
        }

        public static string hexCsvToNiceDisplay(string csv, int bytesPerRow)
        {
            string[] strArray = csv.Split(',');
            int num = 0;
            string str1 = "";
            int byteSplit = 0;
            for (int index = 0; index < strArray.Length; ++index)
            {
                string str2 = str1 + strArray[index];
                ++num;
                if(strArray.Length > 16)
                ++byteSplit;
                if (num >= bytesPerRow)
                {
                    byteSplit = 0;
                    num = 0;
                    str1 = str2 + "\r\n";
                }
                else
                    str1 = str2 + " ";
                if (byteSplit >= 4)
                {
                    str1 = str1 + " ";
                    byteSplit = 0;
                }
            }
            return str1;
        }
        public static string hexToBitString(string hexChar)
        {
            string bitStr = "";
            if (hexChar.Length == 1)
                hexChar = "0" + hexChar;
            byte b = Convert.ToByte(int.Parse(hexChar, NumberStyles.HexNumber));
            BitArray bits = new BitArray(new byte[] { b });
            for(int i =0; i<bits.Length;i++)
            {
                if (bits[i].Equals(true))
                    bitStr =  bitStr + "1";
                else
                    bitStr = bitStr  + "0";
            }
            return bitStr;
        }
        public static string hexCsvToBitDisplay(string csv, int bitsPerRow)
        {
            string[] strArray = csv.Split(',');
            int num = 0;
            string str1 = "";
            for (int index = 0; index < strArray.Length; ++index)
            {
                for (int i = 0; i < 2; i++)
                {
                    str1 = str1 + hexToBitString(strArray[index].Substring(i, 1));
                    ++num;
                    if (num >= bitsPerRow)
                    {
                        num = 0;
                        str1 = str1 + "\r\n";
                    }
                    else
                        str1 = str1 + " ";
                }
            }
            return str1;
        }
    }
}
