namespace zh.fang.common.Exchange
{
    using System;

    public class Int64Exchange : IValueTypeExchange<long>
    {
        public Tuple<long, long> ExchangeByPlus(long a, long b)
        {
            a = a + b;
            b = a - b;
            a = a - b;
            return Tuple.Create(a, b);
        }

        public Tuple<long, long> ExchangeByShift(long a, long b)
        {
            a = a ^ b;
            b = a ^ b;
            a = a ^ b;
            return Tuple.Create(a, b);
        }
    }
}
