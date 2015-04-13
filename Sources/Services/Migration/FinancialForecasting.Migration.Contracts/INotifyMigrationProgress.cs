using System;
using System.ServiceModel;

namespace FinancialForecasting.Migration
{
    [ServiceContract]
    public interface INotifyMigrationProgress
    {
        [OperationContract(IsOneWay = true)]
        void AcceptMaxRows(Int32 maxRows);

        [OperationContract(IsOneWay = true)]
        void AcceptCurrentRow(Int32 rowNumber);

        [OperationContract(IsOneWay = true)]
        void MigrationFinished();
    }
}