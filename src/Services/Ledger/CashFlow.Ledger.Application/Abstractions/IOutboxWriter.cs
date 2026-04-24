namespace CashFlow.Ledger.Application.Abstractions;
public interface IOutboxWriter { void Add<T>(T message, string eventType); }
