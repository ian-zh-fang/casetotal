namespace zh.fang.common.Exchange
{
    using System;

    public interface IValueTypeExchange<T>
    {
        Tuple<T, T> ExchangeByPlus(T a, T b);

        Tuple<T, T> ExchangeByShift(T a, T b);
    }
}
