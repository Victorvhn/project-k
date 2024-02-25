namespace ProjectK.Api.Dtos.v1.PlannedTransactions.Requests;

/// <param name="Amount">Specifies the monetary value associated with the transaction.</param>
public record PayPlannedTransactionRequest(decimal Amount);