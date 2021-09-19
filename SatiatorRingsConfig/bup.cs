using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatiatorRingsConfig
{
    public class bupData
    {
        public string name;
        public string comment;
        public int lang;
        public int date;
        public int dataSize;
        public int blockSize;
    }
    public class bupProcessor
    {
        static int BUP_HEADER_SIZE = 64;
        static int MAX_SAVE_NAME_LENGTH = 12;
        static int MAX_SAVE_COMMENT_LENGTH = 11;

        static public string languageIdToString(int langId)
        {
            if (langId == 0)
                return "Japanese";
            else if (langId == 1)
                return "English";
            else if (langId == 2)
                return "Francais";
            else if (langId == 3)
                return "Deutsch";
            else if (langId == 4)
                return "Espanol";
            else if (langId == 5)
                return "Italiano";
            // language not found
            return "None";
        }

        static public DateTime dateToString(int bupDate)
        {
            int mins = (bupDate % 60) & 0xFF;
            int hours = (int)(bupDate % (60 * 24) / 60) & 0xFF;

            // Compute days count
            int div = bupDate / (60 * 24);

            int year_base = (int)(div / ((365 * 4) + 1));
            year_base = (int)(year_base * 4);
            int days_remain = (int)(div % ((365 * 4) + 1));

            int[] days_count = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            int month = 0;

            for (int i = 0; i < 48; i++)
            {
                int days_per_month = days_count[i % 12];
                if (i == 1)
                    days_per_month++;

                if (days_remain < days_per_month)
                    break;

                days_remain -= days_per_month;
                month++;

                if (i % 12 == 11)
                {
                    month = 0;
                    year_base++;
                }
            }

            year_base += 1980;
            month++;
            days_remain++;

            return new DateTime(year_base, month, days_remain, hours, mins, 0);
        }
        static public bupData parseFile(string fn)
        {
            if(!File.Exists(fn))
            {
                return null;
            }
            byte[] bupData = File.ReadAllBytes(fn);
            if(bupData.Length < BUP_HEADER_SIZE)
            {
                return null;
            }
            bupData bup = new bupData();

            bup.name = Encoding.UTF8.GetString(bupData, 16, MAX_SAVE_NAME_LENGTH);
            bup.comment = Encoding.UTF8.GetString(bupData, 28, MAX_SAVE_COMMENT_LENGTH);
            bup.lang = (int)bupData[39];

            bup.date = BitConverter.ToInt32(bupData.Skip(40).Take(4).Reverse().ToArray(), 0);
            bup.dataSize = BitConverter.ToInt32(bupData.Skip(44).Take(4).Reverse().ToArray(), 0);
            bup.blockSize = BitConverter.ToInt16(bupData.Skip(48).Take(2).Reverse().ToArray(), 0);

            return bup;
        }
    }
}
