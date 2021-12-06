namespace DashTransit.EntityFramework.Entities;

using DashTransit.Core.Domain;
using MassTransit.EntityFrameworkCoreIntegration.Audit;

public class RawAudit : AuditRecord, IRawAuditData
{
}