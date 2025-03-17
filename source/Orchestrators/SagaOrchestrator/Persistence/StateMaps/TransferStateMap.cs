using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SagaOrchestrator.StatemachineInstances;

namespace SagaOrchestrator.Persistence.StateMaps;

public class TransferStateMap : SagaClassMap<TransferStateMachineInstance>
{
    protected override void Configure(EntityTypeBuilder<TransferStateMachineInstance> entity, ModelBuilder model)
    {
        entity.ToTable("TransferStateMachine");
    }
}