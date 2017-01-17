namespace zh.fang.common.Exchange
{
    using System;

    public class Int32Exchange : IValueTypeExchange<int>
    {
        Tuple<int, int> IValueTypeExchange<int>.ExchangeByPlus(int a, int b)
        {
            a = a + b;
            b = a - b;
            a = a - b;

            return Tuple.Create(a, b);
        }

        Tuple<int, int> IValueTypeExchange<int>.ExchangeByShift(int a, int b)
        {
            a = a ^ b;
            b = a ^ b;
            a = a ^ b;
            return Tuple.Create(a, b);
        }
    }
}
