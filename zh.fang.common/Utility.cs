namespace zh.fang.common
{
    using System;

    public static class Utility
    {
        public static string GuidToString16()
        {
            long i = 1;
            foreach (byte b in System.Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - System.DateTime.Now.Ticks);

        }
    }
}
