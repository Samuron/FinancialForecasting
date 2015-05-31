using System.ServiceModel;

namespace FinancialForecasting.Migration
{
    [ServiceContract]
    public interface INotifyMigrationProgress
    {
        [OperationContract(IsOneWay = true)]
        void AcceptMaxRows(int maxRows);

        [OperationContract(IsOneWay = true)]
        void AcceptCurrentRow(int rowNumber);

        [OperationContract(IsOneWay = true)]
        void MigrationFinished();
    }
}