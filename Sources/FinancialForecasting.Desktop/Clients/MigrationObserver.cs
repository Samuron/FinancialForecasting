using System;
using System.Reactive.Subjects;
using System.ServiceModel;
using FinancialForecasting.Migration;

namespace FinancialForecasting.Desktop.Clients
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MigrationObserver : IObservable<int>, INotifyMigrationProgress
    {
        private readonly Subject<int> _subject;

        public MigrationObserver()
        {
            _subject = new Subject<int>();
        }

        public void AcceptMaxRows(int maxRows)
        {
            _subject.OnNext(maxRows);
        }

        public void AcceptCurrentRow(int rowNumber)
        {
            _subject.OnNext(rowNumber);
        }

        public void MigrationFinished()
        {
            _subject.OnCompleted();
        }

        IDisposable IObservable<int>.Subscribe(IObserver<int> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}