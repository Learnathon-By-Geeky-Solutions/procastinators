namespace FinanceTracker.Domain.Exceptions;

public class NotFoundException(string resourceType, string resourceId)
    : Exception($"{resourceType} with id {resourceId} was not found")
{
}
