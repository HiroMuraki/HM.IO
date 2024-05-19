﻿namespace HM.Common;

public static class CallChainExtensions
{
    public static async Task<CallChain<T>> ContinueAsync<T>(this Task<CallChain<T>> self, Action action)
    {
        return (await self).Continue(action);
    }

    public static async Task<CallChain<T>> ContinueAsync<T>(this Task<CallChain<T>> self, Action<T> action)
    {
        return (await self).Continue(action);
    }

    public static async Task<CallChain<T>> ContinueAsync<T>(this Task<CallChain<T>> self, Func<CallChainState> action)
    {
        return (await self).Continue(action);
    }

    public static async Task<CallChain<T>> ContinueAsync<T>(this Task<CallChain<T>> self, Func<T, CallChainState> func)
    {
        return (await self).Continue(func);
    }

    public static async Task<CallChain<T>> ElseIfAsync<T>(this Task<CallChain<T>> self, Boolean condition, Action action)
    {
        return (await self).ElseIf(condition, action);
    }

    public static async Task<CallChain<T>> ElseIfAsync<T>(this Task<CallChain<T>> self, Boolean condition, Action<T> action)
    {
        return (await self).ElseIf(condition, action);
    }

    public static async Task<CallChain<T>> ElseIfAsync<T>(this Task<CallChain<T>> self, Boolean condition, Func<CallChainState> action)
    {
        return (await self).ElseIf(condition, action);
    }

    public static async Task<CallChain<T>> ElseIfAsync<T>(this Task<CallChain<T>> self, Boolean condition, Func<T, CallChainState> action)
    {
        return (await self).ElseIf(condition, action);
    }

    public static async Task ElseAsync<T>(this Task<CallChain<T>> self, Action action)
    {
        (await self).Else(action);
    }

    public static async Task ElseAsync<T>(this Task<CallChain<T>> self, Action<T> action)
    {
        (await self).Else(action);
    }
}
